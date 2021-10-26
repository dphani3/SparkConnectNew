                                
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: DAL.cs
  Description: This is the data access layer for all audit logger operations.
  Date Created : 25-Oct-2010
  Revision History: 
  */

#endregion

#region Namespaces

using System;
using System.Data;
using System.Data.SqlClient;

#endregion

namespace TF.FocusPay.AuditLogger.DataAccessLayer
{
    #region DAL

    /// <summary>
    /// This is the data access layer for all audit logger operations.
    /// </summary>
    internal class DAL : IDisposable
    {
        #region Member Variables

        private bool isDisposed = false;                                    //This is for Garbage Collector.
        private readonly string CONNECTION_STRING = string.Empty;           //Database connection string information.

        #endregion        
        
        #region Constructor

        /// <summary>
        /// This is the Parameterized Constructor for the class which will assign the connection string value to the data access layer.
        /// </summary>
        /// <param name="connectionStringName">Database connection string information.</param>
        internal DAL(string connectionStringName)
        {
            if (!String.IsNullOrEmpty(connectionStringName))
                this.CONNECTION_STRING = connectionStringName.Trim();
        }

        #endregion

        #region Destructor

        /// <summary>
        /// This is the Destructor for the class.
        /// </summary>
        ~DAL()
        {
            //Since finalizer is called, there is no need to free the managed resources. So, false is passed.
            Dispose(false);
        }

        #endregion        

        #region GetDataSet

        /// <summary>
        /// Gets the dataset that is generated after executing the stored procedure with provided parameters.
        /// </summary>
        /// <param name="storedProcedureName">Stored Procedure name.</param>
        /// <param name="responseCode">Response code that details the operation status.</param>
        ///     <list type="bullet">
        ///         <item>
        ///             <description>0 -> Success.</description>
        ///         </item>
        ///         <item>
        ///             <description>-1 -> Database Error.</description>
        ///         </item>
        ///         <item>
        ///             <description>-2 -> System Error.</description>
        ///         </item>
        ///     </list>
        /// <param name="parameters">Parameters for the stored procedure.</param>
        /// <returns>DataSet that has been generated on executing the stored procedure with given parameters.</returns>
        internal DataSet GetDataSet(string storedProcedureName, ref int responseCode, params IDataParameter[] parameters)
        {
            DataSet dataset = null;
            SqlConnection connection = null;

            //If the strored procedure name is not provided, return empty dataset.
            if (String.IsNullOrEmpty(storedProcedureName))
                return null;
            else
            {
                try
                {
                    //Zero indicates success.
                    responseCode = 0;

                    //Create the sql command with given stored procedure name.
                    using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName))
                    {
                        //Indicate that the type of command is stored procedure.
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        //Add the parameters to the sql command.
                        if (parameters != null && parameters.Length > 0)
                        {
                            for (int index = 0; index < parameters.Length; index++)
                            {
                                sqlCommand.Parameters.Add(parameters[index]);
                            }
                        }

                        //Open the database connection with provided connection string information.
                        using (sqlCommand.Connection = connection = OpenConnection(CONNECTION_STRING))
                        {
                            //Get the database values to the adapter and fill the dataset with adapter values.
                            using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
                            {
                                dataAdapter.SelectCommand = sqlCommand;

                                dataset = new DataSet();
                                dataAdapter.Fill(dataset);
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    //-1 indicates database exception.
                    responseCode = -1;
                    
                    if (dataset != null)
                    {
                        dataset.Dispose();
                        dataset = null;
                    }
                }
                catch (Exception)
                {                    
                    //-2 indicates system exception.
                    responseCode = -2;
                    
                    if (dataset != null)
                    {
                        dataset.Dispose();
                        dataset = null;
                    }
                }
                finally
                {
                    //Close the database connection;
                    CloseConnection(connection);
                }
            }

            //Return the dataset that contains the database values.
            return dataset;
        }

        #endregion

        #region GetDataReader

        /// <summary>
        ///  Gets the data reader that is generated after executing the stored procedure with provided parameters.
        /// </summary>
        /// <param name="storedProcedureName">Stored Procedure name.</param>
        /// <param name="responseCode">Response code that details the operation status.</param>
        ///     <list type="bullet">
        ///         <item>
        ///             <description>0 -> Success.</description>
        ///         </item>
        ///         <item>
        ///             <description>-1 -> Database Error.</description>
        ///         </item>
        ///         <item>
        ///             <description>-2 -> System Error.</description>
        ///         </item>
        ///     </list>
        /// <param name="parameters">Parameters for the stored procedure.</param>
        /// <returns>SqlDataReader that has been generated on executing the stored procedure with given parameters.</returns>
        internal SqlDataReader GetDataReader(string storedProcedureName, ref int responseCode, params IDataParameter[] parameters)
        {
            SqlDataReader dataReader = null;

            //If the strored procedure name is not provided, return empty SqlDataReader.
            if (String.IsNullOrEmpty(storedProcedureName))
                return null;
            else
            {
                try
                {
                    //Zero indicates success.
                    responseCode = 0;

                    //Create the sql command with given stored procedure name.
                    using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName))
                    {
                        //Indicate that the type of command is stored procedure.
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        //Add the parameters to the sql command.
                        if (parameters != null && parameters.Length > 0)
                        {
                            for (int index = 0; index < parameters.Length; index++)
                            {
                                sqlCommand.Parameters.Add(parameters[index]);
                            }
                        }

                        //Open the database connection with provided connection string information.
                        sqlCommand.Connection = OpenConnection(CONNECTION_STRING);

                        //Get the database values to the data reader.
                        dataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                }
                catch (SqlException)
                {   
                    //-1 indicates database exception.
                    responseCode = -1;
                    
                    if (dataReader != null)
                    {
                        dataReader.Close();
                        dataReader.Dispose();
                        dataReader = null;
                    }
                }
                catch (Exception)
                {
                    //-2 indicates system exception.
                    responseCode = -2;
                    
                    if (dataReader != null)
                    {
                        dataReader.Close();
                        dataReader.Dispose();
                        dataReader = null;
                    }
                }
            }

            //Return the data reader that contains the database values.
            return dataReader;
        }

