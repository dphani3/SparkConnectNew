
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: ItemInformation.cs
  Description: This is the item information object that encapsulates all items(screens and actions) related information.
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
    #region ItemInformation

    /// <summary>
    /// This is the item information object that encapsulates all items(screens and actions) related information.
    /// </summary>
    [Serializable]
    [XmlRoot]
    public class ItemInformation
    {
        #region Member Variables

        //Item ID.
        private short itemId;

        //Item Name.
        private string itemName;

        //Flag that indicates whether Create permission is allowed or not.
        private bool isCreateAllowed;

        //Flag that indicates whether Edit permission is allowed or not.
        private bool isEditAllowed;

        //Flag that indicates whether Delete permission is allowed or not.
        private bool isDeleteAllowed;

        //Flag that indicates whether View permission is allowed or not.
        private bool isViewAllowed;

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

        #region IsCreateAllowed

        /// <summary>
        /// Allows to get/set the flag which indicates whether Create permission is allowed or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Allowed.</description>
        ///     </item>
        ///     <item>
        ///         <description>False ->Not Allowed.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public bool IsCreateAllowed
        {
            get { return isCreateAllowed; }
            set { isCreateAllowed = value; }
        }

        #endregion

        #region IsEditAllowed

        /// <summary>
        /// Allows to get/set the flag which indicates whether Edit permission is allowed or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Allowed.</description>
        ///     </item>
        ///     <item>
        ///         <description>False ->Not Allowed.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public bool IsEditAllowed
        {
            get { return isEditAllowed; }
            set { isEditAllowed = value; }
        }

        #endregion

        #region IsDeleteAllowed

        /// <summary>
        /// Allows to get/set the flag which indicates whether Delete permission is allowed or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Allowed.</description>
        ///     </item>
        ///     <item>
        ///         <description>False ->Not Allowed.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public bool IsDeleteAllowed
        {
            get { return isDeleteAllowed; }
            set { isDeleteAllowed = value; }
        }

        #endregion

        #region IsViewAllowed

        /// <summary>
        /// Allows to get/set the flag which indicates whether View permission is allowed or not.
        /// <list type="bullet">
        ///     <item>
        ///         <description>True -> Allowed.</description>
        ///     </item>
        ///     <item>
        ///         <description>False ->Not Allowed.</description>
        ///     </item>
        /// </list>
        /// </summary>
        public bool IsViewAllowed
        {
            get { return isViewAllowed; }
            set { isViewAllowed = value; }
        }

        #endregion

        #endregion
    }

    #endregion

    #region ItemMaskInformation

    /// <summary>
    /// This is the item mask information object that encapsulates all items(screens and actions) related information.
    /// </summary>
    public class ItemMaskInformation : ItemInformation
    {
    }

    #endregion

    #region ItemValuesInformation

    /// <summary>
    /// This is the item values information object that encapsulates all items(screens and actions) related information.
    /// </summary>
    public class ItemValuesInformation : ItemInformation
    {
    }

    #endregion

    #region ItemValuesComparer

    /// <summary>
    /// Defines methods to support the comparison of ItemValuesInformation for equality.
    /// </summary>
    public class ItemValuesComparer : IEqualityComparer<ItemValuesInformation>
    {
        #region IEqualityComparer<ItemValuesInformation> Members

        #region Equals

        /// <summary>
        /// Determines whether the specified ItemValuesInformation objects are equal or not.
        /// </summary>
        /// <param name="x">The first object of type ItemValuesInformation to compare.</param>
        /// <param name="y">The second object of type ItemValuesInformation to compare.</param>
        /// <returns>True if the specified objects are equal; otherwise, false.</returns>
        public bool Equals(ItemValuesInformation x, ItemValuesInformation y)
        {
            //Check the equality of both objects based on their ItemId property.
            return x.ItemId == y.ItemId;
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Gets the hash code for the specified ItemValuesInformation object.
        /// </summary>
        /// <param name="obj">The ItemValuesInformation object for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified ItemValuesInformation object.</returns>
        public int GetHashCode(ItemValuesInformation obj)
        {
            //Return the hash code of the ItemId property for the given ItemValuesInformation object.
            return obj.ItemId.GetHashCode();
        }

        #endregion

        #endregion
    }

    #endregion

    #region NewItemInformation

    /// <summary>
    /// This is the new item information object that encapsulates all items(screens and actions) related information w.r.t. role id.
    /// </summary>
    public class NewItemInformation : ItemInformation
    {
        #region Member Variables

        //Role ID.
        private short roleId;

        //User ID.
        private long userId;

        #endregion

        #region Properties

        #region RoleId

        /// <summary>
        /// Allows to get/set the Role ID.
        /// </summary>
        public short RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        #endregion

        #region UserId

        /// <summary>
        /// Allows to get/set the User ID.
        /// </summary>
        public long UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        #endregion

        #endregion
    }

    #endregion

    #region RolePermissions

    /// <summary>
    /// This is the role permission information object that encapsulates all items(screens and actions) related information.
    /// </summary>
    [Serializable]
    [XmlRoot]
    public class RolePermissions
    {
        #region Member Variables

        //Item mask information collection.
        private List<ItemMaskInformation> itemInformationCollection;

        //Response code.
        private int responseCode;

        //Response message.
        private string responseMessage;
        
        #endregion

        #region Properties

        #region ItemInformationCollection

        /// <summary>
        /// Allows to get/set the item mask information collection.
        /// </summary>
        public List<ItemMaskInformation> ItemInformationCollection
        {
            get { return itemInformationCollection; }
            set { itemInformationCollection = value; }
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

        #endregion
    }

    #endregion    
}
