using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.Configuration;
using System.Data.SqlClient;
using TF.FocusPay.EmailServer.EmailBusinessLogic;
using TF.FocusPay.EmailServer.LogManager;

namespace TestEmail
{
    public partial class Form1 : Form
    {
        #region Private Variables

        private string FPEmailServerNameSpace = MethodBase.GetCurrentMethod().DeclaringType.ToString();
        private DataSet qrPriority = new DataSet();
      
        private const int DEFAULT_INT = -1;

        private const string NO_PRIOIRITIES = "There were no priorities in the queue reader. Please add priorities for queue reader and start the service again.";
        private const string THREAD_COUNT_THRESHOLD_MESSAGE = "Entered thread threshold exceeds the maximum number of threads for the queue reader.";
   
        private const string SERVICE_STOPPED = "Service has been stopped due to SQL connection failure. Please check the connection string.";

        private const string SERVICE_NAME = "ServiceName";

        private const string MACHINE_NAME = ".";
        private const string SERVICE_RUNNING = "Running";


        //private ServiceController controller = new ServiceController();

        private string serviceName;


        #endregion

        #region Private Properties
       
        

        /// <summary>
        /// Property for getting the Service Name
        /// </summary>
        private string servicename
        {
            get
            {
                try
                {
                    if (serviceName == null)
                    {
                        serviceName = ConfigurationManager.AppSettings[SERVICE_NAME].ToString();

                    }
                }

                catch (System.FormatException)
                {
                    //ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                }

                return serviceName;
            }
        }
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BLEmailLogic objPriority = new BLEmailLogic(1);
            try
            {
                qrPriority = objPriority.GetPriority();
                if (qrPriority.Tables.Count > 0 && qrPriority.Tables[0].Rows.Count > 0)
                {
                    {
                        BLEmailLogic[] objReader = new BLEmailLogic[qrPriority.Tables[0].Rows.Count];

                        for (int i = 0; i < qrPriority.Tables[0].Rows.Count; i++)
                        {
                            objReader[i] = new BLEmailLogic(Convert.ToInt32(qrPriority.Tables[0].Rows[i]["EmailPriorityID"].ToString()));

                        }
                    }

                }
                else
                {
                    MessageLogger.Write(NO_PRIOIRITIES, CategoryType.Information, null, FPEmailServerNameSpace);
                }
            }
            catch (SqlException sqlEx)
            {
                ExceptionLogger.PublishException(sqlEx, "Error", ListenerType.File);
                ExceptionLogger.PublishException(sqlEx, "Error", ListenerType.Email);
            }
            catch (Exception ex)
            {
                ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                ExceptionLogger.PublishException(ex, "Error", ListenerType.Email);
            }
        }
    }
}
