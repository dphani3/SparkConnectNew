#region Header Information
/***********************************************************************************************************
NameSpace: IR.QueueReader.QueueReaderDataAccess
File: DatabaseHelper.cs
Class: DatabaseHelper
Author: Sarvesh.T.S
Created Date: 16-May-2008
Reviewed By: 
***********************************************************************************************************/
#endregion


#region Namespace Declaration

using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Collections;
using System.IO;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.Security;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Common;

#endregion

#region namespace TF.FocusPay.EmailServer.EmailDataLayer
namespace TF.FocusPay.EmailServer.EmailDataLayer
{
    #region DataBaseHelper Class
    /// <summary>
    /// DbHelper class provides methods for executing database commands. This class makes use of DataAccessApplication
    /// block to execute the database commands.
    /// </summary>

    internal class DatabaseHelper : IDisposable
    {

        #region Constructor

        /// <summary>
        /// This is a default constructor 
        /// </summary>
        public DatabaseHelper()
        {
        }

        #endregion

        #region Dispose

        public void Dispose()
        {

        }

        #endregion

        #region Execute DataSet

        /// <summary>
        /// This is an overloaded method. This method is used to retrieve recordset or dataset from the 
        /// database. It invokes ExecuteDataSet() method of DataAccessApplication block to talk to the database
        /// and retrieve the information.
        /// </summary>
        /// <param name="dbName">Name of the database for which, connection string will be obtained from the config file</param>
        /// <param name="spName">stored procedure name</param>
        /// <param name="dbInParameters">an array object which contains the input parametes to the stored procedure</param>
        /// <returns>A dataset with information retrieved from the database</returns>
        public DataSet ExecuteDataSet(string dbName, string spName, object[] dbInParameters)
        {
            DataSet recordSet = null;
            try
            {
                Database db = DatabaseFactory.CreateDatabase(dbName);
                recordSet = db.ExecuteDataSet(spName, dbInParameters);
            }
            catch (SqlException sqlExp)
            {
                throw sqlExp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return recordSet;
        }

        /// <summary>
        /// This is an overloaded method. This method is used to retrieve recordset or dataset from the 
        /// database. It invokes ExecuteDataSet() method of DataAccessApplication block to talk to the database
        /// and retrieve the information.
        /// </summary>
        /// <param name="dbName">Name of the database for which, connection string will be obtained from the config file</param>
        /// <param name="spName">stored procedure name</param>
        /// <returns>A dataset with information retrieved from the database</returns>
        public DataSet ExecuteDataSet(string dbName, string spName)
        {
            DataSet recordSet = null;
            Database db = DatabaseFactory.CreateDatabase(dbName);
            recordSet = db.ExecuteDataSet(spName);
            return recordSet;
        }

        /// <summary>
        /// This is an overloaded method. This method is used to retrieve recordset or dataset from the 
        /// database. It invokes ExecuteDataSet() method of DataAccessApplication block to talk to the database
        /// and retrieve the information.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(Database database, DbCommand command)
        {
            DataSet recordSet = null;
            recordSet = database.ExecuteDataSet(command);
            return recordSet;
        }

        #endregion Execute DataSet

        #region Execute Scalar

        public object ExecuteScalar(string dbName, string spName, object[] parameters)
        {
            object returnValue;
            Database db = DatabaseFactory.CreateDatabase(dbName);
            try
            {
                returnValue = db.ExecuteScalar(spName, parameters);
            }
            catch (SqlException sqlExp)
            {
                throw sqlExp;
            }
            return returnValue;
        }

        #endregion

        #region Load DataSet
        /// <summary>
        /// This method is used to retrieve information
        /// </summary>
        /// <param name="dbName">Unique ID that represents the database name</param>
        /// <param name="spName">Stored Procedure name</param>
        /// <param name="tableName">String array that consists of table name to which data needs to be populated</param>
        /// <param name="clientID">Unique id that represents a client</param>
        /// <returns></returns>
        public DataSet LoadDataSet(string dbName, string spName, string[] tableName, object[] spParams)
        {
            DataSet dataSet = new DataSet();
            try
            {
                Database dataBase = DatabaseFactory.CreateDatabase(dbName);
                DbCommand selectCommand = dataBase.GetStoredProcCommand(spName);
                dataBase.LoadDataSet(spName, dataSet, tableName, spParams);
            }
            catch (SqlException sqlExp)
            {
                throw sqlExp;
            }
            finally
            {

            }
            return dataSet;
        }


        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// This method is used to update a  recordset or dataset to the database. It invokes ExecuteNonQuery()
        ///  method of DataAccessApplication block to talk to the database and update the information.
        /// </summary>
        /// <param name="dbName">Name of the database for which, connection string will be obtained from the config file</param>
        /// <param name="spName">stored procedure name</param>
        /// <param name="objParams">an array object which contains the input parametes to the stored procedure</param>
        /// <returns>A boolean value indicating whether the transaction is successfull or not</returns>
        public bool ExecuteNonQuery(string dbName, string spName, object[] objParams)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase(dbName);

                int i = db.ExecuteNonQuery(spName, objParams);

                if (i > 0)
                    return true;
                else
                    return false;
            }
            catch (SqlException sqlExp)
            {
                throw sqlExp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="databaseCommand"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(Database database, DbCommand databaseCommand, DbTransaction transaction)
        {
            int rowsAffected;
            try
            {
                rowsAffected = database.ExecuteNonQuery(databaseCommand, transaction);
            }
            catch (SqlException sqlExp)
            {
                throw sqlExp;
            }
            return rowsAffected;
        }


        public int ExecuteNonQuery(Database database, DbCommand command)
        {
            int result = 0;
            try
            {
                result = database.ExecuteNonQuery(command);
            }
            catch (SqlException sqlExp)
            {
                throw sqlExp;
            }
            return result;
        }
        #endregion ExecuteNonQuery

        #region Create Database
        /// <summary>
        /// This method is used to create the database and get the connection string from tyhe config file
        /// </summary>
        /// <param name="databaseName">Key value for which connection string needs to be obtained</param>
        /// <returns>A database object with connection string</returns>
        public Database CreateDatabase(string databaseName)
        {
            Database database = null;
            try
            {
                database = DatabaseFactory.CreateDatabase(databaseName);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return database;
        }

        #endregion

        #region Create Connection
        /// <summary>
        /// This method is used to create a connection with the database for the connection string
        /// </summary>
        /// <param name="database">Database object with connection string in it</param>
        /// <returns>A DbConnection object</returns>
        public DbConnection CreateConnection(Database database)
        {
            DbConnection connection = null;
            try
            {
                connection = database.CreateConnection();
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return connection;
        }

        #endregion

        #region Create Transaction
        /// <summary>
        /// This method is used to Create a Transaction object for the connection.
        /// </summary>
        /// <param name="connection">Connection object with connection string</param>
        /// <returns>A Transaction Object</returns>
        public DbTransaction CreateTransaction(DbConnection connection)
        {
            DbTransaction transaction = null;
            try
            {
                transaction = connection.BeginTransaction();
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return transaction;
        }

        #endregion

        #region Get Connection State
        /// <summary>
        /// This method is used to check the state of the connection.
        /// </summary>
        /// <param name="connection">DB Connection object</param>
        /// <returns>A boolean value if the connection is closed or open</returns>
        public bool GetConnectionState(DbConnection connection)
        {
            bool isConnectionClosed = false;
            //added by Sarvesh
            if (connection == null)
            {
                isConnectionClosed = true;
            }
            ///23-05-2008
            if (connection.State == ConnectionState.Closed)
            {
                isConnectionClosed = true;
            }
            else
            {
                isConnectionClosed = false;
            }
            return isConnectionClosed;
        }
        #endregion

        #region Create Command
        /// <summary>
        /// This method is used to create the database command and also sets the command timeout property
        /// </summary>
        /// <param name="database"></param>
        /// <param name="connection"></param>
        public DbCommand CreateCommand(Database database, DbConnection connection, string spName)
        {
            int cmdTimeoutValue = 0;
            DbCommand command;
            try
            {
                cmdTimeoutValue = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeOutValue"]) * 1000;
                command = database.GetStoredProcCommand(spName);
                command.CommandTimeout = cmdTimeoutValue;
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = connection;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return command;
        }
        #endregion

    }
    #endregion
}
#endregion