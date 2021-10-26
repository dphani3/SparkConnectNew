
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Utility.cs
  Description: This utility class is the extended partial class for manage class to support helper functions.
  Date Created : 20-July-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.SessionState;
using TF.PAM.BusinessLayer.BusinessObjects;

#endregion

#region Utility

/// <summary>
/// This utility class is the extended partial class for manage class to support helper functions.
/// </summary>
public partial class Manage
{
    #region Member Variables

    private const string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";     //Allowed lower case password characters.
    private const string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";     //Allowed upper case password characters.
    private const string PASSWORD_CHARS_NUMERIC = "123456789";                //Allowed numeric password characters.
    private const string PASSWORD_CHARS_SPECIAL = "~`!@#$%^&*(),.:;+=|?/'_\\{}-[]";   //Allowed special password characters.Modified by Nazreen on 22-Oct-2012.  

    #endregion

    #region GeneratePasswrod

    /// <summary>
    /// This method is used to generate the password automatically with provided boundary conditions.
    /// </summary>
    /// <param name="minimumLength">Minimum length required for password generation.</param>
    /// <param name="maximumLength">Maximum length required for password generation.</param>
    /// <returns>System generated strong password.</returns>
    private string GeneratePasswrod(int minimumLength, int maximumLength)
    {
        //Make sure that input parameters are valid.
        if (minimumLength <= 0 || maximumLength <= 0 || minimumLength > maximumLength)
            return null;

        //Create a local array containing supported password characters grouped by types. You can remove character groups from this
        //array, but doing so will weaken the password strength.
        char[][] characterGroups = new char[][]		
            {
                PASSWORD_CHARS_LCASE.ToCharArray(),
			    PASSWORD_CHARS_UCASE.ToCharArray(),
			    PASSWORD_CHARS_NUMERIC.ToCharArray(),
                PASSWORD_CHARS_SPECIAL.ToCharArray() //Added by Nazreen on 11-Oct-2012.  
            };

        //Use this array to track the number of unused characters in each character group.
        int[] charactersLeftInGroup = new int[characterGroups.Length];

        //Initially, all characters in each group are not used.
        for (int i = 0; i < charactersLeftInGroup.Length; i++)
            charactersLeftInGroup[i] = characterGroups[i].Length;

        //Use this array to track (iterate through) unused character groups.
        int[] leftGroupsOrder = new int[characterGroups.Length];

        //Initially, all character groups are not used.
        for (int i = 0; i < leftGroupsOrder.Length; i++)
            leftGroupsOrder[i] = i;

        byte[] randomBytes = new byte[4];
        //Generate 4 byte crypto stream.
        RNGCryptoServiceProvider rngCryptoProvider = new RNGCryptoServiceProvider();
        rngCryptoProvider.GetBytes(randomBytes);

        //Convert 4 bytes into a 32-bit integer value.
        int seed = (randomBytes[0] & 0x7f) << 24 |
                            randomBytes[1] << 16 |
                             randomBytes[2] << 8 |
                                    randomBytes[3];

        //Use the generated 32-bit integer as a seed for random number generation.
        Random random = new Random(seed);

        // This array will hold password characters.
        char[] password = null;
        if (minimumLength < maximumLength)
            password = new char[random.Next(minimumLength, maximumLength + 1)];
        else
            password = new char[minimumLength];

        int nextCharIdx;
        int nextGroupIdx;
        int nextLeftGroupsOrderIdx;
        int lastCharIdx;
        int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

        for (int i = 0; i < password.Length; i++)
        {
            if (lastLeftGroupsOrderIdx == 0)
                nextLeftGroupsOrderIdx = 0;
            else
                nextLeftGroupsOrderIdx = random.Next(0,
                    lastLeftGroupsOrderIdx);

            nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];
            lastCharIdx = charactersLeftInGroup[nextGroupIdx] - 1;

            if (lastCharIdx == 0)
                nextCharIdx = 0;
            else
                nextCharIdx = random.Next(0, lastCharIdx + 1);

            password[i] = characterGroups[nextGroupIdx][nextCharIdx];

            if (lastCharIdx == 0)
                charactersLeftInGroup[nextGroupIdx] =
                    characterGroups[nextGroupIdx].Length;
            else
            {
                if (lastCharIdx != nextCharIdx)
                {
                    char temp = characterGroups[nextGroupIdx][lastCharIdx];
                    characterGroups[nextGroupIdx][lastCharIdx] =
                        characterGroups[nextGroupIdx][nextCharIdx];
                    characterGroups[nextGroupIdx][nextCharIdx] = temp;
                }

                charactersLeftInGroup[nextGroupIdx]--;
            }
            if (lastLeftGroupsOrderIdx == 0)
                lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            else
            {
                if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                {
                    int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                    leftGroupsOrder[lastLeftGroupsOrderIdx] =
                        leftGroupsOrder[nextLeftGroupsOrderIdx];
                    leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                }

                lastLeftGroupsOrderIdx--;
            }
        }

