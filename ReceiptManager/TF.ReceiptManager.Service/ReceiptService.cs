
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: ReceiptService.cs
  Description: This class will allow to control the Receipt Manager service operations like Start and Stop.
  Date Created : 01-Jun-2011
  Revision History: 
  */

#endregion

#region Namespaces

using System.ServiceModel;
using System.ServiceProcess;
using log4net;

#endregion

namespace TF.ReceiptManager.Service
{
    #region Internal Namespaces

    using RGen;
    using RGen.SSRS;

    #endregion

    #region ReceiptService

    /// <summary>
    /// This class will allow to control the Receipt Manager service operations like Start and Stop.
    /// </summary>
    public partial class ReceiptService : ServiceBase
    {
        #region Member Variables

        //log4net logger interface.
        private ILog rmrLogger = null;

        #endregion

        #region Constructor

        /// <summary>
        /// This is the constructor of the class that initializes the service resources as well as logger.
        /// </summary>
        public ReceiptService()
        {
            InitializeComponent();

            //Initialize the logger.
            rmrLogger = LogManager.GetLogger("RMR");
        }

        #endregion

        #region OnStart

        /// <summary>
        /// Executes the receipt manager static constructor on start of the service.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            rmrLogger.Debug("Starting Receipt Manager Service.");

            ReportingService reportingService = Receipt.ReportingService;

            rmrLogger.Debug("Receipt Manager Service started successfully.");
        }

        #endregion

        #region OnStop

        /// <summary>
        ///  Closes the receipt manager WCF endpoints on stop of the service.
        /// </summary>
        protected override void OnStop()
        {
            rmrLogger.Debug("Stopping Receipt Manager Service.");

            //Stop the WCF Host that listens at "net.msmq://localhost/private/receiptmanager" endpoint if any.
            if (Receipt.GenReceiptServiceHost != null && Receipt.GenReceiptServiceHost.State == CommunicationState.Opened)
            {
                Receipt.GenReceiptServiceHost.Close();
                Receipt.GenReceiptServiceHost = null;

                rmrLogger.Debug("Stopped GenReceipt WCF service.");
            }

            //Stop the WCF Host that listens at "net.pipe://receiptmanager" endpoint if any.
            if (Receipt.FetchReceiptServiceHost != null && Receipt.FetchReceiptServiceHost.State == CommunicationState.Opened)
            {
                Receipt.FetchReceiptServiceHost.Close();
                Receipt.FetchReceiptServiceHost = null;

                rmrLogger.Debug("Stopped FetchReceipt WCF service.");
            }            

            rmrLogger.Debug("Receipt Manager Service stopped successfully.");
        }

        #endregion
    }

    #endregion
}
