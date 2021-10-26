
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: FPRequest.cs
  Description: This class will allow the client to post the form data while redirecting to the remote page.
  Date Created : 18-Nov-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.UI;

#endregion

namespace TF.FPCheckOut.HttpRoute
{
    #region FPRequest

    /// <summary>
    /// This class will allow the client to post the form data while redirecting to the remote page.
    /// </summary>
    public static class FPRequest
    {
        #region PostRedirect

        /// <summary>
        /// Redirects the client to a new URL with given post data. 
        /// </summary>
        /// <param name="hostPage">Source page from which the post data should be redirected to the target url.</param>
        /// <param name="targetUrl">Target page url.</param>
        /// <param name="postData">Http post data.</param>
        public static void PostRedirect(Page hostPage, string targetUrl, NameValueCollection postData)
        {
            //Construct the check out form data.
            string formData = ConstructPostForm(targetUrl, postData);

            //Add a literal control to the specified page.
            hostPage.Controls.Add(new LiteralControl(formData));            
        }

        #endregion

        #region PostRedirect Extension Method

        /// <summary>
        /// Redirects the client to a new URL with given post data. 
        /// </summary>
        /// <param name="response">Encapsulated http response information.</param>
        /// <param name="hostPage">Source page from which the post data should be redirected to the target url.</param>
        /// <param name="targetUrl">Target page url.</param>
        /// <param name="postData">Http post data.</param>
        public static void PostRedirect(this HttpResponse response, Page hostPage, string targetUrl, NameValueCollection postData)
        {
            //Construct the check out form data.
            string formData = ConstructPostForm(targetUrl, postData);

            //Add a literal control to the specified page.
            hostPage.Controls.Add(new LiteralControl(formData));
        }

        #endregion        

        #region ConstructPostForm

        /// <summary>
        /// Constructs the http post data that needs to be posted to the given target url.
        /// </summary>
        /// <param name="targetUrl">Target page url.</param>
        /// <param name="postData">Http post data.</param>
        /// <returns>Dynamic form data.</returns>
        private static string ConstructPostForm(string targetUrl, NameValueCollection postData)
        {
            //Identifier for the form.
            string formId = "frmCheckOut";

            //Construct the form with the given post data.
            StringBuilder formData = new StringBuilder();

            //Initialize the form data with method as "POST" and action as a target url.
            formData.Append("<form id=\"" + formId + "\" name=\"" + formId + "\" action=\"" + targetUrl + "\" method=\"POST\">");

            //Append the post data as html hidden variable.
            foreach (string postItem in postData)
            {
                formData.Append("<input type=\"hidden\" name=\"" + postItem + "\" value=\"" + postData[postItem] + "\">");
            }
            formData.Append("</form>");

            //Construct the javascript function that will post the above constructed form without user interaction.
            StringBuilder formScript = new StringBuilder();

            formScript.Append("<script language='javascript'>");
            formScript.Append("var v" + formId + " = document." + formId + ";");
            formScript.Append("v" + formId + ".submit();");
            formScript.Append("</script>");

            //Send back the concatenated form and the javascript data in the same order.
            return formData.ToString() + formScript.ToString();
        }

        #endregion
    }

    #endregion
}
