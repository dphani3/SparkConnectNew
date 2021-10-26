#region Copyright
/* Copyright 2010 (c), ThoughtFocus.
  
  All rights are reserved.  Reproduction or transmission in whole or in part,
  in any form or by any means, electronic, mechanical or otherwise,
  is prohibited without the prior written consent of the copyright owner.
  Author(s): Devaraj
  File Name: BLEmailLogic.cs
  Description: This class represents the Email Logic Business Object
  Date Created : 08-April-2010
  Revision History:
  */
#endregion

#region Namspace declaration

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using TF.FocusPay.EmailServer.EmailDataLayer;
using TF.FocusPay.EmailServer.LogManager;

#endregion

namespace TF.FocusPay.EmailServer.EmailBusinessLogic
{
    public class BLEmailLogic
    {
        #region Private Constants

        private string FPEmailServerNameSpace = MethodBase.GetCurrentMethod().DeclaringType.ToString();

        private const string RETRY_THRESHOLD        = "RetryThreshold";

        private const string ERROR                  = "Error";
        private const string CLOSED                 = "Closed";
        private const string RETRY                  = "Retry";
        private const string WORK_IN_PROGRESS       = "WorkInProgress";
        private const string READY_TO_POST          = "ReadyToPost";
        private const string RETRY_FAILURE          = "RetryFailure";   

        private const int DEFAULT_INT               = -1;
        //Field name constants

        private const string QUEUEID                = "QueueID";
        private const string LASTRETRY              = "LastRetry";
        private const string REPORTSTATUSID         = "ReportStatusID";
        private const string COMPANYID              = "CompanyID";
        private const string MERCHANTID             = "MerchantID";
        //Message constants for Logging
        private const string PROCESSING_STARTS      = "Processing starts for QueueID :";
        private const string PRIORITY               = "   Priority :";
        private const string COMPANY_ID             = "   Company ID :";
        private const string MERCHANT_ID            = "   Merchant ID :";
        private const string QUE_ID                 = "  QueueID :";
        private const string SETTING_ERROR          = "Setting the status as Error for QueueID :";
        private const string SETTING_RETRY          = "Setting the status as Retry for QueueID :";
        private const string SETTING_RETRY_FAILURE  = "Setting the status as Retry Failure QueueID :";
        private const string SETTING_CLOSED         = "Setting the status as Closed for QueueID :";
        private const string PROCESSING_ENDS        = "Processing ends for QueueID :";
        private const string SERVICE_STOPPED        = "Service has been stopped";
        private const string EMAIL_TEMPLATE_FOLDER  = "EmailTemplates";
        private const string BACKSLASH              = "\\";
        private const string NO_RECORD_TO_PROCESS   = "There are no records to process at the moment";

        const int E_RECEIPT_EMAIL_TYPE              = 17;
        const int COPY_OF_E_RECEIPT_EMAIL_TYPE      = 18;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public BLEmailLogic(int priorityNo)
        {
            PriorityNumber = priorityNo;
            PriorityQueueReader();
        }

        #endregion

        #region Public Variables

        public Thread emailSenderThread;

        #endregion

        #region Private Variables

        private int PriorityNumber;

        private string EmailSenderNameSpace = MethodBase.GetCurrentMethod().DeclaringType.ToString();

        private EmailDataAccess ObjDataAccessGetTransFromQueue = new EmailDataAccess();

        private DataSet esPriorityQueue = null;
        private DataSet esPriorityLookup = null;

        private int declined;
        private int closed;
        private int retry;
        private int workInProgress;
        private int readyToPost;
        private int retryFailure;

        private long queueID;

        private long companyID          = 0;
        private int retStatus           = 0;        
        string companyName              = string.Empty;
        string merchantName             = string.Empty;
        string attendantName            = string.Empty;
        int emailTypeID                 = 0;
        string fromAddress              = string.Empty;
        string toAddress                = string.Empty;
        string ccList                   = string.Empty;
        string messageBodyParams        = string.Empty;
        byte[] receiptStream            = null;
        string EmailTemplateName        = string.Empty;
        private bool lastRetryFlag;
        string[] param                  = null;

        #endregion

        #region Private Properties

