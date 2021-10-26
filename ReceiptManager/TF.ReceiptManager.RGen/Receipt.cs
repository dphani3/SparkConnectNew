
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Receipt.cs
  Description: This is the base class for Receipt Manager application layer that will initialize the custom configuration section handlers on start
               of the hosting service.
  Date Created : 31-May-2011
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Configuration;
using System.Linq;
using System.Messaging;
using System.Net;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using log4net;

#endregion

namespace TF.ReceiptManager.RGen
{
    #region Internal Namespaces

    using BusinessLayer.BusinessMethods;
    using GMapsConfiguration;
    using ReportConfiguration;
    using SSRS;
    using SSRSConfiguration;

    #endregion

    #region Receipt

    /// <summary>
    ///  This is the base class for Receipt Manager application layer that will initialize the custom configuration section handlers on start of the 
    ///  hosting service.
    /// </summary>
    public class Receipt
    {
        #region Member Variables

        #region Constant Variables

        //Message Queuing service name.
        private const string MSMQ_SERVICE = "MSMQ";

        //Receipt Manager Message Queue path configuration key.
        private const string QUEUE_NAME = "ReceiptManagerQueue";

        //Transaction time zone index identifier.
        private const string TIME_ZONE_IDENTIFIER = "(";

        //Date and Time format for the receipt file name.
        protected const string DATETIME_FORMAT = "MMddyyyyHHmmss";

        //Successful transaction status message.
        protected const string SUCCESSFUL_TRANSACTION_STATUS = "APPROVED";

        //Transaction receipt file format.
        protected const string RECEIPT_FILE_NAME_FORMAT = "{0}-{1}-{2}-{3}.{4}";

        //Email priority.
        protected const int HIGH_PRIORITY = 1;

        //Money format to trim the additional decimal values.
        protected const string MONEY_FORMAT = "{0:F2}";

        //Database connection string key that can be mapped with configuration file.
        protected const string DATABASE_CONNECTION_STRING_KEY = "RMRCON";

        #endregion

        #region Static Variables

        //log4net logger interface.    
        internal static readonly ILog rmrLogger = null;

        //Google Static Maps Url.
        private static string gMapsStaticUrl = string.Empty;

        //WCF Host for "net.msmq://localhost/private/receiptmanager" endpoint.
        private static ServiceHost genReceiptServiceHost = null;

        //WCF Host for "net.pipe://receiptmanager" endpoint.
        private static ServiceHost fetchReceiptServiceHost = null;

        //Business layer object.
        protected static BusinessOperations businessLayer = null;    

        //SQL Server Reporting Services web service proxy object.
        private static readonly ReportingService reportingService = null;

        //SQL Server Reporting Services credentials configuration section.
        private static readonly SSRSConfigurationSection sSRSConfigurationSection = null;

        //SQL Server Reporting Services report format configuration section.
        private static readonly ReportConfigurationSection reportConfigurationSection = null;

        //Google Static Maps Url configuration section.
        private static readonly GMapsConfigurationSection gMapsConfigurationSection = null;            

        #endregion        

        #endregion

        #region Properties

        #region GenReceiptServiceHost

        /// <summary>
        /// Allows to get/set the WCF Host for "net.msmq://localhost/private/receiptmanager" endpoint.
        /// </summary>
        public static ServiceHost GenReceiptServiceHost
        {
            get { return Receipt.genReceiptServiceHost; }
            set { Receipt.genReceiptServiceHost = value; }
        }

        #endregion

        #region FetchReceiptServiceHost

        /// <summary>
        /// Allows to get/set the WCF Host for "net.pipe://receiptmanager" endpoint.
        /// </summary>
        public static ServiceHost FetchReceiptServiceHost
        {
            get { return Receipt.fetchReceiptServiceHost; }
            set { Receipt.fetchReceiptServiceHost = value; }
        }

        #endregion

        #region ReportingService

