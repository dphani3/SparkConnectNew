using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml;
using TF.FocusPayCheckOut;
using TF.FocusPayCheckOut.Response;

public partial class ConvertQuoteCheckOut : System.Web.UI.Page, IDisposable
{
    #region Member Variables

    //This is for Garbage Collector.
    private bool isDisposed = false;

    //HTTP Post data fields.
    private SortedList<string, string> requestFields = null;

    //Consumer information.
    private ConvertQuoteDetails convertQuoteInfo = null;

    static string cardType = string.Empty;

    //Unmask count for credit card masking.
    private const int UNMASK_COUNT = 4;

    NameValueCollection checkOutPostData = null;

    XmlDocument xmlDoc = null;
    string XMLData = string.Empty;

    string errorMessage = string.Empty;

    #endregion

    #region Constructor

    /// <summary>
    /// This is the Constructor for the Request class.
    /// </summary>
    public ConvertQuoteCheckOut()
    {

    }

    #endregion

    #region Destructor

    /// <summary>
    /// This is the Destructor for the class.
    /// </summary>
    ~ConvertQuoteCheckOut()
    {
        //Since finalizer is called, there is no need to free the managed resources. So, false is passed.
        Dispose(false);
    }

    #endregion

    #region Page_Load

    /// <summary>
    /// This will parses the convert quote information post data and stores the same in session for further use.
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