        return new string(password);
    }

    #endregion

    #region GenerateSalt

    /// <summary>
    /// This will generate the Base64 encoded cryptographic random salt.
    /// </summary>
    /// <returns>Base64 encoded cryptographic random salt.</returns>
    private string GenerateSalt()
    {
        byte[] cryptographicRandomData = new byte[16];
        new RNGCryptoServiceProvider().GetBytes(cryptographicRandomData);

        return Convert.ToBase64String(cryptographicRandomData);
    }

    #endregion

    #region EncryptPassword

    /// <summary>
    /// Encrypts the given plain text password with base64 encoded cryptographic password salt using SHA1 one way hashing algorithm.
    /// </summary>
    /// <param name="password">Password in plain text.</param>
    /// <param name="passwordSalt">Base64 encoded cryptographic random salt.</param>
    /// <returns>Encrypted password.</returns>
    private string EncryptPassword(string password, string passwordSalt)
    {
        //Convert both the password and salt into byte array.
        byte[] passwordBuffer = Encoding.Unicode.GetBytes(password);
        byte[] saltBuffer = Convert.FromBase64String(passwordSalt);

        //Create the empty buffer for encrypted password with the size of summation of password and salt buffers.
        byte[] encryptedPasswordBuffer = new byte[saltBuffer.Length + passwordBuffer.Length];

        //Copy the salt data and password data into empty buffer respectively.
        Array.Copy(saltBuffer, 0, encryptedPasswordBuffer, 0, saltBuffer.Length);
        Array.Copy(passwordBuffer, 0, encryptedPasswordBuffer, saltBuffer.Length, passwordBuffer.Length);

        byte[] cryptoPasswordBuffer = null;

        //Compute the hash using SHA1 one way hashing algorithm.
        using (SHA1 sha1Algorithm = new SHA1CryptoServiceProvider())
        {
            cryptoPasswordBuffer = sha1Algorithm.ComputeHash(encryptedPasswordBuffer);
        }

        //Send back the base64 encoded encrypted password.
        return Convert.ToBase64String(cryptoPasswordBuffer);
    }

    #endregion

    #region ValidatePassword

    /// <summary>
    /// Validates the given encrypted password with computed encrypted password that is generated from plain text password and password salt.
    /// </summary>
    /// <param name="plainTextPassword">Plain text password.</param>
    /// <param name="encryptedPassword">Encrypted password from database.</param>
    /// <param name="passwordSalt">Password salt from database.</param>
    /// <returns>Boolean flag that details the status of the password validation.</returns>
    /// <list type="bullet">
    ///     <item>
    ///         <description>True -> Valid user password.</description>
    ///     </item>
    ///     <item>
    ///         <description>False -> Invalid user password.</description>
    ///     </item>      
    /// </list>    
    private bool ValidatePassword(string plainTextPassword, string encryptedPassword, string passwordSalt)
    {
        //Initialize the flag to false.
        bool isValidPassword = false;

        if (!String.IsNullOrEmpty(plainTextPassword) && !String.IsNullOrEmpty(encryptedPassword) && !String.IsNullOrEmpty(passwordSalt))
        {
            //Compute the encrypted password from given plain text password and password salt.
            string generatedEncryptedPassword = EncryptPassword(plainTextPassword, passwordSalt);

            //If the encrypted password generated successfully.
            if (!String.IsNullOrEmpty(generatedEncryptedPassword))
            {
                //Compare against the given input encrypted password value.
                if (String.Compare(encryptedPassword, generatedEncryptedPassword, false) == 0)
                {
                    //If matches, set the password validation status as success.
                    isValidPassword = true;
                }
            }
        }

        //Send back the password validation status.
        return isValidPassword;
    }

    #endregion

    #region GenerateSessionId

    /// <summary>
    /// Creates the unique session identifier for the current http context.
    /// </summary>
    /// <returns>Unique Session ID.</returns>
    private string GenerateSessionId()
    {
        SessionIDManager sessionManager = new SessionIDManager();
        return sessionManager.CreateSessionID(HttpContext.Current);
    }

    #endregion

    #region IdentifyNewItems

    /// <summary>
    /// Identifies whether any new items exists in the given primary collection on comparing with the secondary collection.
    /// </summary>
    /// <param name="primaryCollecton">Primary item collection.</param>
    /// <param name="secondaryCollection">Secondary item collection.</param>
    /// <returns>New items collection if any.</returns>
    private List<ItemValuesInformation> IdentifyNewItems(List<ItemValuesInformation> primaryCollecton, 
                                                            List<ItemValuesInformation> secondaryCollection)
    {
        List<ItemValuesInformation> newItemCollection = null;

        if (primaryCollecton != null && secondaryCollection != null)
        {
            if (primaryCollecton.Count > 0 && secondaryCollection.Count > 0)
            {
                //Check whether any new items exists in the primary collection based on item id.
                newItemCollection = primaryCollecton.Except(secondaryCollection, new ItemValuesComparer()).ToList();                
            }
        }

        //Send new items collection if any.
        return newItemCollection;
    }

    #endregion

    #region BitwiseAndItemPermissions Overloaded Methods

    #region BitwiseAndItemPermissions - 1

    /// <summary>
    /// Performs bitwise AND operation for the given primary and secondary item values permissions like Create, Edit, Delete and View.
    /// </summary>
    /// <param name="primaryCollecton">Primary item values collection.</param>
    /// <param name="secondaryCollection">Secondary item values collection.</param>
    /// <returns>Bitwise ANDed item values collection.</returns>
    private List<ItemValuesInformation> BitwiseAndItemPermissions(List<ItemValuesInformation> primaryCollecton,
                                                                    List<ItemValuesInformation> secondaryCollection) 
    {
        List<ItemValuesInformation> composedItemCollection = null;

        if (primaryCollecton != null && secondaryCollection != null)
        {
            if (primaryCollecton.Count > 0 && secondaryCollection.Count > 0)
            {
                //Perform bitwise AND operation over the Create, Edit, Delete and View permissions of both collections.
                composedItemCollection = (from primary in primaryCollecton
                                          from secondary in secondaryCollection
                                          where primary.ItemId == secondary.ItemId
                                          select new ItemValuesInformation
                                          {
                                              ItemId            = primary.ItemId,
                                              ItemName          = primary.ItemName,
                                              IsCreateAllowed   = primary.IsCreateAllowed & secondary.IsCreateAllowed,
                                              IsEditAllowed     = primary.IsEditAllowed & secondary.IsEditAllowed,
                                              IsDeleteAllowed   = primary.IsDeleteAllowed & secondary.IsDeleteAllowed,
                                              IsViewAllowed     = primary.IsViewAllowed & secondary.IsViewAllowed
                                          }).ToList();
            }
        }

        //Send bitwise ANDed item values collection.
        return composedItemCollection;
    }

    #endregion

    #region BitwiseAndItemPermissions - 2

    /// <summary>
    /// Performs bitwise AND operation for the given primary item mask and secondary item values permissions like Create, Edit, Delete and View.
    /// </summary>
    /// <param name="primaryCollecton">Primary item mask collection.</param>
    /// <param name="secondaryCollection">Secondary item values collection.</param>
    /// <returns>Bitwise ANDed item mask collection.</returns>
    private List<ItemMaskInformation> BitwiseAndItemPermissions(List<ItemMaskInformation> primaryCollecton,
                                                                    List<ItemValuesInformation> secondaryCollection)
    {
        List<ItemMaskInformation> composedItemCollection = null;

        if (primaryCollecton != null && secondaryCollection != null)
        {
            if (primaryCollecton.Count > 0 && secondaryCollection.Count > 0)
            {
                //Perform bitwise AND operation over the Create, Edit, Delete and View permissions of both collections.
                composedItemCollection = (from primary in primaryCollecton
                                          from secondary in secondaryCollection
                                          where primary.ItemId == secondary.ItemId
                                          select new ItemMaskInformation
                                          {
                                              ItemId            = primary.ItemId,
                                              ItemName          = primary.ItemName,
                                              IsCreateAllowed   = primary.IsCreateAllowed & secondary.IsCreateAllowed,
                                              IsEditAllowed     = primary.IsEditAllowed & secondary.IsEditAllowed,
                                              IsDeleteAllowed   = primary.IsDeleteAllowed & secondary.IsDeleteAllowed,
                                              IsViewAllowed     = primary.IsViewAllowed & secondary.IsViewAllowed
                                          }).ToList();                
            }
        }

        //Send bitwise ANDed item mask collection.
        return composedItemCollection;
    }

    #endregion    

    #endregion

    #region GenerateEditRolePermissions

    /// <summary>
    /// Generates the items mask and values permissions w.r.t. to item id.
    /// </summary>
    /// <param name="valuesCollection">Item values collection.</param>
    /// <param name="maskCollection">Item mask collection.</param>
    /// <returns>Composed item collection with mask and values for permissions.</returns>
    private EditRolePermissions GenerateEditRolePermissions(List<ItemMaskInformation> valuesCollection,
                                                                    List<ItemMaskInformation> maskCollection)
    {
        EditRolePermissions editRolePermissions = null;

        if (valuesCollection != null && maskCollection != null)
        {
            if (valuesCollection.Count > 0 && maskCollection.Count > 0)
            {
                editRolePermissions = new EditRolePermissions
                {
                    ResponseCode                        = GetResponseCode(ResponseCode.SUCCESS),
                    ResponseMessage                     = GetResponseCodeDescription(GetResponseCode(ResponseCode.SUCCESS)),
                    ItemEditInformationCollection       = new List<ItemEditInformation>()
                };

                //Compose item collection with values and mask from respective input collections.
                editRolePermissions.ItemEditInformationCollection = (from valuesData in valuesCollection
                                                                     from maskData in maskCollection
                                                                     where valuesData.ItemId == maskData.ItemId
                                                                     select new ItemEditInformation
                                                                     {
                                                                         ItemId                     = valuesData.ItemId,
                                                                         ItemName                   = valuesData.ItemName,
                                                                         IsCreateMaskAllowed        = maskData.IsCreateAllowed,
                                                                         IsCreateValueAllowed       = valuesData.IsCreateAllowed,
                                                                         IsEditMaskAllowed          = maskData.IsEditAllowed,
                                                                         IsEditValueAllowed         = valuesData.IsEditAllowed,
                                                                         IsDeleteMaskAllowed        = maskData.IsDeleteAllowed,
                                                                         IsDeleteValueAllowed       = valuesData.IsDeleteAllowed,
                                                                         IsViewMaskAllowed          = maskData.IsViewAllowed,
                                                                         IsViewValueAllowed         = valuesData.IsViewAllowed
                                                                     }).ToList();
            }
        }

        //Send composed item permission collection with mask and values.
        return editRolePermissions;
    }

    #endregion

    #region GetResponseCode

    /// <summary>
    /// Formats the given ResponseCode Enum type to its corresponding integer value.
    /// </summary>
    /// <param name="responseCode">ResponseCode Enum type.</param>
    /// <returns>Integer value for the provided ResponseCode Enum type.</returns>
    private int GetResponseCode(ResponseCode responseCode)
    {
        return int.Parse(Enum.Format(typeof(ResponseCode), Enum.Parse(typeof(ResponseCode), responseCode.ToString()), "d"));
    }

    #endregion

    #region GetResponseCodeDescription

    /// <summary>
    /// Gets the string representation of provided integer response code.
    /// </summary>
    /// <param name="responseCode">Response Code.</param>
    /// <returns>String representation of provided response code.</returns>
    private string GetResponseCodeDescription(int responseCode)
    {
        string responseCodeString = string.Format("Description{0}", responseCode);

        return responseCodeString = typeof(ResponseCodeMapping).GetField(responseCodeString).GetValue(this).ToString();
    }

    #endregion
}

