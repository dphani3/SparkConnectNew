
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Utility.cs
  Description:  This is the partial class for BusinessOperations class to extend the utility functions for the main BusinessOperations class.
  Date Created : 31-May-2011
  Revision History: 
  */

#endregion

namespace TF.ReceiptManager.BusinessLayer.BusinessMethods
{
    #region Utility

    /// <summary>
    /// This is the partial class for BusinessOperations class to extend the utility functions for the main BusinessOperations class.
    /// </summary>
    public partial class BusinessOperations
    {
        #region Stored Procedure Declarations

        #region RMR_UpsertReceiptDetails

        const string SP_UPSERT_RECEIPT_DETAILS                                                  = "RMR_UpsertReceiptDetails";

        const string SP_UPSERT_RECEIPT_DETAILS_IN_PARAM_CHANAKYA_TRANSACTION_ID                 = "@ChanakyaTransactionID";
        const string SP_UPSERT_RECEIPT_DETAILS_IN_PARAM_GEO_LOCATION_URL                        = "@GeoLocationURL";
        const string SP_UPSERT_RECEIPT_DETAILS_IN_PARAM_RECEIPT_STREAM                          = "@ReceiptStream";
        const string SP_UPSERT_RECEIPT_DETAILS_IN_PARAM_RECEIPT_FILE_NAME                       = "@ReceiptFileName";
        const string SP_UPSERT_RECEIPT_DETAILS_IN_PARAM_USER_ID                                 = "@UserID";

        #endregion

        #region RMR_GetLatestTransactionForFCRRN

        const string SP_GET_LATEST_TRANSACTION_FOR_FCRRN                                        = "RMR_GetLatestTransactionForFCRRN";

        const string SP_GET_LATEST_TRANSACTION_FOR_FCRRN_IN_PARAM_FCRRN                         = "@FCRRN";        
        const string SP_GET_LATEST_TRANSACTION_FOR_FCRRN_IN_PARAM_USER_ID                       = "@UserID";       

        #endregion

        #region RMR_AddReceiptToEmailQueue

        const string SP_ADD_RECEIPT_TO_EMAIL_QUEUE                                              = "RMR_AddReceiptToEmailQueue";

        const string SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_SYSTEM_ID                           = "@SystemID";
        const string SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_COMPANY_ID                          = "@CompanyID";
        const string SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_MERCHANT_ID                         = "@MerchantID";
        const string SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_ATTENDANT_ID                        = "@AttendantID";
        const string SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_EMAIL_TYPE_ID                       = "@EmailTypeID";
        const string SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_FROM_ADDRESS                        = "@FromAddress";
        const string SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_TO_ADDRESS                          = "@ToAddress";
        const string SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_CC_LIST                             = "@CCList";
        const string SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_MESSAGE_BODY_PARAMS                 = "@MessageBodyParams";
        const string SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_PRIORITY                            = "@Priority";
        const string SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_TEST_MODE                           = "@TestMode";
        const string SP_ADD_RECEIPT_TO_EMAIL_QUEUE_IN_PARAM_RECEIPT_STREAM                      = "@ReceiptStream";

        #endregion

        #endregion
    }

    #endregion
}
