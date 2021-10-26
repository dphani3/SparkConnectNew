#region Header Information
/***********************************************************************************************************
NameSpace: IR.QueueReader.LogManager
File: LogManager.cs
Class: LogManager
Author: Sarvesh.T.S
Created Date: 16-May-2008
Reviewed By: 
***********************************************************************************************************/
#endregion

#region Namspace declaration

using System;
using System.Data;
using System.Configuration;
using System.Security;
using System.Xml.Serialization;
using System.Text;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Resources;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
using EnterpriseLibraryExtensions.Logging.TraceListeners;
using EnterpriseLibraryExtensions.Logging.Configuration;

#endregion

namespace TF.FocusPay.EmailServer.LogManager
{

    #region ExceptionLogger Class
    public static class ExceptionLogger
    {
        /// <summary>
        /// 
        /// </summary>
        #region Member Variables

        static LogWriter writer;
        static string SourceName = typeof(ExceptionLogger).Name.ToString();
        const string logName = "QueueReaderLog";
        const string ErrorCategory = "Error";
        /// <summary>
        /// Collection List to store all TraceListener List.
        /// </summary>
        static List<TraceListener> logFileListener;
        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        static ExceptionLogger()
        {
            // logFileListener = new List<TraceListener>(); 


        }
        #endregion

        #region public static  PublishException(3 overloads)
        /// <summary>
        /// Writes an Error to the log.
        /// </summary>
        /// <param name="ex"></param>
        public static void PublishException(Exception ex)
        {
            Dictionary<string, string> additionalInfo = new Dictionary<string, string>();
            PublishException(ex, "Error", ListenerType.File);
        }

        /// <summary>
        /// Writes an Error to the log.
        /// </summary>
        /// <param name="message"></param>
        public static void PublishException(Exception ex, ListenerType ltype)
        {
            Dictionary<string, string> additionalInfo = new Dictionary<string, string>();
            PublishException(ex, "Error", ltype);
        }

        /// <summary>
        /// Writes a message to the log using the specified category
        /// </summary>
        /// <param name="message">meesage to be logged</param>
        /// <param name="category">category for logging the given message</param>
        public static void PublishException(Exception ex, string category, ListenerType ltype)
        {
            try
            {
                string ToAddresses = String.Empty;
                string[] toaddresses;

                Dictionary<string, string> additionalInfo = new Dictionary<string, string>();
                /// Writes a message to the log using the specified
                /// category.
                LogEntry entry = new LogEntry();
                entry.Categories.Add(category);
                entry.Message = (SerializeToText(ex, ref additionalInfo)).ToString();
                logFileListener = new List<TraceListener>();
                switch (ltype)
                {
                    case ListenerType.File:
                        //logFileListener.Add(GetFileTraceListener(GetTextFormat()));
                        MessageLogger.Write(entry.Message.ToString(), CategoryType.Error, "", "");
                        break;
                    case ListenerType.EventViewer:
                        {
                            logFileListener.Add(GetEventLogTraceListener(GetTextFormat()));
                            entry.Severity = System.Diagnostics.TraceEventType.Error;
                            writer = GetLogWriter(logFileListener);
                            writer.Write(entry);
                            break;
                        }
                    case ListenerType.Email:
                        ToAddresses = ConfigurationManager.AppSettings["LogToAddress"].ToString();
                        toaddresses = ToAddresses.Split(new char[] { ',' });

                        foreach (string address in toaddresses)
                        {
                            logFileListener.Add(GetEmailTraceListener(GetTextFormat(), address));
                            writer = GetLogWriter(logFileListener);
                        }
                        writer.Write(entry);
                        //logFileListener.Add(GetEmailTraceListener(GetTextFormat()));
                        //writer = GetLogWriter(logFileListener);
                        //writer.Write(entry);
                        break;
                    case ListenerType.All:
                        {
                            logFileListener.Add(GetFileTraceListener(GetTextFormat()));
                            logFileListener.Add(GetEventLogTraceListener(GetTextFormat()));
                            GetDataBaseTraceListener(entry);
                            //logFileListener.Add(GetEmailTraceListener(GetTextFormat()));

                            ToAddresses = ConfigurationManager.AppSettings["LogToAddress"].ToString();
                            toaddresses = ToAddresses.Split(new char[] { ',' });
                            foreach (string address in toaddresses)
                            {
                                logFileListener.Add(GetEmailTraceListener(GetTextFormat(), address));
                            }
                            break;
                        }
                    case ListenerType.DataBase:
                        GetDataBaseTraceListener(entry);
                        writer = GetLogWriter(logFileListener);
                        writer.Write(entry);
                        break;
                    default:
                        logFileListener.Add(GetEventLogTraceListener(GetTextFormat()));
                        break;
                }
            }
            catch (Exception exp)
            {
                bool rethrow = ExceptionPolicy.HandleException(exp, "GlobalPolicy");
                ExceptionLogger.PublishException(exp, "Error", ListenerType.File);
                if (rethrow)
                {
                    throw exp;
                }
            }
        }
        #endregion