#endregion

#region ResponseCode

/// <summary>
/// This Enum will define the various Transaction response codes supported by the FocusConnect.
/// </summary>
public enum ResponseCode
{
    SUCCESS                                     = 0,
    INSUFFICIENT_PARAMETERS                     = 1,
    INVALID_USER_PROFILE                        = 2,
    INVALID_LOGIN_CREDENTIALS                   = 3,
    INVALID_APPLICATION_ID                      = 4,
    INCOMPELETE_USER_PROFILE                    = 5,
    NEED_GATEWAY_CREDENTIALS                    = 6,
    GATEWAY_NOT_SUPPORTED                       = 7,
    INVALID_REQUEST_TYPE                        = 8,
    INVALID_OPERATION_MODE                      = 9,
    INVALID_LOGIN_SESSION                       = 10,
    INVALID_TRANSACTION_TYPE                    = 11,
    UNREGISTERED_USER                           = 12,
    POS_LICENSE_NOT_ASSIGNED                    = 13,
    PLEASE_CHANGE_YOUR_PASSWORD                 = 14,
    INVALID_TEMPORARY_PASSWORD                  = 15,
    TRANSACTION_NOT_SUPPORTED                   = 16,
    INCORRECT_USER_NAME                         = 17,
    INVALID_GATEWAY_TRANSACTION_ID              = 18,
    INVALID_CREDIT_CARD_EXPIRY_DATE             = 19,
    INVALID_BATCH_ID                            = 20,
    ALREADY_PROCESSED_TRANSACTION               = 21,
    NO_ACTIVE_BATCH                             = 22,
    UNABLE_TO_UPLOAD_SIGNATURE                  = 23,
    USER_IS_LOCKED_DYNAMIC_TIME                 = 24,
    PASSWORD_MINIMUM_LENGTH_CRITERIA_MISMATCH   = 25,
    WEAK_PASSWORD                               = 26,
    REPEATED_PASSWORD                           = 27,
    DELETED_INACTIVE_USER                       = 28,
    FORCE_CHANGE_PASSWORD                       = 29,
    FORCE_CHANGE_PASSWORD_TIMELINE              = 30,
    INVALID_PASSWORD_ATTEMPTS_LEFT              = 31,
    SESSION_EXPIRED                             = 32,
    USER_IS_LOCKED                              = 33,
    CANNOT_ADJUST_AMOUNT_FIELD                  = 34,
    NO_UNIQUE_GATEWAY                           = 35,
    TRANSACTION_ALREADY_VOIDED                  = 36,
    TRANSACTION_ALREADY_CAPTURED                = 37,
    UNAUTHORIZED_USER                           = 38,
    GATEWAY_MISMATCH                            = 39,
    INVALID_CURRENCY_FORMAT                     = 40,
    INVALID_FORMAT_ID                           = 41,
    INVALID_KEY_SERIAL_NUMBER                   = 42,
    INVALID_NAME_FIELD                          = 43,
    INVALID_TRACK_DATA                          = 44,
    INVALID_ENCRYPTED_DATAGRAM                  = 45,
    INVALID_TRACK_METADATA                      = 46,
    INVALID_PERIPHERAL_ID                       = 47,
    DUPLICATE_USER_NAME                         = 48,
    APP_AUTH_FAILED                             = 49,
    UNAUTHORIZED_ACTION                         = 50,
    WEAK_USERNAME                               = 51,
    TRANSACTION_ALREADY_REFUNDED                = 52,
    PASSWORD_LENGTH                             = 53,
    DATABASE_ERROR                              = 101,
    GATEWAY_TIME_OUT                            = 102,
    CHANAKYA_ERROR                              = 103,
    GATEWAY_DECLINED                            = 201,
    GATEWAY_ERROR                               = 202    
}

