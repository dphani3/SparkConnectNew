
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: FetchReceipt.cs
  Description: This class will allow the external applications to fetch/email the transaction receipt after performing successful transaction.
  Date Created : 01-Jun-2011
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.IO;
using System.Text;

#endregion

namespace TF.ReceiptManager.RGen
{
    #region Internal Namespaces

    using BusinessLayer.BusinessObjects;
    using IRGen;
    using IRGen.DataContracts;
    using SSRS;    

    #endregion

    #region FetchReceipt

    /// <summary>
    /// This class will allow the external applications to fetch/email the transaction receipt after performing successful transaction.
    /// </summary>
    public class FetchReceipt : Receipt, IFetchReceipt
    {
        #region Member Variables

        //Copy of transaction email receipt type.
        const int COPY_OF_E_RECEIPT_EMAIL_TYPE = 18;

        #endregion

        #region Private Constructor

        /// <summary>
        /// This is the private constructor of the class.
        /// </summary>
        private FetchReceipt()
        {            
        }

        #endregion

        #region IFetchReceipt Members

        #region GetReceipt

        /// <summary>
        /// Gets the latest successful transaction receipt for the given FCRRN.
        /// </summary>
        /// <param name="fCRRN">FCRRN number.</param>
        /// <param name="userId">User ID.</param>
        /// <returns>Latest successful transaction receipt details.</returns>
        public ReceiptInformation GetReceipt(string fCRRN, long userId)
        {
            rmrLogger.Debug("Processing GetReceipt request.");

            ReceiptInformation receiptInformation = null;

            try
            {
                //Check whether FCRRN and user id are provided or not.
                if (!string.IsNullOrEmpty(fCRRN) && userId > 0)
                {
                    //Get the latest transaction receipt details for the given FCRRN number.
                    LatestTransactionInformation transactionInformation = businessLayer.GetLatestTransactionDetails(fCRRN, userId);

                    //Check whether the receipt details exists or not.
                    if (transactionInformation != null)
                    {
                        //Check whether the receipt details are retrieved successfully or not.
                        if (transactionInformation.ReceiptStream != null && transactionInformation.ReceiptStream.Length > 0)
                        {
                            rmrLogger.Debug("Retrieved receipt details successfully.");

                            //Construct the transaction receipt information with latest transaction receipt details.
                            receiptInformation = new ReceiptInformation
                            {
                                ReceiptStream       = transactionInformation.ReceiptStream,
                                ReceiptFileName     = transactionInformation.ReceiptFileName
                            };
                        }
                        //Else, regenerate the receipt details.
                        else
                        {
                            rmrLogger.Debug("No receipt details found for the given transaction.");

                            //Generate the copy of receipt.
                            CopyReceiptInformation copyReceiptInformation = GenerateCopyOfReceipt(transactionInformation, userId, null);

                            //Check whether the copy of receipt generated successfully or not.
                            if (copyReceiptInformation != null && copyReceiptInformation.IsReceiptGenerated == true)
                            {
                                rmrLogger.Debug("Successfully regenerated the receipt copy.");

                                //Construct the transaction copy receipt information with latest transaction receipt details.
                                receiptInformation = new ReceiptInformation
                                {
                                    ReceiptStream       = copyReceiptInformation.ReceiptStream,
                                    ReceiptFileName     = copyReceiptInformation.ReceiptFileName
                                };
                            }
                            //Send back null transaction receipt details.
                            else
                            {
                                rmrLogger.Debug("Failed to regenerate the receipt copy.");

                                receiptInformation = null;
                            }
                        }
                    }
                    //Send back null transaction receipt details.
                    else
                    {
                        rmrLogger.Debug("Failed to retrieve the receipt details.");

                        receiptInformation = null;
                    }
                }
                //Send back null transaction receipt details.
                else
                {
                    rmrLogger.Debug("Invalid input parameters.");

                    receiptInformation = null;
                }
            }
            //Send back null transaction receipt details.
            catch (Exception errorMessage)
            {
                rmrLogger.Error("Got Exception on GetReceipt : ", errorMessage);

                receiptInformation = null;
            }

            rmrLogger.Debug("Completed GetReceipt request.");

            //Send back latest transaction receipt details.
            return receiptInformation;
        }

