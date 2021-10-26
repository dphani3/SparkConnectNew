
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: LoginRequest.cs
  Description: This class will process the FocusPay Transaction method named Login.
  Date Created : 20-Nov-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Xml;
using System.Xml.Serialization;

#endregion

namespace TF.REVCheckOut.Request
{
    #region LoginRequest

    /// <summary>
    /// This class will process the FocusPay Transaction method named Login.
    /// </summary>
    [Serializable]
    [XmlRoot]
    public class LoginRequest : Request
    {
        #region Member Variables

        //Login request type.
        private int requestType;

        //Login Device Id.
        private string deviceID;

        //Login User Name.
        private string userName;

        //Login Password.
        private string password;

        //Login Notes.
        private string notes;

        #endregion

        #region Properties

        #region RequestType

        /// <summary>
        /// Allows get/set the login request type.
        /// </summary>
        public int RequestType
        {
            get
            {
                return this.requestType;
            }
            set
            {
                this.requestType = value;
            }
        }

        #endregion

        #region DeviceID

        /// <summary>
        /// Allows get/set the login device id.
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string DeviceID
        {
            get
            {
                return this.deviceID;
            }
            set
            {
                this.deviceID = value;
            }
        }

        #endregion

        #region UserName

        /// <summary>
        /// Allows get/set the login user name.
        /// </summary>
        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
            }
        }

        #endregion

        #region Password

        /// <summary>
        /// Allows get/set the login password.
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        #endregion

        #region Notes

        /// <summary>
        /// Allows get/set the login notes.
        /// </summary>
        [XmlElement(IsNullable = true)]
        public string Notes
        {
            get
            {
                return this.notes;
            }
            set
            {
                this.notes = value;
            }
        }

        #endregion

        #endregion        
    }

    #endregion
}