#endregion

#region ResponseCodeMapping

/// <summary>
/// This class will map the various Transaction response code desriptions supported by the FocusConnect with its code.
/// </summary>
public class ResponseCodeMapping
{
    public const string Description0            = "Approved";
    public const string Description1            = "Insufficient Parameters";
    public const string Description2            = "Invalid User Profile";
    public const string Description3            = "Invalid Login Credentials";
    public const string Description4            = "Invalid Application ID";
    public const string Description5            = "Incomplete User Profile";
    public const string Description6            = "Need Gateway Credentials";
    public const string Description7            = "Cannot find the Gateway setting";
    public const string Description8            = "Invalid Request Type";
    public const string Description9            = "Invalid Operation Mode";
    public const string Description10           = "Invalid Login Session";
    public const string Description11           = "Invalid Transaction Type";
    public const string Description12           = "Unregistered User";
    public const string Description13           = "POS License Not Assigned";
    public const string Description14           = "Please change your temporary password";
    public const string Description15           = "Invalid Temporary Password";
    public const string Description16           = "Transaction Not Supported";
    public const string Description17           = "Incorrect User Name";
    public const string Description18           = "Transaction Not Supported/Invalid Gateway Transaction ID";
    public const string Description19           = "Invalid Credit Card Expiry Date";
    public const string Description20           = "Batch of the original transaction is already settled";
    public const string Description21           = "This transaction has been already processed";
    public const string Description22           = "No Active Batch";
    public const string Description23           = "Unable to upload Signature and Location information";
    public const string Description24           = "User is locked. Please try to login after {0} minute(s) or contact your administrator";
    public const string Description25           = "The new password does not meet minimum length criteria";
    public const string Description26           = "Password should be mix of alphanumeric and atleast one special character except < >";
    public const string Description27           = "The new password cannot be same as your last {0} password(s)";
    public const string Description28           = "The user is deleted due to prolonged inactivity";
    public const string Description29           = "Your password needs to be changed";
    public const string Description30           = "You will be forced to change password in next {0} day(s)";
    public const string Description31           = "Invalid Password. You have {0} more attempt(s)";
    public const string Description32           = "Session Expired. Please login again";
    public const string Description33           = "User is locked. Please contact your administrator";
    public const string Description34           = "Cannot adjust amount field. Adjust allowed only on tips";
    public const string Description35           = "Could not find unique gateway for the given transaction id";
    public const string Description36           = "Transaction not supported. The Original transaction is already Voided";
    public const string Description37           = "Transaction not supported. The Original transaction is already Captured";
    public const string Description38           = "Unauthorized User";
    public const string Description39           = "Gateway mismatch";
    public const string Description40           = "Invalid currency format";
    public const string Description41           = "Invalid Format ID";
    public const string Description42           = "Invalid Key Serial Number";
    public const string Description43           = "Invalid Name Field";
    public const string Description44           = "Invalid Track Data";
    public const string Description45           = "Invalid Encrypted Datagram";
    public const string Description46           = "Invalid Track Metadata";
    public const string Description47           = "Invalid Peripheral ID";
    public const string Description48           = "Username already exists";
    public const string Description49           = "Application Authentication failed";
    public const string Description50           = "Unauthorized Action";
    public const string Description51           = "Username should not be less than {0} character(s)";
    public const string Description52           = "Transaction not supported. The Original transaction is already Refunded";
    public const string Description53           = "Password should be minimum of 8 characters";
    public const string Description101          = "Database Error";
    public const string Description102          = "Gateway Timeout";
    public const string Description103          = "System Error";
    public const string Description201          = "Gateway Declined";
    public const string Description202          = "Gateway Error";    
}

#endregion
