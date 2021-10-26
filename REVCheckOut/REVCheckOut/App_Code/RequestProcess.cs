using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using TF.REVCheckOut;
using TF.REVCheckOut.Request;
using TF.REVCheckOut.Response;


/// <summary>
/// Summary description for RequestXml
/// </summary>
public static class RequestProcess
{
     //FocusConnect url to process the transaction.
    private static string focusConnectUrl = string.Empty;
    //HTTP Post data fields.
    private static SortedList<string, string> requestFields = null;

    //Consumer information.
    private static Consumer consumerInformation = null;

  private static  bool httpresponsetimeout = false;

    public static string ResponseData (string strRequestData)
    {
        string strResponse = string.Empty;
        bool isValid = false;  
        bool isValidCurrency = false;
        string currencySymbol = string.Empty;
        string loginRequestXml = string.Empty;
        bool isRequestEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsRequestLog"]);
        
        if (!String.IsNullOrEmpty(strRequestData))
        {
            Common.LogWrite(strRequestData);
            string[] requestDataCollection = strRequestData.Split('&');


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

                //Construct the consumer object.
                consumerInformation = new Consumer
                {
                    AttendantId = GetRequestField(Common.AID, ""),
                    Password = GetRequestField(Common.PWD, ""),
                    EmailAddress = GetRequestField(Common.EMAIL, ""),
                    Amount = GetRequestField(Common.AMOUNT, ""),
                    Currency = GetRequestField(Common.CURRENCY, ""),
                    IsTestMode = GetRequestField(Common.TEST_MODE, ""),
                    Token = GetRequestField(Common.TOKEN, ""),
                    TransactionID = GetRequestField(Common.REQUEST_TRANSACTIONID, ""),
                    TransactionType = Convert.ToInt32(GetRequestField(Common.TRANSACTION_TYPE, "")),
                    InvoiceNumber = GetRequestField(Common.INVOICE_NUMBER, ""),
                    OrderNumber = GetRequestField(Common.ORDER_NUMBER, ""),
                    ConsumerName = GetRequestField(Common.NAME, ""),
                    CardNumber = GetRequestField(Common.CARD_NUMBER, ""),
                    AppKey = GetRequestField(Common.APP_KEY, ""),
                    Notes = GetRequestField(Common.NOTES, ""),
                    UDField2 = GetRequestField(Common.UDField2, ""),
                    UDField3 = GetRequestField(Common.CustomerNo, "")
                };
            }

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

            //Check Token.
            if (!String.IsNullOrEmpty(consumerInformation.Token))
            {
                Common.LogWrite("Got the Token value.");
            }
            //Else
            else
            {
                Common.LogWrite("Token value is null.");

                isValid = true;
            }

            //Check the Transaction Amount value to the checkout page.
            if (!String.IsNullOrEmpty(consumerInformation.Amount))
            {
                decimal amount = Common.GetDecimalMoneyValue(consumerInformation.Amount);

                string transcationAmount = string.Empty;

                if (!amount.ToString().Contains('.'))
                    transcationAmount = string.Format("{0}.00", amount.ToString());
                else
                    transcationAmount = amount.ToString();

                Common.LogWrite("Got the Transaction Amount.");
            }
            //Else
            else
            {
                Common.LogWrite("Transaction Amount is null.");

                isValid = true;
            }

            //Assign the currency value to the checkout page.
            if (!String.IsNullOrEmpty(consumerInformation.Currency))
            {
                bool isValidCurr = Common.Validate_Currency(consumerInformation.Currency, ref currencySymbol);

                if (isValidCurr)
                {
                    Common.LogWrite("Got the currency type.");
                }
                else
                {
                    Common.LogWrite("Invalid currency type.");

                    isValidCurrency = true;
                }
            }
            //Else
            else
            {
                Common.LogWrite("Currency type is null.");

                isValid = true;
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

            if (isValidCurrency)
            {
                Common.LogWrite("Invalid Currency Type.");

                strResponse = Common.INVALID_CURRENCY_CODE_ERROR_MESSAGE;
            }
            else if (isValid)
            {
                Common.LogWrite("Insufficient Parameters.");

                strResponse = Common.INSUFFICIENT_PARAMETERS_ERROR_MESSAGE;
            }

            else
            {

                if (consumerInformation.AppKey == string.Empty)
                //Create the FocusConnect login request with the help of consumer information details.
                {
                    using (LoginRequest loginRequest = new LoginRequest
                    {
                        RequestType = Common.LOGIN_REQUEST_TYPE,
                        UserName = consumerInformation.AttendantId,
                        Password = consumerInformation.Password
                    })
                    {
                        //Serialize the login request object into Xml format.
                        loginRequestXml = loginRequest.SerializeFocusConnectMessage(loginRequest);

                        if (isRequestEnable)
                        {
                            Common.LogWrite("Generated login xml request." + loginRequestXml);
                        }
                        else
                        {
                            Common.LogWrite("Generated login xml request.");
                        }
                    }

                    if (!String.IsNullOrEmpty(loginRequestXml))
                    {
                        //Get the FocusConnect endpoint from configuration file.
                        focusConnectUrl = ConfigurationManager.AppSettings["FocusConnectUrl"].Trim();

                        //If FocusConnect endpoint exists.
                        if (!String.IsNullOrEmpty(focusConnectUrl))
                        {
                            //Get the login response from FocusConnect.
                            try
                            {
                                using (HttpWebResponse connectLoginResponse = SendConnectRequest(loginRequestXml))
                                {
                                    if(!httpresponsetimeout)
                                    {
                                    //Deserialize to LoginResponse object.
                                    using (LoginResponse loginResponse = TF.REVCheckOut.Response.Response.DeserializeFocusConnectMessage(connectLoginResponse.GetResponseStream(), typeof(LoginResponse)) as LoginResponse)
                                    {
                                        if (loginResponse != null)
                                        {
                                            //Check whether the login is successful or not.
                                            if (loginResponse.Code == 0)
                                            {
                                                string ccRequestXml = string.Empty;

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
                                                        CreditCardNumber = consumerInformation.CardNumber,
                                                        ExpiryDate = string.Empty,
                                                        CVV = string.Empty,



                                                        Name = consumerInformation.ConsumerName,

                                                        Amount = consumerInformation.Amount,

                                                        CardPresentIndicator = Common.CP_INDICATOR,
                                                        TestMode = String.IsNullOrEmpty(consumerInformation.IsTestMode) ? "N" : consumerInformation.IsTestMode,
                                                        Address = string.Empty,
                                                        EmailAddress = consumerInformation.EmailAddress,
                                                        Notes = consumerInformation.Notes,
                                                        Tokenize = string.Empty,
                                                        Token = consumerInformation.Token,
                                                        OrderNumber = consumerInformation.OrderNumber,
                                                        AdditionalInfo = new Additionalinfo { InvoiceNum = consumerInformation.InvoiceNumber },
                                                        UDField2 = GetRequestField(Common.UDField2, ""),
                                                        UDField3 = GetRequestField(Common.CustomerNo, "")
                                                    })
                                                    {
                                                        //Serialize the cc request object into Xml format.
                                                        ccRequestXml = ccRequest.SerializeFocusConnectMessage(ccRequest);
                                                        Common.LogWrite(ccRequestXml);
                                                        if (isRequestEnable)
                                                        {
                                                            Common.LogWrite("Generated transaction xml request." + ccRequestXml);
                                                        }
                                                        else
                                                        {
                                                            Common.LogWrite("Generated transaction xml request.");
                                                        }
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
                                                        CreditCardNumber = consumerInformation.CardNumber,
                                                        ExpiryDate = string.Empty,
                                                        CVV = string.Empty,
                                                        Name = consumerInformation.ConsumerName,

                                                        Amount = consumerInformation.Amount,
                                                        GatewayTransactionID = consumerInformation.TransactionID,
                                                        CardPresentIndicator = Common.CP_INDICATOR,
                                                        TestMode = String.IsNullOrEmpty(consumerInformation.IsTestMode) ? "N" : consumerInformation.IsTestMode,
                                                        Address = string.Empty,
                                                        EmailAddress = consumerInformation.EmailAddress,
                                                        Notes = consumerInformation.Notes,
                                                        Tokenize = string.Empty,
                                                        Token = consumerInformation.Token,
                                                        OrderNumber = consumerInformation.OrderNumber,
                                                        AdditionalInfo = new Additionalinfo { InvoiceNum = consumerInformation.InvoiceNumber },
                                                        UDField2 = GetRequestField(Common.UDField2, ""),
                                                        UDField3 = GetRequestField(Common.CustomerNo, "")
                                                    })
                                                    {
                                                        //Serialize the cc request object into Xml format.
                                                        ccRequestXml = ccRequest.SerializeFocusConnectMessage(ccRequest);
                                                        Common.LogWrite(ccRequestXml);
                                                        if (isRequestEnable)
                                                        {
                                                            Common.LogWrite("Generated transaction xml request." + ccRequestXml);
                                                        }
                                                        else
                                                        {
                                                            Common.LogWrite("Generated transaction xml request.");
                                                        }
                                                    }
                                                }
                                                if (!String.IsNullOrEmpty(ccRequestXml))
                                                {
                                                    //Get the ccTransaction response from FocusConnect.
                                                    if (!httpresponsetimeout)
                                                    {
                                                        try
                                                        {
                                                            using (HttpWebResponse connectCcResponse = SendConnectRequest(ccRequestXml))
                                                            {
                                                                if (!httpresponsetimeout)
                                                                {
                                                                    using (CCTransactionResponse ccResponse = TF.REVCheckOut.Response.Response.DeserializeFocusConnectMessage(connectCcResponse.GetResponseStream(), typeof(CCTransactionResponse)) as CCTransactionResponse)
                                                                    {

                                                                        Common.LogWrite("Got the response from gateway.Response Code: " + ccResponse.Code + " Response Message:" + ccResponse.Description);


                                                                        if (ccResponse != null)
                                                                        {
                                                                            if (ccResponse.Code == 0)
                                                                            {


                                                                                strResponse = Common.ConstructNameValueCheckOutResponse(ccResponse.Code.ToString(), ccResponse.Description,
                                                                                                                                        ccResponse.Amount, ccResponse.GatewayTransactionID, ccResponse.TxnDtTime,
                                                                                                                                         string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                                         !String.IsNullOrEmpty(ccResponse.CreditCardNumber) ? ccResponse.CreditCardNumber : string.Empty,
                                                                                                                                         !String.IsNullOrEmpty(ccResponse.CardName) ? ccResponse.CardName : string.Empty,
                                                                                                                                         string.Empty, string.Empty, string.Empty, string.Empty);


                                                                            }
                                                                            else
                                                                            {
                                                                                //strResponse = ccResponse.Description;
                                                                                strResponse = Common.ConstructNameValueCheckOutResponse(ccResponse.Code.ToString(), ccResponse.Description, string.Empty,
                                                                                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                                            }
                                                                        }
                                                                        //Else If no response from connect
                                                                        else
                                                                        {
                                                                            strResponse = "Gateway time out error.";
                                                                        }
                                                                    }
                                                                    //
                                                                }
                                                                    //http Response Timeout
                                                                else
                                                                {
                                                                    strResponse = Common.ConstructNameValueCheckOutResponse("12", Common.HTTP_RESPONSE_TIMEOUT, string.Empty,
                                                                                                     string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                     string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                               

                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            strResponse = Common.ConstructNameValueCheckOutResponse("12", Common.EXCEPTION_MESSAGE, string.Empty,
                                                                                                              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                            Common.LogWrite("Response Name Value Collection \r\n" + ex.Message + ex.StackTrace);
                                                        }
                                                    }
                                                    ///HERE
                                                }
                                                //Else xml is null
                                                else
                                                {
                                                    strResponse = "Invalid Request Type.";
                                                }
                                            }
                                            //Else Login failed
                                            else
                                            {
                                                strResponse = "Invalid Request Type.";
                                            }
                                        }
                                        //Else Login failed 
                                        else
                                        {
                                            strResponse = "Invalid Request Type.";
                                        }
                                    }
                                    }

                                        //Http response Timeout
                                    else
                                    {
                                        strResponse = Common.ConstructNameValueCheckOutResponse("12", Common.HTTP_RESPONSE_TIMEOUT, string.Empty,
                                                                                                      string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                      string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                               

                                    }
                                    
                                }
                                ////
                            }
                            catch (Exception ex)
                            {
                                strResponse = Common.ConstructNameValueCheckOutResponse("12", Common.EXCEPTION_MESSAGE, string.Empty,
                                                                                                      string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                      string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                Common.LogWrite("Response Name Value Collection \r\n" + ex.Message + ex.StackTrace);
                            }
                            /////here**********
                        
                        }
                        //Else Connect url found
                        else
                        {
                            strResponse = "408 Request Timeout.";
                        }
                    }
                    //Else, posted parameters is not proper
                    else
                    {
                        strResponse = "400 Bad Request.";
                    }
                }
                else
                {
                    
                        string ccRequestXml = string.Empty;

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
                                CreditCardNumber = consumerInformation.CardNumber,
                                ExpiryDate = string.Empty,
                                CVV = string.Empty,



                                Name = consumerInformation.ConsumerName,

                                Amount = consumerInformation.Amount,

                                CardPresentIndicator = Common.CP_INDICATOR,
                                TestMode = String.IsNullOrEmpty(consumerInformation.IsTestMode) ? "N" : consumerInformation.IsTestMode,
                                Address = string.Empty,
                                EmailAddress = consumerInformation.EmailAddress,
                                Notes = consumerInformation.Notes,
                                Tokenize = string.Empty,
                                Token = consumerInformation.Token,
                                OrderNumber = consumerInformation.OrderNumber,
                                AdditionalInfo = new Additionalinfo { InvoiceNum = consumerInformation.InvoiceNumber },
                                UDField2 = GetRequestField(Common.UDField2, ""),
                                UDField3 = GetRequestField(Common.CustomerNo, "")
                            })
                            {
                                //Serialize the cc request object into Xml format.
                                ccRequestXml = ccRequest.SerializeFocusConnectMessage(ccRequest);
                                Common.LogWrite(ccRequestXml);
                                if (isRequestEnable)
                                {
                                    Common.LogWrite("Generated transaction xml request." + ccRequestXml);
                                }
                                else
                                {
                                    Common.LogWrite("Generated transaction xml request.");
                                }
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
                                CreditCardNumber = consumerInformation.CardNumber,
                                ExpiryDate = string.Empty,
                                CVV = string.Empty,
                                Name = consumerInformation.ConsumerName,

                                Amount = consumerInformation.Amount,
                                GatewayTransactionID = consumerInformation.TransactionID,
                                CardPresentIndicator = Common.CP_INDICATOR,
                                TestMode = String.IsNullOrEmpty(consumerInformation.IsTestMode) ? "N" : consumerInformation.IsTestMode,
                                Address = string.Empty,
                                EmailAddress = consumerInformation.EmailAddress,
                                Notes = consumerInformation.Notes,
                                Tokenize = string.Empty,
                                Token = consumerInformation.Token,
                                OrderNumber = consumerInformation.OrderNumber,
                                AdditionalInfo = new Additionalinfo { InvoiceNum = consumerInformation.InvoiceNumber },
                                UDField2 = GetRequestField(Common.UDField2, ""),
                                UDField3 = GetRequestField(Common.CustomerNo, "")

                            })
                            {
                                //Serialize the cc request object into Xml format.
                                ccRequestXml = ccRequest.SerializeFocusConnectMessage(ccRequest);
                                Common.LogWrite(ccRequestXml);
                                if (isRequestEnable)
                                {
                                    Common.LogWrite("Generated transaction xml request." + ccRequestXml);
                                }
                                else
                                {
                                    Common.LogWrite("Generated transaction xml request.");
                                }
                            }
                        }
                        if (!String.IsNullOrEmpty(ccRequestXml))
                        {
                            //Get the ccTransaction response from FocusConnect.
                            if(!httpresponsetimeout)
                            {
                            try
                            {
                                using (HttpWebResponse connectCcResponse = SendConnectRequest(ccRequestXml))
                                {
                                    if (!httpresponsetimeout)
                                    {
                                        using (CCTransactionResponse ccResponse = TF.REVCheckOut.Response.Response.DeserializeFocusConnectMessage(connectCcResponse.GetResponseStream(), typeof(CCTransactionResponse)) as CCTransactionResponse)
                                        {

                                            Common.LogWrite("Got the response from gateway.Response Code: " + ccResponse.Code + " Response Message:" + ccResponse.Description);


                                            if (ccResponse != null)
                                            {
                                                if (ccResponse.Code == 0)
                                                {


                                                    strResponse = Common.ConstructNameValueCheckOutResponse(ccResponse.Code.ToString(), ccResponse.Description,
                                                                                                            ccResponse.Amount, ccResponse.GatewayTransactionID, ccResponse.TxnDtTime,
                                                                                                             string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                             !String.IsNullOrEmpty(ccResponse.CreditCardNumber) ? ccResponse.CreditCardNumber : string.Empty,
                                                                                                             !String.IsNullOrEmpty(ccResponse.CardName) ? ccResponse.CardName : string.Empty,
                                                                                                             string.Empty, string.Empty, string.Empty, string.Empty);


                                                }
                                                else if (ccResponse.Code == 32 || ccResponse.Code == 10)
                                                {
                                                    strResponse = InvalidAppKey();
                                                }
                                                else
                                                {
                                                    //strResponse = ccResponse.Description;
                                                    strResponse = Common.ConstructNameValueCheckOutResponse(ccResponse.Code.ToString(), ccResponse.Description, string.Empty,
                                                                                                            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                }
                                            }
                                            //Else If no response from connect
                                            else
                                            {
                                                strResponse = "Gateway time out error.";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        strResponse = Common.ConstructNameValueCheckOutResponse("12", Common.HTTP_RESPONSE_TIMEOUT, string.Empty,
                                                                                                            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                            string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                    }
                                    //else http timeout
                                }
                            }
                            catch (Exception ex)
                            {
                                strResponse = Common.ConstructNameValueCheckOutResponse("12", Common.EXCEPTION_MESSAGE, string.Empty,
                                                                                                       string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                       string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                Common.LogWrite("Response Name Value Collection \r\n" + ex.Message + ex.StackTrace);
                            }
                            //here
                        }
                        }
                        //Else xml is null
                        else
                        {
                            strResponse = "Invalid Request Type.";
                        }
                    
                }
            }

            }
        
        //Else, posted parameters is not proper
        else
        {
            strResponse = "400 Bad Request.";
        }
        Common.LogWrite("Response Name Value Collection \r\n" + strResponse);
        return strResponse;
    }


    public static string InvalidAppKey()
    {
        string strResponse = string.Empty;
        string SessionIdReLogin = string.Empty;
        string currencySymbol = string.Empty;
        string loginRequestXml = string.Empty;
        bool isRequestEnable = Convert.ToBoolean(ConfigurationManager.AppSettings["IsRequestLog"]);

        {
            using (LoginRequest loginRequest = new LoginRequest
            {
                RequestType = Common.LOGIN_REQUEST_TYPE,
                UserName = consumerInformation.AttendantId,
                Password = consumerInformation.Password
            })
            {
                //Serialize the login request object into Xml format.
                loginRequestXml = loginRequest.SerializeFocusConnectMessage(loginRequest);

                if (isRequestEnable)
                {
                    Common.LogWrite("Generated login xml request." + loginRequestXml);
                }
                else
                {
                    Common.LogWrite("Generated login xml request.");
                }
            }

            if (!String.IsNullOrEmpty(loginRequestXml))
            {
                //Get the FocusConnect endpoint from configuration file.
                focusConnectUrl = ConfigurationManager.AppSettings["FocusConnectUrl"].Trim();

                //If FocusConnect endpoint exists.
                if (!String.IsNullOrEmpty(focusConnectUrl))
                {
                    //Get the login response from FocusConnect.
                    try
                    {
                        using (HttpWebResponse connectLoginResponse = SendConnectRequest(loginRequestXml))
                        {
                            if (!httpresponsetimeout)
                            {
                                //Deserialize to LoginResponse object.
                                using (LoginResponse loginResponse = TF.REVCheckOut.Response.Response.DeserializeFocusConnectMessage(connectLoginResponse.GetResponseStream(), typeof(LoginResponse)) as LoginResponse)
                                {
                                    if (loginResponse != null)
                                    {
                                        //Check whether the login is successful or not.
                                        if (loginResponse.Code == 0)
                                        {
                                            string ccRequestXml = string.Empty;
                                            SessionIdReLogin = loginResponse.SessionID;
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
                                                    CreditCardNumber = consumerInformation.CardNumber,
                                                    ExpiryDate = string.Empty,
                                                    CVV = string.Empty,



                                                    Name = consumerInformation.ConsumerName,

                                                    Amount = consumerInformation.Amount,

                                                    CardPresentIndicator = Common.CP_INDICATOR,
                                                    TestMode = String.IsNullOrEmpty(consumerInformation.IsTestMode) ? "N" : consumerInformation.IsTestMode,
                                                    Address = string.Empty,
                                                    EmailAddress = consumerInformation.EmailAddress,
                                                    Notes = consumerInformation.Notes,
                                                    Tokenize = string.Empty,
                                                    Token = consumerInformation.Token,
                                                    OrderNumber = consumerInformation.OrderNumber,
                                                    AdditionalInfo = new Additionalinfo { InvoiceNum = consumerInformation.InvoiceNumber },
                                                    UDField2 = GetRequestField(Common.UDField2, ""),
                                                    UDField3 = GetRequestField(Common.CustomerNo, "")
                                                })
                                                {
                                                    //Serialize the cc request object into Xml format.
                                                    ccRequestXml = ccRequest.SerializeFocusConnectMessage(ccRequest);
                                                    Common.LogWrite(ccRequestXml);
                                                    if (isRequestEnable)
                                                    {
                                                        Common.LogWrite("Generated transaction xml request." + ccRequestXml);
                                                    }
                                                    else
                                                    {
                                                        Common.LogWrite("Generated transaction xml request.");
                                                    }
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
                                                    CreditCardNumber = consumerInformation.CardNumber,
                                                    ExpiryDate = string.Empty,
                                                    CVV = string.Empty,
                                                    Name = consumerInformation.ConsumerName,

                                                    Amount = consumerInformation.Amount,
                                                    GatewayTransactionID = consumerInformation.TransactionID,
                                                    CardPresentIndicator = Common.CP_INDICATOR,
                                                    TestMode = String.IsNullOrEmpty(consumerInformation.IsTestMode) ? "N" : consumerInformation.IsTestMode,
                                                    Address = string.Empty,
                                                    EmailAddress = consumerInformation.EmailAddress,
                                                    Notes = consumerInformation.Notes,
                                                    Tokenize = string.Empty,
                                                    Token = consumerInformation.Token,
                                                    OrderNumber = consumerInformation.OrderNumber,
                                                    AdditionalInfo = new Additionalinfo { InvoiceNum = consumerInformation.InvoiceNumber },
                                                    UDField2 = GetRequestField(Common.UDField2, ""),
                                                    UDField3 = GetRequestField(Common.CustomerNo, "")
                                                })
                                                {
                                                    //Serialize the cc request object into Xml format.
                                                    ccRequestXml = ccRequest.SerializeFocusConnectMessage(ccRequest);
                                                    Common.LogWrite(ccRequestXml);
                                                    if (isRequestEnable)
                                                    {
                                                        Common.LogWrite("Generated transaction xml request." + ccRequestXml);
                                                    }
                                                    else
                                                    {
                                                        Common.LogWrite("Generated transaction xml request.");
                                                    }
                                                }
                                            }
                                            if (!String.IsNullOrEmpty(ccRequestXml))
                                            {
                                                //Get the ccTransaction response from FocusConnect.
                                                if (!httpresponsetimeout)
                                                {
                                                    try
                                                    {
                                                        using (HttpWebResponse connectCcResponse = SendConnectRequest(ccRequestXml))
                                                        {
                                                            if (!httpresponsetimeout)
                                                            {
                                                                using (CCTransactionResponse ccResponse = TF.REVCheckOut.Response.Response.DeserializeFocusConnectMessage(connectCcResponse.GetResponseStream(), typeof(CCTransactionResponse)) as CCTransactionResponse)
                                                                {

                                                                    Common.LogWrite("Got the response from gateway.Response Code: " + ccResponse.Code + " Response Message:" + ccResponse.Description);


                                                                    if (ccResponse != null)
                                                                    {
                                                                        if (ccResponse.Code == 0)
                                                                        {


                                                                            strResponse = Common.ConstructNameValueCheckOutResponse(ccResponse.Code.ToString(), ccResponse.Description,
                                                                                                                                    ccResponse.Amount, ccResponse.GatewayTransactionID, ccResponse.TxnDtTime,
                                                                                                                                     string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                                     !String.IsNullOrEmpty(ccResponse.CreditCardNumber) ? ccResponse.CreditCardNumber : string.Empty,
                                                                                                                                     !String.IsNullOrEmpty(ccResponse.CardName) ? ccResponse.CardName : string.Empty,
                                                                                                                                     string.Empty, string.Empty, string.Empty, SessionIdReLogin);


                                                                        }

                                                                        else
                                                                        {
                                                                            //strResponse = ccResponse.Description;
                                                                            strResponse = Common.ConstructNameValueCheckOutResponse(ccResponse.Code.ToString(), ccResponse.Description, string.Empty,
                                                                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                                        }
                                                                    }
                                                                    //Else If no response from connect
                                                                    else
                                                                    {
                                                                        strResponse = "Gateway time out error.";
                                                                    }
                                                                }
                                                                //
                                                            }
                                                            //http Response Timeout
                                                            else
                                                            {
                                                                strResponse = Common.ConstructNameValueCheckOutResponse("12", Common.HTTP_RESPONSE_TIMEOUT, string.Empty,
                                                                                                 string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                 string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);


                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        strResponse = Common.ConstructNameValueCheckOutResponse("12", Common.EXCEPTION_MESSAGE, string.Empty,
                                                                                                          string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                                          string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                        Common.LogWrite("Response Name Value Collection \r\n" + ex.Message + ex.StackTrace);
                                                    }
                                                }
                                                ///HERE
                                            }
                                            //Else xml is null
                                            else
                                            {
                                                strResponse = "Invalid Request Type.";
                                            }
                                        }
                                        //Else Login failed
                                        else
                                        {
                                            strResponse = "Invalid Request Type.";
                                        }
                                    }
                                    //Else Login failed 
                                    else
                                    {
                                        strResponse = "Invalid Request Type.";
                                    }
                                }
                            }

                                //Http response Timeout
                            else
                            {
                                strResponse = Common.ConstructNameValueCheckOutResponse("12", Common.HTTP_RESPONSE_TIMEOUT, string.Empty,
                                                                                              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);


                            }

                        }
                        ////
                    }
                    catch (Exception ex)
                    {
                        strResponse = Common.ConstructNameValueCheckOutResponse("12", Common.EXCEPTION_MESSAGE, string.Empty,
                                                                                              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                              string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                        Common.LogWrite("Response Name Value Collection \r\n" + ex.Message + ex.StackTrace);
                    }
                    /////here**********

                }
                //Else Connect url found
                else
                {
                    strResponse = "408 Request Timeout.";
                }
            }
            //Else, posted parameters is not proper
            else
            {
                strResponse = "400 Bad Request.";
            }
        }
        return strResponse;
    }

#region SendConnectRequest

    /// <summary>
    /// Send the HTTP Post request to the FocusConnect with given payload as post data.
    /// </summary>
    /// <param name="payLoad">HTTP POST data.</param>
    /// <returns>FocusConnect response message.</returns>
    private static HttpWebResponse SendConnectRequest (string payLoad)
    {
        try
        {
            //Create the HTTP POST Request to FocusConnect url and append the required data as post content body.
            HttpWebRequest connectRequest = (HttpWebRequest)WebRequest.Create(focusConnectUrl);
            connectRequest.Method = Common.HTTP_METHOD;
            connectRequest.ContentType = Common.CONTENT_TYPE;
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
        catch (WebException ex)
        {
            if (ex.Status == WebExceptionStatus.Timeout)
            {
                httpresponsetimeout = true;
                return null;
            }
                return null;
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
    private static string GetRequestField (string key, string defaultValue)
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
}