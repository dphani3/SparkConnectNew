
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Utility.cs
  Description: This is the partial class for BusinessOperations class to extend the utility functions for the main BusinessOperations class.
  Date Created : 25-Oct-2010
  Revision History: 
  */

#endregion

namespace TF.FocusPay.AuditLogger.BusinessLayer
{
    #region Utility

    /// <summary>
    /// This is the partial class for BusinessOperations class to extend the utility functions for the main BusinessOperations class.
    /// </summary>
    public partial class BusinessOperations
    {
        #region Stored Procedure Declarations
        const string SP_AUDIT_LOG_INFO                                      = "AUD_AuditLogInfo";
        const string SP_AUDIT_DATA_LOG_INFO                                 = "AUD_AuditDataLogInfo";
        const string SP_BULK_AUDIT_DATA_LOG_INFO                            = "AUD_BulkAuditDataLogInfo";
        #endregion

        #region Parameters
        const string SP_AUDIT_LOG_INFO_IN_PARAM_TABLE_ID                    = "@TableID";
        const string SP_AUDIT_LOG_INFO_IN_PARAM_SYSTEM_ID                   = "@SystemID";
        const string SP_AUDIT_LOG_INFO_IN_PARAM_STATE_ID                    = "@StateID";
        const string SP_AUDIT_LOG_INFO_IN_PARAM_EVENT_ID                    = "@EventID";
        const string SP_AUDIT_LOG_INFO_IN_PARAM_EVENT_TYPE_ID               = "@EventTypeID";
        const string SP_AUDIT_LOG_INFO_IN_PARAM_ORIGINATION_EVENT_ID        = "@OriginationEventID";
        const string SP_AUDIT_LOG_INFO_IN_PARAM_ENTITY_ID                   = "@EntityID";
        const string SP_AUDIT_LOG_INFO_IN_PARAM_ORIGINATION_ENTITY_ID       = "@OriginationEntityID";
        const string SP_AUDIT_LOG_INFO_IN_PARAM_AFFECTED_ID                 = "@AffectedID";
        const string SP_AUDIT_LOG_INFO_IN_PARAM_AFFECTED_IDs                = "@AffectedIDs";
        const string SP_AUDIT_LOG_INFO_IN_PARAM_USER_ID                     = "@UserID";
        const string SP_AUDIT_LOG_INFO_IN_PARAM_ORIGINAL_USER_ID            = "@OriginalUserID";
        const string SP_AUDIT_LOG_INFO_IN_PARAM_AUDIT_DETAIL                = "@AuditDetail";        
        const string SP_AUDIT_LOG_INFO_IN_PARAM_AUDIT_DATA                  = "@AuditData";        
        #endregion

    }

    #endregion
}