        #endregion

        #region GetSqlCommand

        /// <summary>
        /// Gets the sql command that contains the values for output parameters of the stored procedure after execution.
        /// </summary>
        /// <param name="storedProcedureName">Stored Procedure name.</param>
        /// <param name="responseCode">Response code that details the operation status.</param>
        ///     <list type="bullet">
        ///         <item>
        ///             <description>0 -> Success.</description>
        ///         </item>
        ///         <item>
        ///             <description>-1 -> Database Error.</description>
        ///         </item>
        ///         <item>
        ///             <description>-2 -> System Error.</description>
        ///         </item>
        ///     </list>
        /// <param name="parameters">Parameters for the stored procedure.</param>
        /// <returns>Sql Command that contains the values for output parameters of the stored procedure after execution.</returns>
        internal SqlCommand GetSqlCommand(string storedProcedureName, ref int responseCode, params IDataParameter[] parameters)
        {
            SqlCommand sqlCommand = null;
            SqlConnection connection = null;

            //If the strored procedure name is not provided, return empty SqlCommand.
            if (String.IsNullOrEmpty(storedProcedureName))
                return null;
            else
            {
                try
                {
                    //Zero indicates success.
                    responseCode = 0;

                    //Create the sql command with given stored procedure name.
                    sqlCommand = new SqlCommand(storedProcedureName);

                    //Indicate that the type of command is stored procedure.
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    //Add the parameters to the sql command.
                    if (parameters != null && parameters.Length > 0)
                    {
                        for (int index = 0; index < parameters.Length; index++)
                        {
                            sqlCommand.Parameters.Add(parameters[index]);
                        }
                    }

                    //Open the database connection with provided connection string information.
                    sqlCommand.Connection = connection = OpenConnection(CONNECTION_STRING);

                    //Execute the stored procedure.
                    sqlCommand.ExecuteNonQuery();
                }
                catch (SqlException)
                { 
                    //-1 indicates database exception.
                    responseCode = -1;

                    if (sqlCommand != null)
                    {
                        sqlCommand.Dispose();
                        sqlCommand = null;
                    }
                }
                catch (Exception)
                {
                    //-2 indicates system exception.
                    responseCode = -2;

                    if (sqlCommand != null)
                    {
                        sqlCommand.Dispose();
                        sqlCommand = null;
                    }
                }
                finally
                {
                    //Close the database connection;
                    CloseConnection(connection);
                }
            }

            //Return the sql command that contains the output parameters values.
            return sqlCommand;
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// Executes the stored procedure with provided parameters and returns the number of rows affected.
        /// </summary>
        /// <param name="storedProcedureName">Stored Procedure name.</param>
        /// <param name="responseCode">Response code that details the operation status.</param>
        ///     <list type="bullet">
        ///         <item>
        ///             <description>0 -> Success.</description>
        ///         </item>
        ///         <item>
        ///             <description>-1 -> Database Error.</description>
        ///         </item>
        ///         <item>
        ///             <description>-2 -> System Error.</description>
        ///         </item>
        ///     </list>
        /// <param name="parameters">Parameters for the stored procedure.</param>
        /// <returns>Number of rows affected on executing the given stored procedure.</returns>
        internal int ExecuteNonQuery(string storedProcedureName, ref int responseCode, params IDataParameter[] parameters)
        {
            int rowsEffected = 0;
            SqlConnection connection = null;

            //If the strored procedure name is not provided, return zero rows as affected.
            if (String.IsNullOrEmpty(storedProcedureName))
                return rowsEffected;
            else
            {
                try
                {
                    //Zero indicates success.
                    responseCode = 0;

                    //Create the sql command with given stored procedure name.
                    using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName))
                    {
                        //Indicate that the type of command is stored procedure.
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        //Add the parameters to the sql command.
                        if (parameters != null && parameters.Length > 0)
                        {
                            for (int index = 0; index < parameters.Length; index++)
                            {
                                sqlCommand.Parameters.Add(parameters[index]);
                            }
                        }

                        //Open the database connection with provided connection string information.
                        using (sqlCommand.Connection = connection = OpenConnection(CONNECTION_STRING))
                        {
                            //Execute the stored procedure with provided parameters and get the number of rows affected.
                            rowsEffected = sqlCommand.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException)
                {
                    //-1 indicates database exception.
                    responseCode = -1;
                    rowsEffected = 0;
                }
                catch (Exception)
                {
                    //-2 indicates system exception.
                    responseCode = -2;
                    rowsEffected = 0;
                }
                finally
                {
                    //Close the database connection;
                    CloseConnection(connection);
                }
            }

            //Return the number of rows affected.
            return rowsEffected;
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes the stored procedure with provided parameters and returns the first column of the first row in the result set returned 
        /// by the stored procedure.
        /// </summary>
        /// <param name="storedProcedureName">Stored Procedure name.</param>
        /// <param name="responseCode">Response code that details the operation status.</param>
        ///     <list type="bullet">
        ///         <item>
        ///             <description>0 -> Success.</description>
        ///         </item>
        ///         <item>
        ///             <description>-1 -> Database Error.</description>
        ///         </item>
        ///         <item>
        ///             <description>-2 -> System Error.</description>
        ///         </item>
        ///     </list>
        /// <param name="parameters">Parameters for the stored procedure.</param>
        /// <returns>Object that represents the first column of the first row in the result set returned by the stored procedure.</returns>
        internal object ExecuteScalar(string storedProcedureName, ref int responseCode, params IDataParameter[] parameters)
        {
            object scalarValue = null;
            SqlConnection connection = null;

            //If the strored procedure name is not provided, return null.
            if (String.IsNullOrEmpty(storedProcedureName))
                return scalarValue;
            else
            {
                try
                {
                    //Zero indicates success.
                    responseCode = 0;

                    //Create the sql command with given stored procedure name.
                    using (SqlCommand sqlCommand = new SqlCommand(storedProcedureName))
                    {
                        //Indicate that the type of command is stored procedure.
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        //Add the parameters to the sql command.
                        if (parameters != null && parameters.Length > 0)
                        {
                            for (int index = 0; index < parameters.Length; index++)
                            {
                                sqlCommand.Parameters.Add(parameters[index]);
                            }
                        }

                        //Open the database connection with provided connection string information.
                        using (sqlCommand.Connection = connection = OpenConnection(CONNECTION_STRING))
                        {
                            //Execute the stored procedure with provided parameters and get the scalar value.
                            scalarValue = sqlCommand.ExecuteScalar();
                        }
                    }
                }
                catch (SqlException)
                {
                    //-1 indicates database exception.
                    responseCode = -1;
                    scalarValue = null;
                }
                catch (Exception)
                {
                    //-2 indicates system exception.
                    responseCode = -2;
                    scalarValue = null;
                }
                finally
                {
                    //Close the database connection;
                    CloseConnection(connection);
                }
            }

            //Return the first column of the first row in the result.
            return scalarValue;
        }

        #endregion

        #region CreateParameter Overloaded Methods

        /// <summary>
        /// Constructs the sql parameter for the stored procedure with the given parameter name, type and value.
        /// </summary>
        /// <param name="parameterName">Parameter Name.</param>
        /// <param name="parameterType">Parameter Type.</param>
        /// <param name="parameterValue">Parameter Value.</param>
        /// <returns>Sql Parameter.</returns>
        internal SqlParameter CreateParameter(string parameterName, SqlDbType parameterType, object parameterValue)
        {
            SqlParameter sqlParameter = new SqlParameter(parameterName, parameterType);

            //Check the boundary conditions and emptiness of the parameter values.
            if (parameterValue != DBNull.Value)
            {
                switch (parameterType)
                {
                    case SqlDbType.VarChar:
                    case SqlDbType.NVarChar:
                    //case SqlDbType.Char: //TBD: Need more observation.
                    case SqlDbType.NChar:
                    case SqlDbType.Text:
                        parameterValue = CheckParamValue((string)parameterValue);
                        break;
                    case SqlDbType.DateTime:
                        parameterValue = CheckParamValue((DateTime)parameterValue);
                        break;
                    case SqlDbType.Int:
                        parameterValue = CheckParamValue((int)parameterValue);
                        break;
                    case SqlDbType.UniqueIdentifier:
                        parameterValue = CheckParamValue(GetGuid(parameterValue));
                        break;
                    case SqlDbType.Bit:
                        if (parameterValue is bool) parameterValue = (int)((bool)parameterValue ? 1 : 0);
                        if ((int)parameterValue < 0 || (int)parameterValue > 1) parameterValue = int.MinValue;
                        parameterValue = CheckParamValue((int)parameterValue);
                        break;
                    case SqlDbType.Float:
                        parameterValue = CheckParamValue((float)(parameterValue));
                        break;
                    case SqlDbType.Decimal:
                        parameterValue = CheckParamValue((decimal)parameterValue);
                        break;
                    case SqlDbType.VarBinary:
                        parameterValue = CheckParamValue((byte[])parameterValue);
                        break;
                }
            }

            sqlParameter.Value = parameterValue;

            return sqlParameter;
        }

        /// <summary>
        /// Constructs the sql parameter for the stored procedure with the given parameter name, type and direction.
        /// </summary>
        /// <param name="parameterName">Parameter Name.</param>
        /// <param name="parameterType">Parameter Type.</param>
        /// <param name="direction">Parameter Direction.</param>
        /// <returns>Sql Parameter.</returns>
        internal SqlParameter CreateParameter(string parameterName, SqlDbType parameterType, ParameterDirection direction)
        {
            SqlParameter sqlParameter = CreateParameter(parameterName, parameterType, DBNull.Value);
            sqlParameter.Direction = direction;

            return sqlParameter;
        }

        /// <summary>
        /// Constructs the sql parameter for the stored procedure with the given parameter name, type, value and direction.
        /// </summary>
        /// <param name="parameterName">Parameter Name.</param>
        /// <param name="parameterType">Parameter Type.</param>
        /// <param name="parameterValue">Parameter Value.</param>
        /// <param name="direction">Parameter Direction.</param>
        /// <returns>Sql Parameter.</returns>
        internal SqlParameter CreateParameter(string parameterName, SqlDbType parameterType, object parameterValue, ParameterDirection direction)
        {
            SqlParameter sqlParameter = CreateParameter(parameterName, parameterType, parameterValue);
            sqlParameter.Direction = direction;

            return sqlParameter;
        }

        /// <summary>
        /// Constructs the sql parameter for the stored procedure with the given parameter name, type, value and size.
        /// </summary>
        /// <param name="parameterName">Parameter Name.</param>
        /// <param name="parameterType">Parameter Type.</param>
        /// <param name="parameterValue">Parameter Value.</param>
        /// <param name="size">Parameter Size.</param>
        /// <returns>Sql Parameter.</returns>
        internal SqlParameter CreateParameter(string parameterName, SqlDbType parameterType, object parameterValue, int size)
        {
            SqlParameter sqlParameter = CreateParameter(parameterName, parameterType, parameterValue);
            sqlParameter.Size = size;

            return sqlParameter;
        }

        /// <summary>
        /// Constructs the sql parameter for the stored procedure with the given parameter name, type, size and direction.
        /// </summary>
        /// <param name="parameterName">Parameter Name.</param>
        /// <param name="parameterType">Parameter Type.</param>
        /// <param name="size">Parameter Size.</param>
        /// <param name="direction">Parameter Direction.</param>
        /// <returns>Sql Parameter.</returns>
        internal SqlParameter CreateParameter(string parameterName, SqlDbType parameterType, int size, ParameterDirection direction)
        {
            SqlParameter sqlParameter = CreateParameter(parameterName, parameterType, DBNull.Value);
            sqlParameter.Direction = direction;            
            sqlParameter.Size = size;

            return sqlParameter;
        }

        /// <summary>
        /// Constructs the sql parameter for the stored procedure with the given parameter name, type, value, size and direction.
        /// </summary>
        /// <param name="parameterName">Parameter Name.</param>
        /// <param name="parameterType">Parameter Type.</param>
        /// <param name="parameterValue">Parameter Value.</param>
        /// <param name="size">Parameter Size.</param>
        /// <param name="direction">Parameter Direction.</param>
        /// <returns>Sql Parameter.</returns>
        internal SqlParameter CreateParameter(string parameterName, SqlDbType parameterType, object parameterValue, int size, ParameterDirection direction)
        {
            SqlParameter sqlParameter = CreateParameter(parameterName, parameterType, parameterValue);
            sqlParameter.Direction = direction;
            sqlParameter.Size = size;

            return sqlParameter;
        }

        /// <summary>
        /// Constructs the sql parameter for the stored procedure with the given parameter name, type, value, size and precision.
        /// </summary>
        /// <param name="parameterName">Parameter Name.</param>
        /// <param name="parameterType">Parameter Type.</param>
        /// <param name="parameterValue">Parameter Value.</param>
        /// <param name="size">Parameter Size.</param>
        /// <param name="precision">Parameter Precision.</param>
        /// <returns>Sql Parameter.</returns>
        internal SqlParameter CreateParameter(string parameterName, SqlDbType parameterType, object parameterValue, int size, byte precision)
        {
            SqlParameter sqlParameter = CreateParameter(parameterName, parameterType, parameterValue);
            sqlParameter.Size = size;
            sqlParameter.Precision = precision;

            return sqlParameter;
        }

        /// <summary>
        /// Constructs the sql parameter for the stored procedure with the given parameter name, type, value, size, precision and direction.
        /// </summary>
        /// <param name="parameterName">Parameter Name.</param>
        /// <param name="parameterType">Parameter Type.</param>
        /// <param name="parameterValue">Parameter Value.</param>
        /// <param name="size">Parameter Size.</param>
        /// <param name="precision">Parameter Precision.</param>
        /// <param name="direction">Parameter Direction.</param>
        /// <returns>Sql Parameter.</returns>
        internal SqlParameter CreateParameter(string parameterName, SqlDbType parameterType, object parameterValue, int size, byte precision, ParameterDirection direction)
        {
            SqlParameter sqlParameter = CreateParameter(parameterName, parameterType, parameterValue);
            sqlParameter.Direction = direction;
            sqlParameter.Size = size;
            sqlParameter.Precision = precision;

            return sqlParameter;
        }

        #endregion

        #region OpenConnection

        /// <summary>
        /// Opens the database connection with given connection string.
        /// </summary>
        /// <param name="connectionString">Database connection string.</param>
        /// <returns>Sql Connection.</returns>
        private SqlConnection OpenConnection(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException();
            }
            else
            {
                SqlConnection sqlConnection = null;

                try
                {
                    sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                }
                catch (Exception)
                {
                    sqlConnection = null;
                }

                return sqlConnection;
            }
        }

        #endregion

        #region CloseConnection

        /// <summary>
        /// Closes the sql connection if the current state is "Open".
        /// </summary>
        /// <param name="connection">Sql Connection.</param>
        private void CloseConnection(SqlConnection connection)
        {
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                    connection = null;
                }
            }
        }

