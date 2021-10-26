
#region Namespaces

using System;
using System.Xml.Serialization;

#endregion

namespace TF.REVCheckOut.Response
{
    #region AuthOrderResponse

    [Serializable]
    [XmlType("Envelope")]
    public class AuthTransactionResponse
    {
        private Body objBody;

        #region Body

        [XmlElement(ElementName = "Body")]
        public Body Body
        {
            get { return objBody; }
            set { objBody = value; }
        }
        #endregion
    }

    [Serializable]
    [XmlType("Body")]
    public class Body
    {
        private AuthorizePaymentCardResponse authPaymenytCardResponse;

        #region AuthorizePaymentCardResponse
       
        [XmlElement(ElementName = "AuthorizePaymentCardResponse")]
        public AuthorizePaymentCardResponse AuthorizePaymentCardResponse
        {
            get { return authPaymenytCardResponse; }
            set { authPaymenytCardResponse = value; }
        }
        #endregion
    }

    [Serializable]
    [XmlType]
    public class AuthorizePaymentCardResponse
    {
        private AuthorizePaymentCardResult authorizePaymentCardResult;

        #region AuthorizePaymentCardResponse

        [XmlElement(ElementName = "AuthorizePaymentCardResult")]
        public AuthorizePaymentCardResult AuthorizePaymentCardResult
        {
            get { return authorizePaymentCardResult; }
            set { authorizePaymentCardResult = value; }
        }
        #endregion


    }
    
    [Serializable]
    [XmlType]
    public class AuthorizePaymentCardResult : Response
    {
        #region Member Variables

        //code.
        private string code;

        //description.
        private string description;

        //authCode
        private string authCode;

        //orderNumber.
        private string orderNumber;

        //pNRef.
        private string pNRef;

        //transactionAmount
        private string transactionAmount;

        //returnCode
        private string returnCode;

        //returnMessage
        private string returnMessage;

        //saved card customer
        private string savedCardCustomer;

        //saved card line
        private string savedCardLine;

        #endregion

        #region Properties

        #region Code

        /// <summary>
        /// Allows get/set the Code.
        /// </summary>
        [XmlElement(ElementName = "VS_Result")]
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                this.code = value;
            }
        }

        #endregion

        #region Description

        /// <summary>
        /// Allows get/set the Description.
        /// </summary>
        [XmlElement(ElementName = "VS_Message")]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                this.description = value;
            }
        }

        #endregion

        #region AuthCode

        /// <summary>
        /// Allows get/set the AuthCode
        /// </summary>
        [XmlElement(ElementName = "AuthCode")]
        public string AuthCode
        {
            get
            {
                return authCode;
            }
            set
            {
                this.authCode = value;
            }
        }

        #endregion

        #region OrderNumber

        /// <summary>
        /// Allows get/set the OrderNumber.
        /// </summary>
        [XmlElement(ElementName = "OrderNumber")]
        public string OrderNumber
        {
            get
            {
                return orderNumber;
            }
            set
            {
                this.orderNumber = value;
            }
        }

        #endregion

        #region PNRef

        /// <summary>
        /// Allows get/set the PNRef.
        /// </summary>     
        [XmlElement(ElementName = "PNRef")]
        public string PNRef
        {
            get
            {
                return pNRef;
            }
            set
            {
                this.pNRef = value;
            }
        }

        #endregion

        #region TransactionAmount

        /// <summary>
        /// Allows get/set the TransactionAmount.
        /// </summary>
        [XmlElement(ElementName = "TransactionAmount")]
        public string TransactionAmount
        {
            get
            {
                return transactionAmount;
            }
            set
            {
                this.transactionAmount = value;
            }
        }

        #endregion

        #region ReturnCode

        /// <summary>
        /// Allows get/set the ReturnCode.
        /// </summary>
        [XmlElement(ElementName = "ReturnCode")]
        public string ReturnCode
        {
            get
            {
                return returnCode;
            }
            set
            {
                this.returnCode = value;
            }
        }

        #endregion

        #region ReturnMessage

        /// <summary>
        /// Allows get/set the ReturnMessage.
        /// </summary>
        [XmlElement(ElementName = "ReturnMessage")]
        public string ReturnMessage
        {
            get
            {
                return returnMessage;
            }
            set
            {
                this.returnMessage = value;
            }
        }

        #endregion

        #region SavedCardCustomer

        /// <summary>
        /// Allows get/set the SavedCardCustomer.
        /// </summary>
        [XmlElement(ElementName = "SavedCardCustomer")]
        public string SavedCardCustomer
        {
            get
            {
                return savedCardCustomer;
            }
            set
            {
                this.savedCardCustomer = value;
            }
        }

        #endregion

        #region SavedCardLine

        /// <summary>
        /// Allows get/set the SavedCardLine.
        /// </summary>
        [XmlElement(ElementName = "SavedCardLine")]
        public string SavedCardLine
        {
            get
            {
                return savedCardLine;
            }
            set
            {
                this.savedCardLine = value;
            }
        }

        #endregion

        #endregion
    }

    #endregion      
}