        #region private static GetLogWriter1

        //private static LogWriter GetLogWriter1(List<TraceListener> logListener)
        //{
        //    LogSource nonExistantLogSource = new LogSource("Empty");
        //    return new LogWriter(new ILogFilter[0],
        //                    traceSources,
        //                    nonExistantLogSource,
        //                    nonExistantLogSource,
        //                    AddToLogSource(logListener),
        //                    "Error",
        //                    false,
        //                    true);
        //}

        #endregion

        #region private static GetLogWriter
        /// <summary>
        /// set messages with a category ofError" or "Debug" to get distributed
        /// to all TraceListeners in traceListeners.
        /// </summary>
        /// <param name="logFileListener">Array of TraceListeners</param>
        /// <returns>IDictionary with logSource</returns>
        private static LogWriter GetLogWriter(List<TraceListener> logListener)
        {


            // Assigning a non-existant LogSource
            // for Logging Application Block
            // Specials Sources that were doesn't cared.
            // Used to say "don't log".
            LogSource nonExistantLogSource = new LogSource("Empty");


            // Let's glue it all together.
            // No filters at this time.
            // I won't log a couple of the Special
            // Sources: All Events and Events not
            // using "Error" or "Debug" categories.
            return new LogWriter(new ILogFilter[0], // ICollection<ILogFilter> filters

                               GetTraceSource(AddToLogSource(logListener)),        // IDictionary<string, LogSource> traceSources

                               nonExistantLogSource,    // LogSource allEventsTraceSource

                               nonExistantLogSource,    // LogSource notProcessedTraceSource

                               AddToLogSource(logListener),    // LogSource errorsTraceSource

                               ErrorCategory,        // string defaultCategory

                               false,                // bool tracingEnabled

                               true);                // bool logWarningsWhenNoCategoriesMatch



        }

        #endregion

        #region private static GetTraceSource
        /// <summary>
        /// set messages with a category ofError" or "Debug" to get distributed
        /// to all TraceListeners in traceListeners.
        /// </summary>
        /// <param name="logFileListener">Array of TraceListeners</param>
        /// <returns>IDictionary with logSource</returns>
        private static IDictionary<string, LogSource> GetTraceSource(LogSource lsource)
        {

            // set messages with a category of
            // "Error" or "Debug" to get distributed
            // to all TraceListeners in traceListeners.
            IDictionary<string, LogSource> traceSources = new Dictionary<string, LogSource>();

            traceSources.Add("Error", lsource);
            //traceSources.Add("Debug", lsource);


            return traceSources;
        }

        #endregion

        #region private GetTextFormat
        /// <summary>
        /// The formatter is responsible for the look 
        /// of the message.
        /// </summary>
        /// <returns></returns>
        private static TextFormatter GetTextFormat()
        {
            // Notice the tokens:
            // {timestamp}, {newline}, {message}, {category}
            TextFormatter formatter = new TextFormatter
                (
                "Message: {message}{newline}" +
                "Category: {category}{newline}");
            return formatter;
        }
        #endregion

