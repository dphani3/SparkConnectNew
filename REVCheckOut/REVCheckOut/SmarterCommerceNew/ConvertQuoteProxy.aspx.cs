using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using TF.REVCheckOut;
using TF.FPCheckOut.HttpRoute;

public partial class ConvertQuoteProxy : System.Web.UI.Page
{
    #region Page_Load

    /// <summary>
    /// This will Post the response message to the callback url.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">EventArgs.</param>
    protected void Page_Load (object sender, EventArgs e)
    {
        NameValueCollection checkOutPostData = null; 
        try
        {
            if(!Page.IsPostBack)
            {
                Common.LogWrite("Check whether the session information exists or not.");

                //Check whether the session information exists or not.
                if(Session["SmartCommQuoteResponse"] != null && Session["ConvertQuoteInfo"] != null)
                {
                    //Extract the session information.
                     checkOutPostData = Session["SmartCommQuoteResponse"] as NameValueCollection;
                    ConvertQuoteDetails createQuoteInfo = Session["ConvertQuoteInfo"] as ConvertQuoteDetails;

                    if(checkOutPostData != null && createQuoteInfo != null)
                    {
                        Common.LogWrite("Check response data is not null.");

                        Common.LogWrite("Send the response as http post.");

                        //Send the response as http post.
                        Response.PostRedirect(this, createQuoteInfo.OrderCallbackUrl, checkOutPostData);

                        Common.LogWrite("Response sending is completed.");

                        //Clear all the session information.
                        Session.Clear();
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
        }
        //Redirect to error page.
        catch(Exception ex)
        {
            Common.LogWrite("Exception: " + ex.StackTrace);
            Response.Redirect("ErrorPage.aspx", false);
            checkOutPostData = Common.ConstructCheckOutResponse(Common.INSUFFICIENT_PARAMETERS_ERROR_CODE, Common.EXCEPTION_MESSAGE, string.Empty, string.Empty,
                                                                                  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                                  string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            Session["SmartCommQuoteResponse"] = checkOutPostData;
            Response.Redirect("ConvertQuoteProxy.aspx", false);
        }
    }

    #endregion
}