
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: UserInformation.cs
  Description: This class is the business object representation for user information.
  Date Created : 16-Aug-2010
  Revision History: 
  */

#endregion

namespace TF.PAM.BusinessLayer.BusinessObjects
{
    #region UserInformation

    /// <summary>
    /// This class is the business object representation for user information.
    /// </summary>
    public class UserInformation
    {
        #region Member Variables

        private string encryptedPassword;           //User encrypted password.
        private string passwordSalt;                //User encrypted password salt.
        private byte unlockMinutes;                 //Number of minutes left to unlock the user.
        private long userId;                        //User ID.
        private long companyId;                     //Users company id.        
        private long merchantId;                    //Users merchant id.
        private long attendantId;                   //Users attendant id.
        private int responseCode;                   //Response code.

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

        #region UnlockMinutes

        /// <summary>
        /// Allows to get/set the number of minutes left to unlock the user.
        /// </summary>
        public byte UnlockMinutes
        {
            get { return unlockMinutes; }
            set { unlockMinutes = value; }
        }

        #endregion

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

        #region CompanyId

        /// <summary>
        /// Allows to get/set the users company id.
        /// </summary>
        public long CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        #endregion

        #region MerchantId

        /// <summary>
        /// Allows to get/set the users merchant id.
        /// </summary>
        public long MerchantId
        {
            get { return merchantId; }
            set { merchantId = value; }
        }

        #endregion

        #region AttendantId

        /// <summary>
        /// Allows to get/set the users attendant id.
        /// </summary>
        public long AttendantId
        {
            get { return attendantId; }
            set { attendantId = value; }
        }

        #endregion

        #region ResponseCode

        /// <summary>
        /// Allows to get/set the response code.
        /// </summary>
        public int ResponseCode
        {
            get { return responseCode; }
            set { responseCode = value; }
        }

        #endregion

        #endregion
    }

    #endregion
}
