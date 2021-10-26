
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: ReceiptInformation.cs
  Description: This class is the data contract that encapsulates the transaction receipt information like receipt data and its file name.
  Date Created : 31-May-2011
  Revision History: 
  */

#endregion

#region Namespaces

using System.Runtime.Serialization;

#endregion

namespace TF.ReceiptManager.IRGen.DataContracts
{
    #region ReceiptInformation

    /// <summary>
    /// This class is the data contract that encapsulates the transaction receipt information like receipt data and its file name.
    /// </summary>
    [DataContract]
    public class ReceiptInformation
    {
        #region Member Variables

        private byte[] receiptStream;               //Transaction receipt data.
        private string receiptFileName;             //Transaction receipt file name.

        #endregion

        #region Properties

        #region ReceiptStream

        /// <summary>
        /// Allows to get/set the transactions receipt data.
        /// </summary>
        [DataMember]
        public byte[] ReceiptStream
        {
            get { return receiptStream; }
            set { receiptStream = value; }
        }

        #endregion

        #region ReceiptFileName

        /// <summary>
        /// Allows to get/set the transactions receipt file name.
        /// </summary>
        [DataMember]
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
