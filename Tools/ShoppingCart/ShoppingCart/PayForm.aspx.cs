using System;
using System.Collections.Specialized;
using System.Configuration;
using TF.FPCheckOut.HttpRoute;

public partial class PayForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnPay_Click(object sender, EventArgs e)
    {
        string checkOutUrl = ConfigurationManager.AppSettings["CheckOutUrl"].Trim();
        string callbackUrl = ConfigurationManager.AppSettings["CallbackUrl"].Trim();

        if (!String.IsNullOrEmpty(checkOutUrl))
        {
            NameValueCollection postData = new NameValueCollection();
            postData.Add("AID", txtAttendantID.Text.Trim());                               //Focuspay Username.
            postData.Add("PWD", txtPassword.Text.Trim());                                  //Focuspay Password.
            postData.Add("Name", txtConsumerName.Text.Trim());                            //Consumer Name.
            postData.Add("Address", txtConsumerAddress.Text.Trim());                      //Consumer Address.
            postData.Add("Email", txtEmailAddress.Text.Trim());                           //Consumer E-Mail Address.
            postData.Add("Amount", txtAmount.Text.Trim());                                //Transaction Amount.
            postData.Add("Currency", txtCurrency.Text.Trim());                            //Transaction currency type like USD, INR.... 
            postData.Add("TestMode", chkIsTestMode.Checked ? "Y" : "N");                   //Is this transaction performed under test/live mode
            //callback url of the consuming application to get back the status of the  transaction performed.
            postData.Add("Callback", callbackUrl);
            //Start - Added by Nazreen
            postData.Add("Tokenize", chkTokenize.Checked ? "true" : "false");               //Is this transaction performed if Tokenize true then get token
            postData.Add("Token", "");                                                      //This is used to perform transaction
            postData.Add("TransactionType", ddlTransType.SelectedItem.Value.Trim());        //Transaction type indicates type of transaction performing.
            postData.Add("TransactionID", txtTransactionID.Text.Trim());                    //This indicates to perform transaction like Refund/Void with previous Transaction ID
            postData.Add("IsGuestUser", (chkIsGuestUser.Checked ? "1" : "0"));              //Guest User
            postData.Add("InvoiceNumber", (txtInvoiceNumber.Text.Trim()));                  //Invoice Number
            postData.Add("OrderNumber", (txtOrderNumber.Text.Trim()));                      //Order Number
            postData.Add("AppKey", (txtAppKey.Text.Trim()));
            postData.Add("Notes", (txtNotes.Text.Trim()));
            postData.Add("CustomerNo", (txtCustomerNo.Text.Trim())); 



            //End

            //Send HTTP POST request to FocusPay Checkout.
            Response.PostRedirect(this, checkOutUrl, postData);
        }
    }

    protected void ddlTransType_SelectedIndexChanged (object sender, EventArgs e)
    {
        txtTransactionID.Text = string.Empty;
        chkTokenize.Checked = false;
        
        if(ddlTransType.SelectedItem.Value=="1")
        {
            txtTransactionID.Enabled = false;
            chkTokenize.Enabled = true;
        }
        else if(ddlTransType.SelectedItem.Value == "2")
        {
            txtTransactionID.Enabled = false;
            chkTokenize.Enabled = false;
        }
        else if(ddlTransType.SelectedItem.Value == "4")
        {
            txtTransactionID.Enabled = false;
            chkTokenize.Enabled = true;
        }
        else
        {
            txtTransactionID.Enabled = true;
            chkTokenize.Enabled = false;
        }
    }
    protected void txtNotes_TextChanged(object sender, EventArgs e)
    {

    }
}
