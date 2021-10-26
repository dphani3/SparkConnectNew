
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: CheckOut.cs
  Description: This page will perform the Sale transaction with given consumer details by sending the same to the FocusConnect.
  Date Created : 20-Nov-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TF.REVCheckOut;
using TF.REVCheckOut.Request;
using TF.REVCheckOut.Response;



#endregion

#region CheckOut

/// <summary>
/// This page will perform the Sale transaction with given consumer details by sending the same to the FocusConnect.
/// </summary>
public partial class CheckOut : System.Web.UI.Page, IDisposable
{
    #region Member Variables

    //FocusConnect url to process the transaction.
    private string focusConnectUrl = string.Empty;

    //This is for Garbage Collector.
    private bool isDisposed = false;

    //HTTP Post data fields.
    private SortedList<string, string> requestFields = null;

    //Consumer information.
    private Consumer consumerInformation = null;

    static string cardType = string.Empty;

    NameValueCollection checkOutPostData = null;

    //private System.Timers.Timer myTimer = new System.Timers.Timer();

    private bool httpresponsetimeout = false;

    NameValueCollection checkOutPostDatarelogin = null;

    bool relogin = false;

    #endregion

    #region Constructor

    /// <summary>
    /// This is the Constructor for the Request class.
    /// </summary>
    public CheckOut()
    {

    }

    #endregion

    #region Destructor

    /// <summary>
    /// This is the Destructor for the class.
    /// </summary>
    ~CheckOut()
    {
        //Since finalizer is called, there is no need to free the managed resources. So, false is passed.
        Dispose(false);
    }

    #endregion

    #region Page_Load

    /// <summary>
    /// This will parses the consumer information the post data and stores the same in session for further use.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">EventArgs.</param>
    protected void Page_Load(object sender, EventArgs e)
            {

            try
            {
                hdnFlag.Value = "true";

                bool isValid = false;

                bool isValidCurrency = false;
                string currencySymbol = string.Empty;

                httpresponsetimeout = false;
                if (!Page.IsPostBack)
                {



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

                                //Construct the consumer object.
                                consumerInformation = new Consumer
                                {
                                    AttendantId = GetRequestField(Common.AID, ""),
                                    Password = GetRequestField(Common.PWD, ""),
                                    ConsumerName = GetRequestField(Common.NAME, ""),
                                    ConsumerAddress = GetRequestField(Common.ADDRESS, ""),
                                    EmailAddress = GetRequestField(Common.EMAIL, ""),
                                    Amount = GetRequestField(Common.AMOUNT, ""),
                                    Currency = GetRequestField(Common.CURRENCY, ""),
                                    IsTestMode = GetRequestField(Common.TEST_MODE, ""),
                                    CallbackUrl = GetRequestField(Common.CALLBACK, ""),
                                    Tokenize = GetRequestField(Common.TOKENIZE, ""),          //Added by Nazreen
                                    Token = GetRequestField(Common.TOKEN, ""),            //Added by Nazreen
                                    TransactionType = Convert.ToInt32(GetRequestField(Common.TRANSACTION_TYPE, "")),  //Added by Nazreen
                                    TransactionID = GetRequestField(Common.TRANSACTION_ID, ""),     //Added by Nazreen
                                    IsGuestUser = GetRequestField(Common.IS_GUEST_USER, ""),
                                    InvoiceNumber = GetRequestField(Common.INVOICE_NUMBER, ""),
                                    OrderNumber = GetRequestField(Common.ORDER_NUMBER, ""),
                                    CardName = GetRequestField(Common.CARD_HOLDER_NAME, ""),
                                    AppKey = GetRequestField(Common.APP_KEY, ""),
                                    Notes = GetRequestField(Common.NOTES, ""),
                                    UDField2 = GetRequestField(Common.UDField2, ""),
                                    UDField3 = GetRequestField(Common.CustomerNo, ""),



                                };

                                //Check Attendant ID.
                                if (!String.IsNullOrEmpty(consumerInformation.AttendantId))
                                {
                                    Common.LogWrite("Got the Attendant ID.");
                                }
                                //Else
                                else
                                {
                                    Common.LogWrite("Attendant ID is null.");

                                    isValid = true;
                                }

                                //Check Password.
                                if (!String.IsNullOrEmpty(consumerInformation.Password))
                                {
                                    Common.LogWrite("Got the Password.");
                                }
                                //Else
                                else
                                {
                                    Common.LogWrite("Password is null.");

                                    isValid = true;
                                }

                                //Assign the consumer name to the checkout page.
                                if (!String.IsNullOrEmpty(consumerInformation.ConsumerName))
                                {
                                    Common.LogWrite("Got Consumer Name.");

                                    lblWelcomeText.Text = string.Format("{0}", consumerInformation.ConsumerName);
                                }
                                //Else
                                else
                                {
                                    Common.LogWrite("Consumer Name is null.");

                                    isValid = true;
                                }

                                //Assign the Transaction amount value to the checkout page.
                                if (!String.IsNullOrEmpty(consumerInformation.Amount))
                                {
                                    decimal amount = Common.GetDecimalMoneyValue(consumerInformation.Amount);

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

                                   
                                    if (consumerInformation.TransactionType == 16)
                                    {
                                        isValid = false;
                                    }
                                    else
                                    {
                                        isValid = true;
                                    }
                                }

                                //Assign the currency value to the checkout page.
                                if (!String.IsNullOrEmpty(consumerInformation.Currency))
                                {
                                    bool isValidCurr = Common.Validate_Currency(consumerInformation.Currency, ref currencySymbol);

                                    if (isValidCurr)
                                    {
                                        Common.LogWrite("Got the Currency Type.");
                                        //lblCurrencyValue.Text = consumerInformation.Currency;
                                        lblCurrencyValue.Text = currencySymbol;
                                    }
                                    else
                                    {
                                        Common.LogWrite("Invalid Currency Type.");
                                        if (consumerInformation.TransactionType == 16)
                                        {
                                            isValidCurrency = false;
                                        }
                                        else
                                        {
                                            isValidCurrency = true;
                                        }
                                    }
                                }
                                //Else
                                else
                                {
                                    Common.LogWrite("Currency Type is null.");

                                    if (consumerInformation.TransactionType == 16)
                                    {
                                        isValid = false;
                                    }
                                    else
                                    {
                                        isValid = true;
                                    }
                                }

                                //Check Transaction Type.
                                if (consumerInformation.TransactionType != 0)
                                {
                                    Common.LogWrite("Got the Transaction Type.");
                                   
                                }
                                //Else
                                else
                                {
                                    Common.LogWrite("Transaction Type is null.");

                                    isValid = true;
                                }

                                if (consumerInformation.IsGuestUser == "0")
                                {
                                    Common.LogWrite("Not a Guest user.");

                                    lblCheckBox.Visible = true;
                                    chkRememberMe.Checked = false;
                                }
                                else
                                {
                                    Common.LogWrite("Guest user.");

                                    lblCheckBox.Visible = false;
                                    chkRememberMe.Checked = false;
                                }
                                if (consumerInformation.TransactionType == Common.SAVE_CARD)
                                {
                                   
                                    lblCurrencyValue.Text = string.Empty;
                                    lblAmountValue.Text = string.Empty;
                                    lblCheckBox.Visible = false;
                                    chkRememberMe.Visible = false;
                                    lblWelcomeText.Text = "Welcome " + consumerInformation.ConsumerName;
                                    lblamounttext.Text = string.Empty;
                                    btnPay.Text = "Submit";
                                    consumerInformation.Tokenize = "true";
                                    lblHeader.Text = "Save Card";
                                    lblcaptha.Text = "below";
                                    lbltxntype.Text = string.Empty;
                                }
                                else
                                {
                                    
                                   
                                    lblamounttext.Text = "Amount: ";
                                    

                                    btnPay.Text = "Pay";
                                }
                                //Store the consumer information into the session.
                                Session["ConsumerInformation"] = consumerInformation;

                                if (isValidCurrency)
                                {
                                    Common.LogWrite("Invalid Currency Type.");

                                    checkOutPostData = Common.ConstructCheckOutResponse(Common.INVALID_CURRENCY_CODE_ERROR_CODE, Common.INVALID_CURRENCY_CODE_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                    string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                    Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                    Session["ConnectResponse"] = checkOutPostData;
                                    Response.Redirect("Proxy.aspx", false);
                                }
                                else if (isValid)
                                {
                                    Common.LogWrite("Insufficient Parameters.");

                                    checkOutPostData = Common.ConstructCheckOutResponse(Common.INSUFFICIENT_PARAMETERS_ERROR_CODE, Common.INSUFFICIENT_PARAMETERS_ERROR_MESSAGE, string.Empty, string.Empty,
                                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                    string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                    Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                    Session["ConnectResponse"] = checkOutPostData;
                                    Response.Redirect("Proxy.aspx", false);
                                }
                            }
                            //Else, redirect to error page.
                            else
                            {
                                Common.LogWrite("Request data is null.");

                                Response.Redirect("ErrorPage.aspx", false);
                            }
                        }
                    }
                    //Else, redirect to error page.
                    else
                    {
                        Common.LogWrite("Request data is null.");

                        Response.Redirect("ErrorPage.aspx", false);
                    }
                }
                else
                {
                    
                    txtExpiryMonth.Items.Clear();
                    txtExpiryYear.Items.Clear();
                   
                    loadDropDownList();
                    txtExpiryMonth.SelectedIndex = Convert.ToInt32(hdnMonth.Value)-1;
                    txtExpiryYear.SelectedIndex = (Convert.ToInt32(hdnYear.Value)) - (DateTime.Now.Year);
                }

            }
            //Redirect to error page.
            catch(Exception ex)
            {
                Common.LogWrite("Exception: " + ex.Message + "\r\n" + ex.StackTrace);
                Response.Redirect("ErrorPage.aspx", false);
            }

