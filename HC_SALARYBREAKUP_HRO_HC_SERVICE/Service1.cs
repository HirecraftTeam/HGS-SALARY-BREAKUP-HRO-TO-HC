using HC_SALARYBREAKUP_HRO_HC_SERVICE.Handler;
using HireCraft.HM_APIService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HC_SALARYBREAKUP_HRO_HC_SERVICE
{
    public partial class Service1 : ServiceBase
    {
        Timer reqTimer = new Timer();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            Log.LogData(ConfigurationManager.AppSettings["ServiceName"] + " Started", Log.Status.Info);
            //CTCBreakUPHROHandler opSalaryBreakUPHandler = new CTCBreakUPHROHandler();
            Log.LogData("CTC Salary BreakUp Service Starts", Log.Status.Info);
            //opSalaryBreakUPHandler.Process();
            try
            {
                
                
                if (Helper.EnableReqSync)
                {
                    reqTimer = new Timer(TimeSpan.FromMinutes(Helper.ServiceMinsRun).TotalMilliseconds);
                    reqTimer.Elapsed += new ElapsedEventHandler(opReqHandler);
                    reqTimer.Start();
                }



            }
            catch (Exception ex)
            {
                Log.LogData(ex.Message, Log.Status.Error);
            }

        }

        void opReqHandler(object sender, ElapsedEventArgs args)
        {
            Task.Run(() =>
            {
                CTCBreakUPHROHandler opSalaryBreakUPHandler = new CTCBreakUPHROHandler();
                Log.LogData("CTC Salary BreakUp Service Starts", Log.Status.Info);
                opSalaryBreakUPHandler.Process();
                Log.LogData("--------------- ", Log.Status.Info);

            });
        }


        protected override void OnStop()
        {
        }
    }
}