        #endregion

        #region Boundary Check and Emptiness/Null Check Methods

        #region GetGuid

        /// <summary>
        /// Gets the GUID value from the given input value of type Object.
        /// </summary>
        /// <param name="value">Object type input.</param>
        /// <returns>GUID value.</returns>
        private Guid GetGuid(object value)
        {
            Guid parameterValue = Guid.Empty;

            if (value is string)
            {
                parameterValue = new Guid((string)value);
            }
            else if (value is Guid)
            {
                parameterValue = (Guid)value;
            }

            return parameterValue;
        }

        #endregion

        #region CheckParamValue Overloaded Methods

        /// <summary>
        /// Emptiness/Null Check for string type.
        /// </summary>
        /// <param name="parameterValue">Input of string type.</param>
        /// <returns>Output of Object type.</returns>
        private object CheckParamValue(string parameterValue)
        {
            if (string.IsNullOrEmpty(parameterValue))
            {
                return DBNull.Value;
            }
            else
            {
                return parameterValue;
            }
        }

        /// <summary>
        /// Emptiness Check for GUID type.
        /// </summary>
        /// <param name="parameterValue">Input of GUID type.</param>
        /// <returns>Output of Object type.</returns>
        private object CheckParamValue(Guid parameterValue)
        {
            if (parameterValue.Equals(Guid.Empty))
            {
                return DBNull.Value;
            }
            else
            {
                return parameterValue;
            }
        }

