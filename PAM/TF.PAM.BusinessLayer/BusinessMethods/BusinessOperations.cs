
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: BusinessOperations.cs
  Description: This is the business layer for the PAM to perform all business operations.
  Date Created : 16-Aug-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

#endregion

namespace TF.PAM.BusinessLayer.BusinessMethods
{
    #region Internal Namespaces

    using BusinessObjects;
    using DataAccessLayer;

    #endregion

    #region BusinessOperations

    /// <summary>
    /// This is the business layer for the PAM to perform all business operations.
    /// </summary>
    public partial class BusinessOperations : IDisposable
    {
        #region Member Variables

        private bool isDisposed = false;                    //This is for Garbage Collector.
        private DAL dataAccessLayer = null;                 //Data Access Layer object to communicate with database.

        #endregion

        #region Constructor

        /// <summary>
        /// This is the Constructor for the class that will instantiate the data access layer.
        /// </summary>
        /// <param name="connectionStringName">Database connection string information.</param>
        public BusinessOperations(string connectionStringName)
        {
            this.dataAccessLayer = new DAL(connectionStringName);
        }

        #endregion

        #region Destructor

        /// <summary>
        /// This is the Destructor for the class.
        /// </summary>
        ~BusinessOperations()
        {
            //Since finalizer is called, there is no need to free the managed resources. So, false is passed.
            Dispose(false);
        }

        #endregion

        #region GetUserInformation

