
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: GenReceipt.cs
  Description: This class will allow the external applications to generate the transaction receipt after performing successful transaction.
  Date Created : 31-May-2011
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

    #region GenReceipt

    /// <summary>
    /// This class will allow the external applications to generate the transaction receipt after performing successful transaction.
    /// </summary>
    public class GenReceipt : Receipt, IGenReceipt
    {
        #region Member Variables

        #region Constant Variables

        //Transaction email receipt type.
        const int E_RECEIPT_EMAIL_TYPE = 17;

        #endregion

        #region Instance Variables

        //Google static maps url.
        string locationMapUrl = string.Empty;

        #endregion

        #endregion

        #region Private Constructor

        /// <summary>
        /// This is the private constructor of the class.
        /// </summary>
        private GenReceipt()
        {           
        }

        #endregion

        #region IReceipt Members

        #region GenerateReceipt

        /// <summary>
        /// Generates the transaction receipt with given transaction details.
        /// </summary>
        /// <param name="transactionInformation">Transaction details.</param>
        public void GenerateReceipt(TransactionInformation transactionInformation)
        {
            rmrLogger.Debug("Processing GenerateReceipt request.");

            try
            {
                //Check whether transaction details provided or not.
                if (transactionInformation != null)
                {                    
                    //Construct the Google static maps url if the geo coordinates provided in the request.
                    locationMapUrl = GenerateLocationMapUrl(transactionInformation.Latitude, transactionInformation.Longitude);                    

                    //Check whether the transaction is successful or not.
                    if (transactionInformation.TransactionStatus == SUCCESSFUL_TRANSACTION_STATUS)
                    {
                        rmrLogger.Debug("Given transaction is a successful one.");

                        //Construct the SSRS web service request parameters for receipt generation.
                        ParameterValue attendantNameParameter           = new ParameterValue { Name = "AttendantName", Value = !String.IsNullOrEmpty(transactionInformation.AttendantName) ? transactionInformation.AttendantName : "" };
                        ParameterValue fCRRNParameter                   = new ParameterValue { Name = "FCRRN", Value = !String.IsNullOrEmpty(transactionInformation.FCRRN) ? transactionInformation.FCRRN : "" };
                        ParameterValue transactionTypeParameter         = new ParameterValue { Name = "TransactionType", Value = !String.IsNullOrEmpty(transactionInformation.TransactionType) ? transactionInformation.TransactionType : "" };
                        ParameterValue transactionIDParameter           = new ParameterValue { Name = "TransactionID", Value = !String.IsNullOrEmpty(transactionInformation.TransactionID) ? transactionInformation.TransactionID : "" };
                        ParameterValue transactionDateParameter         = new ParameterValue { Name = "TransactionDate", Value = !String.IsNullOrEmpty(transactionInformation.TransactionDate) ? transactionInformation.TransactionDate : "" };
                        ParameterValue timeZoneParameter                = new ParameterValue { Name = "TimeZone", Value = !String.IsNullOrEmpty(transactionInformation.TimeZone) ? transactionInformation.TimeZone : "" };
                        ParameterValue cardNumberParameter              = new ParameterValue { Name = "CardNumber", Value = !String.IsNullOrEmpty(transactionInformation.CardNumber) ? transactionInformation.CardNumber : "" };
                        ParameterValue amountParameter                  = new ParameterValue { Name = "Amount", Value = (transactionInformation.Amount != 0) ? transactionInformation.Amount.ToString() : "" };
                        ParameterValue additionalAmountParameter        = new ParameterValue { Name = "AdditionalAmount", Value = (transactionInformation.AdditionalAmount != 0) ? transactionInformation.AdditionalAmount.ToString() : "" };
                        ParameterValue totalAmountParameter             = new ParameterValue { Name = "TotalAmount", Value = (transactionInformation.TotalAmount != 0) ? transactionInformation.TotalAmount.ToString() : "" };
                        ParameterValue statusParameter                  = new ParameterValue { Name = "Status", Value = !String.IsNullOrEmpty(transactionInformation.TransactionStatus) ? transactionInformation.TransactionStatus : "" };
                        ParameterValue customerNameParameter            = new ParameterValue { Name = "CustomerName", Value = !String.IsNullOrEmpty(transactionInformation.CustomerName) ? transactionInformation.CustomerName : "" };
                        ParameterValue customerEmailAddressParameter    = new ParameterValue { Name = "CustomerEmailAddress", Value = !String.IsNullOrEmpty(transactionInformation.EmailAddress) ? transactionInformation.EmailAddress : "" };
                        ParameterValue locationUrlParameter             = new ParameterValue { Name = "LocationUrl", Value = !String.IsNullOrEmpty(locationMapUrl) ? locationMapUrl : "" };
                        ParameterValue currencyTypeParameter            = new ParameterValue { Name = "CurrencyType", Value = !String.IsNullOrEmpty(transactionInformation.CurrencyType) ? transactionInformation.CurrencyType : "" };
                        ParameterValue chanakyaTransactionIDParameter   = new ParameterValue { Name = "ChanakyaTransactionID", Value = (transactionInformation.ChanakyaID != 0) ? transactionInformation.ChanakyaID.ToString() : "" };
                        ParameterValue attendantIDParameter             = new ParameterValue { Name = "AttendantID", Value = (transactionInformation.AttendantID != 0) ? transactionInformation.AttendantID.ToString() : "" };
                        ParameterValue isReceiptRequiredParameter       = new ParameterValue { Name = "IsReceiptInformationRequired", Value = true.ToString() };
                        ParameterValue notes                            = new ParameterValue { Name = "Notes", Value = transactionInformation.Notes.ToString() };
                        ParameterValue convenienceFee                   = new ParameterValue { Name = "ConvenienceFee", Value = (transactionInformation.ConvenienceFee != 0) ? transactionInformation.ConvenienceFee.ToString() : "" };
                        ParameterValue invoiceNumber                    = new ParameterValue { Name = "InvoiceNumber", Value = !String.IsNullOrEmpty(transactionInformation.InvoiceNumber) ? transactionInformation.InvoiceNumber : "" };
                        ParameterValue orderNumber                      = new ParameterValue { Name = "OrderNumber", Value = !String.IsNullOrEmpty(transactionInformation.OrderNumber) ? transactionInformation.OrderNumber : "" };
                        
                        string parameterLanguage        = "en-us";          //Locale.
                        string extension                = string.Empty;     //Extension type.
                        string mimeType                 = string.Empty;     //MIME type. 
                        string encoding                 = string.Empty;     //Encoding type.                                               
                        Warning[] warnings              = null;             //Warnings.
                        string[] streamIds              = null;             //Stream IDs.

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

                            if (!String.IsNullOrEmpty(locationMapUrl))
                            {
                                //Store the google static maps url into database.
                                businessLayer.StoreReceiptDetails(transactionInformation.ChanakyaID, locationMapUrl, null, null, transactionInformation.UserID);

                                rmrLogger.Debug("Stored google static maps url into database.");
                            }
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

                            //Store the receipt details into database along with google static maps url.
                            int status = businessLayer.StoreReceiptDetails(transactionInformation.ChanakyaID, locationMapUrl, receiptData, receiptFileName, transactionInformation.UserID);

                            //If the storage operation is successful.
                            if (status == 0)
                            {
                                rmrLogger.Debug("Receipt has been stored successfully into database.");

                                //Send the transaction details to email queue.
                                status = businessLayer.SendToEmailQueue(new EmailInformation
                                {
                                    SystemId            = string.Empty,
                                    CompanyId           = transactionInformation.CompanyID,
                                    MerchantId          = transactionInformation.MerchantID,
                                    AttendantId         = transactionInformation.AttendantID,
                                    EmailTypeId         = E_RECEIPT_EMAIL_TYPE,
                                    ToAddress           = transactionInformation.EmailAddress,
                                    CCList              = transactionInformation.MerchantEmailAddress,
                                    MessageBody         = GetEmailMessageBody(transactionInformation, receiptFileFullName),
                                    Priority            = HIGH_PRIORITY,
                                    IsTestMode          = String.IsNullOrEmpty(transactionInformation.IsTestMode) ? false : transactionInformation.IsTestMode.ToUpper() == "Y" ? true : false,
                                    ReceiptStream       = (ReportConfigurationSection.Report.SendReceiptDataToEmailQueue) ? receiptData : null
                                });

                                //If the email queue operation is successful.
                                if (status == 0)
                                {
                                    rmrLogger.Debug("Email queue operation is successful.");
                                }
                                //Failed to send the recept details to email queue.
                                else
                                {
                                    rmrLogger.Debug("Failed to send the recept details to email queue.");
                                }
                            }
                            //Failed to store the recept details into database.
                            else
                            {
                                rmrLogger.Debug("Failed to store the recept details into database.");
                            }
                        }
                        //Failed to generate receipt by SSRS.
                        else
                        {
                            rmrLogger.Debug("Failed to generate receipt by SSRS.");

                            //Send the transaction details to email queue.
                            int status = businessLayer.SendToEmailQueue(new EmailInformation
                            {
                                SystemId            = string.Empty,
                                CompanyId           = transactionInformation.CompanyID,
                                MerchantId          = transactionInformation.MerchantID,
                                AttendantId         = transactionInformation.AttendantID,
                                EmailTypeId         = E_RECEIPT_EMAIL_TYPE,
                                ToAddress           = transactionInformation.EmailAddress,
                                CCList              = transactionInformation.MerchantEmailAddress,
                                MessageBody         = GetEmailMessageBody(transactionInformation, null),
                                Priority            = HIGH_PRIORITY,
                                IsTestMode          = String.IsNullOrEmpty(transactionInformation.IsTestMode) ? false : transactionInformation.IsTestMode.ToUpper() == "Y" ? true : false,
                                ReceiptStream       = null
                            });

                            //If the email queue operation is successful.
                            if (status == 0)
                            {
                                rmrLogger.Debug("Email queue operation is successful.");
                            }
                            //Failed to send the recept details to email queue.
                            else
                            {
                                rmrLogger.Debug("Failed to send the recept details to email queue.");
                            }
                        }
                    }
                    //Failed transaction.
                    else
                    {
                        rmrLogger.Debug("Given transaction is a failed one.");

                        //Send the failure transaction details to email queue.
                        int status = businessLayer.SendToEmailQueue(new EmailInformation
                        {
                            SystemId            = string.Empty,
                            CompanyId           = transactionInformation.CompanyID,
                            MerchantId          = transactionInformation.MerchantID,
                            AttendantId         = transactionInformation.AttendantID,
                            EmailTypeId         = E_RECEIPT_EMAIL_TYPE,
                            ToAddress           = transactionInformation.EmailAddress,
                            CCList              = transactionInformation.MerchantEmailAddress,
                            MessageBody         = GetEmailMessageBody(transactionInformation, null),
                            Priority            = HIGH_PRIORITY,
                            IsTestMode          = String.IsNullOrEmpty(transactionInformation.IsTestMode) ? false : transactionInformation.IsTestMode.ToUpper() == "Y" ? true : false,
                            ReceiptStream       = null
                        });

                        //If the email queue operation is successful.
                        if (status == 0)
                        {
                            rmrLogger.Debug("Email queue operation is successful.");
                        }
                        //Failed to send the recept details to email queue.
                        else
                        {
                            rmrLogger.Debug("Failed to send the recept details to email queue.");
                        }
                    }
                }
                //No transaction details provided.
                else
                {
                    rmrLogger.Debug("No transaction details found in the request.");
                }
            }
            catch (Exception errorMessage)
            {
                rmrLogger.Error("Got Exception on GenerateReceipt : ", errorMessage);
            }

            rmrLogger.Debug("Completed GenerateReceipt request.");
        }

        #endregion

        #endregion

        #region Private Methods

        #region GetEmailMessageBody

        /// <summary>
        /// Constructs the E-mail message body to send the transaction email information to email queue.
        /// </summary>
        /// <param name="transactionInformation">Transaction information.</param>
        /// <param name="receiptFileName">Transaction receipt file name.</param>
        /// <returns>E-mail message body.</returns>
        private string GetEmailMessageBody(TransactionInformation transactionInformation, string receiptFileName)
        {
            //E-mail message body.
            string messageBody = string.Empty;

            //Build the E-mail message body.
            StringBuilder messageBuilder = new StringBuilder();

            //Delimiter for the email message body parameters.
            const string messageDelimiter = "|";

            //Append customer name.
            messageBuilder.Append((transactionInformation.CustomerName) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append merchant name.
            messageBuilder.Append((transactionInformation.MerchantName) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append transaction type.
            messageBuilder.Append((transactionInformation.TransactionType) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append transaction amount.
            messageBuilder.Append(String.Format("{0} {1}", transactionInformation.CurrencyType, String.Format(MONEY_FORMAT, transactionInformation.Amount)));
            messageBuilder.Append(messageDelimiter);

            //Check whether the tip amount is zero or not.
            if (transactionInformation.AdditionalAmount != 0)
            {
                //Append the tip amount of transaction.
                messageBuilder.Append(String.Format("{0} {1}", transactionInformation.CurrencyType, String.Format(MONEY_FORMAT, transactionInformation.AdditionalAmount)));
            }
            else
            {
                //Append empty tip amount.
                messageBuilder.Append(string.Empty);
            }
            messageBuilder.Append(messageDelimiter);

            //Append transaction total amount.
            messageBuilder.Append(String.Format("{0} {1}", transactionInformation.CurrencyType, String.Format(MONEY_FORMAT, transactionInformation.TotalAmount)));
            messageBuilder.Append(messageDelimiter);

            //Append masked credit card number.
            messageBuilder.Append((transactionInformation.CardNumber) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append transaction time.
            messageBuilder.Append((transactionInformation.TransactionDate) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append gateway transaction id.
            messageBuilder.Append((transactionInformation.TransactionID) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append transaction status.
            messageBuilder.Append((transactionInformation.TransactionStatus) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append transaction location url.
            messageBuilder.Append((locationMapUrl) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append receipt file name.
            messageBuilder.Append((receiptFileName) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append company brand name.
            messageBuilder.Append((transactionInformation.BrandName) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append company sales email.
            messageBuilder.Append((transactionInformation.SalesEmail) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append company sales phone.
            messageBuilder.Append((transactionInformation.SalesPhone) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append company support email.
            messageBuilder.Append((transactionInformation.SupportEmail) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append company support phone.
            messageBuilder.Append((transactionInformation.SupportPhone) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append company notification email address.
            messageBuilder.Append((transactionInformation.NotificationEmailAddress) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Start - Added by Nazreen
            //Append merchant email address.
            messageBuilder.Append((transactionInformation.MerchantEmailAddress) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append merchant contact phone.
            messageBuilder.Append((transactionInformation.MerchantContactPhone) ?? string.Empty);
            messageBuilder.Append(messageDelimiter);

            //Append notes.
            messageBuilder.Append((transactionInformation.Notes) ?? string.Empty);
            messageBuilder.Append(messageDelimiter); 

            //Check whether the Convenience fee amount is zero or not.
            if(transactionInformation.ConvenienceFee != 0)
            {
                //Append the tip amount of transaction.
                messageBuilder.Append(String.Format("{0} {1}", transactionInformation.CurrencyType, String.Format(MONEY_FORMAT, transactionInformation.ConvenienceFee)));
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

        #endregion
    }

    #endregion
}
