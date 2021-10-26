using System;
using System.Collections.Generic;
using System.Text;

namespace TF.FocusPay.EmailServer.EmailBusinessLogic
{
    public class EmailContext
    {
        public EmailContext()
        {
        }

        

    }

    public enum EmailMessageParams
    {
        RequestID = 1,
        TransactionType,
        MobileNumber,
        ChanakyaTransID,
        GatewayTransactionID,
        CardNumber,
        TotalAmount,
        TxnDateTime,
        ReceiptHeader1,
        ReceiptHeader2,
        ReceiptHeader3,
        ReceiptFooter1,
        ReceiptFooter2,
        ReceiptFooter3,
        LogoID,
    }
}
