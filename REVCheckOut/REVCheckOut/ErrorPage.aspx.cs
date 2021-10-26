
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: ErrorPage.cs
  Description: This page will be shown on any unhandled exception occurs on REV Checkout page.
  Date Created : 20-Nov-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;

#endregion

#region ErrorPage

/// <summary>
/// This page will be shown on any unhandled exception occurs on REV Checkout page.
/// </summary>
public partial class ErrorPage : System.Web.UI.Page
{
    #region Page_Load

    /// <summary>
    /// Fires on load of the error page.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">Event Arguments.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
       
        
        try
        {
        }
        catch(Exception ex)
        {
        }

    }

    #endregion
   
}

#endregion
