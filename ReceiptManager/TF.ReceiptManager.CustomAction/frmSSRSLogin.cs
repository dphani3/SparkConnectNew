
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: frmSSRSLogin.cs
  Description: This form will allow the user to provide the SSRS credentials and encrypts the SSRS section in the configuration file.
  Date Created : 01-Jun-2011
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

#endregion

namespace TF.ReceiptManager.CustomAction
{
    #region Internal Namespaces

    using SSRSConfiguration;

    #endregion

    #region frmSSRSLogin

    /// <summary>
    /// This form will allow the user to provide the SSRS credentials and encrypts the SSRS section in the configuration file.
    /// </summary>
    public partial class frmSSRSLogin : Form
    {
        #region Member Variables

        //SSRS application configuration section name.
        private const string SECTION_NAME = "SSRSConfigurationSection";

        //Receipt Manager service configuration file name.
        private const string APP_CONFIG_FILE_NAME = "TF.ReceiptManager.Service.exe.config";

        //DPAPI configuration encryption provider.
        private const string PROTECTION_PROVIDER = "DataProtectionConfigurationProvider";

        #endregion

        #region Constructor

        /// <summary>
        /// This is the constructor of the class that loads the required components for login form.
        /// </summary>
        public frmSSRSLogin()
        {
            InitializeComponent();

            //Keep the focus on SSRS url textbox.
            txtUrl.Focus();
        }

        #endregion

        #region btnNext_Click

        /// <summary>
        /// This event will be raised when the user clicks "Next" button after providing the SSRS information.
        /// </summary>
        /// <param name="sender">Sender context.</param>
        /// <param name="e">Event Arguments.</param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            //Check whether SSRS Url, Domain, Username and Password have been provided or not.
            if (!String.IsNullOrEmpty(txtUrl.Text) && !String.IsNullOrEmpty(txtDomain.Text) && !String.IsNullOrEmpty(txtUserName.Text) && !String.IsNullOrEmpty(txtPassword.Text))
            {
                //Subscribe for the event if any assembly resolution fails.
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

                //Load the receipt manager service configuration file.
                ExeConfigurationFileMap configurationFile = new ExeConfigurationFileMap();
                configurationFile.ExeConfigFilename = string.Format("{0}\\{1}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), APP_CONFIG_FILE_NAME);
                Configuration applicationConfig = ConfigurationManager.OpenMappedExeConfiguration(configurationFile, ConfigurationUserLevel.None);

                //Get the SSRS configuration section.
                SSRSConfigurationSection sSRSConfigurationSection = applicationConfig.Sections[SECTION_NAME.Trim()] as SSRSConfigurationSection;
                sSRSConfigurationSection.sSRSConfiguration = applicationConfig;

                //Assign the user provided values to SSRS configuration.
                sSRSConfigurationSection.SSRS.Url                               = txtUrl.Text.Trim();
                sSRSConfigurationSection.SSRS.IsAuthenticationRequired          = true;
                sSRSConfigurationSection.SSRS.Domain                            = txtDomain.Text.Trim();
                sSRSConfigurationSection.SSRS.UserName                          = txtUserName.Text.Trim();
                sSRSConfigurationSection.SSRS.Password                          = txtPassword.Text.Trim();

                //Encrypt the SSRS configuration section with DPAPI encryption provider.
                sSRSConfigurationSection.SectionInformation.ProtectSection(PROTECTION_PROVIDER);
                sSRSConfigurationSection.SectionInformation.ForceSave = true;
                sSRSConfigurationSection.sSRSConfiguration.Save(ConfigurationSaveMode.Full);

                //Set the dialog box return value as "OK".
                this.DialogResult = DialogResult.OK;
            }
            //Else, Show the alert message and keep on the same form without moving formward.
            else
            {
                MessageBox.Show("Please provide the mandatory information...", "Insufficient Parameters!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //Set the dialog box return value as "Cancel".
                this.DialogResult = DialogResult.Cancel;                
            }
        }

        #endregion

        #region CurrentDomain_AssemblyResolve

        /// <summary>
        /// This is the callback event on assembly resolution failure event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Assembly Name.</param>
        /// <returns>Assembly.</returns>
        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.LoadFrom(String.Format("{0}\\{1}.dll", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), args.Name));
        }

        #endregion
    }

    #endregion
}
