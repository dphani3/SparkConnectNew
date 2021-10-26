
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: BusinessOperations.cs
  Description: This class will handle all the business operations related to Receipt Manager.
  Date Created : 31-May-2011
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Data;

#endregion

namespace TF.ReceiptManager.BusinessLayer.BusinessMethods
{
    #region Internal Namespaces

    using BusinessObjects;
    using DataAccessLayer;

    #endregion

    #region BusinessOperations

    /// <summary>
    /// This class will handle all the business operations related to Receipt Manager.
    /// </summary>
    public partial class BusinessOperations : IDisposable
    {
        #region Member Variables

        private bool isDisposed = false;                    //This is for Garbage Collector.
        private DAL dataAccessLayer = null;                 //Data Access Layer object to communicate with database.

        #endregion

        #region Constructor

        /// <summary>
        /// This is the Constructor for the class that will instantiate the data access layer.
        /// </summary>
        /// <param name="connectionStringName">Database connection string information.</param>
        public BusinessOperations(string connectionStringName)
        {
            this.dataAccessLayer = new DAL(connectionStringName);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// This is the Destructor for the class.
        /// </summary>
        ~BusinessOperations()
        {
            //Since finalizer is called, there is no need to free the managed resources. So, false is passed.
            Dispose(false);
        }

        #endregion

        #region StoreReceiptDetails

        /// <summary>
        /// Stores the transaction receipt details along with geo location url.
        /// </summary>
        /// <param name="chanakyaId">Chanakya transaction id.</param>
        /// <param name="geoLocationUrl">Google static maps url.</param>
        /// <param name="receiptStream">Transaction receipt data in binary format.</param>
        /// <param name="receiptFileName">Transaction receipt file name.</param>
        /// <param name="userId">User ID.</param>
        /// <returns>Scalar code that details the status of the receipt details data storage operation.</returns>
        public int StoreReceiptDetails(long chanakyaId, string geoLocationUrl, byte[] receiptStream, string receiptFileName, long userId)
        {
            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                int rowsEffected = dataAccessLayer.ExecuteNonQuery(SP_UPSERT_RECEIPT_DETAILS, ref responseCode,
                                                dataAccessLayer.CreateParameter(SP_UPSERT_RECEIPT_DETAILS_IN_PARAM_CHANAKYA_TRANSACTION_ID, SqlDbType.BigInt, chanakyaId),
                                                dataAccessLayer.CreateParameter(SP_UPSERT_RECEIPT_DETAILS_IN_PARAM_GEO_LOCATION_URL, SqlDbType.VarChar, geoLocationUrl),
                                                dataAccessLayer.CreateParameter(SP_UPSERT_RECEIPT_DETAILS_IN_PARAM_RECEIPT_STREAM, SqlDbType.VarBinary, receiptStream),
                                                dataAccessLayer.CreateParameter(SP_UPSERT_RECEIPT_DETAILS_IN_PARAM_RECEIPT_FILE_NAME, SqlDbType.VarChar, receiptFileName),
                                                dataAccessLayer.CreateParameter(SP_UPSERT_RECEIPT_DETAILS_IN_PARAM_USER_ID, SqlDbType.BigInt, userId));

                //If more than zero rows effected, operation is successful.
                if (rowsEffected > 0)
                    responseCode = 0;
                //Else, return as database error.
                else
                    responseCode = -1;
            }
            catch (Exception)
            {
                //Send response code as system error.
                responseCode = -2;
            }

            //Send back the response code.
            return responseCode;
        }

        #endregion

        #region GetLatestTransactionDetails

