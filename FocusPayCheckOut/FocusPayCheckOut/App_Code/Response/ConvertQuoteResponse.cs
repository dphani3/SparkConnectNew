#region Namespaces

using System;
using System.Xml.Serialization;

#endregion

namespace TF.FocusPayCheckOut.Response
{
    #region ConvertQuoteResponse

    [Serializable]
    [XmlType("Envelope")]
    public class ConvrtQuoteResponse
    {
        private ConvertQuoteBody objBody;

        #region ConvertQuoteBody

        [XmlElement(ElementName = "Body")]
        public ConvertQuoteBody Body
        {
            get { return objBody; }
            set { objBody = value; }
        }
        #endregion
    }

    [Serializable]
    [XmlType("Body")]
    public class ConvertQuoteBody
    {
        private ConvertQuoteResponse convertQuoteResponse;

        #region ConvertQuoteResponse

        [XmlElement(ElementName = "ConvertQuoteResponse")]
        public ConvertQuoteResponse ConvertQuoteResponse
        {
            get { return convertQuoteResponse; }
            set { convertQuoteResponse = value; }
        }
        #endregion
    }

    [Serializable]
    [XmlType]
    public class ConvertQuoteResponse
    {
        private ConvertQuoteResult convertQuoteResult;

        #region AuthorizePaymentCardResponse

        [XmlElement(ElementName = "ConvertQuoteResult")]
        public ConvertQuoteResult ConvertQuoteResult
        {
            get { return convertQuoteResult; }
            set { convertQuoteResult = value; }
        }
        #endregion
    }

    [Serializable]
    [XmlType]
    public class ConvertQuoteResult : Response
    {
        #region Member Variables

        ////code.
        //private string code;

        ////description.
        //private string description;

        //auth token
        private string authToken;

        //organization code.
        private string organizationCode;

        //returnCode
        private string returnCode;

        //returnMessage
        private string returnMessage;

        //token user name.
        private string tokenUserName;

        //user name
        private string userName;

        ////saved card customer
        //private string savedCardCustomer;
      
        //order freight total
        private string orderFreightTotal;

        //order number
        private string orderNumber;

        //order subtotal
        private string orderSubTotal;

        //order tax rate
        private string orderTaxRate;

        //order tax total
        private string orderTaxTotal;

        //order time
        private string orderTime;

        //order total
        private string orderTotal;

        //saved card customer
        private string savedCardCustomer;

        //saved card line
        private string savedCardLine;

        #endregion

        #region Properties

        //#region Code

        ///// <summary>
        ///// Allows get/set the Code.
        ///// </summary>
        //[XmlElement(ElementName = "VS_Result")]
        //public string Code
        //{
        //    get
        //    {
        //        return code;
        //    }
        //    set
        //    {
        //        this.code = value;
        //    }
        //}

        //#endregion

        //#region Description

        ///// <summary>
        ///// Allows get/set the Description.
        ///// </summary>
        //[XmlElement(ElementName = "VS_Message")]
        //public string Description
        //{
        //    get
        //    {
        //        return description;
        //    }
        //    set
        //    {
        //        this.description = value;
        //    }
        //}

        //#endregion

        #region AuthToken

        /// <summary>
        /// Allows get/set the AuthToken
        /// </summary>
        [XmlElement(ElementName = "AuthToken")]
        public string AuthToken
        {
            get
            {
                return authToken;
            }
            set
            {
                this.authToken = value;
            }
        }

        #endregion

        #region OrganizationCode

        /// <summary>
        /// Allows get/set the OrganizationCode.
        /// </summary>
        [XmlElement(ElementName = "OrganizationCode")]
        public string OrganizationCode
        {
            get
            {
                return organizationCode;
            }
            set
            {
                this.organizationCode = value;
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

        #region TokenUserName

        /// <summary>
        /// Allows get/set the TokenUserName.
        /// </summary>     
        [XmlElement(ElementName = "TokenUserName")]
        public string TokenUserName
        {
            get
            {
                return tokenUserName;
            }
            set
            {
                this.tokenUserName = value;
            }
        }

        #endregion

        #region UserName

        /// <summary>
        /// Allows get/set the UserName.
        /// </summary>
        [XmlElement(ElementName = "UserName")]
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                this.userName = value;
            }
        }

        #endregion

        #region OrderFreightTotal

        /// <summary>
        /// Allows get/set the OrderFreightTotal.
        /// </summary>
        [XmlElement(ElementName = "OrderFreightTotal")]
        public string OrderFreightTotal
        {
            get
            {
                return orderFreightTotal;
            }
            set
            {
                this.orderFreightTotal = value;
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

        #region OrderSubTotal

        /// <summary>
        /// Allows get/set the OrderSubTotal.
        /// </summary>
        [XmlElement(ElementName = "OrderSubTotal")]
        public string OrderSubTotal
        {
            get
            {
                return orderSubTotal;
            }
            set
            {
                this.orderSubTotal = value;
            }
        }

        #endregion

        #region OrderTaxRate

        /// <summary>
        /// Allows get/set the OrderTaxRate.
        /// </summary>
        [XmlElement(ElementName = "OrderTaxRate")]
        public string OrderTaxRate
        {
            get
            {
                return orderTaxRate;
            }
            set
            {
                this.orderTaxRate = value;
            }
        }

        #endregion

        #region OrderTaxTotal

        /// <summary>
        /// Allows get/set the OrderTaxTotal.
        /// </summary>
        [XmlElement(ElementName = "OrderTaxTotal")]
        public string OrderTaxTotal
        {
            get
            {
                return orderTaxTotal;
            }
            set
            {
                this.orderTaxTotal = value;
            }
        }

        #endregion

        #region OrderTime

        /// <summary>
        /// Allows get/set the OrderTime.
        /// </summary>
        [XmlElement(ElementName = "OrderTime")]
        public string OrderTime
        {
            get
            {
                return orderTime;
            }
            set
            {
                this.orderTime = value;
            }
        }

        #endregion

        #region OrderTotal

        /// <summary>
        /// Allows get/set the OrderTotal.
        /// </summary>
        [XmlElement(ElementName = "OrderTotal")]
        public string OrderTotal
        {
            get
            {
                return orderTotal;
            }
            set
            {
                this.orderTotal = value;
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