        #region Listeners
        #region private FlatFileTraceListener GetFileTraceListener
        /// <summary>
        /// Log messages to a log file.
        /// </summary>
        /// <param name="formatter"></param>
        /// <returns></returns>
        private static FlatFileTraceListener GetFileTraceListener(TextFormatter formatter)
        {
            // Log messages to a log file.
            // Use the formatter passed
            // as well as the header and footer
            // specified.
            string FlatfileName = GetLogFilename() + ".log";
            FlatFileTraceListener logFileListener =
                new FlatFileTraceListener(FlatfileName,
                                           "----------",
                                           "----------",
                                           formatter);
            return logFileListener;


        }
        #endregion


        #region private FormattedEventLogTraceListener GetEventLogTraceListener
        /// <summary>
        /// Log messages to a log file.
        /// </summary>
        /// <param name="formatter"></param>
        /// <returns></returns>
        private static FormattedEventLogTraceListener GetEventLogTraceListener(TextFormatter formatter)
        {
            // Log messages to a log file.
            // Use the formatter passed
            // as well as the header and footer
            // specified.


            FormattedEventLogTraceListener eventlogListener =
                new FormattedEventLogTraceListener(new EventLog(logName, Environment.MachineName, SourceName),
                                                          formatter);
            return eventlogListener;
        }
        #endregion

        #region private GetDataBaseTraceListener
        /// <summary>
        /// Logs messages to the database
        /// </summary>
        /// <param name="formatter"></param>
        /// <returns></returns>

        private static void GetDataBaseTraceListener(LogEntry log)
        {
            string DATABASE_NAME = ConfigurationManager.AppSettings["ErrorLogDatabaseName"];
            string INSERT_PORTAL_ERROR_LOG = "prcWritePortalErrorLog";
            Database db = DatabaseFactory.CreateDatabase(DATABASE_NAME);
            string errorMessage = Convert.ToString(log.Message);
            int length = errorMessage.Length;
            string errorSource = "";
            DateTime errorDate = Convert.ToDateTime(log.TimeStamp);
            string strMachineName = Convert.ToString(log.MachineName);
            object[] logDetails = { errorSource, errorDate, strMachineName, errorMessage };
            int iCount = db.ExecuteNonQuery(INSERT_PORTAL_ERROR_LOG, logDetails);
        }

        #endregion

        #region private EmailTraceListener GetEmailTraceListener
        /// <summary>
        /// Log messages to a log file.
        /// </summary>
        /// <param name="formatter"></param>
        /// <returns></returns>
        private static EmailTraceListener GetEmailTraceListener(TextFormatter formatter, string address)
        {
            // Log messages to a log file.
            // Use the formatter passed
            // as well as the header and footer
            // specified.
            //string toAddress = ConfigurationManager.AppSettings["ToAddress"].ToString();
            string toAddress = address;
            string fromAddress = ConfigurationManager.AppSettings["LogFromAddress"].ToString();
            string subjectLineStarter = ConfigurationManager.AppSettings["EmailSubject"].ToString();
            //string subjectLineStarter = ConfigurationManager.AppSettings["SubjectLineStarter"].ToString();
            string subjectLineEnder = "";
            //string subjectLineEnder = ConfigurationManager.AppSettings["SubjectLineEnder"].ToString();
            string smtpServer = ConfigurationManager.AppSettings["SMTPServer"].ToString();
            int smtpPort = Convert.ToInt16(ConfigurationManager.AppSettings["SMTPPort"]);
            EmailTraceListener mailListener =
                new EmailTraceListener(toAddress, fromAddress, subjectLineStarter, subjectLineEnder, smtpServer, smtpPort, formatter);



            //bool b = SendMessage(me.ssage, formatter);

            return mailListener;
        }

        #endregion


        #endregion

