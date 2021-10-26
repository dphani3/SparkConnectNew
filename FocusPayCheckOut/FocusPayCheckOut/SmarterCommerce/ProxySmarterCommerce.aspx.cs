#region Namespaces

using System;
using System.Collections.Specialized;
using System.Web.UI;
using TF.FocusPayCheckOut;
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
        try
        {
            if(!Page.IsPostBack)
            {
                Common.FocusPayCheckoutLogger.Debug("Check whether the session information exists or not.");

                //Check whether the session information exists or not.
                if(Session["SmartCommOrderResponse"] != null && Session["ConsumerOrderInfo"] != null)
                {
                    //Extract the session information.
                    NameValueCollection checkOutPostData = Session["SmartCommOrderResponse"] as NameValueCollection;
                    ConsumerOrderDetails consumerOrderInfo = Session["ConsumerOrderInfo"] as ConsumerOrderDetails;

                    if(checkOutPostData != null && consumerOrderInfo != null)
                    {
                        Common.FocusPayCheckoutLogger.Debug("Check response data is not null.");

                        Common.FocusPayCheckoutLogger.Debug("Send the response as http post.");

                        //Send the response as http post.
                        Response.PostRedirect(this, consumerOrderInfo.OrderCallbackUrl, checkOutPostData);

                        Common.FocusPayCheckoutLogger.Debug("Response sending is completed.");

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
        catch
        {
            Response.Redirect("ErrorPage.aspx", false);
        }
    }

    #endregion
}