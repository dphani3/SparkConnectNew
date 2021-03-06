
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Consumer.cs
  Description: This class will store the information related to the consumer.
  Date Created : 20-Nov-2010
  Revision History: 
  */

#endregion

namespace TF.REVCheckOut
{
    #region Consumer

    /// <summary>
    /// This class will store the information related to the consumer.
    /// </summary>
    public class Consumer
    {
        #region Member Variables

        //FocusPay attendant username.
        private string attendantId;

        //FocusPay attendant password.
        private string password;

        //Consumer name.
        private string consumerName;

        //Consumer Cardnumber
        private string cardNumber;

        //Consumer address.
        private string consumerAddress;

        //Consumer e-mail address.
        private string emailAddress;

        //Transaction amount.
        private string amount;

        //Transaction currency type.
        private string currency;        

        //Identifier to decide the Live/Demo mode.
        private string isTestMode;    
   
        //Applications callback url to where the response should be redirected.
        private string callbackUrl;

        //Get the token using card number either Sale trans or Authonly
        private string tokenize;                      //Added by Nazreen, 

        //To process transactions using token
        private string token;                          //Added by Nazreen

        //transaction type 
        private int transactionType;

        //transactionID 
        private string transactionID;

        //Is Guest user
        private string isGuestUser;

        private string invoiceNumber;

        private string orderNumber;

        private string cardName;

        private string appKey;

        private string notes;

        private string uDField2;                      //user defined field

        private string uDField3;                      //user defined field

        #endregion

        #region Properties

        #region AttendantId

        /// <summary>
        /// Allows to get/set the FocusPay attendant username.
        /// </summary>
        public string AttendantId
        {
            get { return attendantId; }
            set { attendantId = value; }
        }

        #endregion

        #region Password

        /// <summary>
        /// Allows to get/set the FocusPay attendant password.
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        #endregion

        #region ConsumerName

        /// <summary>
        /// Allows to get/set the Consumer name.
        /// </summary>
        public string ConsumerName
        {
            get { return consumerName; }
            set { consumerName = value; }
        }

        #endregion

        #region Card Number

        /// <summary>
        /// Allows to get/set Card Number.
        /// </summary>
        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }

        #endregion

        #region ConsumerAddress

        /// <summary>
        /// Allows to get/set the Consumer address.
        /// </summary>
        public string ConsumerAddress
        {
            get { return consumerAddress; }
            set { consumerAddress = value; }
        }

        #endregion

        #region EmailAddress

        /// <summary>
        /// Allows to get/set the Consumer e-mail address.
        /// </summary>
        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        #endregion

        #region Amount

        /// <summary>
        /// Allows to get/set the Transaction amount.
        /// </summary>
        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        #endregion

        #region Currency

        /// <summary>
        /// Allows to get/set the Transaction currency type.
        /// </summary>
        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }

        #endregion

        #region IsTestMode

        /// <summary>
        /// Allows to get/set the Identifier that decides the Live/Demo mode.
        /// </summary>
        public string IsTestMode
        {
            get { return isTestMode; }
            set { isTestMode = value; }
        }

        #endregion

        #region CallbackUrl

        /// <summary>
        /// Allows to get/set the applications callback url to where the response should be redirected.
        /// </summary>
        public string CallbackUrl
        {
            get { return callbackUrl; }
            set { callbackUrl = value; }
        }

        #endregion

        #region TOKENIZE

        /// <summary>
        /// Allows to get/set the Tokenize.
        /// </summary>
        public string Tokenize
        {
            get { return tokenize; }
            set { tokenize = value; }
        }

        #endregion

        #region TOKEN

        /// <summary>
        /// Allows to get/set the Token.
        /// </summary>
        public string Token
        {
            get { return token; }
            set { token = value; }
        }

        #endregion

        #region TRANSACTIONTYPE

        /// <summary>
        /// Allows to get/set the TransactionType.
        /// </summary>
        public int TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }

        #endregion
        #region TRANSACTIONID

        /// <summary>
        /// Allows to get/set the TransactionID.
        /// </summary>
        public string TransactionID
        {
            get { return transactionID; }
            set { transactionID = value; }
        }

        #endregion

        #region Is Guest User

        /// <summary>
        /// Allows to get/set Guest User.
        /// </summary>
        public string IsGuestUser
        {
            get { return isGuestUser; }
            set { isGuestUser = value; }
        }

        #endregion

        #region Order Number

        /// <summary>
        /// Allows to get/set Order Number.
        /// </summary>
        public string OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; }
        }

        #endregion

        #region Invoice Number

        /// <summary>
        /// Allows to get/set Invoice Number.
        /// </summary>
        public string InvoiceNumber
        {
            get { return invoiceNumber; }
            set { invoiceNumber = value; }
        }

        #endregion


        #region Card Name

        /// <summary>
        /// Allows to get/set Card Name.
        /// </summary>
        public string CardName
        {
            get { return cardName; }
            set { cardName = value; }
        }

        #endregion


        #region App Key

        /// <summary>
        /// Allows to get/set App Key.
        /// </summary>
        public string AppKey
        {
            get { return appKey; }
            set { appKey = value; }
        }

        #endregion





        #region Notes

        /// <summary>
        /// Allows to get/set Notes
        /// </summary>
        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        #endregion

        #region UDField2

        /// <summary>
        /// Allows to get/set the UDField2.
        /// </summary>
        public string UDField2
        {
            get { return uDField2; }
            set { uDField2 = value; }
        }

        #endregion


        #region UDField3

        /// <summary>
        /// Allows to get/set the UDField3.
        /// </summary>
        public string UDField3
        {
            get { return uDField3; }
            set { uDField3 = value; }
        }

        #endregion

        #endregion
    }

    #endregion
}
