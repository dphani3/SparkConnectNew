
namespace TF.FocusPayCheckOut
{
    public class ConvertQuoteDetails
    {
        #region Consumer

        /// <summary>
        /// This class will store the information related to the consumer.
        /// </summary>
        /// 
        #region Member Variables

        //Authorize Token.
        //private string authToken;

        //Organization Code.
        private string organizationCode;

        //User Name.
        private string userName;

        //Consumer addressLine1.
        private string addressLine1;

        //Consumer addressLine2.
        private string addressLine2;

        //Consumer addressLine3.
        private string addressLine3;

        //Consumer addressLine4
        private string addressLine4;

        //Consumer city
        private string city;

        //Consumer country 
        private string country;

        //Consumer fax 
        private string fax;

        //Consumer name 
        private string name;

        //Consumer phone 
        private string phone;

        //Consumer postalCode 
        private string postalCode;

        //Consumer state 
        private string state;
     
        //Consumer carrierCode 
        private string carrierCode;

        //Consumer orderCompany 
        private string orderCompany;

        //Consumer PO 
        private string customerPO;

        //Quote Number
        private string quoteNumber;

        //Transaction amount.
        private string transactionAmount;

        //Transaction currency type.
        private string orderCurrency;

        //Applications callback url to where the response should be redirected.
        private string orderCallbackUrl;

        //Is Guest user
        private string isGuestUser;

        //saved card customer
        private string savedCardCustomer;

        //saved card line
        private string savedCardLine;

        //shippingInstructions1
        private string shippingInstructions1;

        //shippingInstructions2
        private string shippingInstructions2;

        #endregion

        #region Properties

        #region OrganizationCode

        /// <summary>
        /// Allows to get/set the organization code.
        /// </summary>
        public string OrganizationCode
        {
            get { return organizationCode; }
            set { organizationCode = value; }
        }

        #endregion

        #region UserName

        /// <summary>
        /// Allows to get/set the User Name.
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        #endregion

        #region AddressLine1

        /// <summary>
        /// Allows to get/set the Consumer addressLine1.
        /// </summary>
        public string AddressLine1
        {
            get { return addressLine1; }
            set { addressLine1 = value; }
        }

        #endregion

        #region AddressLine2

        /// <summary>
        /// Allows to get/set the Consumer addressLine2.
        /// </summary>
        public string AddressLine2
        {
            get { return addressLine2; }
            set { addressLine2 = value; }
        }

        #endregion

        #region AddressLine3

        /// <summary>
        /// Allows to get/set the Consumer addressLine3.
        /// </summary>
        public string AddressLine3
        {
            get { return addressLine3; }
            set { addressLine3 = value; }
        }

        #endregion

        #region AddressLine4

        /// <summary>
        /// Allows to get/set the Consumer addressLine4.
        /// </summary>
        public string AddressLine4
        {
            get { return addressLine4; }
            set { addressLine4 = value; }
        }

        #endregion

        #region City

        /// <summary>
        /// Allows to get/set the Consumer city.
        /// </summary>
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        #endregion

        #region Country

        /// <summary>
        /// Allows to get/set the Consumer country.
        /// </summary>
        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        #endregion

        #region Fax

        /// <summary>
        /// Allows to get/set the Consumer fax.
        /// </summary>
        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        #endregion

        #region Name

        /// <summary>
        /// Allows to get/set the Consumer name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion

        #region Phone

        /// <summary>
        /// Allows to get/set the Consumer phone.
        /// </summary>
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        #endregion

        #region Postal Code

        /// <summary>
        /// Allows to get/set the Consumer postalCode.
        /// </summary>
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        #endregion

        #region State

        /// <summary>
        /// Allows to get/set the Consumer state.
        /// </summary>
        public string State
        {
            get { return state; }
            set { state = value; }
        }

        #endregion

        #region Carrier Code

        /// <summary>
        /// Allows to get/set the Carrier Code.
        /// </summary>
        public string CarrierCode
        {
            get { return carrierCode; }
            set { carrierCode = value; }
        }

        #endregion

        #region Order Company

        /// <summary>
        /// Allows to get/set the Consumer order company.
        /// </summary>
        public string OrderCompany
        {
            get { return orderCompany; }
            set { orderCompany = value; }
        }

        #endregion

        #region Consumer PO

        /// <summary>
        /// Allows to get/set the Consumer PO.
        /// </summary>
        public string CustomerPO
        {
            get { return customerPO; }
            set { customerPO = value; }
        }

        #endregion

        #region Quote Number

        /// <summary>
        /// Allows to get/set the Quote Number.
        /// </summary>
        public string QuoteNumber
        {
            get { return quoteNumber; }
            set { quoteNumber = value; }
        }

        #endregion          

        #region TransactionAmount

        /// <summary>
        /// Allows to get/set the Transaction amount.
        /// </summary>
        public string TransactionAmount
        {
            get { return transactionAmount; }
            set { transactionAmount = value; }
        }

        #endregion

        #region OrderCurrency
        /// <summary>
        /// Allows to get/set the Transaction currency type.
        /// </summary>
        public string OrderCurrency
        {
            get { return orderCurrency; }
            set { orderCurrency = value; }
        }
        #endregion

        #region OrderCallbackUrl

        /// <summary>
        /// Allows to get/set the applications callback url to where the response should be redirected.
        /// </summary>
        public string OrderCallbackUrl
        {
            get { return orderCallbackUrl; }
            set { orderCallbackUrl = value; }
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

        #region Saved Card Customer

        /// <summary>
        /// Allows to get/set Saved Card Customer.
        /// </summary>
        public string SavedCardCustomer
        {
            get { return savedCardCustomer; }
            set { savedCardCustomer = value; }
        }

        #endregion

        #region Saved Card Line

        /// <summary>
        /// Allows to get/set Saved Card Line.
        /// </summary>
        public string SavedCardLine
        {
            get { return savedCardLine; }
            set { savedCardLine = value; }
        }

        #endregion

        #region Shipping Instructions1

        /// <summary>
        /// Allows to get/set ShippingI Instructions1.
        /// </summary>
        public string ShippingInstructions1
        {
            get { return shippingInstructions1; }
            set { shippingInstructions1 = value; }
        }

        #endregion

        #region Shipping Instructions2

        /// <summary>
        /// Allows to get/set Shipping Instructions2.
        /// </summary>
        public string ShippingInstructions2
        {
            get { return shippingInstructions2; }
            set { shippingInstructions2 = value; }
        }

        #endregion

        #endregion
    }
        #endregion
}