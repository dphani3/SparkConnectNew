
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: ReceiptManager_CustomAction_Installer.cs
  Description: This is the custom installer class for Receipt Manager service that overrides the installer behavior on Installation and Uninstallation.
  Date Created : 01-Jun-2011
  Revision History: 
  */

#endregion

#region Namespaces

using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Reflection;

#endregion

namespace TF.ReceiptManager.CustomAction
{
    #region ReceiptManager_CustomAction_Installer

    /// <summary>
    /// This is the custom installer class for Receipt Manager service that overrides the installer behavior on Installation and Uninstallation.
    /// </summary>
    [RunInstaller(true)]
    public partial class ReceiptManager_CustomAction_Installer : Installer
    {
        #region Member Variables

        //Receipt Manager service executable name.
        private const string SERVICE_NAME = "TF.ReceiptManager.Service.exe";

        #endregion

        #region Constructor

        /// <summary>
        /// This is the constructor of the class that initializes the properties required for the custom installer.
        /// </summary>
        public ReceiptManager_CustomAction_Installer()
        {
            InitializeComponent();
        }

        #endregion

        #region Overridden Methods

        #region Install

        /// <summary>
        /// Allows the user to provide SSRS credentials and encrypts the same as well as installs the receipt manager service.
        /// </summary>
        /// <param name="stateSaver">An System.Collections.IDictionary used to save information needed to perform a commit, rollback, or uninstall operation.</param>
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);

            //Launch the SSRS Login form to retrieve the SSRS credentials from the user.
            frmSSRSLogin ssrsLoginForm = new frmSSRSLogin();

            //Wait until the user provides credentials.
            while (ssrsLoginForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                //Loop throught this routine...
            }

            //Get the current assembly location and build the Receipt Manager windows service location on fly.
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            assemblyLocation = UncToLocal(assemblyLocation);
            assemblyLocation = Directory.GetParent(assemblyLocation).FullName;

            //Receipt Manager windows service location.
            string serviceLocation = assemblyLocation + "\\" + SERVICE_NAME;

            //Install the Receipt Manager windows service.
            InstallReceiptManagerService(serviceLocation);
        }

        #endregion

        #region Uninstall

        /// <summary>
        /// Uninstalls the receipt manager service.
        /// </summary>
        /// <param name="savedState">An System.Collections.IDictionary used to save information needed to perform a commit, rollback, or uninstall operation.</param>
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);

            //Get the assembly location and build the Receipt Manager windows service location on fly.
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            assemblyLocation = UncToLocal(assemblyLocation);
            assemblyLocation = Directory.GetParent(assemblyLocation).FullName;

            //Receipt Manager windows service location.
            string serviceLocation = assemblyLocation + "\\" + SERVICE_NAME;

            //Uninstall the Receipt Manager windows service.
            UninstallReceiptManagerService(serviceLocation);
        }

        #endregion

        #endregion

        #region Private Methods

        #region UncToLocal

        /// <summary>
        /// Performs UNC path to local path conversion.
        /// </summary>
        /// <param name="path">UNC Path.</param>
        /// <returns>Local Path.</returns>
        private string UncToLocal(string path)
        {
            if (path.StartsWith("file://"))
                path = path.Substring(7);
            if (path.StartsWith("/"))
                path = path.Substring(1);

            path = path.Replace("/", "\\");
            return path;
        }

        #endregion

        #region InstallReceiptManagerService

        /// <summary>
        /// Installs the Receipt Manager Service with given path.
        /// </summary>
        /// <param name="executablePath">Receipt Manager service location.</param>
        private void InstallReceiptManagerService(string executablePath)
        {
            ManagedInstallerClass.InstallHelper(new string[] { executablePath });
        }

        #endregion

        #region UninstallReceiptManagerService

        /// <summary>
        /// Uninstalls the Receipt Manager Service at given path.
        /// </summary>
        /// <param name="executablePath">Receipt Manager service location.</param>
        private void UninstallReceiptManagerService(string executablePath)
        {
            ManagedInstallerClass.InstallHelper(new string[] { "/u", executablePath });
        }

        #endregion

        #endregion
    }

    #endregion
}