        /// <summary>
        /// Gets the latest transaction information related to the given FCRRN.
        /// </summary>
        /// <param name="fCRRN">FCRRN number.</param>        
        /// <param name="userId">User ID.</param>
        /// <returns>Latest transaction information.</returns>
        public LatestTransactionInformation GetLatestTransactionDetails(string fCRRN, long userId)
        {            
            //Declare Latest transaction information object.
            LatestTransactionInformation transactionInformation = null;

            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                using (DataSet transactionInformationSet = dataAccessLayer.GetDataSet(SP_GET_LATEST_TRANSACTION_FOR_FCRRN, ref responseCode,
                                                dataAccessLayer.CreateParameter(SP_GET_LATEST_TRANSACTION_FOR_FCRRN_IN_PARAM_FCRRN, SqlDbType.VarChar, fCRRN),                                                
                                                dataAccessLayer.CreateParameter(SP_GET_LATEST_TRANSACTION_FOR_FCRRN_IN_PARAM_USER_ID, SqlDbType.BigInt, userId)))
                {
                    //If the database operation execution is successful. 
                    if (responseCode == 0)
                    {
                        //If the dataset is not null and if it doesn't have any errors.
                        if (transactionInformationSet != null && !transactionInformationSet.HasErrors)
                        {
                            //Count check.
                            if (transactionInformationSet.Tables.Count == 4)
                            {
                                //Fill the transaction information.
                                transactionInformation = new LatestTransactionInformation
                                {
                                    ReceiptStream               = (transactionInformationSet.Tables[0].Rows.Count == 1) ? (transactionInformationSet.Tables[0].Rows[0][0] != DBNull.Value) ? (byte[])transactionInformationSet.Tables[0].Rows[0][0] : null : null,
                                    ReceiptFileName             = (transactionInformationSet.Tables[0].Rows.Count == 1) ? transactionInformationSet.Tables[0].Rows[0][1].ToString() : null,
                                    GeoLocationUrl              = (transactionInformationSet.Tables[0].Rows.Count == 1) ? transactionInformationSet.Tables[0].Rows[0][2].ToString() : null,

                                    CompanyId                   = (transactionInformationSet.Tables[1].Rows.Count == 1) ? (long)transactionInformationSet.Tables[1].Rows[0][0] : 0,
                                    CompanyName                 = (transactionInformationSet.Tables[1].Rows.Count == 1) ? transactionInformationSet.Tables[1].Rows[0][1].ToString() : null,
                                    MerchantId                  = (transactionInformationSet.Tables[1].Rows.Count == 1) ? (long)transactionInformationSet.Tables[1].Rows[0][2] : 0,
                                    MerchantName                = (transactionInformationSet.Tables[1].Rows.Count == 1) ? transactionInformationSet.Tables[1].Rows[0][3].ToString() : null,
                                    AttendantId                 = (transactionInformationSet.Tables[1].Rows.Count == 1) ? (long)transactionInformationSet.Tables[1].Rows[0][4] : 0,
                                    AttendantName               = (transactionInformationSet.Tables[1].Rows.Count == 1) ? transactionInformationSet.Tables[1].Rows[0][5].ToString() : null,
                                    UserName                    = (transactionInformationSet.Tables[1].Rows.Count == 1) ? transactionInformationSet.Tables[1].Rows[0][6].ToString() : null,
                                    SupportPhone                = (transactionInformationSet.Tables[1].Rows.Count == 1) ? transactionInformationSet.Tables[1].Rows[0][7].ToString() : null,
                                    SupportEmailAddress         = (transactionInformationSet.Tables[1].Rows.Count == 1) ? transactionInformationSet.Tables[1].Rows[0][8].ToString() : null,
                                    SalesPhone                  = (transactionInformationSet.Tables[1].Rows.Count == 1) ? transactionInformationSet.Tables[1].Rows[0][9].ToString() : null,
                                    SalesEmailAddress           = (transactionInformationSet.Tables[1].Rows.Count == 1) ? transactionInformationSet.Tables[1].Rows[0][10].ToString() : null,
                                    BrandName                   = (transactionInformationSet.Tables[1].Rows.Count == 1) ? transactionInformationSet.Tables[1].Rows[0][11].ToString() : null,
                                    NotificationEmailAddress    = (transactionInformationSet.Tables[1].Rows.Count == 1) ? transactionInformationSet.Tables[1].Rows[0][12].ToString() : null,
                                    MerchantEmailAddress        = (transactionInformationSet.Tables[1].Rows.Count == 1) ? transactionInformationSet.Tables[1].Rows[0][13].ToString() : null,
                                    MerchantContactPhone        = (transactionInformationSet.Tables[1].Rows.Count == 1) ? transactionInformationSet.Tables[1].Rows[0][14].ToString() : null, 

                                    FCRRN                       = (transactionInformationSet.Tables[2].Rows.Count == 1) ? transactionInformationSet.Tables[2].Rows[0][0].ToString() : null,
                                    TransactionType             = (transactionInformationSet.Tables[2].Rows.Count == 1) ? transactionInformationSet.Tables[2].Rows[0][1].ToString() : null,
                                    TransactionDate             = (transactionInformationSet.Tables[2].Rows.Count == 1) ? transactionInformationSet.Tables[2].Rows[0][2].ToString() : null,
                                    Amount                      = (transactionInformationSet.Tables[2].Rows.Count == 1) ? (decimal)transactionInformationSet.Tables[2].Rows[0][3] : 0,
                                    AdditionalAmount            = (transactionInformationSet.Tables[2].Rows.Count == 1) ? (decimal)transactionInformationSet.Tables[2].Rows[0][4] : 0,
                                    CurrencyCode                = (transactionInformationSet.Tables[2].Rows.Count == 1) ? transactionInformationSet.Tables[2].Rows[0][5].ToString() : null,
                                    GatewayTransactionId        = (transactionInformationSet.Tables[2].Rows.Count == 1) ? transactionInformationSet.Tables[2].Rows[0][6].ToString() : null,
                                    CardNumber                  = (transactionInformationSet.Tables[2].Rows.Count == 1) ? transactionInformationSet.Tables[2].Rows[0][7].ToString() : null,
                                    TransactionStatus           = (transactionInformationSet.Tables[2].Rows.Count == 1) ? transactionInformationSet.Tables[2].Rows[0][8].ToString() : null,
                                    CustomerName                = (transactionInformationSet.Tables[2].Rows.Count == 1) ? transactionInformationSet.Tables[2].Rows[0][9].ToString() : null,
                                    ChanakyaID                  = (transactionInformationSet.Tables[2].Rows.Count == 1) ? (long)transactionInformationSet.Tables[2].Rows[0][10] : 0,

                                    CustomerEmailAddress        = (transactionInformationSet.Tables[3].Rows.Count == 1) ? transactionInformationSet.Tables[3].Rows[0][0].ToString() : null,
                                };
                            }
                            else
                            {
                                //Send null transaction information.
                                transactionInformation = null;
                            }
                        }
                        else
                        {
                            //Send null transaction information.
                            transactionInformation = null;
                        }
                    }
                    //If the database operation execution is failed due to database error or system error.
                    else if (responseCode == -1 || responseCode == -2)
                    {
                        //Send null transaction information.
                        transactionInformation = null;
                    }
                }
            }
            catch (Exception)
            {
                //Send null transaction information.
                transactionInformation = null;
            }