        /// <summary>
        /// Allows to get the SQL Server Reporting Services web service proxy object.
        /// </summary>
        public static ReportingService ReportingService
        {
            get { return Receipt.reportingService; }            
        }

        #endregion        

        #region ReportConfigurationSection

        /// <summary>
        /// Allows to get the SQL Server Reporting Services report format configuration section.
        /// </summary>
        protected static ReportConfigurationSection ReportConfigurationSection
        {
            get { return Receipt.reportConfigurationSection; }
        }

        #endregion

        #endregion

        #region Static Constructor

        /// <summary>
        /// This is the static constructor of the class that creates the Receipt Manager queue, hosts the WCF services and loads the custom configuration
        /// section handlers.
        /// </summary>
        static Receipt()
        {   
            //Initialize the logger.
            rmrLogger = LogManager.GetLogger("RMR");

            rmrLogger.Debug("RGen is starting.");                 

            //Initialize the MSMQ service status.
            bool msmqServiceStatus = false;

            //Get the MSMQ service controller instance.
            ServiceController msmqService = ServiceController.GetServices().Where((controller) => controller.ServiceName == MSMQ_SERVICE).Single();

            if (msmqService != null)
            {
                //Check whether the MSMQ service is started or not.
                msmqServiceStatus = (msmqService.Status == ServiceControllerStatus.Running) ? true : false;

                //If the service is not started, start the service and wait until it is running.
                if (!msmqServiceStatus)
                {
                    rmrLogger.Debug("Starting MSMQ service.");

                    msmqService.Start();
                    msmqService.WaitForStatus(ServiceControllerStatus.Running);

                    msmqServiceStatus = (msmqService.Status == ServiceControllerStatus.Running) ? true : false;
                }
                else
                {
                    rmrLogger.Debug("MSMQ service is already started.");
                }
            }
            else
            {
                rmrLogger.Debug("No MSMQ service exists in the system.");
            }

            //If the MSMQ service is running.
            if (msmqServiceStatus)
            {
                //Get the Receipt Manager queue name.
                string queueName = ConfigurationManager.AppSettings[QUEUE_NAME];  

                //Create the Queue if it is not exists.
                if (!MessageQueue.Exists(queueName))
                {
                    MessageQueue.Create(queueName, true);

                    rmrLogger.Debug("Message queue is created.");
                }
                else
                {
                    rmrLogger.Debug("Message queue is already exists.");
                }
            }
            else
            {
                rmrLogger.Debug("MSMQ service is not started");
            }

            //MSMQ service should be up and running before launching GenReceipt WCF service.
            if (msmqServiceStatus)
            {
                //Host the WCF service on "net.msmq://localhost/private/receiptmanager" endpoint.
                genReceiptServiceHost = new ServiceHost(typeof(GenReceipt));
                genReceiptServiceHost.Open();

                rmrLogger.Debug("Launched GenReceipt WCF service.");
            }
            else
            {
                rmrLogger.Debug("Failed to launch GenReceipt WCF service.");
            }

            //Host the WCF service on "net.pipe://receiptmanager" endpoint.
            fetchReceiptServiceHost = new ServiceHost(typeof(FetchReceipt));
            fetchReceiptServiceHost.Open();

            rmrLogger.Debug("Launched FetchReceipt WCF service.");

            businessLayer = new BusinessOperations(ConfigurationManager.ConnectionStrings[DATABASE_CONNECTION_STRING_KEY].ConnectionString.Trim());

            rmrLogger.Debug("Business layer initialized successfully.");

            //Load the SSRS credentials configuration section.
            sSRSConfigurationSection = SSRSConfigurationSection.GetSSRSConfigurationSection();

            //Check if the SSRS credentials configuration section loaded properly or not.
            if (sSRSConfigurationSection != null && sSRSConfigurationSection.SSRS != null)
            {
                rmrLogger.Debug("SSRSConfigurationSection loaded successfully.");

                //If the section contains SSRS web service url.
                if (!String.IsNullOrEmpty(sSRSConfigurationSection.SSRS.Url))
                {
                    //Create the instance of the SSRS web service proxy with configured url.
                    reportingService = new ReportingService { Url = sSRSConfigurationSection.SSRS.Url.Trim() };

                    //Check if the SSRS requires authentication or not.
                    if (sSRSConfigurationSection.SSRS.IsAuthenticationRequired)
                    {
                        rmrLogger.Debug("Authentication is required for SSRS.");

                        //Check whether domain, user name and password are provided in the configuration file or not.
                        if (!String.IsNullOrEmpty(sSRSConfigurationSection.SSRS.Domain) && !String.IsNullOrEmpty(sSRSConfigurationSection.SSRS.UserName)
                            && !String.IsNullOrEmpty(sSRSConfigurationSection.SSRS.Password))
                        {
                            //Assign the credentials to the SSRS web service proxy.
                            reportingService.Credentials = new NetworkCredential(sSRSConfigurationSection.SSRS.UserName.Trim(), sSRSConfigurationSection.SSRS.Password.Trim(),
                                sSRSConfigurationSection.SSRS.Domain.Trim());

                            rmrLogger.Debug("SSRS credentials applied successfully.");
                        }
                        else
                        {
                            rmrLogger.Debug("No SSRS credentials found in the configuration.");
                        }
                    }
                    else
                    {
                        rmrLogger.Debug("No authentication is required for SSRS.");
                    }
                }
                else
                {
                    rmrLogger.Debug("No SSRS url found in the configuration.");
                }
            }
            else
            {
                rmrLogger.Debug("Failed to load SSRSConfigurationSection.");
            }

            //Load the SSRS report format configuration section.
            reportConfigurationSection = ReportConfigurationSection.GetReportConfigurationSection();

            //Check if the SSRS report format configuration section loaded properly or not.
            if (reportConfigurationSection != null && reportConfigurationSection.Report != null)
            {
                rmrLogger.Debug("ReportConfigurationSection loaded successfully.");
            }
            else
            {
                rmrLogger.Debug("Failed to load ReportConfigurationSection.");
            }

            //Load the Google Static Maps configuration section.
            gMapsConfigurationSection = GMapsConfigurationSection.GetGMapsConfigurationSection();

            //Check if the Google Static Maps configuration section loaded properly or not.
            if (gMapsConfigurationSection != null && gMapsConfigurationSection.GoogleStaticMaps != null)
            {
                rmrLogger.Debug("GMapsConfigurationSection loaded successfully.");

                //If the section contains Google Static Maps url.
                if (!String.IsNullOrEmpty(gMapsConfigurationSection.GoogleStaticMaps.Url))
                {
                    //Construct the Google Static Maps url with given attributes as query strings.
                    StringBuilder gMapsUrlBuilder = new StringBuilder();

                    //Append the Google Static Maps url.
                    gMapsUrlBuilder.Append(String.Format("{0}?", gMapsConfigurationSection.GoogleStaticMaps.Url.Trim()));

                    //Append the "center" as query string if any.
                    if (!String.IsNullOrEmpty(gMapsConfigurationSection.GoogleStaticMaps.Center))
                        gMapsUrlBuilder.Append(String.Format("center={0}&", gMapsConfigurationSection.GoogleStaticMaps.Center.Trim()));

                    //Append the "zoom" as query string if any.
                    if (!String.IsNullOrEmpty(gMapsConfigurationSection.GoogleStaticMaps.Zoom))
                        gMapsUrlBuilder.Append(String.Format("zoom={0}&", gMapsConfigurationSection.GoogleStaticMaps.Zoom.Trim()));

                    //Append the "size" as query string if any.
                    if (!String.IsNullOrEmpty(gMapsConfigurationSection.GoogleStaticMaps.Size))
                        gMapsUrlBuilder.Append(String.Format("size={0}&", gMapsConfigurationSection.GoogleStaticMaps.Size.Trim()));

                    //Append the "format" as query string if any.
                    if (!String.IsNullOrEmpty(gMapsConfigurationSection.GoogleStaticMaps.Format))
                        gMapsUrlBuilder.Append(String.Format("format={0}&", gMapsConfigurationSection.GoogleStaticMaps.Format.Trim()));

                    //Append the "maptype" as query string if any.
                    if (!String.IsNullOrEmpty(gMapsConfigurationSection.GoogleStaticMaps.Maptype))
                        gMapsUrlBuilder.Append(String.Format("maptype={0}&", gMapsConfigurationSection.GoogleStaticMaps.Maptype.Trim()));

                    //Append the "markers" as query string if any.
                    if (!String.IsNullOrEmpty(gMapsConfigurationSection.GoogleStaticMaps.Markers))
                        gMapsUrlBuilder.Append(String.Format("markers={0}&", gMapsConfigurationSection.GoogleStaticMaps.Markers.Trim()));

                    //Append the "sensor" as query string if any.
                    if (!String.IsNullOrEmpty(gMapsConfigurationSection.GoogleStaticMaps.Sensor))
                        gMapsUrlBuilder.Append(String.Format("sensor={0}", gMapsConfigurationSection.GoogleStaticMaps.Sensor.Trim()));

                    //Construct the resultant Google Static Maps url.
                    gMapsStaticUrl = gMapsUrlBuilder.ToString();
                }
                else
                {
                    rmrLogger.Debug("No GMaps url found in the configuration.");
                }
            }
            else
            {
                rmrLogger.Debug("Failed to load GMapsConfigurationSection.");
            }

            rmrLogger.Debug("RGen is started.");
        }

