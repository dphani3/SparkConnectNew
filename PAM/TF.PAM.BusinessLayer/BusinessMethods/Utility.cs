
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Utility.cs
  Description: This is the partial class for BusinessOperations class to extend the utility functions for the main BusinessOperations class.
  Date Created : 16-Aug-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System.Globalization;

#endregion

namespace TF.PAM.BusinessLayer.BusinessMethods
{
    #region Utility

    /// <summary>
    /// This is the partial class for BusinessOperations class to extend the utility functions for the main BusinessOperations class.
    /// </summary>
    public partial class BusinessOperations
    {
        #region Stored Procedure Declarations

        #region PAM_GetUserInformation

        const string SP_GET_USER_INFORMATION                                                    = "PAM_GetUserInformation";

        const string SP_GET_USER_INFORMATION_IN_PARAM_USER_NAME                                 = "@UserName";
        const string SP_GET_USER_INFORMATION_IN_PARAM_USER_PASSWORD                             = "@UserPassword";

        const string SP_GET_USER_INFORMATION_OUT_PARAM_ENCRYPTED_PASSWORD                       = "@EncryptedPassword";
        const string SP_GET_USER_INFORMATION_OUT_PARAM_PASSWORD_SALT                            = "@PasswordSalt";
        const string SP_GET_USER_INFORMATION_OUT_PARAM_UNLOCK_MINUTES                           = "@UnLockMinutes";
        const string SP_GET_USER_INFORMATION_OUT_PARAM_USER_ID                                  = "@UserID";
        const string SP_GET_USER_INFORMATION_OUT_PARAM_COMPANY_ID                               = "@CompanyID";
        const string SP_GET_USER_INFORMATION_OUT_PARAM_MERCHANT_ID                              = "@MerchantID";
        const string SP_GET_USER_INFORMATION_OUT_PARAM_ATTENDANT_ID                             = "@AttendantID";
        const string SP_GET_USER_INFORMATION_OUT_PARAM_RESPONSE_CODE                            = "@ResponseCode";

        #endregion

        #region PAM_ApplyUserAuthenticationRules

        const string SP_APPLY_USER_AUTHENTICATION_RULES                                         = "PAM_ApplyUserAuthenticationRules";

        const string SP_APPLY_USER_AUTHENTICATION_RULES_IN_PARAM_USER_ID                        = "@UserID";
        const string SP_APPLY_USER_AUTHENTICATION_RULES_IN_PARAM_IS_CORRECT_PASSWORD            = "@IsCorrectPassword";

        const string SP_APPLY_USER_AUTHENTICATION_RULES_OUT_PARAM_LEFT_OUT_CHANGE_PWD_DAYS      = "@LeftoutForceChangePasswordDays";
        const string SP_APPLY_USER_AUTHENTICATION_RULES_OUT_PARAM_LEFT_OUT_INVALID_PWD_ATTEMPTS = "@LeftoutInvalidPasswordAttempts";
        const string SP_APPLY_USER_AUTHENTICATION_RULES_OUT_PARAM_UNLOCK_MINUTES                = "@UnLockMinutes";
        const string SP_APPLY_USER_AUTHENTICATION_RULES_OUT_PARAM_RESPONSE_CODE                 = "@ResponseCode";

        #endregion

        #region PAM_ApplyUserPasswordRules

        const string SP_APPLY_USER_PASSWORD_RULES                                               = "PAM_ApplyUserPasswordRules";

        const string SP_APPLY_USER_PASSWORD_RULES_IN_PARAM_USER_NAME                            = "@UserName";
        const string SP_APPLY_USER_PASSWORD_RULES_IN_PARAM_USER_PASSWORD                        = "@UserPassword";

        const string SP_APPLY_USER_PASSWORD_RULES_OUT_PARAM_PASSWORD_EXPRESSION                 = "@PasswordExpression";
        const string SP_APPLY_USER_PASSWORD_RULES_OUT_PARAM_USER_ID                             = "@UserID";
        const string SP_APPLY_USER_PASSWORD_RULES_OUT_PARAM_RESPONSE_CODE                       = "@ResponseCode";        

        #endregion        

        #region PAM_GetPasswordHistory

        const string SP_GET_PASSWORD_HISTORY                                                    = "PAM_GetPasswordHistory";

        const string SP_GET_PASSWORD_HISTORY_IN_PARAM_USER_ID                                   = "@UserID";

