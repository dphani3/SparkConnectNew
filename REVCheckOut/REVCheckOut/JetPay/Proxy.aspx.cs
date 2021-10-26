
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Proxy.cs
  Description: This class will bypass the response information to the callback url as HTTP POST data.
  Date Created : 20-Nov-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Collections.Specialized;
using System.Web.UI;
using TF.REVCheckOut;
using TF.FPCheckOut.HttpRoute;

#endregion

#region Proxy

/// <summary>
/// This class will bypass the response information to the callback url as HTTP POST data.
/// </summary>
public partial class Proxy : System.Web.UI.Page
{
    #region Page_Load

    /// <summary>
    /// This will Post the response message to the callback url.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">EventArgs.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        NameValueCollection checkOutPostData = Session["ConnectResponse"] as NameValueCollection;
        Consumer consumerInformation = Session["ConsumerInformation"] as Consumer;
        try
        {
            if (!Page.IsPostBack)
            {
                
                Common.LogWrite("Check whether the session information exists or not.");

                //Check whether the session information exists or not.
                if (Session["ConnectResponse"] != null && Session["ConsumerInformation"] != null)
                {
                    //Extract the session information.
                    //NameValueCollection checkOutPostData = Session["ConnectResponse"] as NameValueCollection;
                    //Consumer consumerInformation = Session["ConsumerInformation"] as Consumer;

                    if (checkOutPostData != null && consumerInformation != null)
                    {
                        Common.LogWrite("Check response data is not null.");

                        Common.LogWrite("Send the response as http post.");

                        //Send the response as http post.
                        Response.PostRedirect(this, consumerInformation.CallbackUrl, checkOutPostData);

                        Common.LogWrite("Send the response is completed.");

                        //Clear all the session information.
                        Session.Clear();
                    }
                    //Else, redirect to error page.
                    else
                    {
                       // Response.Redirect("ErrorPage.aspx", false);
                        Response.PostRedirect(this, consumerInformation.CallbackUrl, checkOutPostData);
                    }
                }

                //Else, redirect to error page.
                else
                {
                    Response.Redirect("ErrorPage.aspx", false);
                   // Response.PostRedirect(this, consumerInformation.CallbackUrl, checkOutPostData);
                }
            }
        }
        //Redirect to error page.
        catch
        {
           // Response.Redirect("ErrorPage.aspx", false);
            Response.PostRedirect(this, consumerInformation.CallbackUrl, checkOutPostData);
        }
    }

    #endregion
}

#endregion
