namespace RDP_Portal {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            checkBoxKeepOpening = new System.Windows.Forms.CheckBox();
            buttonAbout = new System.Windows.Forms.Button();
            buttonConnect = new System.Windows.Forms.Button();
            textBoxComputer = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            textBoxUsername = new System.Windows.Forms.TextBox();
            textBoxPassword = new System.Windows.Forms.TextBox();
            textBoxDomain = new System.Windows.Forms.TextBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            groupBoxAdvanced = new System.Windows.Forms.GroupBox();
            textBoxRawRdp = new System.Windows.Forms.TextBox();
            buttonImportRdp = new System.Windows.Forms.Button();
            buttonExportRdp = new System.Windows.Forms.Button();
            buttonPreviewRdp = new System.Windows.Forms.Button();
            buttonSave = new System.Windows.Forms.Button();
            buttonOptions = new System.Windows.Forms.Button();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            buttonCancel = new System.Windows.Forms.Button();
            buttonEdit = new System.Windows.Forms.Button();
            textBoxName = new System.Windows.Forms.TextBox();
            buttonImport = new System.Windows.Forms.Button();
            buttonExport = new System.Windows.Forms.Button();
            treeViewProfiles = new System.Windows.Forms.TreeView();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // checkBoxKeepOpening
            // 
            checkBoxKeepOpening.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            checkBoxKeepOpening.Location = new System.Drawing.Point(11, 459);
            checkBoxKeepOpening.Margin = new System.Windows.Forms.Padding(4);
            checkBoxKeepOpening.Name = "checkBoxKeepOpening";
            checkBoxKeepOpening.Size = new System.Drawing.Size(340, 32);
            checkBoxKeepOpening.TabIndex = 1;
            checkBoxKeepOpening.Text = "Keep opening RDP Portal";
            checkBoxKeepOpening.UseVisualStyleBackColor = true;
            checkBoxKeepOpening.CheckedChanged += checkBoxKeepOpening_CheckedChanged;
            // 
            // buttonAbout
            // 
            buttonAbout.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonAbout.Location = new System.Drawing.Point(212, 421);
            buttonAbout.Margin = new System.Windows.Forms.Padding(4);
            buttonAbout.Name = "buttonAbout";
            buttonAbout.Size = new System.Drawing.Size(88, 30);
            buttonAbout.TabIndex = 2;
            buttonAbout.Text = "About";
            buttonAbout.UseVisualStyleBackColor = true;
            buttonAbout.Click += buttonAbout_Click;
            // 
            // buttonConnect
            // 
            buttonConnect.Location = new System.Drawing.Point(0, 0);
            buttonConnect.Name = "buttonConnect";
            buttonConnect.Size = new System.Drawing.Size(66, 24);
            buttonConnect.TabIndex = 22;
            // 
            // textBoxComputer
            // 
            textBoxComputer.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxComputer.Location = new System.Drawing.Point(108, 34);
            textBoxComputer.Margin = new System.Windows.Forms.Padding(4);
            textBoxComputer.Name = "textBoxComputer";
            textBoxComputer.Size = new System.Drawing.Size(202, 23);
            textBoxComputer.TabIndex = 4;
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(7, 38);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(94, 22);
            label1.TabIndex = 5;
            label1.Text = "Computer:";
            // 
            // label2
            // 
            label2.Location = new System.Drawing.Point(7, 78);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(94, 24);
            label2.TabIndex = 6;
            label2.Text = "User name:";
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(7, 117);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(94, 24);
            label3.TabIndex = 7;
            label3.Text = "Password:";
            // 
            // label4
            // 
            label4.Location = new System.Drawing.Point(7, 155);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(94, 24);
            label4.TabIndex = 8;
            label4.Text = "Domain:";
            // 
            // textBoxUsername
            // 
            textBoxUsername.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxUsername.Location = new System.Drawing.Point(108, 73);
            textBoxUsername.Margin = new System.Windows.Forms.Padding(4);
            textBoxUsername.Name = "textBoxUsername";
            textBoxUsername.Size = new System.Drawing.Size(202, 23);
            textBoxUsername.TabIndex = 10;
            // 
            // textBoxPassword
            // 
            textBoxPassword.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxPassword.Location = new System.Drawing.Point(108, 113);
            textBoxPassword.Margin = new System.Windows.Forms.Padding(4);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new System.Drawing.Size(201, 23);
            textBoxPassword.TabIndex = 11;
            textBoxPassword.UseSystemPasswordChar = true;
            // 
            // textBoxDomain
            // 
            textBoxDomain.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxDomain.Location = new System.Drawing.Point(108, 152);
            textBoxDomain.Margin = new System.Windows.Forms.Padding(4);
            textBoxDomain.Name = "textBoxDomain";
            textBoxDomain.Size = new System.Drawing.Size(201, 23);
            textBoxDomain.TabIndex = 12;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(textBoxDomain);
            groupBox1.Controls.Add(textBoxComputer);
            groupBox1.Controls.Add(textBoxPassword);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(textBoxUsername);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label4);
            groupBox1.Location = new System.Drawing.Point(205, 150);
            groupBox1.Margin = new System.Windows.Forms.Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4);
            groupBox1.Size = new System.Drawing.Size(328, 255);
            groupBox1.TabIndex = 13;
            groupBox1.TabStop = false;
            groupBox1.Text = "Connection";
            // 
            // groupBoxAdvanced
            // 
            groupBoxAdvanced.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBoxAdvanced.Location = new System.Drawing.Point(205, 410);
            groupBoxAdvanced.Margin = new System.Windows.Forms.Padding(4);
            groupBoxAdvanced.Name = "groupBoxAdvanced";
            groupBoxAdvanced.Padding = new System.Windows.Forms.Padding(4);
            groupBoxAdvanced.Size = new System.Drawing.Size(328, 88);
            groupBoxAdvanced.TabIndex = 30;
            groupBoxAdvanced.TabStop = false;
            groupBoxAdvanced.Text = "Advanced RDP Parameters";

            // textBoxRawRdp
            textBoxRawRdp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxRawRdp.Location = new System.Drawing.Point(8, 22);
            textBoxRawRdp.Multiline = true;
            textBoxRawRdp.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBoxRawRdp.Name = "textBoxRawRdp";
            textBoxRawRdp.Size = new System.Drawing.Size(228, 56);
            textBoxRawRdp.TabIndex = 31;

            // buttonImportRdp
            buttonImportRdp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonImportRdp.Location = new System.Drawing.Point(244, 22);
            buttonImportRdp.Name = "buttonImportRdp";
            buttonImportRdp.Size = new System.Drawing.Size(72, 26);
            buttonImportRdp.TabIndex = 32;
            buttonImportRdp.Text = "Import";
            buttonImportRdp.UseVisualStyleBackColor = true;
            buttonImportRdp.Click += buttonImportRdp_Click;

            // buttonExportRdp
            buttonExportRdp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonExportRdp.Location = new System.Drawing.Point(244, 50);
            buttonExportRdp.Name = "buttonExportRdp";
            buttonExportRdp.Size = new System.Drawing.Size(72, 26);
            buttonExportRdp.TabIndex = 33;
            buttonExportRdp.Text = "Export";
            buttonExportRdp.UseVisualStyleBackColor = true;
            buttonExportRdp.Click += buttonExportRdp_Click;

            // buttonPreviewRdp
            buttonPreviewRdp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonPreviewRdp.Location = new System.Drawing.Point(244, 78);
            buttonPreviewRdp.Name = "buttonPreviewRdp";
            buttonPreviewRdp.Size = new System.Drawing.Size(72, 26);
            buttonPreviewRdp.TabIndex = 34;
            buttonPreviewRdp.Text = "Preview";
            buttonPreviewRdp.UseVisualStyleBackColor = true;
            buttonPreviewRdp.Click += buttonPreviewRdp_Click;
            // 
            // buttonSave
            // 
            buttonSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonSave.Location = new System.Drawing.Point(222, 360);
            buttonSave.Margin = new System.Windows.Forms.Padding(4);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new System.Drawing.Size(88, 30);
            buttonSave.TabIndex = 14;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonOptions
            // 
            buttonOptions.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonOptions.Location = new System.Drawing.Point(445, 360);
            buttonOptions.Margin = new System.Windows.Forms.Padding(4);
            buttonOptions.Name = "buttonOptions";
            buttonOptions.Size = new System.Drawing.Size(88, 30);
            buttonOptions.TabIndex = 16;
            buttonOptions.Text = "Options";
            buttonOptions.UseVisualStyleBackColor = true;
            buttonOptions.Click += buttonMoreOptions_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new System.Drawing.Point(0, 0);
            pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(544, 96);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 17;
            pictureBox1.TabStop = false;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonCancel.Location = new System.Drawing.Point(317, 360);
            buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(88, 30);
            buttonCancel.TabIndex = 18;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonEdit
            // 
            buttonEdit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            buttonEdit.Location = new System.Drawing.Point(222, 360);
            buttonEdit.Margin = new System.Windows.Forms.Padding(4);
            buttonEdit.Name = "buttonEdit";
            buttonEdit.Size = new System.Drawing.Size(88, 30);
            buttonEdit.TabIndex = 19;
            buttonEdit.Text = "Edit";
            buttonEdit.UseVisualStyleBackColor = true;
            buttonEdit.Click += buttonEdit_Click;
            // 
            // textBoxName
            // 
            textBoxName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxName.Location = new System.Drawing.Point(205, 114);
            textBoxName.Margin = new System.Windows.Forms.Padding(4);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new System.Drawing.Size(328, 23);
            textBoxName.TabIndex = 13;
            // 
            // buttonImport
            // 
            buttonImport.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            buttonImport.Location = new System.Drawing.Point(13, 421);
            buttonImport.Margin = new System.Windows.Forms.Padding(4);
            buttonImport.Name = "buttonImport";
            buttonImport.Size = new System.Drawing.Size(88, 30);
            buttonImport.TabIndex = 20;
            buttonImport.Text = "Import";
            buttonImport.UseVisualStyleBackColor = true;
            buttonImport.Click += buttonImport_Click;
            // 
            // buttonExport
            // 
            buttonExport.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            buttonExport.Location = new System.Drawing.Point(108, 421);
            buttonExport.Margin = new System.Windows.Forms.Padding(4);
            buttonExport.Name = "buttonExport";
            buttonExport.Size = new System.Drawing.Size(88, 30);
            buttonExport.TabIndex = 21;
            buttonExport.Text = "Export";
            buttonExport.UseVisualStyleBackColor = true;
            buttonExport.Click += buttonExport_Click;
            // 
            // treeViewProfiles
            // 
            treeViewProfiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            treeViewProfiles.Location = new System.Drawing.Point(11, 132);
            treeViewProfiles.Margin = new System.Windows.Forms.Padding(4);
            treeViewProfiles.Name = "treeViewProfiles";
            treeViewProfiles.Size = new System.Drawing.Size(185, 259);
            treeViewProfiles.TabIndex = 0;
            treeViewProfiles.AfterSelect += treeViewProfiles_AfterSelect;
            treeViewProfiles.NodeMouseDoubleClick += treeViewProfiles_NodeMouseDoubleClick;
            
            // add advanced groupbox children
            groupBoxAdvanced.Controls.Add(textBoxRawRdp);
            groupBoxAdvanced.Controls.Add(buttonImportRdp);
            groupBoxAdvanced.Controls.Add(buttonExportRdp);
            groupBoxAdvanced.Controls.Add(buttonPreviewRdp);

            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            ClientSize = new System.Drawing.Size(544, 511);
            Controls.Add(textBoxName);
            Controls.Add(buttonEdit);
            Controls.Add(buttonCancel);
            Controls.Add(pictureBox1);
            Controls.Add(buttonOptions);
            Controls.Add(buttonImport);
            Controls.Add(buttonExport);
            Controls.Add(groupBoxAdvanced);
            Controls.Add(buttonSave);
            Controls.Add(groupBox1);
            Controls.Add(buttonConnect);
            Controls.Add(buttonAbout);
            Controls.Add(checkBoxKeepOpening);
            Controls.Add(treeViewProfiles);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Location = new System.Drawing.Point(15, 15);
            Margin = new System.Windows.Forms.Padding(4);
            MaximumSize = new System.Drawing.Size(702, 848);
            MinimumSize = new System.Drawing.Size(560, 550);
            Name = "MainForm";
            Text = "RDP Portal";
            Load += MainForm_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.TextBox textBoxName;

        private System.Windows.Forms.Button buttonEdit;

        private System.Windows.Forms.TreeView treeViewProfiles;

        private System.Windows.Forms.Button buttonCancel;

        private System.Windows.Forms.PictureBox pictureBox1;

        private System.Windows.Forms.Button buttonOptions;

        private System.Windows.Forms.Button buttonSave;

        private System.Windows.Forms.GroupBox groupBox1;

        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxDomain;

        

        private System.Windows.Forms.TextBox textBoxComputer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.Button buttonConnect;

        private System.Windows.Forms.Button buttonAbout;

        private System.Windows.Forms.CheckBox checkBoxKeepOpening;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.GroupBox groupBoxAdvanced;
        private System.Windows.Forms.TextBox textBoxRawRdp;
        private System.Windows.Forms.Button buttonImportRdp;
        private System.Windows.Forms.Button buttonExportRdp;
        private System.Windows.Forms.Button buttonPreviewRdp;
        // Group/Filter UI removed; groups now managed in tree view

        #endregion
    }
}
