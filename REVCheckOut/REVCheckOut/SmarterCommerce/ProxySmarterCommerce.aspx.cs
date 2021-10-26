#region Namespaces

using System;
using System.Collections.Specialized;
using System.Web.UI;
using TF.REVCheckOut;
using TF.FPCheckOut.HttpRoute;

#endregion

public partial class ProxySmarterCommerce : System.Web.UI.Page
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
                if(Session["SmartCommOrderResponse"] != null && Session["ConsumerOrderInfo"] != null)
                {
                    //Extract the session information.
                     checkOutPostData = Session["SmartCommOrderResponse"] as NameValueCollection;
                    ConsumerOrderDetails consumerOrderInfo = Session["ConsumerOrderInfo"] as ConsumerOrderDetails;

                    if(checkOutPostData != null && consumerOrderInfo != null)
                    {
                        Common.LogWrite("Check response data is not null.");

                        Common.LogWrite("Send the response as http post.");

                        //Send the response as http post.
                        Response.PostRedirect(this, consumerOrderInfo.OrderCallbackUrl, checkOutPostData);

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