        /// <summary>
        /// Property for getting the Error Code
        /// </summary>
        private int Error
        {
            get
            {
                try
                {
                    if (declined == DEFAULT_INT || declined == 0)
                    {
                        declined = Convert.ToInt32(ConfigurationManager.AppSettings[ERROR].ToString());

                    }
                }

                catch (System.FormatException ex)
                {
                    ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return declined;
            }
        }

        /// <summary>
        /// Property for getting the Closed Code
        /// </summary>
        private int Closed
        {
            get
            {
                try
                {
                    if (closed == DEFAULT_INT || closed == 0)
                    {
                        closed = Convert.ToInt32(ConfigurationManager.AppSettings[CLOSED].ToString());

                    }
                }

                catch (System.FormatException ex)
                {
                    ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return closed;

            }
        }

        /// <summary>
        /// Property for getting the Retry Code
        /// </summary>
        private int Retry
        {
            get
            {
                try
                {
                    if (retry == DEFAULT_INT || retry == 0)
                    {
                        retry = Convert.ToInt32(ConfigurationManager.AppSettings[RETRY].ToString());
                    }
                }

                catch (System.FormatException ex)
                {
                    ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return retry;
            }

        }

        /// <summary>
        /// Property for getting the Work In Progress Code
        /// </summary>
        private int WorkInProgress
        {
            get
            {
                try
                {
                    if (workInProgress == DEFAULT_INT || workInProgress == 0)
                    {
                        workInProgress = Convert.ToInt32(ConfigurationManager.AppSettings[WORK_IN_PROGRESS].ToString());

                    }
                }

                catch (System.FormatException ex)
                {
                    ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return workInProgress;
            }


        }

        /// <summary>
        /// Property for getting the Ready To Post Code
        /// </summary>
        private int ReadyToPost
        {
            get
            {
                try
                {
                    if (readyToPost == DEFAULT_INT || readyToPost == 0)
                    {
                        readyToPost = Convert.ToInt32(ConfigurationManager.AppSettings[READY_TO_POST].ToString());

                    }
                }

                catch (System.FormatException ex)
                {
                    ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return readyToPost;
            }
        }

        /// <summary>
        /// Property for getting the Retry Failure Code
        /// </summary>
        private int RetryFailure
        {
            get
            {
                try
                {
                    if (retryFailure == DEFAULT_INT || retryFailure == 0)
                    {
                        retryFailure = Convert.ToInt32(ConfigurationManager.AppSettings[RETRY_FAILURE].ToString());

                    }
                }

                catch (System.FormatException ex)
                {
                    ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return retryFailure;
            }
        }       

        #endregion

        #region Priority Queue Reader

        /// <summary>
        /// 
        /// </summary>
        public void PriorityQueueReader()
        {
            if (esPriorityQueue == null)
            {
                esPriorityQueue = new DataSet();
            }

            try
            {
                try
                {
                    queueID = 0;
                    companyID = 0;

                    companyName = string.Empty;
                    merchantName = string.Empty;
                    attendantName = string.Empty;
                    emailTypeID = 0;
                    fromAddress = string.Empty;
                    toAddress = string.Empty;
                    ccList = string.Empty;
                    messageBodyParams = string.Empty;


                    string EmailTemplatePath = string.Empty;
                    string platformName = string.Empty;
                    string brandName = string.Empty;
                    esPriorityQueue = GetPriorityQueueData(PriorityNumber);

                    if (esPriorityQueue != null && esPriorityQueue.Tables.Count > 0 && esPriorityQueue.Tables[0].Rows.Count > 0)
                    {                        
                        queueID = Convert.ToInt64(esPriorityQueue.Tables[0].Rows[0][Constants.TBL_COLUMN_QUEUE_ID].ToString());
                        companyID = Convert.ToInt64(esPriorityQueue.Tables[0].Rows[0][Constants.TBL_COLUMN_COMPANY_ID]);
                        lastRetryFlag = Convert.ToBoolean(esPriorityQueue.Tables[0].Rows[0][LASTRETRY]);
                        messageBodyParams = Convert.ToString(esPriorityQueue.Tables[0].Rows[0][Constants.TBL_COLUMN_MSG_BODY_PARAMS].ToString());
                        emailTypeID = Convert.ToInt32(esPriorityQueue.Tables[0].Rows[0][Constants.TBL_COLUMN_EMAIL_TYPE_ID].ToString());
                        //fromAddress = Convert.ToString(esPriorityQueue.Tables[0].Rows[0][Constants.TBL_COLUMN_FROM_ADDRESS].ToString());
                        toAddress = Convert.ToString(esPriorityQueue.Tables[0].Rows[0][Constants.TBL_COLUMN_TO_ADDRESS].ToString());
                        ccList = Convert.ToString(esPriorityQueue.Tables[0].Rows[0][Constants.TBL_COLUMN_CC_LIST].ToString());
                        receiptStream = (esPriorityQueue.Tables[0].Rows[0][Constants.TBL_COLUMN_RECEIPT_STREAM] != DBNull.Value) ? (byte[])esPriorityQueue.Tables[0].Rows[0][Constants.TBL_COLUMN_RECEIPT_STREAM] : null;

                        MessageLogger.Write(PROCESSING_STARTS + queueID + PRIORITY + PriorityNumber +
                            COMPANY_ID + companyID, CategoryType.Information, null, FPEmailServerNameSpace);                        

                        EmailTemplateName = GetEmailTemplateNameByID(emailTypeID);
                        EmailTemplatePath = LoadEmailTemplate(EmailTemplateName);
                        FileStream fileStream = new FileStream(EmailTemplatePath, FileMode.Open, FileAccess.Read);
                        DataSet emailTemplateData = new DataSet();
                        emailTemplateData.ReadXml(fileStream);
                        fileStream.Close();
                        string messageBody = Convert.ToString(emailTemplateData.Tables[0].Rows[0][Constants.TBL_COLUMN_MESSAGE]);
                        string subjectLine = Convert.ToString(emailTemplateData.Tables[0].Rows[0][Constants.TBL_COLUMN_SUBJECT]);
                        param = ParseMessageBodyParams(messageBodyParams);
                        switch (emailTypeID)
                        {
                            case 1:
                                messageBody = SubstituteValuesForCompanyCreation(param, messageBody);
                                companyName = param[2];
                                brandName = param[3];
                                subjectLine = "Welcome to " + brandName + " : " + companyName;
                                ccList = param[4];
                                fromAddress = param[8];
                                break;
                            case 2:
                                messageBody = SubstituteValuesForMerchantCreation(param, messageBody);
                                merchantName = param[4];
                                brandName = param[5]; 
                                subjectLine = "Welcome to " + brandName + " : " + merchantName;
                                fromAddress = param[10];
                                break;
                            case 3:
                                messageBody = SubstituteValuesForNewUserLoginCredentials(param, messageBody);
                                brandName = param[5];
                                //EMAIL_ISSUE subjectLine = "Your " + brandName + " User Credentials";
                                subjectLine = "Welcome to " + " " + brandName + "!";//EMAIL_ISSUE
                                fromAddress = param[10];
                                break;
                            case 4:
                                messageBody = SubstituteValuesForAttendantCreation(param, messageBody);
                                brandName = param[6];
                                subjectLine = "Welcome to " + brandName + " : " + param[0];
                                fromAddress = param[11];
                                break;
                            case 5:
                                messageBody = SubstituteValuesForPOSLicenseToCompany(param, messageBody);
                                brandName = param[3];
                                subjectLine = "Your " + brandName + " POS Licenses: Company Assignment";
                                ccList = param[4];
                                ccList = ccList + ";" + param[6];
                                fromAddress = param[8];
                                break;
                            case 6:
                                messageBody = SubstituteValuesForPOSLicenseToMerchant(param, messageBody);
                                brandName = param[5];
                                subjectLine = "Your " + brandName + " POS Licenses: Merchant Assignment";
                                ccList = param[3];
                                fromAddress = param[10];
                                break;
                            case 7:
                                messageBody = SubstituteValuesForAssignPOS(param, messageBody);
                                brandName = param[2];
                                subjectLine = brandName + " POS Assignment: " + param[0];
                                fromAddress = param[7];
                                break;
                            case 8:
                                messageBody = SubstituteValuesForPOSDeAssign(param, messageBody);
                                brandName = param[4];
                                subjectLine = brandName + " POS License Deassigned";
                                fromAddress = param[9];
                                break;
                            case 10:                                
                                messageBody = SubstituteValuesForEReceipt(param, messageBody);
                                brandName = param[20];
                                subjectLine = brandName + " eReceipt:" + " " + param[4] + "-" + param[5];
                                fromAddress = param[21];
                                break;
                            case 11:
                                messageBody = SubstituteValuesForNewResetPassword(param, messageBody);
                                brandName = param[6];
                                //EMAIL_ISSUE subjectLine = brandName + " Account Change Notification";
                                subjectLine = brandName + " " + " - Account Change Request";//EMAIL_ISSUE
                                fromAddress = param[11];
                                break;
                            case 14:
                                messageBody = SubstituteValuesForProspectInformation(param, messageBody);
                                subjectLine = "New Prospect: " + param[0].Trim();
                                toAddress = ConfigurationManager.AppSettings["PlatformSalesEmail"].ToString();
                                break;
                            case 15:
                                messageBody = SubstituteValuesForYouAreAdminNotification(param, messageBody);
                                string entityName = param[2];
                                if (String.IsNullOrEmpty(entityName))
                                {
                                    subjectLine = "Designated Administrator for";
                                }
                                else
                                {
                                    subjectLine = "Designated Administrator for " + " " + entityName;
                                }

                                fromAddress = param[9];
                                break;
                            case 16:
                                messageBody = SubstituteValuesForUpdatedAttendantCredentials(param, messageBody);
                                subjectLine = "Update on your Attendant Credentials";
                                fromAddress = param[9];
                                break;
                            case 17:                                
                                messageBody = SubstituteValuesForTransactionEReceipt(param, messageBody);
                                brandName = param[12];
                                //EMAIL_ISSUE subjectLine = brandName + " eReceipt:" + param[2] + "-" + param[5];
                                merchantName = param[1]; //EMAIL_ISSUE
                                subjectLine = merchantName + " " + " - eReceipt-" + param[2] + "-" + param[5];//EMAIL_ISSUE
                                fromAddress = param[18];
                                break;
                            case 18:                                
                                messageBody = SubstituteValuesForCopyOfEReceipt(param, messageBody);
                                //EMAIL_ISSUE subjectLine = "Copy of eReceipt:" + param[0] + "-" + param[1];
                                merchantName = param[3];
                                subjectLine = "eReceipt from " + " " + merchantName;//EMAIL_ISSUE
                                fromAddress = param[12];
                                break;
                            case 19:
                                messageBody = SubstituteValuesForUpdatedUserID(param, messageBody);
                                brandName = param[3];
                                //EMAIL_ISSUE subjectLine = "Your " + brandName + " User ID change Notification";
                                subjectLine = brandName + " " + " - Account Change Request";
                                fromAddress = param[8];
                                break;
                            case 20:
                                messageBody = SubstituteValuesForUpdatedUserIDMigration(param, messageBody);
                                brandName = param[3];
                                //EMAIL_ISSUE subjectLine = "Your " + brandName + " User ID change Notification";
                                subjectLine = brandName + " " + " - Account Change Request";//EMAIL_ISSUE
                                fromAddress = param[8];
                                break;
                            default:
                                break;
                        }

                        if (!SendwebMail(messageBody, toAddress, fromAddress, subjectLine, ccList))
                        {
                            MessageLogger.Write(SETTING_RETRY + queueID + PRIORITY + PriorityNumber, CategoryType.Information, null, FPEmailServerNameSpace);
                            UpdateQueueRowStatus(queueID, PriorityNumber, Retry, lastRetryFlag);
                        }
                        else
                        {
                            MessageLogger.Write(SETTING_CLOSED + queueID + PRIORITY + PriorityNumber, CategoryType.Information, null, FPEmailServerNameSpace);
                            UpdateQueueRowStatus(queueID, PriorityNumber, Closed, lastRetryFlag);
                        }

                        esPriorityQueue.Clear();
                    }
                    else
                    {
                        esPriorityQueue.Clear();
                    }
                }

                catch (WebException ex)
                {
                    if (lastRetryFlag)
                    {
                        ExceptionLogger.PublishException(ex, "Error", ListenerType.Email);
                    }

                    ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }
                catch (IOException ex)
                {
                    ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }
                catch (SocketException ex)
                {
                    ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }
                catch (System.StackOverflowException ex)
                {
                    ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }
                catch (InvalidOperationException ex)
                {
                    ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }
                catch (NullReferenceException ex)
                {
                    ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }
                catch (SqlException ex)
                {
                    if (lastRetryFlag)
                    {
                        ExceptionLogger.PublishException(ex, "Error", ListenerType.Email);
                    }

                    ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }
                catch (Exception ex)
                {
                    if (lastRetryFlag)
                    {
                        ExceptionLogger.PublishException(ex, "Error", ListenerType.Email);
                    }

                    ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
            }

        }

        #endregion

        #region Getting the rows from queue table

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Priority"></param>
        /// <returns></returns>
        private DataSet GetPriorityQueueData(int Priority)
        {
            try
            {
                esPriorityQueue.Clear();
                esPriorityQueue = ObjDataAccessGetTransFromQueue.GetOrderFromQueue(Priority);
            }
            catch (Exception)
            {
                throw;
            }

            return esPriorityQueue;
        }

        #endregion

        #region Getting the rows from Priority Lookup

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Priority"></param>
        /// <returns></returns>
        public DataSet GetPriority()
        {
            if (esPriorityLookup == null)
            {
                esPriorityLookup = new DataSet();
            }

            esPriorityLookup = ObjDataAccessGetTransFromQueue.GetPriorities();

            return esPriorityLookup;
        }

        #endregion

        #region Updating the Queue row status

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueID"></param>
        /// <param name="Priority"></param>
        /// <param name="rowStatusID"></param>
        /// <param name="lastRetryFlag"></param>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        private int UpdateQueueRowStatus(long queueID, int priority, int status, Boolean lastRetryFlag)
        {
            try
            {
                retStatus = 0;

                retStatus = ObjDataAccessGetTransFromQueue.UpdateOrderStatus(queueID, priority, status, lastRetryFlag);

            }
            catch (Exception ex)
            {
                ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
            }

            return retStatus;
        }

        #endregion

        #region Get Email Template Name By ID

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailTemplateID"></param>
        /// <returns></returns>
        private string GetEmailTemplateNameByID(int emailTemplateID)
        {
            switch (emailTemplateID)
            {
                case 1:
                    return "CompanyCreationTemplate.xml";
                case 2:
                    return "MerchantCreationTemplate.xml";
                case 3:
                    return "NewUserLoginCredentialsTemplate.xml";
                case 4:
                    return "AttendantCreationTemplate.xml";
                case 5:
                    return "POSLicenseToCompanyTemplate.xml";
                case 6:
                    return "POSLicenseToMerchantTemplate.xml";
                case 7:
                    return "AssignPOSToAttendants.xml";
                case 8:
                    return "POSDeAssignTemplate.xml";                
                case 10:
                    return "EmailReceiptsTemplate.xml";
                case 11:
                    return "ResetPasswordTemplate_New.xml";                
                case 14:
                    return "ProspectInformation.xml";
                case 15:
                    return "YouAreAdminNotification.xml";
                case 16:
                    return "UpdatedAttendantCredentials.xml";
                case 17:
                    return "TransactionEmailReceiptTemplate.xml";
                case 18:
                    return "CopyOfEmailReceiptTemplate.xml";
                case 19:
                    return "UpdatedUserIDTemplate.xml";
                case 20:
                    return "UpdatedUserIDMigrationTemplate.xml";
                default:
                    throw new Exception("Email Type ID: " + emailTemplateID + " does not exist");
            }
        }

        #endregion
       
        #region Parse Message Body Params

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageBodyParams"></param>
        /// <returns></returns>
        private string[] ParseMessageBodyParams(string messageBodyParams)
        {
            char[] seperator = {'|'};
            string[] msgArr = messageBodyParams.Split(seperator);

            return msgArr;
        }

        #endregion        

        #region Substitute Values For Assign POS

        /// <summary>
        /// This method is used to substitute dynamic values in the email message body and send email
        /// </summary>
        /// <param name="msgArr"></param>
        /// <param name="message"></param>
        private string SubstituteValuesForAssignPOS(string[] msgArr, string message)
        {
            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[0]);            
            //message = message.Replace(Constants.EMAIL_TEMPLATE_MERCHANT_NAME, msgArr[1]);

            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[2]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[3]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[4]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[5]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[6]);

            return message;
        }

        #endregion

        #region Substitute Values For Company Creation

        private string SubstituteValuesForCompanyCreation(string[] msgArr, string message)
        {           
            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[0]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_URL, msgArr[1]);

            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[3]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[4]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[5]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[6]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[7]);            

            return message;
        }

        #endregion

        #region Substitute Values For Merchant Creation

        private string SubstituteValuesForMerchantCreation(string[] msgArr, string message)
        {
            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[0]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_URL, msgArr[1]);
            //message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_NAME, msgArr[2]);           

            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[5]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[6]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[7]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[8]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[9]);      

            return message;
        }

        #endregion

        #region Substitute Values For Company Login Credebtials

        private string SubstituteValuesForUserLoginCredentials(string[] msgArr, string message)
        {
            string minimumPasswordLength = ConfigurationManager.AppSettings["MinimumPasswordLength"].ToString();
            string invalidPasswordAttempts = ConfigurationManager.AppSettings["InvalidPasswordAttempts"].ToString();
            string samePasswordUsage = ConfigurationManager.AppSettings["SamePasswordUsage"].ToString();
            string forceChangePassword = ConfigurationManager.AppSettings["ForceChangePassword"].ToString();
            string lockOutTimeDuration = ConfigurationManager.AppSettings["LockOutTimeDuration"].ToString();
            string idleTimeDuration = ConfigurationManager.AppSettings["IdleTimeDuration"].ToString();
            string userInActiveDays = ConfigurationManager.AppSettings["UserInActiveDays"].ToString();

            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[0]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_URL, msgArr[1]);
            //message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_NAME, msgArr[2]);
            message = message.Replace(Constants.EMAIL_PLATFORM_USER_NAME_OR_USER_CODE, msgArr[3]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_PASSWORD, msgArr[4]);

            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[5]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[6]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[7]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[8]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[9]);   

