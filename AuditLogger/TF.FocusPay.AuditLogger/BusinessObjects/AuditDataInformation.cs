
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: AuditInformation.cs
  Description: This class is the business object representation for log audit information.
  Date Created : 25-Oct-2010
  Revision History: 
  */

#endregion

namespace TF.FocusPay.AuditLogger.BusinessObjects
{
    #region AuditInformation

    /// <summary>
    /// This class is the business object representation for log audit information.
    /// </summary>
    public class AuditDataInformation
    {        
        #region Properties

        #region TableId

        /// <summary>
        /// Allows to get/set the audit table id.
        /// </summary>
        public TableInfo TableId
        {
            get;
            set;
        }

        #endregion

        #region SystemId

        /// <summary>
        /// Allows to get/set the audit system id.
        /// </summary>
        public SystemInfo SystemId
        {
            get;
            set;
        }

        #endregion

        #region StateId

        /// <summary>
        /// Allows to get/set the audit state id.
        /// </summary>
        public StateInfo StateId
        {
            get;
            set;
        }

        #endregion

        #region EventId

        /// <summary>
        /// Allows to get/set the audit event id.
        /// </summary>
        public EventInfo EventId
        {
            get;
            set;
        }

        #endregion

        #region EventTypeId

        /// <summary>
        /// Allows to get/set the audit event type id.
        /// </summary>
        public EventTypeInfo EventTypeId
        {
            get;
            set;
        }

        #endregion

        #region OriginationEventId

        /// <summary>
        /// Allows to get/set the audit origination event id.
        /// </summary>
        public EventInfo OriginationEventId
        {
            get;
            set;
        }

        #endregion

        #region EntityId

        /// <summary>
        /// Allows to get/set the audit Entity id.
        /// </summary>
        public EntityInfo EntityId
        {
            get;
            set;
        }

        #endregion

        #region OriginationEntityId

        /// <summary>
        /// Allows to get/set the audit origination entity id.
        /// </summary>
        public EntityInfo OriginationEntityId
        {
            get;
            set;
        }

        #endregion

        #region AffectedId

        /// <summary>
        /// Allows to get/set the audit AffectedID which is the record ID of the table/entity affected 
        /// </summary>
        public long AffectedId
        {
            get;
            set;
        }

        #endregion

        #region AffectedIds

        /// <summary>
        /// Allows to get/set the audit AffectedIDs which is the comma separated string of record IDs of the table/entity affected 
        /// </summary>
        public string AffectedIds
        {
            get;
            set;
        }

        #endregion

        #region UserId

        /// <summary>
        /// Allows to get/set the audit user id.
        /// </summary>
        public long UserId
        {
            get;
            set;
        }

        #endregion

        #region OriginalUserId

        /// <summary>
        /// Allows to get/set the audit user id.
        /// </summary>
        public long OriginalUserId
        {
            get;
            set;
        }

        #endregion

        #region AuditDetail

        /// <summary>
        /// Allows to get/set the audit details.
        /// </summary>
        public string AuditDetail
        {
            get;
            set;
        }

        #endregion        

        #region AuditData

        /// <summary>
        /// Allows to get/set the audit data.
        /// </summary>
        public string AuditData
        {
            get;
            set;
        }

        #endregion

        #endregion
    }

    #endregion
}
