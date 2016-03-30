namespace DesktopUserControl
{
    partial class AdProviderControl
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
			this.pbAd = new System.Windows.Forms.PictureBox();
			this.bgWorker = new System.ComponentModel.BackgroundWorker();
			((System.ComponentModel.ISupportInitialize)(this.pbAd)).BeginInit();
			this.SuspendLayout();
			// 
			// pbAd
			// 
			this.pbAd.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbAd.Location = new System.Drawing.Point(0, 0);
			this.pbAd.Name = "pbAd";
			this.pbAd.Size = new System.Drawing.Size(256, 226);
			this.pbAd.TabIndex = 0;
			this.pbAd.TabStop = false;
			this.pbAd.Click += new System.EventHandler(this.pbAd_Click);
			// 
			// bgWorker
			// 
			this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
			this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
			// 
			// AdProviderControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pbAd);
			this.Name = "AdProviderControl";
			this.Size = new System.Drawing.Size(256, 226);
			((System.ComponentModel.ISupportInitialize)(this.pbAd)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbAd;
        private System.ComponentModel.BackgroundWorker bgWorker;
    }
}
