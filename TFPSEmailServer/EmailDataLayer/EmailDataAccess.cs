#region Header Information
/***********************************************************************************************************
NameSpace: IR.QueueReader.QueueReaderDataAccess
File: QueueReaderDataAccess.cs
Class: QueueReaderDataAccess
Author: Sarvesh.T.S
Created Date: 16-May-2008
Reviewed By: 
***********************************************************************************************************/
#endregion

#region Namspace declaration

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using TF.FocusPay.EmailServer.LogManager;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Reflection;
using System.Transactions;

#endregion

namespace TF.FocusPay.EmailServer.EmailDataLayer
{
    public class EmailDataAccess
    {
        #region Global Variables
        /// <summary>
        /// Global varibale to store Namespace name
        /// </summary>
        public static string DataAccessNameSpace = MethodBase.GetCurrentMethod().DeclaringType.ToString();
        /// <summary>
        /// Global DatabaseHelper object
        /// </summary>
        //private DatabaseHelper databaseHelper = null;

        private DatabaseHelper databaseHelperGetQueueData = null;
        private DatabaseHelper databaseHelperUpdateQueueData = null;
        private DatabaseHelper databaseHelperUpdateWorkInProgress = null;
        private DatabaseHelper databaseHelperGetPriorities = null;

        /// <summary>
        /// Global DbConnection object
        /// </summary>
        /// 

        private DbConnection connectionGetQueueData = null;
        private DbConnection connectionUpdateQueueData = null;
        private DbConnection connectionUpdateWorkInProgress = null;
        private DbConnection connectionGetPriorities = null;

        //private DataSet qrPriority = null;
        /// <summary>
        /// Global Database object
        /// </summary>

        private Database databaseGetQueueData = null;
        private Database databaseUpdateQueueData = null;
        private Database databaseUpdateWorkInProgress = null;
        private Database databaseGetPriorities = null;


        private int resultUpdateQueueData = 0;
        private int resultUpdateWorkInProgress = 0;

        #endregion Global Variables

        #region Getting the Priority From Lookup
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet GetPriorities()
        {
            databaseHelperGetPriorities = new DatabaseHelper();
            DataSet qrPriority = new DataSet();
            DbCommand commandGetPriorities = null;
            databaseGetPriorities = databaseHelperGetPriorities.CreateDatabase(DataBaseConstants.DATABASE_NAME);
            connectionGetPriorities = databaseHelperGetPriorities.CreateConnection(databaseGetPriorities);

            qrPriority.Clear();

            try
            {
                if (databaseHelperGetPriorities.GetConnectionState(connectionGetPriorities))
                {
                    connectionGetPriorities.Open();
                }

                commandGetPriorities = databaseHelperGetPriorities.CreateCommand(databaseGetPriorities, connectionGetPriorities, DataBaseConstants.PRC_PRIORITY);
                qrPriority = databaseHelperGetPriorities.ExecuteDataSet(databaseGetPriorities, commandGetPriorities);

            }
            catch (SqlException)
            {
                if (!databaseHelperGetPriorities.GetConnectionState(connectionGetPriorities))
                {
                    connectionGetPriorities.Close();
                }
                throw;
            }
            catch (Exception)
            {
                if (!databaseHelperGetPriorities.GetConnectionState(connectionGetPriorities))
                {
                    connectionGetPriorities.Close();
                }
                throw;
            }
            finally
            {
                if (!databaseHelperGetPriorities.GetConnectionState(connectionGetPriorities))
                {
                    connectionGetPriorities.Close();
                }
                databaseHelperGetPriorities = null;
                connectionGetPriorities = null;
                databaseGetPriorities = null;
            }
            return qrPriority;
        }

        #endregion