        /// <summary>
        /// Gets the user related information like encrypted password and salt.
        /// </summary>
        /// <param name="userName">User Name.</param>
        /// <param name="password">Plain text password.</param>
        /// <returns>User information related to password.</returns>
        public UserInformation GetUserInformation(string userName, string password)
        {            
            UserInformation userInformation = null;

            //This is for identifying the database operation execution status.
            int responseCode = 0;  

            try
            {
                using (SqlCommand userInformationCommand = dataAccessLayer.GetSqlCommand(SP_GET_USER_INFORMATION, ref responseCode,
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_INFORMATION_IN_PARAM_USER_NAME, SqlDbType.VarChar, userName),
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_INFORMATION_IN_PARAM_USER_PASSWORD, SqlDbType.VarChar, password),
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_INFORMATION_OUT_PARAM_ENCRYPTED_PASSWORD, SqlDbType.VarChar, 50, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_INFORMATION_OUT_PARAM_PASSWORD_SALT, SqlDbType.VarChar, 50, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_INFORMATION_OUT_PARAM_UNLOCK_MINUTES, SqlDbType.TinyInt, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_INFORMATION_OUT_PARAM_USER_ID, SqlDbType.BigInt, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_INFORMATION_OUT_PARAM_COMPANY_ID, SqlDbType.BigInt, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_INFORMATION_OUT_PARAM_MERCHANT_ID, SqlDbType.BigInt, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_INFORMATION_OUT_PARAM_ATTENDANT_ID, SqlDbType.BigInt, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_INFORMATION_OUT_PARAM_RESPONSE_CODE, SqlDbType.Int, ParameterDirection.Output)))
                {

                    //If the database operation execution is successful. 
                    if (responseCode == 0)
                    {
                        userInformation = new UserInformation
                        {
                            EncryptedPassword           = userInformationCommand.Parameters[SP_GET_USER_INFORMATION_OUT_PARAM_ENCRYPTED_PASSWORD].Value.ToString(),
                            PasswordSalt                = userInformationCommand.Parameters[SP_GET_USER_INFORMATION_OUT_PARAM_PASSWORD_SALT].Value.ToString(),
                            UnlockMinutes               = GetNumericByteValue(userInformationCommand.Parameters[SP_GET_USER_INFORMATION_OUT_PARAM_UNLOCK_MINUTES].Value.ToString()),
                            UserId                      = GetNumericLongValue(userInformationCommand.Parameters[SP_GET_USER_INFORMATION_OUT_PARAM_USER_ID].Value.ToString()),
                            CompanyId                   = GetNumericLongValue(userInformationCommand.Parameters[SP_GET_USER_INFORMATION_OUT_PARAM_COMPANY_ID].Value.ToString()),
                            MerchantId                  = GetNumericLongValue(userInformationCommand.Parameters[SP_GET_USER_INFORMATION_OUT_PARAM_MERCHANT_ID].Value.ToString()),
                            AttendantId                 = GetNumericLongValue(userInformationCommand.Parameters[SP_GET_USER_INFORMATION_OUT_PARAM_ATTENDANT_ID].Value.ToString()),
                            ResponseCode                = GetNumericIntegerValue(userInformationCommand.Parameters[SP_GET_USER_INFORMATION_OUT_PARAM_RESPONSE_CODE].Value.ToString())
                        };                        
                    }
                    //If the database operation execution is failed due to database error or system error.
                    else if (responseCode == -1 || responseCode == -2)
                    {
                        //Send null user information.
                        userInformation = null;
                    }
                }
            }
            catch (Exception)
            {
                userInformation = null;
            }

            //Return user information.
            return userInformation;
        }

        #endregion        

        #region GetAuthenticationRulesInformation

        /// <summary>
        /// Gets the users authentication rules information based on whether the password is correct or not.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="isCorrectPassword">Flag that indicates whether the password is valid/correct or not.</param>
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Valid user password.</description>
        ///     </item>
        ///     <item>
        ///         <description>False -> Invalid user password.</description>
        ///     </item>      
        /// </list>    
        /// <returns>Users authentication rules information.</returns>
        public AuthenticationRulesInformation GetAuthenticationRulesInformation(long userId, bool isCorrectPassword)
        {
            AuthenticationRulesInformation authenticationRulesInformation = null;

            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                using (SqlCommand authenticationRulesInformationCommand = dataAccessLayer.GetSqlCommand(SP_APPLY_USER_AUTHENTICATION_RULES, ref responseCode,
                                                  dataAccessLayer.CreateParameter(SP_APPLY_USER_AUTHENTICATION_RULES_IN_PARAM_USER_ID, SqlDbType.BigInt, userId),
                                                  dataAccessLayer.CreateParameter(SP_APPLY_USER_AUTHENTICATION_RULES_IN_PARAM_IS_CORRECT_PASSWORD, SqlDbType.Bit, isCorrectPassword),
                                                  dataAccessLayer.CreateParameter(SP_APPLY_USER_AUTHENTICATION_RULES_OUT_PARAM_LEFT_OUT_CHANGE_PWD_DAYS, SqlDbType.Int, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_APPLY_USER_AUTHENTICATION_RULES_OUT_PARAM_LEFT_OUT_INVALID_PWD_ATTEMPTS, SqlDbType.TinyInt, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_APPLY_USER_AUTHENTICATION_RULES_OUT_PARAM_UNLOCK_MINUTES, SqlDbType.TinyInt, ParameterDirection.Output),                                                  
                                                  dataAccessLayer.CreateParameter(SP_APPLY_USER_AUTHENTICATION_RULES_OUT_PARAM_RESPONSE_CODE, SqlDbType.Int, ParameterDirection.Output)))
                {

                    //If the database operation execution is successful. 
                    if (responseCode == 0)
                    {
                        authenticationRulesInformation = new AuthenticationRulesInformation
                        {
                            LeftOutForceChangePasswordDays  = GetNumericIntegerValue(authenticationRulesInformationCommand.Parameters[SP_APPLY_USER_AUTHENTICATION_RULES_OUT_PARAM_LEFT_OUT_CHANGE_PWD_DAYS].Value.ToString()),
                            LeftOutInvalidPasswordAttempts  = GetNumericByteValue(authenticationRulesInformationCommand.Parameters[SP_APPLY_USER_AUTHENTICATION_RULES_OUT_PARAM_LEFT_OUT_INVALID_PWD_ATTEMPTS].Value.ToString()),
                            UnlockMinutes                   = GetNumericByteValue(authenticationRulesInformationCommand.Parameters[SP_APPLY_USER_AUTHENTICATION_RULES_OUT_PARAM_UNLOCK_MINUTES].Value.ToString()),
                            ResponseCode                    = GetNumericIntegerValue(authenticationRulesInformationCommand.Parameters[SP_APPLY_USER_AUTHENTICATION_RULES_OUT_PARAM_RESPONSE_CODE].Value.ToString())
                        };
                    }
                    //If the database operation execution is failed due to database error or system error.
                    else if (responseCode == -1 || responseCode == -2)
                    {
                        //Send null authentication rules information.
                        authenticationRulesInformation = null;
                    }
                }
            }
            catch (Exception)
            {
                authenticationRulesInformation = null;
            }

            //Return authentication rules information.
            return authenticationRulesInformation;
        }

        #endregion

        #region ApplyPasswordValidationRules

        /// <summary>
        /// Applies the password validation rules for the given password and sends back the password regular expression if any.
        /// </summary>
        /// <param name="userName">User Name.</param>
        /// <param name="password">Plain text password.</param>
        /// <returns>Password validation information.</returns>
        public PasswordValidationInformation ApplyPasswordValidationRules(string userName, string password)
        {
            PasswordValidationInformation passwordValidationInformation = null;

            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                using (SqlCommand passwordValidationInformationCommand = dataAccessLayer.GetSqlCommand(SP_APPLY_USER_PASSWORD_RULES, ref responseCode,
                                                  dataAccessLayer.CreateParameter(SP_APPLY_USER_PASSWORD_RULES_IN_PARAM_USER_NAME, SqlDbType.VarChar, userName),
                                                  dataAccessLayer.CreateParameter(SP_APPLY_USER_PASSWORD_RULES_IN_PARAM_USER_PASSWORD, SqlDbType.VarChar, password),
                                                  dataAccessLayer.CreateParameter(SP_APPLY_USER_PASSWORD_RULES_OUT_PARAM_PASSWORD_EXPRESSION, SqlDbType.VarChar, 200, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_APPLY_USER_PASSWORD_RULES_OUT_PARAM_USER_ID, SqlDbType.BigInt, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_APPLY_USER_PASSWORD_RULES_OUT_PARAM_RESPONSE_CODE, SqlDbType.Int, ParameterDirection.Output)))
                {

                    //If the database operation execution is successful. 
                    if (responseCode == 0)
                    {
                        passwordValidationInformation = new PasswordValidationInformation
                        {
                            PasswordRegularExpression   = passwordValidationInformationCommand.Parameters[SP_APPLY_USER_PASSWORD_RULES_OUT_PARAM_PASSWORD_EXPRESSION].Value.ToString(),
                            UserId                      = GetNumericLongValue(passwordValidationInformationCommand.Parameters[SP_APPLY_USER_PASSWORD_RULES_OUT_PARAM_USER_ID].Value.ToString()),
                            ResponseCode                = GetNumericIntegerValue(passwordValidationInformationCommand.Parameters[SP_APPLY_USER_PASSWORD_RULES_OUT_PARAM_RESPONSE_CODE].Value.ToString())
                        };
                    }
                    //If the database operation execution is failed due to database error or system error.
                    else if (responseCode == -1 || responseCode == -2)
                    {
                        //Send null password validation information.
                        passwordValidationInformation = null;
                    }
                }
            }
            catch (Exception)
            {
                passwordValidationInformation = null;
            }

            //Return password validation information.
            return passwordValidationInformation;
        }

        #endregion        

        #region GetPasswordHistoryInformation

        /// <summary>
        /// Gets the previous password history data for the given user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>Password history data for the user.</returns>
        public PasswordHistoryInformationData GetPasswordHistoryInformation(long userId)
        {
            PasswordHistoryInformationData passwordHistoryInformationData = null;

            //This is for identifying the database operation execution status.
            int responseCode = 0;

            //Parameter for Output value.
            SqlParameter outputParameter = null;

            try
            {
                //Create the ouptput parameter to retrieve the same password usage limit.
                outputParameter = dataAccessLayer.CreateParameter(SP_GET_PASSWORD_HISTORY_OUT_PARAM_SAME_PASSWORD_USAGE_RULE,
                                                                                SqlDbType.Int, ParameterDirection.Output);

                using (DataSet passwordHistoryInformationDataSet = dataAccessLayer.GetDataSet(SP_GET_PASSWORD_HISTORY, ref responseCode,
                                                               dataAccessLayer.CreateParameter(SP_GET_PASSWORD_HISTORY_IN_PARAM_USER_ID, SqlDbType.BigInt, userId),
                                                               outputParameter))
                {
                    //If the database operation execution is successful. 
                    if (responseCode == 0)
                    {
                        //If the dataset is not null and if it doesn't have any errors.
                        if (passwordHistoryInformationDataSet != null && !passwordHistoryInformationDataSet.HasErrors)
                        {
                            //Count check.
                            if (passwordHistoryInformationDataSet.Tables.Count == 1)
                            {
                                //If data exists.
                                if (passwordHistoryInformationDataSet.Tables[0].Rows.Count > 0)
                                {
                                    //Initialize the password history information data.
                                    passwordHistoryInformationData = new PasswordHistoryInformationData
                                    {
                                        PasswordHistoryInformationCollection    = new List<PasswordHistoryInformation>(),
                                        SamePasswordUsageLimit                  = GetNumericIntegerValue(outputParameter.Value.ToString())
                                    };

                                    //Extract the encrypted password and salt from each record.
                                    foreach (DataRow passwordRow in passwordHistoryInformationDataSet.Tables[0].Rows)
                                    {
                                        PasswordHistoryInformation passwordInformation = new PasswordHistoryInformation
                                        {
                                            EncryptedPassword   = passwordRow[0].ToString(),
                                            PasswordSalt        = passwordRow[1].ToString()
                                        };

                                        //Add the password history to collection.
                                        passwordHistoryInformationData.PasswordHistoryInformationCollection.Add(passwordInformation);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Send null password history information data.
                            passwordHistoryInformationData = null;
                        }
                    }
                    //If the database operation execution is failed due to database error or system error.
                    else if (responseCode == -1 || responseCode == -2)
                    {
                        //Send null password history information data.
                        passwordHistoryInformationData = null;
                    }
                }
            }
            catch (Exception)
            {
                passwordHistoryInformationData = null;
            }

            finally
            {
                //Nullify the output parameter.
                if (outputParameter != null)
                {
                    outputParameter = null;
                }
            }

            //Return password history information data.
            return passwordHistoryInformationData;
        }

        #endregion

        #region ManageUserSession

        /// <summary>
        /// Creates/Updates the session-id for the specified user.
        /// </summary>
        /// <param name="userId">User Identity.</param>
        /// <param name="sessionId">Session-Id.</param>
        /// <returns>Scalar code that details the status of the session-id creation/updation.</returns>
        public int ManageUserSession(long userId, string sessionId)
        {
            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                int rowsEffected = dataAccessLayer.ExecuteNonQuery(SP_MANAGE_USER_SESSION, ref responseCode,
                                                    dataAccessLayer.CreateParameter(SP_MANAGE_USER_SESSION_IN_PARAM_USER_ID, SqlDbType.BigInt, userId),
                                                    dataAccessLayer.CreateParameter(SP_MANAGE_USER_SESSION_IN_PARAM_SESSION_ID, SqlDbType.VarChar, sessionId));

                //If more than zero rows effected, operation is successful.
                if (rowsEffected > 0)
                    responseCode = 0;
                //Else, return as database error.
                else
                    responseCode = 101;
            }
            catch (Exception)
            {
                //Send response code as Chanakya Error.
                responseCode = 103;
            }

            return responseCode;
        }

        #endregion        

        #region ManageUserActivity

        /// <summary>
        /// Creates/Updates the activity time for the specified user.
        /// </summary>
        /// <param name="userId">User Identity.</param>
        /// <returns>Scalar code that details the status of the activity time creation/updation.</returns>
        public int ManageUserActivity(long userId)
        {
            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                int rowsEffected = dataAccessLayer.ExecuteNonQuery(SP_MANAGE_USER_ACTIVITY, ref responseCode,
                                                dataAccessLayer.CreateParameter(SP_MANAGE_USER_ACTIVITY_IN_PARAM_USER_ID, SqlDbType.BigInt, userId));

                //If more than zero rows effected, operation is successful.
                if (rowsEffected > 0)
                    responseCode = 0;
                //Else, return as database error.
                else
                    responseCode = 101;
            }
            catch (Exception)
            {
                //Send response code as Chanakya Error.
                responseCode = 103;
            }

            return responseCode;
        }

        #endregion        

        #region GetUserPrivileges

        /// <summary>
        /// Gets the privilege information for the given user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>User privilege information.</returns>
        public UserPrivilegeInformation GetUserPrivileges(long userId)
        {
            UserPrivilegeInformation userPrivilegeInformation = null;

            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                using (SqlCommand userPrivilegeInformationCommand = dataAccessLayer.GetSqlCommand(SP_GET_USER_PRIVILEGES, ref responseCode,
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_PRIVILEGES_IN_PARAM_USER_ID, SqlDbType.BigInt, userId),
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_PRIVILEGES_OUT_PARAM_PRIVILEGE_LEVEL, SqlDbType.Int, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_PRIVILEGES_OUT_PARAM_IS_ADMIN, SqlDbType.Bit, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_PRIVILEGES_OUT_PARAM_IS_PRIMITIVE, SqlDbType.Bit, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_PRIVILEGES_OUT_PARAM_ROLE_ID, SqlDbType.SmallInt, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_PRIVILEGES_OUT_PARAM_ADMIN_ROLE_ID, SqlDbType.SmallInt, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_USER_PRIVILEGES_OUT_PARAM_RESPONSE_CODE, SqlDbType.Int, ParameterDirection.Output)))
                {
                    //If the database operation execution is successful. 
                    if (responseCode == 0)
                    {
                        userPrivilegeInformation = new UserPrivilegeInformation
                        {
                            PrivilegeLevel  = GetNumericIntegerValue(userPrivilegeInformationCommand.Parameters[SP_GET_USER_PRIVILEGES_OUT_PARAM_PRIVILEGE_LEVEL].Value.ToString()),
                            IsAdmin         = GetBooleanValue(userPrivilegeInformationCommand.Parameters[SP_GET_USER_PRIVILEGES_OUT_PARAM_IS_ADMIN].Value.ToString()),
                            IsPrimitive     = GetBooleanValue(userPrivilegeInformationCommand.Parameters[SP_GET_USER_PRIVILEGES_OUT_PARAM_IS_PRIMITIVE].Value.ToString()),
                            RoleId          = GetNumericShortValue(userPrivilegeInformationCommand.Parameters[SP_GET_USER_PRIVILEGES_OUT_PARAM_ROLE_ID].Value.ToString()),
                            AdminRoleId     = GetNumericShortValue(userPrivilegeInformationCommand.Parameters[SP_GET_USER_PRIVILEGES_OUT_PARAM_ADMIN_ROLE_ID].Value.ToString()),
                            ResponseCode    = GetNumericIntegerValue(userPrivilegeInformationCommand.Parameters[SP_GET_USER_PRIVILEGES_OUT_PARAM_RESPONSE_CODE].Value.ToString())
                        };
                    }
                    //If the database operation execution is failed due to database error or system error.
                    else if (responseCode == -1 || responseCode == -2)
                    {
                        //Send null user privilege information.
                        userPrivilegeInformation = null;
                    }
                }
            }
            catch (Exception)
            {
                userPrivilegeInformation = null;
            }

            //Return user privilege information.
            return userPrivilegeInformation;
        }

        #endregion        

        #region GetItemMask

        /// <summary>
        /// Gets the item mask information data for the given privilege level.
        /// </summary>
        /// <param name="privilegeLevel">Privilege level.</param>
        /// <param name="isPrimitiveRole">Flag that indicates whether the user role is Primitive or not.</param>
        /// <returns>Item mask information data.</returns>
        public List<ItemMaskInformation> GetItemMask(int privilegeLevel, bool isPrimitiveRole)
        {
            List<ItemMaskInformation> itemMaskInformationCollection = null;

            //This is for identifying the database operation execution status.
            int responseCode = 0;            

            try
            {
                using (DataSet itemMaskInformationDataSet = dataAccessLayer.GetDataSet(SP_GET_ITEM_MASK, ref responseCode,
                                                               dataAccessLayer.CreateParameter(SP_GET_ITEM_MASK_IN_PARAM_PRIVILEGE_LEVEL, SqlDbType.Int, privilegeLevel),
                                                               dataAccessLayer.CreateParameter(SP_GET_ITEM_MASK_IN_PARAM_IS_PRIMITIVE, SqlDbType.Bit, isPrimitiveRole)))
                {
                    //If the database operation execution is successful. 
                    if (responseCode == 0)
                    {
                        //If the dataset is not null and if it doesn't have any errors.
                        if (itemMaskInformationDataSet != null && !itemMaskInformationDataSet.HasErrors)
                        {
                            //Count check.
                            if (itemMaskInformationDataSet.Tables.Count == 1)
                            {
                                //If data exists.
                                if (itemMaskInformationDataSet.Tables[0].Rows.Count > 0)
                                {
                                    //Initialize the item mask information data.
                                    itemMaskInformationCollection = new List<ItemMaskInformation>();

                                    //Extract the item mask information from each record.
                                    foreach (DataRow itemMaskRow in itemMaskInformationDataSet.Tables[0].Rows)
                                    {
                                        ItemMaskInformation itemMaskInformation = new ItemMaskInformation
                                        {
                                            ItemId              = GetNumericShortValue(itemMaskRow[0].ToString()),
                                            ItemName            = itemMaskRow[1].ToString(),
                                            IsCreateAllowed     = GetBooleanValue(itemMaskRow[2].ToString()),
                                            IsEditAllowed       = GetBooleanValue(itemMaskRow[3].ToString()),
                                            IsDeleteAllowed     = GetBooleanValue(itemMaskRow[4].ToString()),
                                            IsViewAllowed       = GetBooleanValue(itemMaskRow[5].ToString())
                                        };                                      

                                        //Add the item mask information to collection.
                                        itemMaskInformationCollection.Add(itemMaskInformation);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Send null item mask information data.
                            itemMaskInformationCollection = null;
                        }
                    }
                    //If the database operation execution is failed due to database error or system error.
                    else if (responseCode == -1 || responseCode == -2)
                    {
                        //Send null item mask information data.
                        itemMaskInformationCollection = null;
                    }
                }
            }
            catch (Exception)
            {
                itemMaskInformationCollection = null;
            }

            //Return item mask information data.
            return itemMaskInformationCollection;
        }

        #endregion

        #region GetItemValues

        /// <summary>
        /// Gets the item values information data for the given privilege level.
        /// </summary>
        /// <param name="privilegeLevel">Privilege level.</param>
        /// <returns>Item values information data.</returns>
        public List<ItemValuesInformation> GetItemValues(int privilegeLevel)
        {
            List<ItemValuesInformation> itemValuesInformationCollection = null;

            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                using (DataSet itemValuesInformationDataSet = dataAccessLayer.GetDataSet(SP_GET_ITEM_VALUES, ref responseCode,
                                                               dataAccessLayer.CreateParameter(SP_GET_ITEM_VALUES_IN_PARAM_PRIVILEGE_LEVEL, SqlDbType.Int, privilegeLevel)))
                                                               
                {
                    //If the database operation execution is successful. 
                    if (responseCode == 0)
                    {
                        //If the dataset is not null and if it doesn't have any errors.
                        if (itemValuesInformationDataSet != null && !itemValuesInformationDataSet.HasErrors)
                        {
                            //Count check.
                            if (itemValuesInformationDataSet.Tables.Count == 1)
                            {
                                //If data exists.
                                if (itemValuesInformationDataSet.Tables[0].Rows.Count > 0)
                                {
                                    //Initialize the item values information data.
                                    itemValuesInformationCollection = new List<ItemValuesInformation>();

                                    //Extract the item values information from each record.
                                    foreach (DataRow itemValuesRow in itemValuesInformationDataSet.Tables[0].Rows)
                                    {
                                        ItemValuesInformation itemValuesInformation = new ItemValuesInformation
                                        {
                                            ItemId              = GetNumericShortValue(itemValuesRow[0].ToString()),
                                            ItemName            = itemValuesRow[1].ToString(),
                                            IsCreateAllowed     = GetBooleanValue(itemValuesRow[2].ToString()),
                                            IsEditAllowed       = GetBooleanValue(itemValuesRow[3].ToString()),
                                            IsDeleteAllowed     = GetBooleanValue(itemValuesRow[4].ToString()),
                                            IsViewAllowed       = GetBooleanValue(itemValuesRow[5].ToString())
                                        };

                                        //Add the item values information to collection.
                                        itemValuesInformationCollection.Add(itemValuesInformation);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Send null item values information data.
                            itemValuesInformationCollection = null;
                        }
                    }
                    //If the database operation execution is failed due to database error or system error.
                    else if (responseCode == -1 || responseCode == -2)
                    {
                        //Send null item values information data.
                        itemValuesInformationCollection = null;
                    }
                }
            }
            catch (Exception)
            {
                itemValuesInformationCollection = null;
            }

            //Return item values information data.
            return itemValuesInformationCollection;
        }

        #endregion

        #region GetUserItemValues

        /// <summary>
        /// Gets the user item values information data for the given role id.
        /// </summary>
        /// <param name="roleId">User role id.</param>
        /// <returns>User item values information data.</returns>
        public List<ItemValuesInformation> GetUserItemValues(short roleId)
        {
            List<ItemValuesInformation> userItemValuesInformationCollection = null;

            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                using (DataSet userItemValuesInformationDataSet = dataAccessLayer.GetDataSet(SP_GET_USER_ITEM_VALUES, ref responseCode,
                                                               dataAccessLayer.CreateParameter(SP_GET_USER_ITEM_VALUES_IN_PARAM_ROLE_ID, SqlDbType.SmallInt, roleId)))
                {
                    //If the database operation execution is successful. 
                    if (responseCode == 0)
                    {
                        //If the dataset is not null and if it doesn't have any errors.
                        if (userItemValuesInformationDataSet != null && !userItemValuesInformationDataSet.HasErrors)
                        {
                            //Count check.
                            if (userItemValuesInformationDataSet.Tables.Count == 1)
                            {
                                //If data exists.
                                if (userItemValuesInformationDataSet.Tables[0].Rows.Count > 0)
                                {
                                    //Initialize the user item values information data.
                                    userItemValuesInformationCollection = new List<ItemValuesInformation>();

                                    //Extract the user item values information from each record.
                                    foreach (DataRow userItemValuesRow in userItemValuesInformationDataSet.Tables[0].Rows)
                                    {
                                        ItemValuesInformation userItemValuesInformation = new ItemValuesInformation
                                        {
                                            ItemId              = GetNumericShortValue(userItemValuesRow[0].ToString()),
                                            ItemName            = userItemValuesRow[1].ToString(),
                                            IsCreateAllowed     = GetBooleanValue(userItemValuesRow[2].ToString()),
                                            IsEditAllowed       = GetBooleanValue(userItemValuesRow[3].ToString()),
                                            IsDeleteAllowed     = GetBooleanValue(userItemValuesRow[4].ToString()),
                                            IsViewAllowed       = GetBooleanValue(userItemValuesRow[5].ToString())
                                        };

                                        //Add the user item values information to collection.
                                        userItemValuesInformationCollection.Add(userItemValuesInformation);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Send null user item values information data.
                            userItemValuesInformationCollection = null;
                        }
                    }
                    //If the database operation execution is failed due to database error or system error.
                    else if (responseCode == -1 || responseCode == -2)
                    {
                        //Send null user item values information data.
                        userItemValuesInformationCollection = null;
                    }
                }
            }
            catch (Exception)
            {
                userItemValuesInformationCollection = null;
            }

            //Return user item values information data.
            return userItemValuesInformationCollection;
        }

        #endregion

        #region InsertItemInformation

        /// <summary>
        /// Inserts the new item information to the given user role id.
        /// </summary>
        /// <param name="itemInformation">New item information.</param>
        /// <returns>Scalar code that details the status of the item information insertion.</returns>
        public int InsertItemInformation(NewItemInformation itemInformation)
        {
            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                int rowsEffected = dataAccessLayer.ExecuteNonQuery(SP_APPEND_ITEM, ref responseCode,
                                                dataAccessLayer.CreateParameter(SP_APPEND_ITEM_IN_PARAM_ROLE_ID, SqlDbType.SmallInt, itemInformation.RoleId),
                                                dataAccessLayer.CreateParameter(SP_APPEND_ITEM_IN_PARAM_ITEM_ID, SqlDbType.SmallInt, itemInformation.ItemId),
                                                dataAccessLayer.CreateParameter(SP_APPEND_ITEM_IN_PARAM_CREATE_ALLOWED, SqlDbType.Bit, itemInformation.IsCreateAllowed),
                                                dataAccessLayer.CreateParameter(SP_APPEND_ITEM_IN_PARAM_EDIT_ALLOWED, SqlDbType.Bit, itemInformation.IsEditAllowed),
                                                dataAccessLayer.CreateParameter(SP_APPEND_ITEM_IN_PARAM_DELETE_ALLOWED, SqlDbType.Bit, itemInformation.IsDeleteAllowed),
                                                dataAccessLayer.CreateParameter(SP_APPEND_ITEM_IN_PARAM_VIEW_ALLOWED, SqlDbType.Bit, itemInformation.IsViewAllowed),
                                                dataAccessLayer.CreateParameter(SP_APPEND_ITEM_IN_PARAM_USER_ID, SqlDbType.BigInt, itemInformation.UserId));

                //If more than zero rows effected, operation is successful.
                if (rowsEffected > 0)
                    responseCode = 0;
                //Else, return as database error.
                else
                    responseCode = 101;
            }
            catch (Exception)
            {
                //Send response code as Chanakya Error.
                responseCode = 103;
            }

            return responseCode;
        }

        #endregion        

        #region GetRolePrivileges

        /// <summary>
        /// Gets the privilege information for the given role id.
        /// </summary>
        /// <param name="roleId">Role ID.</param>
        /// <returns>Role privilege information.</returns>
        public RolePrivilegeInformation GetRolePrivileges(short roleId)
        {
            RolePrivilegeInformation rolePrivilegeInformation = null;

            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                using (SqlCommand rolePrivilegeInformationCommand = dataAccessLayer.GetSqlCommand(SP_GET_ROLE_PRIVILEGE_LEVEL, ref responseCode,
                                                  dataAccessLayer.CreateParameter(SP_GET_ROLE_PRIVILEGE_LEVEL_IN_PARAM_ROLE_ID, SqlDbType.SmallInt, roleId),
                                                  dataAccessLayer.CreateParameter(SP_GET_ROLE_PRIVILEGE_LEVEL_OUT_PARAM_PRIVILEGE_LEVEL, SqlDbType.Int, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_ROLE_PRIVILEGE_LEVEL_OUT_PARAM_IS_ADMIN, SqlDbType.Bit, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_ROLE_PRIVILEGE_LEVEL_OUT_PARAM_IS_PRIMITIVE, SqlDbType.Bit, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_ROLE_PRIVILEGE_LEVEL_OUT_PARAM_ROLE_NAME, SqlDbType.VarChar, 50, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_ROLE_PRIVILEGE_LEVEL_OUT_PARAM_RESPONSE_CODE, SqlDbType.Int, ParameterDirection.Output)))
                {
                    //If the database operation execution is successful. 
                    if (responseCode == 0)
                    {
                        rolePrivilegeInformation = new RolePrivilegeInformation
                        {
                            PrivilegeLevel      = GetNumericIntegerValue(rolePrivilegeInformationCommand.Parameters[SP_GET_ROLE_PRIVILEGE_LEVEL_OUT_PARAM_PRIVILEGE_LEVEL].Value.ToString()),
                            IsAdmin             = GetBooleanValue(rolePrivilegeInformationCommand.Parameters[SP_GET_ROLE_PRIVILEGE_LEVEL_OUT_PARAM_IS_ADMIN].Value.ToString()),
                            IsPrimitive         = GetBooleanValue(rolePrivilegeInformationCommand.Parameters[SP_GET_ROLE_PRIVILEGE_LEVEL_OUT_PARAM_IS_PRIMITIVE].Value.ToString()),
                            RoleName            = rolePrivilegeInformationCommand.Parameters[SP_GET_ROLE_PRIVILEGE_LEVEL_OUT_PARAM_ROLE_NAME].Value.ToString().Trim(),
                            ResponseCode        = GetNumericIntegerValue(rolePrivilegeInformationCommand.Parameters[SP_GET_ROLE_PRIVILEGE_LEVEL_OUT_PARAM_RESPONSE_CODE].Value.ToString())
                        };                        
                    }
                    //If the database operation execution is failed due to database error or system error.
                    else if (responseCode == -1 || responseCode == -2)
                    {
                        //Send null role privilege information.
                        rolePrivilegeInformation = null;
                    }
                }
            }
            catch (Exception)
            {
                rolePrivilegeInformation = null;
            }

            //Return role privilege information.
            return rolePrivilegeInformation;
        }

        #endregion        

        #region GetUserRolePrivileges

        /// <summary>
        /// Gets the privilege information for the given users role.
        /// </summary>
        /// <param name="roleId">Role ID.</param>
        /// <returns>User privilege information.</returns>
        public UserPrivilegeInformation GetUserRolePrivileges(short roleId)
        {
            UserPrivilegeInformation userRolePrivilegeInformation = null;

            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                using (SqlCommand userRolePrivilegeInformationCommand = dataAccessLayer.GetSqlCommand(SP_GET_ROLE_PRIVILEGES, ref responseCode,
                                                  dataAccessLayer.CreateParameter(SP_GET_ROLE_PRIVILEGES_IN_PARAM_ROLE_ID, SqlDbType.SmallInt, roleId),
                                                  dataAccessLayer.CreateParameter(SP_GET_ROLE_PRIVILEGES_OUT_PARAM_PRIVILEGE_LEVEL, SqlDbType.Int, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_ROLE_PRIVILEGES_OUT_PARAM_IS_ADMIN, SqlDbType.Bit, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_ROLE_PRIVILEGES_OUT_PARAM_IS_PRIMITIVE, SqlDbType.Bit, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_ROLE_PRIVILEGES_OUT_PARAM_ADMIN_ROLE_ID, SqlDbType.SmallInt, ParameterDirection.Output),
                                                  dataAccessLayer.CreateParameter(SP_GET_ROLE_PRIVILEGES_OUT_PARAM_RESPONSE_CODE, SqlDbType.Int, ParameterDirection.Output)))
                {
                    //If the database operation execution is successful. 
                    if (responseCode == 0)
                    {
                        userRolePrivilegeInformation = new UserPrivilegeInformation
                        {
                            PrivilegeLevel  = GetNumericIntegerValue(userRolePrivilegeInformationCommand.Parameters[SP_GET_ROLE_PRIVILEGES_OUT_PARAM_PRIVILEGE_LEVEL].Value.ToString()),
                            IsAdmin         = GetBooleanValue(userRolePrivilegeInformationCommand.Parameters[SP_GET_ROLE_PRIVILEGES_OUT_PARAM_IS_ADMIN].Value.ToString()),
                            IsPrimitive     = GetBooleanValue(userRolePrivilegeInformationCommand.Parameters[SP_GET_ROLE_PRIVILEGES_OUT_PARAM_IS_PRIMITIVE].Value.ToString()),
                            AdminRoleId     = GetNumericShortValue(userRolePrivilegeInformationCommand.Parameters[SP_GET_ROLE_PRIVILEGES_OUT_PARAM_ADMIN_ROLE_ID].Value.ToString()),
                            ResponseCode    = GetNumericIntegerValue(userRolePrivilegeInformationCommand.Parameters[SP_GET_ROLE_PRIVILEGES_OUT_PARAM_RESPONSE_CODE].Value.ToString())
                        };
                    }
                    //If the database operation execution is failed due to database error or system error.
                    else if (responseCode == -1 || responseCode == -2)
                    {
                        //Send null users role privilege information.
                        userRolePrivilegeInformation = null;
                    }
                }
            }
            catch (Exception)
            {
                userRolePrivilegeInformation = null;
            }

            //Return users role privilege information.
            return userRolePrivilegeInformation;
        }

        #endregion        

        #region Dispose

        /// <summary>
        /// Implementation of dispose to free resources.
        /// </summary>
        /// <param name="disposedStatus">The status of the disposed operation.</param>
        protected virtual void Dispose(bool disposedStatus)
        {
            if (!isDisposed)
            {
                isDisposed = true;

                //Released unmanaged resources.
                if (disposedStatus)
                {
                    //Release the managed resources. 
                    
                    //Dispose the data access layer. 
                    if (dataAccessLayer != null)
                    {
                        dataAccessLayer.Dispose();
                        dataAccessLayer = null;
                    }
                }
            }
        }

        #endregion

        #region IDisposable Members

        #region Dispose

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //True is passed in dispose method to clean managed resources.
            Dispose(true);

            //If dispose is called already, then inform GC to skip finalize on this instance.
            GC.SuppressFinalize(this);
        }

        #endregion

        #endregion
    }

    #endregion
}
