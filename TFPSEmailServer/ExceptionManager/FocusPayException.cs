
#region Header Information
/***********************************************************************************************************
NameSpace: IR.QueueReader.ExceptionManager
File: ExceptionManager.cs
Class: ExceptionManager
Author: Sarvesh.T.S
Created Date: 16-May-2008
Reviewed By: 
***********************************************************************************************************/
#endregion

#region Namspace declaration
using System;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;
using System.Resources;
using System.Configuration;
using TF.FocusPay.EmailServer.LogManager;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
#endregion

namespace TF.FocusPay.EmailServer.ExceptionManager
{
    public class FocusPayException : ApplicationException
    {

        private string errorMessage = null;
        private string errorCode = null;

        public FocusPayException(Exception exception, string eCode)
        {
            try
            {

                HandleException(exception);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public FocusPayException(string eCode)
        {
            try
            {
                this.errorCode = eCode;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void HandleException(Exception exp)
        {
            bool rethrow = ExceptionPolicy.HandleException(exp, "GlobalPolicy");
            ExceptionLogger.PublishException(exp, "Error", ListenerType.File);
        }


        public string ErrorMessage
        {
            get { return errorMessage; }
        }

        public string ErrorCode
        {
            get { return errorCode; }
        }

    }
}
