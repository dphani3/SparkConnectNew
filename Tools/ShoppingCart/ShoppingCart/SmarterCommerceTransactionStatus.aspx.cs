using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

public partial class SmarterCommerceTransactionStatus : System.Web.UI.Page
{
    #region Variable Declaration

    const string RESPONSE_CODE                      = "ResultCode";
    const string RESPONSE_MESSAGE                   = "Message";
    const string TRANSACTION_AMOUNT                 = "Amount";
    const string AUTH_CODE                          = "AuthCode";
    const string ORDER_NUMBER                       = "OrderNumber";
    const string REMEMBER_ME_CHECKBOX               = "RememberMeCheckbox";
    const string CARD_NUMBER                        = "CardNumber";
    const string CARD_TYPE                          = "CardType";
    const string PNREF                              = "PNRef";  
    const string SAVED_CARD_CUSTOMER                = "SavedCardCustomer";
    const string SAVED_CARD_LINE                    = "Token";


    SortedList<string, string> requestFields        = null;

    #endregion

    #region Page load

    protected void Page_Load (object sender, EventArgs e)
    {
        if(!Page.IsPostBack)
        {
            if(Request.ContentLength > 0)
            {
                using(StreamReader requestReader = new StreamReader(Request.InputStream))
                {
                    string[] requestDataCollection = requestReader.ReadToEnd().Split('&');

                    if(requestDataCollection != null && requestDataCollection.Length > 0)
                    {
                        requestFields = new SortedList<string, string>();

                        foreach(string requestData in requestDataCollection)
                        {
                            string[] requestItem = requestData.Split('=');

                            if(requestItem != null && requestItem.Count() == 2)
                            {
                                requestFields.Add(requestItem[0], HttpUtility.UrlDecode(requestItem[1]));
                            }

                            lblResponseCodeValue.Text                 = GetRequestField(RESPONSE_CODE, "");
                            lblResponseMessageValue.Text              = GetRequestField(RESPONSE_MESSAGE, "");
                            lblTransactionAmountValue.Text            = GetRequestField(TRANSACTION_AMOUNT, "");
                            lblAuthCodeValue.Text                     = GetRequestField(AUTH_CODE, "");
                            lblOrderNumberValue.Text                  = GetRequestField(ORDER_NUMBER, "");
                            lblRememberMeValue.Text                   = GetRequestField(REMEMBER_ME_CHECKBOX, "");
                            lblCardNumberValue.Text                   = GetRequestField(CARD_NUMBER, "");
                            lblCardTypeValue.Text                     = GetRequestField(CARD_TYPE, "");
                            lblPNRefValue.Text                        = GetRequestField(PNREF, "");
                            lblSavedCardCustomerValue.Text            = GetRequestField(SAVED_CARD_CUSTOMER, "");
                            lblSavedCardLineValue.Text                = GetRequestField(SAVED_CARD_LINE, "");
                            

                            //End

                            if(lblResponseCodeValue.Text == "0")
                            {
                                lblResponseCodeValue.ForeColor          = Color.Green;
                                lblResponseMessageValue.ForeColor       = Color.Green;
                                lblTransactionAmountValue.ForeColor     = Color.Green;
                                lblAuthCodeValue.ForeColor              = Color.Green;
                                lblOrderNumberValue.ForeColor           = Color.Green;
                                lblRememberMeValue.ForeColor            = Color.Green;
                                lblCardNumberValue.ForeColor            = Color.Green;
                                lblCardTypeValue.ForeColor              = Color.Green;
                                lblPNRefValue.ForeColor                 = Color.Green;
                                lblSavedCardCustomerValue.ForeColor     = Color.Green;
                                lblSavedCardLineValue.ForeColor         = Color.Green;
                            }
                            else
                            {
                                lblResponseCodeValue.ForeColor          = Color.Red;
                                lblResponseMessageValue.ForeColor       = Color.Red;
                                lblTransactionAmountValue.ForeColor     = Color.Red;
                                lblAuthCodeValue.ForeColor              = Color.Red;
                                lblOrderNumberValue.ForeColor           = Color.Red;
                                lblRememberMeValue.ForeColor            = Color.Red;
                                lblCardNumberValue.ForeColor            = Color.Red;
                                lblCardTypeValue.ForeColor              = Color.Red;
                                lblPNRefValue.ForeColor                 = Color.Red;
                                lblSavedCardCustomerValue.ForeColor     = Color.Red;
                                lblSavedCardLineValue.ForeColor         = Color.Red;
                              
                            }
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region GetRequestField

    private string GetRequestField (string key, string defaultValue)
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

    #region Pay button event

    protected void lnkbtnPay_Click (object sender, EventArgs e)
    {
        Response.Redirect("~/SmarterCommercePayForm.aspx", true);
    }

    #endregion

    #region Saved card button event

    protected void lnkbtnSavedCard_Click (object sender, EventArgs e)
    {
        Response.Redirect("~/SavedCardTransaction.aspx", true);
    }

    #endregion
}