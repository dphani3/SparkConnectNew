
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
    public class AuditInformation
    {
        #region Member Variables

        private byte tableId;               //Audit table id.
        private byte systemId;              //Audit system id.
        private byte stateId;               //Audit state id.
        private byte eventId;               //Audit event id.
        private byte eventTypeId;           //Audit event type id.
        private byte originationEventId;    //Audit origination event id.
        private long userId;                //Audit user id.
        private long originalUserId;        //Audit original user id.
        private string auditDetail;         //Audit details.        
        private string auditData;           //Audit data.
        private byte entityId;              //Audit entity id.
        private byte originationEntityId;   //Audit origination entity id.
        private long affectedId;            //Audit affected id.

        #endregion

        #region Properties

        #region TableId

        /// <summary>
        /// Allows to get/set the audit table id.
        /// </summary>
        public byte TableId
        {
            get { return tableId; }
            set { tableId = value; }
        }

        #endregion

        #region SystemId

        /// <summary>
        /// Allows to get/set the audit system id.
        /// </summary>
        public byte SystemId
        {
            get { return systemId; }
            set { systemId = value; }
        }

        #endregion

        #region StateId

        /// <summary>
        /// Allows to get/set the audit state id.
        /// </summary>
        public byte StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }

        #endregion

        #region EventId

        /// <summary>
        /// Allows to get/set the audit event id.
        /// </summary>
        public byte EventId
        {
            get { return eventId; }
            set { eventId = value; }
        }

        #endregion

        #region EventTypeId

        /// <summary>
        /// Allows to get/set the audit event type id.
        /// </summary>
        public byte EventTypeId
        {
            get { return eventTypeId; }
            set { eventTypeId = value; }
        }

        #endregion

        #region OriginationEventId

        /// <summary>
        /// Allows to get/set the audit origination event id.
        /// </summary>
        public byte OriginationEventId
        {
            get { return originationEventId; }
            set { originationEventId = value; }
        }

        #endregion

        #region EntityId

        /// <summary>
        /// Allows to get/set the audit Entity id.
        /// </summary>
        public byte EntityId
        {
            get { return entityId ; }
            set { entityId  = value; }
        }

        #endregion

        #region OriginationEntityId

        /// <summary>
        /// Allows to get/set the audit origination entity id.
        /// </summary>
        public byte OriginationEntityId
        {
            get { return originationEntityId  ; }
            set { originationEntityId = value; }
        }

        #endregion

        #region AffectedId

        /// <summary>
        /// Allows to get/set the audit AffectedID which is the record ID of the table/entity affected 
        /// </summary>
        public long AffectedId
        {
            get { return affectedId ; }
            set { affectedId  = value; }
        }

        #endregion

        #region UserId

        /// <summary>
        /// Allows to get/set the audit user id.
        /// </summary>
        public long UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        #endregion

        #region OriginalUserId

        /// <summary>
        /// Allows to get/set the audit original user id.
        /// </summary>
        public long OriginalUserId
        {
            get
            {
                return originalUserId;
            }
            set
            {
                originalUserId = value;
            }
        }

        #endregion

        #region AuditDetail

        /// <summary>
        /// Allows to get/set the audit details.
        /// </summary>
        public string AuditDetail
        {
            get { return auditDetail; }
            set { auditDetail = value; }
        }

        #endregion        

        #region AuditData

        /// <summary>
        /// Allows to get/set the audit data.
        /// </summary>
        public string AuditData
        {
            get { return auditData; }
            set { auditData = value; }
        }

        #endregion

        #endregion
    }

    #endregion
}
