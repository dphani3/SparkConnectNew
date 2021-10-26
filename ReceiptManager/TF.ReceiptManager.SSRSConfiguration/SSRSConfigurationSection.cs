
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: SSRSConfigurationSection.cs
  Description:  This class is intended to add the custom section information on the target application configuration file to configure the SQL Server
                Reporting Services credentials.
  Date Created : 31-May-2011
  Revision History: 
  */

#endregion

#region Namespaces

using System.Configuration;
using System.IO;
using System.Reflection;

#endregion

namespace TF.ReceiptManager.SSRSConfiguration
{
    #region SSRSConfigurationSection

    /// <summary>
    ///  This class is intended to add the custom section information on the target application configuration file to configure the SQL Server Reporting
    ///  Services credentials.
    /// </summary>
    public class SSRSConfigurationSection : ConfigurationSection
    {
        #region Member Variables

        //SSRS configuration section name.
        private const string SECTION_NAME = "SSRSConfigurationSection";

        //Receipt Manager service configuration file.
        private const string APP_CONFIG_FILE_NAME = "TF.ReceiptManager.Service.exe.config";

        //Application configuration handle.
        public Configuration sSRSConfiguration;

        //SSRS configuration section handle.
        private static SSRSConfigurationSection sSRSConfigurationSection = null;

        #endregion

        #region Properties

        #region SSRS

        /// <summary>
        /// SQL Server Reporting Services configuration element.
        /// </summary>
        [ConfigurationProperty("SSRS")]
        public SSRS SSRS
        {
            get
            {
                return (SSRS)this["SSRS"];
            }
            set
            {
                this["SSRS"] = value;
            }
        }

        #endregion        

        #endregion        

        #region GetSSRSConfigurationSection

        /// <summary>
        /// Gets the SSRS configuration section handle to get/set the values.
        /// </summary>
        /// <returns>SSRS configuration section handle.</returns>
        public static SSRSConfigurationSection GetSSRSConfigurationSection()
        {
            //Check whether SSRS configuration section handle already exists or not.
            if (sSRSConfigurationSection == null)
            {
                //Define the configuration file map with receipt manager configuration file.
                ExeConfigurationFileMap configurationFile = new ExeConfigurationFileMap();
                configurationFile.ExeConfigFilename = string.Format("{0}\\{1}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), APP_CONFIG_FILE_NAME);
                Configuration applicationConfig = ConfigurationManager.OpenMappedExeConfiguration(configurationFile, ConfigurationUserLevel.None);

                //Get the handle for SSRS configuration section.
                sSRSConfigurationSection = applicationConfig.Sections[SECTION_NAME.Trim()] as SSRSConfigurationSection;
                sSRSConfigurationSection.sSRSConfiguration = applicationConfig;
            }

            //Return the SSRS configuration section handle.
            return sSRSConfigurationSection;
        }

        #endregion
    }

    #endregion

    #region SSRS

    /// <summary>
    /// This class will define the SSRS credentials information.
    /// </summary>
    public class SSRS : ConfigurationElement
    {
        #region Properties

        #region Url

        /// <summary>
        /// SSRS web service url.
        /// </summary>
        [ConfigurationProperty("Url", IsRequired = true)]
        public string Url
        {
            get { return (string)this["Url"]; }
            set { this["Url"] = value; }
        }

        #endregion

        #region IsAuthenticationRequired

        /// <summary>
        /// Flag to check whether the SSRS requires authentication or not.
        /// </summary>
        [ConfigurationProperty("IsAuthenticationRequired", IsRequired = true)]
        public bool IsAuthenticationRequired
        {
            get { return (bool)this["IsAuthenticationRequired"]; }
            set { this["IsAuthenticationRequired"] = value; }
        }

        #endregion

        #region Domain

        /// <summary>
        /// SSRS authentication domain name.
        /// </summary>
        [ConfigurationProperty("Domain", IsRequired = false)]
        public string Domain
        {
            get { return (string)this["Domain"]; }
            set { this["Domain"] = value; }
        }

        #endregion

        #region UserName

        /// <summary>
        /// SSRS authentication user name.
        /// </summary>
        [ConfigurationProperty("UserName", IsRequired = false)]
        public string UserName
        {
            get { return (string)this["UserName"]; }
            set { this["UserName"] = value; }
        }

        #endregion

        #region Password

        /// <summary>
        /// SSRS authentication password.
        /// </summary>
        [ConfigurationProperty("Password", IsRequired = false)]
        public string Password
        {
            get { return (string)this["Password"]; }
            set { this["Password"] = value; }
        }

        #endregion

        #endregion
    }

    #endregion
}
