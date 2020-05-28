namespace ASCOM.SSFocuser
{
    partial class SetupDialogForm
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.comboBoxcomPort = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxMaxStep = new System.Windows.Forms.TextBox();
            this.textBoxStepSize = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.linkLabelJoinUs = new System.Windows.Forms.LinkLabel();
            this.checkBoxPC2Focuser = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelTel = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.labelWebsite = new System.Windows.Forms.Label();
            this.labelVendor = new System.Windows.Forms.Label();
            this.labelDevice = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageDevice = new System.Windows.Forms.TabPage();
            this.tabPageInitialize = new System.Windows.Forms.TabPage();
            this.textBoxVelocity = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPosition = new System.Windows.Forms.TextBox();
            this.comboBoxVelocity = new System.Windows.Forms.ComboBox();
            this.buttonDefineZero = new System.Windows.Forms.Button();
            this.buttonSlewOut = new System.Windows.Forms.Button();
            this.buttonSlewIn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageDevice.SuspendLayout();
            this.tabPageInitialize.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(107, 280);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(60, 25);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(212, 280);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(60, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.SSFocuser.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(269, 22);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(179, 50);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(72, 16);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 51;
            this.label1.Text = "Com Port";
            // 
            // linkLabel
            // 
            this.linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel.AutoSize = true;
            this.linkLabel.Location = new System.Drawing.Point(67, 60);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(83, 12);
            this.linkLabel.TabIndex = 53;
            this.linkLabel.TabStop = true;
            this.linkLabel.Text = "Graycode Team";
            this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
            // 
            // comboBoxcomPort
            // 
            this.comboBoxcomPort.FormattingEnabled = true;
            this.comboBoxcomPort.Location = new System.Drawing.Point(90, 20);
            this.comboBoxcomPort.Name = "comboBoxcomPort";
            this.comboBoxcomPort.Size = new System.Drawing.Size(70, 20);
            this.comboBoxcomPort.TabIndex = 49;
            this.comboBoxcomPort.SelectedValueChanged += new System.EventHandler(this.comboBoxcomPort_SelectedValueChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(10, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 15);
            this.label7.TabIndex = 26;
            this.label7.Text = "Step Size";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(10, 47);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 15);
            this.label10.TabIndex = 15;
            this.label10.Text = "Max Step";
            // 
            // textBoxMaxStep
            // 
            this.textBoxMaxStep.Location = new System.Drawing.Point(90, 45);
            this.textBoxMaxStep.Name = "textBoxMaxStep";
            this.textBoxMaxStep.Size = new System.Drawing.Size(70, 21);
            this.textBoxMaxStep.TabIndex = 23;
            this.textBoxMaxStep.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxMaxStep_KeyPress);
            // 
            // textBoxStepSize
            // 
            this.textBoxStepSize.Location = new System.Drawing.Point(90, 70);
            this.textBoxStepSize.Name = "textBoxStepSize";
            this.textBoxStepSize.Size = new System.Drawing.Size(70, 21);
            this.textBoxStepSize.TabIndex = 24;
            this.textBoxStepSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxStepSize_KeyPress);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.comboBoxcomPort);
            this.groupBox1.Controls.Add(this.picASCOM);
            this.groupBox1.Controls.Add(this.linkLabelJoinUs);
            this.groupBox1.Controls.Add(this.textBoxStepSize);
            this.groupBox1.Controls.Add(this.checkBoxPC2Focuser);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxMaxStep);
            this.groupBox1.Controls.Add(this.chkTrace);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(333, 105);
            this.groupBox1.TabIndex = 55;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Focuser Settings";
            // 
            // linkLabelJoinUs
            // 
            this.linkLabelJoinUs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelJoinUs.AutoSize = true;
            this.linkLabelJoinUs.LinkColor = System.Drawing.Color.Blue;
            this.linkLabelJoinUs.Location = new System.Drawing.Point(176, 75);
            this.linkLabelJoinUs.Name = "linkLabelJoinUs";
            this.linkLabelJoinUs.Size = new System.Drawing.Size(53, 12);
            this.linkLabelJoinUs.TabIndex = 58;
            this.linkLabelJoinUs.TabStop = true;
            this.linkLabelJoinUs.Text = " Join us";
            this.linkLabelJoinUs.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelJoinUs_LinkClicked);
            // 
            // checkBoxPC2Focuser
            // 
            this.checkBoxPC2Focuser.AutoSize = true;
            this.checkBoxPC2Focuser.Location = new System.Drawing.Point(179, 24);
            this.checkBoxPC2Focuser.Name = "checkBoxPC2Focuser";
            this.checkBoxPC2Focuser.Size = new System.Drawing.Size(84, 16);
            this.checkBoxPC2Focuser.TabIndex = 57;
            this.checkBoxPC2Focuser.Text = "PC2Focuser";
            this.checkBoxPC2Focuser.UseVisualStyleBackColor = true;
            this.checkBoxPC2Focuser.CheckedChanged += new System.EventHandler(this.checkBoxPC2Focuser_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelTel);
            this.groupBox2.Controls.Add(this.labelEmail);
            this.groupBox2.Controls.Add(this.labelWebsite);
            this.groupBox2.Controls.Add(this.labelVendor);
            this.groupBox2.Controls.Add(this.labelDevice);
            this.groupBox2.Controls.Add(this.linkLabel);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(323, 121);
            this.groupBox2.TabIndex = 59;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Device Information";
            // 
            // labelTel
            // 
            this.labelTel.AutoSize = true;
            this.labelTel.Location = new System.Drawing.Point(12, 100);
            this.labelTel.Name = "labelTel";
            this.labelTel.Size = new System.Drawing.Size(101, 12);
            this.labelTel.TabIndex = 58;
            this.labelTel.Text = "WeChat: graycode";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(12, 80);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(155, 12);
            this.labelEmail.TabIndex = 57;
            this.labelEmail.Text = "Email: graycode(at)qq.com";
            // 
            // labelWebsite
            // 
            this.labelWebsite.AutoSize = true;
            this.labelWebsite.Location = new System.Drawing.Point(12, 60);
            this.labelWebsite.Name = "labelWebsite";
            this.labelWebsite.Size = new System.Drawing.Size(53, 12);
            this.labelWebsite.TabIndex = 56;
            this.labelWebsite.Text = "Website:";
            // 
            // labelVendor
            // 
            this.labelVendor.AutoSize = true;
            this.labelVendor.Location = new System.Drawing.Point(12, 40);
            this.labelVendor.Name = "labelVendor";
            this.labelVendor.Size = new System.Drawing.Size(149, 12);
            this.labelVendor.TabIndex = 55;
            this.labelVendor.Text = "Developer: Graycode Team";
            // 
            // labelDevice
            // 
            this.labelDevice.AutoSize = true;
            this.labelDevice.Location = new System.Drawing.Point(12, 20);
            this.labelDevice.Name = "labelDevice";
            this.labelDevice.Size = new System.Drawing.Size(185, 12);
            this.labelDevice.TabIndex = 54;
            this.labelDevice.Text = "Device: Searching device......";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageDevice);
            this.tabControl1.Controls.Add(this.tabPageInitialize);
            this.tabControl1.Location = new System.Drawing.Point(10, 121);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(333, 152);
            this.tabControl1.TabIndex = 60;
            // 
            // tabPageDevice
            // 
            this.tabPageDevice.Controls.Add(this.groupBox2);
            this.tabPageDevice.Location = new System.Drawing.Point(4, 22);
            this.tabPageDevice.Name = "tabPageDevice";
            this.tabPageDevice.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDevice.Size = new System.Drawing.Size(325, 126);
            this.tabPageDevice.TabIndex = 0;
            this.tabPageDevice.Text = "Device";
            this.tabPageDevice.UseVisualStyleBackColor = true;
            // 
            // tabPageInitialize
            // 
            this.tabPageInitialize.Controls.Add(this.textBoxVelocity);
            this.tabPageInitialize.Controls.Add(this.label3);
            this.tabPageInitialize.Controls.Add(this.label2);
            this.tabPageInitialize.Controls.Add(this.textBoxPosition);
            this.tabPageInitialize.Controls.Add(this.comboBoxVelocity);
            this.tabPageInitialize.Controls.Add(this.buttonDefineZero);
            this.tabPageInitialize.Controls.Add(this.buttonSlewOut);
            this.tabPageInitialize.Controls.Add(this.buttonSlewIn);
            this.tabPageInitialize.Location = new System.Drawing.Point(4, 22);
            this.tabPageInitialize.Name = "tabPageInitialize";
            this.tabPageInitialize.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInitialize.Size = new System.Drawing.Size(325, 126);
            this.tabPageInitialize.TabIndex = 1;
            this.tabPageInitialize.Text = "Initialize";
            this.tabPageInitialize.UseVisualStyleBackColor = true;
            // 
            // textBoxVelocity
            // 
            this.textBoxVelocity.Location = new System.Drawing.Point(188, 43);
            this.textBoxVelocity.Name = "textBoxVelocity";
            this.textBoxVelocity.ReadOnly = true;
            this.textBoxVelocity.Size = new System.Drawing.Size(82, 21);
            this.textBoxVelocity.TabIndex = 60;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(207, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 15);
            this.label3.TabIndex = 59;
            this.label3.Text = "Velocity";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(49, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 15);
            this.label2.TabIndex = 58;
            this.label2.Text = "Position";
            // 
            // textBoxPosition
            // 
            this.textBoxPosition.Location = new System.Drawing.Point(31, 43);
            this.textBoxPosition.Name = "textBoxPosition";
            this.textBoxPosition.ReadOnly = true;
            this.textBoxPosition.Size = new System.Drawing.Size(100, 21);
            this.textBoxPosition.TabIndex = 4;
            // 
            // comboBoxVelocity
            // 
            this.comboBoxVelocity.FormattingEnabled = true;
            this.comboBoxVelocity.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.comboBoxVelocity.Location = new System.Drawing.Point(276, 43);
            this.comboBoxVelocity.Name = "comboBoxVelocity";
            this.comboBoxVelocity.Size = new System.Drawing.Size(29, 20);
            this.comboBoxVelocity.TabIndex = 3;
            this.comboBoxVelocity.Text = "5";
            this.comboBoxVelocity.SelectedIndexChanged += new System.EventHandler(this.comboBoxVelocity_SelectedIndexChanged);
            // 
            // buttonDefineZero
            // 
            this.buttonDefineZero.Location = new System.Drawing.Point(209, 79);
            this.buttonDefineZero.Name = "buttonDefineZero";
            this.buttonDefineZero.Size = new System.Drawing.Size(98, 23);
            this.buttonDefineZero.TabIndex = 2;
            this.buttonDefineZero.Text = "Define Zero";
            this.buttonDefineZero.UseVisualStyleBackColor = true;
            this.buttonDefineZero.Click += new System.EventHandler(this.buttonDefineZero_Click);
            // 
            // buttonSlewOut
            // 
            this.buttonSlewOut.Location = new System.Drawing.Point(111, 79);
            this.buttonSlewOut.Name = "buttonSlewOut";
            this.buttonSlewOut.Size = new System.Drawing.Size(75, 23);
            this.buttonSlewOut.TabIndex = 1;
            this.buttonSlewOut.Text = "Slew Out";
            this.buttonSlewOut.UseVisualStyleBackColor = true;
            this.buttonSlewOut.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlewOut_MouseDown);
            this.buttonSlewOut.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlewOut_MouseUp);
            // 
            // buttonSlewIn
            // 
            this.buttonSlewIn.Location = new System.Drawing.Point(15, 79);
            this.buttonSlewIn.Name = "buttonSlewIn";
            this.buttonSlewIn.Size = new System.Drawing.Size(75, 23);
            this.buttonSlewIn.TabIndex = 0;
            this.buttonSlewIn.Text = "Slew In";
            this.buttonSlewIn.UseVisualStyleBackColor = true;
            this.buttonSlewIn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonSlewIn_MouseDown);
            this.buttonSlewIn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSlewIn_MouseUp);
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 312);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SS Focuser Setup";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SetupDialogForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageDevice.ResumeLayout(false);
            this.tabPageInitialize.ResumeLayout(false);
            this.tabPageInitialize.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.ComboBox comboBoxcomPort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxMaxStep;
        private System.Windows.Forms.TextBox textBoxStepSize;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxPC2Focuser;
        private System.Windows.Forms.LinkLabel linkLabelJoinUs;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelTel;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.Label labelWebsite;
        private System.Windows.Forms.Label labelVendor;
        private System.Windows.Forms.Label labelDevice;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageDevice;
        private System.Windows.Forms.TabPage tabPageInitialize;
        private System.Windows.Forms.Button buttonDefineZero;
        private System.Windows.Forms.Button buttonSlewOut;
        private System.Windows.Forms.Button buttonSlewIn;
        private System.Windows.Forms.TextBox textBoxPosition;
        private System.Windows.Forms.ComboBox comboBoxVelocity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxVelocity;
    }
}