#region Copyright
/* Copyright 2010 (c), ThoughtFocus.
  
  All rights are reserved.  Reproduction or transmission in whole or in part,
  in any form or by any means, electronic, mechanical or otherwise,
  is prohibited without the prior written consent of the copyright owner.
  Author(s): Devaraj
  File Name: Constants.cs
  Description: String constants are declared here.
  Date Created : 08-April-2010
  Revision History:
  */
#endregion

namespace TF.FocusPay.EmailServer.EmailBusinessLogic
{
    public static class Constants
    {
        #region TABLE COLUMN CONSTANTS

        public const string TBL_COLUMN_QUEUE_ID = "QueueID";
        public const string TBL_COLUMN_COMPANY_ID = "CompanyID";
        public const string TBL_COLUMN_MERCHANT_ID = "MerchantID";
        public const string TBL_COLUMN_ATTENDANT_ID = "AttendantID";
        public const string TBL_COLUMN_MSG_BODY_PARAMS = "MessageBodyParams";
        public const string TBL_COLUMN_STATUS = "StatusID";
        public const string TBL_COLUMN_RETRY_COUNT = "RetryCount";
        public const string TBL_COLUMN_EMAIL_TYPE_ID = "EmailTypeID";
        public const string TBL_COLUMN_MESSAGE = "Message";
        public const string TBL_COLUMN_SUBJECT = "Subject";
        public const string TBL_COLUMN_TO_ADDRESS = "ToAddress";
        public const string TBL_COLUMN_FROM_ADDRESS = "FromAddress";
        public const string TBL_COLUMN_CC_LIST = "CCList";
        public const string TBL_COLUMN_RECEIPT_STREAM = "ReceiptStream";

        #endregion

        #region EMAIL TEMPLATE CONSTANTS

        public const string EMAIL_TEMPLATE_FIRST_NAME                           = "@FIRST_NAME@";
        public const string EMAIL_TEMPLATE_ATTENDANT_NAME                       = "@ATTENDANT_NAME@";
        public const string EMAIL_TEMPLATE_ATTENDANT_EMAIL                      = "@ATTENDANT_EMAIL@";
        public const string EMAIL_TEMPLATE_ATTENDANT_CODE                       = "@ATTENDANT_CODE@";
        public const string EMAIL_TEMPLATE_APPLICATION_ID                       = "@APPICATION_ID@";
        public const string EMAIL_TEMPLATE_TOTAL                                = "@TOTAL@";
        public const string EMAIL_TEMPLATE_COMPANY_ADMIN_EMAIL_ID               = "@COMPANY_ADMIN_EMAIL_ID@";
        public const string EMAIL_TEMPLATE_COMPANY_URL                          = "@COMPANY_URL@";
        public const string EMAIL_TEMPLATE_COMPANY_NAME                         = "@COMPANY_NAME@";
        public const string EMAIL_TEMPLATE_TOTAL_POS_LICENSE_AT_INSTANCE        = "@TOT_POS_LICENSE_AT_INSTANCE@";
        public const string EMAIL_TEMPLATE_MERCHANT_NAME                        = "@MERCHANT_NAME@";
        public const string EMAIL_TEMPLATE_MERCHANT_EMAIL                       = "@MERCHANT_EMAIL@";
        public const string EMAIL_TEMPLATE_MERCHANT_CONTACT_PHONE               = "@MERCHANT_CONTACT_PHONE@"; //Added by Nazreen
        public const string EMAIL_TEMPLATE_PASSWORD                             = "@PASSWORD@";
        public const string EMAIL_TEMPLATE_TRANS_TYPE                           = "@TRANSACTION_TYPE@";
        public const string EMAIL_TEMPLATE_CHANAKYA_ID                          = "@FCRRN_NUMBER@";
        public const string EMAIL_TEMPLATE_TRANS_ID                             = "@TRANSACTION_ID@";
        public const string EMAIL_TEMPLATE_TRANS_DATE                           = "@TRANSACTION_DATE@";
        public const string EMAIL_TEMPLATE_TRANS_TIME                           = "@TRANSACTION_TIME@";
        public const string EMAIL_TEMPLATE_TRANS_AMOUNT                         = "@TRANSACTION_AMOUNT@";
        public const string EMAIL_TEMPLATE_MASKED_CARD_NO                       = "@MASKED_CARD_NUMBER@";       
        public const string EMAIL_TEMPLATE_INVOICE_NUM                          = "@INVOICE_NUMBER@";        
        public const string EMAIL_TEMPLATE_TIP_AMOUNT                           = "@TIP_AMOUNT@";
        public const string EMAIL_TEMPLATE_TRANS_STATUS                         = "@TRANSACTION_STATUS@";
        public const string EMAIL_TEMPLATE_GMAPS_URL                            = "@GMAPS_URL@";
        public const string EMAIL_TEMPLATE_USER_NAME                            = "@USER_NAME@";
        public const string EMAIL_TEMPLATE_TRANS_NOTES                          = "@TRANSACTION_NOTES@"; //Added by Nazreen
        public const string EMAIL_TEMPLATE_TRANS_CONVENIENCE_FEE                = "@CONVENIENCE_FEE@";  //Added by Nazreen
        public const string EMAIL_TEMPLATE_TRANS_TOTAL_AMOUNT                   = "@TOTAL_AMOUNT@";  //Added by Nazreen
        public const string EMAIL_TEMPLATE_RECEIPT_HEADER1                      = "@RECEIPT_HEADER1@";
        public const string EMAIL_TEMPLATE_RECEIPT_HEADER2                      = "@RECEIPT_HEADER2@";
        public const string EMAIL_TEMPLATE_RECEIPT_HEADER3                      = "@RECEIPT_HEADER3@";
        public const string EMAIL_TEMPLATE_RECEIPT_FOOTER1                      = "@RECEIPT_FOOTER1@";
        public const string EMAIL_TEMPLATE_RECEIPT_FOOTER2                      = "@RECEIPT_FOOTER2@";
        public const string EMAIL_TEMPLATE_RECEIPT_FOOTER3                      = "@RECEIPT_FOOTER3@";        
        public const string EMAIL_TEMPLATE_NO_OF_POS_LICENSES                   = "@NO_OF_POS_LICENSES@";
        public const string EMAIL_COM_MNT_NAME                                  = "@NAME@";
        public const string EMAIL_PLATFORM_NAME                                 = "@PLATFORM_NAME@";        
        public const string EMAIL_PLATFORM_SALES_EMAIL_ADDRESS                  = "@PLATFORM_SALES_EMAIL_ADDRESS@";        
        public const string EMAIL_PLATFORM_WEBSITE                              = "@PLATFORM_WEBSITE@";
        public const string EMAIL_PLATFORM_USER_NAME_OR_USER_CODE               = "@USER_NAME_OR_USER_CODE@";
        public const string EMAIL_PLATFORM_COMPANY_ADMIN_NAME                   = "COMPANY_ADMIN_NAME";            
        public const string EMAIL_PLATFORM_FROM_URL                             = "@PLATFORM_FROM_URL@";

