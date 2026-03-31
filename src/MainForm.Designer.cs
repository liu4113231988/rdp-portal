namespace RDP_Portal {
    partial class MainForm {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            pictureBox1 = new System.Windows.Forms.PictureBox();
            treeViewProfiles = new System.Windows.Forms.TreeView();
            textBoxName = new System.Windows.Forms.TextBox();
            panelSettings = new System.Windows.Forms.Panel();
            tableLayoutPanelSettings = new System.Windows.Forms.TableLayoutPanel();
            label1 = new System.Windows.Forms.Label();
            textBoxComputer = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            textBoxUsername = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            textBoxPassword = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            textBoxDomain = new System.Windows.Forms.TextBox();
            labelResolution = new System.Windows.Forms.Label();
            comboBoxResolution = new System.Windows.Forms.ComboBox();
            labelScreenMode = new System.Windows.Forms.Label();
            comboBoxScreenMode = new System.Windows.Forms.ComboBox();
            labelColorDepth = new System.Windows.Forms.Label();
            comboBoxColorDepth = new System.Windows.Forms.ComboBox();
            labelAudioMode = new System.Windows.Forms.Label();
            comboBoxAudioMode = new System.Windows.Forms.ComboBox();
            checkBoxRedirectPrinters = new System.Windows.Forms.CheckBox();
            checkBoxRedirectClipboard = new System.Windows.Forms.CheckBox();
            checkBoxRedirectDrives = new System.Windows.Forms.CheckBox();
            checkBoxRedirectPorts = new System.Windows.Forms.CheckBox();
            checkBoxRedirectSmartCards = new System.Windows.Forms.CheckBox();
            checkBoxPromptCredentials = new System.Windows.Forms.CheckBox();
            panelButtons = new System.Windows.Forms.Panel();
            buttonConnect = new System.Windows.Forms.Button();
            buttonCancel = new System.Windows.Forms.Button();
            buttonOptions = new System.Windows.Forms.Button();
            buttonSave = new System.Windows.Forms.Button();
            panelBottom = new System.Windows.Forms.Panel();
            buttonImport = new System.Windows.Forms.Button();
            buttonExport = new System.Windows.Forms.Button();
            buttonImportGroups = new System.Windows.Forms.Button();
            buttonExportGroups = new System.Windows.Forms.Button();
            buttonAbout = new System.Windows.Forms.Button();
            checkBoxKeepOpening = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panelSettings.SuspendLayout();
            tableLayoutPanelSettings.SuspendLayout();
            panelButtons.SuspendLayout();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new System.Drawing.Point(0, 0);
            pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(628, 91);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            imageListTreeView = new System.Windows.Forms.ImageList();
            imageListTreeView.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            imageListTreeView.ImageSize = new System.Drawing.Size(16, 16);
            // 
            // treeViewProfiles
            // 
            treeViewProfiles.AllowDrop = true;
            treeViewProfiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            treeViewProfiles.ImageIndex = 0;
            treeViewProfiles.ImageList = imageListTreeView;
            treeViewProfiles.Location = new System.Drawing.Point(12, 113);
            treeViewProfiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            treeViewProfiles.Name = "treeViewProfiles";
            treeViewProfiles.SelectedImageIndex = 0;
            treeViewProfiles.Size = new System.Drawing.Size(180, 475);
            treeViewProfiles.TabIndex = 1;
            treeViewProfiles.AfterSelect += treeViewProfiles_AfterSelect;
            treeViewProfiles.NodeMouseDoubleClick += treeViewProfiles_NodeMouseDoubleClick;
            treeViewProfiles.MouseClick += treeViewProfiles_MouseClick;
            treeViewProfiles.MouseDoubleClick += treeViewProfiles_MouseDoubleClick;
            treeViewProfiles.ItemDrag += treeViewProfiles_ItemDrag;
            treeViewProfiles.DragEnter += treeViewProfiles_DragEnter;
            treeViewProfiles.DragDrop += treeViewProfiles_DragDrop;
            // 
            // textBoxName
            // 
            textBoxName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxName.Location = new System.Drawing.Point(200, 113);
            textBoxName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new System.Drawing.Size(416, 23);
            textBoxName.TabIndex = 2;
            // 
            // panelSettings
            // 
            panelSettings.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panelSettings.Controls.Add(tableLayoutPanelSettings);
            panelSettings.Location = new System.Drawing.Point(200, 147);
            panelSettings.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            panelSettings.Name = "panelSettings";
            panelSettings.Size = new System.Drawing.Size(416, 442);
            panelSettings.TabIndex = 3;
            // 
            // tableLayoutPanelSettings
            // 
            tableLayoutPanelSettings.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tableLayoutPanelSettings.ColumnCount = 3;
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            tableLayoutPanelSettings.Controls.Add(label1, 0, 0);
            tableLayoutPanelSettings.Controls.Add(textBoxComputer, 1, 0);
            tableLayoutPanelSettings.Controls.Add(label2, 0, 1);
            tableLayoutPanelSettings.Controls.Add(textBoxUsername, 1, 1);
            tableLayoutPanelSettings.Controls.Add(label3, 0, 2);
            tableLayoutPanelSettings.Controls.Add(textBoxPassword, 1, 2);
            tableLayoutPanelSettings.Controls.Add(label4, 0, 3);
            tableLayoutPanelSettings.Controls.Add(textBoxDomain, 1, 3);
            tableLayoutPanelSettings.Controls.Add(labelResolution, 0, 4);
            tableLayoutPanelSettings.Controls.Add(comboBoxResolution, 1, 4);
            tableLayoutPanelSettings.Controls.Add(labelScreenMode, 0, 5);
            tableLayoutPanelSettings.Controls.Add(comboBoxScreenMode, 1, 5);
            tableLayoutPanelSettings.Controls.Add(labelColorDepth, 0, 6);
            tableLayoutPanelSettings.Controls.Add(comboBoxColorDepth, 1, 6);
            tableLayoutPanelSettings.Controls.Add(labelAudioMode, 0, 7);
            tableLayoutPanelSettings.Controls.Add(comboBoxAudioMode, 1, 7);
            tableLayoutPanelSettings.Controls.Add(checkBoxRedirectPrinters, 2, 0);
            tableLayoutPanelSettings.Controls.Add(checkBoxRedirectClipboard, 2, 1);
            tableLayoutPanelSettings.Controls.Add(checkBoxRedirectDrives, 2, 2);
            tableLayoutPanelSettings.Controls.Add(checkBoxRedirectPorts, 2, 3);
            tableLayoutPanelSettings.Controls.Add(checkBoxRedirectSmartCards, 2, 4);
            tableLayoutPanelSettings.Controls.Add(checkBoxPromptCredentials, 2, 5);
            tableLayoutPanelSettings.Controls.Add(panelButtons, 0, 8);
            tableLayoutPanelSettings.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelSettings.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanelSettings.Name = "tableLayoutPanelSettings";
            tableLayoutPanelSettings.RowCount = 9;
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            tableLayoutPanelSettings.Size = new System.Drawing.Size(416, 335);
            tableLayoutPanelSettings.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(4, 9);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(92, 17);
            label1.TabIndex = 0;
            label1.Text = "Computer:";
            // 
            // textBoxComputer
            // 
            textBoxComputer.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxComputer.Location = new System.Drawing.Point(104, 6);
            textBoxComputer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxComputer.Name = "textBoxComputer";
            textBoxComputer.Size = new System.Drawing.Size(218, 23);
            textBoxComputer.TabIndex = 1;
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(4, 45);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(92, 17);
            label2.TabIndex = 2;
            label2.Text = "Username:";
            // 
            // textBoxUsername
            // 
            textBoxUsername.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxUsername.Location = new System.Drawing.Point(104, 42);
            textBoxUsername.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxUsername.Name = "textBoxUsername";
            textBoxUsername.Size = new System.Drawing.Size(218, 23);
            textBoxUsername.TabIndex = 3;
            // 
            // label3
            // 
            label3.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(4, 81);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(92, 17);
            label3.TabIndex = 4;
            label3.Text = "Password:";
            // 
            // textBoxPassword
            // 
            textBoxPassword.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxPassword.Location = new System.Drawing.Point(104, 78);
            textBoxPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new System.Drawing.Size(218, 23);
            textBoxPassword.TabIndex = 5;
            textBoxPassword.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            label4.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(4, 117);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(92, 17);
            label4.TabIndex = 6;
            label4.Text = "Domain:";
            // 
            // textBoxDomain
            // 
            textBoxDomain.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            textBoxDomain.Location = new System.Drawing.Point(104, 114);
            textBoxDomain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            textBoxDomain.Name = "textBoxDomain";
            textBoxDomain.Size = new System.Drawing.Size(218, 23);
            textBoxDomain.TabIndex = 7;
            // 
            // labelResolution
            // 
            labelResolution.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            labelResolution.AutoSize = true;
            labelResolution.Location = new System.Drawing.Point(4, 153);
            labelResolution.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelResolution.Name = "labelResolution";
            labelResolution.Size = new System.Drawing.Size(92, 17);
            labelResolution.TabIndex = 8;
            labelResolution.Text = "Resolution:";
            // 
            // comboBoxResolution
            // 
            comboBoxResolution.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            comboBoxResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxResolution.FormattingEnabled = true;
            comboBoxResolution.Items.AddRange(new object[] { "800x600", "1024x768", "1280x720", "1280x1024", "1366x768", "1440x900", "1600x900", "1680x1050", "1920x1080", "1920x1200", "2560x1440", "3840x2160" });
            comboBoxResolution.Location = new System.Drawing.Point(104, 149);
            comboBoxResolution.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            comboBoxResolution.Name = "comboBoxResolution";
            comboBoxResolution.Size = new System.Drawing.Size(218, 25);
            comboBoxResolution.TabIndex = 9;
            // 
            // labelScreenMode
            // 
            labelScreenMode.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            labelScreenMode.AutoSize = true;
            labelScreenMode.Location = new System.Drawing.Point(4, 189);
            labelScreenMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelScreenMode.Name = "labelScreenMode";
            labelScreenMode.Size = new System.Drawing.Size(92, 17);
            labelScreenMode.TabIndex = 10;
            labelScreenMode.Text = "Screen Mode:";
            // 
            // comboBoxScreenMode
            // 
            comboBoxScreenMode.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            comboBoxScreenMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxScreenMode.FormattingEnabled = true;
            comboBoxScreenMode.Items.AddRange(new object[] { "Windowed", "Full Screen", "All Monitors" });
            comboBoxScreenMode.Location = new System.Drawing.Point(104, 185);
            comboBoxScreenMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            comboBoxScreenMode.Name = "comboBoxScreenMode";
            comboBoxScreenMode.Size = new System.Drawing.Size(218, 25);
            comboBoxScreenMode.TabIndex = 11;
            // 
            // labelColorDepth
            // 
            labelColorDepth.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            labelColorDepth.AutoSize = true;
            labelColorDepth.Location = new System.Drawing.Point(4, 225);
            labelColorDepth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelColorDepth.Name = "labelColorDepth";
            labelColorDepth.Size = new System.Drawing.Size(92, 17);
            labelColorDepth.TabIndex = 12;
            labelColorDepth.Text = "Color Depth:";
            // 
            // comboBoxColorDepth
            // 
            comboBoxColorDepth.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            comboBoxColorDepth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxColorDepth.FormattingEnabled = true;
            comboBoxColorDepth.Items.AddRange(new object[] { "15 bit", "16 bit", "24 bit", "32 bit" });
            comboBoxColorDepth.Location = new System.Drawing.Point(104, 221);
            comboBoxColorDepth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            comboBoxColorDepth.Name = "comboBoxColorDepth";
            comboBoxColorDepth.Size = new System.Drawing.Size(218, 25);
            comboBoxColorDepth.TabIndex = 13;
            // 
            // labelAudioMode
            // 
            labelAudioMode.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            labelAudioMode.AutoSize = true;
            labelAudioMode.Location = new System.Drawing.Point(4, 261);
            labelAudioMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelAudioMode.Name = "labelAudioMode";
            labelAudioMode.Size = new System.Drawing.Size(92, 17);
            labelAudioMode.TabIndex = 14;
            labelAudioMode.Text = "Audio Mode:";
            // 
            // comboBoxAudioMode
            // 
            comboBoxAudioMode.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            comboBoxAudioMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxAudioMode.FormattingEnabled = true;
            comboBoxAudioMode.Items.AddRange(new object[] { "Play locally", "Play on remote", "Do not play" });
            comboBoxAudioMode.Location = new System.Drawing.Point(104, 257);
            comboBoxAudioMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            comboBoxAudioMode.Name = "comboBoxAudioMode";
            comboBoxAudioMode.Size = new System.Drawing.Size(218, 25);
            comboBoxAudioMode.TabIndex = 15;
            // 
            // checkBoxRedirectPrinters
            // 
            checkBoxRedirectPrinters.Anchor = System.Windows.Forms.AnchorStyles.Left;
            checkBoxRedirectPrinters.AutoSize = true;
            checkBoxRedirectPrinters.Location = new System.Drawing.Point(330, 7);
            checkBoxRedirectPrinters.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            checkBoxRedirectPrinters.Name = "checkBoxRedirectPrinters";
            checkBoxRedirectPrinters.Size = new System.Drawing.Size(71, 21);
            checkBoxRedirectPrinters.TabIndex = 16;
            checkBoxRedirectPrinters.Text = "Printers";
            // 
            // checkBoxRedirectClipboard
            // 
            checkBoxRedirectClipboard.Anchor = System.Windows.Forms.AnchorStyles.Left;
            checkBoxRedirectClipboard.AutoSize = true;
            checkBoxRedirectClipboard.Location = new System.Drawing.Point(330, 43);
            checkBoxRedirectClipboard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            checkBoxRedirectClipboard.Name = "checkBoxRedirectClipboard";
            checkBoxRedirectClipboard.Size = new System.Drawing.Size(82, 21);
            checkBoxRedirectClipboard.TabIndex = 17;
            checkBoxRedirectClipboard.Text = "Clipboard";
            // 
            // checkBoxRedirectDrives
            // 
            checkBoxRedirectDrives.Anchor = System.Windows.Forms.AnchorStyles.Left;
            checkBoxRedirectDrives.AutoSize = true;
            checkBoxRedirectDrives.Location = new System.Drawing.Point(330, 79);
            checkBoxRedirectDrives.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            checkBoxRedirectDrives.Name = "checkBoxRedirectDrives";
            checkBoxRedirectDrives.Size = new System.Drawing.Size(63, 21);
            checkBoxRedirectDrives.TabIndex = 18;
            checkBoxRedirectDrives.Text = "Drives";
            // 
            // checkBoxRedirectPorts
            // 
            checkBoxRedirectPorts.Anchor = System.Windows.Forms.AnchorStyles.Left;
            checkBoxRedirectPorts.AutoSize = true;
            checkBoxRedirectPorts.Location = new System.Drawing.Point(330, 115);
            checkBoxRedirectPorts.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            checkBoxRedirectPorts.Name = "checkBoxRedirectPorts";
            checkBoxRedirectPorts.Size = new System.Drawing.Size(57, 21);
            checkBoxRedirectPorts.TabIndex = 19;
            checkBoxRedirectPorts.Text = "Ports";
            // 
            // checkBoxRedirectSmartCards
            // 
            checkBoxRedirectSmartCards.Anchor = System.Windows.Forms.AnchorStyles.Left;
            checkBoxRedirectSmartCards.AutoSize = true;
            checkBoxRedirectSmartCards.Location = new System.Drawing.Point(330, 151);
            checkBoxRedirectSmartCards.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            checkBoxRedirectSmartCards.Name = "checkBoxRedirectSmartCards";
            checkBoxRedirectSmartCards.Size = new System.Drawing.Size(82, 21);
            checkBoxRedirectSmartCards.TabIndex = 20;
            checkBoxRedirectSmartCards.Text = "Smart Cards";
            // 
            // checkBoxPromptCredentials
            // 
            checkBoxPromptCredentials.Anchor = System.Windows.Forms.AnchorStyles.Left;
            checkBoxPromptCredentials.AutoSize = true;
            checkBoxPromptCredentials.Location = new System.Drawing.Point(330, 187);
            checkBoxPromptCredentials.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            checkBoxPromptCredentials.Name = "checkBoxPromptCredentials";
            checkBoxPromptCredentials.Size = new System.Drawing.Size(82, 21);
            checkBoxPromptCredentials.TabIndex = 21;
            checkBoxPromptCredentials.Text = "Prompt Creds";
            // 
            // panelButtons
            // 
            panelButtons.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tableLayoutPanelSettings.SetColumnSpan(panelButtons, 3);
            panelButtons.Controls.Add(buttonConnect);
            panelButtons.Controls.Add(buttonCancel);
            panelButtons.Controls.Add(buttonOptions);
            panelButtons.Controls.Add(buttonSave);
            panelButtons.Location = new System.Drawing.Point(3, 292);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new System.Drawing.Size(410, 39);
            panelButtons.TabIndex = 22;
            // 
            // buttonConnect
            // 
            buttonConnect.Anchor = System.Windows.Forms.AnchorStyles.Left;
            buttonConnect.Location = new System.Drawing.Point(0, 5);
            buttonConnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buttonConnect.Name = "buttonConnect";
            buttonConnect.Size = new System.Drawing.Size(80, 29);
            buttonConnect.TabIndex = 0;
            buttonConnect.Text = "Connect";
            buttonConnect.UseVisualStyleBackColor = true;
            buttonConnect.Click += buttonConnect_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            buttonCancel.Location = new System.Drawing.Point(245, 5);
            buttonCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(80, 29);
            buttonCancel.TabIndex = 2;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonOptions
            // 
            buttonOptions.Anchor = System.Windows.Forms.AnchorStyles.Right;
            buttonOptions.Location = new System.Drawing.Point(330, 5);
            buttonOptions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buttonOptions.Name = "buttonOptions";
            buttonOptions.Size = new System.Drawing.Size(80, 29);
            buttonOptions.TabIndex = 1;
            buttonOptions.Text = "Options";
            buttonOptions.UseVisualStyleBackColor = true;
            buttonOptions.Click += buttonMoreOptions_Click;
            // 
            // buttonSave
            // 
            buttonSave.Anchor = System.Windows.Forms.AnchorStyles.Right;
            buttonSave.Location = new System.Drawing.Point(158, 5);
            buttonSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new System.Drawing.Size(80, 29);
            buttonSave.TabIndex = 3;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // panelBottom
            // 
            panelBottom.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            panelBottom.Controls.Add(buttonImport);
            panelBottom.Controls.Add(buttonExport);
            panelBottom.Controls.Add(buttonImportGroups);
            panelBottom.Controls.Add(buttonExportGroups);
            panelBottom.Controls.Add(buttonAbout);
            panelBottom.Controls.Add(checkBoxKeepOpening);
            panelBottom.Location = new System.Drawing.Point(0, 601);
            panelBottom.Margin = new System.Windows.Forms.Padding(0);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new System.Drawing.Size(628, 57);
            panelBottom.TabIndex = 4;
            // 
            // buttonImport
            // 
            buttonImport.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            buttonImport.Location = new System.Drawing.Point(12, 14);
            buttonImport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buttonImport.Name = "buttonImport";
            buttonImport.Size = new System.Drawing.Size(80, 34);
            buttonImport.TabIndex = 0;
            buttonImport.Text = "Import";
            buttonImport.UseVisualStyleBackColor = true;
            buttonImport.Click += buttonImport_Click;
            // 
            // buttonExport
            // 
            buttonExport.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            buttonExport.Location = new System.Drawing.Point(96, 14);
            buttonExport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buttonExport.Name = "buttonExport";
            buttonExport.Size = new System.Drawing.Size(80, 34);
            buttonExport.TabIndex = 1;
            buttonExport.Text = "Export";
            buttonExport.UseVisualStyleBackColor = true;
            buttonExport.Click += buttonExport_Click;
            // 
            // buttonImportGroups
            // 
            buttonImportGroups.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            buttonImportGroups.Location = new System.Drawing.Point(180, 14);
            buttonImportGroups.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buttonImportGroups.Name = "buttonImportGroups";
            buttonImportGroups.Size = new System.Drawing.Size(100, 34);
            buttonImportGroups.TabIndex = 4;
            buttonImportGroups.Text = "Import Groups";
            buttonImportGroups.UseVisualStyleBackColor = true;
            buttonImportGroups.Click += buttonImportGroups_Click;
            // 
            // buttonExportGroups
            // 
            buttonExportGroups.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            buttonExportGroups.Location = new System.Drawing.Point(284, 14);
            buttonExportGroups.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buttonExportGroups.Name = "buttonExportGroups";
            buttonExportGroups.Size = new System.Drawing.Size(100, 34);
            buttonExportGroups.TabIndex = 5;
            buttonExportGroups.Text = "Export Groups";
            buttonExportGroups.UseVisualStyleBackColor = true;
            buttonExportGroups.Click += buttonExportGroups_Click;
            // 
            // buttonAbout
            // 
            buttonAbout.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            buttonAbout.Location = new System.Drawing.Point(536, 14);
            buttonAbout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buttonAbout.Name = "buttonAbout";
            buttonAbout.Size = new System.Drawing.Size(80, 34);
            buttonAbout.TabIndex = 2;
            buttonAbout.Text = "About";
            buttonAbout.UseVisualStyleBackColor = true;
            buttonAbout.Click += buttonAbout_Click;
            // 
            // checkBoxKeepOpening
            // 
            checkBoxKeepOpening.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            checkBoxKeepOpening.AutoSize = true;
            checkBoxKeepOpening.Location = new System.Drawing.Point(353, 20);
            checkBoxKeepOpening.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            checkBoxKeepOpening.Name = "checkBoxKeepOpening";
            checkBoxKeepOpening.Size = new System.Drawing.Size(175, 21);
            checkBoxKeepOpening.TabIndex = 3;
            checkBoxKeepOpening.Text = "Keep opening RDP Portal";
            checkBoxKeepOpening.UseVisualStyleBackColor = true;
            checkBoxKeepOpening.CheckedChanged += checkBoxKeepOpening_CheckedChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            ClientSize = new System.Drawing.Size(628, 658);
            Controls.Add(panelSettings);
            Controls.Add(textBoxName);
            Controls.Add(treeViewProfiles);
            Controls.Add(pictureBox1);
            Controls.Add(panelBottom);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            MinimumSize = new System.Drawing.Size(600, 697);
            Name = "MainForm";
            Text = "RDP Portal";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panelSettings.ResumeLayout(false);
            tableLayoutPanelSettings.ResumeLayout(false);
            tableLayoutPanelSettings.PerformLayout();
            panelButtons.ResumeLayout(false);
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TreeView treeViewProfiles;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Panel panelSettings;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSettings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxComputer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxDomain;
        private System.Windows.Forms.Label labelResolution;
        private System.Windows.Forms.ComboBox comboBoxResolution;
        private System.Windows.Forms.Label labelScreenMode;
        private System.Windows.Forms.ComboBox comboBoxScreenMode;
        private System.Windows.Forms.Label labelColorDepth;
        private System.Windows.Forms.ComboBox comboBoxColorDepth;
        private System.Windows.Forms.Label labelAudioMode;
        private System.Windows.Forms.ComboBox comboBoxAudioMode;
        private System.Windows.Forms.CheckBox checkBoxRedirectPrinters;
        private System.Windows.Forms.CheckBox checkBoxRedirectClipboard;
        private System.Windows.Forms.CheckBox checkBoxRedirectDrives;
        private System.Windows.Forms.CheckBox checkBoxRedirectPorts;
        private System.Windows.Forms.CheckBox checkBoxRedirectSmartCards;
        private System.Windows.Forms.CheckBox checkBoxPromptCredentials;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonOptions;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Button buttonImportGroups;
        private System.Windows.Forms.Button buttonExportGroups;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.CheckBox checkBoxKeepOpening;
        private System.Windows.Forms.ImageList imageListTreeView;

        #endregion
    }
}
