
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Response.cs
  Description: This is the base response class for FocusPay Transaction methods like Login and CCTransaction.
  Date Created : 20-Nov-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.IO;

#endregion

namespace TF.REVCheckOut.Response
{
    #region Internal Namespaces

    using Serialization;

    #endregion

    #region Response

    /// <summary>
    /// This is the base response class for FocusPay Transaction methods like Login and CCTransaction.
    /// </summary>
    public class Response : IDisposable
    {
        #region Member Variables

        //This is for Garbage Collector.
        private bool isDisposed = false;
        
        //Serializer wrapper for all request/response types.
        protected static FocusConnectSerializer connectSerializer = null;

        #endregion

        #region Constructor

        /// <summary>
        /// This is the Constructor for the Request class.
        /// </summary>
        public Response()
        {
            
        }

        #endregion

        #region Destructor

        /// <summary>
        /// This is the Destructor for the class.
        /// </summary>
        ~Response()
        {
            //Since finalizer is called, there is no need to free the managed resources. So, false is passed.
            Dispose(false);
        }

        #endregion

        #region DeserializeFocusConnectMessage

        /// <summary>
        /// Deserializes the XML document provided through the stream into corresponding Request/Response object.
        /// </summary>
        /// <param name="messageStream">Stream that contains the XML document to deserialize.</param>
        /// <param name="objectType">Type of Request/Response object.</param>
        /// <returns>Object being deserialized.</returns>
        public static object DeserializeFocusConnectMessage(Stream messageStream, Type objectType)
        {
            connectSerializer = new FocusConnectSerializer(objectType);
            return connectSerializer.Deserialize(messageStream);
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