        #endregion

        #region EmailReceipt

        /// <summary>
        /// Emails the latest successful transaction receipt for the given FCRRN.
        /// </summary>
        /// <param name="fCRRN">FCRRN number.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="toAddress">Email To Address.</param>
        /// <returns>Boolean flag that indicates the emailing transaction receipt is successful or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>true  -> Emailed successfully.</description>
        ///     </item>
        ///     <item>
        ///         <description>false -> Failed to email.</description>
        ///     </item>
        /// </list>
        /// </returns>
        public bool EmailReceipt(string fCRRN, long userId, string toAddress)
        {
            rmrLogger.Debug("Processing EmailReceipt request.");

            //Transaction receipt email status.
            bool isEmailSent = false;

            try
            {
                //Check whether FCRRN, user id and email to address are provided or not.
                if (!string.IsNullOrEmpty(fCRRN) && userId > 0 && !string.IsNullOrEmpty(toAddress))                
                {
                    //Get the latest transaction receipt details for the given FCRRN number.
                    LatestTransactionInformation transactionInformation = businessLayer.GetLatestTransactionDetails(fCRRN, userId);

                    //Check whether the receipt details exists or not.
                    if (transactionInformation != null)
                    {
                        //Check whether the receipt details are retrieved successfully or not.
                        if (transactionInformation.ReceiptStream != null && transactionInformation.ReceiptStream.Length > 0)
                        {
                            rmrLogger.Debug("Retrieved receipt details successfully.");

                            //Construct the receipt file full path as "LocalPath\\MMddyyyyHHmmss-BrandName-TransactionType-FCRRN.Extension" format.
                            string receiptFileFullName = string.Format("{0}{1}", ReportConfigurationSection.Report.LocalPath.Trim(), transactionInformation.ReceiptFileName);

                            //Check whether receipt file exists in the local file system or not.
                            if (!File.Exists(receiptFileFullName))
                            {
                                rmrLogger.Debug("No file found in the local file system.");

                                //Store the file into local file system.
                                File.WriteAllBytes(receiptFileFullName, transactionInformation.ReceiptStream);

                                rmrLogger.Debug("File has been created from the stream successfully.");
                            }
                            else
                            {
                                rmrLogger.Debug("File exists in the local file system.");
                            }

                            //Send the copy of transaction details to email queue.
                            int status = businessLayer.SendToEmailQueue(new EmailInformation
                            {
                                SystemId            = string.Empty,
                                CompanyId           = transactionInformation.CompanyId,
                                MerchantId          = transactionInformation.MerchantId,
                                AttendantId         = transactionInformation.AttendantId,
                                EmailTypeId         = COPY_OF_E_RECEIPT_EMAIL_TYPE,
                                ToAddress           = toAddress,
                                CCList              = string.Empty,
                                MessageBody         = GetEmailMessageBody(transactionInformation),
                                Priority            = HIGH_PRIORITY,
                                ReceiptStream       = (ReportConfigurationSection.Report.SendReceiptDataToEmailQueue) ? transactionInformation.ReceiptStream : null
                            });

                            //If the email queue operation is successful.
                            if (status == 0)
                            {
                                rmrLogger.Debug("Email queue operation is successful.");

                                isEmailSent = true;
                            }
                            //Send back email status as failed.
                            else
                            {
                                rmrLogger.Debug("Failed to send the recept details to email queue.");

                                isEmailSent = false;
                            }
                        }
                        //Else, regenerate the receipt details.
                        else
                        {
                            rmrLogger.Debug("No receipt details found for the given transaction.");

                            //Generate the copy of receipt.
                            CopyReceiptInformation copyReceiptInformation = GenerateCopyOfReceipt(transactionInformation, userId, toAddress);

                            //Check whether the copy of receipt emailed successfully or not.
                            if (copyReceiptInformation != null && copyReceiptInformation.IsReceiptGenerated == true)
                            {
                                rmrLogger.Debug("Successfully emailed the receipt copy.");

                                isEmailSent = true;
                            }
                            //Send back email status as failed.
                            else
                            {
                                rmrLogger.Debug("Failed to email the receipt copy.");

                                isEmailSent = false;
                            }                            
                        }
                    }
                    //Send back email status as failed.
                    else
                    {
                        rmrLogger.Debug("Failed to retrieve the receipt details.");

                        isEmailSent = false;
                    }
                }
                //Send back email status as failed.
                else
                {
                    rmrLogger.Debug("Invalid input parameters.");

                    isEmailSent = false;
                }
            }
            //Send back email status as failed.
            catch (Exception errorMessage)
            {
                rmrLogger.Error("Got Exception on EmailReceipt : ", errorMessage);

                isEmailSent = false;
            }

            rmrLogger.Debug("Completed EmailReceipt request.");

            //Send back the email status.
            return isEmailSent;
        }

