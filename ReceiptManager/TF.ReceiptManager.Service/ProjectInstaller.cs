
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: ProjectInstaller.cs
  Description: This is the main installer class for Receipt Manager service.
  Date Created : 01-Jun-2011
  Revision History: 
  */

#endregion

#region Namespaces

using System.ComponentModel;
using System.Configuration.Install;

#endregion

namespace TF.ReceiptManager.Service
{
    #region ProjectInstaller

    /// <summary>
    /// This is the main installer class for Receipt Manager service.
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        #region Constructor

        /// <summary>
        /// This is the constructor of the class that initializes the properties required for the service.
        /// </summary>
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        #endregion
    }

    #endregion
}
