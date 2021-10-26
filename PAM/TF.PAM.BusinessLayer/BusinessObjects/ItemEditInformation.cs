
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: ItemEditInformation.cs
  Description: This is the item edit information object that encapsulates all items(screens and actions) related information.
  Date Created : 03-Jan-2011
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#endregion

namespace TF.PAM.BusinessLayer.BusinessObjects
{
    #region ItemEditInformation

    /// <summary>
    /// This is the item edit information object that encapsulates all items(screens and actions) related information.
    /// </summary>
    [Serializable]
    [XmlRoot]
    public class ItemEditInformation
    {
        #region Member Variables

        //Item ID.
        private short itemId;

        //Item Name.
        private string itemName;

        //Flag that indicates whether Create permission mask is allowed or not.
        private bool isCreateMaskAllowed;

        //Flag that indicates whether Create permission value is allowed or not.
        private bool isCreateValueAllowed;

        //Flag that indicates whether Edit permission mask is allowed or not.
        private bool isEditMaskAllowed;

        //Flag that indicates whether Edit permission value is allowed or not.
        private bool isEditValueAllowed;

        //Flag that indicates whether Delete permission mask is allowed or not.
        private bool isDeleteMaskAllowed;

        //Flag that indicates whether Delete permission value is allowed or not.
        private bool isDeleteValueAllowed;

        //Flag that indicates whether View permission mask is allowed or not.
        private bool isViewMaskAllowed;

        //Flag that indicates whether View permission value is allowed or not.
        private bool isViewValueAllowed;        

        #endregion

        #region Properties

        #region ItemId

        /// <summary>
        /// Allows to get/set the Item ID.
        /// </summary>
        public short ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        #endregion

        #region ItemName

        /// <summary>
        /// Allows to get/set the Item Name.
        /// </summary>
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        #endregion        

        #region IsCreateMaskAllowed

        /// <summary>
        /// Allows to get/set the flag which indicates whether Create permission mask is allowed or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Enable.</description>
        ///     </item>
        ///     <item>
        ///         <description>False ->Disable.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public bool IsCreateMaskAllowed
        {
            get { return isCreateMaskAllowed; }
            set { isCreateMaskAllowed = value; }
        }

        #endregion

        #region IsCreateValueAllowed

        /// <summary>
        /// Allows to get/set the flag which indicates whether Create permission value is allowed or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Check.</description>
        ///     </item>
        ///     <item>
        ///         <description>False ->Uncheck.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public bool IsCreateValueAllowed
        {
            get { return isCreateValueAllowed; }
            set { isCreateValueAllowed = value; }
        }

        #endregion

        #region IsEditMaskAllowed

        /// <summary>
        /// Allows to get/set the flag which indicates whether Edit permission mask is allowed or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Enable.</description>
        ///     </item>
        ///     <item>
        ///         <description>False ->Disable.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public bool IsEditMaskAllowed
        {
            get { return isEditMaskAllowed; }
            set { isEditMaskAllowed = value; }
        }

        #endregion

        #region IsEditValueAllowed

        /// <summary>
        /// Allows to get/set the flag which indicates whether Edit permission value is allowed or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Check.</description>
        ///     </item>
        ///     <item>
        ///         <description>False ->Uncheck.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public bool IsEditValueAllowed
        {
            get { return isEditValueAllowed; }
            set { isEditValueAllowed = value; }
        }

        #endregion

        #region IsDeleteMaskAllowed

        /// <summary>
        /// Allows to get/set the flag which indicates whether Delete permission mask is allowed or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Enable.</description>
        ///     </item>
        ///     <item>
        ///         <description>False ->Disable.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public bool IsDeleteMaskAllowed
        {
            get { return isDeleteMaskAllowed; }
            set { isDeleteMaskAllowed = value; }
        }

        #endregion

        #region IsDeleteValueAllowed

        /// <summary>
        /// Allows to get/set the flag which indicates whether Delete permission value is allowed or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Check.</description>
        ///     </item>
        ///     <item>
        ///         <description>False ->Uncheck.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public bool IsDeleteValueAllowed
        {
            get { return isDeleteValueAllowed; }
            set { isDeleteValueAllowed = value; }
        }

        #endregion

        #region IsViewMaskAllowed

        /// <summary>
        /// Allows to get/set the flag which indicates whether View permission mask is allowed or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Enable.</description>
        ///     </item>
        ///     <item>
        ///         <description>False ->Disable.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public bool IsViewMaskAllowed
        {
            get { return isViewMaskAllowed; }
            set { isViewMaskAllowed = value; }
        }

        #endregion

        #region IsViewValueAllowed

        /// <summary>
        /// Allows to get/set the flag which indicates whether View permission value is allowed or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Check.</description>
        ///     </item>
        ///     <item>
        ///         <description>False ->Uncheck.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public bool IsViewValueAllowed
        {
            get { return isViewValueAllowed; }
            set { isViewValueAllowed = value; }
        }

        #endregion

        #endregion
    }

    #endregion    

    #region EditRolePermissions

    /// <summary>
    /// This is the role permission information object that encapsulates all items(screens and actions) related information on edit mode.
    /// </summary>
    [Serializable]
    [XmlRoot]
    public class EditRolePermissions
    {
        #region Member Variables

        //Edit item information collection.
        private List<ItemEditInformation> itemEditInformationCollection;

        //Response code.
        private int responseCode;

        //Response message.
        private string responseMessage;

        //Role Name.
        private string roleName;        

        #endregion

        #region Properties

        #region ItemEditInformationCollection

        /// <summary>
        /// Allows to get/set the edit item information collection.
        /// </summary>
        public List<ItemEditInformation> ItemEditInformationCollection
        {
            get { return itemEditInformationCollection; }
            set { itemEditInformationCollection = value; }
        }

        #endregion

        #region ResponseCode

        /// <summary>
        /// Allows to get/set the Response code.
        /// </summary>
        public int ResponseCode
        {
            get { return responseCode; }
            set { responseCode = value; }
        }

        #endregion

        #region ResponseMessage

        /// <summary>
        /// Allows to get/set the Response message.
        /// </summary>
        public string ResponseMessage
        {
            get { return responseMessage; }
            set { responseMessage = value; }
        }

        #endregion

        #region RoleName

        /// <summary>
        /// Allows to get/set the Role Name.
        /// </summary>
        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        #endregion

        #endregion
    }

    #endregion
}
