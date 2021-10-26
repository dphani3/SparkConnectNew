
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: IGenReceipt.cs
  Description: This interface will allow the external applications like FocusConnect to generate the transaction receipt after performing the 
               successful transaction.
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

    #region IGenReceipt

    /// <summary>
    /// This interface will allow the external applications like FocusConnect to generate the transaction receipt after performing the successful transaction.
    /// </summary>
    [ServiceContract(Namespace = "http://www.tfpayments.com/")]
    public interface IGenReceipt
    {
        #region GenerateReceipt

        /// <summary>
        /// Generates the transaction receipt with given transaction details.
        /// </summary>
        /// <param name="transactionInformation">Transaction details.</param>
        [OperationContract(IsOneWay=true)]
        void GenerateReceipt(TransactionInformation transactionInformation);

        #endregion
    }

    #endregion
}
