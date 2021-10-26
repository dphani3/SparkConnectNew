
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: CCTransactionRequest.cs
  Description: This class will process the FocusPay Transaction method named CCTransaction.
  Date Created : 20-Nov-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Xml.Serialization;

#endregion

namespace TF.FocusPayCheckOut.Request
{
    #region CCTransactionRequest

    /// <summary>
    /// This class will process the FocusPay Transaction method named CCTransaction.
    /// </summary>
    [Serializable]
    [XmlRoot]
    public class CCTransactionRequest : Request
    {
        #region Member Variables

        //CCTransaction request type.
        private int requestType;

        //Attendant device id.
        private string deviceID;

        //User session id.
        private string sessionID;

        //Attendant user name.
        private string userName;

        //Attendant password.
        private string password;

        //Identifier to decide the Live/Demo mode.
        private string testMode;

        //Credit Card Transaction type.
        private int transactionType;

        //Name on Credit Card.
        private string name;

        //Credit card number.
        private string creditCardNumber;

        //Credit card CVV number.
        private string cVV;

        //Credit card expiry date.
        private string expiryDate;

        //Transaction Amount.
        private string amount;

        //Card holder address.
        private string address;

        //Card holder zip address.
        private string zip;

        //Card holder email address.
        private string emailAddress;

        //Flag to identify whether to perform the transactions in Manual/Swipe mode.
        private int cardPresentIndicator;

        //Credit card track 1 data.
        private string tdata1;

        //Credit card track 2 data.
        private string tdata2;

        //Gateway transaction id.
        private string gatewayTransactionID;

        //Notes if any.
        private string notes;

        //Current STAN number.
        private string sTAN;

        //Current Batch ID.
        private string batchID;

        //Tip Amount if any.
        private string tipAmount;

        //Get the token using card number either Sale trans or Authonly
        private string tokenize;                      //Added by Nazreen, 

        //To process transactions using token
        private string token;                          //Added by Nazreen

        #endregion

        #region Properties

        #region RequestType

        /// <summary>
        /// Allows to get/set the CCTransaction request type.
        /// </summary>
        public int RequestType
        {
            get { return requestType; }
            set { requestType = value; }
        }

        #endregion

        #region DeviceID

        /// <summary>
        /// Allows to get/set the attendant device id.
        /// </summary>
        public string DeviceID
        {
            get { return deviceID; }
            set { deviceID = value; }
        }

        #endregion

        #region SessionID

        /// <summary>
        /// Allows to get/set the users session id.
        /// </summary>
        public string SessionID
        {
            get { return sessionID; }
            set { sessionID = value; }
        }

        #endregion

        #region UserName

        /// <summary>
        /// Allows to get/set the attendant user name.
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        #endregion

        #region Password

        /// <summary>
        /// Allows to get/set the attendant password.
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        #endregion

        #region TestMode

        /// <summary>
        /// Allows to get/set the identifier that decides the Live/Demo mode.
        /// </summary>
        public string TestMode
        {
            get { return testMode; }
            set { testMode = value; }
        }

        #endregion

        #region TransactionType

        /// <summary>
        /// Allows to get/set the Credit Card Transaction type.
        /// </summary>
        public int TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }

        #endregion

        #region Name

        /// <summary>
        /// Allows to get/set the Name on Credit Card.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        #region CreditCardNumber

        /// <summary>
        /// Allows to get/set the Credit card number.
        /// </summary>
        public string CreditCardNumber
        {
            get { return creditCardNumber; }
            set { creditCardNumber = value; }
        }

        #endregion

        #region CVV

        /// <summary>
        /// Allows to get/set the Credit card CVV number.
        /// </summary>
        public string CVV
        {
            get { return cVV; }
            set { cVV = value; }
        }

        #endregion

        #region ExpiryDate

        /// <summary>
        /// Allows to get/set the Credit card expiry date.
        /// </summary>
        public string ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; }
        }

        #endregion

        #region Amount

        /// <summary>
        /// Allows to get/set the transaction Amount.
        /// </summary>
        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        #endregion

        #region Address

        /// <summary>
        /// Allows to get/set the card holders address.
        /// </summary>
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        #endregion

        #region Zip

        /// <summary>
        /// Allows to get/set the card holder zip address.
        /// </summary>
        public string Zip
        {
            get { return zip; }
            set { zip = value; }
        }

        #endregion

        #region EmailAddress

        /// <summary>
        /// Allows to get/set the card holders email address.
        /// </summary>
        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        #endregion

        #region CardPresentIndicator

        /// <summary>
        /// Allows to get/set the flag that identifies whether to perform the transactions in Manual/Swipe mode.
        /// <list type="bullet">
        ///     <item>
        ///         <description>0 -> Card Not Present(MANUAL) Mode.</description>
        ///     </item>
        ///     <item>
        ///         <description>1 -> Card Present(SWIPE) Mode.</description>
        ///     </item>      
        /// </list>
        /// </summary>
        public int CardPresentIndicator
        {
            get { return cardPresentIndicator; }
            set { cardPresentIndicator = value; }
        }

        #endregion

        #region Tdata1

        /// <summary>
        /// Allows to get/set the credit card track 1 data. Send the track 1 data when the CardPresentIndicator is 1 and Track 2 is 
        /// not available.
        /// </summary>
        public string Tdata1
        {
            get { return tdata1; }
            set { tdata1 = value; }
        }

        #endregion

        #region Tdata2

        /// <summary>
        /// Allows to get/set the credit card track 2 data. Send the track 2 data when the CardPresentIndicator is 1.
        /// </summary>
        public string Tdata2
        {
            get { return tdata2; }
            set { tdata2 = value; }
        }

        #endregion

        #region GatewayTransactionID

        /// <summary>
        /// Allows to get/set the gateway transaction id.
        /// </summary>
        public string GatewayTransactionID
        {
            get { return gatewayTransactionID; }
            set { gatewayTransactionID = value; }
        }

        #endregion

        #region Notes

        /// <summary>
        /// Allows to get/set the notes.
        /// </summary>
        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        #endregion

        #region STAN

        /// <summary>
        /// Allows to get/set the current STAN number.
        /// </summary>
        public string STAN
        {
            get { return sTAN; }
            set { sTAN = value; }
        }

        #endregion

        #region BatchID

        /// <summary>
        /// Allows to get/set the current Batch ID.
        /// </summary>
        public string BatchID
        {
            get { return batchID; }
            set { batchID = value; }
        }

        #endregion

        #region TipAmount

        /// <summary>
        /// Allows to get/set the tip Amount.
        /// </summary>
        public string TipAmount
        {
            get { return tipAmount; }
            set { tipAmount = value; }
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

        #endregion
    }

    #endregion
}
