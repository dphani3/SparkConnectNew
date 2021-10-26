
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: TransactionInformation.cs
  Description: This class is the data contract that encapsulates the transaction information.
  Date Created : 31-May-2011
  Revision History: 
  */

#endregion

#region Namespaces

using System.Runtime.Serialization;

#endregion

namespace TF.ReceiptManager.IRGen.DataContracts
{
    #region TransactionInformation

    /// <summary>
    /// This class is the data contract that encapsulates the transaction information.
    /// </summary>
    [DataContract]
    public class TransactionInformation
    {
        #region Member Variables

        private long chanakyaID;                    //Chanakya transaction id.
        private long companyID;                     //Company Id.
        private long merchantID;                    //Merchant Id.
        private long attendantID;                   //Attendant Id.
        private long userID;                        //User Id.

        private string merchantName;                //Merchant name.
        private string attendantName;               //Attendant name.

        private string isTestMode;                  //Mode of transaction (Test/Live).
        private string transactionID;               //Gateway transaction id.
        private string transactionType;             //Transaction type.
        private string transactionDate;             //Transaction date.
        private string timeZone;                    //Transaction server timezone.
        private string fCRRN;                       //FCRRN number.

        private string cardNumber;                  //Credit card number.
        private decimal amount;                     //Transaction amount.
        private decimal additionalAmount;           //Transaction tip amount if any.
        private decimal totalAmount;                //Transaction total amount (Amount + Additional Amount).
        private string currencyType;                //Transaction currency type.
        private string transactionStatus;           //Transaction status (APPROVED/FAILED).

        private string customerName;                //Customer name on card.
        private string emailAddress;                //Customer email address.
        private string merchantEmailAddress;        //Merchant email address.

        private string latitude;                    //Transaction locations latitude.
        private string longitude;                   //Transaction locations longitude.        

        private string supportPhone;                //Company support phone nymber.
        private string supportEmail;                //Company support email address.
        private string salesPhone;                  //Company sales phone numer.
        private string salesEmail;                  //Company sales email address.                   
        private string brandName;                   //Company brand name.
        private string notificationEmailAddress;    //Company notification email address.

        private string merchantContactPhone;         //Merchant conatct phone added by Nazreen.

        private string notes;                       //Added by Nazreen
        private decimal convenienceFee;             //Added by Nazreen
        private string invoiceNumber;
        private string orderNumber;
        #endregion

        #region Properties

        #region ChanakyaID

        /// <summary>
        /// Allows to get/set the Chanakya transaction id.
        /// </summary>
        [DataMember]
        public long ChanakyaID
        {
            get { return chanakyaID; }
            set { chanakyaID = value; }
        }

        #endregion

        #region CompanyID

        /// <summary>
        /// Allows to get/set the Company Id.
        /// </summary>
        [DataMember]
        public long CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        #endregion

        #region MerchantID

        /// <summary>
        /// Allows to get/set the Merchant Id.
        /// </summary>
        [DataMember]
        public long MerchantID
        {
            get { return merchantID; }
            set { merchantID = value; }
        }

        #endregion

        #region AttendantID

        /// <summary>
        /// Allows to get/set the Attendant Id.
        /// </summary>
        [DataMember]
        public long AttendantID
        {
            get { return attendantID; }
            set { attendantID = value; }
        }

        #endregion

        #region UserID

        /// <summary>
        /// Allows to get/set the User Id.
        /// </summary>
        [DataMember]
        public long UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        #endregion

        #region MerchantName

        /// <summary>
        /// Allows to get/set the merchants name.
        /// </summary>
        [DataMember]
        public string MerchantName
        {
            get { return merchantName; }
            set { merchantName = value; }
        }

        #endregion

        #region AttendantName

        /// <summary>
        /// Allows to get/set the attendants name.
        /// </summary>
        [DataMember]
        public string AttendantName
        {
            get { return attendantName; }
            set { attendantName = value; }
        }

        #endregion

        #region IsTestMode

        /// <summary>
        /// Allows to get/set the mode of transaction.
        /// </summary>
        [DataMember]
        public string IsTestMode
        {
            get { return isTestMode; }
            set { isTestMode = value; }
        }

        #endregion

        #region TransactionID

        /// <summary>
        /// Allows to get/set the gateway transaction id.
        /// </summary>
        [DataMember]
        public string TransactionID
        {
            get { return transactionID; }
            set { transactionID = value; }
        }

        #endregion

        #region TransactionType

        /// <summary>
        /// Allows to get/set the transaction type.
        /// </summary>
        [DataMember]
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
        [DataMember]
        public string TransactionDate
        {
            get { return transactionDate; }
            set { transactionDate = value; }
        }

        #endregion

        #region TimeZone

        /// <summary>
        /// Allows to get/set the transaction server timezone.
        /// </summary>
        [DataMember]
        public string TimeZone
        {
            get { return timeZone; }
            set { timeZone = value; }
        }

        #endregion

        #region FCRRN

        /// <summary>
        /// Allows to get/set the FCRRN number.
        /// </summary>
        [DataMember]
        public string FCRRN
        {
            get { return fCRRN; }
            set { fCRRN = value; }
        }

