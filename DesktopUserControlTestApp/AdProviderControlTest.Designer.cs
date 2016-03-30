using DesktopUserControl;
namespace DesktopUserControlTestApp
{
    partial class AdProviderControlTest
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
            this.components = new System.ComponentModel.Container();
            this.tbErrors1 = new System.Windows.Forms.TextBox();
            this.btnAd1Run = new System.Windows.Forms.Button();
            this.btnAd1Stop = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.nudAd1Freq = new System.Windows.Forms.NumericUpDown();
            this.AdMonitor = new System.Windows.Forms.Timer(this.components);
            this.btnClearLog1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panelParameters = new System.Windows.Forms.Panel();
            this.tbDevice = new System.Windows.Forms.NumericUpDown();
            this.tbAdditionalInfo = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbCompany = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.tbReferrer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbFirstName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbLastName = new System.Windows.Forms.TextBox();
            this.tbCategories = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.adProviderControl1 = new DesktopUserControl.AdProviderControl();
            ((System.ComponentModel.ISupportInitialize)(this.nudAd1Freq)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbDevice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbErrors1
            // 
            this.tbErrors1.Location = new System.Drawing.Point(8, 53);
            this.tbErrors1.Margin = new System.Windows.Forms.Padding(4);
            this.tbErrors1.Multiline = true;
            this.tbErrors1.Name = "tbErrors1";
            this.tbErrors1.ReadOnly = true;
            this.tbErrors1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbErrors1.Size = new System.Drawing.Size(480, 227);
            this.tbErrors1.TabIndex = 2;
            // 
            // btnAd1Run
            // 
            this.btnAd1Run.Location = new System.Drawing.Point(389, 393);
            this.btnAd1Run.Margin = new System.Windows.Forms.Padding(4);
            this.btnAd1Run.Name = "btnAd1Run";
            this.btnAd1Run.Size = new System.Drawing.Size(100, 28);
            this.btnAd1Run.TabIndex = 3;
            this.btnAd1Run.Text = "Włącz";
            this.btnAd1Run.UseVisualStyleBackColor = true;
            this.btnAd1Run.Click += new System.EventHandler(this.btnAd1Run_Click);
            // 
            // btnAd1Stop
            // 
            this.btnAd1Stop.Location = new System.Drawing.Point(281, 393);
            this.btnAd1Stop.Margin = new System.Windows.Forms.Padding(4);
            this.btnAd1Stop.Name = "btnAd1Stop";
            this.btnAd1Stop.Size = new System.Drawing.Size(100, 28);
            this.btnAd1Stop.TabIndex = 4;
            this.btnAd1Stop.Text = "Wyłącz";
            this.btnAd1Stop.UseVisualStyleBackColor = true;
            this.btnAd1Stop.Click += new System.EventHandler(this.btnAd1Stop_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Częstotliwość ms";
            // 
            // nudAd1Freq
            // 
            this.nudAd1Freq.Location = new System.Drawing.Point(129, 6);
            this.nudAd1Freq.Margin = new System.Windows.Forms.Padding(4);
            this.nudAd1Freq.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudAd1Freq.Name = "nudAd1Freq";
            this.nudAd1Freq.Size = new System.Drawing.Size(83, 22);
            this.nudAd1Freq.TabIndex = 8;
            this.nudAd1Freq.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // AdMonitor
            // 
            this.AdMonitor.Enabled = true;
            this.AdMonitor.Interval = 500;
            this.AdMonitor.Tick += new System.EventHandler(this.AdMonitor_Tick);
            // 
            // btnClearLog1
            // 
            this.btnClearLog1.Location = new System.Drawing.Point(411, 17);
            this.btnClearLog1.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearLog1.Name = "btnClearLog1";
            this.btnClearLog1.Size = new System.Drawing.Size(79, 28);
            this.btnClearLog1.TabIndex = 13;
            this.btnClearLog1.Text = "Wyczyść";
            this.btnClearLog1.UseVisualStyleBackColor = true;
            this.btnClearLog1.Click += new System.EventHandler(this.btnClearLog1_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.adProviderControl1);
            this.panel1.Location = new System.Drawing.Point(521, 15);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(756, 724);
            this.panel1.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panelParameters);
            this.groupBox1.Controls.Add(this.btnAd1Stop);
            this.groupBox1.Controls.Add(this.btnAd1Run);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(497, 427);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parametry";
            // 
            // panelParameters
            // 
            this.panelParameters.Controls.Add(this.tbDevice);
            this.panelParameters.Controls.Add(this.tbAdditionalInfo);
            this.panelParameters.Controls.Add(this.label1);
            this.panelParameters.Controls.Add(this.label11);
            this.panelParameters.Controls.Add(this.nudAd1Freq);
            this.panelParameters.Controls.Add(this.tbCompany);
            this.panelParameters.Controls.Add(this.label2);
            this.panelParameters.Controls.Add(this.label10);
            this.panelParameters.Controls.Add(this.label3);
            this.panelParameters.Controls.Add(this.tbEmail);
            this.panelParameters.Controls.Add(this.nudWidth);
            this.panelParameters.Controls.Add(this.label9);
            this.panelParameters.Controls.Add(this.nudHeight);
            this.panelParameters.Controls.Add(this.label4);
            this.panelParameters.Controls.Add(this.tbReferrer);
            this.panelParameters.Controls.Add(this.label5);
            this.panelParameters.Controls.Add(this.label7);
            this.panelParameters.Controls.Add(this.tbFirstName);
            this.panelParameters.Controls.Add(this.label8);
            this.panelParameters.Controls.Add(this.tbLastName);
            this.panelParameters.Controls.Add(this.tbCategories);
            this.panelParameters.Controls.Add(this.label6);
            this.panelParameters.Location = new System.Drawing.Point(8, 17);
            this.panelParameters.Margin = new System.Windows.Forms.Padding(4);
            this.panelParameters.Name = "panelParameters";
            this.panelParameters.Size = new System.Drawing.Size(484, 373);
            this.panelParameters.TabIndex = 1;
            // 
            // tbDevice
            // 
            this.tbDevice.Location = new System.Drawing.Point(139, 233);
            this.tbDevice.Margin = new System.Windows.Forms.Padding(4);
            this.tbDevice.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.tbDevice.Name = "tbDevice";
            this.tbDevice.Size = new System.Drawing.Size(83, 22);
            this.tbDevice.TabIndex = 29;
            this.tbDevice.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // tbAdditionalInfo
            // 
            this.tbAdditionalInfo.Location = new System.Drawing.Point(139, 327);
            this.tbAdditionalInfo.Margin = new System.Windows.Forms.Padding(4);
            this.tbAdditionalInfo.Name = "tbAdditionalInfo";
            this.tbAdditionalInfo.Size = new System.Drawing.Size(335, 22);
            this.tbAdditionalInfo.TabIndex = 28;
            this.tbAdditionalInfo.Text = "info dodatkowe";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 331);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(90, 17);
            this.label11.TabIndex = 27;
            this.label11.Text = "Dodatkowe : ";
            // 
            // tbCompany
            // 
            this.tbCompany.Location = new System.Drawing.Point(139, 295);
            this.tbCompany.Margin = new System.Windows.Forms.Padding(4);
            this.tbCompany.Name = "tbCompany";
            this.tbCompany.Size = new System.Drawing.Size(335, 22);
            this.tbCompany.TabIndex = 26;
            this.tbCompany.Text = "Querilogic";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Imię : ";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 299);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 17);
            this.label10.TabIndex = 25;
            this.label10.Text = "Firma : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Nazwisko : ";
            // 
            // tbEmail
            // 
            this.tbEmail.Location = new System.Drawing.Point(139, 263);
            this.tbEmail.Margin = new System.Windows.Forms.Padding(4);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(335, 22);
            this.tbEmail.TabIndex = 24;
            this.tbEmail.Text = "test@test.test";
            // 
            // nudWidth
            // 
            this.nudWidth.Location = new System.Drawing.Point(129, 106);
            this.nudWidth.Margin = new System.Windows.Forms.Padding(4);
            this.nudWidth.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(83, 22);
            this.nudWidth.TabIndex = 11;
            this.nudWidth.Value = new decimal(new int[] {
            728,
            0,
            0,
            0});
            this.nudWidth.ValueChanged += new System.EventHandler(this.nudWidth_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 267);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 17);
            this.label9.TabIndex = 23;
            this.label9.Text = "Email : ";
            // 
            // nudHeight
            // 
            this.nudHeight.Location = new System.Drawing.Point(244, 106);
            this.nudHeight.Margin = new System.Windows.Forms.Padding(4);
            this.nudHeight.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(83, 22);
            this.nudHeight.TabIndex = 12;
            this.nudHeight.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.nudHeight.ValueChanged += new System.EventHandler(this.nudWidth_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 110);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 17);
            this.label4.TabIndex = 13;
            this.label4.Text = "Wymiary : ";
            // 
            // tbReferrer
            // 
            this.tbReferrer.Location = new System.Drawing.Point(139, 203);
            this.tbReferrer.Margin = new System.Windows.Forms.Padding(4);
            this.tbReferrer.Name = "tbReferrer";
            this.tbReferrer.Size = new System.Drawing.Size(335, 22);
            this.tbReferrer.TabIndex = 21;
            this.tbReferrer.Text = "aplikacja desktopowa DesktopUserControl";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(220, 110);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "x";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 238);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 17);
            this.label7.TabIndex = 20;
            this.label7.Text = "ID Nośnika : ";
            // 
            // tbFirstName
            // 
            this.tbFirstName.Location = new System.Drawing.Point(129, 46);
            this.tbFirstName.Margin = new System.Windows.Forms.Padding(4);
            this.tbFirstName.Name = "tbFirstName";
            this.tbFirstName.Size = new System.Drawing.Size(335, 22);
            this.tbFirstName.TabIndex = 15;
            this.tbFirstName.Text = "Jan";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 207);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 17);
            this.label8.TabIndex = 19;
            this.label8.Text = "Referrer : ";
            // 
            // tbLastName
            // 
            this.tbLastName.Location = new System.Drawing.Point(129, 75);
            this.tbLastName.Margin = new System.Windows.Forms.Padding(4);
            this.tbLastName.Name = "tbLastName";
            this.tbLastName.Size = new System.Drawing.Size(335, 22);
            this.tbLastName.TabIndex = 16;
            this.tbLastName.Text = "Kowalski";
            // 
            // tbCategories
            // 
            this.tbCategories.Location = new System.Drawing.Point(188, 138);
            this.tbCategories.Margin = new System.Windows.Forms.Padding(4);
            this.tbCategories.Multiline = true;
            this.tbCategories.Name = "tbCategories";
            this.tbCategories.Size = new System.Drawing.Size(283, 57);
            this.tbCategories.TabIndex = 18;
            this.tbCategories.Text = "IT";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 142);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(176, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "Kategorie (po przecinku) : ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbErrors1);
            this.groupBox2.Controls.Add(this.btnClearLog1);
            this.groupBox2.Location = new System.Drawing.Point(16, 449);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(497, 289);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Logi";
            // 
            // adProviderControl1
            // 
            this.adProviderControl1._data0 = null;
            this.adProviderControl1._data1 = null;
            this.adProviderControl1._data2 = null;
            this.adProviderControl1._data3 = null;
            this.adProviderControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.adProviderControl1.ID = 0;
            this.adProviderControl1.IsActive = false;
            this.adProviderControl1.Location = new System.Drawing.Point(5, 0);
            this.adProviderControl1.Margin = new System.Windows.Forms.Padding(5);
            this.adProviderControl1.MaxBufferSize = 2147483647;
            this.adProviderControl1.MaxReceivedMessageSize = 2147483647;
            this.adProviderControl1.Name = "adProviderControl1";
            this.adProviderControl1.OpenTimeOut = 120000;
            this.adProviderControl1.ReciveTimeOut = 120000;
            this.adProviderControl1.RequestFrequency = 0;
            this.adProviderControl1.SendTimeOut = 120000;
            this.adProviderControl1.Size = new System.Drawing.Size(740, 100);
            this.adProviderControl1.TabIndex = 0;
            this.adProviderControl1.WebServiceUrl = "http://demo.ec2.pl/AdServerWS/WebServiceADContentProvider.asmx";
            // 
            // AdProviderControlTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1287, 745);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "AdProviderControlTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AdProviderControlTest";
            this.Load += new System.EventHandler(this.AdProviderControlTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudAd1Freq)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panelParameters.ResumeLayout(false);
            this.panelParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbDevice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AdProviderControl adProviderControl1;
        private System.Windows.Forms.TextBox tbErrors1;
        private System.Windows.Forms.Button btnAd1Run;
        private System.Windows.Forms.Button btnAd1Stop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudAd1Freq;
        private System.Windows.Forms.Timer AdMonitor;
        private System.Windows.Forms.Button btnClearLog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbLastName;
        private System.Windows.Forms.TextBox tbFirstName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.TextBox tbCategories;
		private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbReferrer;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbCompany;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbAdditionalInfo;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panelParameters;
		private System.Windows.Forms.NumericUpDown tbDevice;
    }
}