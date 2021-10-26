
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: RolePrivilegeInformation.cs
  Description: This is the role privilege information object that encapsulates roles privilege level and admin status information.
  Date Created : 03-Jan-2011
  Revision History: 
  */

#endregion

namespace TF.PAM.BusinessLayer.BusinessObjects
{
    #region RolePrivilegeInformation

    /// <summary>
    /// This is the role privilege information object that encapsulates roles privilege level and admin status information.
    /// </summary>
    public class RolePrivilegeInformation
    {
        #region Member Variables

        //Users privilege level.
        private int privilegeLevel;

        //Flag that indicates whether the users role is Admin role or not.
        private bool isAdmin;

        //Flag that indicates whether the user role is Primitive or not.
        private bool isPrimitive;

        //Role Name.
        private string roleName;        

        //Response Code.
        private int responseCode;

        #endregion

        #region Properties

        #region PrivilegeLevel

        /// <summary>
        /// Allows to get/set the users privilege level.
        /// </summary>
        public int PrivilegeLevel
        {
            get { return privilegeLevel; }
            set { privilegeLevel = value; }
        }

        #endregion

        #region IsAdmin

        /// <summary>
        /// Allows to get/set the flag which indicates whether the users role is Admin role or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Admin Role.</description>
        ///     </item>
        ///     <item>
        ///         <description>False ->Non-Admin Role.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; }
        }

        #endregion

        #region IsPrimitive

        /// <summary>
        /// Allows to get/set the flag which indicates whether the user role is Primitive or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Primitive role.</description>
        ///     </item>
        ///     <item>
        ///         <description>False ->Non Primitive role.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public bool IsPrimitive
        {
            get { return isPrimitive; }
            set { isPrimitive = value; }
        }

        #endregion

        #region RoleName

        /// <summary>
        /// Allows to get/set the role name.
        /// </summary>
        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        #endregion

        #region ResponseCode

        /// <summary>
        /// Allows to get/set the Response Code.
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
