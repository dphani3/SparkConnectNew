
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: LoginResponse.cs
  Description: This class is the object representation for Login response data.
  Date Created : 20-Nov-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Xml.Serialization;

#endregion

namespace TF.REVCheckOut.Response
{
    #region LoginResponse

    /// <summary>
    /// This class is the object representation for Login response data.
    /// </summary>
    [Serializable]
    [XmlType]
    public class LoginResponse : Response
    {
        #region Member Variables

        //Login response code.
        private int code;

        //Login response description.
        private string description;

        //Attendant device id.
        private string deviceID;        

        //Logged-in session id.
        private string sessionID;

        //Attendant merchant id.
        private long merchantID;

        //Attendants merchant name.
        private string merchantName;

        //Attendant receipt header 1.
        private string receiptHeader1;

        //Attendant receipt header 2.
        private string receiptHeader2;

        //Attendant receipt header 3.
        private string receiptHeader3;

        //Attendant receipt footer 1.
        private string receiptFooter1;

        //Attendant receipt footer 2.
        private string receiptFooter2;

        //Attendant receipt footer 3.
        private string receiptFooter3;

        //Merchant logo image.
        private byte[] logo;

        //Next STAN number.
        private long nSTAN;

        //Next Batch ID.
        private long nBatchID;

        //Allowed transactions and their types.
        private TransactionsType[] allowedTransactions;

        //FocusConnect Version.
        private string version;

        #endregion

        #region Properties

        #region Code

        /// <summary>
        /// Allows get/set the login response code.
        /// </summary>
        public int Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

        #endregion

        #region Description

        /// <summary>
        /// Allows get/set the login response description.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        #endregion

        #region DeviceID

        /// <summary>
        /// Allows get/set the attendant device id.
        /// </summary>
        public string DeviceID
        {
            get
            {
                return this.deviceID;
            }
            set
            {
                this.deviceID = value;
            }
        }

        #endregion        

        #region SessionID

        /// <summary>
        /// Allows get/set the logged-in session-id.
        /// </summary>
        public string SessionID
        {
            get
            {
                return this.sessionID;
            }
            set
            {
                this.sessionID = value;
            }
        }

        #endregion

        #region MerchantID

        /// <summary>
        /// Allows get/set the attendant merchant id.
        /// </summary>        
        public long MerchantID
        {
            get
            {
                return this.merchantID;
            }
            set
            {
                this.merchantID = value;
            }
        }

        #endregion

        #region MerchantName

        /// <summary>
        /// Allows get/set the attendants merchant name.
        /// </summary>
        public string MerchantName
        {
            get
            {
                return this.merchantName;
            }
            set
            {
                this.merchantName = value;
            }
        }

        #endregion

        #region ReceiptHeader1

        /// <summary>
        /// Allows get/set the attendant receipt header 1.
        /// </summary>
        public string ReceiptHeader1
        {
            get
            {
                return this.receiptHeader1;
            }
            set
            {
                this.receiptHeader1 = value;
            }
        }

        #endregion

        #region ReceiptHeader2

        /// <summary>
        /// Allows get/set the attendant receipt header 2.
        /// </summary>
        public string ReceiptHeader2
        {
            get
            {
                return this.receiptHeader2;
            }
            set
            {
                this.receiptHeader2 = value;
            }
        }

        #endregion

        #region ReceiptHeader3

        /// <summary>
        /// Allows get/set the attendant receipt header 3.
        /// </summary>
        public string ReceiptHeader3
        {
            get
            {
                return this.receiptHeader3;
            }
            set
            {
                this.receiptHeader3 = value;
            }
        }

        #endregion

        #region ReceiptFooter1

        /// <summary>
        /// Allows get/set the attendant receipt footer 1.
        /// </summary>
        public string ReceiptFooter1
        {
            get
            {
                return this.receiptFooter1;
            }
            set
            {
                this.receiptFooter1= value;
            }
        }

        #endregion

        #region ReceiptFooter2

        /// <summary>
        /// Allows get/set the attendant receipt footer 2.
        /// </summary>
        public string ReceiptFooter2
        {
            get
            {
                return this.receiptFooter2;
            }
            set
            {
                this.receiptFooter2 = value;
            }
        }

        #endregion

        #region ReceiptFooter3

        /// <summary>
        /// Allows get/set the attendant receipt footer 3.
        /// </summary>
        public string ReceiptFooter3
        {
            get
            {
                return this.receiptFooter3;
            }
            set
            {
                this.receiptFooter3 = value;
            }
        }

        #endregion

        #region Logo

        /// <summary>
        /// Allows get/set the merchant logo image.
        /// </summary>
        [XmlElement(DataType = "base64Binary")]
        public byte[] Logo
        {
            get
            {
                return this.logo;
            }
            set
            {
                this.logo = value;
            }
        }

        #endregion

        #region NSTAN

        /// <summary>
        /// Allows get/set the next STAN number.
        /// </summary>
        public long NSTAN
        {
            get
            {
                return this.nSTAN;
            }
            set
            {
                this.nSTAN = value;
            }
        }

        #endregion

        #region NBatchID

        /// <summary>
        /// Allows get/set the next Batch ID.
        /// </summary>
        public long NBatchID
        {
            get
            {
                return this.nBatchID;
            }
            set
            {
                this.nBatchID = value;
            }
        }

        #endregion

        #region AllowedTransactions

        /// <summary>
        /// Allows get/set the supported transaction types.
        /// </summary>
        [XmlArrayItem(ElementName="Type")]
        public TransactionsType[] AllowedTransactions
        {
            get { return allowedTransactions; }
            set { allowedTransactions = value; }
        }

        #endregion

        #region Version

        /// <summary>
        /// Allows get/set the FocusConnect Version.
        /// </summary>        
        public string Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }

        #endregion

        #endregion
    }

    #endregion    

    #region TransactionsType

    /// <summary>
    /// This class holds the information regarding the gateway transaction types supported for the FocusConnect transaction types.
    /// </summary>
    [Serializable]
    [XmlType]
    public class TransactionsType
    {
        #region Member Variables

        //Transaction type id.
        private int id;

        //Supported gateway transactions.
        private string transactions;

        #endregion

        #region Properties

        #region ID

        /// <summary>
        /// Allows to get/set the transaction type id.
        /// </summary>
        [XmlAttribute]
        public int ID
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        #endregion

        #region Transactions

        /// <summary>
        /// Allows to get/set the supported gateway transactions.
        /// </summary>
        [XmlAttribute]
        public string Transactions
        {
            get
            {
                return this.transactions;
            }
            set
            {
                this.transactions = value;
            }
        }

        #endregion

        #endregion
    }

    #endregion
}
