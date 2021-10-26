
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: EmailInformation.cs
  Description: This class is the business object representation for email queue information.
  Date Created : 31-May-2011
  Revision History: 
  */

#endregion

namespace TF.ReceiptManager.BusinessLayer.BusinessObjects
{
    #region EmailInformation

    /// <summary>
    /// This class is the business object representation for email queue information.
    /// </summary>
    public class EmailInformation
    {
        #region Member Variables

        private string systemId;            //Email system id.
        private long companyId;             //Company ID.
        private long merchantId;            //Merchant ID.
        private long attendantId;           //Attendant ID.
        private int emailTypeId;            //E-mail type id (17/18).
        private string fromAddress;         //E-mail From address.
        private string toAddress;           //E-mail To address.
        private string cCList;              //E-mail CC list.
        private string messageBody;         //E-mail message body.
        private int priority;               //E-mail priority.
        private bool isTestMode;            //Transaction operation mode.
        private byte[] receiptStream;       //Transaction receipt data.

        #endregion

        #region Properties

        #region SystemId

        /// <summary>
        /// Allows to get/set the e-mail system id.
        /// </summary>
        public string SystemId
        {
            get { return systemId; }
            set { systemId = value; }
        }

        #endregion

        #region CompanyId

        /// <summary>
        /// Allows to get/set the attendants company id.
        /// </summary>
        public long CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        #endregion

        #region MerchantId

        /// <summary>
        /// Allows to get/set the attendants merchant id.
        /// </summary>
        public long MerchantId
        {
            get { return merchantId; }
            set { merchantId = value; }
        }

        #endregion

        #region AttendantId

        /// <summary>
        /// Allows to get/set the attendant id.
        /// </summary>
        public long AttendantId
        {
            get { return attendantId; }
            set { attendantId = value; }
        }

        #endregion

        #region EmailTypeId

        /// <summary>
        /// Allows to get/set the e-mail type id.
        /// </summary>
        public int EmailTypeId
        {
            get { return emailTypeId; }
            set { emailTypeId = value; }
        }

        #endregion

        #region FromAddress

        /// <summary>
        /// Allows to get/set the e-mail From address.
        /// </summary>
        public string FromAddress
        {
            get { return fromAddress; }
            set { fromAddress = value; }
        }

        #endregion

        #region ToAddress

        /// <summary>
        /// Allows to get/set the e-mail To address.
        /// </summary>
        public string ToAddress
        {
            get { return toAddress; }
            set { toAddress = value; }
        }

        #endregion

        #region CCList

        /// <summary>
        /// Allows to get/set the e-mail CC list.
        /// </summary>
        public string CCList
        {
            get { return cCList; }
            set { cCList = value; }
        }

        #endregion

        #region MessageBody

        /// <summary>
        /// Allows to get/set the e-mail message body.
        /// </summary>
        public string MessageBody
        {
            get { return messageBody; }
            set { messageBody = value; }
        }

        #endregion

        #region Priority

        /// <summary>
        /// Allows to get/set the e-mail priority.
        /// </summary>
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        #endregion

        #region IsTestMode

        /// <summary>
        /// Allows to get/set the transaction operation mode.
        /// </summary>
        public bool IsTestMode
        {
            get { return isTestMode; }
            set { isTestMode = value; }
        }

        #endregion

        #region ReceiptStream

        /// <summary>
        /// Allows to get/set the transaction receipt data.
        /// </summary>
        public byte[] ReceiptStream
        {
            get { return receiptStream; }
            set { receiptStream = value; }
        }

        #endregion

        #endregion
    }

    #endregion
}
