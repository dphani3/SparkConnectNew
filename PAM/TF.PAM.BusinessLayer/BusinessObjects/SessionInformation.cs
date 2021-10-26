
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: SessionInformation.cs
  Description: This is the session information object that encapsulates the current login session assigned for the given user along with response
               code if any.
  Date Created : 25-Aug-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Xml.Serialization;

#endregion

namespace TF.PAM.BusinessLayer.BusinessObjects
{
    #region SessionInformation

    /// <summary>
    /// This is the session information object that encapsulates the current login session assigned for the given user along with response 
    /// code if any.
    /// </summary>
    [Serializable]
    [XmlRoot]
    public class SessionInformation
    {
        #region Member Variables

        //Current session associated with the user.
        private string sessionId;

        //Session generation response code.
        private int responseCode;       

        #endregion

        #region Properties

        #region SessionId

        /// <summary>
        /// Allows to get/set the current session associated with the user.
        /// </summary>
        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        #endregion

        #region ResponseCode

        /// <summary>
        /// Allows to get/set the session generation response code.
        /// </summary>
        public int ResponseCode
        {
            get { return responseCode; }
            set { responseCode = value; }
        }

        #endregion

        #endregion
    }

    #endregion
}