        #region static LogSource AddToLogSource
        /// <summary>
        /// Add collection of TraceListeners to LogSource.
        /// </summary>
        /// <param name="ListenerArray">Array of listenerS</param>
        /// <returns>LogSource </returns>
        private static LogSource AddToLogSource(List<TraceListener> ListenerArray)
        {
            // collection of TraceListeners.
            LogSource mainLogSource =
                new LogSource("TestLogSource", SourceLevels.All);
            mainLogSource.Listeners.AddRange(ListenerArray);
            return mainLogSource;

        }

        #endregion

        #region static string GetLogFilename
        /// <summary>
        /// gets the log file (flat file) name
        /// from AppSettings in the web.config.
        /// </summary>
        /// <returns></returns>
        private static string GetLogFilename()
        {
            string file = "";
            return file = System.Configuration.ConfigurationManager.AppSettings["logFilePath"].ToString() +
                          System.Configuration.ConfigurationManager.AppSettings["LogFileName"];


        }
        #endregion

        #region static StringBuilder SerializeToText
        /// <summary>
        /// Serializes the exception tree into text		
        /// <param name="exception">Exception to be serialized</param>
        /// <param name="additionalInformation">Additional information associated with the exception</param>
        /// <returns>StringBuilder - Text Serialized form of the exception tree</returns>
        /// </summary>
        internal static StringBuilder SerializeToText(Exception ex, ref Dictionary<string, string> additionalInformation)
        {
            try
            {


                const string TEXT_SEPERATOR = "----------------------------------------------------";

                //Build the Additional Params String
                StringBuilder sbExceptionMessageTemplate = new StringBuilder();

                if (!(additionalInformation == null))
                {

                    // //Adding the header
                    sbExceptionMessageTemplate.AppendFormat("{0}Exception Log Generated On {2} at {3}{0}{1}{0}General Information{0}{4}", Environment.NewLine, TEXT_SEPERATOR, DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), TEXT_SEPERATOR);


                    //Adding all the Additional Parameters
                    foreach (String strParam in (PopulateEnvironmentInformation(ref additionalInformation)).Keys)
                    {
                        sbExceptionMessageTemplate.AppendFormat("{0}{1}: {2}", Environment.NewLine, strParam, additionalInformation[strParam]);
                    }
                }
                if ((ex == null))
                {
                    sbExceptionMessageTemplate.AppendFormat("{0}{0}No Exception Information Exists{0}", Environment.NewLine);
                }
                else
                {
                    //Extract the details for the entire exception tree
                    Dictionary<string, string> customAdditionalInfo;
                    Exception currentException = ex;
                    Int32 intExceptionCount = 1;
                    while (!(currentException == null))
                    {
                        sbExceptionMessageTemplate.AppendFormat("{0}{0}Exception Information: Exception #{1}{0}{2}", Environment.NewLine, intExceptionCount.ToString(), TEXT_SEPERATOR);

                        sbExceptionMessageTemplate.AppendFormat("{0}Exception Type: {1}", Environment.NewLine, currentException.GetType().FullName);


                        //Getting the properties of the current exception
                        foreach (PropertyInfo pInfo in currentException.GetType().GetProperties())
                        {
                            //StackTrace and Innerexception are treated seperately
                            if (((pInfo.Name != "StackTrace") && (pInfo.Name != "InnerException")))
                            {
                                //Check for NULL property Values
                                if ((pInfo.GetValue(currentException, null) == null))
                                {
                                    sbExceptionMessageTemplate.AppendFormat("{0}{1}: NULL/Undefined", Environment.NewLine, pInfo.Name);
                                }
                                else
                                {
                                    //Get the associated Additional Information if the Exception is of Type ApplicationExceptionBase
                                    if (pInfo.Name == "AdditionalInformation")
                                    {
                                        if (!(pInfo.GetValue(currentException, null) == null))
                                        {
                                            customAdditionalInfo = ((Dictionary<string, string>)(pInfo.GetValue(currentException, null)));

                                            if ((customAdditionalInfo.Count > 0))
                                            {
                                                sbExceptionMessageTemplate.AppendFormat("{0}Additional Exception Information:", Environment.NewLine);

                                                foreach (String lstrParam in customAdditionalInfo.Keys)
                                                {
                                                    sbExceptionMessageTemplate.AppendFormat("{0}  {1}: {2}", Environment.NewLine, lstrParam, customAdditionalInfo[lstrParam]);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        sbExceptionMessageTemplate.AppendFormat("{0}{1}: {2}", Environment.NewLine, pInfo.Name, pInfo.GetValue(currentException, null));
                                    }
                                }
                            }
                        }
                        // //Writing out the Stack Trace Information
                        if (!(currentException.StackTrace == null))
                        {
                            sbExceptionMessageTemplate.AppendFormat("{0}{0}Stack Trace Information{0}{1}", Environment.NewLine, TEXT_SEPERATOR);
                            sbExceptionMessageTemplate.AppendFormat("{0}{1}", Environment.NewLine, currentException.StackTrace);

                        }
                        // //Get the Inner Exception
                        currentException = currentException.InnerException;
                        intExceptionCount++;
                    }
                }
                return sbExceptionMessageTemplate;
            }
            catch (Exception lserException)
            {

                throw new Exception("Error serializing  exception to text", lserException);
            }
        }
        #endregion

        #region Helper Method
        private static Dictionary<string, string> PopulateEnvironmentInformation(ref Dictionary<string, string> additionalInformation)
        {

            const string SOURCE_NAME = "ExceptionLogger";

            if ((additionalInformation == null))
            {
                additionalInformation = new Dictionary<string, string>();

            }
            //Adding Machine Name
            try
            {
                //  if (Environment.GetLogicalDrives()
                additionalInformation.Add((SOURCE_NAME + ".ApplicationName"), "FocusPay Email Server");
                additionalInformation.Add((SOURCE_NAME + ".MachineName"), Environment.MachineName);
            }
            catch (SecurityException novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".MachineName"), "Insufficient permissions to access information." + novar.Message);
            }
            catch (Exception novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".MachineName"), "Error accessing information. " + novar.Message);
            }

