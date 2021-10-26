
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: ReportConfigurationSection.cs
  Description:  This class is intended to add the custom section information on the target application configuration file to configure the SQL Server
                Reporting Services report format.
  Date Created : 31-May-2011
  Revision History: 
  */

#endregion

#region Namespaces

using System.Configuration;
using System.IO;
using System.Reflection;

#endregion

namespace TF.ReceiptManager.ReportConfiguration
{
    #region ReportConfigurationSection

    /// <summary>
    /// This class is intended to add the custom section information on the target application configuration file to configure the SQL Server Reporting 
    /// Services report format.
    /// </summary>
    public class ReportConfigurationSection : ConfigurationSection
    {
        #region Member Variables

        //Report configuration section name.
        private const string SECTION_NAME = "ReportConfigurationSection";

        //Receipt Manager service configuration file.
        private const string APP_CONFIG_FILE_NAME = "TF.ReceiptManager.Service.exe.config";

        //Application configuration handle.
        private Configuration reportConfiguration;

        //Report configuration section handle.
        private static ReportConfigurationSection reportConfigurationSection = null;

        #endregion

        #region Properties

        #region Report

        /// <summary>
        /// Report configuration element.
        /// </summary>
        [ConfigurationProperty("Report")]
        public Report Report
        {
            get
            {
                return (Report)this["Report"];
            }
            set
            {
                this["Report"] = value;
            }
        }

        #endregion

        #endregion        

        #region GetReportConfigurationSection

        /// <summary>
        /// Gets the Report configuration section handle to get/set the values.
        /// </summary>
        /// <returns>Report configuration section handle.</returns>
        public static ReportConfigurationSection GetReportConfigurationSection()
        {
            //Check whether Report configuration section handle already exists or not.
            if (reportConfigurationSection == null)
            {
                //Define the configuration file map with receipt manager configuration file.
                ExeConfigurationFileMap configurationFile = new ExeConfigurationFileMap();
                configurationFile.ExeConfigFilename = string.Format("{0}\\{1}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), APP_CONFIG_FILE_NAME);
                Configuration applicationConfig = ConfigurationManager.OpenMappedExeConfiguration(configurationFile, ConfigurationUserLevel.None);              

                //Get the handle for report configuration section.
                reportConfigurationSection = applicationConfig.Sections[SECTION_NAME.Trim()] as ReportConfigurationSection;
                reportConfigurationSection.reportConfiguration = applicationConfig;
            }

            //Return the report configuration section handle.
            return reportConfigurationSection;
        }

        #endregion
    }

    #endregion

    #region Report

    /// <summary>
    /// This class will define the SSRS report format information.
    /// </summary>
    public class Report : ConfigurationElement
    {
        #region Properties

        #region Path

        /// <summary>
        /// SSRS report path.
        /// </summary>
        [ConfigurationProperty("Path", IsRequired = true)]
        public string Path
        {
            get { return (string)this["Path"]; }
            set { this["Path"] = value; }
        }

        #endregion

        #region Format

        /// <summary>
        /// SSRS report format.
        /// </summary>
        [ConfigurationProperty("Format", IsRequired = true)]
        public string Format
        {
            get { return (string)this["Format"]; }
            set { this["Format"] = value; }
        }

        #endregion

        #region ContentType

        /// <summary>
        /// SSRS report content-type.
        /// </summary>
        [ConfigurationProperty("ContentType", IsRequired = true)]
        public string ContentType
        {
            get { return (string)this["ContentType"]; }
            set { this["ContentType"] = value; }
        }

        #endregion

        #region FileExtension

        /// <summary>
        /// SSRS report file extension.
        /// </summary>
        [ConfigurationProperty("FileExtension", IsRequired = true)]
        public string FileExtension
        {
            get { return (string)this["FileExtension"]; }
            set { this["FileExtension"] = value; }
        }

        #endregion

        #region IsFileStorageAllowed

        /// <summary>
        /// Flag to check whether the report can be stored in the local file system or not.
        /// </summary>
        [ConfigurationProperty("IsFileStorageAllowed", IsRequired = true)]
        public bool IsFileStorageAllowed
        {
            get { return (bool)this["IsFileStorageAllowed"]; }
            set { this["IsFileStorageAllowed"] = value; }
        }

        #endregion

        #region LocalPath

        /// <summary>
        /// Local file system directory path to store the receipts.
        /// </summary>
        [ConfigurationProperty("LocalPath", IsRequired = false)]
        public string LocalPath
        {
            get { return (string)this["LocalPath"]; }
            set { this["LocalPath"] = value; }
        }

        #endregion

        #region SendReceiptDataToEmailQueue

        /// <summary>
        /// Flag to check whether the receipt stream can be added to the email queue or not.
        /// </summary>
        [ConfigurationProperty("SendReceiptDataToEmailQueue", IsRequired = true)]
        public bool SendReceiptDataToEmailQueue
        {
            get { return (bool)this["SendReceiptDataToEmailQueue"]; }
            set { this["SendReceiptDataToEmailQueue"] = value; }
        }

        #endregion

        #endregion
    }

    #endregion
}
