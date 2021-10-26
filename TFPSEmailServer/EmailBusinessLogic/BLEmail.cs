using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Xml;
using System.Configuration;
using System.Data.SqlClient;
using TF.FocusPay.EmailServer.EmailDataLayer;

namespace TF.FocusPay.EmailServer.EmailBusinessLogic
{
    public class BLEmail
    {
        #region Private Constants



        private const string RETRY_THRESHOLD = "RetryThreshold";

        private const string ERROR = "Error";
        private const string CLOSED = "Closed";
        private const string RETRY = "Retry";
        private const string WORK_IN_PROGRESS = "WorkInProgress";
        private const string READY_TO_POST = "ReadyToPost";
        private const string RETRY_FAILURE = "RetryFailure";

        private const string SLEEP_TIME = "SleepTime";
        private const string SERVICE_NAME = "ServiceName";
        private const string EXCEPTION_SLEEP_TIME = "ExceptionSleepTime"; //Added by on 11-Aug-2008

        private const int DEFAULT_INT = -1;

        private const string RESPONSE_CODE = "/Response/Code";
        private const string RESPONSE_MESSAGE = "/Response/Message";

        private const string MACHINE_NAME = ".";
        private const string SERVICE_RUNNING = "Running";


        //Field name constants

        private const string QUEUEID = "QueueID";
        private const string LASTRETRY = "LastRetry";
        private const string REPORTSTATUSID = "ReportStatusID";
        private const string ORDERNUMBER = "OrderNumber";
        private const string COMPANYID = "CompanyID";
        private const string CLIENTID = "ClientID";
        private const string ORDERAMOUNT = "OrderAmount";



        //Message constants for Logging

        private const string PROCESSING_STARTS = "Processing starts for QueueID :";
        private const string PRIORITY = "   Priority :";
        private const string ORDER_NO = "   Order Number :";
        private const string COMPANY_ID = "   Company ID :";
        private const string CLIENT_ID = "   Client ID :";
        private const string ORDER_AMOUNT = "   Order Amount :$";
        private const string REPORT_STATUS_ID = "   Report Status ID :";

        private const string STATUS_ID = "   Status ID :";
        private const string SENDING_REQUEST_TO_OMS = "Sending the request to OMS for QueueID :";
        private const string RESPONSE_CODE_OMS = "Response code from OMS :";
        private const string QUE_ID = "  QueueID :";
        private const string RESPONSE_MESSAGE_OMS = "Response message from OMS :";
        private const string SETTING_ERROR = "Setting the status as Error for QueueID :";
        private const string SETTING_RETRY = "Setting the status as Retry for QueueID :";
        private const string SETTING_RETRY_FAILURE = "Setting the status as Retry Failure QueueID :";
        private const string SETTING_CLOSED = "Setting the status as Closed for QueueID :";
        private const string PROCESSING_ENDS = "Processing ends for QueueID :";
        private const string SERVICE_STOPPED = "Service has been stopped";

        #endregion

        #region Public Variables

        public Thread queueReaderThread;

        #endregion

        #region Private Variables

        private int PriorityNumber;

        private string QueueReaderNameSpace = MethodBase.GetCurrentMethod().DeclaringType.ToString();

        private DLEmail ObjDLEmail = new DLEmail();

        

        private DataSet qrPriorityQueue = null;
        private DataSet qrPriorityLookup = null;

        private int declined;
        private int closed;
        private int retry;
        private int workInProgress;
        private int readyToPost;
        private int retryFailure;
        private string serviceName;

        private int sleepTime;
        private int exceptionSleepTime;//Added on 11-Aug-2008
        private bool errorFlag;//Added on 11-Aug-2008

        private long queueID;

        private long orderNumber;
        private long actionID;

        private string companyID = String.Empty;
        private string clientID = String.Empty;
        private string orderAmount = String.Empty;

        private bool lastRetryFlag;



        private int retStatus = 0;
        private string xmlStringResponse = String.Empty;
        private int omsResponseCode = 0;
        private string omsResponseMessage = String.Empty;
        private int resCode = 0;
        private string resMessage = String.Empty;



        private XmlNodeList nlRes;
        private XmlNodeList nlMessage;

        #endregion

        #region Private Properties