        #region Getting the Order From Queue
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Priority"></param>
        /// <returns></returns>
        public DataSet GetOrderFromQueue(int Priority)
        {
            databaseHelperGetQueueData = new DatabaseHelper();
            DataSet qrQueueData = new DataSet();
            DbCommand commandGetQueueData = null;
            databaseGetQueueData = databaseHelperGetQueueData.CreateDatabase(DataBaseConstants.DATABASE_NAME);
            connectionGetQueueData = databaseHelperGetQueueData.CreateConnection(databaseGetQueueData);

            qrQueueData.Clear();

            try
            {
                if (databaseHelperGetQueueData.GetConnectionState(connectionGetQueueData))
                {
                    connectionGetQueueData.Open();
                }

                commandGetQueueData = databaseHelperGetQueueData.CreateCommand(databaseGetQueueData, connectionGetQueueData, DataBaseConstants.PRC_PRIORITY_QUEUE_INFORMATION);

                databaseGetQueueData.AddParameter(commandGetQueueData, "@Priority", DbType.Int32, ParameterDirection.Input,
                                                               "Priority", DataRowVersion.Current, Priority);

                qrQueueData = databaseHelperGetQueueData.ExecuteDataSet(databaseGetQueueData, commandGetQueueData);
            }
            catch (SqlException sqlException)
            {
                if (!databaseHelperGetQueueData.GetConnectionState(connectionGetQueueData))
                {
                    connectionGetQueueData.Close();
                }
                throw sqlException;
            }
            catch (Exception exception)
            {
                if (!databaseHelperGetQueueData.GetConnectionState(connectionGetQueueData))
                {
                    connectionGetQueueData.Close();
                }
                throw exception;
            }
            finally
            {
                if (!databaseHelperGetQueueData.GetConnectionState(connectionGetQueueData))
                {
                    connectionGetQueueData.Close();
                }
                databaseHelperGetQueueData = null;
                connectionGetQueueData = null;
                databaseGetQueueData = null;
            }
            return qrQueueData;
        }

        #endregion

        # region Updating the Order Status
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueID"></param>
        /// <param name="responseCode"></param>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        public int UpdateOrderStatus(long queueID, int priority, int statusID, Boolean lastRetryFlag)
        {
            databaseHelperUpdateQueueData = new DatabaseHelper();
            DbCommand commandUpdateQueueData = null;
            databaseUpdateQueueData = databaseHelperUpdateQueueData.CreateDatabase(DataBaseConstants.DATABASE_NAME);
            connectionUpdateQueueData = databaseHelperUpdateQueueData.CreateConnection(databaseUpdateQueueData);
            resultUpdateQueueData = 0;
            try
            {
                if (databaseHelperUpdateQueueData.GetConnectionState(connectionUpdateQueueData))
                {
                    connectionUpdateQueueData.Open();
                }

                commandUpdateQueueData = databaseHelperUpdateQueueData.CreateCommand(databaseUpdateQueueData, connectionUpdateQueueData, DataBaseConstants.PRC_UPDATE_QUEUE_STATUS);

                databaseUpdateQueueData.AddParameter(commandUpdateQueueData, "@QueueID", DbType.Int64, ParameterDirection.Input,
                                                               "QueueID", DataRowVersion.Current, queueID);

                databaseUpdateQueueData.AddParameter(commandUpdateQueueData, "@Priority", DbType.Int32, ParameterDirection.Input,
                                                               "Priority", DataRowVersion.Current, priority);

                databaseUpdateQueueData.AddParameter(commandUpdateQueueData, "@RowStatusID", DbType.Int32, ParameterDirection.Input,
                                                               "Status", DataRowVersion.Current, statusID);

                databaseUpdateQueueData.AddParameter(commandUpdateQueueData, "@LastRetryFlag", DbType.Boolean, ParameterDirection.Input,
                                                               "LastRetryFlag", DataRowVersion.Current, lastRetryFlag);

                
                resultUpdateQueueData = databaseHelperUpdateQueueData.ExecuteNonQuery(databaseUpdateQueueData, commandUpdateQueueData);

            }
            catch (SqlException sqlException)
            {
                if (!databaseHelperUpdateQueueData.GetConnectionState(connectionUpdateQueueData))
                {
                    connectionUpdateQueueData.Close();
                }
                throw sqlException;
            }
            catch (Exception exception)
            {
                if (!databaseHelperUpdateQueueData.GetConnectionState(connectionUpdateQueueData))
                {
                    connectionUpdateQueueData.Close();
                }
                throw exception;
            }
            finally
            {
                if (!databaseHelperUpdateQueueData.GetConnectionState(connectionUpdateQueueData))
                {
                    connectionUpdateQueueData.Close();
                }
                databaseHelperUpdateQueueData = null;
                connectionUpdateQueueData = null;
                databaseUpdateQueueData = null;
            }
            return resultUpdateQueueData;
        }
        #endregion

