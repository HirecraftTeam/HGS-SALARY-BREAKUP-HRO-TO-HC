using System.Configuration;
namespace IStudioService

{
    partial class Installer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.ServiceProcess.ServiceInstaller FileProcessingServiceInstaller;
        private System.ServiceProcess.ServiceProcessInstaller FileProcessingServiceProcessInstaller;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FileProcessingServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            this.FileProcessingServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            // 
            // FileProcessingServiceInstaller
            // 
            this.FileProcessingServiceInstaller.ServiceName = ConfigurationManager.AppSettings["ServiceName"];
            this.FileProcessingServiceInstaller.DisplayName = ConfigurationManager.AppSettings["ServiceName"];
            // 
            // FileProcessingServiceProcessInstaller
            // 
            this.FileProcessingServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.FileProcessingServiceProcessInstaller.Password = null;
            this.FileProcessingServiceProcessInstaller.Username = null;
            // 
            // ServiceInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] { this.FileProcessingServiceInstaller, this.FileProcessingServiceProcessInstaller });
        }

        #endregion
    }

}