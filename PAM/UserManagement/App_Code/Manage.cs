
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Manage.asmx.cs
  Description: This is a web service that provides the interfaces for Password Audit Management (PAM).
  Date Created : 16-Aug-2010
  Revision History: 03-Oct-2010     Ketan Patel     Modified Audit Logging inputs AffectedId and EntityId
                    06-Jan-2011     KRISHNA NSS     Added Role Management support for PAM.
  */

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Services;
using log4net;
using TF.FocusPay.AuditLogger.BusinessObjects;
using TF.PAM.BusinessLayer.BusinessMethods;
using TF.PAM.BusinessLayer.BusinessObjects;
using AuditLogger = TF.FocusPay.AuditLogger.BusinessLayer;

#endregion

#region Manage

/// <summary>
/// This is a web service that provided the interfaces for Password Audit Management (PAM).
/// </summary>
[WebService(Namespace = "http://www.tfpayments.com/", Name = "Password Audit Management")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public partial class Manage : WebService
{
    #region Member Variables

    //Database connection string key that can be mapped with configuration file.
    const string DATABASE_CONNECTION_STRING_KEY = "PAMCON";

    //Database connection string.
    private string databaseConnectionString = string.Empty;

    //Business layer object.
    private BusinessOperations businessLayer = null;

    //log4net logger interface.
    private readonly ILog pamLogger = null;

    //Audit logger business layer object.
    private AuditLogger.BusinessOperations auditBusinessLayer = null;

    #endregion

    #region Constructor

    /// <summary>
    /// This is the Constructor for the service that will initialize the logger and database connection syting.
    /// </summary>
    public Manage()
    {
        //Initialize the logger.
        pamLogger = LogManager.GetLogger("PAM");

        //Initialize the database connection syting.
        databaseConnectionString = ConfigurationManager.ConnectionStrings[DATABASE_CONNECTION_STRING_KEY].ConnectionString.Trim();
    }

    #endregion

    #region CreatePassword

    /// <summary>
    /// This method creates the Plain text temporary password, Encrypted password and Password salt.
    /// </summary>
    /// <returns>Password information that contains the Plain text temporary password, Encrypted password and Password salt.</returns>
    [WebMethod(Description = "Creates the Plain text temporary password, Encrypted password and Password salt.")]
    public PasswordInformation CreatePassword()
    {
        pamLogger.Debug("Processing CreatePassword request.");

        //Initialize the password information structure.
        PasswordInformation passwordInformation = null;

        //Initialize the audit information structure.
        AuditDataInformation auditInformation = null;

        //Initialize the audit business layer.
        auditBusinessLayer = new TF.FocusPay.AuditLogger.BusinessLayer.BusinessOperations();

        try
        {
            //Generate the temporary password.
            string temporaryPassword = GeneratePasswrod(8, 8);            

            //If the temporary password generated successfully.
            if (!String.IsNullOrEmpty(temporaryPassword))
            {
                pamLogger.Debug("Temporary password generated successfully.");

                //Generate the Base64 encoded cryptographic random salt.
                string passwordSalt = GenerateSalt();

                //If the cryptographic random salt generated successfully.
                if (!String.IsNullOrEmpty(passwordSalt))
                {
                    pamLogger.Debug("Password salt generated successfully.");

                    //Generate the encrypted password for the generated temporary password.
                    string encryptedPassword = EncryptPassword(temporaryPassword, passwordSalt);

                    //If the encrypted password generated successfully.
                    if (!String.IsNullOrEmpty(encryptedPassword))
                    {
                        pamLogger.Debug("Encrypted password generated successfully.");

                        //Construct the password information structure with generated temporary password, encrypted password and password salt.
                        passwordInformation = new PasswordInformation
                        {
                            Password            = temporaryPassword,
                            EncryptedPassword   = encryptedPassword,
                            PasswordSalt        = passwordSalt
                        };

                        auditInformation = new AuditDataInformation
                        {
                            StateId     = StateInfo.Success,
                            AuditDetail = "Encrypted password generated successfully."
                        };
                    }
                    else
                    {
                        pamLogger.Debug("Unable to generate encrypted password.");

                        auditInformation = new AuditDataInformation
                        {
                            StateId     = StateInfo.Error,
                            AuditDetail = "Unable to generate encrypted password."
                        };
                    }
                }
                else
                {
                    pamLogger.Debug("Unable to generate password salt.");

                    auditInformation = new AuditDataInformation
                    {
                        StateId     = StateInfo.Error,
                        AuditDetail = "Unable to generate password salt."
                    };
                }
            }
            else
            {
                pamLogger.Debug("Unable to generate temporary password.");

                auditInformation = new AuditDataInformation
                {
                    StateId     = StateInfo.Error,
                    AuditDetail = "Unable to generate temporary password."
                };
            }
        }
        catch(Exception errorMessage)
        {
            pamLogger.Error("Exception on executing CreatePassword", errorMessage);

            auditInformation = new AuditDataInformation
            {
                StateId     = StateInfo.Exception,
                AuditDetail = errorMessage.ToString(),
            };

            //Send back the null password information structure.
            passwordInformation = null;
        }

        pamLogger.Debug("Sending CreatePassword response.");

        if (auditInformation != null)
        {
            auditInformation.SystemId               = SystemInfo.FocusPayPAM;
            auditInformation.EventId                = EventInfo.CreatePassword;
            auditInformation.OriginationEventId     = EventInfo.CreatePassword;
            auditInformation.EventTypeId            = EventTypeInfo.IdentificationAndAuthentication;

            auditBusinessLayer.BeginLogAuditInformation(auditInformation, null, null);
        }

        //Send back the password information structure.
        return passwordInformation;
    }

    #endregion

    #region AuthenticateUser

    /// <summary>
    /// This method authenticates the user with given username and plain text password.
    /// </summary>
    /// <param name="userName">Username.</param>
    /// <param name="password">Plain text password</param>
    /// <returns>Authentication status that details the authentication status code and authentication status message.</returns>
    [WebMethod(Description = "Authenticates the user with given username and password.")]
    public AuthenticationStatus AuthenticateUser(string userName, string password)
    {
        pamLogger.Debug("Processing AuthenticateUser request.");

        //Initialize the authentication information structure.
        AuthenticationStatus authenticationStatus = null;

        //Initialize the audit information structure.
        AuditDataInformation auditInformation = null;

        //Initialize the audit business layer.
        auditBusinessLayer = new TF.FocusPay.AuditLogger.BusinessLayer.BusinessOperations();

        try
        {
            //Check if username and password are provided or not.
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
            {
                //Initialize the business layer.
                businessLayer = new BusinessOperations(databaseConnectionString);

                //Get the user information like encrypted password and salt.
                UserInformation userInformation = businessLayer.GetUserInformation(userName, password);

                //If the user information exists.
                if (userInformation != null)
                {
                    pamLogger.Debug("Got user information.");

                    //Initialize the authentication response message.
                    authenticationStatus = new AuthenticationStatus();

                    //If the user is not Unregistered user, retrieve the user information.
                    if(userInformation.ResponseCode != GetResponseCode(ResponseCode.UNREGISTERED_USER))
                    {
                        authenticationStatus.UserId         = userInformation.UserId;
                        authenticationStatus.CompanyId      = userInformation.CompanyId;
                        authenticationStatus.MerchantId     = userInformation.MerchantId;
                        authenticationStatus.AttendantId    = userInformation.AttendantId;
                    }

                    //If the user is unlocked and not temporary (or) temporary.
                    if (userInformation.ResponseCode == GetResponseCode(ResponseCode.SUCCESS) || userInformation.ResponseCode == GetResponseCode(ResponseCode.PLEASE_CHANGE_YOUR_PASSWORD))
                    {
                        pamLogger.Debug("User is either unlocked and not temporary (or) temporary.");

                        //Check whether the input password is valid or not.
                        bool isValidPassword = ValidatePassword(password, userInformation.EncryptedPassword, userInformation.PasswordSalt);

                        //If the user is temporary.
                        if (userInformation.ResponseCode == GetResponseCode(ResponseCode.PLEASE_CHANGE_YOUR_PASSWORD))
                        {
                            //If it is a valid temporary password, send back as "Please Change Your Temporary Password (14)".
                            if (isValidPassword)
                            {
                                pamLogger.Debug("It's a valid temporary password.");
                                
                                authenticationStatus.ResposeCode        = userInformation.ResponseCode;
                                authenticationStatus.ResponseMessage    = GetResponseCodeDescription(userInformation.ResponseCode);

                                auditInformation = new AuditDataInformation
                                {
                                    StateId     = StateInfo.Alert,
                                    AuditDetail = authenticationStatus.ResponseMessage                                   
                                };                                
                            }
                            //Else, send back as "Invalid Temporary Password (15)".
                            else
                            {
                                pamLogger.Debug("It's an invalid temporary password.");
                                
                                authenticationStatus.ResposeCode        = GetResponseCode(ResponseCode.INVALID_TEMPORARY_PASSWORD);
                                authenticationStatus.ResponseMessage    = GetResponseCodeDescription(GetResponseCode(ResponseCode.INVALID_TEMPORARY_PASSWORD));

                                auditInformation = new AuditDataInformation
                                {
                                    StateId     = StateInfo.Error,
                                    AuditDetail = authenticationStatus.ResponseMessage
                                };                                
                            }

                            auditInformation.AffectedId             = userInformation.UserId;
                            auditInformation.EntityId               = EntityInfo.User;
                            auditInformation.OriginationEntityId    = EntityInfo.User;
                            auditInformation.SystemId               = SystemInfo.FocusPayPAM;
                            auditInformation.EventId                = EventInfo.AuthenticateUser;
                            auditInformation.OriginationEventId     = EventInfo.AuthenticateUser;
                            auditInformation.EventTypeId            = EventTypeInfo.IdentificationAndAuthentication;
                            auditBusinessLayer.BeginLogAuditInformation(auditInformation, null, null);

                            //Send back the authentication status.
                            return authenticationStatus;
                        }

                        //Get the authentication rule information by sending the password validation status.
                        AuthenticationRulesInformation authenticationRulesInformation = businessLayer.GetAuthenticationRulesInformation(userInformation.UserId, isValidPassword);

                        //If the authentication rule information exists.
                        if (authenticationRulesInformation != null)
                        {
                            pamLogger.Debug("Authentication rules applied successfully.");

                            //If the user is locked, send back the number of minutes left for the user to get unlocked.
                            if (authenticationRulesInformation.ResponseCode == GetResponseCode(ResponseCode.USER_IS_LOCKED_DYNAMIC_TIME))
                            {
                                pamLogger.Debug("User is locked.");
                                
                                authenticationStatus.ResposeCode    = authenticationRulesInformation.ResponseCode;

                                //Dynamically construct the message with number of minutes left to unlock the user.
                                authenticationStatus.ResponseMessage = String.Format(GetResponseCodeDescription(authenticationRulesInformation.ResponseCode), authenticationRulesInformation.UnlockMinutes.ToString());

                                auditInformation = new AuditDataInformation
                                {
                                    StateId     = StateInfo.Message,
                                    AuditDetail = authenticationStatus.ResponseMessage
                                };
                            }
                            //Else if the user has to change the password, send back the number of days left for changing password.
                            else if (authenticationRulesInformation.ResponseCode == GetResponseCode(ResponseCode.FORCE_CHANGE_PASSWORD_TIMELINE))
                            {
                                pamLogger.Debug("User has to change the password.");                                

                                //Send it as "Success" since its not an error, instead as a reminder.
                                authenticationStatus.ResposeCode = GetResponseCode(ResponseCode.SUCCESS);

                                //Dynamically construct the message with number of days left for changing password.
                                authenticationStatus.ResponseMessage = String.Format(GetResponseCodeDescription(authenticationRulesInformation.ResponseCode), authenticationRulesInformation.LeftOutForceChangePasswordDays.ToString());

                                auditInformation = new AuditDataInformation
                                {
                                    StateId     = StateInfo.Alert,
                                    AuditDetail = authenticationStatus.ResponseMessage
                                };                                
                            }
                            //Else if it is an invalid password attempt, send back the number of password attempts left for the user before its get locked.
                            else if (authenticationRulesInformation.ResponseCode == GetResponseCode(ResponseCode.INVALID_PASSWORD_ATTEMPTS_LEFT))
                            {
                                pamLogger.Debug("Invalid password attempt.");
                                
                                authenticationStatus.ResposeCode    = authenticationRulesInformation.ResponseCode;

                                //Dynamically construct the message with number of password attempts left for the user before its get locked.
                                authenticationStatus.ResponseMessage = String.Format(GetResponseCodeDescription(authenticationRulesInformation.ResponseCode), (authenticationRulesInformation.LeftOutInvalidPasswordAttempts.ToString() != "0") ? authenticationRulesInformation.LeftOutInvalidPasswordAttempts.ToString() : "no");

                                auditInformation = new AuditDataInformation
                                {
                                    StateId     = StateInfo.Warning,
                                    AuditDetail = authenticationStatus.ResponseMessage
                                };
                            }
                            //Else if it is password needs to be changed, send back the response code as "14 (Please Change Your Temporary Password)" for POS compatibility.
                            else if (authenticationRulesInformation.ResponseCode == GetResponseCode(ResponseCode.FORCE_CHANGE_PASSWORD))
                            {
                                pamLogger.Debug("Your password needs to be changed.");
                                
                                authenticationStatus.ResposeCode    = GetResponseCode(ResponseCode.PLEASE_CHANGE_YOUR_PASSWORD);

                                //Send back the response message as "Your password needs to be changed".
                                authenticationStatus.ResponseMessage = GetResponseCodeDescription(authenticationRulesInformation.ResponseCode);

                                auditInformation = new AuditDataInformation
                                {
                                    StateId     = StateInfo.Prompt,
                                    AuditDetail = authenticationStatus.ResponseMessage
                                };
                            }
                            //Else, send back the generated message.
                            else
                            {                                
                                authenticationStatus.ResposeCode        = authenticationRulesInformation.ResponseCode;
                                authenticationStatus.ResponseMessage    = GetResponseCodeDescription(authenticationRulesInformation.ResponseCode);

                                auditInformation = new AuditDataInformation
                                {
                                    StateId     = (authenticationStatus.ResposeCode == 0) ? StateInfo.Success : StateInfo.Message,
                                    AuditDetail = authenticationStatus.ResponseMessage
                                };
                               
                                pamLogger.Debug(authenticationStatus.ResponseMessage);
                            }
                        }
                        //Send back as "System Error (103)".
                        else
                        {
                            pamLogger.Debug("Unable to get authentication rule information.");
                            
                            authenticationStatus.ResposeCode        = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                            authenticationStatus.ResponseMessage    = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                            auditInformation = new AuditDataInformation
                            {
                                StateId     = StateInfo.Error,
                                AuditDetail = authenticationStatus.ResponseMessage
                            };                           
                        }
                    }
                    //Else if the user is locked, send back the number of minutes left for the user to get unlocked.
                    else if (userInformation.ResponseCode == GetResponseCode(ResponseCode.USER_IS_LOCKED_DYNAMIC_TIME))
                    {
                        pamLogger.Debug("User is locked.");
                        
                        authenticationStatus.ResposeCode    = userInformation.ResponseCode;

                        //Dynamically construct the message with number of minutes left to unlock the user.
                        authenticationStatus.ResponseMessage = String.Format(GetResponseCodeDescription(userInformation.ResponseCode), userInformation.UnlockMinutes.ToString());

                        auditInformation = new AuditDataInformation
                        {
                            StateId     = StateInfo.Message,
                            AuditDetail = authenticationStatus.ResponseMessage
                        };                        
                    }
                    //Else, send back the generated error message.
                    else
                    {
                        authenticationStatus.ResposeCode        = userInformation.ResponseCode;
                        authenticationStatus.ResponseMessage    = GetResponseCodeDescription(userInformation.ResponseCode);

                        auditInformation = new AuditDataInformation
                        {
                            StateId     = StateInfo.Error,
                            AuditDetail = authenticationStatus.ResponseMessage
                        };                        

                        pamLogger.Debug(authenticationStatus.ResponseMessage);
                    }

                    if (auditInformation != null)
                    {
                        auditInformation.AffectedId             = userInformation.UserId;
                        auditInformation.EntityId               = EntityInfo.User;
                        auditInformation.OriginationEntityId    = EntityInfo.User;
                    }
                }
                //Else, send back as "Unregistered User (12)".
                else
                {
                    pamLogger.Debug("User information doesn't exist.");

                    authenticationStatus = new AuthenticationStatus
                    {
                        ResposeCode         = GetResponseCode(ResponseCode.UNREGISTERED_USER),
                        ResponseMessage     = GetResponseCodeDescription(GetResponseCode(ResponseCode.UNREGISTERED_USER))
                    };

                    auditInformation = new AuditDataInformation
                    {
                        StateId     = StateInfo.Error,
                        AuditDetail = authenticationStatus.ResponseMessage
                    };
                }
            }
            //Else, send back as "Insufficient Parameters (1)".
            else
            {
                pamLogger.Debug("Empty parameters provided for the AuthenticateUser.");

                authenticationStatus = new AuthenticationStatus
                {
                    ResposeCode         = GetResponseCode(ResponseCode.INSUFFICIENT_PARAMETERS),
                    ResponseMessage     = GetResponseCodeDescription(GetResponseCode(ResponseCode.INSUFFICIENT_PARAMETERS))
                };

                auditInformation = new AuditDataInformation
                {
                    StateId     = StateInfo.Error,
                    AuditDetail = authenticationStatus.ResponseMessage
                };
            }
        }
        catch (Exception errorMessage)
        {
            pamLogger.Error("Exception on executing AuthenticateUser", errorMessage);

            auditInformation = new AuditDataInformation
            {                 
                StateId     = StateInfo.Exception,
                AuditDetail = errorMessage.ToString()                
            };
            
            //Send back the null authentication status.
            authenticationStatus = null;
        }
        finally
        {
            //Dispose the business layer. 
            if (businessLayer != null)
            {
                businessLayer.Dispose();
                businessLayer = null;
            }
        }

        pamLogger.Debug("Sending AuthenticateUser response.");

        if (auditInformation != null)
        {
            auditInformation.SystemId               = SystemInfo.FocusPayPAM;
            auditInformation.EventId                = EventInfo.AuthenticateUser;
            auditInformation.OriginationEventId     = EventInfo.AuthenticateUser;
            auditInformation.EventTypeId            = EventTypeInfo.IdentificationAndAuthentication;

            auditBusinessLayer.BeginLogAuditInformation(auditInformation, null, null);
        }

        //Send back the authentication status.
        return authenticationStatus;
    }

    #endregion

    #region ValidatePassword

    /// <summary>
    /// This method validates the given password against all password validation rules for the user.
    /// </summary>
    /// <param name="userName">Username.</param>
    /// <param name="password">Plain text password.</param>
    /// <returns>Validation status that details the validation status code and validation status message along with password details if any.</returns>
    [WebMethod(Description = "Validates the given password against all password validation rules.")]
    public ValidationStatus ValidatePassword(string userName, string password)
    {
        pamLogger.Debug("Processing ValidatePassword request.");

        //Initialize the password validation structure.
        ValidationStatus validationStatus = null;

        //Initialize the audit information structure.
        AuditDataInformation auditInformation = null;

        //Initialize the audit business layer.
        auditBusinessLayer = new TF.FocusPay.AuditLogger.BusinessLayer.BusinessOperations();

        try
        {
            //Check if username and password are provided or not.
            if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
            {
                //Initialize the business layer.
                businessLayer = new BusinessOperations(databaseConnectionString);

                //Apply the password validation rules for the given password.
                PasswordValidationInformation passwordValidationInformation = businessLayer.ApplyPasswordValidationRules(userName, password);

                //If the password validation information exists.
                if (passwordValidationInformation != null)
                {
                    //Initialize the password validation response message.
                    validationStatus = new ValidationStatus();

                    //If the password length validation successful.
                    if (passwordValidationInformation.ResponseCode == GetResponseCode(ResponseCode.SUCCESS))
                    {
                        pamLogger.Debug("Password validation rules applied successfully.");

                        //Get the regular expression to check the password strength.
                        string passwordExpression = passwordValidationInformation.PasswordRegularExpression;

                        //If the password strength expression exists.
                        if (!String.IsNullOrEmpty(passwordExpression))
                        {
                            //Check whether the given password matches with the regular expression or not.
                            Match expressionMatch = Regex.Match(password, passwordExpression);

                            //If the password strength validation successful.
                            if (expressionMatch.Success)
                            {
                                pamLogger.Debug("Password strength validated successfully.");

                                //Get the previous password history for the given user.
                                PasswordHistoryInformationData passwordHistory = businessLayer.GetPasswordHistoryInformation(passwordValidationInformation.UserId);

                                //If the password history exists.
                                if (passwordHistory != null)
                                {
                                    pamLogger.Debug("User has password history.");

                                    //If the password history contains data.
                                    if (passwordHistory.PasswordHistoryInformationCollection != null && passwordHistory.PasswordHistoryInformationCollection.Count > 0)
                                    {
                                        //Check whether the given password matches with any of the previous password history data.
                                        bool matchedPasswordHistoryItem = passwordHistory.PasswordHistoryInformationCollection.Where
                                            (passwordHistoryItem => ValidatePassword(password, passwordHistoryItem.EncryptedPassword, passwordHistoryItem.PasswordSalt)).Any();

                                        //If no match found, send back the new encrypted password and salt for the given plain text password.
                                        if (!matchedPasswordHistoryItem)
                                        {
                                            pamLogger.Debug("No match found from the password history.");

                                            //Generate the Base64 encoded cryptographic random salt.
                                            string passwordSalt = GenerateSalt();

                                            pamLogger.Debug("Password salt generated successfully.");

                                            //Generate the encrypted password for the given password.
                                            string encryptedPassword = EncryptPassword(password, passwordSalt);

                                            pamLogger.Debug("Encrypted password generated successfully.");

                                            validationStatus.EncryptedPassword  = encryptedPassword;
                                            validationStatus.PasswordSalt       = passwordSalt;
                                            validationStatus.UserId             = passwordValidationInformation.UserId;
                                            validationStatus.ResposeCode        = GetResponseCode(ResponseCode.SUCCESS);
                                            validationStatus.ResponseMessage    = GetResponseCodeDescription(GetResponseCode(ResponseCode.SUCCESS));

                                            auditInformation = new AuditDataInformation
                                            {
                                                StateId     = StateInfo.Success,
                                                AuditDetail = validationStatus.ResponseMessage
                                            };
                                        }
                                        //If there is a match found from the history, send back as "The new password cannot be same as your last X passwords"
                                        else
                                        {
                                            pamLogger.Debug("There is a match from the password history.");

                                            validationStatus.UserId             = passwordValidationInformation.UserId;
                                            validationStatus.ResposeCode        = GetResponseCode(ResponseCode.REPEATED_PASSWORD);
                                            validationStatus.ResponseMessage    = String.Format(GetResponseCodeDescription(GetResponseCode(ResponseCode.REPEATED_PASSWORD)), passwordHistory.SamePasswordUsageLimit.ToString());

                                            auditInformation = new AuditDataInformation
                                            {
                                                StateId     = StateInfo.Error,
                                                AuditDetail = validationStatus.ResponseMessage
                                            };
                                        }
                                    }
                                }
                                //Else, send back the new encrypted password and salt for the given plain text password.
                                else
                                {
                                    pamLogger.Debug("User has no password history.");

                                    //Generate the Base64 encoded cryptographic random salt.
                                    string passwordSalt = GenerateSalt();

                                    pamLogger.Debug("Password salt generated successfully.");

                                    //Generate the encrypted password for the given password.
                                    string encryptedPassword = EncryptPassword(password, passwordSalt);

                                    pamLogger.Debug("Encrypted password generated successfully.");

                                    validationStatus.EncryptedPassword  = encryptedPassword;
                                    validationStatus.PasswordSalt       = passwordSalt;
                                    validationStatus.UserId             = passwordValidationInformation.UserId;
                                    validationStatus.ResposeCode        = GetResponseCode(ResponseCode.SUCCESS);
                                    validationStatus.ResponseMessage    = GetResponseCodeDescription(GetResponseCode(ResponseCode.SUCCESS));

                                    auditInformation = new AuditDataInformation
                                    {
                                        StateId     = StateInfo.Success,
                                        AuditDetail = validationStatus.ResponseMessage
                                    };
                                }
                            }
                            else if (password.Length < 8)
                            {
                                pamLogger.Debug("Password should be minimum of 8 characters.");

                                validationStatus.UserId = passwordValidationInformation.UserId;
                                validationStatus.ResposeCode = GetResponseCode(ResponseCode.PASSWORD_LENGTH);
                                validationStatus.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.PASSWORD_LENGTH));

                                auditInformation = new AuditDataInformation
                                {
                                    StateId = StateInfo.Error,
                                    AuditDetail = validationStatus.ResponseMessage
                                };
                            }
                            //Else, the new password is not strong enough.
                            else
                            {
                                pamLogger.Debug("Password should be mix of alphanumeric and atleast one special character except < >.");

                                    validationStatus.UserId = passwordValidationInformation.UserId;
                                    validationStatus.ResposeCode = GetResponseCode(ResponseCode.WEAK_PASSWORD);
                                    validationStatus.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.WEAK_PASSWORD));
                               
                                auditInformation = new AuditDataInformation
                                {
                                    StateId = StateInfo.Error,
                                    AuditDetail = validationStatus.ResponseMessage
                                };
                            }
                        }
                    }
                    //Else, the new password does not meet minimum length criteria.
                    else
                    {
                        pamLogger.Debug("The new password does not meet minimum length criteria.");
                        validationStatus.UserId             = passwordValidationInformation.UserId;
                        validationStatus.ResposeCode        = passwordValidationInformation.ResponseCode;
                        validationStatus.ResponseMessage    = GetResponseCodeDescription(passwordValidationInformation.ResponseCode);

                        auditInformation = new AuditDataInformation
                        {
                            StateId     = StateInfo.Error,
                            AuditDetail = validationStatus.ResponseMessage
                        };
                    }
                }
                else
                {
                    pamLogger.Debug("Unable to apply password validation rules.");

                    //Send back as "System Error (103)".
                    validationStatus = new ValidationStatus
                    {
                        ResposeCode         = GetResponseCode(ResponseCode.CHANAKYA_ERROR),
                        ResponseMessage     = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR))
                    };

                    auditInformation = new AuditDataInformation
                    {
                        StateId     = StateInfo.Error,
                        AuditDetail = validationStatus.ResponseMessage
                    };
                }
            }
            //Else, send back as "Insufficient Parameters (1)".
            else
            {
                pamLogger.Debug("Empty parameters provided for the ValidatePassword.");

                validationStatus = new ValidationStatus
                {
                    ResposeCode         = GetResponseCode(ResponseCode.INSUFFICIENT_PARAMETERS),
                    ResponseMessage     = GetResponseCodeDescription(GetResponseCode(ResponseCode.INSUFFICIENT_PARAMETERS))
                };

                auditInformation = new AuditDataInformation
                {
                    StateId     = StateInfo.Error,
                    AuditDetail = validationStatus.ResponseMessage
                };
            }
        }
        catch (Exception errorMessage)
        {
            pamLogger.Error("Exception on executing ValidatePassword", errorMessage);

            auditInformation = new AuditDataInformation
            {
                StateId     = StateInfo.Exception,
                AuditDetail = errorMessage.ToString()
            };

            //Send back the null validation status.
            validationStatus = null;
        }
        finally
        {
            //Dispose the business layer. 
            if (businessLayer != null)
            {
                businessLayer.Dispose();
                businessLayer = null;
            }
        }

        pamLogger.Debug("Sending ValidatePassword response.");

        if (auditInformation != null)
        {
            auditInformation.SystemId               = SystemInfo.FocusPayPAM;
            auditInformation.EventId                = EventInfo.ValidatePassword;
            auditInformation.OriginationEventId     = EventInfo.ValidatePassword;
            auditInformation.EventTypeId            = EventTypeInfo.IdentificationAndAuthentication;

            auditBusinessLayer.BeginLogAuditInformation(auditInformation, null, null);
        }

        //Send back the validation status.
        return validationStatus;
    }

    #endregion

    #region GenerateSession

    /// <summary>
    /// This method creates the session-id for the given user and stores the same in database.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <returns>Session information for the user.</returns>
    [WebMethod(Description = "Creates the session-id for the given user.")]
    public SessionInformation GenerateSession(long userId)
    {
        pamLogger.Debug("Processing GenerateSession request.");

        //Initialize the session information structure.
        SessionInformation sessionInformation = null;

        //Initialize the audit information structure.
        AuditDataInformation auditInformation = null;

        //Initialize the audit business layer.
        auditBusinessLayer = new TF.FocusPay.AuditLogger.BusinessLayer.BusinessOperations();

        try
        {
            //Check whether the user id is greater than zero or not.
            if (userId > 0)
            {
                //Create the session-id.
                string sessionId = GenerateSessionId();                

                //If session-id created successfully.
                if (!String.IsNullOrEmpty(sessionId))
                {
                    pamLogger.Debug("Successfully generated Session-ID");

                    //Initialize the business layer.
                    businessLayer = new BusinessOperations(databaseConnectionString);

                    //Store the session-id for the given user.
                    int responseCode = businessLayer.ManageUserSession(userId, sessionId);

                    pamLogger.Debug("Got the response code for session management as : " + responseCode.ToString());

                    //Send back the response with generated response code.
                    sessionInformation = new SessionInformation
                    {
                        ResponseCode    = responseCode,
                        SessionId       = (responseCode == 0) ? sessionId : string.Empty
                    };

                    auditInformation = new AuditDataInformation
                    {
                        StateId     = (sessionInformation.ResponseCode == 0) ? StateInfo.Success : StateInfo.Message,
                        AuditDetail = GetResponseCodeDescription(sessionInformation.ResponseCode)
                    };
                }
                //Else, send back the response code as "System Error (103)".
                else
                {
                    pamLogger.Debug("Unable to generate Session-ID");

                    sessionInformation = new SessionInformation
                    {
                        ResponseCode = GetResponseCode(ResponseCode.CHANAKYA_ERROR)
                    };

                    auditInformation = new AuditDataInformation
                    {
                        StateId     = StateInfo.Error,
                        AuditDetail = "Unable to generate Session-ID"
                    };
                }
            }
            //Else, send back the response code as "Unregistered User (12)".
            else
            {
                pamLogger.Debug("Invalid user id.");

                sessionInformation = new SessionInformation
                {
                    ResponseCode = GetResponseCode(ResponseCode.UNREGISTERED_USER)
                };

                auditInformation = new AuditDataInformation
                {
                    StateId     = StateInfo.Error,
                    AuditDetail = GetResponseCodeDescription(sessionInformation.ResponseCode)
                };
            }
        }
        catch (Exception errorMessage)
        {
            pamLogger.Error("Exception on executing GenerateSession", errorMessage);

            auditInformation = new AuditDataInformation
            {
                StateId     = StateInfo.Exception,
                AuditDetail = errorMessage.ToString()
            };

            //Send back the null session information structure.
            sessionInformation = null;
        }
        finally
        {
            //Dispose the business layer. 
            if (businessLayer != null)
            {
                businessLayer.Dispose();
                businessLayer = null;
            }
        }

        pamLogger.Debug("Sending GenerateSession response.");

        if (auditInformation != null)
        {
            auditInformation.SystemId               = SystemInfo.FocusPayPAM;
            auditInformation.EventId                = EventInfo.GenerateSession;
            auditInformation.OriginationEventId     = EventInfo.GenerateSession;
            auditInformation.EventTypeId            = EventTypeInfo.IdentificationAndAuthentication;

            auditBusinessLayer.BeginLogAuditInformation(auditInformation, null, null);
        }

        //Send back the session information structure.
        return sessionInformation;
    }

    #endregion

    #region LogUserActivity

    /// <summary>
    /// This method logs the activity time for the given user.
    /// </summary>
    /// <param name="userId">User ID.</param>
    [WebMethod(Description = "Logs the user activity time.")]
    public void LogUserActivity(long userId)
    {
        pamLogger.Debug("Processing LogUserActivity request.");        
        
        try
        {
            //Check whether the user id is greater than zero or not.
            if (userId > 0)
            {
                //Initialize the business layer.
                businessLayer = new BusinessOperations(databaseConnectionString);

                //Log the user activity time.
                int activityLogStatus = businessLayer.ManageUserActivity(userId);

                pamLogger.Debug("Got the response code for activity log as : " + activityLogStatus.ToString());
            }
            //Else, it is "Unregistered User (12)".
            else
            {
                pamLogger.Debug("Invalid user id.");
            }                       
        }
        catch (Exception errorMessage)
        {
            pamLogger.Error("Exception on executing LogUserActivity", errorMessage);            
        }
        finally
        {
            //Dispose the business layer. 
            if (businessLayer != null)
            {
                businessLayer.Dispose();
                businessLayer = null;
            }
        }

        pamLogger.Debug("Sending LogUserActivity response.");        
    }

    #endregion

    #region GetRolePermissions

    /// <summary>
    /// This method gets the role permissions associated with the given user id.
    /// </summary>
    /// <param name="userId">Logged-in user id.</param>
    /// <returns>Role permission information that contains all items(screens and actions) permission information.</returns>
    [WebMethod(Description = "Gets the role permissions associated with the given user id.")]
    public RolePermissions GetRolePermissions(long userId)
    {
        pamLogger.Debug("Processing GetRolePermissions request.");

        //Declare the role permissions object.
        RolePermissions rolePermissions = null;        

        try
        {
            //Check whether the user id is greater than zero or not.
            if (userId > 0)
            {
                //Initialize the business layer.
                businessLayer = new BusinessOperations(databaseConnectionString);

                //Get the privilege information for the given user.
                UserPrivilegeInformation userPrivilegeInformation = businessLayer.GetUserPrivileges(userId);

                //Check if privilege information exists and users privilege level is greater than zero.
                if (userPrivilegeInformation != null && userPrivilegeInformation.PrivilegeLevel > 0)
                {
                    //Initialize the role permissions object.
                    rolePermissions = new RolePermissions();

                    //If users privilege information exists.
                    if (userPrivilegeInformation.ResponseCode == GetResponseCode(ResponseCode.SUCCESS))
                    {
                        pamLogger.Debug("Successfully got the privilege information for given user.");

                        //Get item mask information from the master data for the given privilege level (1/2/3/4).
                        List<ItemMaskInformation> itemMaskCollection = businessLayer.GetItemMask(userPrivilegeInformation.PrivilegeLevel, userPrivilegeInformation.IsPrimitive);

                        //Get item values information from the master data for the given privilege level (1/2/3/4).
                        List<ItemValuesInformation> itemValuesCollection = businessLayer.GetItemValues(userPrivilegeInformation.PrivilegeLevel);

                        //Check whether master data exists or not.
                        if (itemMaskCollection != null && itemValuesCollection != null)
                        {
                            //Declare admin role privileges collection.
                            List<ItemValuesInformation> adminRolePrivileges = null;

                            //Check whether master data contains any information.
                            if (itemMaskCollection.Count > 0 && itemValuesCollection.Count > 0)
                            {                                
                                pamLogger.Debug(string.Format("Successfully got the item master data for given {0} privilege level : {1}", userPrivilegeInformation.IsPrimitive ? "primitive users" : "non-primitive users", userPrivilegeInformation.PrivilegeLevel.ToString()));

                                //Initialize the audit business layer.
                                auditBusinessLayer = new TF.FocusPay.AuditLogger.BusinessLayer.BusinessOperations();

                                #region Admin User

                                //If the given user id is of Admin user.
                                if (userPrivilegeInformation.RoleId == userPrivilegeInformation.AdminRoleId && userPrivilegeInformation.IsAdmin == true)
                                {
                                    pamLogger.Debug("Given user is Admin user.");

                                    //Get the item permissions for users role.
                                    List<ItemValuesInformation> selfRolePrivileges = businessLayer.GetUserItemValues(userPrivilegeInformation.RoleId);

                                    //Check whether user role has any item permissions.
                                    if (selfRolePrivileges != null)
                                    {
                                        pamLogger.Debug("Successfully got the item permissions for the users role.");

                                        //Check whether there are any new items needs to be added to the users role.
                                        List<ItemValuesInformation> newItemsCollection = IdentifyNewItems(itemValuesCollection, selfRolePrivileges);

                                        //If there are any new items exists.
                                        if (newItemsCollection != null && newItemsCollection.Count > 0)
                                        {
                                            pamLogger.Debug(string.Format("Found {0} new items for the user role: {1}.", newItemsCollection.Count, userPrivilegeInformation.RoleId.ToString()));
                                            
                                            //Append the new items for the user role with master data values.
                                            foreach (ItemValuesInformation newItem in newItemsCollection)
                                            {
                                                int responseCode = businessLayer.InsertItemInformation(new NewItemInformation
                                                                     {
                                                                         RoleId             = userPrivilegeInformation.RoleId,
                                                                         ItemId             = newItem.ItemId,
                                                                         ItemName           = newItem.ItemName,
                                                                         IsCreateAllowed    = newItem.IsCreateAllowed,
                                                                         IsEditAllowed      = newItem.IsEditAllowed,
                                                                         IsDeleteAllowed    = newItem.IsDeleteAllowed,
                                                                         IsViewAllowed      = newItem.IsViewAllowed,
                                                                         UserId             = userId
                                                                     });

                                                //Check whether the item information is added successfully or not. If not, send back the
                                                //role permissions with reponse as 101 (Database Error).
                                                if (responseCode != GetResponseCode(ResponseCode.SUCCESS))
                                                {
                                                    pamLogger.Debug("Failed to append the new item information.");

                                                    rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.DATABASE_ERROR);
                                                    rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.DATABASE_ERROR));

                                                    return rolePermissions;
                                                }
                                                //Else, if the item information added successfully.
                                                else
                                                {
                                                    pamLogger.Debug("Successfully appended the new item information.");

                                                    //Log the item information for audit purpose.
                                                    auditBusinessLayer.BeginLogAuditInformation(new AuditDataInformation
                                                    {
                                                        TableId             = TableInfo.RolePrivileges,
                                                        SystemId            = SystemInfo.FocusPayPAM,
                                                        StateId             = StateInfo.Success,
                                                        EventTypeId         = EventTypeInfo.BackgroundProcess,
                                                        EventId             = EventInfo.AddNewPrivilegeItem,                                                        
                                                        OriginationEventId  = EventInfo.AuthenticateUser,
                                                        EntityId            = EntityInfo.Role,
                                                        OriginationEntityId = EntityInfo.User,
                                                        AffectedId          = userPrivilegeInformation.RoleId,
                                                        UserId              = userId,
                                                        AuditDetail         = "New item added successfully"
                                                    }, null, null);
                                                }
                                            }                                          

                                            //Get the latest item permissions for users role.
                                            adminRolePrivileges = businessLayer.GetUserItemValues(userPrivilegeInformation.RoleId);

                                            pamLogger.Debug("Got the latest item permissions for the users role.");
                                        }
                                        //If no new items exists, consider the existing item permissions as latest item permissions.
                                        else
                                        {
                                            pamLogger.Debug("No new items found for the user role.");

                                            adminRolePrivileges = selfRolePrivileges;
                                        }
                                    }
                                    //Else, send back role permissions with response as 103(System Error).
                                    else
                                    {
                                        pamLogger.Debug("Unable to get the item permissions for the users role.");

                                        rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                        rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                        return rolePermissions;
                                    }
                                }

                                #endregion

                                #region Non-Admin User

                                //If the given user id is of Non-Admin user.
                                else
                                {
                                    pamLogger.Debug("Given user is Non-Admin user.");

                                    //Get the item permissions for users admin role.
                                    adminRolePrivileges = businessLayer.GetUserItemValues(userPrivilegeInformation.AdminRoleId);

                                    //Check whether users admin role has any item permissions.
                                    if (adminRolePrivileges != null)
                                    {
                                        pamLogger.Debug("Successfully got the item permissions for the users admin role.");

                                        //Check whether there are any new items needs to be added to the users admin role.
                                        List<ItemValuesInformation> newAdminItemsCollection = IdentifyNewItems(itemValuesCollection, adminRolePrivileges);

                                        //If there are any new items exists.
                                        if (newAdminItemsCollection != null && newAdminItemsCollection.Count > 0)
                                        {
                                            pamLogger.Debug(string.Format("Found {0} new items for the users admin role: {1}.", newAdminItemsCollection.Count, userPrivilegeInformation.AdminRoleId.ToString()));

                                            //Append the new items for the users admin role with master data values.
                                            foreach (ItemValuesInformation newAdminItem in newAdminItemsCollection)
                                            {
                                                int responseCode = businessLayer.InsertItemInformation(new NewItemInformation
                                                                     {
                                                                         RoleId             = userPrivilegeInformation.AdminRoleId,
                                                                         ItemId             = newAdminItem.ItemId,
                                                                         ItemName           = newAdminItem.ItemName,
                                                                         IsCreateAllowed    = newAdminItem.IsCreateAllowed,
                                                                         IsEditAllowed      = newAdminItem.IsEditAllowed,
                                                                         IsDeleteAllowed    = newAdminItem.IsDeleteAllowed,
                                                                         IsViewAllowed      = newAdminItem.IsViewAllowed,
                                                                         UserId             = userId
                                                                     });

                                                //Check whether the item information is added successfully or not. If not, send back the
                                                //role permissions with reponse as 101 (Database Error).
                                                if (responseCode != GetResponseCode(ResponseCode.SUCCESS))
                                                {
                                                    pamLogger.Debug("Failed to append the new item information to admin role.");

                                                    rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.DATABASE_ERROR);
                                                    rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.DATABASE_ERROR));

                                                    return rolePermissions;
                                                }
                                                //Else, if the item information added successfully.
                                                else
                                                {
                                                    pamLogger.Debug("Successfully appended the new item information to admin role.");

                                                    //Log the item information for audit purpose.
                                                    auditBusinessLayer.BeginLogAuditInformation(new AuditDataInformation
                                                    {
                                                        TableId                 = TableInfo.RolePrivileges,
                                                        SystemId                = SystemInfo.FocusPayPAM,
                                                        StateId                 = StateInfo.Success,
                                                        EventTypeId             = EventTypeInfo.BackgroundProcess,
                                                        EventId                 = EventInfo.AddNewPrivilegeItem,
                                                        OriginationEventId      = EventInfo.AuthenticateUser,
                                                        EntityId                = EntityInfo.Role,
                                                        OriginationEntityId     = EntityInfo.User,
                                                        AffectedId              = userPrivilegeInformation.AdminRoleId,
                                                        UserId                  = userId,
                                                        AuditDetail             = "New item added successfully"
                                                    }, null, null);
                                                }
                                            }                                            

                                            //Get the latest item permissions for users admin role.
                                            adminRolePrivileges = businessLayer.GetUserItemValues(userPrivilegeInformation.AdminRoleId);

                                            pamLogger.Debug("Got the latest item permissions for the users admin role.");
                                        }
                                        //No new items found for the users admin role.
                                        else
                                        {
                                            pamLogger.Debug("No new items found for the users admin role.");                                            
                                        }
                                    }
                                    //Else, send back role permissions with response as 103(System Error).
                                    else
                                    {
                                        pamLogger.Debug("Unable to get the item permissions for the users admin role.");

                                        rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                        rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                        return rolePermissions;
                                    }

                                    //Get the item permissions for users role.
                                    List<ItemValuesInformation> selfRolePrivileges = businessLayer.GetUserItemValues(userPrivilegeInformation.RoleId);

                                    //Check whether user role has any item permissions.
                                    if (selfRolePrivileges != null)
                                    {
                                        pamLogger.Debug("Successfully got the item permissions for the users role.");

                                        //Check whether there are any new items needs to be added to the users role.
                                        List<ItemValuesInformation> newItemsCollection = IdentifyNewItems(itemValuesCollection, selfRolePrivileges);

                                        //If there are any new items exists.
                                        if (newItemsCollection != null && newItemsCollection.Count > 0)
                                        {
                                            pamLogger.Debug(string.Format("Found {0} new items for the users role: {1}.", newItemsCollection.Count, userPrivilegeInformation.RoleId.ToString()));

                                            //Append the new items for the user role with values as 0(false).
                                            foreach (ItemValuesInformation newItem in newItemsCollection)
                                            {
                                                int responseCode = businessLayer.InsertItemInformation(new NewItemInformation
                                                {
                                                    RoleId              = userPrivilegeInformation.RoleId,
                                                    ItemId              = newItem.ItemId,
                                                    ItemName            = newItem.ItemName,
                                                    IsCreateAllowed     = false,
                                                    IsEditAllowed       = false,
                                                    IsDeleteAllowed     = false,
                                                    IsViewAllowed       = false,
                                                    UserId              = userId
                                                });

                                                //Check whether the item information is added successfully or not. If not, send back the
                                                //role permissions with reponse as 101 (Database Error).
                                                if (responseCode != GetResponseCode(ResponseCode.SUCCESS))
                                                {
                                                    pamLogger.Debug("Failed to append the new item information to users role.");

                                                    rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.DATABASE_ERROR);
                                                    rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.DATABASE_ERROR));

                                                    return rolePermissions;
                                                }
                                                //Else, if the item information added successfully.
                                                else
                                                {
                                                    pamLogger.Debug("Successfully appended the new item information to users role.");

                                                    //Log the item information for audit purpose.
                                                    auditBusinessLayer.BeginLogAuditInformation(new AuditDataInformation
                                                    {
                                                        TableId                 = TableInfo.RolePrivileges,
                                                        SystemId                = SystemInfo.FocusPayPAM,
                                                        StateId                 = StateInfo.Success,
                                                        EventTypeId             = EventTypeInfo.BackgroundProcess,
                                                        EventId                 = EventInfo.AddNewPrivilegeItem,
                                                        OriginationEventId      = EventInfo.AuthenticateUser,
                                                        EntityId                = EntityInfo.Role,
                                                        OriginationEntityId     = EntityInfo.User,
                                                        AffectedId              = userPrivilegeInformation.RoleId,
                                                        UserId                  = userId,
                                                        AuditDetail             = "New item added successfully"
                                                    }, null, null);
                                                }
                                            }                                            

                                            //Get the latest item permissions for users role.
                                            selfRolePrivileges = businessLayer.GetUserItemValues(userPrivilegeInformation.RoleId);

                                            pamLogger.Debug("Got the latest item permissions for the users role.");
                                        }
                                        //No new items found for the user role.
                                        else
                                        {
                                            pamLogger.Debug("No new items found for the user role.");                                            
                                        }
                                    }
                                    //Else, send back role permissions with response as 103(System Error).
                                    else
                                    {
                                        pamLogger.Debug("Unable to get the item permissions for the users role.");

                                        rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                        rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                        return rolePermissions;
                                    }

                                    //Perform bitwise AND for admin and user roles item permissions.
                                    adminRolePrivileges = BitwiseAndItemPermissions(adminRolePrivileges, selfRolePrivileges);

                                    pamLogger.Debug("Successfully performed bitwise AND for admin and user roles item permissions.");
                                }

                                #endregion
                            }
                            //Else, send back role permissions with response as 103(System Error).
                            else
                            {
                                rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                pamLogger.Debug("No information exists in the item master data.");

                                return rolePermissions;
                            }

                            //Perform bitwise AND for item mask and admin roles item permissions.
                            itemMaskCollection = BitwiseAndItemPermissions(itemMaskCollection, adminRolePrivileges);

                            //Check whether bitwise AND operation performed successfully or not.
                            if (itemMaskCollection != null && itemMaskCollection.Count > 0)
                            {
                                pamLogger.Debug("Successfully performed bitwise AND for item mask and admin roles item permissions.");

                                rolePermissions.ResponseCode                = GetResponseCode(ResponseCode.SUCCESS);
                                rolePermissions.ResponseMessage             = GetResponseCodeDescription(GetResponseCode(ResponseCode.SUCCESS));
                                rolePermissions.ItemInformationCollection   = itemMaskCollection;
                            }
                            //Else, send back role permissions with response as 103(System Error).
                            else
                            {
                                rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                pamLogger.Debug("Failed to perform bitwise AND for item mask and admin roles item permissions.");
                            }
                        }
                        //Else, send back role permissions with response as 103(System Error).
                        else
                        {
                            rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                            rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                            pamLogger.Debug(string.Format("Unable to get the master data for given {0} privilege level : {1}", userPrivilegeInformation.IsPrimitive ? "primitive users" : "non-primitive users", userPrivilegeInformation.PrivilegeLevel.ToString()));
                        }
                    }
                    //Else, send back with generated response code 12 (Unregistered User).
                    else
                    {
                        rolePermissions.ResponseCode    = userPrivilegeInformation.ResponseCode;
                        rolePermissions.ResponseMessage = GetResponseCodeDescription(userPrivilegeInformation.ResponseCode);

                        pamLogger.Debug(string.Format("Unable to get the privilege information for the given user : {0}",rolePermissions.ResponseMessage));
                    }
                }
                //Else, Send back the null role permissions.
                else
                {
                    pamLogger.Debug("Unable to get the privilege information for the given user.");
                }
            }
            else
            {
                pamLogger.Debug("User ID is 0(Zero).");

                //Send back the null role permissions.
                rolePermissions = null;
            }
        }
        catch (Exception errorMessage)
        {
            pamLogger.Error("Exception on executing GetRolePermissions", errorMessage);

            //Send back the null role permissions.
            rolePermissions = null;
        }
        finally
        {
            //Dispose the business layer. 
            if (businessLayer != null)
            {
                businessLayer.Dispose();
                businessLayer = null;
            }
        }

        //Send back the role permissions.
        return rolePermissions;
    }

    #endregion

    #region GetSelectedRolePermissions

    /// <summary>
    /// This method gets the role permissions associated with the selected role id w.r.t logged-in user id.
    /// </summary>
    /// <param name="userId">Logged-in user id.</param>
    /// <param name="roleId">Selected role id.</param>
    /// <returns>Role permission information that contains all items(screens and actions) permission information on edit mode.</returns>
    [WebMethod(Description = "Gets the role permissions associated with the selected role id w.r.t logged-in user id.")]
    public EditRolePermissions GetSelectedRolePermissions(long userId, short roleId)
    {
        pamLogger.Debug("Processing GetSelectedRolePermissions request.");

        //Declare the role permissions object.
        EditRolePermissions rolePermissions = null;        

        try
        {
            //Check whether the user id and role id are greater than zero or not.
            if (userId > 0 && roleId > 0)
            {
                //Initialize the role permissions object.
                businessLayer = new BusinessOperations(databaseConnectionString);

                //Get the privilege level information for the selected role.
                RolePrivilegeInformation rolePrivilegeInformation = businessLayer.GetRolePrivileges(roleId);

                //Check whether the privilege level information exists for the selected role.
                if (rolePrivilegeInformation != null && rolePrivilegeInformation.PrivilegeLevel > 0)
                {
                    //Initialize the role permissions object with role name.
                    rolePermissions = new EditRolePermissions { RoleName = rolePrivilegeInformation.RoleName };

                    //Check whether the privilege level information retrieved successfully.
                    if (rolePrivilegeInformation.ResponseCode == GetResponseCode(ResponseCode.SUCCESS))
                    {
                        pamLogger.Debug("Successfully got the privilege level information for selected role.");

                        //Get item mask information from the master data for the selected role privilege level (1/2/3/4).
                        List<ItemMaskInformation> itemMaskCollection = businessLayer.GetItemMask(rolePrivilegeInformation.PrivilegeLevel, rolePrivilegeInformation.IsPrimitive);

                        //Get item values information from the master data for the selected role privilege level (1/2/3/4).
                        List<ItemValuesInformation> itemValuesCollection = businessLayer.GetItemValues(rolePrivilegeInformation.PrivilegeLevel);

                        //Check whether master data exists or not.
                        if (itemMaskCollection != null && itemValuesCollection != null)
                        {                            
                            pamLogger.Debug(string.Format("Successfully got the item master data for given {0} privilege level : {1}", rolePrivilegeInformation.IsPrimitive ? "primitive users" : "non-primitive users", rolePrivilegeInformation.PrivilegeLevel.ToString()));

                            //Get the privilege information for the given user.
                            UserPrivilegeInformation userPrivilegeInformation = businessLayer.GetUserPrivileges(userId);

                            //Check if privilege information exists and users privilege level is greater than zero.
                            if (userPrivilegeInformation != null && userPrivilegeInformation.PrivilegeLevel > 0)
                            {
                                //If users privilege information exists.
                                if (userPrivilegeInformation.ResponseCode == GetResponseCode(ResponseCode.SUCCESS))
                                {
                                    pamLogger.Debug("Successfully got the privilege information for given user.");

                                    //Initialize the audit business layer.
                                    auditBusinessLayer = new TF.FocusPay.AuditLogger.BusinessLayer.BusinessOperations();

                                    #region Admin User

                                    //If the given user id is of Admin user.
                                    if (userPrivilegeInformation.RoleId == userPrivilegeInformation.AdminRoleId && userPrivilegeInformation.IsAdmin == true)
                                    {
                                        pamLogger.Debug("Given user is Admin user.");

                                        #region Admin Role

                                        //If the selected role is of Admin(Default) role.
                                        if (rolePrivilegeInformation.IsAdmin)
                                        {
                                            pamLogger.Debug("Selected role is Admin(Default) role.");

                                            //Get the item permissions for selected role.
                                            List<ItemValuesInformation> selectedRolePrivileges = businessLayer.GetUserItemValues(roleId);

                                            //Check whether item permissions exists for the selected role.
                                            if (selectedRolePrivileges != null)
                                            {
                                                pamLogger.Debug("Successfully got the item permissions for the selected role.");

                                                //Check whether there are any new items needs to be added to the selected role.
                                                List<ItemValuesInformation> newItemsCollection = IdentifyNewItems(itemValuesCollection, selectedRolePrivileges);

                                                //If there are any new items exists.
                                                if (newItemsCollection != null && newItemsCollection.Count > 0)
                                                {
                                                    pamLogger.Debug(string.Format("Found {0} new items for the selected role: {1}.", newItemsCollection.Count, roleId.ToString()));

                                                    //Append the new items for the selected role with master data values.
                                                    foreach (ItemValuesInformation newItem in newItemsCollection)
                                                    {
                                                        int responseCode = businessLayer.InsertItemInformation(new NewItemInformation
                                                                             {
                                                                                 RoleId                 = roleId,
                                                                                 ItemId                 = newItem.ItemId,
                                                                                 ItemName               = newItem.ItemName,
                                                                                 IsCreateAllowed        = newItem.IsCreateAllowed,
                                                                                 IsEditAllowed          = newItem.IsEditAllowed,
                                                                                 IsDeleteAllowed        = newItem.IsDeleteAllowed,
                                                                                 IsViewAllowed          = newItem.IsViewAllowed,
                                                                                 UserId                 = userId
                                                                             });

                                                        //Check whether the item information is added successfully or not. If not, send back the
                                                        //role permissions with reponse as 101 (Database Error).
                                                        if (responseCode != GetResponseCode(ResponseCode.SUCCESS))
                                                        {
                                                            pamLogger.Debug("Failed to append the new item information for the selected role.");

                                                            rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.DATABASE_ERROR);
                                                            rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.DATABASE_ERROR));

                                                            return rolePermissions;
                                                        }
                                                        //Else, if the item information added successfully.
                                                        else
                                                        {
                                                            pamLogger.Debug("Successfully appended the new item information for the selected role.");

                                                            //Log the item information for audit purpose.
                                                            auditBusinessLayer.BeginLogAuditInformation(new AuditDataInformation
                                                            {
                                                                TableId                 = TableInfo.RolePrivileges,
                                                                SystemId                = SystemInfo.FocusPayPAM,
                                                                StateId                 = StateInfo.Success,
                                                                EventTypeId             = EventTypeInfo.BackgroundProcess,
                                                                EventId                 = EventInfo.AddNewPrivilegeItem,
                                                                OriginationEventId      = EventInfo.EditRolesDetails,
                                                                EntityId                = EntityInfo.Role,
                                                                OriginationEntityId     = EntityInfo.Role,
                                                                AffectedId              = roleId,
                                                                UserId                  = userId,
                                                                AuditDetail             = "New item added successfully"
                                                            }, null, null);
                                                        }
                                                    }                                                   

                                                    //Get the latest item permissions for selected role.
                                                    selectedRolePrivileges = businessLayer.GetUserItemValues(roleId);

                                                    pamLogger.Debug("Got the latest item permissions for the selected role.");
                                                }
                                                //No new items found for the selected role.
                                                else
                                                {
                                                    pamLogger.Debug("No new items found for the selected role.");
                                                }
                                            }
                                            //Else, send back role permissions with response as 103(System Error).
                                            else
                                            {
                                                pamLogger.Debug("Unable to get the item permissions for the selected role.");

                                                rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                                rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                                return rolePermissions;
                                            }

                                            //Generate the items mask and values permissions by computing as following:
                                            //Values = itemMaskCollection AND selectedRolePrivileges
                                            //Mask   = itemMaskCollection
                                            rolePermissions = GenerateEditRolePermissions(
                                                                BitwiseAndItemPermissions(itemMaskCollection, selectedRolePrivileges),
                                                                itemMaskCollection);

                                            //Assign the role name.
                                            rolePermissions.RoleName = rolePrivilegeInformation.RoleName;

                                            pamLogger.Debug("Successfully generated the items mask and values permissions for the Admin users selected Admin role.");
                                        }

                                        #endregion

                                        #region Non-Admin Role

                                        //Else, if the selected role is of Non-Admin role.
                                        else
                                        {
                                            pamLogger.Debug("Selected role is Non-Admin role.");

                                            //Get the item permissions for users self role.
                                            List<ItemValuesInformation> selfRolePrivileges = businessLayer.GetUserItemValues(userPrivilegeInformation.RoleId);

                                            //Check whether user role has any item permissions.
                                            if (selfRolePrivileges != null)
                                            {
                                                pamLogger.Debug("Successfully got the item permissions for the users role.");

                                                //Check whether there are any new items needs to be added to the users role.
                                                List<ItemValuesInformation> newItemsCollection = IdentifyNewItems(itemValuesCollection, selfRolePrivileges);

                                                //If there are any new items exists.
                                                if (newItemsCollection != null && newItemsCollection.Count > 0)
                                                {
                                                    pamLogger.Debug(string.Format("Found {0} new items for the user role: {1}.", newItemsCollection.Count, userPrivilegeInformation.RoleId.ToString()));

                                                    //Append the new items for the user role with master data values.
                                                    foreach (ItemValuesInformation newItem in newItemsCollection)
                                                    {
                                                        int responseCode = businessLayer.InsertItemInformation(new NewItemInformation
                                                        {
                                                            RoleId              = userPrivilegeInformation.RoleId,
                                                            ItemId              = newItem.ItemId,
                                                            ItemName            = newItem.ItemName,
                                                            IsCreateAllowed     = newItem.IsCreateAllowed,
                                                            IsEditAllowed       = newItem.IsEditAllowed,
                                                            IsDeleteAllowed     = newItem.IsDeleteAllowed,
                                                            IsViewAllowed       = newItem.IsViewAllowed,
                                                            UserId              = userId
                                                        });

                                                        //Check whether the item information is added successfully or not. If not, send back the
                                                        //role permissions with reponse as 101 (Database Error).
                                                        if (responseCode != GetResponseCode(ResponseCode.SUCCESS))
                                                        {
                                                            pamLogger.Debug("Failed to append the new item information to the users role.");

                                                            rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.DATABASE_ERROR);
                                                            rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.DATABASE_ERROR));

                                                            return rolePermissions;
                                                        }
                                                        //Else, if the item information added successfully.
                                                        else
                                                        {
                                                            pamLogger.Debug("Successfully appended the new item information for the users role.");

                                                            //Log the item information for audit purpose.
                                                            auditBusinessLayer.BeginLogAuditInformation(new AuditDataInformation
                                                            {
                                                                TableId                 = TableInfo.RolePrivileges,
                                                                SystemId                = SystemInfo.FocusPayPAM,
                                                                StateId                 = StateInfo.Success,
                                                                EventTypeId             = EventTypeInfo.BackgroundProcess,
                                                                EventId                 = EventInfo.AddNewPrivilegeItem,
                                                                OriginationEventId      = EventInfo.EditRolesDetails,
                                                                EntityId                = EntityInfo.Role,
                                                                OriginationEntityId     = EntityInfo.Role,
                                                                AffectedId              = userPrivilegeInformation.RoleId,
                                                                UserId                  = userId,
                                                                AuditDetail             = "New item added successfully"
                                                            }, null, null);
                                                        }
                                                    }                                                    

                                                    //Get the latest item permissions for users role.
                                                    selfRolePrivileges = businessLayer.GetUserItemValues(userPrivilegeInformation.RoleId);

                                                    pamLogger.Debug("Got the latest item permissions for the users role.");
                                                }
                                                //No new items found for the users role.
                                                else
                                                {
                                                    pamLogger.Debug("No new items found for the users role.");
                                                }
                                            }
                                            //Else, send back role permissions with response as 103(System Error).
                                            else
                                            {
                                                pamLogger.Debug("Unable to get the item permissions for the self role.");

                                                rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                                rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                                return rolePermissions;
                                            }

                                            //Get the item permissions for selected role.
                                            List<ItemValuesInformation> selectedRolePrivileges = businessLayer.GetUserItemValues(roleId);

                                            //Check whether selected role has any item permissions.
                                            if (selectedRolePrivileges != null)
                                            {
                                                pamLogger.Debug("Successfully got the item permissions for the selected role.");

                                                //Check whether there are any new items needs to be added to the selected role.
                                                List<ItemValuesInformation> newItemsCollection = IdentifyNewItems(itemValuesCollection, selectedRolePrivileges);

                                                //If there are any new items exists.
                                                if (newItemsCollection != null && newItemsCollection.Count > 0)
                                                {
                                                    pamLogger.Debug(string.Format("Found {0} new items for the selected role: {1}.", newItemsCollection.Count, roleId.ToString()));

                                                    //Append the new items for the selected role with values as 0(false).
                                                    foreach (ItemValuesInformation newItem in newItemsCollection)
                                                    {
                                                        int responseCode = businessLayer.InsertItemInformation(new NewItemInformation
                                                        {
                                                            RoleId              = roleId,
                                                            ItemId              = newItem.ItemId,
                                                            ItemName            = newItem.ItemName,
                                                            IsCreateAllowed     = false,
                                                            IsEditAllowed       = false,
                                                            IsDeleteAllowed     = false,
                                                            IsViewAllowed       = false,
                                                            UserId              = userId
                                                        });

                                                        //Check whether the item information is added successfully or not. If not, send back the
                                                        //role permissions with reponse as 101 (Database Error).
                                                        if (responseCode != GetResponseCode(ResponseCode.SUCCESS))
                                                        {
                                                            pamLogger.Debug("Failed to append the new item information for the selected role.");

                                                            rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.DATABASE_ERROR);
                                                            rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.DATABASE_ERROR));

                                                            return rolePermissions;
                                                        }
                                                        //Else, if the item information added successfully.
                                                        else
                                                        {
                                                            pamLogger.Debug("Successfully appended the new item information for the selected role.");

                                                            //Log the item information for audit purpose.
                                                            auditBusinessLayer.BeginLogAuditInformation(new AuditDataInformation
                                                            {
                                                                TableId                 = TableInfo.RolePrivileges,
                                                                SystemId                = SystemInfo.FocusPayPAM,
                                                                StateId                 = StateInfo.Success,
                                                                EventTypeId             = EventTypeInfo.BackgroundProcess,
                                                                EventId                 = EventInfo.AddNewPrivilegeItem,
                                                                OriginationEventId      = EventInfo.EditRolesDetails,
                                                                EntityId                = EntityInfo.Role,
                                                                OriginationEntityId     = EntityInfo.Role,
                                                                AffectedId              = roleId,
                                                                UserId                  = userId,
                                                                AuditDetail             = "New item added successfully"
                                                            }, null, null);
                                                        }
                                                    }                                                   

                                                    //Get the latest item permissions for selected role.
                                                    selectedRolePrivileges = businessLayer.GetUserItemValues(roleId);

                                                    pamLogger.Debug("Got the latest item permissions for the selected role.");
                                                }
                                                //No new items found for the selected role.
                                                else
                                                {
                                                    pamLogger.Debug("No new items found for the selected role.");
                                                }
                                            }
                                            //Else, send back role permissions with response as 103(System Error).
                                            else
                                            {
                                                pamLogger.Debug("Unable to get the item permissions for the selected role.");

                                                rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                                rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                                return rolePermissions;
                                            }

                                            //Perform bitwise AND for item mask and user role permissions.
                                            itemMaskCollection = BitwiseAndItemPermissions(itemMaskCollection, selfRolePrivileges);

                                            pamLogger.Debug("Successfully performed bitwise AND for item mask and user role permissions.");

                                            //Generate the items mask and values permissions by computing as following:
                                            //Values = itemMaskCollection AND selfRolePrivileges AND selectedRolePrivileges
                                            //Mask   = itemMaskCollection AND selfRolePrivileges
                                            rolePermissions = GenerateEditRolePermissions(
                                                                BitwiseAndItemPermissions(itemMaskCollection, selectedRolePrivileges),
                                                                itemMaskCollection);

                                            //Assign the role name.
                                            rolePermissions.RoleName = rolePrivilegeInformation.RoleName;

                                            pamLogger.Debug("Successfully generated the items mask and values permissions for the Admin users selected Non-Admin role.");
                                        }

                                        #endregion
                                    }

                                    #endregion

                                    #region Non-Admin User

                                    //Else, if the given user id is of Non-Admin user.
                                    else
                                    {
                                        pamLogger.Debug("Given user is Non-Admin user and selected role is Non-Admin role.");                                        

                                        //Get the item permissions for users admin role.
                                        List<ItemValuesInformation> adminRolePrivileges = businessLayer.GetUserItemValues(userPrivilegeInformation.AdminRoleId);

                                        //Check whether users admin role has any item permissions.
                                        if (adminRolePrivileges != null)
                                        {
                                            pamLogger.Debug("Successfully got the item permissions for the users admin role.");

                                            //Check whether there are any new items needs to be added to the users admin role.
                                            List<ItemValuesInformation> newAdminItemsCollection = IdentifyNewItems(itemValuesCollection, adminRolePrivileges);

                                            //If there are any new items exists.
                                            if (newAdminItemsCollection != null && newAdminItemsCollection.Count > 0)
                                            {
                                                pamLogger.Debug(string.Format("Found {0} new items for the users admin role: {1}.", newAdminItemsCollection.Count, userPrivilegeInformation.AdminRoleId.ToString()));

                                                //Append the new items for the users admin role with master data values.
                                                foreach (ItemValuesInformation newAdminItem in newAdminItemsCollection)
                                                {
                                                    int responseCode = businessLayer.InsertItemInformation(new NewItemInformation
                                                                         {
                                                                             RoleId             = userPrivilegeInformation.AdminRoleId,
                                                                             ItemId             = newAdminItem.ItemId,
                                                                             ItemName           = newAdminItem.ItemName,
                                                                             IsCreateAllowed    = newAdminItem.IsCreateAllowed,
                                                                             IsEditAllowed      = newAdminItem.IsEditAllowed,
                                                                             IsDeleteAllowed    = newAdminItem.IsDeleteAllowed,
                                                                             IsViewAllowed      = newAdminItem.IsViewAllowed,
                                                                             UserId             = userId
                                                                         });

                                                    //Check whether the item information is added successfully or not. If not, send back the
                                                    //role permissions with reponse as 101 (Database Error).
                                                    if (responseCode != GetResponseCode(ResponseCode.SUCCESS))
                                                    {
                                                        pamLogger.Debug("Failed to append the new item information to admin role.");

                                                        rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.DATABASE_ERROR);
                                                        rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.DATABASE_ERROR));

                                                        return rolePermissions;
                                                    }
                                                    //Else, if the item information added successfully.
                                                    else
                                                    {
                                                        pamLogger.Debug("Successfully appended the new item information to admin role.");

                                                        //Log the item information for audit purpose.
                                                        auditBusinessLayer.BeginLogAuditInformation(new AuditDataInformation
                                                        {
                                                            TableId                     = TableInfo.RolePrivileges,
                                                            SystemId                    = SystemInfo.FocusPayPAM,
                                                            StateId                     = StateInfo.Success,
                                                            EventTypeId                 = EventTypeInfo.BackgroundProcess,
                                                            EventId                     = EventInfo.AddNewPrivilegeItem,
                                                            OriginationEventId          = EventInfo.EditRolesDetails,
                                                            EntityId                    = EntityInfo.Role,
                                                            OriginationEntityId         = EntityInfo.Role,
                                                            AffectedId                  = userPrivilegeInformation.AdminRoleId,
                                                            UserId                      = userId,
                                                            AuditDetail                 = "New item added successfully"
                                                        }, null, null);
                                                    }                                                    

                                                    //Get the latest item permissions for users admin role.
                                                    adminRolePrivileges = businessLayer.GetUserItemValues(userPrivilegeInformation.AdminRoleId);

                                                    pamLogger.Debug("Got the latest item permissions for the users admin role.");
                                                }
                                            }
                                            //No new items found for the users admin role.
                                            else
                                            {
                                                pamLogger.Debug("No new items found for the users admin role.");
                                            }
                                        }
                                        //Else, send back role permissions with response as 103(System Error).
                                        else
                                        {
                                            pamLogger.Debug("Unable to get the item permissions for the users admin role.");

                                            rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                            rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                            return rolePermissions;
                                        }

                                        //Get the item permissions for users role.
                                        List<ItemValuesInformation> selfRolePrivileges = businessLayer.GetUserItemValues(userPrivilegeInformation.RoleId);

                                        //Check whether user role has any item permissions.
                                        if (selfRolePrivileges != null)
                                        {
                                            pamLogger.Debug("Successfully got the item permissions for the users role.");

                                            //Check whether there are any new items needs to be added to the users role.
                                            List<ItemValuesInformation> newItemsCollection = IdentifyNewItems(itemValuesCollection, selfRolePrivileges);

                                            //If there are any new items exists.
                                            if (newItemsCollection != null && newItemsCollection.Count > 0)
                                            {
                                                pamLogger.Debug(string.Format("Found {0} new items for the users role: {1}.", newItemsCollection.Count, userPrivilegeInformation.RoleId.ToString()));

                                                //Append the new items for the user role with values as 0(false).
                                                foreach (ItemValuesInformation newItem in newItemsCollection)
                                                {
                                                    int responseCode = businessLayer.InsertItemInformation(new NewItemInformation
                                                    {
                                                        RoleId              = userPrivilegeInformation.RoleId,
                                                        ItemId              = newItem.ItemId,
                                                        ItemName            = newItem.ItemName,
                                                        IsCreateAllowed     = false,
                                                        IsEditAllowed       = false,
                                                        IsDeleteAllowed     = false,
                                                        IsViewAllowed       = false,
                                                        UserId              = userId
                                                    });

                                                    //Check whether the item information is added successfully or not. If not, send back the
                                                    //role permissions with reponse as 101 (Database Error).
                                                    if (responseCode != GetResponseCode(ResponseCode.SUCCESS))
                                                    {
                                                        pamLogger.Debug("Failed to append the new item information to users role.");

                                                        rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.DATABASE_ERROR);
                                                        rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.DATABASE_ERROR));

                                                        return rolePermissions;
                                                    }
                                                    //Else, if the item information added successfully.
                                                    else
                                                    {
                                                        pamLogger.Debug("Successfully appended the new item information for the users role.");

                                                        //Log the item information for audit purpose.
                                                        auditBusinessLayer.BeginLogAuditInformation(new AuditDataInformation
                                                        {
                                                            TableId                 = TableInfo.RolePrivileges,
                                                            SystemId                = SystemInfo.FocusPayPAM,
                                                            StateId                 = StateInfo.Success,
                                                            EventTypeId             = EventTypeInfo.BackgroundProcess,
                                                            EventId                 = EventInfo.AddNewPrivilegeItem,
                                                            OriginationEventId      = EventInfo.EditRolesDetails,
                                                            EntityId                = EntityInfo.Role,
                                                            OriginationEntityId     = EntityInfo.Role,
                                                            AffectedId              = userPrivilegeInformation.RoleId,
                                                            UserId                  = userId,
                                                            AuditDetail             = "New item added successfully"
                                                        }, null, null);
                                                    }
                                                }                                               

                                                //Get the latest item permissions for users role.
                                                selfRolePrivileges = businessLayer.GetUserItemValues(userPrivilegeInformation.RoleId);

                                                pamLogger.Debug("Got the latest item permissions for the users role.");
                                            }
                                            //No new items found for the user role.
                                            else
                                            {
                                                pamLogger.Debug("No new items found for the user role.");
                                            }
                                        }
                                        //Else, send back role permissions with response as 103(System Error).
                                        else
                                        {
                                            pamLogger.Debug("Unable to get the item permissions for the users role.");

                                            rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                            rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                            return rolePermissions;
                                        }

                                        //Get the item permissions for selected role.
                                        List<ItemValuesInformation> selectedRolePrivileges = businessLayer.GetUserItemValues(roleId);

                                        //Check whether selected role has any item permissions.
                                        if (selectedRolePrivileges != null)
                                        {
                                            pamLogger.Debug("Successfully got the item permissions for the selected role.");

                                            //Check whether there are any new items needs to be added to the selected role.
                                            List<ItemValuesInformation> newItemsCollection = IdentifyNewItems(itemValuesCollection, selectedRolePrivileges);

                                            //If there are any new items exists.
                                            if (newItemsCollection != null && newItemsCollection.Count > 0)
                                            {
                                                pamLogger.Debug(string.Format("Found {0} new items for the selected role: {1}.", newItemsCollection.Count, roleId.ToString()));

                                                //Append the new items for the selected role with values as 0(false).
                                                foreach (ItemValuesInformation newItem in newItemsCollection)
                                                {
                                                    int responseCode = businessLayer.InsertItemInformation(new NewItemInformation
                                                    {
                                                        RoleId              = roleId,
                                                        ItemId              = newItem.ItemId,
                                                        ItemName            = newItem.ItemName,
                                                        IsCreateAllowed     = false,
                                                        IsEditAllowed       = false,
                                                        IsDeleteAllowed     = false,
                                                        IsViewAllowed       = false,
                                                        UserId              = userId
                                                    });

                                                    //Check whether the item information is added successfully or not. If not, send back the
                                                    //role permissions with reponse as 101 (Database Error).
                                                    if (responseCode != GetResponseCode(ResponseCode.SUCCESS))
                                                    {
                                                        pamLogger.Debug("Failed to append the new item information for the selected role.");

                                                        rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.DATABASE_ERROR);
                                                        rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.DATABASE_ERROR));

                                                        return rolePermissions;
                                                    }
                                                    //Else, if the item information added successfully.
                                                    else
                                                    {
                                                        pamLogger.Debug("Successfully appended the new item information for the selected role.");

                                                        //Log the item information for audit purpose.
                                                        auditBusinessLayer.BeginLogAuditInformation(new AuditDataInformation
                                                        {
                                                            TableId                 = TableInfo.RolePrivileges,
                                                            SystemId                = SystemInfo.FocusPayPAM,
                                                            StateId                 = StateInfo.Success,
                                                            EventTypeId             = EventTypeInfo.BackgroundProcess,
                                                            EventId                 = EventInfo.AddNewPrivilegeItem,
                                                            OriginationEventId      = EventInfo.EditRolesDetails,
                                                            EntityId                = EntityInfo.Role,
                                                            OriginationEntityId     = EntityInfo.Role,
                                                            AffectedId              = roleId,
                                                            UserId                  = userId,
                                                            AuditDetail             = "New item added successfully"
                                                        }, null, null);
                                                    }
                                                }                                                

                                                //Get the latest item permissions for selected role.
                                                selectedRolePrivileges = businessLayer.GetUserItemValues(roleId);

                                                pamLogger.Debug("Got the latest item permissions for the selected role.");
                                            }
                                            //No new items found for the selected role.
                                            else
                                            {
                                                pamLogger.Debug("No new items found for the selected role.");
                                            }
                                        }
                                        //Else, send back role permissions with response as 103(System Error).
                                        else
                                        {
                                            pamLogger.Debug("Unable to get the item permissions for the selected role.");

                                            rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                            rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                            return rolePermissions;
                                        }

                                        //Perform bitwise AND for item mask and admin role permissions.
                                        itemMaskCollection = BitwiseAndItemPermissions(itemMaskCollection, adminRolePrivileges);

                                        pamLogger.Debug("Successfully performed bitwise AND for item mask and admin role permissions.");

                                        //Generate the items mask and values permissions by computing as following:
                                        //Values = itemMaskCollection AND adminRolePrivileges AND selectedRolePrivileges
                                        //Mask   = itemMaskCollection AND adminRolePrivileges AND selfRolePrivileges
                                        rolePermissions = GenerateEditRolePermissions(
                                                            BitwiseAndItemPermissions(itemMaskCollection, selectedRolePrivileges),
                                                            BitwiseAndItemPermissions(itemMaskCollection, selfRolePrivileges));

                                        //Assign the role name.
                                        rolePermissions.RoleName = rolePrivilegeInformation.RoleName;

                                        pamLogger.Debug("Successfully generated the items mask and values permissions for the Non-Admin users selected Non-Admin role.");
                                    }

                                    #endregion
                                }
                                //Else, send back with generated response code 12 (Unregistered User).
                                else
                                {
                                    rolePermissions.ResponseCode    = userPrivilegeInformation.ResponseCode;
                                    rolePermissions.ResponseMessage = GetResponseCodeDescription(userPrivilegeInformation.ResponseCode);

                                    pamLogger.Debug(string.Format("Unable to get the privilege information for the given user : {0}", rolePermissions.ResponseMessage));
                                }
                            }
                            //Else, send back role permissions with response as 103(System Error).
                            else
                            {
                                rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                pamLogger.Debug("Unable to get the privilege information for the given user.");
                            }
                        }
                        //Else, send back role permissions with response as 103(System Error).
                        else
                        {
                            rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                            rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                            pamLogger.Debug(string.Format("Unable to get the master data for given {0} privilege level : {1}", rolePrivilegeInformation.IsPrimitive ? "primitive users" : "non-primitive users", rolePrivilegeInformation.PrivilegeLevel.ToString()));                            
                        }
                    }
                    //Else, send back role permissions with response as 103(System Error).
                    else
                    {
                        rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                        rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                        pamLogger.Debug("Failed to get the privilege level information for the selected role.");                        
                    }
                }
                //Else, send back the null role permissions.
                else
                {
                    pamLogger.Debug("Unable to get the privilege level information for the selected role.");

                    //Send back the null role permissions.
                    rolePermissions = null;
                }
            }
            else
            {
                pamLogger.Debug("Either User ID (or) Role ID is 0(Zero).");

                //Send back the null role permissions.
                rolePermissions = null;
            }
        }
        catch (Exception errorMessage)
        {
            pamLogger.Error("Exception on executing GetSelectedRolePermissions", errorMessage);

            //Send back the null role permissions.
            rolePermissions = null;
        }
        finally
        {
            //Dispose the business layer. 
            if (businessLayer != null)
            {
                businessLayer.Dispose();
                businessLayer = null;
            }
        }

        //Send back the role permissions.
        return rolePermissions;
    }

    #endregion

    #region GetUserRolePermissions

    /// <summary>
    /// This method gets the role permissions associated with the given role id.
    /// </summary>
    /// <param name="roleId">Selected role id.</param>
    /// <returns>Role permission information that contains all items(screens and actions) permission information.</returns>
    [WebMethod(Description = "Gets the role permissions associated with the given role id.")]
    public RolePermissions GetUserRolePermissions(short roleId)
    {
        pamLogger.Debug("Processing GetUserRolePermissions request.");

        //Declare the role permissions object.
        RolePermissions rolePermissions = null;

        try
        {
            //Check whether the role id is greater than zero or not.
            if (roleId > 0)
            {
                //Initialize the business layer.
                businessLayer = new BusinessOperations(databaseConnectionString);

                //Get the privilege information for the given role.
                UserPrivilegeInformation userPrivilegeInformation = businessLayer.GetUserRolePrivileges(roleId);

                //Check if privilege information exists and roles privilege level is greater than zero.
                if (userPrivilegeInformation != null && userPrivilegeInformation.PrivilegeLevel > 0)
                {
                    //Initialize the role permissions object.
                    rolePermissions = new RolePermissions();

                    //If roles privilege information exists.
                    if (userPrivilegeInformation.ResponseCode == GetResponseCode(ResponseCode.SUCCESS))
                    {
                        pamLogger.Debug("Successfully got the privilege information for given role.");

                        //Get item mask information from the master data for the given privilege level (1/2/3/4).
                        List<ItemMaskInformation> itemMaskCollection = businessLayer.GetItemMask(userPrivilegeInformation.PrivilegeLevel, userPrivilegeInformation.IsPrimitive);
                       
                        //Check whether master mask data exists or not.
                        if (itemMaskCollection != null)
                        {
                            //Declare admin role privileges collection.
                            List<ItemValuesInformation> adminRolePrivileges = null;

                            //Check whether master mask data contains any information.
                            if (itemMaskCollection.Count > 0)
                            {
                                pamLogger.Debug(string.Format("Successfully got the item mask master data for given {0} privilege level : {1}", userPrivilegeInformation.IsPrimitive ? "primitive roles" : "non-primitive roles", userPrivilegeInformation.PrivilegeLevel.ToString()));

                                #region Admin Role

                                //If the given role id is of Admin role.
                                if (roleId == userPrivilegeInformation.AdminRoleId && userPrivilegeInformation.IsAdmin == true)
                                {
                                    pamLogger.Debug("Given role is Admin role.");

                                    //Get the item permissions for admin/self role.
                                    adminRolePrivileges = businessLayer.GetUserItemValues(roleId);

                                    //Check whether admin/self role has any item permissions.
                                    if (adminRolePrivileges != null)
                                    {
                                        pamLogger.Debug("Successfully got the item permissions for the admin/self role.");                                        
                                    }
                                    //Else, send back role permissions with response as 103(System Error).
                                    else
                                    {
                                        pamLogger.Debug("Unable to get the item permissions for the admin/self role.");

                                        rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                        rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                        return rolePermissions;
                                    }
                                }

                                #endregion

                                #region Non-Admin Role

                                //If the given role id is of Non-Admin role.
                                else
                                {
                                    pamLogger.Debug("Given role is Non-Admin role.");

                                    //Get the item permissions for admin role.
                                    adminRolePrivileges = businessLayer.GetUserItemValues(userPrivilegeInformation.AdminRoleId);

                                    //Check whether admin role has any item permissions.
                                    if (adminRolePrivileges != null)
                                    {
                                        pamLogger.Debug("Successfully got the item permissions for the admin role.");                                        
                                    }
                                    //Else, send back role permissions with response as 103(System Error).
                                    else
                                    {
                                        pamLogger.Debug("Unable to get the item permissions for the admins role.");

                                        rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                        rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                        return rolePermissions;
                                    }

                                    //Get the item permissions for self role.
                                    List<ItemValuesInformation> selfRolePrivileges = businessLayer.GetUserItemValues(roleId);

                                    //Check whether self role has any item permissions.
                                    if (selfRolePrivileges != null)
                                    {
                                        pamLogger.Debug("Successfully got the item permissions for the self role.");                                        
                                    }
                                    //Else, send back role permissions with response as 103(System Error).
                                    else
                                    {
                                        pamLogger.Debug("Unable to get the item permissions for the self role.");

                                        rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                        rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                        return rolePermissions;
                                    }

                                    //Perform bitwise AND for admin and self roles item permissions.
                                    adminRolePrivileges = BitwiseAndItemPermissions(adminRolePrivileges, selfRolePrivileges);

                                    pamLogger.Debug("Successfully performed bitwise AND for admin and self roles item permissions.");
                                }

                                #endregion
                            }
                            //Else, send back role permissions with response as 103(System Error).
                            else
                            {
                                rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                pamLogger.Debug("No information exists in the item mask master data.");

                                return rolePermissions;
                            }

                            //Perform bitwise AND for item mask and admin roles item permissions.
                            itemMaskCollection = BitwiseAndItemPermissions(itemMaskCollection, adminRolePrivileges);

                            //Check whether bitwise AND operation performed successfully or not.
                            if (itemMaskCollection != null && itemMaskCollection.Count > 0)
                            {
                                pamLogger.Debug("Successfully performed bitwise AND for item mask and admin roles item permissions.");

                                rolePermissions.ResponseCode                = GetResponseCode(ResponseCode.SUCCESS);
                                rolePermissions.ResponseMessage             = GetResponseCodeDescription(GetResponseCode(ResponseCode.SUCCESS));
                                rolePermissions.ItemInformationCollection   = itemMaskCollection;
                            }
                            //Else, send back role permissions with response as 103(System Error).
                            else
                            {
                                rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                                rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                                pamLogger.Debug("Failed to perform bitwise AND for item mask and admin roles item permissions.");
                            }
                        }
                        //Else, send back role permissions with response as 103(System Error).
                        else
                        {
                            rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                            rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                            pamLogger.Debug(string.Format("Unable to get the master mask data for given {0} privilege level : {1}", userPrivilegeInformation.IsPrimitive ? "primitive roles" : "non-primitive roles", userPrivilegeInformation.PrivilegeLevel.ToString()));
                        }
                    }
                    //Else, send back role permissions with response as 103(System Error).
                    else
                    {
                        rolePermissions.ResponseCode    = GetResponseCode(ResponseCode.CHANAKYA_ERROR);
                        rolePermissions.ResponseMessage = GetResponseCodeDescription(GetResponseCode(ResponseCode.CHANAKYA_ERROR));

                        pamLogger.Debug(string.Format("Unable to get the privilege information for the given role : {0}", rolePermissions.ResponseMessage));
                    }
                }
                //Else, Send back the null role permissions.
                else
                {
                    pamLogger.Debug("Unable to get the privilege information for the given role.");
                }
            }
            else
            {
                pamLogger.Debug("Role ID is 0(Zero).");

                //Send back the null role permissions.
                rolePermissions = null;
            }
        }
        catch (Exception errorMessage)
        {
            pamLogger.Error("Exception on executing GetUserRolePermissions", errorMessage);

            //Send back the null role permissions.
            rolePermissions = null;
        }
        finally
        {
            //Dispose the business layer. 
            if (businessLayer != null)
            {
                businessLayer.Dispose();
                businessLayer = null;
            }
        }

        //Send back the role permissions.
        return rolePermissions;
    }

    #endregion
}

#endregion
