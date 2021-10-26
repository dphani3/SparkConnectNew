
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Consumer.cs
  Description: This class will store the information related to the consumer.
  Date Created : 20-Nov-2010
  Revision History: 
  */

#endregion

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using TF.REVCheckOut.Response;

namespace TF.REVCheckOut
{
    #region Consumer

    /// <summary>
    /// This class will store the information related to the consumer.
    /// </summary>
    public class SmarterCommProcessRequest
    {
        private static SortedList<string, string> requestFields = null;
        private static ConsumerOrderDetails consumerOrderInformation= null;
        private static ConvertQuoteDetails convertQuoteInformation = null;
        private static string milacronWebserviceUrl = ConfigurationManager.AppSettings["MilacronWebserviceUrl"].ToString();

        private static XmlDocument xmlDoc = null;
        private static string XMLData = string.Empty;

        private static string error_Message = string.Empty;
        private static string strResponse = string.Empty;
        private static bool httpresponsetimeout = false;

        #region Process SOAP Xml request
        
        public static string ProcessAutorizeRequest (string xmlRequest, string typeofUrl,  ref string errorMessage,ref bool httpresponsetimeout)
        {
            string strResponsedata = string.Empty;
            string soapAction = string.Empty;

            if(typeofUrl == Common.TYPE_OF_SOAP_METHOD_TOKEN)
            {
                 soapAction = Common.tokenUrl.ToString();
            }
            else if(typeofUrl == Common.TYPE_OF_SOAP_METHOD_AUTHORIZE_CARD)
            {
                   soapAction = Common.authorizePaymentCardUrl.ToString();
            }
            else if(typeofUrl == Common.TYPE_OF_SOAP_METHOD_CONVERT_QUOTE)
            {
                soapAction = Common.convertQuoteUrl.ToString();
            }
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                Common.LogWrite("Request sent to gateway.");

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(milacronWebserviceUrl);

                //req.ContentType = "text/xml;charset=\"utf-8\"";
                req.Timeout = int.Parse(System.Configuration.ConfigurationManager.AppSettings["httptimeout"]);
                req.ContentType = Common.CONTENT_TYPE;
                req.Accept = Common.CONTENT_TYPE;
                req.Method = Common.HTTP_METHOD;
                req.Headers.Add(String.Format("SOAPAction: \"{0}\"", soapAction));

                Common.LogWrite("Set request parameters compelted.");

                try
                {
                    using(Stream stm = req.GetRequestStream())
                    {
                        using(StreamWriter stmw = new StreamWriter(stm))
                        {
                            stmw.Write(xmlRequest);
                        }
                    }
                }
                catch(Exception ex)
                {
                    errorMessage = ex.Message;

                    Common.LogWrite("Exception in Stream Writer: " + ex.Message);

                }

                string responseXML = "";
                try
                {
                    using(System.Net.WebResponse resp = req.GetResponse())
                    {
                        using(StreamReader sr = new StreamReader(resp.GetResponseStream()))
                        {
                            responseXML = sr.ReadToEnd();

                            Common.LogWrite("response xml: " + responseXML);
                        }
                    }
                }
                catch(WebException ex)
                {
                    errorMessage = ex.Message;

                    Common.LogWrite("Exception in getting response: " + ex.Message);

                }

                strResponsedata = RemoveAllNamespaces(responseXML);
            }
            catch(WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    Common.LogWrite("Http Response Timeout");
                    httpresponsetimeout = true;
                    errorMessage = "Http Response Timeout";
                }
                else
                {
                    errorMessage = ex.Message;

                    Common.LogWrite("Exception: " + ex.Message);
                }
            }
            Common.LogWrite(strResponsedata);
            return strResponsedata;
        }

        #endregion

        #region Auth Payment Card Response Data
        public static string AuthPaymentCardResponseData (string xmlRequest)
        {
            try
            {

                if(!String.IsNullOrEmpty(xmlRequest))
                {

                    Common.LogWrite(xmlRequest);
                    string[] requestDataCollection = xmlRequest.Split('&');
                    //Read the post data.
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
                        consumerOrderInformation = new ConsumerOrderDetails
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
                            SavedCardCustomer = GetRequestField(Common.SAVED_CARD_CUSTOMER, ""),
                            SavedCardLine = GetRequestField(Common.SAVED_CARD_LINE, ""),
                        };
                      
                        
                        if(consumerOrderInformation.OrganizationCode != string.Empty && consumerOrderInformation.UserName != string.Empty &&
                        consumerOrderInformation.AddressLine1 != string.Empty && consumerOrderInformation.Name != string.Empty &&
                        consumerOrderInformation.PostalCode != string.Empty && consumerOrderInformation.OrderCompany != string.Empty &&
                        consumerOrderInformation.OrderNumber != string.Empty &&  consumerOrderInformation.SavedCardCustomer != string.Empty && 
                        consumerOrderInformation.SavedCardLine != string.Empty)
                        {
                            Common.LogWrite("All input parameters are valid.");
                            //lock(_locker)
                            //{
                            //Common.LogWrite("Process is locked.");

                            string authorizeToken = string.Empty;

                            //Common.LogWrite("Process is locked.");

                            if(HttpContext.Current.Application[Common.APPLICATION_SESSION_NAME] == null)
                            {
                                //Generate Authorize Token
                              string  errorResponse = GetAuthorizeToken(ref authorizeToken);

                              if(errorResponse != string.Empty)
                              {
                                  strResponse = errorResponse;
                              }
                              else if(authorizeToken != string.Empty)
                              {
                                  HttpContext.Current.Application[Common.APPLICATION_SESSION_NAME] = authorizeToken;

                                strResponse = ProcessAuthTransaction(authorizeToken);
                              }
                            }
                            else
                            {
                                authorizeToken = HttpContext.Current.Application[Common.APPLICATION_SESSION_NAME].ToString();

                               strResponse = ProcessAuthTransaction(authorizeToken);
                            }

                            //Common.LogWrite("Process is Unocked.");
                        }
                        else
                        {
                            Common.LogWrite("Insufficient Parameters.");

                            strResponse = Common.INSUFFICIENT_PARAMETERS_ERROR_MESSAGE;
                        }

                    }
                    else
                    {
                        Common.LogWrite("Insufficient Parameters.");

                        strResponse = Common.INSUFFICIENT_PARAMETERS_ERROR_MESSAGE;
                    }
                }
                else
                {
                    strResponse = "400 Bad Request.";
                }
            }
            catch(Exception ex)
            {
                Common.LogWrite("Exception: " + ex.Message);

                strResponse = "500 Internal Server";
            }
            Common.LogWrite(strResponse);
            return strResponse;
        }
        #endregion

        #region Convert Quote Response Data
        public static string ConvertQuoteResponseData (string xmlRequest)
        {
            try
            {

                if(!String.IsNullOrEmpty(xmlRequest))
                {
                    Common.LogWrite(xmlRequest);
                    string[] requestDataCollection = xmlRequest.Split('&');
                    //Read the post data.
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

                        Common.LogWrite("Posted data in new card: " + requestFields.ToString());

                        //Construct the consumer object.
                        convertQuoteInformation            = new ConvertQuoteDetails
                        {
                            OrganizationCode               = GetRequestField(Common.ORGANIZATION_CODE, ""),
                            UserName                       = GetRequestField(Common.USER_NAME, ""),
                            CarrierCode                    = GetRequestField(Common.CARRIER_CODE, ""),  
                            OrderCompany                   = GetRequestField(Common.QUOTE_COMPANY, ""),
                            CustomerPO                     = GetRequestField(Common.CUSTOMER_PO, ""),
                            QuoteNumber                    = GetRequestField(Common.QUOTE_NUMBER, ""),
                            ShippingInstructions1          = GetRequestField(Common.SHIPPING_INSTRUCTIONS1, ""),
                            ShippingInstructions2          = GetRequestField(Common.SHIPPING_INSTRUCTIONS2, ""),
                            SavedCardCustomer              = GetRequestField(Common.SAVED_CARD_CUSTOMER, ""),
                            SavedCardLine                  = GetRequestField(Common.SAVED_CARD_LINE, ""),
                        };

                        if(convertQuoteInformation.OrganizationCode != string.Empty && convertQuoteInformation.UserName != string.Empty &&
                           convertQuoteInformation.CarrierCode != string.Empty && convertQuoteInformation.OrderCompany != string.Empty &&
                           convertQuoteInformation.CustomerPO != string.Empty && convertQuoteInformation.QuoteNumber != string.Empty    &&
                           convertQuoteInformation.SavedCardLine != string.Empty)
                        {
                            Common.LogWrite("All input parameters are valid.");
                           
                            string authorizeToken = string.Empty;

                            if(HttpContext.Current.Application[Common.APPLICATION_SESSION_NAME] == null)
                            {
                                //Generate Authorize Token
                                string errorResponse = GetAuthorizeToken(ref authorizeToken);

                                if(errorResponse != string.Empty)
                                {
                                    strResponse = errorResponse;
                                }
                                else if(authorizeToken != string.Empty)
                                {
                                    HttpContext.Current.Application[Common.APPLICATION_SESSION_NAME] = authorizeToken;

                                    strResponse = ProcessConverQuoteTransaction(authorizeToken);
                                }
                            }
                            else
                            {
                                authorizeToken = HttpContext.Current.Application[Common.APPLICATION_SESSION_NAME].ToString();

                                strResponse = ProcessConverQuoteTransaction(authorizeToken);
                            }
                        }
                        else
                        {
                            Common.LogWrite("Insufficient Parameters.");

                            strResponse = Common.INSUFFICIENT_PARAMETERS_ERROR_MESSAGE;
                        }

                    }
                    else
                    {
                        Common.LogWrite("Insufficient Parameters.");

                        strResponse = Common.INSUFFICIENT_PARAMETERS_ERROR_MESSAGE;
                    }
                }
                else
                {
                    strResponse = "400 Bad Request.";
                }
            }
            catch(Exception ex)
            {
                Common.LogWrite("Exception: " + ex.Message);

                strResponse = "500 Internal Server";
            }
            Common.LogWrite(strResponse);
            return strResponse;
        }
        #endregion

        #region Process Auth Transaction
        private static string ProcessAuthTransaction (string authorizeToken)
        {
            string response = string.Empty;
            string strAuthorizeToken = string.Empty;

            string authTransrequestXML = Common.GetAuthorizePaymentCardXml(consumerOrderInformation, authorizeToken, string.Empty,
                                                                           string.Empty, string.Empty, string.Empty, 0, false);

            Common.LogWrite("Generated Authorize payment card request soap xml.:" + authTransrequestXML);
            Common.LogWrite("Generated Authorize payment card request soap xml.");

            Common.LogWrite("Sending Authorize payment card request to milacron gateway. ");

            string responseData = ProcessAutorizeRequest(authTransrequestXML, Common.TYPE_OF_SOAP_METHOD_AUTHORIZE_CARD, ref error_Message,ref httpresponsetimeout);

            Common.LogWrite("Check the any exception in gateway.");

            if(error_Message != string.Empty)
            {
                Common.LogWrite("Exception from gateway: " + error_Message);
                Common.LogWrite("Sent response as Internal Gateway Server Error.");
                if (!httpresponsetimeout)
                { response = Common.SERVER_ERROR_MESSAGE; }
                else { response = Common.HTTP_RESPONSE_TIMEOUT; }
            }

            else if(responseData != string.Empty)
            {
                Common.LogWrite("Check the response is not null.");

                Common.LogWrite("Got the response from gateway.");

                XMLData = string.Empty;

                xmlDoc = new XmlDocument();

                //Remove xml namespacess
                xmlDoc = Common.RemoveXmlns(responseData);

                Common.LogWrite("Authorize payment card response soap xml as: " + xmlDoc.InnerXml);
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
                    if(objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Code != string.Empty &&
                       objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Code == "0")
                    {

                        Common.LogWrite("Gateway has approved the transaction.");

                        Common.LogWrite("Creating approved transaction response as name value collection.");

                        response = Common.ConstructNameValueCheckOutResponse(objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Code,
                                                                                objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Description,
                                                                                objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.TransactionAmount.ToString(),
                                                                                string.Empty, string.Empty, objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.SavedCardLine,
                                                                                objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.AuthCode,
                                                                                objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.OrderNumber,
                                                                                string.Empty, string.Empty, string.Empty,string.Empty,
                                                                                objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.PNRef,
                                                                                objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.SavedCardCustomer, string.Empty);

                    }
                    else if(objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Code != string.Empty)
                    {
                        Common.LogWrite("Gateway has not approved the transaction.");

                        Common.LogWrite("Gateway has return as " + objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Description);

                        strResponse = string.Empty;

                        Common.LogWrite("Creating failed transaction response as name value collection.");
                       
                        response = Common.ConstructNameValueCheckOutResponse(objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Code,
                                                                          objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.Description,
                                                                          (objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.TransactionAmount != string.Empty) ? objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.TransactionAmount : string.Empty,
                                                                          string.Empty, string.Empty,
                                                                          (objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.SavedCardLine != string.Empty) ? objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.SavedCardLine : string.Empty,
                                                                          (objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.AuthCode != string.Empty) ? objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.AuthCode : string.Empty,
                                                                          (objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.OrderNumber != string.Empty) ? objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.OrderNumber : string.Empty,
                                                                          string.Empty, string.Empty, string.Empty, string.Empty,
                                                                          (objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.PNRef != string.Empty) ? objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.PNRef : string.Empty,
                                                                          (objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.SavedCardCustomer != string.Empty) ? objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.SavedCardCustomer : string.Empty, string.Empty);
                    }
                    else if(objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.ReturnCode != string.Empty &&
                            objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.ReturnCode == "-2")
                    {
                        Common.LogWrite("Token expires, " + objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.ReturnMessage);

                        string errorresponse = GetAuthorizeToken(ref strAuthorizeToken);

                        if(errorresponse != string.Empty)
                        {
                            response = errorresponse;
                        }
                        else if(strAuthorizeToken != string.Empty)
                        {
                            //Clear the previous AuthToken
                            HttpContext.Current.Application[Common.APPLICATION_SESSION_NAME] = null;

                            //Assign new AuthToken
                            HttpContext.Current.Application[Common.APPLICATION_SESSION_NAME] = strAuthorizeToken;

                            //Process the Auth Transaction
                            ProcessAuthTransaction(strAuthorizeToken);
                        }
                    }
                    else if(objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.ReturnCode != string.Empty)
                    {

                        Common.LogWrite("Gateway has not approved the transaction.");

                        Common.LogWrite("Gateway has return as " + objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.ReturnMessage);

                        strResponse = string.Empty;

                        Common.LogWrite("Creating failed transaction response as name value collection.");

                        response = Common.ConstructNameValueCheckOutResponse(objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.ReturnCode,
                                                                               objAuthResponse.Body.AuthorizePaymentCardResponse.AuthorizePaymentCardResult.ReturnMessage,
                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    }
                    else
                    {
                        response = "Invalid Request Type.";
                    }
                }
                //Else If no response from connect
                else
                {
                    response = "Invalid Request Type.";
                }
                //}
                //else
                //{
                //    response = "Gateway time out error.";
                //}
            }
            Common.LogWrite(response);
            return response;
        }
        #endregion

        #region Get Authorize Token
        private static string GetAuthorizeToken (ref string strAuthToken)
        {
            string response = string.Empty;
            //Initialize AuthTokenResponse object
            AuthTokenResponse objAuthTokenResponse = new AuthTokenResponse();

            //Get the Authorize Token request xml
            string tokenRequestXml = Common.GetTokenRequestXML();

            if(tokenRequestXml != string.Empty)
            {

                Common.LogWrite("Generated token request soap xml." + tokenRequestXml);
                Common.LogWrite("Generated Authorize Token request soap xml.");

                Common.LogWrite("Sending Token request to milacron gateway. ");


                string tokenResponse = SmarterCommProcessRequest.ProcessAutorizeRequest(tokenRequestXml, Common.TYPE_OF_SOAP_METHOD_TOKEN, ref error_Message,ref httpresponsetimeout);

                Common.LogWrite("Check the any exception in gateway.");

                if(error_Message != string.Empty)
                {
                    Common.LogWrite("Exception from gateway: " + error_Message);
                    Common.LogWrite("Sent response as Internal Gateway Server Error.");

                    //strResponse = "Server Error in '/MGSL_Test' Application";
                    
                    response = Common.SERVER_ERROR_MESSAGE;
                }
                else if(tokenResponse != string.Empty)
                {
                    Common.LogWrite("Check the response is not null.");

                    Common.LogWrite("Got the response from gateway.");

                    //Remove namspaces from soap xml
                    xmlDoc = Common.RemoveXmlns(tokenResponse);

                    //Get the Token response xml
                    XMLData = xmlDoc.InnerXml.ToString();

                    Common.LogWrite("Token response soap xml as: " + xmlDoc.InnerXml);
                    Common.LogWrite("Got the Authorize Token response soap xml.");

                    Common.LogWrite("Parsing Token response soap xml.");

                    //Get Authorize Token response Info object
                    objAuthTokenResponse = (AuthTokenResponse)Common.ParsingProcess(XMLData, typeof(AuthTokenResponse));

                    Common.LogWrite("Parsing Token response soap xml completed.");

                    if(objAuthTokenResponse != null && objAuthTokenResponse.Body.AuthorizeTokenResponse.TokenResponse.Token != string.Empty)
                    {
                        Common.LogWrite("Got the new AuthToken.");

                        strAuthToken = objAuthTokenResponse.Body.AuthorizeTokenResponse.TokenResponse.Token;
                    }
                    else
                    {
                        response = "Gateway time out error.";
                    }
                }
            }
            else
            {
                response = "Invalid Request Type.";
            }

            Common.LogWrite(response);
            return response;
        }
        #endregion

        #region Proce Convert Quote Transaction
        private static string ProcessConverQuoteTransaction (string authorizeToken)
        {
            string response = string.Empty;
            string strAuthorizeToken = string.Empty;
            string errorresponse = string.Empty;
            
            string createQuoterequestXML = Common.CreateQuoteXml(convertQuoteInformation, authorizeToken, string.Empty,
                                                                           string.Empty, string.Empty, string.Empty, 0, false);

            Common.LogWrite("Generated convert quote request soap xml.:" + createQuoterequestXML);
            Common.LogWrite("Generated convert quote request soap xml.");

            Common.LogWrite("Sending convert quote request to milacron gateway. ");

            string responseData = ProcessAutorizeRequest(createQuoterequestXML, Common.TYPE_OF_SOAP_METHOD_CONVERT_QUOTE, ref error_Message , ref httpresponsetimeout);

            Common.LogWrite("Check the any exception in gateway.");

            if(error_Message != string.Empty)
            {
                Common.LogWrite("Exception from gateway: " + error_Message);
                Common.LogWrite("Sent response as Internal Gateway Server Error.");

                response = Common.SERVER_ERROR_MESSAGE;
            }

            else if(responseData != string.Empty)
            {
                Common.LogWrite("Check the response is not null.");

                Common.LogWrite("Got the response from gateway.");

                XMLData = string.Empty;

                xmlDoc = new XmlDocument();

                //Remove xml namespacess
                xmlDoc = Common.RemoveXmlns(responseData);

                Common.LogWrite("Convert quote response soap xml as: " + xmlDoc.InnerXml);
                Common.LogWrite("Got the convert quote response soap xml.");

                XMLData = xmlDoc.InnerXml.ToString();

                //Initialize Authorize payment card response
                ConvrtQuoteResponse objConvertQuoteResponse = new ConvrtQuoteResponse();

                Common.LogWrite("Parsing convert quote response soap xml.");

                //Get Auth Token response Info object
                objConvertQuoteResponse = (ConvrtQuoteResponse)Common.ParsingProcess(XMLData, typeof(ConvrtQuoteResponse));

                Common.LogWrite("Parsing convert quote response soap xml completed.");

                if(objConvertQuoteResponse != null)
                {
                    if(objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnCode != string.Empty &&
                       objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnCode == "0")
                    {

                        Common.LogWrite("Gateway has approved the transaction.");

                        Common.LogWrite("Creating approved transaction response as name value collection.");

                        response = Common.ConstructNameValueCheckOutResponse(objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnCode,
                                                                                objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnMessage,
                                                                                objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.OrderTotal.ToString(),
                                                                                string.Empty, string.Empty, objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.SavedCardLine,
                                                                                string.Empty, objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.OrderNumber,
                                                                                string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.SavedCardCustomer, string.Empty);

                    }
                    else if (objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnCode == "-1")
                    {
                        strResponse = string.Empty;

                        Common.LogWrite("Gateway has not approved the transaction.");
                        Common.LogWrite("Gateway has return as " + objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnMessage);
                        Common.LogWrite("Creating failed transaction response as name value collection.");

                        response = Common.ConstructNameValueCheckOutResponse(objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnCode,
                                                                           objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnMessage,
                                                                           (objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.OrderTotal != string.Empty) ? objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.OrderTotal : string.Empty,
                                                                           string.Empty, string.Empty,
                                                                           (objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.SavedCardLine != string.Empty) ? objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.SavedCardLine : string.Empty,
                                                                           string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                           (objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.SavedCardCustomer != string.Empty) ? objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.SavedCardCustomer : string.Empty, string.Empty);


                    }
                    else if (objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnCode == "-2")
                    {
                        Common.LogWrite("Token expires, " + objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnMessage);

                        errorresponse = GetAuthorizeToken(ref strAuthorizeToken);

                        if (errorresponse != string.Empty)
                        {
                            response = errorresponse;
                        }
                        else if (strAuthorizeToken != string.Empty)
                        {
                            //Clear the previous AuthToken
                            HttpContext.Current.Application[Common.APPLICATION_SESSION_NAME] = null;

                            //Assign new AuthToken
                            HttpContext.Current.Application[Common.APPLICATION_SESSION_NAME] = strAuthorizeToken;

                            //Process the Auth Transaction
                            ProcessConverQuoteTransaction(strAuthorizeToken);
                        }
                    }
                    else
                    {

                        Common.LogWrite("Gateway has not approved the transaction.");

                        Common.LogWrite("Gateway has return as " + objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnMessage);

                        strResponse = string.Empty;

                        Common.LogWrite("Creating failed transaction response as name value collection.");

                        response = Common.ConstructNameValueCheckOutResponse(objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnCode,
                                                                               objConvertQuoteResponse.Body.ConvertQuoteResponse.ConvertQuoteResult.ReturnMessage,
                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                               string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    }
                }
                //Else If no response from connect
                else
                {
                    response = "Invalid Request Type.";
                }
                //}
                //else
                //{
                //    response = "Gateway time out error.";
                //}
            }
            Common.LogWrite(response);
            return response;
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

        #region Remove name spaces

        public static string RemoveAllNamespaces (string responseXML)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(responseXML));

            return xmlDocumentWithoutNs.ToString();
        }

        //Core recursion function
        private static XElement RemoveAllNamespaces (XElement xmlDocument)
        {
            if(!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach(XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

        #endregion
    }

    #endregion
}