        #endregion

        #region CardNumber

        /// <summary>
        /// Allows to get/set the credit card number.
        /// </summary>
        [DataMember]
        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }

        #endregion

        #region Amount

        /// <summary>
        /// Allows to get/set the transaction amount.
        /// </summary>
        [DataMember]
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        #endregion

        #region AdditionalAmount

        /// <summary>
        /// Allows to get/set the transaction tip amount if any.
        /// </summary>
        [DataMember]
        public decimal AdditionalAmount
        {
            get { return additionalAmount; }
            set { additionalAmount = value; }
        }

        #endregion

        #region TotalAmount

        /// <summary>
        /// Allows to get/set the transaction total amount.
        /// </summary>
        [DataMember]
        public decimal TotalAmount
        {
            get { return totalAmount; }
            set { totalAmount = value; }
        }

        #endregion

        #region CurrencyType

        /// <summary>
        /// Allows to get/set the transaction currency type.
        /// </summary>
        [DataMember]
        public string CurrencyType
        {
            get { return currencyType; }
            set { currencyType = value; }
        }

        #endregion

        #region TransactionStatus

        /// <summary>
        /// Allows to get/set the transaction status.
        /// </summary>
        [DataMember]
        public string TransactionStatus
        {
            get { return transactionStatus; }
            set { transactionStatus = value; }
        }

        #endregion

        #region CustomerName

        /// <summary>
        /// Allows to get/set the customers name.
        /// </summary>
        [DataMember]
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        #endregion

        #region EmailAddress

        /// <summary>
        /// Allows to get/set the customers email address.
        /// </summary>
        [DataMember]
        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        #endregion

        #region MerchantEmailAddress

        /// <summary>
        /// Allows to get/set the merchants email address.
        /// </summary>
        [DataMember]
        public string MerchantEmailAddress
        {
            get { return merchantEmailAddress; }
            set { merchantEmailAddress = value; }
        }

        #endregion

        #region Latitude

        /// <summary>
        /// Allows to get/set the transaction locations latitude.
        /// </summary>
        [DataMember]
        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        #endregion

        #region Longitude

        /// <summary>
        /// Allows to get/set the transaction locations longitude.
        /// </summary>
        [DataMember]
        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        #endregion        

        #region SupportPhone

        /// <summary>
        /// Allows to get/set the company support phone nymber.
        /// </summary>
        [DataMember]
        public string SupportPhone
        {
            get { return supportPhone; }
            set { supportPhone = value; }
        }

        #endregion

        #region SupportEmail

        /// <summary>
        /// Allows to get/set the company support email address.
        /// </summary>
        [DataMember]
        public string SupportEmail
        {
            get { return supportEmail; }
            set { supportEmail = value; }
        }

        #endregion

        #region SalesPhone

        /// <summary>
        /// Allows to get/set the company sales phone numer.
        /// </summary>
        [DataMember]
        public string SalesPhone
        {
            get { return salesPhone; }
            set { salesPhone = value; }
        }

        #endregion

        #region SalesEmail

        /// <summary>
        /// Allows to get/set the company sales email address.  
        /// </summary>
        [DataMember]
        public string SalesEmail
        {
            get { return salesEmail; }
            set { salesEmail = value; }
        }

        #endregion

        #region BrandName

        /// <summary>
        /// Allows to get/set the company brand name.
        /// </summary>
        [DataMember]
        public string BrandName
        {
            get { return brandName; }
            set { brandName = value; }
        }

        #endregion

        #region NotificationEmailAddress

        /// <summary>
        /// Allows to get/set the company notification email address.
        /// </summary>
        [DataMember]
        public string NotificationEmailAddress
        {
            get { return notificationEmailAddress; }
            set { notificationEmailAddress = value; }
        }

        #endregion

        #region MerchantContactPhone
        //Added by Nazreen.
        /// <summary>
        /// Allows to get/set the Merchant contact phone.
        /// </summary>
        [DataMember]
        public string MerchantContactPhone
        {
            get { return merchantContactPhone; }
            set { merchantContactPhone = value; }
        }

        #endregion

        #region Notes
        //Added by Nazreen.
        /// <summary>
        /// Allows to get/set the Notes.
        /// </summary>
        [DataMember]
        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        #endregion

        #region ConvenienceFee
        //Added by Nazreen.
        /// <summary>
        /// Allows to get/set the Convenience fee.
        /// </summary>
        [DataMember]
        public decimal ConvenienceFee
        {
            get { return convenienceFee; }
            set { convenienceFee = value; }
        }

        #endregion

        #region InvoiceNumber
        //Added by Suhas.
        /// <summary>
        /// Allows to get/set the Invoice Number.
        /// </summary>
        [DataMember]
        public string InvoiceNumber
        {
            get { return invoiceNumber; }
            set { invoiceNumber = value; }
        }

        #endregion


        #region OrderNumber
        //Added by Suhas.
        /// <summary>
        /// Allows to get/set the Order Number .
        /// </summary>
        [DataMember]
        public string OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; }
        }

        #endregion


        #endregion
    }

    #endregion
}
