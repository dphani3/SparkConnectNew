namespace TF.ReceiptManager.Service
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
            this.ReceiptManagerProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ReceiptManagerServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ReceiptManagerProcessInstaller
            // 
            this.ReceiptManagerProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ReceiptManagerProcessInstaller.Password = null;
            this.ReceiptManagerProcessInstaller.Username = null;
            // 
            // ReceiptManagerServiceInstaller
            // 
            this.ReceiptManagerServiceInstaller.Description = "Allows to generate/retrieve/email the eReceipt for the credit card transactions.";
            this.ReceiptManagerServiceInstaller.DisplayName = "TFPS Receipt Manager";
            this.ReceiptManagerServiceInstaller.ServiceName = "Receipt Manager";
            this.ReceiptManagerServiceInstaller.ServicesDependedOn = new string[] {
        "Message Queuing"};
            this.ReceiptManagerServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ReceiptManagerProcessInstaller,
            this.ReceiptManagerServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ReceiptManagerProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ReceiptManagerServiceInstaller;
    }
}