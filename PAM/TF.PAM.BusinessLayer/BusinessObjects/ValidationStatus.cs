
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: ValidationStatus.cs
  Description: This class is the business object representation for password validation status.
  Date Created : 26-Aug-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Xml.Serialization;

#endregion

namespace TF.PAM.BusinessLayer.BusinessObjects
{
    #region ValidationStatus

    /// <summary>
    /// This class is the business object representation for password validation status.
    /// </summary>
    [Serializable]
    [XmlRoot]
    public class ValidationStatus
    {
        #region Member Variables

        //User ID.
        private long userId;

        //Password in encrypted format.
        private string encryptedPassword;

        //Password salt.
        private string passwordSalt;

        //Validation response code.
        private int resposeCode;

        //Validation response message.
        private string responseMessage;

        #endregion

        #region Properties

        #region UserId

        /// <summary>
        /// Allows to get/set the User ID.
        /// </summary>
        public long UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        #endregion

        #region EncryptedPassword

        /// <summary>
        /// Allows to get/set the encrypted password.
        /// </summary>
        public string EncryptedPassword
        {
            get { return encryptedPassword; }
            set { encryptedPassword = value; }
        }

        #endregion

        #region PasswordSalt

        /// <summary>
        /// Allows to get/set the password salt.
        /// </summary>
        public string PasswordSalt
        {
            get { return passwordSalt; }
            set { passwordSalt = value; }
        }

        #endregion

        #region ResposeCode

        /// <summary>
        /// Allows to get/set the validation response code.
        /// </summary>
        public int ResposeCode
        {
            get { return resposeCode; }
            set { resposeCode = value; }
        }

        #endregion

        #region ResponseMessage

        /// <summary>
        /// Allows to get/set the validation response message.
        /// </summary>
        public string ResponseMessage
        {
            get { return responseMessage; }
            set { responseMessage = value; }
        }

        #endregion

        #endregion
    }

    #endregion
}
