using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using TF.FocusPayCheckOut;
using TF.FocusPayCheckOut.Request;
using TF.FocusPayCheckOut.Response;


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

    public static string ResponseData (string strRequestData)
    {
        string strResponse = string.Empty;
        bool isValid = false;  
        bool isValidCurrency = false;
        string currencySymbol = string.Empty;
        string loginRequestXml = string.Empty;  

        if(!String.IsNullOrEmpty(strRequestData))
        {
            string[] requestDataCollection = strRequestData.Split('&');


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
                    TransactionType = Convert.ToInt32(GetRequestField(Common.TRANSACTION_TYPE, ""))
                };
            }

            //Check Attendant ID.
            if(!String.IsNullOrEmpty(consumerInformation.AttendantId))
            {
                Common.FocusPayCheckoutLogger.Debug("Got the Attendant ID.");
            }
            //Else
            else
            {
                Common.FocusPayCheckoutLogger.Debug("Attendant ID is null.");

                isValid = true;
            }

            //Check Password.
            if(!String.IsNullOrEmpty(consumerInformation.Password))
            {
                Common.FocusPayCheckoutLogger.Debug("Got the Password.");
            }
            //Else
            else
            {
                Common.FocusPayCheckoutLogger.Debug("Password is null.");

                isValid = true;
            }

            //Check Token.
            if(!String.IsNullOrEmpty(consumerInformation.Token))
            {
                Common.FocusPayCheckoutLogger.Debug("Got the Token value.");
            }
            //Else
            else
            {
                Common.FocusPayCheckoutLogger.Debug("Token value is null.");

                isValid = true;
            }

            //Check the Transaction Amount value to the checkout page.
            if(!String.IsNullOrEmpty(consumerInformation.Amount))
            {
                decimal amount = Common.GetDecimalMoneyValue(consumerInformation.Amount);

                string transcationAmount = string.Empty;

                if(!amount.ToString().Contains('.'))
                    transcationAmount = string.Format("{0}.00", amount.ToString());
                else
                    transcationAmount = amount.ToString();

                Common.FocusPayCheckoutLogger.Debug("Got the Transaction Amount.");
            }
            //Else
            else
            {
                Common.FocusPayCheckoutLogger.Debug("Transaction Amount is null.");

                isValid = true;
            }

            //Assign the currency value to the checkout page.
            if(!String.IsNullOrEmpty(consumerInformation.Currency))
            {
                bool isValidCurr = Common.Validate_Currency(consumerInformation.Currency, ref currencySymbol);

                if(isValidCurr)
                {
                    Common.FocusPayCheckoutLogger.Debug("Got the currency type.");
                }
                else
                {
                    Common.FocusPayCheckoutLogger.Debug("Invalid currency type.");

                    isValidCurrency = true;
                }
            }
            //Else
            else
            {
                Common.FocusPayCheckoutLogger.Debug("Currency type is null.");

                isValid = true;
            }

            //Check Transaction Type.
            if(consumerInformation.TransactionType != 0)
            {
                Common.FocusPayCheckoutLogger.Debug("Got the Transaction Type.");
            }
            //Else
            else
            {
                Common.FocusPayCheckoutLogger.Debug("Transaction Type is null.");

                isValid = true;
            }

            if(isValidCurrency)
            {
                Common.FocusPayCheckoutLogger.Debug("Invalid Currency Type.");

                strResponse =  Common.INVALID_CURRENCY_CODE_ERROR_MESSAGE;
            }
            else if(isValid)
            {
                Common.FocusPayCheckoutLogger.Debug("Insufficient Parameters.");

                strResponse = Common.INSUFFICIENT_PARAMETERS_ERROR_MESSAGE;
            }

            else
            {
                //Create the FocusConnect login request with the help of consumer information details.
                using(LoginRequest loginRequest = new LoginRequest
                {
                    RequestType = Common.LOGIN_REQUEST_TYPE,
                    UserName = consumerInformation.AttendantId,
                    Password = consumerInformation.Password
                })
                {
                    //Serialize the login request object into Xml format.
                    loginRequestXml = loginRequest.SerializeFocusConnectMessage(loginRequest);
                }

                if(!String.IsNullOrEmpty(loginRequestXml))
                {
                    //Get the FocusConnect endpoint from configuration file.
                    focusConnectUrl = ConfigurationManager.AppSettings["FocusConnectUrl"].Trim();

                    //If FocusConnect endpoint exists.
                    if(!String.IsNullOrEmpty(focusConnectUrl))
                    {
                        //Get the login response from FocusConnect.
                        using(HttpWebResponse connectLoginResponse = SendConnectRequest(loginRequestXml))
                        {
                            //Deserialize to LoginResponse object.
                            using(LoginResponse loginResponse = TF.FocusPayCheckOut.Response.Response.DeserializeFocusConnectMessage(connectLoginResponse.GetResponseStream(), typeof(LoginResponse)) as LoginResponse)
                            {
                                if(loginResponse != null)
                                {
                                    //Check whether the login is successful or not.
                                    if(loginResponse.Code == 0)
                                    {
                                        string ccRequestXml = string.Empty;

                                        if(consumerInformation.TransactionType == Common.SALE_TRANSACTION ||
                                                            consumerInformation.TransactionType == Common.AUTHORIZE_TRANSACTION ||
                                                            consumerInformation.TransactionType == Common.CREDIT_TRANSACTION)
                                        {
                                            //If login is success, send CC request to FocusConnect.
                                            using(CCTransactionRequest ccRequest = new CCTransactionRequest
                                            {
                                                RequestType = Common.CREDIT_CARD_REQUEST_TYPE,
                                                UserName = consumerInformation.AttendantId,
                                                Password = consumerInformation.Password,
                                                SessionID = loginResponse.SessionID,

                                                TransactionType = consumerInformation.TransactionType,
                                                CreditCardNumber = string.Empty,
                                                ExpiryDate = string.Empty,
                                                CVV = string.Empty,
                                                Name = string.Empty,
                                                Amount = consumerInformation.Amount,

                                                CardPresentIndicator = Common.CP_INDICATOR,
                                                TestMode = String.IsNullOrEmpty(consumerInformation.IsTestMode) ? "N" : consumerInformation.IsTestMode,
                                                Address = string.Empty,
                                                EmailAddress = consumerInformation.EmailAddress,
                                                Notes = string.Empty,
                                                Tokenize = string.Empty,
                                                Token = consumerInformation.Token,
                                            })
                                            {
                                                //Serialize the cc request object into Xml format.
                                                ccRequestXml = ccRequest.SerializeFocusConnectMessage(ccRequest);
                                            }
                                        }

                                        else if(consumerInformation.TransactionType == Common.PRIORAUTHCAPTURE_TRANSACTION ||
                                                           (consumerInformation.TransactionType == Common.CREDIT_TRANSACTION &&
                                                          !String.IsNullOrEmpty(consumerInformation.TransactionID)) ||
                                                          consumerInformation.TransactionType == Common.REFUND_TRANSACTION ||
                                                          consumerInformation.TransactionType == Common.VOID_TRANSACTION)
                                        {
                                            //If login is success, send CC request to FocusConnect.
                                            using(CCTransactionRequest ccRequest = new CCTransactionRequest
                                            {
                                                RequestType = Common.CREDIT_CARD_REQUEST_TYPE,
                                                UserName = consumerInformation.AttendantId,
                                                Password = consumerInformation.Password,
                                                SessionID = loginResponse.SessionID,

                                                TransactionType = consumerInformation.TransactionType,
                                                CreditCardNumber = string.Empty,
                                                ExpiryDate = string.Empty,
                                                CVV = string.Empty,
                                                Name = string.Empty,

                                                Amount = consumerInformation.Amount,
                                                GatewayTransactionID = consumerInformation.TransactionID,
                                                CardPresentIndicator = Common.CP_INDICATOR,
                                                TestMode = String.IsNullOrEmpty(consumerInformation.IsTestMode) ? "N" : consumerInformation.IsTestMode,
                                                Address = string.Empty,
                                                EmailAddress = consumerInformation.EmailAddress,
                                                Notes = string.Empty,
                                                Tokenize = string.Empty,
                                                Token = consumerInformation.Token
                                            })
                                            {
                                                //Serialize the cc request object into Xml format.
                                                ccRequestXml = ccRequest.SerializeFocusConnectMessage(ccRequest);
                                            }
                                        }
                                        if(!String.IsNullOrEmpty(ccRequestXml))
                                        {
                                            //Get the ccTransaction response from FocusConnect.
                                            using(HttpWebResponse connectCcResponse = SendConnectRequest(ccRequestXml))
                                            {
                                                using(CCTransactionResponse ccResponse = TF.FocusPayCheckOut.Response.Response.DeserializeFocusConnectMessage(connectCcResponse.GetResponseStream(), typeof(CCTransactionResponse)) as CCTransactionResponse)
                                                {
                                                    if(ccResponse != null)
                                                    {
                                                        if(ccResponse.Code == 0)
                                                        {
                                                            //strResponse = "ResultCode=" + ccResponse.Code
                                                            //            + ",Description=" + ccResponse.Description
                                                            //            + ",Amount=" + ccResponse.Amount
                                                            //            + ",TransactionID=" + ccResponse.GatewayTransactionID
                                                            //            + ",TxnDtTime=" + ccResponse.TxnDtTime;

                                                            //NameValueCollection checkOutPostData = Common.ConstructCheckOutResponse(
                                                            //                                                       ccResponse.Code,
                                                            //                                                       ccResponse.Description,
                                                            //                                                       ccResponse.Amount,
                                                            //                                                       ccResponse.GatewayTransactionID,
                                                            //                                                       ccResponse.TxnDtTime,null);

                                                    //        KeyValuePair<string, string>[] kvpArr = new KeyValuePair<string, string>[]
                                                    //{
                                                    //            new KeyValuePair<string, string>("ResultCode", ccResponse.Code.ToString()),
                                                    //            new KeyValuePair<string, string>("Message", ccResponse.Description),
                                                    //            new KeyValuePair<string,string>("Amount", ccResponse.Amount),
                                                    //             new KeyValuePair<string,string>("TransactionID", ccResponse.GatewayTransactionID),
                                                    //             new KeyValuePair<string,string>("TransactionTime", ccResponse.TxnDtTime)
                                                    //};

                                                    //        var parameters = new StringBuilder();
                                                    //        foreach(var item in kvpArr)
                                                    //        {
                                                    //            parameters.AppendFormat("{0}={1}&", item.Key, HttpUtility.UrlEncode(item.Value.ToString()));
                                                    //        }
                                                    //        parameters.Remove(parameters.Length - 1, 1); // remove the last '&'

                                                    //        strResponse = parameters.ToString();

                                                            strResponse = Common.ConstructNameValueCheckOutResponse(ccResponse.Code.ToString(), ccResponse.Description,
                                                                                                                    ccResponse.Amount, ccResponse.GatewayTransactionID, ccResponse.TxnDtTime,
                                                                                                                     string.Empty, string.Empty, string.Empty, string.Empty, 
                                                                                                                    string.Empty, string.Empty, string.Empty, string.Empty);
                                                        }
                                                        else
                                                        {
                                                            //strResponse = ccResponse.Description;
                                                            strResponse = Common.ConstructNameValueCheckOutResponse(ccResponse.Code.ToString(), ccResponse.Description, string.Empty,
                                                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 
                                                                                                                    string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                                                        }                                             
                                                    }
                                                    //Else If no response from connect
                                                    else
                                                    {
                                                        strResponse = "Gateway time out error.";
                                                    }
                                                }
                                            }
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
        }
        //Else, posted parameters is not proper
        else
        {
            strResponse = "400 Bad Request.";
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
        //Create the HTTP POST Request to FocusConnect url and append the required data as post content body.
        HttpWebRequest connectRequest = (HttpWebRequest)WebRequest.Create(focusConnectUrl);
        connectRequest.Method = Common.HTTP_METHOD;
        connectRequest.ContentType = Common.CONTENT_TYPE;

        byte[] postData = Encoding.UTF8.GetBytes(payLoad);
        connectRequest.ContentLength = postData.Length;

        //Append the post data.
        using(Stream requestStream = connectRequest.GetRequestStream())
        {
            requestStream.Write(postData, 0, postData.Length);
        }

        //Send back the response from FocusConnect.
        return connectRequest.GetResponse() as HttpWebResponse;
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