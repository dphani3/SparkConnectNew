using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Net;
using System.IO;

public partial class SavedCardQuoteTransaction : System.Web.UI.Page
{
    #region Page Load

    protected void Page_Load (object sender, EventArgs e)
    {

    }

    #endregion

    #region Pay button event

    protected void btnPay_Click (object sender, EventArgs e)
    {
        string checkOutSmarterCommerceUrl = ConfigurationManager.AppSettings["CheckOutSmtComQuoteSavedCardUrl"].Trim();

        if(!String.IsNullOrEmpty(checkOutSmarterCommerceUrl))
        {
            KeyValuePair<string, string>[] kvpArr = new KeyValuePair<string, string>[]
            {
                 new KeyValuePair<string, string>("OrganizationCode", txtOrganizationCode.Text.Trim()),                                      //Organization Code
                 new KeyValuePair<string, string>("UserName", txtUserName.Text.Trim()),                                                      //User Name
                 new KeyValuePair<string, string>("CarrierCode", txtCarrierCode.Text.Trim()),                                                //Carrier Code
                 new KeyValuePair<string, string>("Company", txtCompany.Text.Trim()),                                                        //Company
                 new KeyValuePair<string, string>("CustomerPO", txtCustomerPO.Text.Trim()),                                                  //Customer PO
                 new KeyValuePair<string, string>("QuoteNumber", txtQuoteNumber.Text.Trim()),                                                //QuoteNumber
                 new KeyValuePair<string, string>("ShippingInstructions1", txtShippingInstructions1.Text.Trim()),                            //ShippingInstructions1
                 new KeyValuePair<string, string>("ShippingInstructions2", txtShippingInstructions2.Text.Trim()),                            //ShippingInstructions2
                 new KeyValuePair<string, string>("SavedCardCustomer", txtSavedCardCustomer.Text.Trim()),                                    //Saved Card Customer
                 new KeyValuePair<string, string>("Token", txtSavedCardLine.Text.Trim())                                             //Saved Card Line
            
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