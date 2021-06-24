using HireCraft.HM_APIService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_SALARYBREAKUP_HRO_HC_SERVICE
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            try
            {


                //#if DEBUG
                bool _IsInstalled = false;
                bool serviceStarting = false;
                string SERVICE_NAME = ConfigurationManager.AppSettings["ServiceName"];

                ServiceController[] services = ServiceController.GetServices();

                foreach (ServiceController service in services)
                {
                    if (service.ServiceName.Equals(SERVICE_NAME))
                    {
                        _IsInstalled = true;
                        if (service.Status == ServiceControllerStatus.StartPending)
                        {
                            // If the status is StartPending then the service was started via the SCM             
                            serviceStarting = true;
                        }
                        break;
                    }
                }

                if (!serviceStarting)
                {
                    if (_IsInstalled == true)
                    {
                        // Thanks to PIEBALDconsult's Concern V2.0
                        DialogResult dr = new DialogResult();
                        dr = MessageBox.Show("Uninstall " + SERVICE_NAME + "?", "UnInstall " + ConfigurationManager.AppSettings["ServiceName"], MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                        if (dr == DialogResult.Yes)
                        {
                            SelfInstaller.UninstallMe();
                            MessageBox.Show("Successfully uninstalled the " + SERVICE_NAME, ConfigurationManager.AppSettings["ServiceName"],
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        DialogResult dr = new DialogResult();
                        dr = MessageBox.Show("Install " + SERVICE_NAME + "?", "Install " + ConfigurationManager.AppSettings["ServiceName"], MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (dr == DialogResult.Yes)
                        {
                            SelfInstaller.InstallMe();
                            MessageBox.Show("Successfully installed the " + SERVICE_NAME, ConfigurationManager.AppSettings["ServiceName"],
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    // Started from the SCM
                    System.ServiceProcess.ServiceBase[] servicestorun;
                    servicestorun = new System.ServiceProcess.ServiceBase[] { new Service1() };
                    ServiceBase.Run(servicestorun);
                }
                //#endif

            }

            catch (Exception ex)
            {
                Log.LogData("main function in program.cs: " + ex.Message + ex.StackTrace, Log.Status.Error);
            }
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new Service1()
            //};
            //ServiceBase.Run(ServicesToRun);
        }

        public static class SelfInstaller
        {
            private static readonly string _exePath = Assembly.GetExecutingAssembly().Location;
            public static bool InstallMe()
            {
                try
                {
                    ManagedInstallerClass.InstallHelper(
                        new string[] { _exePath });
                }
                catch
                {
                    return false;
                }
                return true;
            }

            public static bool UninstallMe()
            {
                try
                {
                    ManagedInstallerClass.InstallHelper(
                        new string[] { "/u", _exePath });
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }
    }
}
