
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: AuthenticationRulesInformation.cs
  Description: This class is the business object representation for authentication rules information.
  Date Created : 17-Aug-2010
  Revision History: 
  */

#endregion

namespace TF.PAM.BusinessLayer.BusinessObjects
{
    #region AuthenticationRulesInformation

    /// <summary>
    /// This class is the business object representation for authentication rules information.
    /// </summary>
    public class AuthenticationRulesInformation
    {
        #region Member Variables

        private int leftOutForceChangePasswordDays;         //Number of days left for changing password.
        private byte leftOutInvalidPasswordAttempts;        //Number of password attempts left for the user before its get locked.
        private byte unlockMinutes;                         //Number of minutes left to unlock the user.
        private int responseCode;                           //Response code.

        #endregion

        #region Properties

        #region LeftOutForceChangePasswordDays

        /// <summary>
        /// Allows to get/set the number of days left for changing password.
        /// </summary>
        public int LeftOutForceChangePasswordDays
        {
            get { return leftOutForceChangePasswordDays; }
            set { leftOutForceChangePasswordDays = value; }
        }

        #endregion

        #region LeftOutInvalidPasswordAttempts

        /// <summary>
        /// Allows to get/set the number of password attempts left for the user before its get locked.
        /// </summary>
        public byte LeftOutInvalidPasswordAttempts
        {
            get { return leftOutInvalidPasswordAttempts; }
            set { leftOutInvalidPasswordAttempts = value; }
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
