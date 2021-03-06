
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Transact.ashx
  Description: This handler is the entry point for the external applications like MPOS, VPOS, Thin Client etc...
  Date Created : 28-Apr-2010
  Revision History: 
  */

#endregion

<%@ WebHandler Language="C#" Class="TransactHandler" %>

#region Namespaces

using System;
using System.Text;
using System.Web;
using FocusTransact = TF.FocusPay.FocusConnect;

#endregion

#region Transact

/// <summary>
/// This handler is the entry point for the external applications like MPOS, VPOS, Thin Client etc...
/// </summary>
public class TransactHandler : IHttpHandler 
{
    #region Member Variables
    
    const string HTTP_CONTENT_TYPE = "text/xml";    //Http content-type.

    #endregion

    #region ProcessRequest
    
    /// <summary>
    /// Processes the input xml from the request context and responds back with proper response message.
    /// </summary>
    /// <param name="context">Http context that encapsulates the focus connect request.</param>
    public void ProcessRequest (HttpContext context) 
    {
        //Check if request context contains some data or not.
        if (context.Request != null && context.Request.ContentLength > 0)
        {
            //Read the focus connect request into buffer.
            byte[] inputBuffer = new byte[context.Request.ContentLength];
            context.Request.InputStream.Read(inputBuffer, 0, inputBuffer.Length);
            
            //Pass the request to foucs connect engine and send back the response generated by focus connect engine.
            using (FocusTransact.Transact focusTransact = new TF.FocusPay.FocusConnect.Transact())
            {
               
                string responseData = focusTransact.ProcessRequest(Encoding.UTF8.GetString(inputBuffer));
                
                //Avoid caching issues.
                context.Response.Cache.SetNoServerCaching();
                context.Response.Cache.SetNoStore();

                //Add the content-type as "text/xml".
                context.Response.ContentType = HTTP_CONTENT_TYPE;
                context.Response.Write(responseData);
                
                context.Response.End();
            }
        }
    }

    #endregion

    #region IsReusable

    /// <summary>
    /// Another instance can use this handler.
    /// </summary>
    public bool IsReusable 
    {
        get 
        {
            return true;
        }
    }

    #endregion
}

#endregion