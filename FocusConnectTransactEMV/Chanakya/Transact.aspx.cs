
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Transact.aspx.cs
  Description: This file is the entry point for the external applications like MPOS, VPOS, Thin Client etc...
  Date Created : 12-Apr-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.IO;
using System.Net;
using System.Web.UI;
using FocusTransact = TF.FocusPay.FocusConnect;

#endregion

#region Transact

/// <summary>
/// This file is the entry point for the external applications like MPOS, VPOS, Thin Client etc...
/// </summary>
public partial class Transact : Page
{
    #region Member Variables
        
    const string HTTP_CONTENT_TYPE = "text/xml";    //Http content-type.

    #endregion

    #region Page_Load

    /// <summary>
    /// This method will be fired on loading the Transact page. This method will load the focus pay request as xml input and process the same.
    /// </summary>
    /// <param name="sender">Method invoker.</param>
    /// <param name="e">Event Arguments.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //If the request to the page is not past back.
        if (!Page.IsPostBack)
        {
            //If the http mthod is "POST".
            if (Request.HttpMethod.Equals(WebRequestMethods.Http.Post))
            {
                //Read the entire xml request into a stream and process the same.
                using (StreamReader requestStreamReader = new StreamReader(Request.InputStream))
                {
                    string requestData = requestStreamReader.ReadToEnd();
                    ProcessRequest(requestData);
                }
            }
        }
    }

    #endregion    

    #region btnSubmit_Click

    /// <summary>
    /// This method will be fired on submitting the focus pay xml request.
    /// </summary>
    /// <param name="sender">Button Invoker.</param>
    /// <param name="e">Event Arguments.</param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string requestData = txtInput.Text.Trim(); 

        if (!String.IsNullOrEmpty(requestData))
            ProcessRequest(requestData);
    }

    #endregion

    #region ProcessRequest

    /// <summary>
    /// Processes the input xml request and responds back with proper response message.
    /// </summary>
    /// <param name="requestData">Input focus pay request data in xml format.</param>
    private void ProcessRequest(string requestData)
    {
        using (FocusTransact.Transact focusTransact = new TF.FocusPay.FocusConnect.Transact())
        {
            string responseData = focusTransact.ProcessRequest(requestData);

            //Make sure to add the response content-type as "text/xml".
            Response.ContentType = HTTP_CONTENT_TYPE;
            Response.Write(responseData);
            Response.End();
        }
    }

    #endregion
}

#endregion