            //myTimer.Start();

        
        
    }
   
    #endregion

    #region btnPay_Click

    /// <summary>
    /// Process the current transaction information by sending the transaction details to FocusConnect.
    /// </summary>
    /// <param name="sender">Page/Control that initiates the event.</param>
    /// <param name="e">Event Arguments.</param>
    protected void btnPay_Click(object sender, EventArgs e)
    {        
        try
        {
            Common.LogWrite("Pay button event.");

            //Check whether the all controls on the page have been validated or not.
            if (Page.IsValid)
            {
                Common.LogWrite("Page validation done.");

                //Validate the captcha information aslo.
                captNoBot.ValidateCaptcha(txtCaptcha.Value.Trim());
                //bool isValidCaptcha = SampleCaptcha.Validate(txtCaptcha.Value.Trim());

                lblExpiryMonthYear.Text = string.Empty;
                lblCaptcha.Text = string.Empty;
                lblCardNoError.Text = string.Empty;
               
               bool isRequestEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsRequestLog"]);

                //If Catptcha is validated.
                if(captNoBot.UserValidated)
                //if(isValidCaptcha)
                {
                    Common.LogWrite("Valid captcha.");

                    int expiryMonth = Convert.ToInt32(hdnMonth.Value);
                    int expiryYear = Convert.ToInt32(hdnYear.Value);
                    int expiryYearLasttwo = Convert.ToInt32(hdnYear.Value.Substring(2, 2));
                    int currentMonth = DateTime.Now.Month;
                    string currentYear = DateTime.Now.Year.ToString();



                    //if((expiryMonth < currentMonth && expiryYear < Convert.ToInt32(currentYear.Substring(2, 2))) || (expiryMonth > 12 && expiryYear < Convert.ToInt32(currentYear.Substring(2, 2))))
                    if (expiryYear < Convert.ToInt32(currentYear))
                    {
                        lblExpiryMonthYear.Text = "Invalid Card Expiry";
                        txtCaptcha.Value = string.Empty;
                        hdnFlag.Value = "true";

                        Common.LogWrite("Invalid card expiry year.");

                    }
                    else if (expiryYear > Convert.ToInt32(currentYear))
                    {
                        if (expiryMonth < 1 || expiryMonth > 12)
                        {
                            lblExpiryMonthYear.Text = "Invalid Card Expiry";
                            txtCaptcha.Value = string.Empty;
                            hdnFlag.Value = "true";

                            Common.LogWrite("Invalid card expiry month.");

                        }
                    }
                    else if (expiryYear == Convert.ToInt32(currentYear))
                    {
                        if (expiryMonth < currentMonth)
                        {
                            lblExpiryMonthYear.Text = "Invalid Card Expiry";
                            txtCaptcha.Value = string.Empty;
                            hdnFlag.Value = "true";

                            Common.LogWrite("Invalid card expiry month.");

                        }
                        else if (expiryMonth < 1 || expiryMonth > 12)
                        {
                            lblExpiryMonthYear.Text = "Invalid Card Expiry";
                            txtCaptcha.Value = string.Empty;
                            hdnFlag.Value = "true";

                            Common.LogWrite("Invalid card expiry month.");

                        }
                    }
                    
                    //Check whether the session related to consumer information exists or not.
                    if(Session["ConsumerInformation"] != null)
                    {
                        Common.LogWrite("Check consumer details Session data is null.");

                        hdnFlag.Value = "";

                        //Extract the consumer information from the session.
                        consumerInformation = Session["ConsumerInformation"] as Consumer;

                        string cardNumber = txtCardNumber.Value.Trim().Replace(" ", string.Empty);

                        string cardExpiryDate = Common.GetCardExpiryDate(expiryYearLasttwo, expiryMonth);


                        if (consumerInformation.AppKey == string.Empty)
                        {
                            string loginRequestXml = string.Empty;

                            //Create the FocusConnect login request with the help of consumer information details.

                            using (LoginRequest loginRequest = new LoginRequest
                            {
                                RequestType = Common.LOGIN_REQUEST_TYPE,
                                UserName = consumerInformation.AttendantId,
                                Password = consumerInformation.Password,
                               
                            })
                            {
                                //Serialize the login request object into Xml format.
                                loginRequestXml = loginRequest.SerializeFocusConnectMessage(loginRequest);
                                Common.LogWrite(DateTime.Now.ToString() + loginRequestXml);
                            }

                            if (isRequestEnable)
                            {
                                Common.LogWrite("Generated login xml request." + loginRequestXml);
                            }
                            else
                            {
                                Common.LogWrite("Generated login xml request.");
                            }

                            if (!String.IsNullOrEmpty(loginRequestXml))
                            {
                                //Get the FocusConnect endpoint from configuration file.
                                focusConnectUrl = ConfigurationManager.AppSettings["FocusConnectUrl"].Trim();


                             
                                //If FocusConnect endpoint exists.
                                if (!String.IsNullOrEmpty(focusConnectUrl))
                                {
                                    Common.LogWrite("Connect Url is not empty.");

                                    Common.LogWrite("Sending login request to connect.");

                                  


                                    //Get the login response from FocusConnect.
                                    Stream Responsestream;
                                    String responseString = string.Empty;
                                    if(!httpresponsetimeout)
                                    {
                                    try
                                    {
                                        using (HttpWebResponse connectLoginResponse = SendConnectRequest(loginRequestXml))
                                        {
                                            if (!httpresponsetimeout)
                                            {
                                                try
                                                {
                                                    using (Responsestream = connectLoginResponse.GetResponseStream())
                                                    {
                                                        StreamReader reader = new StreamReader(Responsestream, Encoding.UTF8);
                                                        responseString = reader.ReadToEnd();
                                                        Common.LogWrite(DateTime.Now.ToString() + responseString);
                                                    }
                                                }

                                                catch (Exception ex)
                                                {
                                                    if (httpresponsetimeout)
                                                    {
                                                        Common.LogWrite(" Http Post Exception transaction response." + ex.Message);
                                                        checkOutPostData = Common.ConstructCheckOutResponse(12, Common.HTTP_RESPONSE_TIMEOUT, string.Empty, string.Empty,
                                                                                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                        string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                        Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                        Session["ConnectResponse"] = checkOutPostData;
                                                        Response.Redirect("Proxy.aspx", false);
                                                    }
                                                    else
                                                    {
                                                        Common.LogWrite(" Http Post Exception transaction response." + ex.Message);
                                                        checkOutPostData = Common.ConstructCheckOutResponse(12, Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                        string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                        Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                        Session["ConnectResponse"] = checkOutPostData;
                                                        Response.Redirect("Proxy.aspx", false);
                                                    }

                                                }


                                                Stream stream = GenerateStreamFromString(responseString);
                                                //Deserialize to LoginResponse object.
                                                using (LoginResponse loginResponse = TF.REVCheckOut.Response.Response.DeserializeFocusConnectMessage(stream, typeof(LoginResponse)) as LoginResponse)
                                                {

                                                    Common.LogWrite("Got the response from gateway.");

                                                    if (loginResponse != null)
                                                    {
                                                        Common.LogWrite("Connect response is not null.");

                                                        //Check whether the login is successful or not.
                                                        if (loginResponse.Code == 0)
                                                        {
                                                            Common.LogWrite("Gateway has approved the transaction.");

                                                            string ccRequestXml = string.Empty;
                                                            if (consumerInformation.TransactionType == Common.SAVE_CARD)
                                                            {
                                                                consumerInformation.TransactionType = Common.AUTHORIZE_TRANSACTION;
                                                            }
                                                            if (consumerInformation.TransactionType == Common.SALE_TRANSACTION ||
                                                               consumerInformation.TransactionType == Common.AUTHORIZE_TRANSACTION ||
                                                               consumerInformation.TransactionType == Common.CREDIT_TRANSACTION ||
                                                              consumerInformation.TransactionType == Common.SAVE_CARD)
                                                            {
                                                                //If login is success, send CC request to FocusConnect.
                                                                using (CCTransactionRequest ccRequest = new CCTransactionRequest
                                                                {
                                                                    RequestType = Common.CREDIT_CARD_REQUEST_TYPE,
                                                                    UserName = consumerInformation.AttendantId,
                                                                    Password = consumerInformation.Password,
                                                                    SessionID = loginResponse.SessionID,
                                                                    
                                                                    TransactionType = consumerInformation.TransactionType,
                                                                    CreditCardNumber = cardNumber,
                                                                    //ExpiryDate = String.Format("{0}{1}", txtExpiryMonth.Text.Trim(), txtExpiryYear.Text.Trim().Substring(2, 2)),
                                                                    //ExpiryDate = expiryMonth.ToString() + expiryYear.ToString(),
                                                                    ExpiryDate = cardExpiryDate,
                                                                    CVV = txtCvvNumber.Value.Trim(),
                                                                    Name = txtCardName.Value.Trim(),
                                                                    Amount = consumerInformation.Amount,
                                                                    CardPresentIndicator = Common.CP_INDICATOR,
                                                                    TestMode = String.IsNullOrEmpty(consumerInformation.IsTestMode) ? "N" : consumerInformation.IsTestMode,
                                                                    Address = consumerInformation.ConsumerAddress,
                                                                    EmailAddress = consumerInformation.EmailAddress,
                                                                    Notes = consumerInformation.Notes,
                                                                    Tokenize = consumerInformation.Tokenize, //Added by Nazreen                                                        
                                                                    Token = consumerInformation.Token, //Added by Nazreen
                                                                    OrderNumber = consumerInformation.OrderNumber,
                                                                    AdditionalInfo = new Additionalinfo { InvoiceNum = consumerInformation.InvoiceNumber },
                                                                    UDField2 = consumerInformation.UDField2,
                                                                    UDField3 = consumerInformation.UDField3
                                                                })
                                                                {
                                                                    //Serialize the cc request object into Xml format.
                                                                    ccRequestXml = ccRequest.SerializeFocusConnectMessage(ccRequest);
                                                                    Common.LogWrite(DateTime.Now.ToString() + ccRequestXml);
                                                                }

                                                                if (isRequestEnable)
                                                                {
                                                                    Common.LogWrite("Generated transaction xml request." + ccRequestXml);
                                                                }
                                                                else
                                                                {
                                                                    Common.LogWrite("Generated transaction xml request.");
                                                                }
                                                            }
                                                            else if (consumerInformation.TransactionType == Common.PRIORAUTHCAPTURE_TRANSACTION ||
                                                                (consumerInformation.TransactionType == Common.CREDIT_TRANSACTION &&
                                                               !String.IsNullOrEmpty(consumerInformation.TransactionID)) ||
                                                               consumerInformation.TransactionType == Common.REFUND_TRANSACTION ||
                                                               consumerInformation.TransactionType == Common.VOID_TRANSACTION)
                                                            {
                                                                //If login is success, send CC request to FocusConnect.
                                                                using (CCTransactionRequest ccRequest = new CCTransactionRequest
                                                                {
                                                                    RequestType = Common.CREDIT_CARD_REQUEST_TYPE,
                                                                    UserName = consumerInformation.AttendantId,
                                                                    Password = consumerInformation.Password,
                                                                    SessionID = loginResponse.SessionID,

                                                                    TransactionType = consumerInformation.TransactionType,
                                                                    CreditCardNumber = cardNumber,
                                                                    //ExpiryDate = String.Format("{0}{1}", txtExpiryMonth.Text.Trim(), txtExpiryYear.Text.Trim().Substring(2, 2)),
                                                                    //ExpiryDate = expiryMonth.ToString() + expiryYear.ToString(),
                                                                    ExpiryDate = cardExpiryDate,
                                                                    CVV = txtCvvNumber.Value.Trim(),
                                                                    Name = txtCardName.Value.Trim(),
                                                                    Amount = consumerInformation.Amount,
                                                                    GatewayTransactionID = consumerInformation.TransactionID,
                                                                    CardPresentIndicator = Common.CP_INDICATOR,
                                                                    TestMode = String.IsNullOrEmpty(consumerInformation.IsTestMode) ? "N" : consumerInformation.IsTestMode,
                                                                    Address = consumerInformation.ConsumerAddress,
                                                                    EmailAddress = consumerInformation.EmailAddress,
                                                                    Notes = consumerInformation.Notes,
                                                                    Tokenize = string.Empty, //Added by Nazreen
                                                                    Token = consumerInformation.Token, //Added by Nazreen
                                                                    OrderNumber = consumerInformation.OrderNumber,
                                                                    AdditionalInfo = new Additionalinfo { InvoiceNum = consumerInformation.InvoiceNumber },
                                                                    UDField2 = consumerInformation.UDField2,
                                                                    UDField3 = consumerInformation.UDField3
                                                                })
                                                                {
                                                                    //Serialize the cc request object into Xml format.
                                                                    ccRequestXml = ccRequest.SerializeFocusConnectMessage(ccRequest);
                                                                    Common.LogWrite("JetPay Request " + ccRequestXml);
                                                                }

                                                                if (isRequestEnable)
                                                                {
                                                                    Common.LogWrite("Generated transaction xml request." + ccRequestXml);
                                                                }
                                                                else
                                                                {
                                                                    Common.LogWrite("Generated transaction xml request.");
                                                                }
                                                            }

                                                            if (!String.IsNullOrEmpty(ccRequestXml))
                                                            {
                                                                Common.LogWrite("Sending transaction request to connect.");

                                                                //Get the ccTransaction response from FocusConnect.
                                                                Stream ccResponsestream;
                                                                String ccresponseString = string.Empty;
                                                                if (!httpresponsetimeout)
                                                                {

                                                                    try
                                                                    {
                                                                        using (HttpWebResponse connectCcResponse = SendConnectRequest(ccRequestXml))
                                                                        {
                                                                            if (!httpresponsetimeout)
                                                                            {
                                                                                try
                                                                                {
                                                                                    using (ccResponsestream = connectCcResponse.GetResponseStream())
                                                                                    {
                                                                                        StreamReader reader = new StreamReader(ccResponsestream, Encoding.UTF8);
                                                                                        ccresponseString = reader.ReadToEnd();
                                                                                        Common.LogWrite(DateTime.Now.ToString() + ccresponseString);
                                                                                    }
                                                                                }
                                                                                catch (Exception ex)
                                                                                {
                                                                                    Common.LogWrite(" Http Post Exception transaction response." + ex.Message);
                                                                                    checkOutPostData = Common.ConstructCheckOutResponse(12, Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                                                                                                                           string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                                                                           string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                                                    string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                                                    Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                                                    Session["ConnectResponse"] = checkOutPostData;
                                                                                    Response.Redirect("Proxy.aspx", false);
                                                                                }


                                                                                Stream ccstream = GenerateStreamFromString(ccresponseString);
                                                                                using (CCTransactionResponse ccResponse = TF.REVCheckOut.Response.Response.DeserializeFocusConnectMessage(ccstream, typeof(CCTransactionResponse)) as CCTransactionResponse)
                                                                                {

                                                                                    Common.LogWrite("Got the response from gateway.Response Code: " + ccResponse.Code + " Response Message:" + ccResponse.Description);


                                                                                    if (ccResponse != null)
                                                                                    {
                                                                                        Common.LogWrite("Connect response is not null.");

                                                                                        NameValueCollection checkOutPostData = null;

                                                                                        if (ccResponse.Code == 0)
                                                                                        {
                                                                                            Common.LogWrite("Gateway has approved the transaction.");

                                                                                            cardType = Common.GetCardType(txtCardNumber.Value.Trim().Replace(" ", string.Empty));

                                                                                            Common.LogWrite("Generating approved transaction response.");

                                                                                            checkOutPostData = Common.ConstructCheckOutResponse(ccResponse.Code, ccResponse.Description, ccResponse.Amount, ccResponse.GatewayTransactionID, ccResponse.TxnDtTime, ccResponse.CardName,
                                                                                                                                               (!String.IsNullOrEmpty(ccResponse.Token) ? ccResponse.Token : string.Empty), string.Empty, string.Empty,
                                                                                                                                               (lblCheckBox.Visible == true && chkRememberMe.Checked) ? "true" : (lblCheckBox.Visible == false) ? string.Empty : "false",
                                                                                                                                               ccResponse.CreditCardNumber, cardType, string.Empty, string.Empty, string.Empty);
                                                                                            string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                                                            Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                                                            //ccResponse.CreditCardNumber.Substring(ccResponse.CreditCardNumber.LastIndexOf("*") + 1, 4)  
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            Common.LogWrite("Generating failed transaction response.");

                                                                                            checkOutPostData = Common.ConstructCheckOutResponse(ccResponse.Code, ccResponse.Description, ccResponse.Amount, ccResponse.GatewayTransactionID, ccResponse.TxnDtTime,
                                                                                                                                                (!String.IsNullOrEmpty(ccResponse.Token) ? ccResponse.Token : string.Empty), string.Empty, string.Empty, string.Empty,
                                                                                                                                                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                                                            string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                                                            Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                                                        }

                                                                                        Session["ConnectResponse"] = checkOutPostData;
                                                                                        Response.Redirect("Proxy.aspx", false);
                                                                                    }
                                                                                    //Else, redirect to error page.
                                                                                    else
                                                                                    {


                                                                                        checkOutPostData = Common.ConstructCheckOutResponse(12, Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                                                                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                                                        string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                                                        Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                                                        Session["ConnectResponse"] = checkOutPostData;
                                                                                        Response.Redirect("Proxy.aspx", false);
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                checkOutPostData = Common.ConstructCheckOutResponse(12, Common.HTTP_RESPONSE_TIMEOUT, string.Empty, string.Empty,
                                                                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                                                string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                                                Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                                                Session["ConnectResponse"] = checkOutPostData;
                                                                                Response.Redirect("Proxy.aspx", false);
                                                                            }
                                                                        }
                                                                    }
                                                                    //Exception while posting request to Transcation focusconnet and getting Transcation response 
                                                                    catch (HttpException ex)
                                                                    {
                                                                        Common.LogWrite(" Http Post Exception transaction response." + ex.Message);
                                                                        checkOutPostData = Common.ConstructCheckOutResponse(12, Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                                                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                                        string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                                        Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                                        Session["ConnectResponse"] = checkOutPostData;
                                                                        Response.Redirect("Proxy.aspx", false);
                                                                    }
                                                                }
                                                                //else httptimeout response 
                                                            }
                                                            //Else, redirect to error page.
                                                            else
                                                            {
                                                                Response.Redirect("ErrorPage.aspx", false);
                                                            }
                                                        }
                                                        //Send back the login response code and desription to the callback url.
                                                        else
                                                        {
                                                            checkOutPostData = Common.ConstructCheckOutResponse(loginResponse.Code, loginResponse.Description, string.Empty, string.Empty,
                                                                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                            string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                            Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                            Session["ConnectResponse"] = checkOutPostData;
                                                            Response.Redirect("Proxy.aspx", false);
                                                        }
                                                    }
                                                    //Else, redirect to error page.
                                                    else
                                                    {
                                                        Response.Redirect("ErrorPage.aspx", false);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                checkOutPostData = Common.ConstructCheckOutResponse(12, Common.HTTP_RESPONSE_TIMEOUT, string.Empty, string.Empty,                                                                                                                                                                            
                                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                Session["ConnectResponse"] = checkOutPostData;
                                                Response.Redirect("Proxy.aspx", false);
                                            }
                                        }
                                    }
                                    //Exception while posting Login request to focusconnet and getting Login response 
                                    catch (HttpException ex)
                                    {
                                        checkOutPostData = Common.ConstructCheckOutResponse(12, Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                        string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                        Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                        Session["ConnectResponse"] = checkOutPostData;
                                        Response.Redirect("Proxy.aspx", false);
                                    }
                                }
                                    //else part httptimeout
                                }
                                //Else, redirect to error page.
                                else
                                {
                                    Response.Redirect("ErrorPage.aspx", false);
                                }
                            }
                            //Else, redirect to error page.
                            else
                            {
                                Response.Redirect("ErrorPage.aspx", false);
                            }
                        }

                             //Request with APP Key 
                        else 
                        {
                            //Request with APP Key 
                            string ccRequestXml = string.Empty;
                            string focusConnectUrl = System.Configuration.ConfigurationManager.AppSettings["FocusConnectUrl"].Trim();
                            if (consumerInformation.TransactionType == Common.SAVE_CARD)
                            {
                                consumerInformation.TransactionType = Common.AUTHORIZE_TRANSACTION;
                            }
                            if (consumerInformation.TransactionType == Common.SALE_TRANSACTION ||
                               consumerInformation.TransactionType == Common.AUTHORIZE_TRANSACTION ||
                               consumerInformation.TransactionType == Common.CREDIT_TRANSACTION)
                            {
                                //If login is success, send CC request to FocusConnect.
                                using (CCTransactionRequest ccRequest = new CCTransactionRequest
                                {
                                    RequestType = Common.CREDIT_CARD_REQUEST_TYPE,
                                    UserName = consumerInformation.AttendantId,
                                    Password = consumerInformation.Password,
                                    SessionID = consumerInformation.AppKey,

                                    TransactionType = consumerInformation.TransactionType,
                                    CreditCardNumber = cardNumber,
                                    //ExpiryDate = String.Format("{0}{1}", txtExpiryMonth.Text.Trim(), txtExpiryYear.Text.Trim().Substring(2, 2)),
                                    //ExpiryDate = expiryMonth.ToString() + expiryYear.ToString(),
                                    ExpiryDate = cardExpiryDate,
                                    CVV = txtCvvNumber.Value.Trim(),
                                    Name = txtCardName.Value.Trim(),
                                    Amount = consumerInformation.Amount,
                                    CardPresentIndicator = Common.CP_INDICATOR,
                                    TestMode = String.IsNullOrEmpty(consumerInformation.IsTestMode) ? "N" : consumerInformation.IsTestMode,
                                    Address = consumerInformation.ConsumerAddress,
                                    EmailAddress = consumerInformation.EmailAddress,
                                    Notes = consumerInformation.Notes,
                                    Tokenize = consumerInformation.Tokenize, //Added by Nazreen                                                        
                                    Token = consumerInformation.Token, //Added by Nazreen
                                    OrderNumber = consumerInformation.OrderNumber,
                                    AdditionalInfo = new Additionalinfo { InvoiceNum = consumerInformation.InvoiceNumber },
                                    UDField2 = consumerInformation.UDField2,
                                    UDField3 = consumerInformation.UDField3
                                })
                                {
                                    //Serialize the cc request object into Xml format.
                                    ccRequestXml = ccRequest.SerializeFocusConnectMessage(ccRequest);
                                    Common.LogWrite(DateTime.Now.ToString() + ccRequestXml);
                                }

                                if (isRequestEnable)
                                {
                                    Common.LogWrite("Generated transaction xml request." + ccRequestXml);
                                }
                                else
                                {
                                    Common.LogWrite("Generated transaction xml request.");
                                }
                            }
                            else if (consumerInformation.TransactionType == Common.PRIORAUTHCAPTURE_TRANSACTION ||
                                (consumerInformation.TransactionType == Common.CREDIT_TRANSACTION &&
                               !String.IsNullOrEmpty(consumerInformation.TransactionID)) ||
                               consumerInformation.TransactionType == Common.REFUND_TRANSACTION ||
                               consumerInformation.TransactionType == Common.VOID_TRANSACTION)
                            {
                                //If login is success, send CC request to FocusConnect.
                                using (CCTransactionRequest ccRequest = new CCTransactionRequest
                                {
                                    RequestType = Common.CREDIT_CARD_REQUEST_TYPE,
                                    UserName = consumerInformation.AttendantId,
                                    Password = consumerInformation.Password,
                                    SessionID = consumerInformation.AppKey,

                                    TransactionType = consumerInformation.TransactionType,
                                    CreditCardNumber = cardNumber,
                                    //ExpiryDate = String.Format("{0}{1}", txtExpiryMonth.Text.Trim(), txtExpiryYear.Text.Trim().Substring(2, 2)),
                                    //ExpiryDate = expiryMonth.ToString() + expiryYear.ToString(),
                                    ExpiryDate = cardExpiryDate,
                                    CVV = txtCvvNumber.Value.Trim(),
                                    Name = txtCardName.Value.Trim(),
                                    Amount = consumerInformation.Amount,
                                    GatewayTransactionID = consumerInformation.TransactionID,
                                    CardPresentIndicator = Common.CP_INDICATOR,
                                    TestMode = String.IsNullOrEmpty(consumerInformation.IsTestMode) ? "N" : consumerInformation.IsTestMode,
                                    Address = consumerInformation.ConsumerAddress,
                                    EmailAddress = consumerInformation.EmailAddress,
                                    Notes =  consumerInformation.Notes,
                                    Tokenize = string.Empty, //Added by Nazreen
                                    Token = consumerInformation.Token, //Added by Nazreen
                                    OrderNumber = consumerInformation.OrderNumber,
                                    AdditionalInfo = new Additionalinfo { InvoiceNum = consumerInformation.InvoiceNumber },
                                    UDField2 = consumerInformation.UDField2,
                                    UDField3 = consumerInformation.UDField3
                                })
                                {
                                    //Serialize the cc request object into Xml format.
                                    ccRequestXml = ccRequest.SerializeFocusConnectMessage(ccRequest);
                                    Common.LogWrite("JetPay Request " + ccRequestXml);
                                }

                                if (isRequestEnable)
                                {
                                    Common.LogWrite("Generated transaction xml request." + ccRequestXml);
                                }
                                else
                                {
                                    Common.LogWrite("Generated transaction xml request.");
                                }
                            }

                            if (!String.IsNullOrEmpty(ccRequestXml))
                            {
                                Common.LogWrite("Sending transaction request to connect.");

                                //Get the ccTransaction response from FocusConnect.
                                Stream ccResponsestream;
                                String ccresponseString = string.Empty;
                                if(!httpresponsetimeout)
                                {
                                try
                                {
                                    using (HttpWebResponse connectCcResponse = SendConnectRequest(ccRequestXml))
                                    {
                                        if (!httpresponsetimeout)
                                        {
                                            try
                                            {
                                                using (ccResponsestream = connectCcResponse.GetResponseStream())
                                                {
                                                    StreamReader reader = new StreamReader(ccResponsestream, Encoding.UTF8);
                                                    ccresponseString = reader.ReadToEnd();
                                                    Common.LogWrite(DateTime.Now.ToString() + ccresponseString);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Common.LogWrite(" Http Post Exception transaction response." + ex.Message);
                                                checkOutPostData = Common.ConstructCheckOutResponse(12, Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                                                                                       string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                                       string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                Session["ConnectResponse"] = checkOutPostData;
                                                Response.Redirect("Proxy.aspx", false);
                                            }
                                            Stream ccstream = GenerateStreamFromString(ccresponseString);

                                            using (CCTransactionResponse ccResponse = TF.REVCheckOut.Response.Response.DeserializeFocusConnectMessage(ccstream, typeof(CCTransactionResponse)) as CCTransactionResponse)
                                            {

                                                Common.LogWrite("Got the response from gateway.Response Code: " + ccResponse.Code + " Response Message:" + ccResponse.Description);


                                                if (ccResponse != null)
                                                {
                                                    Common.LogWrite("Connect response is not null.");

                                                    NameValueCollection checkOutPostData = null;

                                                    if (ccResponse.Code == 0)
                                                    {
                                                        Common.LogWrite("Gateway has approved the transaction.");

                                                        cardType = Common.GetCardType(txtCardNumber.Value.Trim().Replace(" ", string.Empty));

                                                        Common.LogWrite("Generating approved transaction response.");

                                                        checkOutPostData = Common.ConstructCheckOutResponse(ccResponse.Code, ccResponse.Description, ccResponse.Amount, ccResponse.GatewayTransactionID, ccResponse.TxnDtTime, ccResponse.CardName,
                                                                                                           (!String.IsNullOrEmpty(ccResponse.Token) ? ccResponse.Token : string.Empty), string.Empty, string.Empty,
                                                                                                           (lblCheckBox.Visible == true && chkRememberMe.Checked) ? "true" : (lblCheckBox.Visible == false) ? string.Empty : "false",
                                                                                                           ccResponse.CreditCardNumber, cardType, string.Empty, string.Empty, string.Empty);
                                                        string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                        Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                        //ccResponse.CreditCardNumber.Substring(ccResponse.CreditCardNumber.LastIndexOf("*") + 1, 4)  
                                                    }
                                                    else if (ccResponse.Code == 32 || ccResponse.Code == 10)
                                                    {
                                                        relogin = true;

                                                        NameValueCollection checkOutPostDatarelogin = null;
                                                        Common.LogWrite("Invalid App-Key , Initiating new login transaction");

                                                       
                                                        checkOutPostDatarelogin = Appkeyinvalid();
                                                        Session["ConnectResponse"] = checkOutPostDatarelogin;
                                                        Response.Redirect("Proxy.aspx", false);
                                                       
                                                    }
                                                    else
                                                    {
                                                        Common.LogWrite("Generating failed transaction response.");

                                                        checkOutPostData = Common.ConstructCheckOutResponse(ccResponse.Code, ccResponse.Description, ccResponse.Amount, ccResponse.GatewayTransactionID, ccResponse.TxnDtTime,
                                                                                                            (!String.IsNullOrEmpty(ccResponse.Token) ? ccResponse.Token : string.Empty), string.Empty, string.Empty, string.Empty,
                                                                                                            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                        string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                        Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                    }
                                                    if (!relogin)
                                                    {
                                                        Session["ConnectResponse"] = checkOutPostData;
                                                        Response.Redirect("Proxy.aspx", false);
                                                    }
                                                }
                                                //Else, redirect to error page.
                                                else
                                                {
                                                    Response.Redirect("ErrorPage.aspx", false);
                                                }
                                            }
                                        }
                                       
                                    }
                                }
                                //Exception while posting request to Transcation focusconnet and getting Transcation response 
                                catch (HttpException ex)
                                {
                                    Common.LogWrite(" Http Post Exception transaction response." + ex.Message);
                                    checkOutPostData = Common.ConstructCheckOutResponse(12,Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                                                                           string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                           string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                    string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                    Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                    Session["ConnectResponse"] = checkOutPostData;
                                    Response.Redirect("Proxy.aspx", false);
                                }

                            }

                                else
                                {
                                    Common.LogWrite(" Http Post Exception transaction response.");
                                    checkOutPostData = Common.ConstructCheckOutResponse(12, Common.HTTP_RESPONSE_TIMEOUT, string.Empty, string.Empty,
                                                                                                                           string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                           string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                    string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                    Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                    Session["ConnectResponse"] = checkOutPostData;
                                    Response.Redirect("Proxy.aspx", false);
                                }
                            }
                        }
                        //
                    }
                    //Else, redirect to error page.
                    else
                    {
                        Response.Redirect("ErrorPage.aspx", false);
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

            if (httpresponsetimeout)
            {
                Common.LogWrite(" Http Post Exception transaction response." + ex.Message);
                checkOutPostData = Common.ConstructCheckOutResponse(12, Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                                                       string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                       string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                Session["ConnectResponse"] = checkOutPostData;
                Response.Redirect("Proxy.aspx", false);
            }
            else
            {
                Response.Redirect("ErrorPage.aspx", false);
            }
        }
    }

    #endregion

    #region btnCancel_Click

    /// <summary>
    /// Cancels the current transaction operation.
    /// </summary>
    /// <param name="sender">Page/Control that initiates the event.</param>
    /// <param name="e">Event Arguments.</param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //Redirect the page to the Callback url if the session related to consumer exists.
        if(Session["ConsumerInformation"] != null)
        {
            consumerInformation = Session["ConsumerInformation"] as Consumer;

            NameValueCollection checkOutPostData = Common.ConstructCheckOutResponse(Common.TRANSACTION_CANCEL_CODE, Common.TRANSACTION_CANCEL_MESSAGE,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            Common.LogWrite("Transaction has cancelled.");
            string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
            Common.LogWrite("Response Name Value Collection \r\n" + allValues);
            Session["ConnectResponse"] = checkOutPostData;
            Response.Redirect("Proxy.aspx", false);

            //Response.Redirect(consumerInformation.CallbackUrl, false);
        }
        //Else, redirect to error page.
        else
        {
            Response.Redirect("ErrorPage.aspx", false);
        }
    }

    #endregion


    public Stream GenerateStreamFromString(string s)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(s));
    }

   public void loadDropDownList()
   {
       
      for (int i =1 ; i <= 12; i++) 
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

   #region custValCardNumber_ServerValidate

   /// <summary>
   /// Custom validator to validate the credit card number.
   /// </summary>
   /// <param name="source">Page that initiates the event.</param>
   /// <param name="args">Event Argument that contains the credit card number.</param>
   protected void custValCardNumber_ServerValidate(object source, ServerValidateEventArgs args)
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
    private string GetRequestField(string key, string defaultValue)
    {
        string requestValue;

        if (requestFields.TryGetValue(key, out requestValue))
        {
            return requestValue;
        }
        else
        {
            return defaultValue;
        }
    }

    #endregion          

    #region SendConnectRequest

    /// <summary>
    /// Send the HTTP Post request to the FocusConnect with given payload as post data.
    /// </summary>
    /// <param name="payLoad">HTTP POST data.</param>
    /// <returns>FocusConnect response message.</returns>
    private HttpWebResponse SendConnectRequest(string payLoad)
    {
        string focusConnectUrl = System.Configuration.ConfigurationManager.AppSettings["FocusConnectUrl"].Trim();
        //Create the HTTP POST Request to FocusConnect url and append the required data as post content body.
       
        try
        {HttpWebRequest connectRequest   = (HttpWebRequest)WebRequest.Create(focusConnectUrl);
        connectRequest.Method           = Common.HTTP_METHOD;
        connectRequest.ContentType      = Common.CONTENT_TYPE;
        connectRequest.Timeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings["httptimeout"]);
        byte[] postData = Encoding.UTF8.GetBytes(payLoad);
        connectRequest.ContentLength = postData.Length;

        //Append the post data.
        using (Stream requestStream = connectRequest.GetRequestStream())
        {
            requestStream.Write(postData, 0, postData.Length);
        }

        //Send back the response from FocusConnect.
        return connectRequest.GetResponse() as HttpWebResponse;
        }
        catch(WebException ex)
        {
            if(ex.Status == WebExceptionStatus.Timeout)
            {
                Common.LogWrite("Http Response Timeout");
                httpresponsetimeout = true;
                return null;
            }
            return null;
        }
        
    }

    #endregion 
    

    public NameValueCollection Appkeyinvalid()
    {
        {
            int expiryMonth = Convert.ToInt32(hdnMonth.Value);
            int expiryYear = Convert.ToInt32(hdnYear.Value);
            int expiryYearLasttwo = Convert.ToInt32(hdnYear.Value.Substring(2, 2));
            int currentMonth = DateTime.Now.Month;
            string currentYear = DateTime.Now.Year.ToString();
            string loginRequestXml = string.Empty;
            string cardNumber = txtCardNumber.Value.Trim().Replace(" ", string.Empty);
            string cardExpiryDate = Common.GetCardExpiryDate(expiryYearLasttwo, expiryMonth);
            bool isRequestEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsRequestLog"]);
            string sessionID_relogin = string.Empty;

            focusConnectUrl = ConfigurationManager.AppSettings["FocusConnectUrl"].Trim();

            //Create the FocusConnect login request with the help of consumer information details.
            using (LoginRequest loginRequest = new LoginRequest
            {
                RequestType = Common.LOGIN_REQUEST_TYPE,
                UserName = consumerInformation.AttendantId,
                Password = consumerInformation.Password

            })
            {
                //Serialize the login request object into Xml format.
                loginRequestXml = loginRequest.SerializeFocusConnectMessage(loginRequest);
            }

            if (isRequestEnable)
            {
                Common.LogWrite("Generated login xml request." + loginRequestXml);
            }
            else
            {
                Common.LogWrite("Generated login xml request.");
            }

            if (!String.IsNullOrEmpty(loginRequestXml))
            {
                //Get the FocusConnect endpoint from configuration file.

                //If FocusConnect endpoint exists.
                if (!String.IsNullOrEmpty(focusConnectUrl))
                {
                    Common.LogWrite("Connect Url is not empty.");

                    Common.LogWrite("Sending login request to connect.");



                    //Get the login response from FocusConnect.
                    Stream Responsestream;
                    String responseString = string.Empty;
                    if (!httpresponsetimeout)
                    {
                        try
                        {
                            using (HttpWebResponse connectLoginResponse = SendConnectRequest(loginRequestXml))
                            {
                                if (!httpresponsetimeout)
                                {
                                    //Deserialize to LoginResponse object.
                                    try
                                    {

                                        using (Responsestream = connectLoginResponse.GetResponseStream())
                                        {
                                            StreamReader reader = new StreamReader(Responsestream, Encoding.UTF8);
                                            responseString = reader.ReadToEnd();
                                            Common.LogWrite(DateTime.Now.ToString() + responseString);
                                        }
                                    }

                                    catch (Exception ex)
                                    {
                                        if (httpresponsetimeout)
                                        {
                                            Common.LogWrite(" Http Post Exception transaction response." + ex.Message);
                                            checkOutPostData = Common.ConstructCheckOutResponse(12, Common.HTTP_RESPONSE_TIMEOUT, string.Empty, string.Empty,
                                                                                                                                   string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                                   string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                            string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                            Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                            Session["ConnectResponse"] = checkOutPostData;
                                            Response.Redirect("Proxy.aspx", false);
                                        }
                                        else
                                        {
                                            Common.LogWrite(" Http Post Exception transaction response." + ex.Message);
                                            checkOutPostData = Common.ConstructCheckOutResponse(12, Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                                                                                   string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                                   string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                            string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                            Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                            Session["ConnectResponse"] = checkOutPostData;
                                            Response.Redirect("Proxy.aspx", false);
                                        }

                                    }


                                    Stream stream = GenerateStreamFromString(responseString);

                                    using (LoginResponse loginResponse = TF.REVCheckOut.Response.Response.DeserializeFocusConnectMessage(stream, typeof(LoginResponse)) as LoginResponse)
                                    {
                                        Common.LogWrite("Got the response from gateway.");

                                        if (loginResponse != null)
                                        {
                                            Common.LogWrite("Connect response is not null.");

                                            //Check whether the login is successful or not.
                                            if (loginResponse.Code == 0)
                                            {
                                                sessionID_relogin = loginResponse.SessionID;
                                                Common.LogWrite("Gateway has approved the transaction.");

                                                string ccRequestXml = string.Empty;
                                                if (consumerInformation.TransactionType == Common.SAVE_CARD)
                                                {
                                                    consumerInformation.TransactionType = Common.AUTHORIZE_TRANSACTION;
                                                }
                                                if (consumerInformation.TransactionType == Common.SALE_TRANSACTION ||
                                                   consumerInformation.TransactionType == Common.AUTHORIZE_TRANSACTION ||
                                                   consumerInformation.TransactionType == Common.CREDIT_TRANSACTION)
                                                {
                                                    //If login is success, send CC request to FocusConnect.
                                                    using (CCTransactionRequest ccRequest = new CCTransactionRequest
                                                    {
                                                        RequestType = Common.CREDIT_CARD_REQUEST_TYPE,
                                                        UserName = consumerInformation.AttendantId,
                                                        Password = consumerInformation.Password,
                                                        SessionID = loginResponse.SessionID,

                                                        TransactionType = consumerInformation.TransactionType,
                                                        CreditCardNumber = cardNumber,
                                                        //ExpiryDate = String.Format("{0}{1}", txtExpiryMonth.Text.Trim(), txtExpiryYear.Text.Trim().Substring(2, 2)),
                                                        //ExpiryDate = expiryMonth.ToString() + expiryYear.ToString(),
                                                        ExpiryDate = cardExpiryDate,
                                                        CVV = txtCvvNumber.Value.Trim(),
                                                        Name = txtCardName.Value.Trim(),
                                                        Amount = consumerInformation.Amount,
                                                        CardPresentIndicator = Common.CP_INDICATOR,
                                                        TestMode = String.IsNullOrEmpty(consumerInformation.IsTestMode) ? "N" : consumerInformation.IsTestMode,
                                                        Address = consumerInformation.ConsumerAddress,
                                                        EmailAddress = consumerInformation.EmailAddress,
                                                        Notes = consumerInformation.Notes,
                                                        Tokenize = consumerInformation.Tokenize, //Added by Nazreen                                                        
                                                        Token = consumerInformation.Token, //Added by Nazreen
                                                        OrderNumber = consumerInformation.OrderNumber,
                                                        AdditionalInfo = new Additionalinfo { InvoiceNum = consumerInformation.InvoiceNumber },
                                                        UDField2 = consumerInformation.UDField2,
                                                        UDField3 = consumerInformation.UDField3

                                                    })
                                                    {
                                                        //Serialize the cc request object into Xml format.
                                                        ccRequestXml = ccRequest.SerializeFocusConnectMessage(ccRequest);
                                                    }

                                                    if (isRequestEnable)
                                                    {
                                                        Common.LogWrite("Generated transaction xml request." + ccRequestXml);
                                                    }
                                                    else
                                                    {
                                                        Common.LogWrite("Generated transaction xml request.");
                                                    }
                                                }
                                                else if (consumerInformation.TransactionType == Common.PRIORAUTHCAPTURE_TRANSACTION ||
                                                    (consumerInformation.TransactionType == Common.CREDIT_TRANSACTION &&
                                                   !String.IsNullOrEmpty(consumerInformation.TransactionID)) ||
                                                   consumerInformation.TransactionType == Common.REFUND_TRANSACTION ||
                                                   consumerInformation.TransactionType == Common.VOID_TRANSACTION)
                                                {
                                                    //If login is success, send CC request to FocusConnect.
                                                    using (CCTransactionRequest ccRequest = new CCTransactionRequest
                                                    {
                                                        RequestType = Common.CREDIT_CARD_REQUEST_TYPE,
                                                        UserName = consumerInformation.AttendantId,
                                                        Password = consumerInformation.Password,
                                                        SessionID = loginResponse.SessionID,

                                                        TransactionType = consumerInformation.TransactionType,
                                                        CreditCardNumber = cardNumber,
                                                        //ExpiryDate = String.Format("{0}{1}", txtExpiryMonth.Text.Trim(), txtExpiryYear.Text.Trim().Substring(2, 2)),
                                                        //ExpiryDate = expiryMonth.ToString() + expiryYear.ToString(),
                                                        ExpiryDate = cardExpiryDate,
                                                        CVV = txtCvvNumber.Value.Trim(),
                                                        Name = txtCardName.Value.Trim(),
                                                        Amount = consumerInformation.Amount,
                                                        GatewayTransactionID = consumerInformation.TransactionID,
                                                        CardPresentIndicator = Common.CP_INDICATOR,
                                                        TestMode = String.IsNullOrEmpty(consumerInformation.IsTestMode) ? "N" : consumerInformation.IsTestMode,
                                                        Address = consumerInformation.ConsumerAddress,
                                                        EmailAddress = consumerInformation.EmailAddress,
                                                        Notes = consumerInformation.Notes,
                                                        Tokenize = string.Empty, //Added by Nazreen
                                                        Token = consumerInformation.Token, //Added by Nazreen
                                                        OrderNumber = consumerInformation.OrderNumber,
                                                        AdditionalInfo = new Additionalinfo { InvoiceNum = consumerInformation.InvoiceNumber },
                                                        UDField2 = consumerInformation.UDField2,
                                                        UDField3 = consumerInformation.UDField3
                                                    })
                                                    {
                                                        //Serialize the cc request object into Xml format.
                                                        ccRequestXml = ccRequest.SerializeFocusConnectMessage(ccRequest);
                                                    }

                                                    if (isRequestEnable)
                                                    {
                                                        Common.LogWrite("Generated transaction xml request." + ccRequestXml);
                                                    }
                                                    else
                                                    {
                                                        Common.LogWrite("Generated transaction xml request.");
                                                    }
                                                }

                                                if (!String.IsNullOrEmpty(ccRequestXml))
                                                {
                                                    Common.LogWrite("Sending transaction request to connect.");
                                                    if (!httpresponsetimeout)
                                                    {
                                                        //Get the ccTransaction response from FocusConnect.
                                                        Stream ccResponsestream;
                                                        String ccresponseString = string.Empty;
                                                        try
                                                        {
                                                            using (HttpWebResponse connectCcResponse = SendConnectRequest(ccRequestXml))
                                                            {
                                                                if (!httpresponsetimeout)
                                                                {
                                                                    try
                                                                    {
                                                                        using (ccResponsestream = connectCcResponse.GetResponseStream())
                                                                        {
                                                                            StreamReader reader = new StreamReader(ccResponsestream, Encoding.UTF8);
                                                                            ccresponseString = reader.ReadToEnd();
                                                                            Common.LogWrite(DateTime.Now.ToString() + ccresponseString);
                                                                        }
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        Common.LogWrite(" Http Post Exception transaction response." + ex.Message);
                                                                        checkOutPostData = Common.ConstructCheckOutResponse(12, Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                                                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                                        string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                                        Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                                        Session["ConnectResponse"] = checkOutPostData;
                                                                        Response.Redirect("Proxy.aspx", false);
                                                                    }


                                                                    Stream ccstream = GenerateStreamFromString(ccresponseString);

                                                                    using (CCTransactionResponse ccResponse = TF.REVCheckOut.Response.Response.DeserializeFocusConnectMessage(ccstream, typeof(CCTransactionResponse)) as CCTransactionResponse)
                                                                    {
                                                                        Common.LogWrite("Got the response from gateway.");

                                                                        Common.LogWrite("Got the response from gateway.Response Code: " + ccResponse.Code + " Response Message:" + ccResponse.Description);


                                                                        if (ccResponse != null)
                                                                        {
                                                                            Common.LogWrite("Connect response is not null.");



                                                                            if (ccResponse.Code == 0)
                                                                            {
                                                                                Common.LogWrite("Gateway has approved the transaction.");

                                                                                cardType = Common.GetCardType(txtCardNumber.Value.Trim().Replace(" ", string.Empty));

                                                                                Common.LogWrite("Generating approved transaction response.");

                                                                                checkOutPostDatarelogin = Common.ConstructCheckOutResponse(ccResponse.Code, ccResponse.Description, ccResponse.Amount, ccResponse.GatewayTransactionID, ccResponse.TxnDtTime, ccResponse.CardName,
                                                                                                                                   (!String.IsNullOrEmpty(ccResponse.Token) ? ccResponse.Token : string.Empty), string.Empty, string.Empty,
                                                                                                                                   (lblCheckBox.Visible == true && chkRememberMe.Checked) ? "true" : (lblCheckBox.Visible == false) ? string.Empty : "false",
                                                                                                                                   ccResponse.CreditCardNumber, cardType, string.Empty, string.Empty, sessionID_relogin);
                                                                                string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                                                Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                                                //ccResponse.CreditCardNumber.Substring(ccResponse.CreditCardNumber.LastIndexOf("*") + 1, 4)  
                                                                            }
                                                                            else
                                                                            {
                                                                                Common.LogWrite("Generating failed transaction response.");

                                                                                checkOutPostDatarelogin = Common.ConstructCheckOutResponse(ccResponse.Code, ccResponse.Description, ccResponse.Amount, ccResponse.GatewayTransactionID, ccResponse.TxnDtTime,
                                                                                                                                    (!String.IsNullOrEmpty(ccResponse.Token) ? ccResponse.Token : string.Empty), string.Empty, string.Empty, string.Empty,
                                                                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                                                string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                                                Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                                            }

                                                                            Session["ConnectResponse"] = checkOutPostDatarelogin;
                                                                            Response.Redirect("Proxy.aspx", false);
                                                                        }
                                                                        //Else, redirect to error page.
                                                                        else
                                                                        {
                                                                            Response.Redirect("ErrorPage.aspx", false);
                                                                        }
                                                                    }

                                                                    //
                                                                }

                                                                else
                                                                {
                                                                    checkOutPostDatarelogin = Common.ConstructCheckOutResponse(12, Common.HTTP_RESPONSE_TIMEOUT, string.Empty, string.Empty,
                                                                                                             string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                             string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                                    string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                                    Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                                    Session["ConnectResponse"] = checkOutPostData;
                                                                    Response.Redirect("Proxy.aspx", false);
                                                                }

                                                            }
                                                        }
                                                        catch (HttpException ex)
                                                        {
                                                            Common.LogWrite(" Http Post Exception transaction response." + ex.Message);
                                                            checkOutPostDatarelogin = Common.ConstructCheckOutResponse(12, Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                                                                                                   string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                                                   string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                            string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                            Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                            Session["ConnectResponse"] = checkOutPostData;
                                                            Response.Redirect("Proxy.aspx", false);
                                                        }
                                                    }
                                                    //
                                                }
                                                //Else, redirect to error page.
                                                else
                                                {
                                                    Response.Redirect("ErrorPage.aspx", false);
                                                }
                                            }
                                            //Send back the login response code and desription to the callback url.
                                            else
                                            {
                                                checkOutPostDatarelogin = Common.ConstructCheckOutResponse(loginResponse.Code, loginResponse.Description, string.Empty, string.Empty,
                                                                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                                Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                                Session["ConnectResponse"] = checkOutPostData;
                                                Response.Redirect("Proxy.aspx", false);
                                            }
                                        }
                                        //Else, redirect to error page.
                                        else
                                        {
                                            Response.Redirect("ErrorPage.aspx", false);
                                        }
                                    }
                                    /////
                                }
                                else
                                {
                                    checkOutPostDatarelogin = Common.ConstructCheckOutResponse(12, Common.HTTP_RESPONSE_TIMEOUT, string.Empty, string.Empty,
                                                                                                          string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                          string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                    string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                                    Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                                    Session["ConnectResponse"] = checkOutPostData;
                                    Response.Redirect("Proxy.aspx", false);
                                }

                            }
                        }
                        catch (HttpException ex)
                        {
                            checkOutPostDatarelogin = Common.ConstructCheckOutResponse(12, Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                                                              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                            string allValues = string.Join(System.Environment.NewLine, checkOutPostData.AllKeys.Select(key => checkOutPostData[key]));
                            Common.LogWrite("Response Name Value Collection \r\n" + allValues);
                            Session["ConnectResponse"] = checkOutPostData;
                            Response.Redirect("Proxy.aspx", false);
                        }
                    }
                    /////
                }
                //Else, redirect to error page.
                else
                {
                    Response.Redirect("ErrorPage.aspx", false);
                }
            }
            //Else, redirect to error page.
            else
            {
                Response.Redirect("ErrorPage.aspx", false);
            }
        }
        return checkOutPostDatarelogin;
    }

    #region Dispose

    /// <summary>
    /// Implementation of dispose to free resources.
    /// </summary>
    /// <param name="disposedStatus">The status of the disposed operation.</param>
    protected virtual void Dispose(bool disposedStatus)
    {
        if (!isDisposed)
        {
            isDisposed = true;

            //Released unmanaged resources.
            if (disposedStatus)
            {
                //Release the managed resources.                

                //Nullify the consumerInformation.
                if (consumerInformation != null)
                {
                    consumerInformation = null;
                }

                //Nullify the requestFields.
                if (requestFields != null)
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
    public new void Dispose()
    {
        //True is passed in dispose method to clean managed resources.
        Dispose(true);

        //If dispose is called already, then inform GC to skip finalize on this instance.
        GC.SuppressFinalize(this);
    }

    #endregion

    #endregion




   
}

#endregion