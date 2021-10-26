<%@ WebHandler Language="C#" Class="SavedCardQuoteTransRequest" %>

using System;
using System.Web;
using TF.FocusPayCheckOut;

public class SavedCardQuoteTransRequest : IHttpHandler {

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

                string responseData = SmarterCommProcessRequest.ConvertQuoteResponseData(System.Text.Encoding.UTF8.GetString(inputBuffer));

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
            context.Response.Write("Request failed");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}