        #endregion

        #endregion

        #region Private Methods

        #region ExtractTransactionDate

        /// <summary>
        /// Extracts the transaction time from the receipt file name.
        /// </summary>
        /// <param name="receiptFileName">Transaction receipt file name.</param>
        /// <returns>Transaction time.</returns>
        private string ExtractTransactionDate(string receiptFileName)
        {
            //Parsed transaction date.
            string parsedTransactionDate = string.Empty;

            //Check whether the file name has been provided or not.
            if (!String.IsNullOrEmpty(receiptFileName))
            {
                //Extract the date from the file name.
                string transactionDate = receiptFileName.Substring(0, receiptFileName.IndexOf('-'));

                //If the date has been extracted successfully.
                if (!String.IsNullOrEmpty(transactionDate))
                {
                    //Parse the date into human readable format.
                    parsedTransactionDate = DateTime.ParseExact(transactionDate, DATETIME_FORMAT, null).ToString();
                }
            }

            //Send back the transaction date.
            return parsedTransactionDate;
        }

        #endregion

        #region GetEmailMessageBody

        /// <summary>
        /// Constructs the E-mail message body to send the copy of transaction email information to email queue.
        /// </summary>
        /// <param name="transactionInformation">Transaction information.</param>
        /// <returns>E-mail message body.</returns>
        private string GetEmailMessageBody(LatestTransactionInformation transactionInformation)
        {
            //E-mail message body.
            string messageBody = string.Empty;

            //Build the E-mail message body.
            StringBuilder messageBuilder = new StringBuilder();

            //Delimiter for the email message body parameters.
            const string messageDelimiter = "|";
            
            //Append transaction type.
            messageBuilder.Append(transactionInformation.TransactionType);
            messageBuilder.Append(messageDelimiter);        

            //Append transaction total amount.
            messageBuilder.Append(String.Format("{0} {1}", transactionInformation.CurrencyCode, String.Format(MONEY_FORMAT, (transactionInformation.Amount + transactionInformation.AdditionalAmount))));
            messageBuilder.Append(messageDelimiter);

            //Append transaction time.
            messageBuilder.Append(ExtractTransactionDate(transactionInformation.ReceiptFileName));
            messageBuilder.Append(messageDelimiter);            

            //Append merchant name.
            messageBuilder.Append(transactionInformation.MerchantName);
            messageBuilder.Append(messageDelimiter);

            //Append user name.
            messageBuilder.Append(transactionInformation.UserName);
            messageBuilder.Append(messageDelimiter);
            
            //Append receipt file full path.
            messageBuilder.Append(string.Format("{0}{1}", ReportConfigurationSection.Report.LocalPath.Trim(), transactionInformation.ReceiptFileName));
            messageBuilder.Append(messageDelimiter);

            //Append company brand name.
            messageBuilder.Append(transactionInformation.BrandName);
            messageBuilder.Append(messageDelimiter);

            //Append company sales email.
            messageBuilder.Append(transactionInformation.SalesEmailAddress);
            messageBuilder.Append(messageDelimiter);

            //Append company sales phone.
            messageBuilder.Append(transactionInformation.SalesPhone);
            messageBuilder.Append(messageDelimiter);

            //Append company support email.
            messageBuilder.Append(transactionInformation.SupportEmailAddress);
            messageBuilder.Append(messageDelimiter);

            //Append company support phone.
            messageBuilder.Append(transactionInformation.SupportPhone);
            messageBuilder.Append(messageDelimiter);

            //Append company notification email address.
            messageBuilder.Append(transactionInformation.NotificationEmailAddress);
            messageBuilder.Append(messageDelimiter);

            //Start - Added by Nazreen
            //Append merchant email address.
            messageBuilder.Append((transactionInformation.MerchantEmailAddress) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append merchant contact phone.
            messageBuilder.Append((transactionInformation.MerchantContactPhone) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append customer name.
            messageBuilder.Append((transactionInformation.CustomerName) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append notes.
            messageBuilder.Append((transactionInformation.Notes) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Check whether the Convenience fee amount is zero or not.
            if(transactionInformation.ConvenienceFee != 0)
            {
                //Append the tip amount of transaction.
                messageBuilder.Append(String.Format("{0} {1}", transactionInformation.CurrencyCode, String.Format(MONEY_FORMAT, transactionInformation.ConvenienceFee)));
            }
            else
            {
                //Append empty tip amount.
                messageBuilder.Append(string.Empty);
            }

            //End -

            messageBuilder.Append(messageDelimiter); 

            //Append InvoiceNumber
            messageBuilder.Append((transactionInformation.InvoiceNumber) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append OrderNumber
            messageBuilder.Append((transactionInformation.OrderNumber) ?? string.Empty);

            //Convert to string and send.
            messageBody = messageBuilder.ToString();

            //Send the E-mail message body.
            return messageBody;
        }

        #endregion

        #region GenerateCopyOfReceipt

        /// <summary>
        /// Generates the copy of transaction receipt and emails if it is required.
        /// </summary>
        /// <param name="transactionInformation">Latest transaction details.</param>
        /// <param name="userId">User ID.</param>
        /// <param name="toAddress">To Email address.</param>
        /// <returns>Copy of transaction receipt details.</returns>
        private CopyReceiptInformation GenerateCopyOfReceipt(LatestTransactionInformation transactionInformation, long userId, string toAddress)
        {            
            rmrLogger.Debug("Processing GenerateCopyOfReceipt request.");

            //Receipt copy information.
            CopyReceiptInformation copyReceiptInformation = null;

            try
            {
                //Check whether latest transaction details are provided or not.
                if (transactionInformation != null)
                {
                    rmrLogger.Debug("Got latest transaction details.");

                    //Check the transaction status.
                    if (!String.IsNullOrEmpty(transactionInformation.TransactionStatus) && transactionInformation.TransactionStatus.ToUpper() == SUCCESSFUL_TRANSACTION_STATUS)
                    {
                        rmrLogger.Debug("Given transaction is a successful one.");

                        //Construct the SSRS web service request parameters for receipt generation.
                        ParameterValue attendantNameParameter               = new ParameterValue { Name = "AttendantName", Value = !String.IsNullOrEmpty(transactionInformation.AttendantName) ? transactionInformation.AttendantName : "" };
                        ParameterValue fCRRNParameter                       = new ParameterValue { Name = "FCRRN", Value = !String.IsNullOrEmpty(transactionInformation.FCRRN) ? transactionInformation.FCRRN : "" };
                        ParameterValue transactionTypeParameter             = new ParameterValue { Name = "TransactionType", Value = !String.IsNullOrEmpty(transactionInformation.TransactionType) ? transactionInformation.TransactionType : "" };
                        ParameterValue transactionIDParameter               = new ParameterValue { Name = "TransactionID", Value = !String.IsNullOrEmpty(transactionInformation.GatewayTransactionId) ? transactionInformation.GatewayTransactionId : "" };
                        ParameterValue transactionDateParameter             = new ParameterValue { Name = "TransactionDate", Value = !String.IsNullOrEmpty(transactionInformation.TransactionDate) ? transactionInformation.TransactionDate : "" };
                        ParameterValue timeZoneParameter                    = new ParameterValue { Name = "TimeZone", Value = "" };
                        ParameterValue cardNumberParameter                  = new ParameterValue { Name = "CardNumber", Value = !String.IsNullOrEmpty(transactionInformation.CardNumber) ? transactionInformation.CardNumber : "" };
                        ParameterValue amountParameter                      = new ParameterValue { Name = "Amount", Value = (transactionInformation.Amount != 0) ? String.Format(MONEY_FORMAT, transactionInformation.Amount) : "" };
                        ParameterValue additionalAmountParameter            = new ParameterValue { Name = "AdditionalAmount", Value = (transactionInformation.AdditionalAmount != 0) ? String.Format(MONEY_FORMAT, transactionInformation.AdditionalAmount) : "" };
                        ParameterValue totalAmountParameter                 = new ParameterValue { Name = "TotalAmount", Value = String.Format(MONEY_FORMAT, transactionInformation.Amount + transactionInformation.AdditionalAmount) };
                        ParameterValue statusParameter                      = new ParameterValue { Name = "Status", Value = !String.IsNullOrEmpty(transactionInformation.TransactionStatus) ? transactionInformation.TransactionStatus.ToUpper() : "" };
                        ParameterValue customerNameParameter                = new ParameterValue { Name = "CustomerName", Value = !String.IsNullOrEmpty(transactionInformation.CustomerName) ? transactionInformation.CustomerName : "" };
                        ParameterValue customerEmailAddressParameter        = new ParameterValue { Name = "CustomerEmailAddress", Value = !String.IsNullOrEmpty(transactionInformation.CustomerEmailAddress) ? transactionInformation.CustomerEmailAddress : "" };
                        ParameterValue locationUrlParameter                 = new ParameterValue { Name = "LocationUrl", Value = !String.IsNullOrEmpty(transactionInformation.GeoLocationUrl) ? transactionInformation.GeoLocationUrl : "" };
                        ParameterValue currencyTypeParameter                = new ParameterValue { Name = "CurrencyType", Value = !String.IsNullOrEmpty(transactionInformation.CurrencyCode) ? transactionInformation.CurrencyCode : "" };
                        ParameterValue chanakyaTransactionIDParameter       = new ParameterValue { Name = "ChanakyaTransactionID", Value = (transactionInformation.ChanakyaID != 0) ? transactionInformation.ChanakyaID.ToString() : "" };
                        ParameterValue attendantIDParameter                 = new ParameterValue { Name = "AttendantID", Value = (transactionInformation.AttendantId != 0) ? transactionInformation.AttendantId.ToString() : "" };
                        ParameterValue isReceiptRequiredParameter           = new ParameterValue { Name = "IsReceiptInformationRequired", Value = false.ToString() };
                        ParameterValue notes                                = new ParameterValue { Name = "Notes", Value = transactionInformation.ConvenienceFee.ToString() };
                        ParameterValue convenienceFee                       = new ParameterValue { Name = "ConvenienceFee", Value = (transactionInformation.ConvenienceFee != 0) ? String.Format(MONEY_FORMAT,transactionInformation.ConvenienceFee) : "" };
                        ParameterValue invoiceNumber                        = new ParameterValue { Name = "InvoiceNumber", Value = !String.IsNullOrEmpty(transactionInformation.InvoiceNumber) ? transactionInformation.InvoiceNumber : "" };
                        ParameterValue orderNumber                          = new ParameterValue { Name = "OrderNumber", Value = !String.IsNullOrEmpty(transactionInformation.OrderNumber) ? transactionInformation.OrderNumber : "" };
                        
                        string parameterLanguage            = "en-us";          //Locale.
                        string extension                    = string.Empty;     //Extension type.
                        string mimeType                     = string.Empty;     //MIME type. 
                        string encoding                     = string.Empty;     //Encoding type.                                               
                        Warning[] warnings                  = null;             //Warnings.
                        string[] streamIds                  = null;             //Stream IDs.

                        rmrLogger.Debug("Sending request to SSRS.");

                        byte[] receiptData = null;

                        try
                        {
                            //Load the SSRS report template.
                            ReportingService.LoadReport(ReportConfigurationSection.Report.Path, null);

                            //Send the transaction details to the SSRS and get back the receipt stream.
                            ReportingService.SetExecutionParameters(new ParameterValue[] { 
                                                                                attendantNameParameter, 
                                                                                fCRRNParameter,
                                                                                transactionTypeParameter, 
                                                                                transactionIDParameter, 
                                                                                transactionDateParameter, 
                                                                                timeZoneParameter, 
                                                                                cardNumberParameter,
                                                                                amountParameter,
                                                                                additionalAmountParameter, 
                                                                                totalAmountParameter, 
                                                                                statusParameter,
                                                                                customerNameParameter,
                                                                                customerEmailAddressParameter,
                                                                                locationUrlParameter, 
                                                                                currencyTypeParameter, 
                                                                                chanakyaTransactionIDParameter, 
                                                                                attendantIDParameter,
                                                                                isReceiptRequiredParameter,
                                                                                notes,
                                                                                convenienceFee,
                                                                                invoiceNumber,
                                                                                orderNumber},
                                                                                parameterLanguage);

                            receiptData = ReportingService.Render(ReportConfigurationSection.Report.Format, null, out extension, out mimeType,
                                                                    out encoding, out warnings, out streamIds);                            
                        }
                        catch (Exception ssrsErrorMessage)
                        {
                            rmrLogger.Error("Got Exception from SSRS : ", ssrsErrorMessage);

                            receiptData = null;
                        }

                        //Check whether receipt data has been generated successfully or not.
                        if (receiptData != null && receiptData.Length > 0)
                        {
                            rmrLogger.Debug("SSRS has generated receipt successfully.");

                            //Parse the transaction date into "MMddyyyyHHmmss" format.
                            string parsedTransactionDate = ParseTransactionDate(transactionInformation.TransactionDate);

                            //Construct the receipt file name into "MMddyyyyHHmmss-BrandName-TransactionType-FCRRN.Extension" format.
                            string receiptFileName = string.Format(RECEIPT_FILE_NAME_FORMAT, parsedTransactionDate, transactionInformation.BrandName,
                                                                    transactionInformation.TransactionType, transactionInformation.FCRRN,
                                                                    ReportConfigurationSection.Report.FileExtension);                                                        

                            rmrLogger.Debug("Receipt file name generated successfully.");

                            //Assign the file name to transaction information.
                            transactionInformation.ReceiptFileName = receiptFileName;

                            //Full path for the receipt file.
                            string receiptFileFullName = string.Empty;

                            //Check whether the local file system storage is allowed or not.
                            if (ReportConfigurationSection.Report.IsFileStorageAllowed)
                            {
                                rmrLogger.Debug("Local file system storage is allowed.");

                                //Create the local directory path if it is not exists.
                                if (!Directory.Exists(ReportConfigurationSection.Report.LocalPath.Trim()))
                                    Directory.CreateDirectory(ReportConfigurationSection.Report.LocalPath.Trim());

                                //Construct the receipt file full path as "LocalPath\\MMddyyyyHHmmss-BrandName-TransactionType-FCRRN.Extension" format.
                                receiptFileFullName = string.Format("{0}{1}", ReportConfigurationSection.Report.LocalPath.Trim(), receiptFileName.Trim());

                                //Store the file into local file system.
                                File.WriteAllBytes(receiptFileFullName, receiptData);

                                rmrLogger.Debug("Receipt has been stored successfully into local file system.");
                            }
                            else
                            {
                                rmrLogger.Debug("Local file system storage is not allowed.");
                            }

                            //Store the receipt details into database.
                            int status = businessLayer.StoreReceiptDetails(transactionInformation.ChanakyaID, transactionInformation.GeoLocationUrl, receiptData, receiptFileName, userId);

                            //If the storage operation is successful.
                            if (status == 0)
                            {
                                rmrLogger.Debug("Receipt has been stored successfully into database.");

                                //Check whether email needs to be send or not.
                                if (!String.IsNullOrEmpty(toAddress))
                                {
                                    rmrLogger.Debug("Caller is from EmailReceipt.");

                                    //Send the copy of transaction details to email queue.
                                    status = businessLayer.SendToEmailQueue(new EmailInformation
                                    {
                                        SystemId            = string.Empty,
                                        CompanyId           = transactionInformation.CompanyId,
                                        MerchantId          = transactionInformation.MerchantId,
                                        AttendantId         = transactionInformation.AttendantId,
                                        EmailTypeId         = COPY_OF_E_RECEIPT_EMAIL_TYPE,
                                        ToAddress           = toAddress,
                                        CCList              = string.Empty,
                                        MessageBody         = GetEmailMessageBody(transactionInformation),
                                        Priority            = HIGH_PRIORITY,
                                        ReceiptStream       = (ReportConfigurationSection.Report.SendReceiptDataToEmailQueue) ? receiptData : null
                                    });

                                    //If the email queue operation is successful.
                                    if (status == 0)
                                    {
                                        rmrLogger.Debug("Email queue operation is successful.");

                                        //Send back the copy of receipt details. 
                                        copyReceiptInformation = new CopyReceiptInformation
                                        {
                                            IsReceiptGenerated      = true,
                                            ReceiptStream           = receiptData,
                                            ReceiptFileName         = receiptFileName
                                        };
                                    }
                                    //Failed to send the recept details to email queue.
                                    else
                                    {
                                        rmrLogger.Debug("Failed to send the recept details to email queue.");

                                        copyReceiptInformation = null;
                                    }
                                }
                                else
                                {
                                    rmrLogger.Debug("Caller is from GetReceipt.");

                                    //Send back the copy of receipt details. 
                                    copyReceiptInformation = new CopyReceiptInformation
                                    {
                                        IsReceiptGenerated      = true,
                                        ReceiptStream           = receiptData,
                                        ReceiptFileName         = receiptFileName
                                    };
                                }
                            }   
                            //Failed to store the recept details into database.
                            else
                            {
                                rmrLogger.Debug("Failed to store the recept details into database.");

                                copyReceiptInformation = null;
                            }
                        }
                        //Failed to generate receipt by SSRS.
                        else
                        {
                            rmrLogger.Debug("Failed to generate receipt by SSRS.");

                            copyReceiptInformation = null;
                        }
                    }
                    //Failed transaction.
                    else
                    {
                        rmrLogger.Debug("Given transaction is a failed one.");

                        copyReceiptInformation = null;
                    }
                }
                //Send back null receipt copy information.
                else
                {
                    rmrLogger.Debug("No latest transaction details were found.");

                    copyReceiptInformation = null;
                }
            }
            //Send back null receipt copy information.
            catch (Exception errorMessage)
            {
                rmrLogger.Error("Got Exception on GenerateCopyOfReceipt : ", errorMessage);

                copyReceiptInformation = null;
            }

            rmrLogger.Debug("Completed GenerateCopyOfReceipt request.");

            //Send back the receipt copy information.
            return copyReceiptInformation;
        }

        #endregion

        #endregion
    }

    #endregion

    #region CopyReceiptInformation

    /// <summary>
    /// This class details the receipt copy generation details. 
    /// </summary>
    internal class CopyReceiptInformation
    {
        #region Member Variables

        //Flag that indicates whether the transaction receipt has been generated successfully or not.
        private bool isReceiptGenerated;

        //Receipt stream data.
        private byte[] receiptStream;

        //Receipt file name.
        private string receiptFileName;

        #endregion

        #region Properties

        #region IsReceiptGenerated

        /// <summary>
        /// Allows to get/set the flag which indicates whether the transaction receipt has been generated successfully or not.
        /// </summary>
        public bool IsReceiptGenerated
        {
            get { return isReceiptGenerated; }
            set { isReceiptGenerated = value; }
        }

        #endregion

        #region ReceiptStream

        /// <summary>
        /// Allows to get/set the receipt stream data.
        /// </summary>
        public byte[] ReceiptStream
        {
            get { return receiptStream; }
            set { receiptStream = value; }
        }

        #endregion

        #region ReceiptFileName

        /// <summary>
        /// Allows to get/set the receipt file name.
        /// </summary>
        public string ReceiptFileName
        {
            get { return receiptFileName; }
            set { receiptFileName = value; }
        }

        #endregion

        #endregion
    }

    #endregion
}
