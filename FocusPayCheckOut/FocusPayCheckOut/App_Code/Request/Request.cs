
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Request.cs
  Description: This is the base request class for FocusPay Transaction methods like Login and CCTransaction.
  Date Created : 20-Nov-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.IO;
using System.Text;

#endregion

namespace TF.FocusPayCheckOut.Request
{
    #region Internal Namespaces

    using Serialization;

    #endregion

    #region Request

    /// <summary>
    /// This is the base request class for FocusPay Transaction methods like Login and CCTransaction.
    /// </summary>
    public class Request : IDisposable
    {
        #region Member Variables

        //This is for Garbage Collector.
        private bool isDisposed = false;

        //This will be used for serializing all request/response types like Login and CCTransaction.
        private MemoryStream messageStream = null;

        //Serializer wrapper for all request/response types.
        private FocusConnectSerializer connectSerializer = null;

        #endregion

        #region Constructor

        /// <summary>
        /// This is the Constructor for the Request class.
        /// </summary>
        public Request()
        {
            
        }

        #endregion

        #region Destructor

        /// <summary>
        /// This is the Destructor for the class.
        /// </summary>
        ~Request()
        {
            //Since finalizer is called, there is no need to free the managed resources. So, false is passed.
            Dispose(false);
        }

        #endregion

        #region SerializeFocusConnectMessage

        /// <summary>
        /// Gets the Xml format of the request/response data for the given connect object type.
        /// </summary>
        /// <param name="connectObject">Connect object.</param>
        /// <returns>Xml serialized data for the given request/response object.</returns>
        public string SerializeFocusConnectMessage(object connectObject)
        {
            string messageData = string.Empty;

            //Serialize the provided request/response object using in-memory stream.
            using (messageStream = new MemoryStream())
            {
                connectSerializer = new FocusConnectSerializer(connectObject.GetType());
                connectSerializer.Serialize(messageStream, connectObject);
                messageData = Encoding.UTF8.GetString(messageStream.ToArray());
            }

            return messageData;
        }

        #endregion       

        #region Dispose

        /// <summary>
        /// Implementation of dispose to free resources.
        /// </summary>
        /// <param name="disposedStatus">The status of the disposed operation.</param>
        protected virtual void Dispose(bool disposedStatus)
        {
            if (!isDisposed)
            {
                isDisposed = true;

                //Released unmanaged resources.
                if (disposedStatus)
                {
                    //Release the managed resources.                

                    //Dispose the messageStream.
                    if (messageStream != null)
                    {
                        messageStream.Flush();
                        messageStream.Close();
                        messageStream.Dispose();
                        messageStream = null;
                    }

                    //Dispose the connectSerializer.
                    if (connectSerializer != null)
                    {
                        connectSerializer.Dispose();
                        connectSerializer = null;
                    }
                }
            }
        }

        #endregion

        #region IDisposable Members

        #region Dispose

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //True is passed in dispose method to clean managed resources.
            Dispose(true);

            //If dispose is called already, then inform GC to skip finalize on this instance.
            GC.SuppressFinalize(this);
        }

        #endregion

        #endregion
    }

    #endregion
}
