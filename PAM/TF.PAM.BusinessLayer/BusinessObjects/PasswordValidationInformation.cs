
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: PasswordValidationInformation.cs
  Description: This class is the business object representation for password validation information.
  Date Created : 18-Aug-2010
  Revision History: 
  */

#endregion

namespace TF.PAM.BusinessLayer.BusinessObjects
{
    #region PasswordValidationInformation

    /// <summary>
    /// This class is the business object representation for password validation information.
    /// </summary>
    public class PasswordValidationInformation
    {
        #region Member Variables

        private string passwordRegularExpression;   //Regular expression to validate the password.
        private long userId;                        //User ID.        
        private int responseCode;                   //Response code.

        #endregion

        #region Properties

        #region PasswordRegularExpression

        /// <summary>
        /// Allows to get/set the regular expression to validate the password.
        /// </summary>
        public string PasswordRegularExpression
        {
            get { return passwordRegularExpression; }
            set { passwordRegularExpression = value; }
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
