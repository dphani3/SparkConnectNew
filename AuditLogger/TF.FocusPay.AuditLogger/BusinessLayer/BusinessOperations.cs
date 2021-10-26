
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: BusinessOperations.cs
  Description: This is the business layer for the Audit Logger to perform all business operations.
  Date Created : 25-Oct-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Configuration;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Reflection; 

#endregion

namespace TF.FocusPay.AuditLogger.BusinessLayer
{
    #region Internal Namespaces

    using BusinessObjects;
    using DataAccessLayer;

    #endregion

    #region BusinessOperations

    /// <summary>
    /// This is the business layer for the Audit Logger to perform all business operations.
    /// </summary>
    public partial class BusinessOperations : IDisposable
    {
        #region Member Variables

        private bool isDisposed = false;                    //This is for Garbage Collector.
        private DAL dataAccessLayer = null;                 //Data Access Layer object to communicate with database.

        //Database connection string key that can be mapped with configuration file.
        const string DATABASE_CONNECTION_STRING_KEY = "AUDITCON";

        #endregion

        #region Constructor

        /// <summary>
        /// This is the Constructor for the class that will instantiate the data access layer.
        /// </summary>
        /// <param name="connectionStringName">Database connection string information.</param>
        public BusinessOperations()
        {
            var appConfig = ConfigurationManager.OpenExeConfiguration(AppDomain.CurrentDomain.BaseDirectory + "bin\\TF.FocusPay.AuditLogger.dll"); 
            string dllConfigData = appConfig.ConnectionStrings.ConnectionStrings[DATABASE_CONNECTION_STRING_KEY].ConnectionString.Trim();
            this.dataAccessLayer = new DAL(dllConfigData);
           
        }

        #endregion

        #region Destructor

        /// <summary>
        /// This is the Destructor for the class.
        /// </summary>
        ~BusinessOperations()
        {
            //Since finalizer is called, there is no need to free the managed resources. So, false is passed.
            Dispose(false);
        }

        #endregion

        #region LogAuditInformation

