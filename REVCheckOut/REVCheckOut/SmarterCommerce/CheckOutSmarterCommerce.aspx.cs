
#region Namespaces

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using TF.REVCheckOut;
using TF.REVCheckOut.Response;
using System.Text;

#endregion

public partial class CheckOutSmarterCommerce : System.Web.UI.Page, IDisposable
{
    #region Member Variables
    
    //This is for Garbage Collector.
    private bool isDisposed = false;

    //HTTP Post data fields.
    private SortedList<string, string> requestFields = null;

    //Consumer information.
    private ConsumerOrderDetails consumerOrderInfo = null;

    static string cardType = string.Empty;

    //Unmask count for credit card masking.
    private const int UNMASK_COUNT = 4;

    NameValueCollection checkOutPostData = null;

    XmlDocument xmlDoc = null;
    string XMLData = string.Empty;

    string errorMessage = string.Empty;

    bool httpresponsetimeout = false;

    #endregion

    #region Constructor

    /// <summary>
    /// This is the Constructor for the Request class.
    /// </summary>
    public CheckOutSmarterCommerce()
    {

    }

    #endregion

    #region Destructor

    /// <summary>
    /// This is the Destructor for the class.
    /// </summary>
    ~CheckOutSmarterCommerce ()
    {
        //Since finalizer is called, there is no need to free the managed resources. So, false is passed.
        Dispose(false);
    }

    #endregion

    #region Page_Load

