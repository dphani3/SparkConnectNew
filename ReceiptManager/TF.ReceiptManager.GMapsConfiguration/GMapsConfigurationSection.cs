
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: GMapsConfigurationSection.cs
  Description:  This class is intended to add the custom section information on the target application configuration file to configure the Google Static
                Maps API details.
  Date Created : 31-May-2011
  Revision History: 
  */

#endregion

#region Namespaces

using System.Configuration;
using System.IO;
using System.Reflection;

#endregion

namespace TF.ReceiptManager.GMapsConfiguration
{
    #region GMapsConfigurationSection

    /// <summary>
    /// This class is intended to add the custom section information on the target application configuration file to configure the Google Static Maps 
    /// API details.
    /// </summary>
    public class GMapsConfigurationSection : ConfigurationSection
    {
        #region Member Variables

        //Google Maps configuration section name.
        private const string SECTION_NAME = "GMapsConfigurationSection";

        //Receipt Manager service configuration file.
        private const string APP_CONFIG_FILE_NAME = "TF.ReceiptManager.Service.exe.config";

        //Application configuration handle.
        private Configuration gMapsConfiguration;

        //Google Maps configuration section handle.
        private static GMapsConfigurationSection gMapsConfigurationSection = null;

        #endregion

        #region Properties

        #region GoogleStaticMaps

        /// <summary>
        /// Google Static Maps configuration element.
        /// </summary>
        [ConfigurationProperty("GoogleStaticMaps")]
        public GoogleStaticMaps GoogleStaticMaps
        {
            get
            {
                return (GoogleStaticMaps)this["GoogleStaticMaps"];
            }
            set
            {
                this["GoogleStaticMaps"] = value;
            }
        }

        #endregion        

        #endregion        

        #region GetGMapsConfigurationSection

        /// <summary>
        /// Gets the Google Maps configuration section handle to get/set the values.
        /// </summary>
        /// <returns>Google Maps configuration section handle.</returns>
        public static GMapsConfigurationSection GetGMapsConfigurationSection()
        {
            //Check whether Google Maps configuration section handle already exists or not.
            if (gMapsConfigurationSection == null)
            {
                //Define the configuration file map with receipt manager configuration file.
                ExeConfigurationFileMap configurationFile = new ExeConfigurationFileMap();
                configurationFile.ExeConfigFilename = string.Format("{0}\\{1}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), APP_CONFIG_FILE_NAME);
                Configuration applicationConfig = ConfigurationManager.OpenMappedExeConfiguration(configurationFile, ConfigurationUserLevel.None);

                //Get the handle for Google Maps configuration section.
                gMapsConfigurationSection = applicationConfig.Sections[SECTION_NAME.Trim()] as GMapsConfigurationSection;
                gMapsConfigurationSection.gMapsConfiguration = applicationConfig;
            }

            //Return the Google Maps configuration section handle.
            return gMapsConfigurationSection;
        }

        #endregion
    }

    #endregion

    #region GoogleStaticMaps

    /// <summary>
    /// This class will define the Google Static Maps API information.
    /// </summary>
    public class GoogleStaticMaps : ConfigurationElement
    {
        #region Url

        /// <summary>
        /// Google Static Maps Url.
        /// </summary>
        [ConfigurationProperty("Url", IsRequired = true)]
        public string Url
        {
            get { return (string)this["Url"]; }
            set { this["Url"] = value; }
        }

        #endregion

        #region Center

        /// <summary>
        /// Center position of the Map.
        /// </summary>
        [ConfigurationProperty("Center", IsRequired = true)]
        public string Center
        {
            get { return (string)this["Center"]; }
            set { this["Center"] = value; }
        }

        #endregion

        #region Zoom

        /// <summary>
        /// Zoom level of the Map.
        /// </summary>
        [ConfigurationProperty("Zoom", IsRequired = true)]
        public string Zoom
        {
            get { return (string)this["Zoom"]; }
            set { this["Zoom"] = value; }
        }

        #endregion

        #region Size

        /// <summary>
        /// Size of the Map.
        /// </summary>
        [ConfigurationProperty("Size", IsRequired = true)]
        public string Size
        {
            get { return (string)this["Size"]; }
            set { this["Size"] = value; }
        }

        #endregion

        #region Format

        /// <summary>
        /// Image format of the Map.
        /// </summary>
        [ConfigurationProperty("Format", IsRequired = true)]
        public string Format
        {
            get { return (string)this["Format"]; }
            set { this["Format"] = value; }
        }

        #endregion

        #region Maptype

        /// <summary>
        /// Type of the Map.
        /// </summary>
        [ConfigurationProperty("Maptype", IsRequired = true)]
        public string Maptype
        {
            get { return (string)this["Maptype"]; }
            set { this["Maptype"] = value; }
        }

        #endregion

        #region Markers

        /// <summary>
        /// Markers for the Map.
        /// </summary>
        [ConfigurationProperty("Markers", IsRequired = true)]
        public string Markers
        {
            get { return (string)this["Markers"]; }
            set { this["Markers"] = value; }
        }

        #endregion

        #region Sensor

        /// <summary>
        /// Flag to check whether the application is using a sensor device or not.
        /// </summary>
        [ConfigurationProperty("Sensor", IsRequired = true)]
        public string Sensor
        {
            get { return (string)this["Sensor"]; }
            set { this["Sensor"] = value; }
        }

        #endregion
    }

    #endregion
}
