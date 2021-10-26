using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using TF.FocusPay.EmailServer.ExceptionManager;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using TF.FocusPay.EmailServer.LogManager;
using System.Reflection;
using System.Transactions;

namespace TF.FocusPay.EmailServer.EmailDataLayer
{
    public class DLEmail
    {
        public DLEmail()
        {
        }

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
        private DatabaseHelper databaseHelperGetPriorities = null;

        /// <summary>
        /// Global DbConnection object
        /// </summary>
        /// 

        private DbConnection connectionGetQueueData = null;
        private DbConnection connectionGetPriorities = null;

        //private DataSet qrPriority = null;
        /// <summary>
        /// Global Database object
        /// </summary>

        private Database databaseGetQueueData = null; 
        private Database databaseGetPriorities = null;

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
            catch (SqlException sqlException)
            {
                if (!databaseHelperGetPriorities.GetConnectionState(connectionGetPriorities))
                {
                    connectionGetPriorities.Close();
                }
                throw sqlException;
            }
            catch (Exception exception)
            {
                if (!databaseHelperGetPriorities.GetConnectionState(connectionGetPriorities))
                {
                    connectionGetPriorities.Close();
                }
                throw exception;
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
    }
}