        /// <summary>
        /// Property for getting the Thread Sleep Time
        /// </summary>
        private int SleepTime
        {
            get
            {
                try
                {
                    if (sleepTime == DEFAULT_INT || sleepTime == 0)
                    {
                        sleepTime = Convert.ToInt32(ConfigurationManager.AppSettings[SLEEP_TIME].ToString());

                    }
                }

                catch (System.FormatException ex)
                {
                    //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return sleepTime;

            }
        }

        /// <summary>
        /// Property for getting the Exception Sleep Time. Added on 11-Aug-2008
        /// </summary>
        private int ExceptionSleepTime
        {
            get
            {
                try
                {
                    if (exceptionSleepTime == DEFAULT_INT || exceptionSleepTime == 0)
                    {
                        exceptionSleepTime = Convert.ToInt32(ConfigurationManager.AppSettings[EXCEPTION_SLEEP_TIME].ToString());
                    }
                }

                catch (System.FormatException ex)
                {
                    //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return exceptionSleepTime;

            }
        }


        /// <summary>
        /// Property for getting the Error Code
        /// </summary>
        private int Error
        {
            get
            {
                try
                {
                    if (declined == DEFAULT_INT || declined == 0)
                    {
                        declined = Convert.ToInt32(ConfigurationManager.AppSettings[ERROR].ToString());

                    }
                }

                catch (System.FormatException ex)
                {
                    //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return declined;
            }
        }

        /// <summary>
        /// Property for getting the Closed Code
        /// </summary>
        private int Closed
        {
            get
            {
                try
                {
                    if (closed == DEFAULT_INT || closed == 0)
                    {
                        closed = Convert.ToInt32(ConfigurationManager.AppSettings[CLOSED].ToString());

                    }
                }

                catch (System.FormatException ex)
                {
                    //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return closed;

            }
        }

        /// <summary>
        /// Property for getting the Retry Code
        /// </summary>
        private int Retry
        {
            get
            {
                try
                {
                    if (retry == DEFAULT_INT || retry == 0)
                    {
                        retry = Convert.ToInt32(ConfigurationManager.AppSettings[RETRY].ToString());
                    }
                }

                catch (System.FormatException ex)
                {
                    //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return retry;
            }

        }

        /// <summary>
        /// Property for getting the Work In Progress Code
        /// </summary>
        private int WorkInProgress
        {
            get
            {
                try
                {
                    if (workInProgress == DEFAULT_INT || workInProgress == 0)
                    {
                        workInProgress = Convert.ToInt32(ConfigurationManager.AppSettings[WORK_IN_PROGRESS].ToString());

                    }
                }

                catch (System.FormatException ex)
                {
                    //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return workInProgress;
            }


        }

        /// <summary>
        /// Property for getting the Ready To Post Code
        /// </summary>
        private int ReadyToPost
        {
            get
            {
                try
                {
                    if (readyToPost == DEFAULT_INT || readyToPost == 0)
                    {
                        readyToPost = Convert.ToInt32(ConfigurationManager.AppSettings[READY_TO_POST].ToString());

                    }
                }

                catch (System.FormatException ex)
                {
                    //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return readyToPost;
            }
        }

        /// <summary>
        /// Property for getting the Retry Failure Code
        /// </summary>
        private int RetryFailure
        {
            get
            {
                try
                {
                    if (retryFailure == DEFAULT_INT || retryFailure == 0)
                    {
                        retryFailure = Convert.ToInt32(ConfigurationManager.AppSettings[RETRY_FAILURE].ToString());

                    }
                }

                catch (System.FormatException ex)
                {
                    //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return retryFailure;
            }
        }

        /// <summary>
        /// Property for getting the Service Name
        /// </summary>
        private string ServiceName
        {
            get
            {
                try
                {
                    if (serviceName == null)
                    {
                        serviceName = ConfigurationManager.AppSettings[SERVICE_NAME].ToString();

                    }
                }

                catch (System.FormatException ex)
                {
                    //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return serviceName;
            }
        }

        #endregion

        #region Priority Queue Reader
        /// <summary>
        /// 
        /// </summary>
        public void PriorityQueueReader()
        {
            if (qrPriorityQueue == null)
            {
                qrPriorityQueue = new DataSet();
            }

            try
            {
                errorFlag = false;

                while (queueReaderThread.IsAlive)

                try
                {
                    {
                        queueID = 0;
                        orderNumber = 0;
                        companyID = null;
                        clientID = null;
                        orderAmount = null;

                        qrPriorityQueue = GetPriorityQueueData(PriorityNumber);

                        if (qrPriorityQueue != null && qrPriorityQueue.Tables.Count > 0 && qrPriorityQueue.Tables[0].Rows.Count > 0)
                        {
                            queueID = Convert.ToInt64(qrPriorityQueue.Tables[0].Rows[0][QUEUEID].ToString());
                            lastRetryFlag = Convert.ToBoolean(qrPriorityQueue.Tables[0].Rows[0][LASTRETRY]);

                            actionID = Convert.ToInt32(qrPriorityQueue.Tables[0].Rows[0][REPORTSTATUSID].ToString());
                            orderNumber = Convert.ToInt64(qrPriorityQueue.Tables[0].Rows[0][ORDERNUMBER].ToString());
                            companyID = qrPriorityQueue.Tables[0].Rows[0][COMPANYID].ToString();
                            clientID = qrPriorityQueue.Tables[0].Rows[0][CLIENTID].ToString();
                            orderAmount = qrPriorityQueue.Tables[0].Rows[0][ORDERAMOUNT].ToString();

                            companyID = null;
                            clientID = null;
                            orderAmount = null;

                            errorFlag = false;//Added on 11-Aug-2008

                            xmlStringResponse = null;

                            omsResponseMessage = null;

                            qrPriorityQueue.Clear();

                        }
                        else
                        {

                            qrPriorityQueue.Clear();
                            xmlStringResponse = null;
                            omsResponseMessage = null;
                            companyID = null;
                            clientID = null;
                            orderAmount = null;

                        }

                        Thread.Sleep(SleepTime);

                    }
                }
                catch (IOException ex)
                {
                    //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }
                catch (System.StackOverflowException ex)
                {
                    //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }
                catch (InvalidOperationException ex)
                {
                    //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }
                catch (NullReferenceException ex)
                {
                    //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }
                catch (SqlException ex)
                {
                    if (errorFlag == false)
                    {
                        errorFlag = true;
                        //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                        //ExceptionLogger.PublishException(ex, "Error", ListenerType.Email);//Added on 11-Aug-2008
                    }
                    Thread.Sleep(ExceptionSleepTime);
                }
                catch (Exception ex)
                {
                    //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

            }
            catch (Exception ex)
            {
                //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
            }

        }
        #endregion

        #region Getting the rows from email queue table
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Priority"></param>
        /// <returns></returns>
        private DataSet GetPriorityQueueData(int Priority)
        {


            qrPriorityQueue.Clear();
            qrPriorityQueue = ObjDLEmail.GetOrderFromQueue(Priority);


            return qrPriorityQueue;
        }
        #endregion

        #region Send Mail
        
        public void SendMail()
        {
            try
            {
                string configurationPath = "";
                string subject = "";
                string message = "";
                string smtpHost = "";
                int smtpPort;
                string fromAddress = "";
                string techSupportNumber = "";
                //string CompanyCode = "";
                configurationPath = "..\\..\\EmailTemplates" + "\\" + "POSActivationTemplate.xml";
                FileStream fileStream = new FileStream(configurationPath, FileMode.Open, FileAccess.Read);
                DataSet emailTemplateData = new DataSet();
                emailTemplateData.ReadXml(fileStream);
                //fileStream.Close();
                //DataColumn[] columns = new DataColumn[1];
                //columns[0] = emailTemplateData.Tables[0].Columns["Key"];
                //emailTemplateData.Tables[0].PrimaryKey = columns;
                //emailTemplateData.Tables[0].AcceptChanges();
                //emailTemplateDataRow = emailTemplateData.Tables[0].Rows.Find(key);
                //subject = emailTemplateDataRow["Subject"].ToString();
                //subject = subject.Replace("@COMPANY_NAME@", companyName);
                //message = emailTemplateDataRow["Message"].ToString();
                ////OD1.5 Bug#4940 Smitha 28th Jan 2009
                //string wcsUrl = companyDetails.Tables[0].Rows[0]["WCS_ROOT_URL"].ToString();
                //string address1 = companyDetails.Tables[0].Rows[0]["AddressLine1"].ToString();
                //string address2 = companyDetails.Tables[0].Rows[0]["AddressLine2"].ToString();
                //string companyURL = companyDetails.Tables[0].Rows[0]["Company_URL"].ToString();
                //wcsUrl = wcsUrl + Constants.WCS_MAIN_LINK;
                //message = message.Replace("@WCS_URL@", wcsUrl);
                //message = message.Replace("@COMPANY_ADDRESS_LINE1@", address1);
                //message = message.Replace("@COMPANY_ADDRESS_LINE2@", address2);
                //message = message.Replace("@COMPANY_URL@", companyURL);
                //techSupportNumber = GetTechSupportNumber(CompanyCode);
                //message = message.Replace("@TECHNICAL_SUPPORT_PHONE@", techSupportNumber);
                //message = message.Replace("@COMPANYCODE@", CompanyCode);
                //message = message.Replace("@COMPANY_NAME@", companyName);
                //message = message.Replace("@CLIENTID@", clientID);
                //message = message.Replace("@USERID@", userID);
                //message = message.Replace("@PASSWORD@", password);

                //smtpHost = ConfigurationManager.AppSettings["smtpServer"];
                //smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
                //DataSet dsFromAddress = new DataSet();
                //dsFromAddress = GetExceptionMailDetails(CompanyCode);
                //fromAddress = dsFromAddress.Tables[0].Rows[0]["ExceptionEmailFromAddress"].ToString();
                //dsFromAddress = null;
                //SmtpClient smtpTempPwdClient = new SmtpClient(smtpHost, smtpPort);
                //smtpTempPwdClient.Send(fromAddress, emailID, subject, message);
            }
            catch (System.Net.Mail.SmtpException smtpException)
            {
                throw smtpException;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion

        
    }
}
