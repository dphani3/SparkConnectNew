using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

public partial class SavedCardTransaction : System.Web.UI.Page
{
    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #endregion

    #region Pay button event

    protected void btnPay_Click (object sender, EventArgs e)                                                        
    {
        string checkOutSmarterCommerceUrl = ConfigurationManager.AppSettings["CheckOutSavedCardUrl"].Trim();
        //string callbackSmarterCommerceUrl = ConfigurationManager.AppSettings["CallbackSmarterCommerceUrl"].Trim();

        if(!String.IsNullOrEmpty(checkOutSmarterCommerceUrl))
        {
            KeyValuePair<string, string>[] kvpArr = new KeyValuePair<string, string>[]
            {
                 new KeyValuePair<string, string>("OrganizationCode", txtOrganizationCode.Text.Trim()),                 //Organization Code
             new KeyValuePair<string, string>("UserName", txtUserName.Text.Trim()),                                //User Name
             new KeyValuePair<string, string>("AddressLine1", txtAddressLine1.Text.Trim()),                         //Costomer AddressLine1
             new KeyValuePair<string, string>("AddressLine2", txtAddressLine2.Text.Trim()),                         //Costomer AddressLine2
             new KeyValuePair<string, string>("AddressLine3", txtAddressLine3.Text.Trim()),                         //Costomer AddressLine3
             new KeyValuePair<string, string>("AddressLine4", txtAddressLine4.Text.Trim()),                         //Costomer AddressLine4
             new KeyValuePair<string, string>("City", txtCity.Text.Trim()),                                         //City
             new KeyValuePair<string, string>("Country", txtCountry.Text.Trim()),                                   //Country
             new KeyValuePair<string, string>("Fax", txtFax.Text.Trim()),                                           //Fax
             new KeyValuePair<string, string>("Name", txtName.Text.Trim()),                                         //Name
             new KeyValuePair<string, string>("Phone", txtPhone.Text.Trim()),                                       //Phone
             new KeyValuePair<string, string>("PostalCode", txtPostalCode.Text.Trim()),                             //Postal Code
             new KeyValuePair<string, string>("State", txtState.Text.Trim()),                                       //State
             new KeyValuePair<string, string>("OrderCompany", txtOrderCompany.Text.Trim()),                         //Order Company
             new KeyValuePair<string, string>("OrderNumber", txtOrderNumber.Text.Trim()),                           //Order Number
             new KeyValuePair<string, string>("OrderType", txtOrderType.Text.Trim()),                               //Order Type
             new KeyValuePair<string, string>("Token", txtSavedCardLine.Text.Trim()),                               //Transaction Amount.
             new KeyValuePair<string, string>("SavedCardCustomer", txtSavedCardCustomer.Text.Trim())                //Transaction currency type like USD, INR.... 
           
             };

            var parameters = new StringBuilder();
            foreach(var item in kvpArr)
            {
                parameters.AppendFormat("{0}={1}&", item.Key, HttpUtility.UrlEncode(item.Value.ToString()));
            }
            parameters.Remove(parameters.Length - 1, 1); // remove the last '&'

            //Create the HTTP POST Request to Checkout url and append the required data as post content body.
            HttpWebRequest connectRequest = (HttpWebRequest)WebRequest.Create(checkOutSmarterCommerceUrl);
            connectRequest.Method = "POST";
            connectRequest.ContentType = "application/x-www-form-urlencoded";

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

    #endregion
}