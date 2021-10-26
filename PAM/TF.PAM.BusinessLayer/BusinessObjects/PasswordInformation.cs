
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: PasswordInformation.cs
  Description: This is the password information object that encapsulates all password related information like password, salt and 
               encryped password.
  Date Created : 20-Jul-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Xml.Serialization;

#endregion

namespace TF.PAM.BusinessLayer.BusinessObjects
{
    #region PasswordInformation

    /// <summary>
    /// This is the password information object that encapsulates all password related information like password, salt and encryped password.
    /// </summary>
    [Serializable]
    [XmlRoot]
    public class PasswordInformation
    {
        #region Member Variables

        //Password in plain text.
        private string password;

        //Password in encrypted format.
        private string encryptedPassword;

        //Password Salt for encrypt/decrypt.
        private string passwordSalt;

        #endregion

        #region Properties

        #region Password

        /// <summary>
        /// Allows to get/set the plain text password value.
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        #endregion

        #region EncryptedPassword

        /// <summary>
        /// Allows to get/set the encrypted password value.
        /// </summary>
        public string EncryptedPassword
        {
            get { return encryptedPassword; }
            set { encryptedPassword = value; }
        }

        #endregion

        #region PasswordSalt

        /// <summary>
        /// Allows to get/set the password salt value.
        /// </summary>
        public string PasswordSalt
        {
            get { return passwordSalt; }
            set { passwordSalt = value; }
        }

        #endregion

        #endregion
    }

    #endregion
}