        #endregion

        #region Protected Methods

        #region GenerateLocationMapUrl

        /// <summary>
        /// Generates the Google Static Maps Url with given geo location coordinates.
        /// </summary>
        /// <param name="latitude">Latitude of the location.</param>
        /// <param name="longitude">Longitude of the location.</param>
        /// <returns>Google Static Maps Url.</returns>
        protected string GenerateLocationMapUrl(string latitude, string longitude)
        {
            //Google Static Maps url.
            string locationMapUrl = string.Empty;

            //Construct the Google Static Maps url if the geo cordinates exists as well as url format.
            if (!String.IsNullOrEmpty(latitude) && !String.IsNullOrEmpty(longitude) && !String.IsNullOrEmpty(gMapsStaticUrl))
            {
                locationMapUrl = String.Format(gMapsStaticUrl, latitude, longitude);
            }

            //Send back the Google Static Maps url.
            return locationMapUrl;
        }

        #endregion

        #region ParseTransactionDate

        /// <summary>
        /// Parses the given transaction date into "MMddyyyyHHmmss" format.
        /// </summary>
        /// <param name="transactionDate">Transaction date.</param>
        /// <returns>Parsed transaction date into "MMddyyyyHHmmss" format.</returns>
        protected string ParseTransactionDate(string transactionDate)
        {
            //Parsed transaction date.
            string parsedTransactionDate = string.Empty;

            //Check whether the transaction date has been provided or not.
            if (!String.IsNullOrEmpty(transactionDate))
            {
                //Check whether the transaction date has any time zone information.
                if (transactionDate.Contains(TIME_ZONE_IDENTIFIER))
                {
                    //Remove the time zone information from transaction date.
                    transactionDate = transactionDate.Substring(0, transactionDate.IndexOf(TIME_ZONE_IDENTIFIER));

                    //Parse the transaction date into "MMddyyyyHHmmss" format.
                    parsedTransactionDate = DateTime.Parse(transactionDate).ToString(DATETIME_FORMAT);
                }
                //Parse the transaction date.
                else
                {
                    //Parse the transaction date into "MMddyyyyHHmmss" format.
                    parsedTransactionDate = DateTime.Parse(transactionDate).ToString(DATETIME_FORMAT);
                }
            }

            //Send back the parsed transaction date.
            return parsedTransactionDate;
        }

        #endregion

        #endregion
    }

    #endregion
}
