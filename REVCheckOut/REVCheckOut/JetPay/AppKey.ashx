<%@ WebHandler Language="C#" Class="AppKey" %>

using System;
using System.Web;
using TF.REVCheckOut.Response;
using TF.REVCheckOut.Request;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Text;
using System.Net;
using TF.REVCheckOut;
public class AppKey : IHttpHandler {


    private System.Collections.Generic.SortedList<string, string> requestFields = null;

    //Consumer information.
    private Consumer consumerInformation = null;

    static string cardType = string.Empty;

    NameValueCollection checkOutPostData = null;
    
    public void ProcessRequest (HttpContext context) {
        byte[] inputBuffer = new byte[context.Request.ContentLength];
        context.Request.InputStream.Read(inputBuffer, 0, inputBuffer.Length);
        string requestData = Encoding.UTF8.GetString(inputBuffer, 0, inputBuffer.Length);

        string responseData = LoginConnect(requestData);


       
       
        context.Response.ContentType = "text/plain";
        context.Response.Write(responseData);
    }

    public string LoginConnect(string RequestString)
    {
                            string response= string.Empty;
                            string[] requestDataCollection = RequestString.Split('&');
                            Common.LogWrite(DateTime.Now.ToString() + RequestString);
                            Common.LogWrite("Reading Namevalue collection data.");

                            if (requestDataCollection != null && requestDataCollection.Length > 0)
                            {
                                //Fill the request fileds into collection.
                                requestFields = new System.Collections.Generic.SortedList<string, string>();

                                foreach (string requestData in requestDataCollection)
                                {
                                    string[] requestItem = requestData.Split('=');

                                    if (requestItem != null)
                                    {
                                        requestFields.Add(requestItem[0], HttpUtility.UrlDecode(requestItem[1]));
                                    }

                                    Common.LogWrite("Retriving the parameter value from the namevalue collection and assigning respective consumer class properties.");

                                    //Construct the consumer object.
                                    consumerInformation = new Consumer
                                    {
                                        AttendantId = GetRequestField(Common.AID, ""),
                                        Password = GetRequestField(Common.PWD, ""),
                                    };


                                    if (!String.IsNullOrEmpty(consumerInformation.AttendantId))
                                    {
                                        Common.LogWrite("Got the Attendant ID.");
                                    }
                                    //Else
                                    else
                                    {
                                        Common.LogWrite("Attendant ID is null.");


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


                                    }
                                }
                                string loginRequestXml = string.Empty;
                                bool isRequestEnable = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsRequestLog"]);
                                //Create the FocusConnect login request with the help of consumer information details.

                                using (LoginRequest loginRequest = new LoginRequest
                                {
                                    RequestType = Common.LOGIN_REQUEST_TYPE,
                                    UserName = consumerInformation.AttendantId,
                                    Password = consumerInformation.Password,
                                    Notes = consumerInformation.ConsumerName
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
                                    string focusConnectUrl = System.Configuration.ConfigurationManager.AppSettings["FocusConnectUrl"].Trim();



                                    //If FocusConnect endpoint exists.
                                    if (!String.IsNullOrEmpty(focusConnectUrl))
                                    {
                                        Common.LogWrite("Connect Url is not empty.");

                                        Common.LogWrite("Sending login request to connect.");




                                        //Get the login response from FocusConnect.
                                        Stream Responsestream;
                                        String responseString = string.Empty;
                                        try
                                        {
                                            using (HttpWebResponse connectLoginResponse = SendConnectRequest(loginRequestXml))
                                            {

                                                using (Responsestream = connectLoginResponse.GetResponseStream())
                                                {
                                                    StreamReader reader = new StreamReader(Responsestream, Encoding.UTF8);
                                                    responseString = reader.ReadToEnd();
                                                    Common.LogWrite(DateTime.Now.ToString() + responseString);
                                                }

                                                Stream stream = GenerateStreamFromString(responseString);
                                                //Deserialize to LoginResponse object.
                                                using (LoginResponse loginResponse = Response.DeserializeFocusConnectMessage(stream, typeof(LoginResponse)) as LoginResponse)
                                                {
                                                    if (loginResponse.Code != 0)
                                                    {
                                                        response = Common.RESPONSE_CODE + loginResponse.Code + Common.MESSAGE + loginResponse.Description;
                                                    }
                                                    else
                                                    {
                                                        response = "SESSIONID=" + loginResponse.SessionID;
                                                    }
                                                }

                                            }
                                        }

                                        catch (HttpException ex)
                                        {
                                            response = "login Failed";
                                        }
                                    }

                                }

                            
                            }
                            return response;                   
    }

      public Stream GenerateStreamFromString(string s)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(s));
    }
                              
    private HttpWebResponse SendConnectRequest(string payLoad)
    {
         string focusConnectUrl = System.Configuration.ConfigurationManager.AppSettings["FocusConnectUrl"].Trim();
        //Create the HTTP POST Request to FocusConnect url and append the required data as post content body.
        HttpWebRequest connectRequest   = (HttpWebRequest)WebRequest.Create(focusConnectUrl);
        connectRequest.Method           = Common.HTTP_METHOD;
        connectRequest.ContentType      = Common.CONTENT_TYPE;

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
    public bool IsReusable {
        get {
            return false;
        }
    }

}