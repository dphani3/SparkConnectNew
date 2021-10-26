
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: FocusConnectSerializer.cs
  Description: This is the Generic Xml Serializer wrapper class for serializing/deserializing the given Request/response types.
  Date Created : 19-Nov-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

#endregion

namespace TF.FocusPayCheckOut.Serialization
{
    #region FocusConnectSerializer

    /// <summary>
    /// This is the Generic Xml Serializer wrapper class for serializing/deserializing the given Request/response types.
    /// </summary>
    public class FocusConnectSerializer : IDisposable
    {
        #region Member Variables

        //This is for Garbage Collector.
        private bool isDisposed = false;
        //Type of object to serialize/deserialize.
        private Type type = null;
        //Xml Serializer that will actually do serialization/deserialization process.
        private XmlSerializer serializer = null;

        #endregion

        #region Parameterized Constructor

        /// <summary>
        /// This is the Parameterized Constructor that will capture the object of which type to do serialize/deserialize and also adds the
        /// dynamic XmlAttribute Overrides to customize the focus pay request/response root elements.
        /// </summary>
        /// <param name="type">Type of object to serialize/deserialize.</param>
        public FocusConnectSerializer(Type type)
        {
            this.type = type;
            serializer = new XmlSerializer(this.type, CreateXmlAttributeOverrides());
        }

        #endregion

        #region Destructor

        /// <summary>
        /// This is the Destructor for the class.
        /// </summary>
        ~FocusConnectSerializer()
        {
            //Since finalizer is called, there is no need to free the managed resources. So, false is passed.
            Dispose(false);
        }

        #endregion

        #region Serialize

        /// <summary>
        /// Serializes the specified object into XML format and writes the same into the given stream.
        /// </summary>
        /// <param name="transactionStream">Stream used to write the XML document.</param>
        /// <param name="objectInstance">Object to serialize.</param>
        public void Serialize(Stream transactionStream, object objectInstance)
        {
            serializer.Serialize(transactionStream, objectInstance);
        }

        #endregion

        #region Deserialize

        /// <summary>
        /// Deserializes the XML document provided through the stream into corresponding Request/Response object.
        /// </summary>
        /// <param name="transactionStream">Stream that contains the XML document to deserialize.</param>
        /// <returns>Object being deserialized.</returns>
        public object Deserialize(Stream transactionStream)
        {
            return serializer.Deserialize(transactionStream);
        }

        #endregion        

        #region CreateXmlAttributeOverrides

        /// <summary>
        /// Creates the Dynamic XmlAttribute Overrides to customize the focus pay request/response root elements.
        /// </summary>
        /// <returns>Dynamic XmlAttribute Overrides object.</returns>
        private XmlAttributeOverrides CreateXmlAttributeOverrides()
        {
            //Create an XmlAttributes to override the default root element name.
            XmlAttributes xmlAttributes = new XmlAttributes();

            //Create an XmlRootAttribute and set its element name.
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute();

            //If the base class for the input type is Request, customize the request root element to configuration based value + Request.
            if (type.BaseType != null && type.BaseType.Name == "Request")
                xmlRootAttribute.ElementName = String.Format("{0}Request", ConfigurationManager.AppSettings["RequestResponseTemplate"].Trim());

            //Else, customize the response root element to configuration based value + Response.
            else
                xmlRootAttribute.ElementName = String.Format("{0}Response", ConfigurationManager.AppSettings["RequestResponseTemplate"].Trim());

            //Set the XmlRoot property to the XmlRoot object.
            xmlAttributes.XmlRoot = xmlRootAttribute;

            XmlAttributeOverrides xmlAttributeOverrides = new XmlAttributeOverrides();

            //Add the XmlAttributes object to the XmlAttributeOverrides object.
            xmlAttributeOverrides.Add(type, xmlAttributes);

            return xmlAttributeOverrides;
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

                    //Nullify the type information.
                    if (type != null)
                        type = null;

                    //Nullify the XmlSerializer.
                    if (serializer != null)
                        serializer = null;
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
