using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

public partial class TransactionStatus : System.Web.UI.Page
{
    const string RESPONSE_CODE              = "ResultCode";
    const string MESSAGE                    = "Message";
    const string AMOUNT                     = "Amount";
    const string TRANSACTION_ID             = "TransactionID";
    const string TRANSACTION_TIME           = "TransactionTime";
    const string TOKEN                      = "Token"; //Added by Nazreen
    const string REMEMBER_ME_CHECKBOX       = "RememberMeCheckbox";
    const string CARD_NUMBER                = "CardNumber";
    const string CARD_TYPE                  = "CardType";
    const string CARD_NAME                  = "CardName";
    const string APP_KEY = "AppKey";

    SortedList<string, string> requestFields = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            trToken.Visible = false;

            if (Request.ContentLength > 0)
            {
                using (StreamReader requestReader = new StreamReader(Request.InputStream))
                {
                    string[] requestDataCollection = requestReader.ReadToEnd().Split('&');

                    if (requestDataCollection != null && requestDataCollection.Length > 0)
                    {                        
                        requestFields = new SortedList<string, string>();

                        foreach (string requestData in requestDataCollection)
                        {
                            string[] requestItem = requestData.Split('=');

                            if (requestItem != null && requestItem.Count() == 2)
                            {
                                requestFields.Add(requestItem[0], HttpUtility.UrlDecode(requestItem[1]));
                            }

                            lblResponseCodeValue.Text       = GetRequestField(RESPONSE_CODE, "");
                            lblResponseMessageValue.Text    = GetRequestField(MESSAGE, "");
                            lblAmountValue.Text             = GetRequestField(AMOUNT, "");
                            lblTransactionIDValue.Text      = GetRequestField(TRANSACTION_ID, "");
                            lblTransactionTimeValue.Text    = GetRequestField(TRANSACTION_TIME, "");

                            //Start - Added by Nazreen
                            if(!String.IsNullOrEmpty(GetRequestField(TOKEN, "")))
                            {
                                trToken.Visible = true;
                                lblToken.Text = GetRequestField(TOKEN, "");
                            }
                            lblRememberMe.Text = GetRequestField(REMEMBER_ME_CHECKBOX, "");
                            lblCardNumber.Text = GetRequestField(CARD_NUMBER, "");
                            lblcardName.Text = GetRequestField(CARD_NAME, "");
                            lblCardType.Text = GetRequestField(CARD_TYPE, "");
                            lblAppKey.Text = GetRequestField(APP_KEY, "");

                            //End

                            if(lblResponseCodeValue.Text == "0")
                            {
                                lblResponseCodeValue.ForeColor = Color.Green;
                                lblResponseMessageValue.ForeColor = Color.Green;
                                lblAmountValue.ForeColor = Color.Green;
                                lblTransactionIDValue.ForeColor = Color.Green;
                                lblTransactionTimeValue.ForeColor = Color.Green;
                                //Start - Added by Nazreen
                                lblToken.ForeColor = Color.Green; 
                                lblRememberMe.ForeColor = Color.Green;
                                lblCardNumber.ForeColor = Color.Green;
                                lblcardName.ForeColor = Color.Green;
                                lblCardType.ForeColor = Color.Green;
                                lblAppKey.ForeColor = Color.Green;
                                //End
                            }
                            else
                            {
                                lblResponseCodeValue.ForeColor = Color.Red;
                                lblResponseMessageValue.ForeColor = Color.Red;
                                lblAmountValue.ForeColor = Color.Red;
                                lblTransactionIDValue.ForeColor = Color.Red;
                                lblTransactionTimeValue.ForeColor = Color.Red;
                                //Start - Added by Nazreen
                                lblToken.ForeColor = Color.Red; 
                                lblRememberMe.ForeColor = Color.Red;
                                lblCardNumber.ForeColor = Color.Red;
                                lblcardName.ForeColor = Color.Red;
                                lblCardType.ForeColor = Color.Red;
                                lblAppKey.ForeColor = Color.Red;
                                //End
                            }
                        }
                    }
                }
            }
        }
    }
    protected void lnkbtnPay_Click (object sender, EventArgs e)
    {
        Response.Redirect("~/PayForm.aspx", true);
    }
    protected void lnkbtnToken_Click (object sender, EventArgs e)
    {
        Response.Redirect("~/TransactionRequest.aspx", true);
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
}
