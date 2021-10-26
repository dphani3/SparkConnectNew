﻿<%@ WebHandler Language="C#" Class="SavedCardTransactionRequest" %>

using System;
using System.Web;
using TF.REVCheckOut;

public class SavedCardTransactionRequest : IHttpHandler
{

    #region Member Variables
    const string HTTP_CONTENT_TYPE = "application/x-www-form-urlencoded";    //Http content-type.
    #endregion

    public void ProcessRequest (HttpContext context)
    {
        try
        {
            if(context.Request != null && context.Request.ContentLength > 0)
            {
                //Read the focus connect request into buffer.
                byte[] inputBuffer = new byte[context.Request.ContentLength];
                context.Request.InputStream.Read(inputBuffer, 0, inputBuffer.Length);
                string requestData = System.Text.Encoding.UTF8.GetString(inputBuffer, 0, inputBuffer.Length);
                Common.LogWrite(requestData);
                string responseData = SmarterCommProcessRequest.AuthPaymentCardResponseData(System.Text.Encoding.UTF8.GetString(inputBuffer));
                Common.LogWrite(responseData);
                //Avoid caching issues.
                context.Response.Cache.SetNoServerCaching();
                context.Response.Cache.SetNoStore();

                //Add the content-type as "text/xml".
                context.Response.ContentType = HTTP_CONTENT_TYPE;
                context.Response.Write(responseData);
            }
        }
        catch(Exception ex)
        {
            Common.LogWrite("Exception: "+ex.Message+"\r\n" + ex.StackTrace);
            context.Response.Write("Request failed");
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}