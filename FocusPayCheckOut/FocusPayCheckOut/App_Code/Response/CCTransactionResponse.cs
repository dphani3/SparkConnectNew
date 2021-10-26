
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: CCTransactionResponse.cs
  Description: This class is the object representation for CCTransaction response data.
  Date Created : 20-Nov-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Xml.Serialization;

#endregion

namespace TF.FocusPayCheckOut.Response
{
    #region CCTransactionResponse

    /// <summary>
    /// This class is the object representation for CCTransaction response data.
    /// </summary>
    [Serializable]
    [XmlRoot]
    public class CCTransactionResponse : Response
    {
        #region Member Variables

        //CCTransaction response code.
        private int code;

        //CCTransaction response description.
        private string description;

        //Attendant device id.
        private string deviceID;

        //Attendant Name.
        private string attendant;

        //Transaction credit card number.
        private string creditCardNumber;

        //Transaction amount.
        private string amount;

        //Transaction date and time.
        private string txnDtTime;

        //Transaction Gateway ID.
        private string gatewayTransactionID;

        //Invoice Number.
        private string fCRRN;

        //Next STAN number.
        private long nSTAN;

        //Next Batch ID.
        private long nBatchID;

        //FocusConnect Version.
        private string version;

        //To process Transactions using Token
        private string token;

        #endregion

        #region Member Variables

        #region Code

        /// <summary>
        /// Allows get/set the CCTransaction response code.
        /// </summary>
        public int Code
        {
            get { return code; }
            set { code = value; }
        }

        #endregion

        #region Description

        /// <summary>
        /// Allows get/set the CCTransaction response description.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        #endregion

        #region DeviceID

        /// <summary>
        /// Allows get/set the attendants device id.
        /// </summary>
        public string DeviceID
        {
            get { return deviceID; }
            set { deviceID = value; }
        }

        #endregion

        #region Attendant

        /// <summary>
        /// Allows get/set the attendants name.
        /// </summary>
        public string Attendant
        {
            get { return attendant; }
            set { attendant = value; }
        }

        #endregion

        #region CreditCardNumber

        /// <summary>
        /// Allows get/set the transaction credit card number.
        /// </summary>
        public string CreditCardNumber
        {
            get { return creditCardNumber; }
            set { creditCardNumber = value; }
        }

        #endregion

        #region Amount

        /// <summary>
        /// Allows get/set the transaction amount.
        /// </summary>
        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        #endregion

        #region TxnDtTime

        /// <summary>
        /// Allows get/set the transaction date and time.
        /// </summary>
        public string TxnDtTime
        {
            get { return txnDtTime; }
            set { txnDtTime = value; }
        }

        #endregion

        #region GatewayTransactionID

        /// <summary>
        /// Allows get/set the transaction Gateway ID.
        /// </summary>
        public string GatewayTransactionID
        {
            get { return gatewayTransactionID; }
            set { gatewayTransactionID = value; }
        }

        #endregion

        #region FCRRN

        /// <summary>
        /// Allows get/set the invoice number.
        /// </summary>
        public string FCRRN
        {
            get { return fCRRN; }
            set { fCRRN = value; }
        }

        #endregion

        #region NSTAN

        /// <summary>
        /// Allows get/set the next STAN number.
        /// </summary>
        public long NSTAN
        {
            get { return nSTAN; }
            set { nSTAN = value; }
        }

        #endregion

        #region NBatchID

        /// <summary>
        /// Allows get/set the next Batch ID.
        /// </summary>
        public long NBatchID
        {
            get { return nBatchID; }
            set { nBatchID = value; }
        }

        #endregion

        #region Version

        /// <summary>
        /// Allows get/set the FocusConnect Version.
        /// </summary>
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        #endregion

        #region Token

        /// <summary>
        /// Allows get/set the Token.
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
