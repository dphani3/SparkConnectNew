
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: IFetchReceipt.cs
  Description: This interface will allow the external applications like Management Portal to get/email the transaction receipt which was performed earlier.
  Date Created : 31-May-2011
  Revision History: 
  */

#endregion

#region Namespaces

using System.ServiceModel;

#endregion

namespace TF.ReceiptManager.IRGen
{
    #region Internal Namespaces

    using DataContracts;

    #endregion

    #region IFetchReceipt

    /// <summary>
    /// This interface will allow the external applications like Management Portal to get/email the transaction receipt which was performed earlier.
    /// </summary>
    [ServiceContract(Namespace = "http://www.tfpayments.com/")]
    public interface IFetchReceipt
    {
        #region GetReceipt

        /// <summary>
        /// Gets the latest successful transaction receipt for the given FCRRN.
        /// </summary>
        /// <param name="fCRRN">FCRRN number.</param>        
        /// <param name="userId">User ID.</param>
        /// <returns>Latest successful transaction receipt details.</returns>
        [OperationContract]
        ReceiptInformation GetReceipt(string fCRRN, long userId);

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
        [OperationContract]
        bool EmailReceipt(string fCRRN, long userId, string toAddress);

        #endregion
    }

    #endregion
}
