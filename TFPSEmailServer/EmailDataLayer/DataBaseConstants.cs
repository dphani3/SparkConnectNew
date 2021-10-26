#region Header Information
/***********************************************************************************************************
NameSpace: IR.QueueReader.QueueReaderDataAccess
Class: DataBaseConstants.cs
Author: Sarvesh.T.S
Created Date: 16-May-2008
Reviewed By: 
***********************************************************************************************************/
#endregion

#region Namspace declaration

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

#endregion

namespace TF.FocusPay.EmailServer.EmailDataLayer
{
    public static class DataBaseConstants
    {
        #region Constructor
        static DataBaseConstants()
        {
        }
        #endregion Constructor

        #region Database Name
        public static string DATABASE_NAME = "QueueReader";
        #endregion Database Name

        #region Stored Procedure names

        public const string PRC_PRIORITY_QUEUE_INFORMATION = "SelectNextRecordFromEmailQueue";
        public const string PRC_UPDATE_QUEUE_STATUS = "UpdateEmailQueue";
        public const string PRC_PRIORITY = "SelectEmailPriorities";

        #endregion Stored Procedure names

        #region Table Names

        public const string QUEUE_READER_TABLE = "ServiceQueue";

        #endregion Table Names

        #region Stored Procedure Parameters Names


        #endregion Stored Procedure Parameters Names

        #region Table Parameters

        //public const int OMS_QUEUE_TABLE_PARAMETERS = 14;

        #endregion Table Parameters





    }
}