            message = message.Replace(Constants.EMAIL_MIN_PASSWORD_LENGTH, minimumPasswordLength);
            message = message.Replace(Constants.EMAIL_INVALID_PASSWORD_ATTEMPTS, invalidPasswordAttempts);
            message = message.Replace(Constants.EMAIL_SAME_PASSWORD_USAGE, samePasswordUsage);
            message = message.Replace(Constants.EMAIL_FORCE_CHANGE_PASSWORD, forceChangePassword);
            message = message.Replace(Constants.EMAIL_LOCKOUT_TIME, lockOutTimeDuration);
            message = message.Replace(Constants.EMAIL_SESSION_TIMEOUT, idleTimeDuration);
            message = message.Replace(Constants.EMAIL_USER_INACTIVE_DAYS, userInActiveDays);

            return message;
        }

        #endregion       

        #region Substitute Values For User Login Credebtials

        private string SubstituteValuesForNewUserLoginCredentials(string[] msgArr, string message)
        {
            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[0]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_URL, msgArr[1]);
            //message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_NAME, msgArr[2]);
            message = message.Replace(Constants.EMAIL_PLATFORM_USER_NAME_OR_USER_CODE, msgArr[3]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_PASSWORD, msgArr[4]);

            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[5]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[6]);//EMAIL_ISSUE
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[7]);//EMAIL_ISSUE
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[8]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[9]);            

            return message;
        }

        #endregion       

        #region Substitute Values For POS License To Company

        private string SubstituteValuesForPOSLicenseToCompany(string[] msgArr, string message)
        {
            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[0]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TOTAL_POS_LICENSE_AT_INSTANCE, msgArr[1]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TOTAL, msgArr[2]);

            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[3]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[4]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[5]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[6]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[7]); 

            return message;
        }

        #endregion

        #region Substitute Values For POS License To Merchant

        private string SubstituteValuesForPOSLicenseToMerchant(string[] msgArr, string message)
        {            
            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[0]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TOTAL_POS_LICENSE_AT_INSTANCE, msgArr[1]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TOTAL, msgArr[2]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_ADMIN_EMAIL_ID, msgArr[3]);
            //message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_NAME, msgArr[4]);

            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[5]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[6]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[7]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[8]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[9]); 

            return message;
        }

        #endregion

        #region Substitute Values For POS DeAssign

        private string SubstituteValuesForPOSDeAssign(string[] msgArr, string message)
        {           
            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[0]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_ATTENDANT_NAME, msgArr[1]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TOTAL, msgArr[2]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_ADMIN_EMAIL_ID, msgArr[3]);

            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[4]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[5]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[6]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[7]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[8]); 

            return message;
        }

        #endregion

        #region Substitute Values For E - Receipt

        private string SubstituteValuesForEReceipt (string[] msgArr, string message)
        {
            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[0]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_ATTENDANT_NAME, msgArr[1]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_DATE, msgArr[2]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_TIME, msgArr[3]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_TYPE, msgArr[4]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_TOTAL_AMOUNT, msgArr[5]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_CHANAKYA_ID, msgArr[6]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_AMOUNT, msgArr[7]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_MASKED_CARD_NO, msgArr[8]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_RECEIPT_HEADER1, msgArr[10]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_RECEIPT_HEADER2, msgArr[11]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_RECEIPT_HEADER3, msgArr[12]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_RECEIPT_FOOTER1, msgArr[13]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_RECEIPT_FOOTER2, msgArr[14]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_RECEIPT_FOOTER3, msgArr[15]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_INVOICE_NUM, msgArr[16]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_MERCHANT_NAME, msgArr[17]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_STATUS, msgArr[19]);         //EMAIL_ISSUE moved to before if condition
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_NOTES, msgArr[20]); //Added by Nazreen
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_CONVENIENCE_FEE, msgArr[21]); //Added by Nazreen
            message = message.Replace(Constants.EMAIL_TEMPLATE_INVOIVE_NUMBER, msgArr[22]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_ORDER_NUMBER, msgArr[23]);

            if(!String.IsNullOrEmpty(msgArr[18]))
            {
                //if (!double.Parse(msgArr[18]).Equals(0))
                //{
                message = message.Insert(message.IndexOf("</tr>", message.LastIndexOf("left")) + 5, @"<tr><td align=/""left/""><b>&nbsp;Tip Amount</b></td><td align=/""left/"">: @TIP_AMOUNT@</td></tr>");
                message = message.Replace(Constants.EMAIL_TEMPLATE_TIP_AMOUNT, msgArr[18]);
                //}               
            }



            return message;
        }

        #endregion

        #region Substitute Values For Reset Password

        private string SubstituteValuesForResetPassword(string[] msgArr, string message)
        {            
            string minimumPasswordLength = ConfigurationManager.AppSettings["MinimumPasswordLength"].ToString();
            string invalidPasswordAttempts = ConfigurationManager.AppSettings["InvalidPasswordAttempts"].ToString();
            string samePasswordUsage = ConfigurationManager.AppSettings["SamePasswordUsage"].ToString();
            string forceChangePassword = ConfigurationManager.AppSettings["ForceChangePassword"].ToString();
            string lockOutTimeDuration = ConfigurationManager.AppSettings["LockOutTimeDuration"].ToString();
            string idleTimeDuration = ConfigurationManager.AppSettings["IdleTimeDuration"].ToString();
            string userInActiveDays = ConfigurationManager.AppSettings["UserInActiveDays"].ToString();

            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[0]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_URL, msgArr[1]);
            message = message.Replace(Constants.EMAIL_PLATFORM_USER_NAME_OR_USER_CODE, msgArr[2]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_PASSWORD, msgArr[3]);
            //message = message.Replace(Constants.EMAIL_TEMPLATE_ADMIN_EMAIL, msgArr[4]);

            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[6]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[7]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[8]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[9]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[10]); 

            message = message.Replace(Constants.EMAIL_MIN_PASSWORD_LENGTH, minimumPasswordLength);
            message = message.Replace(Constants.EMAIL_INVALID_PASSWORD_ATTEMPTS, invalidPasswordAttempts);
            message = message.Replace(Constants.EMAIL_SAME_PASSWORD_USAGE, samePasswordUsage);
            message = message.Replace(Constants.EMAIL_FORCE_CHANGE_PASSWORD, forceChangePassword);
            message = message.Replace(Constants.EMAIL_LOCKOUT_TIME, lockOutTimeDuration);
            message = message.Replace(Constants.EMAIL_SESSION_TIMEOUT, idleTimeDuration);
            message = message.Replace(Constants.EMAIL_USER_INACTIVE_DAYS, userInActiveDays);
            message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_URL, msgArr[1]);//EMAIL_ISSUE

            return message;
        }

        #endregion        

        #region Substitute Values For New Reset Password

        private string SubstituteValuesForNewResetPassword(string[] msgArr, string message)
        {
            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[0]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_URL, msgArr[1]);
            message = message.Replace(Constants.EMAIL_PLATFORM_USER_NAME_OR_USER_CODE, msgArr[2]);//EMAIL_ISSUE enabled
            message = message.Replace(Constants.EMAIL_TEMPLATE_PASSWORD, msgArr[3]);
            //message = message.Replace(Constants.EMAIL_TEMPLATE_ADMIN_EMAIL, msgArr[4]);

            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[6]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[7]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[8]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[9]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[10]);          

            return message;
        }

        #endregion        

        #region Substitute Values For Prospect Information

        private string SubstituteValuesForProspectInformation(string[] msgArr, string message)
        {   
            message = message.Replace(Constants.PROSPECT_NAME, msgArr[0]);
            message = message.Replace(Constants.PROSPECT_PHONE, msgArr[1]);
            message = message.Replace(Constants.PROSPECT_EMAIL, msgArr[2]);
            message = message.Replace(Constants.PROSPECT_PLATFORMS, (!String.IsNullOrEmpty(msgArr[3]) ? msgArr[3] : "--"));
            message = message.Replace(Constants.PROSPECT_REMARKS, (!String.IsNullOrEmpty(msgArr[4]) ? msgArr[4] : "--"));
            message = message.Replace(Constants.PROSPECT_LICENSES_NUMBER, (int.Parse(msgArr[5]) != 0) ? msgArr[5] : "--");
            message = message.Replace(Constants.PROSPECT_PREFERRED_PROGRAM, (!String.IsNullOrEmpty(msgArr[6]) ? msgArr[6] : "--"));
            message = message.Replace(Constants.PROSPECT_INDUSTRY, (!String.IsNullOrEmpty(msgArr[7]) ? msgArr[7] : "--"));            

            return message;
        }

        #endregion

        #region Substitute Values For Attendant Creation

        private string SubstituteValuesForAttendantCreation(string[] msgArr, string message)
        {
            string attendantEmailHTML = string.Empty;
            if(string.IsNullOrEmpty(msgArr[2]))
            {
                attendantEmailHTML = string.Empty;
            }
            else
            {
                attendantEmailHTML = "User Name : @EMAIL_ID@ <br/>";
            }
            message = message.Replace(Constants.EMAIL_TEMPLATE_ATTENDANT_EMAIL, attendantEmailHTML);
            message = message.Replace("@EMAIL_ID@", msgArr[2]);

            string attendantCodeHTML = string.Empty;
            if (string.IsNullOrEmpty(msgArr[3]))
            {
                attendantCodeHTML = string.Empty;
            }
            else
            {
                attendantCodeHTML = "User Code : @USER_CODE@ <br/>";
            }
            message = message.Replace(Constants.EMAIL_TEMPLATE_ATTENDANT_CODE, attendantCodeHTML);
            message = message.Replace("@USER_CODE@", msgArr[3]);

            string attendantName = string.Empty;

            if (!string.IsNullOrEmpty(attendantEmailHTML))
            {
                attendantName = msgArr[0];
            }
            else
            {
                attendantName = msgArr[1];
            }

            message = message.Replace(Constants.EMAIL_TEMPLATE_MERCHANT_NAME, attendantName);
            message = message.Replace(Constants.EMAIL_TEMPLATE_ATTENDANT_NAME, msgArr[0]);

            string attendantEmail = string.Empty;
            if (string.IsNullOrEmpty(msgArr[2]))
            {
                if (string.IsNullOrEmpty(msgArr[5]))
                {
                    attendantEmail = string.Empty;
                }
                else
                {
                    attendantEmail = msgArr[5];
                }
            }
            else
            {
                attendantEmail = msgArr[2];
            }

            message = message.Replace(Constants.EMAIL_TEMPLATE_MERCHANT_EMAIL, attendantEmail);
            message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_URL, msgArr[4]);           

            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[6]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[7]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[8]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[9]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[10]); 

            return message;
        }

        #endregion        

        #region Substitute Values For You Are Admin Notification

        private string SubstituteValuesForYouAreAdminNotification(string[] msgArr, string message)
        {   
            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[3]);

            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[4]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[5]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[6]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[7]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[8]);
            if (String.IsNullOrEmpty(msgArr[2]))
            {
                message = message.Replace(Constants.EMAIL_ENTITY_NAME," ");
            }
            else
            {
                message = message.Replace(Constants.EMAIL_ENTITY_NAME, msgArr[2]);
            }
            return message;
        }

        #endregion

        #region Substitute Values For Updated Attendant Credentials

        private string SubstituteValuesForUpdatedAttendantCredentials(string[] msgArr, string message)
        {
            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[0]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_ATTENDANT_EMAIL, msgArr[1]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_ATTENDANT_CODE, msgArr[2]);

            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[4]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[5]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[6]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[7]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[8]); 

            return message;
        }

        #endregion      

        #region Substitute Values For Transaction E - Receipt

        private string SubstituteValuesForTransactionEReceipt (string[] msgArr, string message)
        {
            if(!String.IsNullOrEmpty(msgArr[4]))
            {
                message = message.Replace("Amount                  : @TRANSACTION_AMOUNT@", "Amount                  : @TRANSACTION_AMOUNT@<br/>        Additional Amount       : @TIP_AMOUNT@");
                message = message.Replace(Constants.EMAIL_TEMPLATE_TIP_AMOUNT, msgArr[4]);
            }

            if(!String.IsNullOrEmpty(msgArr[8]))
            {
                message = message.Replace("Date and Time           : @TRANSACTION_DATE@", "Date and Time           : @TRANSACTION_DATE@<br/>        Transaction ID          : @TRANSACTION_ID@");
                message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_ID, msgArr[8]);
            }

            //message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_ID, msgArr[5]);//EMAIL_ISSUE added
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_TOTAL_AMOUNT, msgArr[5]);//EMAIL_ISSUE added
            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[0]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_MERCHANT_NAME, msgArr[1]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_TYPE, msgArr[2]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_AMOUNT, msgArr[3]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_MASKED_CARD_NO, msgArr[6]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_DATE, msgArr[7]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_STATUS, msgArr[9]);
            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[12]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[13]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[14]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[15]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[16]);
            //Added by Nazreen
            message = message.Replace(Constants.EMAIL_TEMPLATE_MERCHANT_EMAIL, msgArr[18]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_MERCHANT_CONTACT_PHONE, msgArr[19]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_NOTES, msgArr[20]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_CONVENIENCE_FEE, msgArr[21]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_INVOIVE_NUMBER, msgArr[22]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_ORDER_NUMBER, msgArr[23]);

            if(String.IsNullOrEmpty(msgArr[10]))
            {
                //EMAIL_ISSUE message = message.Replace(@"<span id='gmaps'>Please <a href='@GMAPS_URL@' target='_blank'>click here</a> to view the location of the transaction.</span><br /><br />", "");
                message = message.Replace(@"<span id='gmaps'>To view a map of the location where the transaction was made, please <a href='@GMAPS_URL@' target='_blank'>click here</a></span><br /><br />", "");
            }
            else
            {
                message = message.Replace(Constants.EMAIL_TEMPLATE_GMAPS_URL, msgArr[10]);
            }

            return message;
        }

        #endregion

        #region Substitute Values For Copy of E - Receipt

        private string SubstituteValuesForCopyOfEReceipt(string[] msgArr, string message)
        {
            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[14]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_TRANS_DATE, msgArr[2]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_MERCHANT_NAME, msgArr[3]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_USER_NAME, msgArr[4]);           
            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[6]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[7]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[8]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[9]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[10]);
            //Added by Nazreen
            message = message.Replace(Constants.EMAIL_TEMPLATE_MERCHANT_EMAIL, msgArr[12]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_MERCHANT_CONTACT_PHONE, msgArr[13]);

            return message;
        }

        #endregion

        #region Substitute Values For Updated User ID

        private string SubstituteValuesForUpdatedUserID(string[] msgArr, string message)
        {           
            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[0]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_URL, msgArr[9]);//EMAIL_ISSUE
            message = message.Replace(Constants.EMAIL_PLATFORM_USER_NAME_OR_USER_CODE, msgArr[2]);
           
            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[3]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[4]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[5]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[6]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[7]);            

            return message;
        }

        #endregion       

        #region Substitute Values For Updated User ID Migration

        private string SubstituteValuesForUpdatedUserIDMigration(string[] msgArr, string message)
        {
            message = message.Replace(Constants.EMAIL_TEMPLATE_FIRST_NAME, msgArr[0]);
            message = message.Replace(Constants.EMAIL_TEMPLATE_COMPANY_URL, msgArr[1]);//EMAIL_ISSUE
            message = message.Replace(Constants.EMAIL_PLATFORM_USER_NAME_OR_USER_CODE, msgArr[2]);

            message = message.Replace(Constants.EMAIL_BRAND_NAME, msgArr[3]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_EMAIL, msgArr[4]);
            message = message.Replace(Constants.EMAIL_COMPANY_SALES_PHONE, msgArr[5]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_EMAIL, msgArr[6]);
            message = message.Replace(Constants.EMAIL_COMPANY_SUPPORT_PHONE, msgArr[7]);

            string minimumUserIDLength = ConfigurationManager.AppSettings["MinimumUIDLength"].ToString();
            message = message.Replace(Constants.UID_CHANGE_MIN_LENGTH, minimumUserIDLength);

            return message;
        }

        #endregion       

        #region Send Mail - Using Web Mail SMTP Server

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageBody"></param>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        private bool SendwebMail(string messageBody, 
                                     string toAddress, 
                                     string fromAddress,
                                     string subjectLine, 
                                     string ccList)
        {
            string fromAddressCredential = Convert.ToString(ConfigurationManager.AppSettings["WebMailEmailID"]).Trim();
            string passwordCredential = Convert.ToString(ConfigurationManager.AppSettings["WebMailEmailPwd"]).Trim();
            string webMailSMTPServer = Convert.ToString(ConfigurationManager.AppSettings["WebMailSMTPServer"]).Trim();
            string webMailSMTPPort = Convert.ToString(ConfigurationManager.AppSettings["WebMailSMTPPort"]).Trim();
            bool isAuthenticationRequired = bool.Parse(ConfigurationManager.AppSettings["IsAuthenticationRequired"].Trim());
            bool isSslRequired = bool.Parse(ConfigurationManager.AppSettings["IsSslRequired"].Trim());
           
            if (String.IsNullOrEmpty(fromAddress))
            {
                fromAddress = fromAddressCredential;
            }
            try
            {
                System.Web.Mail.MailMessage myMail = new System.Web.Mail.MailMessage();
                myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", webMailSMTPServer);
                myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", webMailSMTPPort);
                myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2");

                if (isAuthenticationRequired)
                {
                    myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                    myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", fromAddressCredential);
                    myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", passwordCredential);
                }

                if(isSslRequired)
                    myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
                else
                    myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "false");
                
                myMail.From = fromAddress;
                myMail.To = toAddress;
                myMail.Cc = ccList;
                myMail.Subject = subjectLine;
                myMail.BodyFormat = System.Web.Mail.MailFormat.Html;
                myMail.Body = messageBody;

                System.Web.Mail.SmtpMail.SmtpServer = webMailSMTPServer + ":" + webMailSMTPPort;

                if (emailTypeID == E_RECEIPT_EMAIL_TYPE && !String.IsNullOrEmpty(param[11]) && File.Exists(param[11].Trim()))
                {
                    myMail.Attachments.Add(new System.Web.Mail.MailAttachment(param[11]));
                }
                else if (emailTypeID == COPY_OF_E_RECEIPT_EMAIL_TYPE && !String.IsNullOrEmpty(param[5]) && File.Exists(param[5].Trim()))
                {
                    myMail.Attachments.Add(new System.Web.Mail.MailAttachment(param[5]));
                }

                System.Web.Mail.SmtpMail.Send(myMail);               
              
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                ExceptionLogger.PublishException(ex, "Error", ListenerType.Email);

                return false;
            }
        }

        #endregion

        #region Load Email Template

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public string LoadEmailTemplate(string templateName)
        {
            StringBuilder sb = new StringBuilder();
            string templatePath = AppDomain.CurrentDomain.BaseDirectory;
            sb.Append(templatePath);
            sb.Append(EMAIL_TEMPLATE_FOLDER);
            sb.Append(BACKSLASH);
            sb.Append(templateName);
            MessageLogger.Write("Template Path: ", CategoryType.Information, Convert.ToString(sb), FPEmailServerNameSpace);

            return Convert.ToString(sb);
        }

        #endregion
        
    }
}



