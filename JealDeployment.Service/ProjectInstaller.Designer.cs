namespace JealDeployment.Service
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.defaultServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.defaultServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // defaultServiceProcessInstaller
            // 
            this.defaultServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.defaultServiceProcessInstaller.Password = null;
            this.defaultServiceProcessInstaller.Username = null;
            // 
            // defaultServiceInstaller
            // 
            this.defaultServiceInstaller.ServiceName = "JealDeployment";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.defaultServiceProcessInstaller,
            this.defaultServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller defaultServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller defaultServiceInstaller;
    }
}