        /// <summary>
        /// Logs the user audit information for the given event details.
        /// </summary>
        /// <param name="auditInformation">User audit information.</param>
        public void LogAuditInformation(AuditInformation auditInformation)
        {
            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                //Check whether the audit information exists or not.
                if (auditInformation != null)
                {
                    dataAccessLayer.ExecuteNonQuery(SP_AUDIT_LOG_INFO, ref responseCode,
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_TABLE_ID, SqlDbType.TinyInt, auditInformation.TableId),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_SYSTEM_ID, SqlDbType.TinyInt, auditInformation.SystemId),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_STATE_ID, SqlDbType.TinyInt, auditInformation.StateId),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_EVENT_ID, SqlDbType.TinyInt, auditInformation.EventId),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_EVENT_TYPE_ID, SqlDbType.TinyInt, auditInformation.EventTypeId),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_ORIGINATION_EVENT_ID, SqlDbType.TinyInt, auditInformation.OriginationEventId),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_USER_ID, SqlDbType.BigInt, auditInformation.UserId),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_ORIGINAL_USER_ID, SqlDbType.BigInt, auditInformation.OriginalUserId),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_AUDIT_DETAIL, SqlDbType.VarChar, auditInformation.AuditDetail),                                                        
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_AUDIT_DATA, SqlDbType.VarChar, auditInformation.AuditData),
                                                        
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_ENTITY_ID, SqlDbType.TinyInt, auditInformation.EntityId),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_ORIGINATION_ENTITY_ID , SqlDbType.TinyInt , auditInformation.OriginationEntityId ),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_AFFECTED_ID , SqlDbType.BigInt , auditInformation.AffectedId));
                }
                
            }
            catch (Exception)
            {
                //Dont send any response code.
                
            }
        }

        /// <summary>
        /// Logs the user audit information for the given event details.
        /// </summary>
        /// <param name="auditInformation">User audit information.</param>
        public void LogAuditInformation(AuditDataInformation auditDataInformation)
        {
            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                //Check whether the audit information exists or not.
                if (auditDataInformation != null)
                {
                    dataAccessLayer.ExecuteNonQuery(SP_AUDIT_DATA_LOG_INFO, ref responseCode,
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_TABLE_ID, SqlDbType.TinyInt, auditDataInformation.TableId.GetHashCode() ),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_SYSTEM_ID, SqlDbType.TinyInt, auditDataInformation.SystemId.GetHashCode()),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_STATE_ID, SqlDbType.TinyInt, auditDataInformation.StateId.GetHashCode()),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_EVENT_ID, SqlDbType.TinyInt, auditDataInformation.EventId.GetHashCode()),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_EVENT_TYPE_ID, SqlDbType.TinyInt, auditDataInformation.EventTypeId.GetHashCode()),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_ORIGINATION_EVENT_ID, SqlDbType.TinyInt, auditDataInformation.OriginationEventId.GetHashCode()),
                                                        
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_ORIGINATION_ENTITY_ID, SqlDbType.TinyInt, auditDataInformation.OriginationEntityId.GetHashCode()),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_ENTITY_ID, SqlDbType.TinyInt, auditDataInformation.EntityId.GetHashCode()),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_AFFECTED_ID, SqlDbType.BigInt, auditDataInformation.AffectedId),

                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_USER_ID, SqlDbType.BigInt, auditDataInformation.UserId),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_ORIGINAL_USER_ID, SqlDbType.BigInt, auditDataInformation.OriginalUserId),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_AUDIT_DETAIL, SqlDbType.VarChar, auditDataInformation.AuditDetail),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_AUDIT_DATA, SqlDbType.VarChar, auditDataInformation.AuditData));
                }

            }
            catch (Exception)
            {
                //Dont send any response code.

            }
        }

        
        /// <summary>
        /// Logs the user audit information for the given event details.
        /// </summary>
        /// <param name="auditInformation">User audit information.</param>
        public void LogBulkAuditInformation(AuditDataInformation auditDataInformation)
        {
            //This is for identifying the database operation execution status.
            int responseCode = 0;

            try
            {
                //Check whether the audit information exists or not.
                if (auditDataInformation != null)
                {
                    dataAccessLayer.ExecuteNonQuery(SP_BULK_AUDIT_DATA_LOG_INFO, ref responseCode,
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_TABLE_ID, SqlDbType.TinyInt, auditDataInformation.TableId.GetHashCode()),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_SYSTEM_ID, SqlDbType.TinyInt, auditDataInformation.SystemId.GetHashCode()),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_STATE_ID, SqlDbType.TinyInt, auditDataInformation.StateId.GetHashCode()),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_EVENT_ID, SqlDbType.TinyInt, auditDataInformation.EventId.GetHashCode()),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_EVENT_TYPE_ID, SqlDbType.TinyInt, auditDataInformation.EventTypeId.GetHashCode()),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_ORIGINATION_EVENT_ID, SqlDbType.TinyInt, auditDataInformation.OriginationEventId.GetHashCode()),

                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_ORIGINATION_ENTITY_ID, SqlDbType.TinyInt, auditDataInformation.OriginationEntityId.GetHashCode()),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_ENTITY_ID, SqlDbType.TinyInt, auditDataInformation.EntityId.GetHashCode()),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_AFFECTED_IDs, SqlDbType.VarChar, auditDataInformation.AffectedIds),

                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_USER_ID, SqlDbType.BigInt, auditDataInformation.UserId),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_ORIGINAL_USER_ID, SqlDbType.BigInt, auditDataInformation.OriginalUserId),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_AUDIT_DETAIL, SqlDbType.VarChar, auditDataInformation.AuditDetail),
                                                        dataAccessLayer.CreateParameter(SP_AUDIT_LOG_INFO_IN_PARAM_AUDIT_DATA, SqlDbType.VarChar, auditDataInformation.AuditData));
                }

            }
            catch (Exception)
            {
                //Dont send any response code.
            }
        }

        #endregion

        #region BeginLogAuditInformation

        /// <summary>
        /// Starts logging the user audit information for the given event details asynchronously.
        /// </summary>
        /// <param name="auditInformation">User audit information.</param>
        /// <param name="callback">References a method to be called when a user audit asynchronous operation completes.</param>
        /// <param name="state">Object state that needs to be called on asynchronous operation completion.</param>
        /// <returns>An IAsyncResult that represents the result of the asynchronous user audit operation.</returns>
        public IAsyncResult BeginLogAuditInformation(AuditInformation auditInformation, AsyncCallback callback, object state)
        {
            Action<AuditInformation> logAuditInformation = new Action<AuditInformation>(LogAuditInformation);

            return logAuditInformation.BeginInvoke(auditInformation, callback, state);
        }

        /// <summary>
        /// Starts logging the user audit information for the given event details asynchronously.
        /// </summary>
        /// <param name="auditInformation">User audit information.</param>
        /// <param name="callback">References a method to be called when a user audit asynchronous operation completes.</param>
        /// <param name="state">Object state that needs to be called on asynchronous operation completion.</param>
        /// <returns>An IAsyncResult that represents the result of the asynchronous user audit operation.</returns>
        public IAsyncResult BeginLogAuditInformation(AuditDataInformation auditDataInformation, AsyncCallback callback, object state)
        {
            Action<AuditDataInformation> logAuditInformation = new Action<AuditDataInformation>(LogAuditInformation);
            return logAuditInformation.BeginInvoke(auditDataInformation, callback, state);
        }

        /// <summary>
        /// Starts logging the user audit information for the given event details asynchronously.
        /// </summary>
        /// <param name="auditInformation">User audit information.</param>
        /// <param name="callback">References a method to be called when a user audit asynchronous operation completes.</param>
        /// <param name="state">Object state that needs to be called on asynchronous operation completion.</param>
        /// <returns>An IAsyncResult that represents the result of the asynchronous user audit operation.</returns>
        public IAsyncResult BeginLogAuditInformation(AuditDataInformation auditDataInformation,bool isBulkInsert, AsyncCallback callback, object state)
        {
            if (isBulkInsert && auditDataInformation.AffectedIds != null && auditDataInformation.AffectedIds != "")
            {
                Action<AuditDataInformation> logAuditInformation = new Action<AuditDataInformation>(LogBulkAuditInformation);
                return logAuditInformation.BeginInvoke(auditDataInformation, callback, state);
            }
            return null;
        }

        #endregion

        #region EndLogAuditInformation

        /// <summary>
        /// Ends the logging of user audit information.
        /// </summary>
        /// <param name="result">The IAsyncResult that represents a asynchronous user audit operation, returned when calling BeginInvoke.</param>
        public void EndLogAuditInformation(IAsyncResult result)
        {
            Action<AuditInformation> logAuditInformation = (result as AsyncResult).AsyncDelegate as Action<AuditInformation>;

            logAuditInformation.EndInvoke(result);
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

                    //Dispose the data access layer. 
                    if (dataAccessLayer != null)
                    {
                        dataAccessLayer.Dispose();
                        dataAccessLayer = null;
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
