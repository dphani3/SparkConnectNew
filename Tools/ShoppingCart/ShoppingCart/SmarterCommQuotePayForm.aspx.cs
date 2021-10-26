using System;
using System.Collections.Specialized;
using System.Configuration;
using TF.FPCheckOut.HttpRoute;

public partial class SmarterCommQuotePayForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnPay_Click (object sender, EventArgs e)
    {
        string checkOutSmarterCommerceUrl = ConfigurationManager.AppSettings["CheckOutSmtComQuoteUrl"].Trim();
        string callbackSmarterCommerceUrl = ConfigurationManager.AppSettings["CallbackSmtComQuoteUrl"].Trim();

        if(!String.IsNullOrEmpty(checkOutSmarterCommerceUrl))
        {
            NameValueCollection postData = new NameValueCollection();
            //postData.Add("AuthToken", txtAuthToken.Text.Trim());                               //Authorize Token
            postData.Add("OrganizationCode", txtOrganizationCode.Text.Trim());                 //Organization Code
            postData.Add("UserName", txtUserName.Text.Trim());                                 //User Name
            postData.Add("AddressLine1", txtAddressLine1.Text.Trim());                         //Costomer AddressLine1
            postData.Add("AddressLine2", txtAddressLine2.Text.Trim());                         //Costomer AddressLine2
            postData.Add("AddressLine3", txtAddressLine3.Text.Trim());                         //Costomer AddressLine3
            postData.Add("AddressLine4", txtAddressLine4.Text.Trim());                         //Costomer AddressLine4
            postData.Add("City", txtCity.Text.Trim());                                         //City
            postData.Add("Country", txtCountry.Text.Trim());                                   //Country
            postData.Add("Fax", txtFax.Text.Trim());                                           //Fax
            postData.Add("Name", txtName.Text.Trim());                                         //Name
            postData.Add("Phone", txtPhone.Text.Trim());                                       //Phone
            postData.Add("PostalCode", txtPostalCode.Text.Trim());                             //Postal Code
            postData.Add("State", txtState.Text.Trim());                                       //State
            postData.Add("CarrierCode", txtCarrierCode.Text.Trim());                           //Carrier Code
            postData.Add("Company", txtCompany.Text.Trim());                                   //Company
            postData.Add("CustomerPO", txtCustomerPO.Text.Trim());                             //Customer PO
            postData.Add("QuoteNumber", txtQuoteNumber.Text.Trim());                           //Quote Number
            postData.Add("Amount", txtAmount.Text.Trim());                                     //Transaction Amount.
            postData.Add("Currency", txtCurrency.Text.Trim());                                            //Transaction currency type like USD, INR.... 
            postData.Add("IsGuestUser", (chkIsGuestUser.Checked ? "1" : "0"));                 //Guest User
            //callback url of the consuming application to get back the status of the  transaction performed.
            postData.Add("Callback", callbackSmarterCommerceUrl);

            //Send HTTP POST request to FocusPay Checkout.
            Response.PostRedirect(this, checkOutSmarterCommerceUrl, postData);
        }
    }
}