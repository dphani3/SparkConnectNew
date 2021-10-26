using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using TF.FocusPay.EmailServer.EmailBusinessLogic;
using TF.FocusPay.EmailServer.LogManager;

namespace TFPSEmailServer
{
    public partial class TFPSEmailService : ServiceBase
    {
        #region Private Variables

        private string FPEmailServerNameSpace = MethodBase.GetCurrentMethod().DeclaringType.ToString();
        private DataSet qrPriority = new DataSet();
     
        private const int DEFAULT_INT = -1;

        private const string NO_PRIOIRITIES = "There were no priorities in the queue. Please add priorities and start the service again.";
        private const string THREAD_COUNT_THRESHOLD_MESSAGE = "Entered thread threshold exceeds the maximum number of threads for the queue reader.";
   
        private const string SERVICE_STOPPED = "Service has been stopped due to SQL connection failure. Please check the connection string.";        

        private const string MACHINE_NAME = ".";
        private const string SERVICE_RUNNING = "Running";


        private ServiceController controller = new ServiceController();        

        private System.Threading.Timer oTimer;

        #endregion        

        public TFPSEmailService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
              
                int intervalTime = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalSeconds"]);
                TimerCallback tmrCallBack = new TimerCallback(oTimer_TimerCallback);
                oTimer = new System.Threading.Timer(tmrCallBack);
                oTimer.Change(new TimeSpan(0, 0, intervalTime), new TimeSpan(0, 0, intervalTime));
            }
            catch (SqlException ex)
            {
                ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                ExceptionLogger.PublishException(ex, "Error", ListenerType.Email);
                MessageLogger.Write(SERVICE_STOPPED, CategoryType.Information, null, FPEmailServerNameSpace);
            }
            catch (Exception ex)
            {
                ExceptionLogger.PublishException(ex, "Error", ListenerType.File);
                ExceptionLogger.PublishException(ex, "Error", ListenerType.Email);
                MessageLogger.Write(SERVICE_STOPPED, CategoryType.Information, null, FPEmailServerNameSpace);
            }
        }

        protected override void OnStop()
        {
            
        }

        private void Process()
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

        private void oTimer_TimerCallback(object state)
        {   
                Process();
        }
    }
}
