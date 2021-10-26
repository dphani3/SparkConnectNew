
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: LatestTransactionInformation.cs
  Description: This class is the business object representation for latest transaction information.
  Date Created : 31-May-2011
  Revision History: 
  */

#endregion

namespace TF.ReceiptManager.BusinessLayer.BusinessObjects
{
    #region LatestTransactionInformation

    /// <summary>
    /// This class is the business object representation for latest transaction information.
    /// </summary>
    public class LatestTransactionInformation
    {
        #region Member Variables

        private byte[] receiptStream;               //Transaction receipt stream.
        private string receiptFileName;             //Transaction receipt file name.

        private long companyId;                     //Attendants company id.
        private string companyName;                 //Attendants company name.
        private long merchantId;                    //Attendants merchant id.
        private string merchantName;                //Attendants merchant name.
        private long attendantId;                   //Attendant id.
        private string userName;                    //Attendants user name.
        private string supportPhone;                //Attendants company support phone.
        private string supportEmailAddress;         //Attendants company support email address.
        private string salesPhone;                  //Attendants company sales phone.
        private string salesEmailAddress;           //Attendants company sales email address.
        private string brandName;                   //Attendants company brand name.
        private string notificationEmailAddress;    //Attendants company notification email address.

        private string transactionType;             //Transaction type.
        private string transactionDate;             //Transaction date.
        private decimal amount;                     //Transaction amount.
        private decimal additionalAmount;           //Transaction additional amount if any.
        private string currencyCode;                //Transaction currency type.

        #endregion

        #region Properties

        #region ReceiptStream

        /// <summary>
        /// Allows to get/set the transactions receipt stream.
        /// </summary>
        public byte[] ReceiptStream
        {
            get { return receiptStream; }
            set { receiptStream = value; }
        }

        #endregion

        #region ReceiptFileName

        /// <summary>
        /// Allows to get/set the transactions receipt file name.
        /// </summary>
        public string ReceiptFileName
        {
            get { return receiptFileName; }
            set { receiptFileName = value; }
        }

        #endregion

        #region CompanyId

        /// <summary>
        /// Allows to get/set the attendants company id.
        /// </summary>
        public long CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        #endregion

        #region CompanyName

        /// <summary>
        /// Allows to get/set the attendants company name.
        /// </summary>
        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        #endregion

        #region MerchantId

        /// <summary>
        /// Allows to get/set the attendants merchant id.
        /// </summary>
        public long MerchantId
        {
            get { return merchantId; }
            set { merchantId = value; }
        }

        #endregion

        #region MerchantName

        /// <summary>
        /// Allows to get/set the attendants merchant name.
        /// </summary>
        public string MerchantName
        {
            get { return merchantName; }
            set { merchantName = value; }
        }

        #endregion

        #region AttendantId

        /// <summary>
        /// Allows to get/set the attendants id.
        /// </summary>
        public long AttendantId
        {
            get { return attendantId; }
            set { attendantId = value; }
        }

        #endregion

        #region UserName

        /// <summary>
        /// Allows to get/set the attendants user name.
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        #endregion

        #region SupportPhone

        /// <summary>
        /// Allows to get/set the attendants company support phone.
        /// </summary>
        public string SupportPhone
        {
            get { return supportPhone; }
            set { supportPhone = value; }
        }

        #endregion

        #region SupportEmailAddress

        /// <summary>
        /// Allows to get/set the attendants company support email address.
        /// </summary>
        public string SupportEmailAddress
        {
            get { return supportEmailAddress; }
            set { supportEmailAddress = value; }
        }

        #endregion

        #region SalesPhone

        /// <summary>
        /// Allows to get/set the attendants company sales phone.
        /// </summary>
        public string SalesPhone
        {
            get { return salesPhone; }
            set { salesPhone = value; }
        }

        #endregion

        #region SalesEmailAddress

        /// <summary>
        /// Allows to get/set the attendants company sales email address.
        /// </summary>
        public string SalesEmailAddress
        {
            get { return salesEmailAddress; }
            set { salesEmailAddress = value; }
        }

        #endregion

        #region BrandName

        /// <summary>
        /// Allows to get/set the attendants company brand name.
        /// </summary>
        public string BrandName
        {
            get { return brandName; }
            set { brandName = value; }
        }

        #endregion

        #region NotificationEmailAddress

        /// <summary>
        /// Allows to get/set the attendants company notification email address.
        /// </summary>
        public string NotificationEmailAddress
        {
            get { return notificationEmailAddress; }
            set { notificationEmailAddress = value; }
        }

        #endregion

        #region TransactionType

        /// <summary>
        /// Allows to get/set the transaction type.
        /// </summary>
        public string TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }

        #endregion

        #region TransactionDate

        /// <summary>
        /// Allows to get/set the transaction date.
        /// </summary>
        public string TransactionDate
        {
            get { return transactionDate; }
            set { transactionDate = value; }
        }

        #endregion

        #region Amount

        /// <summary>
        /// Allows to get/set the transaction amount.
        /// </summary>
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        #endregion

        #region AdditionalAmount

        /// <summary>
        /// Allows to get/set the transaction additional amount if any.
        /// </summary>
        public decimal AdditionalAmount
        {
            get { return additionalAmount; }
            set { additionalAmount = value; }
        }

        #endregion

        #region CurrencyCode

        /// <summary>
        /// Allows to get/set the transaction currency type.
        /// </summary>
        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }

        #endregion

        #endregion
    }

    #endregion
}