        public const string EMAIL_BRAND_NAME                                    = "@BRAND_NAME@";
        public const string EMAIL_ENTITY_NAME                                   = "@ENTITY_NAME@";
        public const string EMAIL_COMPANY_SALES_EMAIL                           = "@COMPANY_SALES_EMAIL@";
        public const string EMAIL_COMPANY_SALES_PHONE                           = "@COMPANY_SALES_PHONE@";
        public const string EMAIL_COMPANY_SUPPORT_EMAIL                         = "@COMPANY_SUPPORT_EMAIL@";
        public const string EMAIL_COMPANY_SUPPORT_PHONE                         = "@COMPANY_SUPPORT_PHONE@";

        public const string EMAIL_FORCE_CHANGE_PASSWORD                         = "@FORCE_CHANGE_PASSWORD@";
        public const string EMAIL_MIN_PASSWORD_LENGTH                           = "@MIN_PASSWORD_LENGTH@";
        public const string EMAIL_SAME_PASSWORD_USAGE                           = "@SAME_PASSWORD_USAGE@";
        public const string EMAIL_INVALID_PASSWORD_ATTEMPTS                     = "@INVALID_PASSWORD_ATTEMPTS@";
        public const string EMAIL_LOCKOUT_TIME                                  = "@LOCKOUT_TIME@";
        public const string EMAIL_SESSION_TIMEOUT                               = "@SESSION_TIMEOUT@";
        public const string EMAIL_USER_INACTIVE_DAYS                            = "@USER_INACTIVE_DAYS@";        

        public const string PROSPECT_NAME                                       = "@PROSPECT_NAME@";
        public const string PROSPECT_PHONE                                      = "@PROSPECT_PHONE@";
        public const string PROSPECT_EMAIL                                      = "@PROSPECT_EMAIL@";
        public const string PROSPECT_PLATFORMS                                  = "@PROSPECT_PLATFORMS@";
        public const string PROSPECT_REMARKS                                    = "@PROSPECT_REMARKS@";
        public const string PROSPECT_LICENSES_NUMBER                            = "@PROSPECT_LICENSES_NUMBER@";
        public const string PROSPECT_PREFERRED_PROGRAM                          = "@PROSPECT_PREFERRED_PROGRAM@";
        public const string PROSPECT_INDUSTRY                                   = "@PROSPECT_INDUSTRY@";

        public const string UID_CHANGE_MIN_LENGTH                               = "@MIN_UID_LENGTH@";
        public const string EMAIL_TEMPLATE_INVOIVE_NUMBER                       = "@INVOIVE_NUMBER@";
        public const string EMAIL_TEMPLATE_ORDER_NUMBER                         = "@ORDER_NUMBER@";
        #endregion
    }
}