        # region Updating the Work in Progress Status
        /// <summary>
        /// 
        /// </summary>
        /// <param name="QueueID"></param>
        /// <param name="RowStatus"></param>
        /// <param name="WorkInProgressMessage"></param>
        /// <returns></returns>
        public int UpdateWorkInProgress(long QueueID, int RowStatus, string WorkInProgressMessage)
        {
            databaseHelperUpdateWorkInProgress = new DatabaseHelper();
            DbCommand commandUpdateWorkInProgress = null;
            databaseUpdateWorkInProgress = databaseHelperUpdateWorkInProgress.CreateDatabase(DataBaseConstants.DATABASE_NAME);
            connectionUpdateWorkInProgress = databaseHelperUpdateWorkInProgress.CreateConnection(databaseUpdateWorkInProgress);
            resultUpdateWorkInProgress = 0;
            try
            {
                if (databaseHelperUpdateWorkInProgress.GetConnectionState(connectionUpdateWorkInProgress))
                {
                    connectionUpdateWorkInProgress.Open();
                }

                commandUpdateWorkInProgress = databaseHelperUpdateWorkInProgress.CreateCommand(databaseUpdateWorkInProgress, connectionUpdateWorkInProgress, DataBaseConstants.PRC_UPDATE_QUEUE_STATUS);

                databaseUpdateWorkInProgress.AddParameter(commandUpdateWorkInProgress, "@QueueID", DbType.Int64, ParameterDirection.Input,
                                                               "QueueID", DataRowVersion.Current, QueueID);

                databaseUpdateWorkInProgress.AddParameter(commandUpdateWorkInProgress, "@StatusID", DbType.Int32, ParameterDirection.Input,
                                                               "RowStatusID", DataRowVersion.Current, RowStatus);

                databaseUpdateWorkInProgress.AddParameter(commandUpdateWorkInProgress, "@ResponseMessage", DbType.String, ParameterDirection.Input,
                                                               "ResponseMessage", DataRowVersion.Current, WorkInProgressMessage);

                resultUpdateWorkInProgress = databaseHelperUpdateWorkInProgress.ExecuteNonQuery(databaseUpdateWorkInProgress, commandUpdateWorkInProgress);

            }
            catch (SqlException sqlException)
            {
                if (!databaseHelperUpdateWorkInProgress.GetConnectionState(connectionUpdateWorkInProgress))
                {
                    connectionUpdateWorkInProgress.Close();
                }
                throw sqlException;
            }
            catch (Exception exception)
            {
                if (!databaseHelperUpdateWorkInProgress.GetConnectionState(connectionUpdateWorkInProgress))
                {
                    connectionUpdateWorkInProgress.Close();
                }
                throw exception;
            }
            finally
            {
                if (!databaseHelperUpdateWorkInProgress.GetConnectionState(connectionUpdateWorkInProgress))
                {
                    connectionUpdateWorkInProgress.Close();
                }
                databaseHelperUpdateWorkInProgress = null;
                connectionUpdateWorkInProgress = null;
                databaseUpdateWorkInProgress = null;
            }
            return resultUpdateWorkInProgress;
        }
        #endregion


    }
}