        const string SP_GET_PASSWORD_HISTORY_OUT_PARAM_SAME_PASSWORD_USAGE_RULE                 = "@SamePasswordUsageRule";

        #endregion

        #region PAM_ManageUserSession

        const string SP_MANAGE_USER_SESSION                                                     = "PAM_ManageUserSession";

        const string SP_MANAGE_USER_SESSION_IN_PARAM_USER_ID                                    = "@UserID";
        const string SP_MANAGE_USER_SESSION_IN_PARAM_SESSION_ID                                 = "@SessionID";

        #endregion    

        #region PAM_ManageUserActivity

        const string SP_MANAGE_USER_ACTIVITY                                                    = "PAM_ManageUserActivity";

        const string SP_MANAGE_USER_ACTIVITY_IN_PARAM_USER_ID                                   = "@UserID";
        
        #endregion    

        #region PAM_GetUserPrivileges

        const string SP_GET_USER_PRIVILEGES                                                     = "PAM_GetUserPrivileges";

        const string SP_GET_USER_PRIVILEGES_IN_PARAM_USER_ID                                    = "@UserID";
        
        const string SP_GET_USER_PRIVILEGES_OUT_PARAM_PRIVILEGE_LEVEL                           = "@PrivilegeLevel";
        const string SP_GET_USER_PRIVILEGES_OUT_PARAM_IS_ADMIN                                  = "@IsAdmin";
        const string SP_GET_USER_PRIVILEGES_OUT_PARAM_IS_PRIMITIVE                              = "@IsPrimitive";
        const string SP_GET_USER_PRIVILEGES_OUT_PARAM_ROLE_ID                                   = "@RoleID";
        const string SP_GET_USER_PRIVILEGES_OUT_PARAM_ADMIN_ROLE_ID                             = "@AdminRoleID";
        const string SP_GET_USER_PRIVILEGES_OUT_PARAM_RESPONSE_CODE                             = "@ResponseCode";

        #endregion   
 
        #region PAM_GetItemMask

        const string SP_GET_ITEM_MASK                                                           = "PAM_GetItemMask";

        const string SP_GET_ITEM_MASK_IN_PARAM_PRIVILEGE_LEVEL                                  = "@PrivilegeLevel";
        const string SP_GET_ITEM_MASK_IN_PARAM_IS_PRIMITIVE                                     = "@IsPrimitive";

        #endregion

        #region PAM_GetItemValues

        const string SP_GET_ITEM_VALUES                                                         = "PAM_GetItemValues";

        const string SP_GET_ITEM_VALUES_IN_PARAM_PRIVILEGE_LEVEL                                = "@PrivilegeLevel";        

        #endregion

        #region PAM_GetUserItemValues

        const string SP_GET_USER_ITEM_VALUES                                                    = "PAM_GetUserItemValues";

        const string SP_GET_USER_ITEM_VALUES_IN_PARAM_ROLE_ID                                   = "@RoleID";

        #endregion

        #region PAM_AppendItem

        const string SP_APPEND_ITEM                                                             = "PAM_AppendItem";

        const string SP_APPEND_ITEM_IN_PARAM_ROLE_ID                                            = "@RoleID";
        const string SP_APPEND_ITEM_IN_PARAM_ITEM_ID                                            = "@ItemID";
        const string SP_APPEND_ITEM_IN_PARAM_CREATE_ALLOWED                                     = "@CreateAllowed";
        const string SP_APPEND_ITEM_IN_PARAM_EDIT_ALLOWED                                       = "@EditAllowed";
        const string SP_APPEND_ITEM_IN_PARAM_DELETE_ALLOWED                                     = "@DeleteAllowed";
        const string SP_APPEND_ITEM_IN_PARAM_VIEW_ALLOWED                                       = "@ViewAllowed";
        const string SP_APPEND_ITEM_IN_PARAM_USER_ID                                            = "@UserID";                

        #endregion

        #region PAM_GetRolePrivilegeLevel

        const string SP_GET_ROLE_PRIVILEGE_LEVEL                                                = "PAM_GetRolePrivilegeLevel";

        const string SP_GET_ROLE_PRIVILEGE_LEVEL_IN_PARAM_ROLE_ID                               = "@RoleID";

