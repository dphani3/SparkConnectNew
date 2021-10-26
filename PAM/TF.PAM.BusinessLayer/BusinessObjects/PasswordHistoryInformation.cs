
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: PasswordHistoryInformation.cs
  Description: This class is the business object representation for password history information.
  Date Created : 18-Aug-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System.Collections.Generic;

#endregion

namespace TF.PAM.BusinessLayer.BusinessObjects
{
    #region PasswordHistoryInformation

    /// <summary>
    /// This class is the business object representation for password history information.
    /// </summary>
    public class PasswordHistoryInformation
    {
        #region Member Variables

        private string encryptedPassword;           //User encrypted password.
        private string passwordSalt;                //User encrypted password salt.

        #endregion

        #region Properties

        #region EncryptedPassword

        /// <summary>
        /// Allows to get/set the users encrypted password.
        /// </summary>
        public string EncryptedPassword
        {
            get { return encryptedPassword; }
            set { encryptedPassword = value; }
        }

        #endregion

        #region PasswordSalt

        /// <summary>
        /// Allows to get/set the users encrypted password salt.
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

    #region PasswordHistoryInformationData

    /// <summary>
    /// This class is the business object representation for password history information data.
    /// </summary>
    public class PasswordHistoryInformationData
    {
        #region Member Variables

        private List<PasswordHistoryInformation> passwordHistoryInformationCollection;  //Password history information collection.
        private int samePasswordUsageLimit;                                             //Limit for using the same password on change.

        #endregion

        #region Properties

        #region PasswordHistoryInformationCollection

        /// <summary>
        /// Allows to get/set the password history information collection.
        /// </summary>
        public List<PasswordHistoryInformation> PasswordHistoryInformationCollection
        {
            get { return passwordHistoryInformationCollection; }
            set { passwordHistoryInformationCollection = value; }
        }

        #endregion

        #region SamePasswordUsageLimit

        /// <summary>
        /// Allows to get/set the limit for using the same password on change.
        /// </summary>
        public int SamePasswordUsageLimit
        {
            get { return samePasswordUsageLimit; }
            set { samePasswordUsageLimit = value; }
        }

        #endregion

        #endregion
    }

    #endregion

}
