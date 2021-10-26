
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: UserPrivilegeInformation.cs
  Description: This class is the business object representation for user privilege information.
  Date Created : 03-Jan-2011
  Revision History: 
  */

#endregion

namespace TF.PAM.BusinessLayer.BusinessObjects
{
    #region UserPrivilegeInformation

    /// <summary>
    /// This class is the business object representation for user privilege information.
    /// </summary>
    public class UserPrivilegeInformation
    {
        #region Member Variables

        //User privilege level.
        private int privilegeLevel;

        //Flag that indicates whether the user is Admin or not.
        private bool isAdmin;

        //Flag that indicates whether the user role is Primitive or not.
        private bool isPrimitive;

        //User role id.
        private short roleId;

        //Users admin role id.
        private short adminRoleId;

        //Response code.
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
        /// Allows to get/set the flag which indicates whether the user is Admin or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Admin.</description>
        ///     </item>
        ///     <item>
        ///         <description>False ->Non Admin.</description>
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

        #region RoleId

        /// <summary>
        /// Allows to get/set the users role id.
        /// </summary>
        public short RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        #endregion

        #region AdminRoleId

        /// <summary>
        /// Allows to get/set the users admin role id. If the user is admin, then RoleId and AdminRoleId are same.
        /// </summary>
        public short AdminRoleId
        {
            get { return adminRoleId; }
            set { adminRoleId = value; }
        }

        #endregion

        #region ResponseCode

        /// <summary>
        /// Allows to get/set the Response code.
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