        const string SP_GET_ROLE_PRIVILEGE_LEVEL_OUT_PARAM_PRIVILEGE_LEVEL                      = "@PrivilegeLevel";
        const string SP_GET_ROLE_PRIVILEGE_LEVEL_OUT_PARAM_IS_ADMIN                             = "@IsAdmin";
        const string SP_GET_ROLE_PRIVILEGE_LEVEL_OUT_PARAM_IS_PRIMITIVE                         = "@IsPrimitive";
        const string SP_GET_ROLE_PRIVILEGE_LEVEL_OUT_PARAM_ROLE_NAME                            = "@RoleName";
        const string SP_GET_ROLE_PRIVILEGE_LEVEL_OUT_PARAM_RESPONSE_CODE                        = "@ResponseCode";       

        #endregion 

        #region PAM_GetRolePrivileges

        const string SP_GET_ROLE_PRIVILEGES                                                     = "PAM_GetRolePrivileges";

        const string SP_GET_ROLE_PRIVILEGES_IN_PARAM_ROLE_ID                                    = "@RoleID";

        const string SP_GET_ROLE_PRIVILEGES_OUT_PARAM_PRIVILEGE_LEVEL                           = "@PrivilegeLevel";
        const string SP_GET_ROLE_PRIVILEGES_OUT_PARAM_IS_ADMIN                                  = "@IsAdmin";
        const string SP_GET_ROLE_PRIVILEGES_OUT_PARAM_IS_PRIMITIVE                              = "@IsPrimitive";
        const string SP_GET_ROLE_PRIVILEGES_OUT_PARAM_ROLE_ID                                   = "@RoleID";
        const string SP_GET_ROLE_PRIVILEGES_OUT_PARAM_ADMIN_ROLE_ID                             = "@AdminRoleID";
        const string SP_GET_ROLE_PRIVILEGES_OUT_PARAM_RESPONSE_CODE                             = "@ResponseCode";

        #endregion   

        #endregion

        #region GetNumericLongValue

        /// <summary>
        /// Gets the Int64 representation value from the given string representation.
        /// </summary>
        /// <param name="numericLongString">String representation of long value.</param>
        /// <returns>Int64 representation of given value.</returns>
        private long GetNumericLongValue(string numericLongString)
        {
            long numericLongValue;
            long.TryParse(numericLongString, NumberStyles.Number, CultureInfo.CurrentCulture, out numericLongValue);

            return numericLongValue;
        }

        #endregion

        #region GetNumericIntegerValue

        /// <summary>
        /// Gets the Int32 representation value from the given string representation.
        /// </summary>
        /// <param name="numericIntegerString">String representation of int value.</param>
        /// <returns>Int32 representation of given value.</returns>
        private int GetNumericIntegerValue(string numericIntegerString)
        {
            int numericIntegerValue;
            int.TryParse(numericIntegerString, NumberStyles.Number, CultureInfo.CurrentCulture, out numericIntegerValue);

            return numericIntegerValue;
        }

        #endregion

        #region GetNumericShortValue

        /// <summary>
        /// Gets the Int16 representation value from the given string representation.
        /// </summary>
        /// <param name="numericShortString">String representation of short value.</param>
        /// <returns>Int16 representation of given value.</returns>
        private short GetNumericShortValue(string numericShortString)
        {
            short numericShortValue;
            short.TryParse(numericShortString, NumberStyles.Number, CultureInfo.CurrentCulture, out numericShortValue);

            return numericShortValue;
        }

        #endregion

        #region GetNumericByteValue

        /// <summary>
        /// Gets the Byte representation value from the given string representation.
        /// </summary>
        /// <param name="numericByteString">String representation of byte value.</param>
        /// <returns>Byte representation of given value.</returns>
        private byte GetNumericByteValue(string numericByteString)
        {
            byte numericByteValue;
            byte.TryParse(numericByteString, NumberStyles.Number, CultureInfo.CurrentCulture, out numericByteValue);

            return numericByteValue;
        }

        #endregion        

        #region GetBooleanValue

        /// <summary>
        /// Gets the Boolean representation value from the given string representation.
        /// </summary>
        /// <param name="booleanString">String representation of boolean value.</param>
        /// <returns>Boolean representation of given value.</returns>
        private bool GetBooleanValue(string booleanString)
        {
            bool booleanValue = false;
            bool.TryParse(booleanString, out booleanValue);

            return booleanValue;
        }

        #endregion        
    }

    #endregion
}
