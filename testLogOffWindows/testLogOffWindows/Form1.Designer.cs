namespace testLogOffWindows
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.msgRtb = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.disableMonitoringBtn = new System.Windows.Forms.Button();
            this.getWristStrapStateBtn = new System.Windows.Forms.Button();
            this.calibrationBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.timerComboBox = new System.Windows.Forms.ComboBox();
            this.taskMessageLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.wristStrapDisconnectCheckBox = new System.Windows.Forms.CheckBox();
            this.ledOffCheckBox = new System.Windows.Forms.CheckBox();
            this.ledOnCheckBox = new System.Windows.Forms.CheckBox();
            this.calibrationGroupBox = new System.Windows.Forms.GroupBox();
            this.queryLimitBtn = new System.Windows.Forms.Button();
            this.setNewLimitBtn = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.lowLimitRtb = new System.Windows.Forms.RichTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.highLimitRtb = new System.Windows.Forms.RichTextBox();
            this.minLabel = new System.Windows.Forms.Label();
            this.nominalLabel = new System.Windows.Forms.Label();
            this.maxLabel = new System.Windows.Forms.Label();
            this.sampleSizeLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.enableStartupCheckBox = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.wristStrapNotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.calibrationGroupBox.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // msgRtb
            // 
            this.msgRtb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msgRtb.Location = new System.Drawing.Point(769, 3);
            this.msgRtb.Name = "msgRtb";
            this.msgRtb.Size = new System.Drawing.Size(507, 274);
            this.msgRtb.TabIndex = 1;
            this.msgRtb.Text = "";
            this.msgRtb.TextChanged += new System.EventHandler(this.msgRtb_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.Controls.Add(this.msgRtb, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.calibrationGroupBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52.23048F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47.76952F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1279, 538);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.button1, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.disableMonitoringBtn, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.getWristStrapStateBtn, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.calibrationBtn, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(249, 274);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(3, 219);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(243, 52);
            this.button1.TabIndex = 8;
            this.button1.Text = "View Log File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(243, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // disableMonitoringBtn
            // 
            this.disableMonitoringBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.disableMonitoringBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.disableMonitoringBtn.Location = new System.Drawing.Point(3, 111);
            this.disableMonitoringBtn.Name = "disableMonitoringBtn";
            this.disableMonitoringBtn.Size = new System.Drawing.Size(243, 48);
            this.disableMonitoringBtn.TabIndex = 4;
            this.disableMonitoringBtn.Text = "Disable Monitoring";
            this.disableMonitoringBtn.UseVisualStyleBackColor = true;
            this.disableMonitoringBtn.Click += new System.EventHandler(this.disableMonitoringBtn_Click);
            // 
            // getWristStrapStateBtn
            // 
            this.getWristStrapStateBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.getWristStrapStateBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.getWristStrapStateBtn.Location = new System.Drawing.Point(3, 57);
            this.getWristStrapStateBtn.Name = "getWristStrapStateBtn";
            this.getWristStrapStateBtn.Size = new System.Drawing.Size(243, 48);
            this.getWristStrapStateBtn.TabIndex = 3;
            this.getWristStrapStateBtn.Text = "Get State";
            this.getWristStrapStateBtn.UseVisualStyleBackColor = true;
            this.getWristStrapStateBtn.Click += new System.EventHandler(this.getWristStrapStateBtn_Click);
            // 
            // calibrationBtn
            // 
            this.calibrationBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calibrationBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calibrationBtn.Location = new System.Drawing.Point(3, 165);
            this.calibrationBtn.Name = "calibrationBtn";
            this.calibrationBtn.Size = new System.Drawing.Size(243, 48);
            this.calibrationBtn.TabIndex = 0;
            this.calibrationBtn.Text = "Calibration";
            this.calibrationBtn.UseVisualStyleBackColor = true;
            this.calibrationBtn.Click += new System.EventHandler(this.calibrationBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.timerComboBox);
            this.panel1.Controls.Add(this.taskMessageLabel);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.wristStrapDisconnectCheckBox);
            this.panel1.Controls.Add(this.ledOffCheckBox);
            this.panel1.Controls.Add(this.ledOnCheckBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(258, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(505, 274);
            this.panel1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(20, 211);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(275, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Lock Off Timer Delay Interval (Second)";
            // 
            // timerComboBox
            // 
            this.timerComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timerComboBox.FormattingEnabled = true;
            this.timerComboBox.Location = new System.Drawing.Point(23, 230);
            this.timerComboBox.Name = "timerComboBox";
            this.timerComboBox.Size = new System.Drawing.Size(92, 39);
            this.timerComboBox.TabIndex = 6;
            this.timerComboBox.SelectedIndexChanged += new System.EventHandler(this.timerComboBox_SelectedIndexChanged);
            // 
            // taskMessageLabel
            // 
            this.taskMessageLabel.AutoSize = true;
            this.taskMessageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.taskMessageLabel.Location = new System.Drawing.Point(16, 162);
            this.taskMessageLabel.Name = "taskMessageLabel";
            this.taskMessageLabel.Size = new System.Drawing.Size(115, 39);
            this.taskMessageLabel.TabIndex = 3;
            this.taskMessageLabel.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Wrist Strap Status";
            // 
            // wristStrapDisconnectCheckBox
            // 
            this.wristStrapDisconnectCheckBox.AutoSize = true;
            this.wristStrapDisconnectCheckBox.Location = new System.Drawing.Point(23, 116);
            this.wristStrapDisconnectCheckBox.Name = "wristStrapDisconnectCheckBox";
            this.wristStrapDisconnectCheckBox.Size = new System.Drawing.Size(135, 17);
            this.wristStrapDisconnectCheckBox.TabIndex = 2;
            this.wristStrapDisconnectCheckBox.Text = "Wrist Strap Disconnect";
            this.wristStrapDisconnectCheckBox.UseVisualStyleBackColor = true;
            // 
            // ledOffCheckBox
            // 
            this.ledOffCheckBox.AutoSize = true;
            this.ledOffCheckBox.Location = new System.Drawing.Point(23, 82);
            this.ledOffCheckBox.Name = "ledOffCheckBox";
            this.ledOffCheckBox.Size = new System.Drawing.Size(92, 17);
            this.ledOffCheckBox.TabIndex = 1;
            this.ledOffCheckBox.Text = "User strap out";
            this.ledOffCheckBox.UseVisualStyleBackColor = true;
            // 
            // ledOnCheckBox
            // 
            this.ledOnCheckBox.AutoSize = true;
            this.ledOnCheckBox.Location = new System.Drawing.Point(23, 50);
            this.ledOnCheckBox.Name = "ledOnCheckBox";
            this.ledOnCheckBox.Size = new System.Drawing.Size(85, 17);
            this.ledOnCheckBox.TabIndex = 0;
            this.ledOnCheckBox.Text = "User strap in";
            this.ledOnCheckBox.UseVisualStyleBackColor = true;
            // 
            // calibrationGroupBox
            // 
            this.calibrationGroupBox.Controls.Add(this.queryLimitBtn);
            this.calibrationGroupBox.Controls.Add(this.setNewLimitBtn);
            this.calibrationGroupBox.Controls.Add(this.label8);
            this.calibrationGroupBox.Controls.Add(this.lowLimitRtb);
            this.calibrationGroupBox.Controls.Add(this.label7);
            this.calibrationGroupBox.Controls.Add(this.highLimitRtb);
            this.calibrationGroupBox.Controls.Add(this.minLabel);
            this.calibrationGroupBox.Controls.Add(this.nominalLabel);
            this.calibrationGroupBox.Controls.Add(this.maxLabel);
            this.calibrationGroupBox.Controls.Add(this.sampleSizeLabel);
            this.calibrationGroupBox.Controls.Add(this.label6);
            this.calibrationGroupBox.Controls.Add(this.label5);
            this.calibrationGroupBox.Controls.Add(this.label4);
            this.calibrationGroupBox.Controls.Add(this.label3);
            this.calibrationGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calibrationGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calibrationGroupBox.Location = new System.Drawing.Point(258, 283);
            this.calibrationGroupBox.Name = "calibrationGroupBox";
            this.calibrationGroupBox.Size = new System.Drawing.Size(505, 252);
            this.calibrationGroupBox.TabIndex = 4;
            this.calibrationGroupBox.TabStop = false;
            this.calibrationGroupBox.Text = "Calibration Section";
            // 
            // queryLimitBtn
            // 
            this.queryLimitBtn.Location = new System.Drawing.Point(6, 133);
            this.queryLimitBtn.Name = "queryLimitBtn";
            this.queryLimitBtn.Size = new System.Drawing.Size(98, 61);
            this.queryLimitBtn.TabIndex = 14;
            this.queryLimitBtn.Text = "Query Limit";
            this.queryLimitBtn.UseVisualStyleBackColor = true;
            this.queryLimitBtn.Click += new System.EventHandler(this.queryLimitBtn_Click);
            // 
            // setNewLimitBtn
            // 
            this.setNewLimitBtn.Location = new System.Drawing.Point(227, 200);
            this.setNewLimitBtn.Name = "setNewLimitBtn";
            this.setNewLimitBtn.Size = new System.Drawing.Size(153, 34);
            this.setNewLimitBtn.TabIndex = 13;
            this.setNewLimitBtn.Text = "Set New Limit";
            this.setNewLimitBtn.UseVisualStyleBackColor = true;
            this.setNewLimitBtn.Click += new System.EventHandler(this.setNewLimitBtn_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(127, 174);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 20);
            this.label8.TabIndex = 12;
            this.label8.Text = "Low Limit :";
            // 
            // lowLimitRtb
            // 
            this.lowLimitRtb.Location = new System.Drawing.Point(227, 169);
            this.lowLimitRtb.Name = "lowLimitRtb";
            this.lowLimitRtb.Size = new System.Drawing.Size(119, 25);
            this.lowLimitRtb.TabIndex = 11;
            this.lowLimitRtb.Text = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(122, 136);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 20);
            this.label7.TabIndex = 10;
            this.label7.Text = "High Limit :";
            // 
            // highLimitRtb
            // 
            this.highLimitRtb.Location = new System.Drawing.Point(227, 136);
            this.highLimitRtb.Name = "highLimitRtb";
            this.highLimitRtb.Size = new System.Drawing.Size(119, 25);
            this.highLimitRtb.TabIndex = 9;
            this.highLimitRtb.Text = "";
            // 
            // minLabel
            // 
            this.minLabel.AutoSize = true;
            this.minLabel.Location = new System.Drawing.Point(139, 92);
            this.minLabel.Name = "minLabel";
            this.minLabel.Size = new System.Drawing.Size(19, 20);
            this.minLabel.TabIndex = 8;
            this.minLabel.Text = "0";
            // 
            // nominalLabel
            // 
            this.nominalLabel.AutoSize = true;
            this.nominalLabel.Location = new System.Drawing.Point(139, 72);
            this.nominalLabel.Name = "nominalLabel";
            this.nominalLabel.Size = new System.Drawing.Size(19, 20);
            this.nominalLabel.TabIndex = 7;
            this.nominalLabel.Text = "0";
            // 
            // maxLabel
            // 
            this.maxLabel.AutoSize = true;
            this.maxLabel.Location = new System.Drawing.Point(139, 52);
            this.maxLabel.Name = "maxLabel";
            this.maxLabel.Size = new System.Drawing.Size(19, 20);
            this.maxLabel.TabIndex = 6;
            this.maxLabel.Text = "0";
            // 
            // sampleSizeLabel
            // 
            this.sampleSizeLabel.AutoSize = true;
            this.sampleSizeLabel.Location = new System.Drawing.Point(139, 32);
            this.sampleSizeLabel.Name = "sampleSizeLabel";
            this.sampleSizeLabel.Size = new System.Drawing.Size(19, 20);
            this.sampleSizeLabel.TabIndex = 5;
            this.sampleSizeLabel.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(91, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 20);
            this.label6.TabIndex = 4;
            this.label6.Text = "Min :";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(55, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 20);
            this.label5.TabIndex = 3;
            this.label5.Text = "Nominal :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(87, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Max :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "Sample Size :";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.enableStartupCheckBox, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 283);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(249, 252);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // enableStartupCheckBox
            // 
            this.enableStartupCheckBox.AutoSize = true;
            this.enableStartupCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enableStartupCheckBox.Location = new System.Drawing.Point(3, 204);
            this.enableStartupCheckBox.Name = "enableStartupCheckBox";
            this.enableStartupCheckBox.Size = new System.Drawing.Size(243, 45);
            this.enableStartupCheckBox.TabIndex = 15;
            this.enableStartupCheckBox.Text = "Enable Startup";
            this.enableStartupCheckBox.UseVisualStyleBackColor = true;
            this.enableStartupCheckBox.CheckedChanged += new System.EventHandler(this.enableStartupCheckBox_CheckedChanged);
            this.enableStartupCheckBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.enableStartupCheckBox_MouseDown);
            this.enableStartupCheckBox.MouseEnter += new System.EventHandler(this.enableStartupCheckBox_MouseEnter);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // wristStrapNotify
            // 
            this.wristStrapNotify.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.wristStrapNotify.Icon = ((System.Drawing.Icon)(resources.GetObject("wristStrapNotify.Icon")));
            this.wristStrapNotify.Text = "wristStrapNotify";
            this.wristStrapNotify.Visible = true;
            this.wristStrapNotify.DoubleClick += new System.EventHandler(this.wristStrapNotify_DoubleClick);
            this.wristStrapNotify.MouseClick += new System.Windows.Forms.MouseEventHandler(this.wristStrapNotify_MouseClick);
            this.wristStrapNotify.MouseMove += new System.Windows.Forms.MouseEventHandler(this.wristStrapNotify_MouseMove);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1279, 538);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "ESD Wrist Strap Monitoring System, Jabil Automation TND";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.calibrationGroupBox.ResumeLayout(false);
            this.calibrationGroupBox.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox msgRtb;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button getWristStrapStateBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox wristStrapDisconnectCheckBox;
        private System.Windows.Forms.CheckBox ledOffCheckBox;
        private System.Windows.Forms.CheckBox ledOnCheckBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label taskMessageLabel;
        private System.Windows.Forms.Button disableMonitoringBtn;
        private System.Windows.Forms.NotifyIcon wristStrapNotify;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox timerComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox calibrationGroupBox;
        private System.Windows.Forms.Button calibrationBtn;
        private System.Windows.Forms.Label minLabel;
        private System.Windows.Forms.Label nominalLabel;
        private System.Windows.Forms.Label maxLabel;
        private System.Windows.Forms.Label sampleSizeLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button setNewLimitBtn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RichTextBox lowLimitRtb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RichTextBox highLimitRtb;
        private System.Windows.Forms.Button queryLimitBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox enableStartupCheckBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    }
}

