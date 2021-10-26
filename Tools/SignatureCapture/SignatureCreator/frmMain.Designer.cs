namespace SignatureCreator
{
    partial class frmMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ofdImageSelector = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblSignatureLocation = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtSignaturePath = new System.Windows.Forms.TextBox();
            this.gbImageSelector = new System.Windows.Forms.GroupBox();
            this.rtxtImageData = new System.Windows.Forms.RichTextBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbImageSelector.SuspendLayout();
            this.SuspendLayout();
            // 
            // ofdImageSelector
            // 
            this.ofdImageSelector.FileName = "Signature Selector";
            this.ofdImageSelector.Filter = "All Files(*.*)|*.*";
            this.ofdImageSelector.Title = "Please select the image types only...";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblSignatureLocation);
            this.splitContainer1.Panel1.Controls.Add(this.btnSelect);
            this.splitContainer1.Panel1.Controls.Add(this.txtSignaturePath);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbImageSelector);
            this.splitContainer1.Size = new System.Drawing.Size(828, 546);
            this.splitContainer1.SplitterDistance = 78;
            this.splitContainer1.TabIndex = 0;
            // 
            // lblSignatureLocation
            // 
            this.lblSignatureLocation.AutoSize = true;
            this.lblSignatureLocation.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSignatureLocation.Location = new System.Drawing.Point(81, 28);
            this.lblSignatureLocation.Name = "lblSignatureLocation";
            this.lblSignatureLocation.Size = new System.Drawing.Size(244, 13);
            this.lblSignatureLocation.TabIndex = 2;
            this.lblSignatureLocation.Text = "Please select the signature location:";
            // 
            // btnSelect
            // 
            this.btnSelect.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.Location = new System.Drawing.Point(672, 28);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtSignaturePath
            // 
            this.txtSignaturePath.Location = new System.Drawing.Point(349, 28);
            this.txtSignaturePath.Name = "txtSignaturePath";
            this.txtSignaturePath.Size = new System.Drawing.Size(299, 20);
            this.txtSignaturePath.TabIndex = 0;
            // 
            // gbImageSelector
            // 
            this.gbImageSelector.Controls.Add(this.rtxtImageData);
            this.gbImageSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbImageSelector.Location = new System.Drawing.Point(0, 0);
            this.gbImageSelector.Name = "gbImageSelector";
            this.gbImageSelector.Size = new System.Drawing.Size(828, 464);
            this.gbImageSelector.TabIndex = 0;
            this.gbImageSelector.TabStop = false;
            this.gbImageSelector.Text = "Signature Image";
            // 
            // rtxtImageData
            // 
            this.rtxtImageData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtImageData.Location = new System.Drawing.Point(3, 16);
            this.rtxtImageData.Name = "rtxtImageData";
            this.rtxtImageData.Size = new System.Drawing.Size(822, 445);
            this.rtxtImageData.TabIndex = 0;
            this.rtxtImageData.Text = "";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 546);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "FocusPay Signature Creator";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.gbImageSelector.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdImageSelector;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblSignatureLocation;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtSignaturePath;
        private System.Windows.Forms.GroupBox gbImageSelector;
        private System.Windows.Forms.RichTextBox rtxtImageData;
    }
}