            //Return transaction information.
            return transactionInformation;
        }

        #endregion

        #region SendToEmailQueue

        /// <summary>
        /// Sends an e-mail to the destined user with given email information.
        /// </summary>
        /// <param name="emailInformation">E-mail information.</param>
        /// <returns>Scalar code that details the status of the e-mail queuing operation.</returns>
        public int SendToEmailQueue(EmailInformation emailInformation)
        {
            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                int rowsEffected = dataAccessLayer.ExecuteNonQuery(SP_ADD_RECEIPT_TO_EMAIL_QUEUE, ref responseCode,
                                                    dataAccessLayer.CreateParameter(SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_SYSTEM_ID, SqlDbType.VarChar, emailInformation.SystemId),
                                                    dataAccessLayer.CreateParameter(SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_COMPANY_ID, SqlDbType.BigInt, emailInformation.CompanyId),
                                                    dataAccessLayer.CreateParameter(SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_MERCHANT_ID, SqlDbType.BigInt, emailInformation.MerchantId),
                                                    dataAccessLayer.CreateParameter(SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_ATTENDANT_ID, SqlDbType.BigInt, emailInformation.AttendantId),
                                                    dataAccessLayer.CreateParameter(SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_EMAIL_TYPE_ID, SqlDbType.Int, emailInformation.EmailTypeId),
                                                    dataAccessLayer.CreateParameter(SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_FROM_ADDRESS, SqlDbType.VarChar, emailInformation.FromAddress),
                                                    dataAccessLayer.CreateParameter(SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_TO_ADDRESS, SqlDbType.VarChar, emailInformation.ToAddress),
                                                    dataAccessLayer.CreateParameter(SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_CC_LIST, SqlDbType.VarChar, emailInformation.CCList),
                                                    dataAccessLayer.CreateParameter(SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_MESSAGE_BODY_PARAMS, SqlDbType.VarChar, emailInformation.MessageBody),
                                                    dataAccessLayer.CreateParameter(SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_PRIORITY, SqlDbType.Int, emailInformation.Priority),
                                                    dataAccessLayer.CreateParameter(SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_TEST_MODE, SqlDbType.Char, emailInformation.IsTestMode ? char.Parse("Y") : char.Parse("N")),
                                                    dataAccessLayer.CreateParameter(SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_RECEIPT_STREAM, SqlDbType.VarBinary, emailInformation.ReceiptStream));

                //If more than zero rows effected, operation is successful.
                if (rowsEffected > 0)
                    responseCode = 0;
                //Else, return as database error.
                else
                    responseCode = -1;
            }
            catch (Exception)
            {
                //Send response code as system error.
                responseCode = -2;
            }

            //Send back the response code.
            return responseCode;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Implementation of dispose to free resources.
        /// </summary>
        /// <param name="disposedStatus">The status of the disposed operation.</param>
        protected virtual void Dispose(bool disposedStatus)
        {
            if (!isDisposed)
            {
                isDisposed = true;

                //Released unmanaged resources.
                if (disposedStatus)
                {
                    //Release the managed resources. 

                    //Dispose the data access layer. 
                    if (dataAccessLayer != null)
                    {
                        dataAccessLayer.Dispose();
                        dataAccessLayer = null;
                    }
                }
            }
        }

        #endregion

        #region IDisposable Members

        #region Dispose

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //True is passed in dispose method to clean managed resources.
            Dispose(true);

            //If dispose is called already, then inform GC to skip finalize on this instance.
            GC.SuppressFinalize(this);
        }

        #endregion

        #endregion
    }

    #endregion
}