    /// <summary>
    /// This will parses the consumer order information the post data and stores the same in session for further use.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">EventArgs.</param>
    protected void Page_Load (object sender, EventArgs e)
    {
        try
        {

            hdnFlag.Value = "true";

            bool isValid = false;
            bool isValidCurrency = false;
            string currencySymbol = string.Empty;

            if (!Page.IsPostBack)
            {
                //Check whether the request has post data or not.
                if (Request.ContentLength > 0)
                {
                    Common.LogWrite("Request has data.");

                    //Read the post data.
                    using (StreamReader requestReader = new StreamReader(Request.InputStream))
                    {
                        string RequestString = requestReader.ReadToEnd().ToString();
                        string[] requestDataCollection = RequestString.Split('&');
                        Common.LogWrite(DateTime.Now.ToString() + RequestString);
                        Common.LogWrite("Reading Namevalue collection data.");

                        if (requestDataCollection != null && requestDataCollection.Length > 0)
                        {
                            //Fill the request fileds into collection.
                            requestFields = new SortedList<string, string>();

                            foreach (string requestData in requestDataCollection)
                            {
                                string[] requestItem = requestData.Split('=');

                                if (requestItem != null && requestItem.Count() == 2)
                                {
                                    requestFields.Add(requestItem[0], HttpUtility.UrlDecode(requestItem[1]));
                                }
                            }

                            Common.LogWrite("Retriving the parameter value from the namevalue collection and assigning respective consumer class properties.");
                            //Construct the consumer order Details object.
                            consumerOrderInfo = new ConsumerOrderDetails
                            {
                                OrganizationCode = GetRequestField(Common.ORGANIZATION_CODE, ""),
                                UserName = GetRequestField(Common.USER_NAME, ""),
                                AddressLine1 = GetRequestField(Common.ADDRESS_LINE1, ""),
                                AddressLine2 = GetRequestField(Common.ADDRESS_LINE2, ""),
                                AddressLine3 = GetRequestField(Common.ADDRESS_LINE3, ""),
                                AddressLine4 = GetRequestField(Common.ADDRESS_LINE4, ""),
                                City = GetRequestField(Common.CITY, ""),
                                Country = GetRequestField(Common.COUNTRY, ""),
                                Fax = GetRequestField(Common.FAX, ""),
                                Name = GetRequestField(Common.NAME, ""),
                                Phone = GetRequestField(Common.PHONE, ""),
                                PostalCode = GetRequestField(Common.POSTAL_CODE, ""),
                                State = GetRequestField(Common.STATE, ""),
                                OrderCompany = GetRequestField(Common.ORDER_COMPANY, ""),
                                OrderNumber = GetRequestField(Common.ORDER_NUMBER, ""),
                                OrderType = GetRequestField(Common.ORDER_TYPE, ""),
                                TransactionAmount = GetRequestField(Common.AMOUNT, ""),
                                OrderCurrency = GetRequestField(Common.CURRENCY, ""),
                                IsGuestUser = GetRequestField(Common.ORDER_IS_GUEST_USER, ""),
                                OrderCallbackUrl = GetRequestField(Common.CALLBACK, ""),
                            };

                            //Check the Organization Code.
                            if (!String.IsNullOrEmpty(consumerOrderInfo.OrganizationCode))
                            {
                                Common.LogWrite("Got the Organization Code.");

                            }
                            //Else
                            else
                            {
                                Common.LogWrite("Organization Code is null.");

                                isValid = true;
                            }

                            //Check the User Name.
                            if (!String.IsNullOrEmpty(consumerOrderInfo.UserName))
                            {
                                Common.LogWrite("Got the User Name.");

                            }
                            //Else
                            else
                            {
                                Common.LogWrite("User Name is null.");

                                isValid = true;
                            }

                            //Check the AddressLine1.
                            if (!String.IsNullOrEmpty(consumerOrderInfo.AddressLine1))
                            {
                                Common.LogWrite("Got the AddressLine1.");

                            }
                            //Else
                            else
                            {
                                Common.LogWrite("AddressLine1 is null.");

                                isValid = true;
                            }

                            //Assign the consumer name to the checkout page.
                            if (!String.IsNullOrEmpty(consumerOrderInfo.Name))
                            {
                                Common.LogWrite("Got the consumer name.");

                                lblWelcomeText.Text = string.Format("{0}", consumerOrderInfo.Name);
                            }
                            //Else
                            else
                            {
                                Common.LogWrite("Consumer name is null.");

                                isValid = true;

                            }

                            //Check the Postal Code.
                            if (!String.IsNullOrEmpty(consumerOrderInfo.PostalCode))
                            {
                                Common.LogWrite("Got the Postal Code.");
                            }
                            //Else
                            else
                            {
                                Common.LogWrite("Postal Code is null.");

                                isValid = true;
                            }

                            //Check the Order Company.
                            if (!String.IsNullOrEmpty(consumerOrderInfo.OrderCompany))
                            {
                                Common.LogWrite("Got the Order Company.");
                            }
                            //Else
                            else
                            {
                                Common.LogWrite("Order Company is null.");

                                isValid = true;
                            }

                            //Assign the Order Number value to the checkout page.
                            if (!String.IsNullOrEmpty(consumerOrderInfo.OrderNumber))
                            {
                                Common.LogWrite("Got the Order Number.");

                                lblOrderNumber.Text = consumerOrderInfo.OrderNumber;
                            }
                            //Else
                            else
                            {
                                Common.LogWrite("Order Number is null.");

                                isValid = true;
                            }

                            ////Check the Order Type.
                            //if(!String.IsNullOrEmpty(consumerOrderInformation.OrderType))
                            //{
                            //    Common.LogWrite("Got the Order Type.");
                            //}
                            ////Else
                            //else
                            //{
                            //    Common.LogWrite("Order Type is null.");

                            //    isValid = true;
                            //}

                            //Assign the Transaction Amount value to the checkout page.
                            if (!String.IsNullOrEmpty(consumerOrderInfo.TransactionAmount))
                            {
                                decimal amount = Common.GetDecimalMoneyValue(consumerOrderInfo.TransactionAmount);

                                string transcationAmount = string.Empty;

                                if (!amount.ToString().Contains('.'))
                                    transcationAmount = string.Format("{0}.00", amount.ToString());
                                else
                                    transcationAmount = amount.ToString();

                                Common.LogWrite("Got the Transaction Amount.");

                                lblAmountValue.Text = transcationAmount;
                            }
                            //Else
                            else
                            {
                                Common.LogWrite("Transaction Amount is null.");

                                isValid = true;
                            }

                            //Assign the currency value to the checkout page.
                            if (!String.IsNullOrEmpty(consumerOrderInfo.OrderCurrency))
                            {
                                bool isValidCurr = Common.Validate_Currency(consumerOrderInfo.OrderCurrency, ref currencySymbol);

                                if (isValidCurr)
                                {
                                    Common.LogWrite("Got the Currency Type.");

                                    lblCurrencyValue.Text = currencySymbol;
                                }
                                else
                                {
                                    Common.LogWrite("Invalid Currency Type.");

                                    isValidCurrency = true;
                                }
                            }
                            //Else
                            else
                            {
                                Common.LogWrite("Currency type is null.");

                                isValid = true;
                            }

                            //If the user is registered user then enable SavedSave Card Number check box
                            if (consumerOrderInfo.IsGuestUser == "0")
                            {
                                Common.LogWrite("Not a Guest user.");

                                lblCheckBox.Visible = true;
                                chkRememberMe.Checked = false;
                            }
                            //else the user is guest user the disalbe the Save Card Number check box
                            else
                            {
                                Common.LogWrite("Guest user.");

                                lblCheckBox.Visible = false;
                                chkRememberMe.Checked = false;
                            }

                            //Store the consumer order information into the session.
                            Session["ConsumerOrderInfo"] = consumerOrderInfo;

                            if (isValidCurrency)
                            {
                                Common.LogWrite("Invalid Currency Type.");

                                checkOutPostData = Common.ConstructCheckOutResponse(Common.INVALID_CURRENCY_CODE_ERROR_CODE, Common.INVALID_CURRENCY_CODE_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                                string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                Session["SmartCommOrderResponse"] = checkOutPostData;
                                Response.Redirect("ProxySmarterCommerce.aspx", false);
                            }
                            else if (isValid)
                            {
                                Common.LogWrite("Insufficient Parameters.");

                                checkOutPostData = Common.ConstructCheckOutResponse(Common.INSUFFICIENT_PARAMETERS_ERROR_CODE, Common.INSUFFICIENT_PARAMETERS_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                Session["SmartCommOrderResponse"] = checkOutPostData;
                                Response.Redirect("ProxySmarterCommerce.aspx", false);
                            }
                        }
                        //Else, redirect to error page.
                        else
                        {
                            Common.LogWrite("Request data is null.");

                            checkOutPostData = Common.ConstructCheckOutResponse(Common.INSUFFICIENT_PARAMETERS_ERROR_CODE, Common.INSUFFICIENT_PARAMETERS_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                                     string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                     string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                            string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                            Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                            Session["SmartCommOrderResponse"] = checkOutPostData;
                            Response.Redirect("ProxySmarterCommerce.aspx", false);
                        }
                    }
                }
                //Else, redirect to error page.
                else
                {
                    Common.LogWrite("Request data is null.");

                    checkOutPostData = Common.ConstructCheckOutResponse(Common.INSUFFICIENT_PARAMETERS_ERROR_CODE, Common.INVALID_REQUEST, string.Empty, string.Empty,
                                                                                     string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                     string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                    Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                    Session["SmartCommOrderResponse"] = checkOutPostData;
                    Response.Redirect("ProxySmarterCommerce.aspx", false);
                }
            }
            else
            {
                txtExpiryMonth.Items.Clear();
                txtExpiryYear.Items.Clear();

                loadDropDownList();
                txtExpiryMonth.SelectedIndex = Convert.ToInt32(hdnMonth.Value) - 1;
                txtExpiryYear.SelectedIndex = (Convert.ToInt32(hdnYear.Value)) - (DateTime.Now.Year);
            }

        }
        //Redirect to error page.
        catch(Exception ex)
        {
            Common.LogWrite("Exception: " + ex.Message);

            checkOutPostData = Common.ConstructCheckOutResponse(Common.INSUFFICIENT_PARAMETERS_ERROR_CODE, Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                                      string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                      string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
            Common.LogWrite("Response Name Value Collection \r\n" + allValues);
            Session["SmartCommOrderResponse"] = checkOutPostData;
            Response.Redirect("ProxySmarterCommerce.aspx", false);
        }
    }
    #endregion

    #region btnPay_Click

    /// <summary>
    /// Process the current transaction information by sending the transaction details to FocusConnect.
    /// </summary>
    /// <param name="sender">Page/Control that initiates the event.</param>
    /// <param name="e">Event Arguments.</param>
    protected void btnPay_Click (object sender, EventArgs e)
    {
        Mutex m = new Mutex();

        m.WaitOne();

        try
        {
            Common.LogWrite("Pay button event.");

            //Check whether the all controls on the page have been validated or not.
            if(Page.IsValid)
            {
                Common.LogWrite("Page validation done.");

                //Validate the captcha information aslo.
                captNoBot.ValidateCaptcha(txtCaptcha.Value.Trim());

                lblExpiryMonthYear.Text = string.Empty;
                lblCaptcha.Text = string.Empty;
                lblCardNoError.Text = string.Empty;

                //If Catptcha is validated.
                if(captNoBot.UserValidated)
                {
                    Common.LogWrite("Valid captcha.");

                    int expiryMonth = Convert.ToInt32(hdnMonth.Value);
                    int expiryYear = Convert.ToInt32(hdnYear.Value);

                    int currentMonth = DateTime.Now.Month;
                    string currentYear = DateTime.Now.Year.ToString();

                    //Check card expity year as less than current year.
                    if (expiryYear < Convert.ToInt32(currentYear))
                    {
                        lblExpiryMonthYear.Text = "Invalid Card Expiry";
                        txtCaptcha.Value = string.Empty;
                        hdnFlag.Value = "true";

                        Common.LogWrite("Invalid card expiry year.");

                    }
                    //check card expiry year is greater than current year
                    else if (expiryYear > Convert.ToInt32(currentYear))
                    {
                        //Check card expiry month is more than 12
                        if (expiryMonth < 1 || expiryMonth > 12)
                        {
                            lblExpiryMonthYear.Text = "Invalid Card Expiry";
                            txtCaptcha.Value = string.Empty;
                            hdnFlag.Value = "true";

                            Common.LogWrite("Invalid card expiry month.");

                        }
                    }
                    //Check card expiry year is current year
                    else if (expiryYear == Convert.ToInt32(currentYear))
                    {
                        //Check card expiry month is less than current month
                        if (expiryMonth < currentMonth)
                        {
                            lblExpiryMonthYear.Text = "Invalid Card Expiry";
                            txtCaptcha.Value = string.Empty;
                            hdnFlag.Value = "true";

                            Common.LogWrite("Invalid card expiry month.");

                        }
                        //Check card expiry month is more than 12
                        else if (expiryMonth < 1 || expiryMonth > 12)
                        {
                            lblExpiryMonthYear.Text = "Invalid Card Expiry";
                            txtCaptcha.Value = string.Empty;
                            hdnFlag.Value = "true";

                            Common.LogWrite("Invalid card expiry month.");

                        }
                    }
                                            
                    //Check whether the session related to consumer order information exists or not.
                    if(Session["ConsumerOrderInfo"] != null)
                    {
                        Common.LogWrite("Check consumer order details Session data.");

                        hdnFlag.Value = "";

                        //Extract the consumer order information from the session.
                        consumerOrderInfo = Session["ConsumerOrderInfo"] as ConsumerOrderDetails;

                        //if(consumerOrderInformation.OrganizationCode != string.Empty && consumerOrderInformation.UserName != string.Empty &&
                        //   consumerOrderInformation.AddressLine1 != string.Empty && consumerOrderInformation.City != string.Empty &&
                        //   consumerOrderInformation.Country != string.Empty && consumerOrderInformation.Name != string.Empty &&
                        //   consumerOrderInformation.PostalCode != string.Empty && consumerOrderInformation.State != string.Empty && consumerOrderInformation.OrderCompany != string.Empty &&
                        //   consumerOrderInformation.OrderNumber != string.Empty && consumerOrderInformation.OrderType != string.Empty &&
                        //   consumerOrderInformation.TransactionAmount != string.Empty && consumerOrderInformation.OrderCurrency != string.Empty)
                        //{
                        if(consumerOrderInfo.OrganizationCode != string.Empty && consumerOrderInfo.UserName != string.Empty &&
                          consumerOrderInfo.AddressLine1 != string.Empty && consumerOrderInfo.Name != string.Empty &&
                          consumerOrderInfo.PostalCode != string.Empty && consumerOrderInfo.OrderCompany != string.Empty &&
                          consumerOrderInfo.OrderNumber != string.Empty && consumerOrderInfo.TransactionAmount != string.Empty && 
                          consumerOrderInfo.OrderCurrency != string.Empty)
                        {
                            Common.LogWrite("Validation success for all mandatory fileds.");

                            string authorizeToken = string.Empty;

                            if(Application[Common.APPLICATION_SESSION_NAME] == null)
                            {
                                Common.LogWrite("Auth token is null.");

                                //Generate Authorize Token
                                authorizeToken = GetAuthorizeToken();

                                if(authorizeToken != string.Empty)
                                {
                                    Common.LogWrite("New Auth token is generated.");

                                    Application[Common.APPLICATION_SESSION_NAME] = authorizeToken;

                                    Common.LogWrite("Processing Authorize payment Card transaction.");

                                    ProcessAuthTransaction(authorizeToken);
                                }
                            }
                            else
                            {
                                Common.LogWrite("Auth token is not null.");

                                authorizeToken = Application[Common.APPLICATION_SESSION_NAME].ToString();

                                Common.LogWrite("Processing Authorize payment Card transaction.");

                                ProcessAuthTransaction(authorizeToken);
                            }
                        }
                        else
                        {
                            Common.LogWrite("Validation unsuccess, some of the parameter(s) missing.");
                            Common.LogWrite("Sent response as Insufficient Parameters.");

                            checkOutPostData = Common.ConstructCheckOutResponse(Common.INSUFFICIENT_PARAMETERS_ERROR_CODE, Common.INSUFFICIENT_PARAMETERS_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                            string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                            Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                            Session["SmartCommOrderResponse"] = checkOutPostData;
                            Response.Redirect("ProxySmarterCommerce.aspx", false);
                        }
                    }
                    //Else
                    else
                    {
                        Common.LogWrite("Consumer order Session data is null.");
                        Common.LogWrite("Sending response as Insufficient Parameters.");

                        checkOutPostData = Common.ConstructCheckOutResponse(Common.INSUFFICIENT_PARAMETERS_ERROR_CODE, Common.INSUFFICIENT_PARAMETERS_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                        string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                        Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                        Session["SmartCommOrderResponse"] = checkOutPostData;
                        Response.Redirect("ProxySmarterCommerce.aspx", false);
                    }
                }
                else
                {
                    hdnFlag.Value = "true";
                    txtCaptcha.Value = string.Empty;
                    lblCaptcha.Text = "Invalid captcha, enter the characters shown in picture.";

                    Common.LogWrite("Invalid captcha.");

                }
            }
        }
        //Redirect to error page.
        catch(Exception ex)
        {
            Common.LogWrite("Exception: " + ex.Message);
            Common.LogWrite("Exception: " + ex.InnerException);
            Common.LogWrite("Exception: " + ex.StackTrace);

           // Response.Redirect("ErrorPage.aspx", false);
            checkOutPostData = Common.ConstructCheckOutResponse(Common.INSUFFICIENT_PARAMETERS_ERROR_CODE, Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                          string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                          string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
            Common.LogWrite("Response Name Value Collection \r\n" + allValues);
            Session["SmartCommOrderResponse"] = checkOutPostData;
            Response.Redirect("ProxySmarterCommerce.aspx", false);
        }
        finally
        {
            m.ReleaseMutex();
        }
    }

    #endregion

    #region btnCancel_Click

    /// <summary>
    /// Cancels the current transaction operation.
    /// </summary>
    /// <param name="sender">Page/Control that initiates the event.</param>
    /// <param name="e">Event Arguments.</param>
    protected void btnCancel_Click (object sender, EventArgs e)
    {
        //Redirect the page to the Callback url if the session related to consumer exists.
       
            consumerOrderInfo = Session["ConsumerOrderInfo"] as ConsumerOrderDetails;

            NameValueCollection checkOutPostData = Common.ConstructCheckOutResponse(Common.TRANSACTION_CANCEL_CODE,Common.TRANSACTION_CANCEL_MESSAGE,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            Common.LogWrite("Transaction has cancelled.");
            string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
            Common.LogWrite("Response Name Value Collection \r\n" + allValues);
            Session["SmartCommOrderResponse"] = checkOutPostData;
            Response.Redirect("ProxySmarterCommerce.aspx", false);

        
        
    }

    #endregion

    #region Get AuthorizeToken
    private string GetAuthorizeToken ()
    {
        string autorizeToken = string.Empty;

        //Get Authorize Token request xml
        string tokenRequestXml = Common.GetTokenRequestXML();

        //Initialize AuthTokenResponse object
        AuthTokenResponse objAuthTokenResponse = new AuthTokenResponse();

        if(tokenRequestXml != string.Empty)
        {
            Common.LogWrite("Generated token request soap xml." + tokenRequestXml);
            //Common.LogWrite("Generated token request soap xml." );

            Common.LogWrite("Sending Token request to milacron gateway. ");

            string tokenResponse = SmarterCommProcessRequest.ProcessAutorizeRequest(tokenRequestXml, Common.TYPE_OF_SOAP_METHOD_TOKEN, ref errorMessage,ref httpresponsetimeout);

            Common.LogWrite("Check the any exception in gateway.");

            if(errorMessage != string.Empty)
            {
                Common.LogWrite("Exception from gateway: " + errorMessage);
                Common.LogWrite("Sent response as Gateway Error.");
                if (!httpresponsetimeout)
                {
                    checkOutPostData = Common.ConstructCheckOutResponse(Common.SERVER_ERROR_CODE, Common.SERVER_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                }
                else
                {
                    checkOutPostData = Common.ConstructCheckOutResponse(12, Common.HTTP_RESPONSE_TIMEOUT, string.Empty, string.Empty,
                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                }
                string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                Session["SmartCommOrderResponse"] = checkOutPostData;
                Response.Redirect("ProxySmarterCommerce.aspx", false);
            }
            else if(tokenResponse != string.Empty)
            {
                Common.LogWrite("Got the response from gateway.");

                //Remove namspaces from soap xml
                xmlDoc = Common.RemoveXmlns(tokenResponse);

                //Get the Token response xml
                XMLData = xmlDoc.InnerXml.ToString();

                //Common.LogWrite("Token response soap xml as: " + xmlDoc.InnerXml);
                Common.LogWrite("Generated Authorize Token response soap xml."+tokenResponse);
                Common.LogWrite("Parsing Token response soap xml.");

                //Get Auth Token response Info object
                objAuthTokenResponse = (AuthTokenResponse)Common.ParsingProcess(XMLData, typeof(AuthTokenResponse));

                Common.LogWrite("Parsing Token response soap xml completed.");

                if(objAuthTokenResponse != null && objAuthTokenResponse.Body.AuthorizeTokenResponse.TokenResponse.Token != string.Empty)
                {
                    Common.LogWrite("Got the new AuthToken.");

                    autorizeToken = objAuthTokenResponse.Body.AuthorizeTokenResponse.TokenResponse.Token;

                }
            }
            else
            {
                Common.LogWrite("Sent response as Gateway Error.");

                checkOutPostData = Common.ConstructCheckOutResponse(Common.MILACRON_GATEWAY_ERROR_CODE, Common.MILACRON_GATEWAY_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                Session["SmartCommOrderResponse"] = checkOutPostData;
                Response.Redirect("ProxySmarterCommerce.aspx", false);
            }
        }

        return autorizeToken;
    }

#endregion

    #region Process Auth Transaction
    private void ProcessAuthTransaction (string authorizeToken)
    {
        try
        {
            int expiryMonth = Convert.ToInt32(hdnMonth.Value);
            int expiryYear = Convert.ToInt32(hdnYear.Value);

            //Remove spacing from card number
            string cardNumber = txtCardNumber.Value.Trim().Replace(" ", string.Empty);

            //Get the card type Visa or Mster etc
            cardType = Common.GetCardType(txtCardNumber.Value.Trim().Replace(" ", string.Empty));

            Common.LogWrite("Got the card type as " + cardType);

            //Get the complete expirydate
            string cardExpiryDate = Common.GetCardExpiryDate(expiryYear.ToString(), expiryMonth);

            int bSavedCard = (lblCheckBox.Visible == true && chkRememberMe.Checked == true ? 1 : (lblCheckBox.Visible == false) ? 0 : 0);

            //Get Authorize Payment Card xml request.
            string authTransrequestXML = Common.GetAuthorizePaymentCardXml(consumerOrderInfo, authorizeToken.ToString(), cardExpiryDate, 
                                                                           cardNumber, txtCvvNumber.Value, cardType, bSavedCard, true);

            //Common.LogWrite("Generated Authorize payment card request soap xml.:" + authTransrequestXML);
            Common.LogWrite("Generated Authorize payment card request soap xml." + authTransrequestXML);
            Common.LogWrite("Sending Authorize payment card request to milacron gateway. ");

            string responseData = SmarterCommProcessRequest.ProcessAutorizeRequest(authTransrequestXML, Common.TYPE_OF_SOAP_METHOD_AUTHORIZE_CARD, ref errorMessage,ref httpresponsetimeout);

            Common.LogWrite("Check the any exception in gateway.");

            if(errorMessage != string.Empty)
            {
                Common.LogWrite("Exception from gateway: " + errorMessage);
                Common.LogWrite("Sent response as Gateway Error.");

                if (!httpresponsetimeout)
                {
                    checkOutPostData = Common.ConstructCheckOutResponse(Common.SERVER_ERROR_CODE, Common.SERVER_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                }
                else
                {
                    checkOutPostData = Common.ConstructCheckOutResponse(12, Common.HTTP_RESPONSE_TIMEOUT, string.Empty, string.Empty,
                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                }

                string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                Session["SmartCommOrderResponse"] = checkOutPostData;
                Response.Redirect("ProxySmarterCommerce.aspx", false);
            }
            else if(responseData != string.Empty)
            {
                Common.LogWrite("Check the response is not null."); 
                Common.LogWrite("Got the response from gateway.");

                XMLData = string.Empty;

                xmlDoc = new XmlDocument();

                //Remove xml namespacess
                xmlDoc = Common.RemoveXmlns(responseData);

                //Common.LogWrite("Authorize payment card response soap xml as: " + xmlDoc.InnerXml);
                Common.LogWrite("Got the Authorize payment card response soap xml.");

                XMLData = xmlDoc.InnerXml.ToString();

                //Initialize Authorize payment card response
                AuthTransactionResponse objAuthResponse = new AuthTransactionResponse();

                Common.LogWrite("Parsing Authorize Payment Card response soap xml.");

                //Get Auth Token response Info object
                objAuthResponse = (AuthTransactionResponse)Common.ParsingProcess(XMLData, typeof(AuthTransactionResponse));

                Common.LogWrite("Parsing Authorize Payment Card response soap xml completed.");

                if(objAuthResponse != null)
                {
                    if(objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Code != string.Empty && objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Code == "0")
                    {

                        string maskedCardNumber = Common.MaskCreditCardNumber(cardNumber, UNMASK_COUNT);

                        Common.LogWrite("Gateway has approved the transaction.");

                        Common.LogWrite("Creating approved transaction response as name value collection.");

                        checkOutPostData = Common.ConstructCheckOutResponse(Convert.ToInt32(objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Code),
                                                                            objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Description,
                                                                            objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.TransactionAmount.ToString(),
                                                                            string.Empty, string.Empty, string.Empty, objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.SavedCardLine,
                                                                            objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.AuthCode,
                                                                            objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.OrderNumber,
                                                                            (lblCheckBox.Visible == true && chkRememberMe.Checked) ? "true" : (lblCheckBox.Visible == false) ? string.Empty : "false",
                                                                            maskedCardNumber, cardType, objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.PNRef,
                                                                            objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.SavedCardCustomer, string.Empty);

                        string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                        Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                        Session["SmartCommOrderResponse"] = checkOutPostData;
                        Response.Redirect("ProxySmarterCommerce.aspx", false);

                    }
                    else if(objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Code != string.Empty)
                    {
                        Common.LogWrite("Gateway has not approved the transaction.");
                        Common.LogWrite("Gateway has return as " + objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Description);
                        Common.LogWrite("Creating failed transaction response as name value collection.");

                        checkOutPostData = Common.ConstructCheckOutResponse(Convert.ToInt32(objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Code),
                                                                           objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Description,
                                                                           (objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.TransactionAmount != string.Empty) ? objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.TransactionAmount : string.Empty,
                                                                           string.Empty, string.Empty, string.Empty,
                                                                           (objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.SavedCardLine != string.Empty) ? objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.SavedCardLine : string.Empty,
                                                                           (objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.AuthCode != string.Empty) ? objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.AuthCode : string.Empty,
                                                                           (objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.OrderNumber != string.Empty) ? objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.OrderNumber : string.Empty,
                                                                           string.Empty, string.Empty, string.Empty,
                                                                           (objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.PNRef != string.Empty) ? objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.PNRef : string.Empty,
                                                                           (objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.SavedCardCustomer != string.Empty) ? objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.SavedCardCustomer : string.Empty, string.Empty);

                        string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                        Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                        Session["SmartCommOrderResponse"] = checkOutPostData;
                        Response.Redirect("ProxySmarterCommerce.aspx", false);
                    }
                    else if(objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.ReturnCode != string.Empty && objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.ReturnCode == "-2")
                    {
                        Common.LogWrite("Token expires, " + objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.ReturnMessage);

                        string authToken = GetAuthorizeToken();

                        if(authToken != string.Empty)
                        {
                            //Clear the previous AuthToken
                            Application[Common.APPLICATION_SESSION_NAME] = null;

                            //Assign new AuthToken
                            Application[Common.APPLICATION_SESSION_NAME] = authToken;

                            //Process the Auth Transaction
                            ProcessAuthTransaction(authToken);
                        }
                    }
                    else if(objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.ReturnCode != string.Empty)
                    {

                        Common.LogWrite("Gateway has not approved the transaction.");
                        Common.LogWrite("Gateway has return as " + objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.ReturnMessage);
                        Common.LogWrite("Creating failed transaction response as name value collection.");

                        checkOutPostData = Common.ConstructCheckOutResponse(Convert.ToInt32(objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.ReturnCode),
                                                                           objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.ReturnMessage,
                                                                           string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                           string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                        string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                        Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                        Session["SmartCommOrderResponse"] = checkOutPostData;
                        Response.Redirect("ProxySmarterCommerce.aspx", false);
                    }
                }
                else
                {
                    Common.LogWrite("Sent response as Gateway Error.");

                    checkOutPostData = Common.ConstructCheckOutResponse(Common.MILACRON_GATEWAY_ERROR_CODE, Common.MILACRON_GATEWAY_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                    Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                    Session["SmartCommOrderResponse"] = checkOutPostData;
                    Response.Redirect("ProxySmarterCommerce.aspx", false);
                }
            }
            else
            {

                 Common.LogWrite("Sent response as Gateway Error.");

                checkOutPostData = Common.ConstructCheckOutResponse(Common.MILACRON_GATEWAY_ERROR_CODE, Common.MILACRON_GATEWAY_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                Session["SmartCommOrderResponse"] = checkOutPostData;
                Response.Redirect("ProxySmarterCommerce.aspx", false);
            }
        }
        catch(Exception ex)
        {
           

            Common.LogWrite("Exception: " + ex.Message);
            Common.LogWrite("Exception: " + ex.InnerException);
            Common.LogWrite("Exception: " + ex.StackTrace);

            // Response.Redirect("ErrorPage.aspx", false);
            checkOutPostData = Common.ConstructCheckOutResponse(Common.INSUFFICIENT_PARAMETERS_ERROR_CODE, Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                          string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                          string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
            Common.LogWrite("Response Name Value Collection \r\n" + allValues);
            Session["SmartCommOrderResponse"] = checkOutPostData;
            Response.Redirect("ProxySmarterCommerce.aspx", false);
        }

    }

#endregion

    #region custValCardNumber_ServerValidate

    /// <summary>
    /// Custom validator to validate the credit card number.
    /// </summary>
    /// <param name="source">Page that initiates the event.</param>
    /// <param name="args">Event Argument that contains the credit card number.</param>
    protected void custValCardNumber_ServerValidate (object source, ServerValidateEventArgs args)
    {
        //args.IsValid = Common.ApplyLuhnsFormula(args.Value);
        //args.IsValid = Common.GetCardType(args.Value.Trim().Replace(" ", string.Empty), ref cardType);
    }

    #endregion

    #region GetRequestField

    /// <summary>
    /// Gets the checkout request field w.r.t. given key identifier.
    /// </summary>
    /// <param name="key">Key of the request field.</param>
    /// <param name="defaultValue">Default value that will be used on failure to retrieve the value for the associated key.</param>
    /// <returns>Request field value for the associated key.</returns>
    private string GetRequestField (string key, string defaultValue)
    {
        string requestValue;

        if(requestFields.TryGetValue(key, out requestValue))
        {
            return requestValue;
        }
        else
        {
            return defaultValue;
        }
    }

    #endregion          

    public void loadDropDownList()
    {
        for (int i = 1; i <= 12; i++)
        {
            DateTime month = new DateTime(2000, i, 1);
            ListItem li = new ListItem(month.ToString("MM"));
            txtExpiryMonth.Items.Add(li);
        }


        //Populate the credit card expiration year drop down (go out 12 years)  
        for (int i = 0; i <= 35; i++)
        {
            String year = (DateTime.Today.Year + i).ToString();
            ListItem li = new ListItem(year, year);
            txtExpiryYear.Items.Add(li);
        }

    }

    public Stream GenerateStreamFromString(string s)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(s));
    }

 

    #region Dispose

    /// <summary>
    /// Implementation of dispose to free resources.
    /// </summary>
    /// <param name="disposedStatus">The status of the disposed operation.</param>
    protected virtual void Dispose (bool disposedStatus)
    {
        if(!isDisposed)
        {
            isDisposed = true;

            //Released unmanaged resources.
            if(disposedStatus)
            {
                //Release the managed resources.                

                //Nullify the consumerInformation.
                if(consumerOrderInfo != null)
                {
                    consumerOrderInfo = null;
                }

                //Nullify the requestFields.
                if(requestFields != null)
                {
                    requestFields = null;
                }
            }
        }
    }

    #endregion

    #region IDisposable Members

    #region Dispose

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public new void Dispose ()
    {
        //True is passed in dispose method to clean managed resources.
        Dispose(true);

        //If dispose is called already, then inform GC to skip finalize on this instance.
        GC.SuppressFinalize(this);
    }

    #endregion

    #endregion 
}