
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: AuthenticationStatus.cs
  Description: This class is the business object representation for authentication status.
  Date Created : 17-Aug-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Xml.Serialization;

#endregion

namespace TF.PAM.BusinessLayer.BusinessObjects
{
    #region AuthenticationStatus

    /// <summary>
    /// This class is the business object representation for authentication status.
    /// </summary>
    [Serializable]
    [XmlRoot]
    public class AuthenticationStatus
    {
        #region Member Variables

        //User ID.
        private long userId;

        //Company ID.
        private long companyId;

        //Merchant ID.
        private long merchantId;

        //Attendant ID.
        private long attendantId;
        
        //Authentication response code.
        private int resposeCode;

        //Authentication response message.
        private string responseMessage;

        #endregion

        #region Properties

        #region UserId

        /// <summary>
        /// Allows to get/set the User ID.
        /// </summary>
        public long UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        #endregion

        #region CompanyId

        /// <summary>
        /// Allows to get/set the company id.
        /// </summary>
        public long CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        #endregion

        #region MerchantId

        /// <summary>
        /// Allows to get/set the merchant id.
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

        #region ResposeCode

        /// <summary>
        /// Allows to get/set the authentication response code.
        /// </summary>
        public int ResposeCode
        {
            get { return resposeCode; }
            set { resposeCode = value; }
        }

        #endregion

        #region ResponseMessage

        /// <summary>
        /// Allows to get/set the authentication response message.
        /// </summary>
        public string ResponseMessage
        {
            get { return responseMessage; }
            set { responseMessage = value; }
        }

        #endregion

        #endregion
    }

    #endregion    
}