            //Adding Time Stamp
            try
            {
                additionalInformation.Add((SOURCE_NAME + ".TimeStamp"), DateTime.Now.ToString());
            }
            catch (SecurityException novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".TimeStamp"), "Insufficient permissions to access information." + novar.Message);
            }
            catch (Exception novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".TimeStamp"), "Error accessing information. " + novar.Message);
            }
            //Adding Executing Assembly Full Name
            try
            {
                additionalInformation.Add((SOURCE_NAME + ".FullName"), Assembly.GetExecutingAssembly().FullName);
            }
            catch (SecurityException novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".FullName"), "Insufficient permissions to access information." + novar.Message);
            }
            catch (Exception novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".FullName"), "Error accessing information." + novar.Message);
            }
            //Adding App Domain Name
            try
            {
                additionalInformation.Add((SOURCE_NAME + ".AppDomainName"), AppDomain.CurrentDomain.FriendlyName);
            }
            catch (SecurityException novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".AppDomainName"), "Insufficient permissions to access information." + novar.Message);
            }
            catch (Exception novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".AppDomainName"), "Error accessing information. " + novar.Message);
            }
            //Adding Thread Identity
            try
            {
                additionalInformation.Add((SOURCE_NAME + ".ThreadIdentity"), Thread.CurrentPrincipal.Identity.Name);
            }
            catch (SecurityException novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".ThreadIdentity"), "Insufficient permissions to access information. " + novar.Message);
            }
            catch (Exception novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".ThreadIdentity"), "Error accessing information. " + novar.Message);
            }
            //Adding the current Windows Identity(logged on user)
            try
            {
                additionalInformation.Add((SOURCE_NAME + ".WindowsIdentity"), WindowsIdentity.GetCurrent().Name);
            }
            catch (SecurityException novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".WindowsIdentity"), "Insufficient permissions to access information. " + novar.Message);
            }
            catch (Exception novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".WindowsIdentity"), "Error accessing information. " + novar.Message);
            }
            //Adding the OS version
            try
            {
                additionalInformation.Add((SOURCE_NAME + ".OSVersion"), Environment.OSVersion.ToString());
            }
            catch (SecurityException novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".OSVersion"), "Insufficient permissions to access information. " + novar.Message);
            }
            catch (Exception novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".OSVersion"), "Error accessing information." + novar.Message);
            }
            //Adding the CLR Version
            try
            {
                additionalInformation.Add((SOURCE_NAME + ".CLRVersion"), Environment.Version.ToString());
            }
            catch (SecurityException novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".CLRVersion"), "Insufficient permissions to access information" + novar.PermissionType.Name);
            }
            catch (Exception novar)
            {
                additionalInformation.Add((SOURCE_NAME + ".CLRVersion"), "Error accessing information" + novar.Message);
            }
            //Return the populated Dictionary object
            return additionalInformation;
        }
        #endregion

        #region Constants


        #endregion

    }

    #endregion

    #region MessageLogger Class
    /// <summary>
    /// This class is used to log Debug, Information and warning statements. 
    /// </summary>
    public static class MessageLogger
    {
        static readonly LogWriter writer;
        //private static bool debugValue = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugLogValue"]);
        //private static bool informationValue = Convert.ToBoolean(ConfigurationManager.AppSettings["InfoLogValue"]);
        //private static bool warningValue = Convert.ToBoolean(ConfigurationManager.AppSettings["WarnLogValue"]);
        //public static string LogFilePath = GetLogFilePath();
        /// <summary>
        /// This is a static method used to create log format, logsource and creating the log entry. 
        /// </summary>
        static MessageLogger()
        {
            // The formatter is responsible for the look of the message. Notice the tokens:{timestamp}, {message}
            TextFormatter formatter = new TextFormatter
                ("{timestamp}" + " " +
                "{message}" + " ");
            //Log messages to a log file.Use the formatter mentioned above as well as the header and footer specified.

            

            TraceOptions option = TraceOptions.None;
            string sizeThresholdUnit = ConfigurationManager.AppSettings["SizeThresholdUnit"].ToString();
            string ageThresholdUnit = ConfigurationManager.AppSettings["AgeThresholdUnit"].ToString();
            int sizeThreshold = Convert.ToInt32(ConfigurationManager.AppSettings["SizeThreshold"]);
            string filePath = ConfigurationManager.AppSettings["LogFilePath"].ToString() +
                              ConfigurationManager.AppSettings["LogFileName"].ToString();
            SizeThresholdUnit rollingSizeUnit = GetSizeThresholdUnit(sizeThresholdUnit);
            AgeThresholdUnit rollingAgeUnit = GetAgeThresholdUnit(ageThresholdUnit);

            RollingFileTraceListenerData configurationData = new RollingFileTraceListenerData("Rolling File Trace Listener", filePath, "", "",
                                                                                              "None", option, "yyyy-MM-dd",
                                                                                              1, rollingAgeUnit, sizeThreshold,
                                                                                              rollingSizeUnit, 0);

            RollingFileTraceListener logFileListener =
                new RollingFileTraceListener(configurationData, formatter);

            // My collection of TraceListeners. I am only using one.  Could add more.
            LogSource mainLogSource =
                new LogSource("MainLogSource", SourceLevels.All);
            mainLogSource.Listeners.Add(logFileListener);

            // Assigning a non-existant LogSource for Logging Application Block Specials Sources.
            LogSource nonExistantLogSource = new LogSource("Empty");


            /* All messages with a category of "Error","Debug","Information" and "Warning" to get distributed to all 
               Flat file TraceListener in mainLogSource*/

            IDictionary<string, LogSource> traceSources =
                          new Dictionary<string, LogSource>();
            //traceSources.Add("Error", mainLogSource);
            traceSources.Add(Convert.ToString(CategoryType.Debug), mainLogSource);
            traceSources.Add(Convert.ToString(CategoryType.Information), mainLogSource);
            traceSources.Add(Convert.ToString(CategoryType.Warning), mainLogSource);
            traceSources.Add(Convert.ToString(CategoryType.Error), mainLogSource);

            writer = new LogWriter(new ILogFilter[0],
                            traceSources,
                            nonExistantLogSource,
                            nonExistantLogSource,
                            mainLogSource,
                            Convert.ToString(CategoryType.Error),
                            false,
                            true);
        }

        private static SizeThresholdUnit GetSizeThresholdUnit(string sizeThresholdUnit)
        {
            SizeThresholdUnit rollingSizeUnit = SizeThresholdUnit.None;
            switch (sizeThresholdUnit.ToUpper())
            {

                case "KB":
                    rollingSizeUnit = SizeThresholdUnit.Kilobytes;
                    break;
                case "MB":
                    rollingSizeUnit = SizeThresholdUnit.Megabytes;
                    break;
                case "GB":
                    rollingSizeUnit = SizeThresholdUnit.Gigabytes;
                    break;
                default:
                    rollingSizeUnit = SizeThresholdUnit.None;
                    break;
            }
            return rollingSizeUnit;
        }

        private static AgeThresholdUnit GetAgeThresholdUnit(string ageThresholdUnit)
        {
            AgeThresholdUnit rollingAgeUnit = AgeThresholdUnit.None;
            switch (ageThresholdUnit.ToUpper())
            {

                case "DAYS":
                    rollingAgeUnit = AgeThresholdUnit.Days;
                    break;
                case "HOURS":
                    rollingAgeUnit = AgeThresholdUnit.Hours;
                    break;
                case "MINUTES":
                    rollingAgeUnit = AgeThresholdUnit.Minutes;
                    break;
                case "MONTHS":
                    rollingAgeUnit = AgeThresholdUnit.Months;
                    break;
                case "WEEKS":
                    rollingAgeUnit = AgeThresholdUnit.Weeks;
                    break;
                default:
                    rollingAgeUnit = AgeThresholdUnit.None;
                    break;
            }
            return rollingAgeUnit;
        }


        private static string GetLogFilePath()
        {
            string logFilePath = String.Empty;
            string strFileName = ConfigurationManager.AppSettings["LogFileName"];
            if (ConfigurationManager.AppSettings["LogFilePath"] == String.Empty)
            {
                logFilePath = System.IO.Path.GetTempPath() + "\\QueueReaderLog" + "\\" + strFileName;
            }
            else
            {
                logFilePath = ConfigurationManager.AppSettings["logFilePath"] + strFileName;
            }
            return logFilePath;
        }


        /// <summary>
        /// Writes a message to the log using the specified
        /// category.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="CategoryType"></param>
        /// <param name="value"></param>
        /// <param name="strNameSpace"></param>
        public static void Write(string message, CategoryType cType, string value, string nameSpace)
        {
            LogEntry entry = new LogEntry();
            string logMessage = "";
            DateTime dtDateTime = DateTime.Now;
            logMessage = " " + "[" + cType.ToString() + "]" + " " + nameSpace + " - " + " " + message + " " + value;
            entry.TimeStamp = dtDateTime;
            entry.Categories.Add(cType.ToString());
            entry.Message = logMessage;
            switch (cType)
            {
                case CategoryType.Debug:
                    writer.Write(entry);
                    break;
                case CategoryType.Information:
                    writer.Write(entry);
                    break;
                case CategoryType.Warning:
                    writer.Write(entry);
                    break;
                case CategoryType.Error:
                    writer.Write(entry);
                    break;
                default:
                    break;
            }

            //Added By Sarvesh on 21-May-2008
            entry = null;
            logMessage = null;


        }
    }
    #endregion

    #region Enumerated Listener Type
    /// <summary>
    /// This enumeration is used to list types of trace listeners. This is used to log entries to a flat file,
    /// database, email and event log.
    /// </summary>
    public enum ListenerType
    {
        File,
        Email,
        EventViewer,
        DataBase,
        All
    }
    #endregion

    #region Enumerated Catagory Type
    /// <summary>
    /// This enumeration is used to listCategory types.
    /// </summary>
    public enum CategoryType
    {
        Error,
        Debug,
        Information,
        Warning
    }
    #endregion
}

