using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;


public partial class TransactionRequest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnPay_Click (object sender, EventArgs e)
    {
        lblResponse.Text = string.Empty;
        spanResponse.Visible = false;
        trResponse.Visible = false;

        string checkOutUrl = ConfigurationManager.AppSettings["CheckOutTokenUrl"].Trim();

        if(!String.IsNullOrEmpty(checkOutUrl))
        {
            //Create keyvalue parameters
            KeyValuePair<string, string>[] kvpArr = new KeyValuePair<string, string>[]
            {
                 new KeyValuePair<string, string>("AID", txtAttendantID.Text.Trim()),
                 new KeyValuePair<string, string>("PWD", txtPassword.Text.Trim()),
                 new KeyValuePair<string,string>("Email", txtEmailAddress.Text.Trim()),
                 new KeyValuePair<string,string>("Amount", txtAmount.Text.Trim()),
                 new KeyValuePair<string,string>("Currency", txtCurrency.Text.Trim()),
                 new KeyValuePair<string,string>("TestMode", chkIsTestMode.Checked ? "Y" : "N"),
                 new KeyValuePair<string,string>("Token", txtToken.Text.Trim()),
                 new KeyValuePair<string,string>("TransactionType", ddlTransType.SelectedItem.Value.Trim()),
                 new KeyValuePair<string,string>("TransactionID", txtTransactionID.Text.Trim()),
                 new KeyValuePair<string,string>("Name",txtname.Text.Trim()),
                 new KeyValuePair<string,string>("CardNumber", txtCreditCardNumber.Text.Trim()),
                 new KeyValuePair<string,string>("InvoiceNumber", txtInvoiceNumber.Text.Trim()),
                 new KeyValuePair<string,string>("OrderNumber", txtOrderNumber.Text.Trim()),
                 new KeyValuePair<string,string>("AppKey", txtAppKey.Text.Trim()),
                 new KeyValuePair<string,string>("Notes", txtNotes.Text.Trim()),
                 new KeyValuePair<string,string>("CustomerNo", txtCustomerNo.Text.Trim())
               
             };

            var parameters = new StringBuilder();
            foreach(var item in kvpArr)
            {
                parameters.AppendFormat("{0}={1}&", item.Key, HttpUtility.UrlEncode(item.Value.ToString()));
            }
            parameters.Remove(parameters.Length - 1, 1); // remove the last '&'

            //Create the HTTP POST Request to Checkout url and append the required data as post content body.
            HttpWebRequest connectRequest = (HttpWebRequest)WebRequest.Create(checkOutUrl);
            connectRequest.Method = "POST";
            connectRequest.ContentType = "application/x-www-form-urlencoded";
            connectRequest.UseDefaultCredentials = true;
            connectRequest.PreAuthenticate = true;
            connectRequest.Credentials = CredentialCache.DefaultCredentials;

            byte[] postData = Encoding.UTF8.GetBytes(parameters.ToString());
            connectRequest.ContentLength = postData.Length;

            // Get the request stream.
            Stream dataStream = connectRequest.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(postData, 0, postData.Length);
            // Close the Stream object.
            dataStream.Close();

            //Send back the response from Checkout.
            HttpWebResponse httpReponse = connectRequest.GetResponse() as HttpWebResponse;

            // Get the stream containing content returned by the server.
            dataStream = httpReponse.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            
            trResponse.Visible = true;
            spanResponse.Visible = true;
            lblResponse.Text = responseFromServer;

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            httpReponse.Close();
        }

    }

    protected void ddlTransType_SelectedIndexChanged (object sender, EventArgs e)
    {
        txtTransactionID.Text = string.Empty;        

        if(ddlTransType.SelectedItem.Value == "1")
        {
            txtTransactionID.Enabled = false;
        }
        else if(ddlTransType.SelectedItem.Value == "2")
        {
            txtTransactionID.Enabled = false;
        }
        else if(ddlTransType.SelectedItem.Value == "4")
        {
            txtTransactionID.Enabled = false;
        }
        else
        {
            txtTransactionID.Enabled = true;
        }
    }
}