        /// <summary>
        /// Boundary Check for DateTime type.
        /// </summary>
        /// <param name="parameterValue">Input of DateTime type.</param>
        /// <returns>Output of Object type.</returns>
        private object CheckParamValue(DateTime parameterValue)
        {
            if (parameterValue.Equals(DateTime.MinValue))
            {
                return DBNull.Value;
            }
            else
            {
                return parameterValue;
            }
        }

        /// <summary>
        /// Boundary Check for double type.
        /// </summary>
        /// <param name="parameterValue">Input of double type.</param>
        /// <returns>Output of Object type.</returns>
        private object CheckParamValue(double parameterValue)
        {
            if (parameterValue.Equals(double.MinValue))
            {
                return DBNull.Value;
            }
            else
            {
                return parameterValue;
            }
        }

        /// <summary>
        /// Boundary Check for float type.
        /// </summary>
        /// <param name="parameterValue">Input of float type.</param>
        /// <returns>Output of Object type.</returns>
        private object CheckParamValue(float parameterValue)
        {
            if (parameterValue.Equals(float.MinValue))
            {
                return DBNull.Value;
            }
            else
            {
                return parameterValue;
            }
        }

        /// <summary>
        /// Boundary Check for Decimal type.
        /// </summary>
        /// <param name="parameterValue">Input of Decimal type.</param>
        /// <returns>Output of Object type.</returns>
        private object CheckParamValue(Decimal parameterValue)
        {
            if (parameterValue.Equals(Decimal.MinValue))
            {
                return DBNull.Value;
            }
            else
            {
                return parameterValue;
            }
        }

        /// <summary>
        /// Boundary Check for Integer type.
        /// </summary>
        /// <param name="parameterValue">Input of Integer type.</param>
        /// <returns>Output of Object type.</returns>
        private object CheckParamValue(int parameterValue)
        {
            if (parameterValue.Equals(int.MinValue))
            {
                return DBNull.Value;
            }
            else
            {
                return parameterValue;
            }
        }

        /// <summary>
        /// Boundary Check for binary collection.
        /// </summary>
        /// <param name="parameterValue">Input of binary type.</param>
        /// <returns>Output of Object type.</returns>
        private object CheckParamValue(byte[] parameterValue)
        {
            if (parameterValue == null)
            {
                return DBNull.Value;
            }
            else
            {
                return parameterValue;
            }
        }

        #endregion

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

                    //Since there are no disposable objects, it is kept empty. It will be used in future, if any disposable objects are going to be used.
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