            if(!Page.IsPostBack)
            {
                //Check whether the request has post data or not.
                if(Request.ContentLength > 0)
                {
                    Common.FocusPayCheckoutLogger.Debug("Request has data.");

                    //Read the post data.
                    using(StreamReader requestReader = new StreamReader(Request.InputStream))
                    {
                        string[] requestDataCollection = requestReader.ReadToEnd().Split('&');

                        Common.FocusPayCheckoutLogger.Debug("Reading Namevalue collection data.");

                        if(requestDataCollection != null && requestDataCollection.Length > 0)
                        {
                            //Fill the request fileds into collection.
                            requestFields = new SortedList<string, string>();

                            foreach(string requestData in requestDataCollection)
                            {
                                string[] requestItem = requestData.Split('=');

                                if(requestItem != null && requestItem.Count() == 2)
                                {
                                    requestFields.Add(requestItem[0], HttpUtility.UrlDecode(requestItem[1]));
                                }
                            }

                            Common.FocusPayCheckoutLogger.Debug("Posted data in new card: " + requestFields.ToString());
                            Common.FocusPayCheckoutLogger.Debug("Retriving the parameter value from the namevalue collection and assigning respective creeate quote class properties.");
                            //Construct the convert quote details object.
                            convertQuoteInfo             = new ConvertQuoteDetails
                            {
                                OrganizationCode        = GetRequestField(Common.ORGANIZATION_CODE, ""),
                                UserName                = GetRequestField(Common.USER_NAME, ""),
                                AddressLine1            = GetRequestField(Common.ADDRESS_LINE1, ""),
                                AddressLine2            = GetRequestField(Common.ADDRESS_LINE2, ""),
                                AddressLine3            = GetRequestField(Common.ADDRESS_LINE3, ""),
                                AddressLine4            = GetRequestField(Common.ADDRESS_LINE4, ""),
                                City                    = GetRequestField(Common.CITY, ""),
                                Country                 = GetRequestField(Common.COUNTRY, ""),
                                Fax                     = GetRequestField(Common.FAX, ""),
                                Name                    = GetRequestField(Common.NAME, ""),
                                Phone                   = GetRequestField(Common.PHONE, ""),
                                PostalCode              = GetRequestField(Common.POSTAL_CODE, ""),
                                State                   = GetRequestField(Common.STATE, ""),
                                CarrierCode             = GetRequestField(Common.CARRIER_CODE, ""),
                                OrderCompany            = GetRequestField(Common.QUOTE_COMPANY, ""),
                                CustomerPO              = GetRequestField(Common.CUSTOMER_PO, ""),
                                QuoteNumber             = GetRequestField(Common.QUOTE_NUMBER,""),
                                TransactionAmount       = GetRequestField(Common.AMOUNT, ""),
                                OrderCurrency           = GetRequestField(Common.CURRENCY, ""),
                                IsGuestUser             = GetRequestField(Common.ORDER_IS_GUEST_USER, ""),
                                OrderCallbackUrl        = GetRequestField(Common.CALLBACK, ""),
                            };

                            //Check the Organization Code.
                            if(!String.IsNullOrEmpty(convertQuoteInfo.OrganizationCode))
                            {
                                Common.FocusPayCheckoutLogger.Debug("Got the Organization Code.");

                            }
                            //Else
                            else
                            {
                                Common.FocusPayCheckoutLogger.Debug("Organization Code is null.");

                                isValid = true;
                            }

                            //Check the User Name.
                            if(!String.IsNullOrEmpty(convertQuoteInfo.UserName))
                            {
                                Common.FocusPayCheckoutLogger.Debug("Got the User Name.");

                            }
                            //Else
                            else
                            {
                                Common.FocusPayCheckoutLogger.Debug("User Name is null.");

                                isValid = true;
                            }

                            //Check the AddressLine1.
                            if(!String.IsNullOrEmpty(convertQuoteInfo.AddressLine1))
                            {
                                Common.FocusPayCheckoutLogger.Debug("Got the AddressLine1.");

                            }
                            //Else
                            else
                            {
                                Common.FocusPayCheckoutLogger.Debug("AddressLine1 is null.");

                                isValid = true;
                            }

                            //Assign the consumer name to the checkout page.
                            if(!String.IsNullOrEmpty(convertQuoteInfo.Name))
                            {
                                Common.FocusPayCheckoutLogger.Debug("Got the consumer name.");

                                lblWelcomeText.Text = string.Format("{0}", convertQuoteInfo.Name);
                            }
                            //Else
                            else
                            {
                                Common.FocusPayCheckoutLogger.Debug("Consumer name is null.");

                                isValid = true;

                            }

                            //Check the Postal Code.
                            if(!String.IsNullOrEmpty(convertQuoteInfo.PostalCode))
                            {
                                Common.FocusPayCheckoutLogger.Debug("Got the Postal Code.");
                            }
                            //Else
                            else
                            {
                                Common.FocusPayCheckoutLogger.Debug("Postal Code is null.");

                                isValid = true;
                            }

                            //Check the Order Company.
                            if(!String.IsNullOrEmpty(convertQuoteInfo.OrderCompany))
                            {
                                Common.FocusPayCheckoutLogger.Debug("Got the Order Company.");
                            }
                            //Else
                            else
                            {
                                Common.FocusPayCheckoutLogger.Debug("Order Company is null.");

                                isValid = true;
                            }

                            //Assign the Quote Number value to the checkout page.
                            if(!String.IsNullOrEmpty(convertQuoteInfo.QuoteNumber))
                            {
                                Common.FocusPayCheckoutLogger.Debug("Got the Quote Number.");

                                lblQuoteNumber.Text = convertQuoteInfo.QuoteNumber;
                            }
                            //Else
                            else
                            {
                                Common.FocusPayCheckoutLogger.Debug("Quote Number is null.");

                                isValid = true;
                            }

                            //Assign the Transaction Amount value to the checkout page.
                            if(!String.IsNullOrEmpty(convertQuoteInfo.TransactionAmount))
                            {
                                decimal amount = Common.GetDecimalMoneyValue(convertQuoteInfo.TransactionAmount);

                                string transcationAmount = string.Empty;

                                if(!amount.ToString().Contains('.'))
                                    transcationAmount = string.Format("{0}.00", amount.ToString());
                                else
                                    transcationAmount = amount.ToString();

                                Common.FocusPayCheckoutLogger.Debug("Got the Transaction Amount.");

                                lblAmountValue.Text = transcationAmount;
                            }
                            //Else
                            else
                            {
                                Common.FocusPayCheckoutLogger.Debug("Transaction Amount is null.");

                                isValid = true;
                            }

                            //Assign the currency value to the checkout page.
                            if(!String.IsNullOrEmpty(convertQuoteInfo.OrderCurrency))
                            {
                                bool isValidCurr = Common.Validate_Currency(convertQuoteInfo.OrderCurrency, ref currencySymbol);

                                if(isValidCurr)
                                {
                                    Common.FocusPayCheckoutLogger.Debug("Got the Currency Type.");

                                    lblCurrencyValue.Text = currencySymbol;
                                }
                                else
                                {
                                    Common.FocusPayCheckoutLogger.Debug("Invalid Currency Type.");

                                    isValidCurrency = true;
                                }
                            }
                            //Else
                            else
                            {
                                Common.FocusPayCheckoutLogger.Debug("Currency type is null.");

                                isValid = true;
                            }

                            //If the user is registered user then enable SavedSave Card Number check box
                            if(convertQuoteInfo.IsGuestUser == "0")
                            {
                                Common.FocusPayCheckoutLogger.Debug("Not a Guest user.");

                                lblCheckBox.Visible = true;
                                chkRememberMe.Checked = false;
                            }
                            //else the user is guest user the disalbe the Save Card Number check box
                            else
                            {
                                Common.FocusPayCheckoutLogger.Debug("Guest user.");

                                lblCheckBox.Visible = false;
                                chkRememberMe.Checked = false;
                            }

                            //Store the convert quote information into the session.
                            Session["ConvertQuoteInfo"] = convertQuoteInfo;

                            if(isValidCurrency)
                            {
                                Common.FocusPayCheckoutLogger.Debug("Invalid Currency Type.");

                                checkOutPostData = Common.ConstructCheckOutResponse(Common.INVALID_CURRENCY_CODE_ERROR_CODE, Common.INVALID_CURRENCY_CODE_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty);

                                Session["SmartCommQuoteResponse"] = checkOutPostData;
                                Response.Redirect("ConvertQuoteProxy.aspx", false);
                            }
                            else if(isValid)
                            {
                                Common.FocusPayCheckoutLogger.Debug("Insufficient Parameters.");

                                checkOutPostData = Common.ConstructCheckOutResponse(Common.INSUFFICIENT_PARAMETERS_ERROR_CODE, Common.INSUFFICIENT_PARAMETERS_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty);
                                Session["SmartCommQuoteResponse"] = checkOutPostData;
                                Response.Redirect("ConvertQuoteProxy.aspx", false);
                            }
                        }
                        //Else, redirect to error page.
                        else
                        {
                            Common.FocusPayCheckoutLogger.Debug("Request data is null.");

                            Response.Redirect("ErrorPage.aspx", false);
                        }
                    }
                }
                //Else, redirect to error page.
                else
                {
                    Common.FocusPayCheckoutLogger.Debug("Request data is null.");

                    Response.Redirect("ErrorPage.aspx", false);
                }
            }

        }
        //Redirect to error page.
        catch(Exception ex)
        {
            Common.FocusPayCheckoutLogger.Error("Exception: " + ex.Message);

            Response.Redirect("ErrorPage.aspx", false);
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
        try
        {
            Common.FocusPayCheckoutLogger.Debug("Pay button event.");

            //Check whether the all controls on the page have been validated or not.
            if(Page.IsValid)
            {
                Common.FocusPayCheckoutLogger.Debug("Page validation done.");

                //Validate the captcha information aslo.
                captNoBot.ValidateCaptcha(txtCaptcha.Value.Trim());

                lblExpiryMonthYear.Text = string.Empty;
                lblCaptcha.Text = string.Empty;
                lblCardNoError.Text = string.Empty;

                //If Catptcha is validated.
                if(captNoBot.UserValidated)
                {
                    Common.FocusPayCheckoutLogger.Debug("Valid captcha.");

                    int expiryMonth = Convert.ToInt32(txtExpiryMonthYear.Value.Substring(0, 2));
                    int expiryYear = Convert.ToInt32(txtExpiryMonthYear.Value.Substring(txtExpiryMonthYear.Value.LastIndexOf("/") + 2, 2));

                    int currentMonth = DateTime.Now.Month;
                    string currentYear = DateTime.Now.Year.ToString();

                    //Check card expity year as less than current year.
                    if(expiryYear < Convert.ToInt32(currentYear.Substring(2, 2)))
                    {
                        lblExpiryMonthYear.Text = "Invalid Card Expiry";
                        txtCaptcha.Value = string.Empty;
                        hdnFlag.Value = "true";

                        Common.FocusPayCheckoutLogger.Debug("Invalid card expiry year.");

                    }
                    //check card expiry year is greater than current year
                    else if(expiryYear > Convert.ToInt32(currentYear.Substring(2, 2)))
                    {
                        //Check card expiry month is more than 12
                        if(expiryMonth > 12)
                        {
                            lblExpiryMonthYear.Text = "Invalid Card Expiry";
                            txtCaptcha.Value = string.Empty;
                            hdnFlag.Value = "true";

                            Common.FocusPayCheckoutLogger.Debug("Invalid card expiry month.");

                        }
                    }
                    //Check card expiry year is current year
                    else if(expiryYear == Convert.ToInt32(currentYear.Substring(2, 2)))
                    {
                        //Check card expiry month is less than current month
                        if(expiryMonth < currentMonth)
                        {
                            lblExpiryMonthYear.Text = "Invalid Card Expiry";
                            txtCaptcha.Value = string.Empty;
                            hdnFlag.Value = "true";

                            Common.FocusPayCheckoutLogger.Debug("Invalid card expiry month.");

                        }
                        //Check card expiry month is more than 12
                        else if(expiryMonth > 12)
                        {
                            lblExpiryMonthYear.Text = "Invalid Card Expiry";
                            txtCaptcha.Value = string.Empty;
                            hdnFlag.Value = "true";

                            Common.FocusPayCheckoutLogger.Debug("Invalid card expiry month.");

                        }
                    }
                    //Check whether the session related to convert quote information exists or not.
                    if(Session["ConvertQuoteInfo"] != null)
                    {
                        Common.FocusPayCheckoutLogger.Debug("Check convert quote details Session data is null.");

                        hdnFlag.Value = "";

                        //Extract the convert quote information from the session.
                        convertQuoteInfo = Session["ConvertQuoteInfo"] as ConvertQuoteDetails;

                        if(convertQuoteInfo.OrganizationCode != string.Empty && convertQuoteInfo.UserName != string.Empty &&
                          convertQuoteInfo.AddressLine1 != string.Empty && convertQuoteInfo.Name != string.Empty &&
                          convertQuoteInfo.PostalCode != string.Empty && convertQuoteInfo.OrderCompany != string.Empty &&
                          convertQuoteInfo.QuoteNumber != string.Empty && convertQuoteInfo.TransactionAmount != string.Empty &&
                          convertQuoteInfo.OrderCurrency != string.Empty)
                        {
                            Common.FocusPayCheckoutLogger.Debug("Validation success for all mandatory fileds.");

                            string authorizeToken = string.Empty;

                            if(Application[Common.APPLICATION_SESSION_NAME] == null)
                            {
                                Common.FocusPayCheckoutLogger.Debug("Auth token is null.");

                                //Generate Authorize Token
                                authorizeToken = GetAuthorizeToken();

                                if(authorizeToken != string.Empty)
                                {
                                    Common.FocusPayCheckoutLogger.Debug("New Auth token is generated.");

                                    Application[Common.APPLICATION_SESSION_NAME] = authorizeToken;

                                    Common.FocusPayCheckoutLogger.Debug("Processing Conver Quote transaction.");

                                    ProcessConverQuoteTransaction(authorizeToken);
                                }
                            }
                            else
                            {
                                Common.FocusPayCheckoutLogger.Debug("Auth token is not null.");

                                authorizeToken = Application[Common.APPLICATION_SESSION_NAME].ToString();

                                Common.FocusPayCheckoutLogger.Debug("Processing Conver Quote transaction.");

                                ProcessConverQuoteTransaction(authorizeToken);
                            }
                        }
                        else
                        {
                            Common.FocusPayCheckoutLogger.Debug("Validation unsuccess, some of the parameter(s) missing.");
                            Common.FocusPayCheckoutLogger.Debug("Sent response as Insufficient Parameters.");

                            checkOutPostData = Common.ConstructCheckOutResponse(Common.INSUFFICIENT_PARAMETERS_ERROR_CODE, Common.INSUFFICIENT_PARAMETERS_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                string.Empty, string.Empty, string.Empty, string.Empty);
                            Session["SmartCommQuoteResponse"] = checkOutPostData;
                            Response.Redirect("ConvertQuoteProxy.aspx", false);
                        }
                    }
                    //Else
                    else
                    {
                        Common.FocusPayCheckoutLogger.Debug("Conver quote Session data is null.");
                        Common.FocusPayCheckoutLogger.Debug("Sending response as Insufficient Parameters.");

                        checkOutPostData = Common.ConstructCheckOutResponse(Common.INSUFFICIENT_PARAMETERS_ERROR_CODE, Common.INSUFFICIENT_PARAMETERS_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                            string.Empty, string.Empty, string.Empty, string.Empty);
                        Session["SmartCommQuoteResponse"] = checkOutPostData;
                        Response.Redirect("ConvertQuoteProxy.aspx", false);
                    }
                }
                else
                {
                    hdnFlag.Value = "true";
                    txtCaptcha.Value = string.Empty;
                    lblCaptcha.Text = "Invalid captcha, enter the characters shown in picture.";

                    Common.FocusPayCheckoutLogger.Debug("Invalid captcha.");

                }
            }
        }
        //Redirect to error page.
        catch(Exception ex)
        {
            Common.FocusPayCheckoutLogger.Error("Exception: " + ex.Message);

            Response.Redirect("ErrorPage.aspx", false);
        }
        finally
        {
           
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
        if(Session["ConvertQuoteInfo"] != null)
        {
            convertQuoteInfo = Session["ConvertQuoteInfo"] as ConvertQuoteDetails;

            NameValueCollection checkOutPostData = Common.ConstructCheckOutResponse(Common.TRANSACTION_CANCEL_CODE, Common.TRANSACTION_CANCEL_MESSAGE,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty);

            Common.FocusPayCheckoutLogger.Debug("Transaction has cancelled.");

            Session["SmartCommQuoteResponse"] = checkOutPostData;
            Response.Redirect("ConvertQuoteProxy.aspx", false);

        }
        //Else, redirect to error page.
        else
        {
            Response.Redirect("ErrorPage.aspx", false);
        }
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
            Common.FocusPayCheckoutLogger.Debug("Generated token request soap xml." + tokenRequestXml);
            Common.FocusPayCheckoutLogger.Debug("Generated token request soap xml.");

            Common.FocusPayCheckoutLogger.Debug("Sending Token request to milacron gateway. ");

            string tokenResponse = SmarterCommProcessRequest.ProcessAutorizeRequest(tokenRequestXml, Common.TYPE_OF_SOAP_METHOD_TOKEN, ref errorMessage);

            Common.FocusPayCheckoutLogger.Debug("Check the any exception in gateway.");

            if(errorMessage != string.Empty)
            {
                Common.FocusPayCheckoutLogger.Debug("Exception from gateway: " + errorMessage);
                Common.FocusPayCheckoutLogger.Debug("Sent response as Gateway Error.");

                checkOutPostData = Common.ConstructCheckOutResponse(Common.SERVER_ERROR_CODE, Common.SERVER_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                    string.Empty, string.Empty, string.Empty, string.Empty);

                Session["SmartCommQuoteResponse"] = checkOutPostData;
                Response.Redirect("ConvertQuoteProxy.aspx", false);
            }
            else if(tokenResponse != string.Empty)
            {
                Common.FocusPayCheckoutLogger.Debug("Got the response from gateway.");

                //Remove namspaces from soap xml
                xmlDoc = Common.RemoveXmlns(tokenResponse);

                //Get the Token response xml
                XMLData = xmlDoc.InnerXml.ToString();

                Common.FocusPayCheckoutLogger.Debug("Token response soap xml as: " + xmlDoc.InnerXml);
                Common.FocusPayCheckoutLogger.Debug("Generated Authorize Token response soap xml."); 
                Common.FocusPayCheckoutLogger.Debug("Parsing Token response soap xml.");

                //Get Auth Token response Info object
                objAuthTokenResponse = (AuthTokenResponse)Common.ParsingProcess(XMLData, typeof(AuthTokenResponse));

                Common.FocusPayCheckoutLogger.Debug("Parsing Token response soap xml completed.");

                if(objAuthTokenResponse != null && objAuthTokenResponse.Body.AuthorizeTokenResponse.TokenResponse.Token != string.Empty)
                {
                    Common.FocusPayCheckoutLogger.Debug("Got the new AuthToken.");

                    autorizeToken = objAuthTokenResponse.Body.AuthorizeTokenResponse.TokenResponse.Token;

                }
            }
            else
            {
                Common.FocusPayCheckoutLogger.Debug("Sent response as Gateway Error.");

                checkOutPostData = Common.ConstructCheckOutResponse(Common.MILACRON_GATEWAY_ERROR_CODE, Common.MILACRON_GATEWAY_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                    string.Empty, string.Empty, string.Empty, string.Empty);
                Session["SmartCommQuoteResponse"] = checkOutPostData;
                Response.Redirect("ConvertQuoteProxy.aspx", false);
            }
        }

        return autorizeToken;
    }

    #endregion

    #region Process Auth Transaction
    private void ProcessConverQuoteTransaction (string authorizeToken)
    {
        string authToken = string.Empty;

        try
        {
            int expiryMonth = Convert.ToInt32(txtExpiryMonthYear.Value.Substring(0, 2));
            int expiryYear = Convert.ToInt32(txtExpiryMonthYear.Value.Substring(txtExpiryMonthYear.Value.LastIndexOf("/") + 2, 2));

            //Remove spacing from card number
            string cardNumber = txtCardNumber.Value.Trim().Replace(" ", string.Empty);

            //Get the card type Visa or Mster etc
            cardType = Common.GetCardType(txtCardNumber.Value.Trim().Replace(" ", string.Empty));

            Common.FocusPayCheckoutLogger.Debug("Got the card type as " + cardType);

            //Get the complete expirydate
            string cardExpiryDate = Common.GetCardExpiryDate("20" + expiryYear.ToString(), expiryMonth);

            int bSavedCard = (lblCheckBox.Visible == true && chkRememberMe.Checked == true ? 1 : (lblCheckBox.Visible == false) ? 0 : 0);

            //Get Authorize Payment Card xml request.
            string createQuoterequestXML = Common.CreateQuoteXml(convertQuoteInfo, authorizeToken.ToString(), cardExpiryDate,
                                                                           cardNumber, txtCvvNumber.Value, cardType, bSavedCard, true);

            Common.FocusPayCheckoutLogger.Debug("Generated Convert Quote request soap xml.:" + createQuoterequestXML);
            Common.FocusPayCheckoutLogger.Debug("Generated Convert Quote request soap xml."); 
            Common.FocusPayCheckoutLogger.Debug("Sending Authorize payment card request to milacron gateway. ");

            string responseData = SmarterCommProcessRequest.ProcessAutorizeRequest(createQuoterequestXML, Common.TYPE_OF_SOAP_METHOD_CONVERT_QUOTE, ref errorMessage);

            Common.FocusPayCheckoutLogger.Debug("Check the any exception in gateway.");

            if(errorMessage != string.Empty)
            {
                Common.FocusPayCheckoutLogger.Debug("Exception from gateway: " + errorMessage); 
                Common.FocusPayCheckoutLogger.Debug("Sent response as Gateway Error.");

                checkOutPostData = Common.ConstructCheckOutResponse(Common.SERVER_ERROR_CODE, Common.SERVER_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                    string.Empty, string.Empty, string.Empty, string.Empty);

                Session["SmartCommQuoteResponse"] = checkOutPostData;
                Response.Redirect("ConvertQuoteProxy.aspx", false);
            }
            else if(responseData != string.Empty)
            {
                Common.FocusPayCheckoutLogger.Debug("Check the response is not null.");  
                Common.FocusPayCheckoutLogger.Debug("Got the response from gateway.");

                XMLData = string.Empty;

                xmlDoc = new XmlDocument();

                //Remove xml namespacess
                xmlDoc = Common.RemoveXmlns(responseData);

                Common.FocusPayCheckoutLogger.Debug("Convert Quote response soap xml as: " + xmlDoc.InnerXml);
                Common.FocusPayCheckoutLogger.Debug("Got the Convert Quote response soap xml.");

                XMLData = xmlDoc.InnerXml.ToString();

                //Initialize Convert Quote response
                ConvrtQuoteResponse objConvertQuoteResponse = new ConvrtQuoteResponse();

                Common.FocusPayCheckoutLogger.Debug("Parsing Convert Quote response soap xml.");

                //Get Auth Token response Info object
                objConvertQuoteResponse = (ConvrtQuoteResponse)Common.ParsingProcess(XMLData, typeof(ConvrtQuoteResponse));

                Common.FocusPayCheckoutLogger.Debug("Parsing Convert Quote  response soap xml completed.");

                if(objConvertQuoteResponse != null)
                {
                    if (objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnCode != string.Empty)
                    {

                        if (objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnCode == "0")
                        {
                            string maskedCardNumber = Common.MaskCreditCardNumber(cardNumber, UNMASK_COUNT);

                            Common.FocusPayCheckoutLogger.Debug("Gateway has approved the transaction.");

                            Common.FocusPayCheckoutLogger.Debug("Creating approved transaction response as name value collection.");

                            checkOutPostData = Common.ConstructCheckOutResponse(Convert.ToInt32(objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnCode),
                                                                                objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnMessage,
                                                                                objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.OrderTotal.ToString(),
                                                                                string.Empty, string.Empty, objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.SavedCardLine,
                                                                                string.Empty, objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.OrderNumber,
                                                                                (lblCheckBox.Visible == true && chkRememberMe.Checked) ? "true" : (lblCheckBox.Visible == false) ? string.Empty : "false",
                                                                                maskedCardNumber, cardType, string.Empty, objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.SavedCardCustomer);

                            Session["SmartCommQuoteResponse"] = checkOutPostData;
                            Response.Redirect("ConvertQuoteProxy.aspx", false);

                        }
                        else if (objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnCode == "-1")
                        {

                            Common.FocusPayCheckoutLogger.Debug("Gateway has not approved the transaction.");
                            Common.FocusPayCheckoutLogger.Debug("Gateway has return as " + objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnMessage);
                            Common.FocusPayCheckoutLogger.Debug("Creating failed transaction response as name value collection.");

                            checkOutPostData = Common.ConstructCheckOutResponse(Convert.ToInt32(objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnCode),
                                                                               objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnMessage,
                                                                               (objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.OrderTotal != string.Empty) ? objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.OrderTotal : string.Empty,
                                                                               string.Empty, string.Empty,
                                                                               (objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.SavedCardLine != string.Empty) ? objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.SavedCardLine : string.Empty,
                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                               (objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.SavedCardCustomer != string.Empty) ? objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.SavedCardCustomer : string.Empty);

                            Session["SmartCommQuoteResponse"] = checkOutPostData;
                            Response.Redirect("ConvertQuoteProxy.aspx", false);
                        }
                        else if (objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnCode == "-2")
                        {
                            Common.FocusPayCheckoutLogger.Debug("Token expires, " + objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnMessage);

                            authToken = GetAuthorizeToken();

                            if (authToken != string.Empty)
                            {
                                //Clear the previous AuthToken
                                Application[Common.APPLICATION_SESSION_NAME] = null;

                                //Assign new AuthToken
                                Application[Common.APPLICATION_SESSION_NAME] = authToken;

                                //Process the Conver Quote Transaction
                                ProcessConverQuoteTransaction(authToken);
                            }
                        }
                        else
                        {

                            Common.FocusPayCheckoutLogger.Debug("Gateway has not approved the transaction.");
                            Common.FocusPayCheckoutLogger.Debug("Gateway has return as " + objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnMessage);
                            Common.FocusPayCheckoutLogger.Debug("Creating failed transaction response as name value collection.");

                            checkOutPostData = Common.ConstructCheckOutResponse(Convert.ToInt32(objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnCode),
                                                                               objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnMessage,
                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                            Session["SmartCommQuoteResponse"] = checkOutPostData;
                            Response.Redirect("ConvertQuoteProxy.aspx", false);
                        }
                    }
                    else
                    {
                        Common.FocusPayCheckoutLogger.Debug("Sent response as Gateway Error.");

                        checkOutPostData = Common.ConstructCheckOutResponse(Common.MILACRON_GATEWAY_ERROR_CODE, Common.MILACRON_GATEWAY_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                            string.Empty, string.Empty, string.Empty, string.Empty);
                        Session["SmartCommQuoteResponse"] = checkOutPostData;
                        Response.Redirect("ConvertQuoteProxy.aspx", false);
                    }
                }
                else
                {
                    Common.FocusPayCheckoutLogger.Debug("Sent response as Gateway Error.");

                    checkOutPostData = Common.ConstructCheckOutResponse(Common.MILACRON_GATEWAY_ERROR_CODE, Common.MILACRON_GATEWAY_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                        string.Empty, string.Empty, string.Empty, string.Empty);
                    Session["SmartCommQuoteResponse"] = checkOutPostData;
                    Response.Redirect("ConvertQuoteProxy.aspx", false);
                }
            }
            else
            {

                Common.FocusPayCheckoutLogger.Debug("Sent response as Gateway Error.");

                checkOutPostData = Common.ConstructCheckOutResponse(Common.MILACRON_GATEWAY_ERROR_CODE, Common.MILACRON_GATEWAY_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                    string.Empty, string.Empty, string.Empty, string.Empty);
                Session["SmartCommQuoteResponse"] = checkOutPostData;
                Response.Redirect("ConvertQuoteProxy.aspx", false);
            }
        }
        catch(Exception ex)
        {
            Common.FocusPayCheckoutLogger.Error("Exception: " + ex.Message);

            Response.Redirect("ErrorPage.aspx", false);
        }

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
                if(convertQuoteInfo != null)
                {
                    convertQuoteInfo = null;
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