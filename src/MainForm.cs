using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RDP_Portal
{
    public partial class MainForm : Form
    {

        private Config? _config;
        private bool _editMode = false;
        private Profile? selectedProfile = null;

        public MainForm()
        {
            try
            {
                InitializeComponent();
                _config = null!;
                Logger.Info("MainForm initialized");
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to initialize MainForm", ex);
                MessageBox.Show("LOAD ERROR!");
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.N))
            {
                AddNewProfile();
                return true;
            }
            if (keyData == (Keys.Control | Keys.S))
            {
                if (EditMode && treeViewProfiles.SelectedNode?.Tag is Profile)
                {
                    buttonSave_Click(this, EventArgs.Empty);
                }
                return true;
            }
            if (keyData == Keys.F5)
            {
                if (treeViewProfiles.SelectedNode?.Tag is Profile)
                {
                    buttonConnect_Click(this, EventArgs.Empty);
                }
                return true;
            }
            if (keyData == Keys.Delete)
            {
                if (treeViewProfiles.SelectedNode != null && !EditMode)
                {
                    buttonDelete_Click(this, EventArgs.Empty);
                }
                return true;
            }
            if (keyData == (Keys.Control | Keys.D))
            {
                if (treeViewProfiles.SelectedNode?.Tag is Profile)
                {
                    var cloneItem = new ToolStripMenuItem();
                    CloneProfile((Profile)treeViewProfiles.SelectedNode.Tag);
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void CloneProfile(Profile original)
        {
            try
            {
                var cloned = new Profile
                {
                    Name = original.Name + " (Copy)",
                    Computer = original.Computer,
                    Username = original.Username,
                    Password = original.Password,
                    Domain = original.Domain,
                    GroupName = original.GroupName,
                    DesktopWidth = original.DesktopWidth,
                    DesktopHeight = original.DesktopHeight,
                    ScreenMode = original.ScreenMode,
                    UseMultiMon = original.UseMultiMon,
                    ColorDepth = original.ColorDepth,
                    AudioMode = original.AudioMode,
                    RedirectPrinters = original.RedirectPrinters,
                    RedirectClipboard = original.RedirectClipboard,
                    RedirectDrives = original.RedirectDrives,
                    RedirectPorts = original.RedirectPorts,
                    RedirectSmartCards = original.RedirectSmartCards,
                    PromptForCredentials = original.PromptForCredentials,
                    AuthenticationLevel = original.AuthenticationLevel,
                    EnableCredSSPSupport = original.EnableCredSSPSupport,
                    JustAdded = true,
                    Filename = ""
                };
                _config!.Profiles.Add(cloned);
                PopulateTree(selectProfile: cloned);
                SelectProfile(true);
                EditMode = true;
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to clone profile", ex);
                MessageBox.Show("Failed to clone profile: " + ex.Message);
            }
        }

        [SupportedOSPlatform("windows")]
        private async void MainForm_Load(object sender, EventArgs e)
        {
            _config = await Config.GetConfigAsync();

            SetupTreeViewIcons();

            // Populate group combo box with existing groups
            UpdateGroupList();

            if (_config.Profiles.Count == 0)
            {
                AddNewProfile();
            }

            PopulateTree();

            checkBoxKeepOpening.Checked = _config.KeepOpening;

            // Restore window position and size
            if (_config.WindowLeft >= 0 && _config.WindowTop >= 0)
            {
                StartPosition = FormStartPosition.Manual;
                Location = new Point(_config.WindowLeft, _config.WindowTop);
            }
            if (_config.WindowWidth > 0 && _config.WindowHeight > 0)
            {
                Size = new Size(_config.WindowWidth, _config.WindowHeight);
            }

            // Ensure a profile is selected on startup and make right-side editable by default
            if (treeViewProfiles.SelectedNode == null)
            {
                if (treeViewProfiles.Nodes.Count > 0)
                {
                    var g = treeViewProfiles.Nodes[0];
                    if (g != null && g.Nodes.Count > 0)
                    {
                        treeViewProfiles.SelectedNode = g.Nodes[0];
                        SelectProfile(true);
                    }
                }
            }

            // Default to edit mode so fields are editable without pressing Edit
            EditMode = false;

            // create context menu for tree (dynamic visibility based on selected node)
            var cms = new ContextMenuStrip();

            var newGroupItem = new ToolStripMenuItem("New Group", null, (s, ev) =>
            {
                try
                {
                    if (_config == null)
                    {
                        MessageBox.Show("Configuration not loaded yet.");
                        return;
                    }

                    var name = Prompt.ShowDialog("Group name:", "New Group");
                    if (!String.IsNullOrWhiteSpace(name))
                    {
                        if (_config.Groups == null) _config.Groups = new List<Group>();
                        if (!_config.Groups.Any(t => t.GroupName.Equals(name, StringComparison.OrdinalIgnoreCase)))
                        {
                            _config.Groups.Add(new Group() { GroupName = name });
                            _config.SaveGroups();
                            _config.Groups = new ProfileRepository(new DatabaseContext()).GetAllGroups();
                            PopulateTree();
                        }
                        else
                        {
                            MessageBox.Show("Group '" + name + "' already exists.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Failed to create new group", ex);
                    MessageBox.Show("Failed to create group: " + ex.Message);
                }
            });

            var newProfileItem = new ToolStripMenuItem("New Profile", null, (s, ev) =>
            {
                try
                {
                    if (_config == null)
                    {
                        MessageBox.Show("Configuration not loaded yet.");
                        return;
                    }

                    var groupName = "";
                    if (treeViewProfiles.SelectedNode != null && !(treeViewProfiles.SelectedNode.Tag is Profile))
                    {
                        groupName = treeViewProfiles.SelectedNode.Text;
                    }
                    var p = new Profile() { JustAdded = true, GroupName = groupName };
                    _config.Profiles.Add(p);
                    PopulateTree(selectProfile: p);
                    SelectProfile(true);
                    EditMode = true;
                }
                catch (Exception ex)
                {
                    Logger.Error("Failed to create new profile", ex);
                    MessageBox.Show("Failed to create profile: " + ex.Message);
                }
            });

            var sep = new ToolStripSeparator();

            var editItem = new ToolStripMenuItem("Edit", null, (s, ev) =>
            {
                if (treeViewProfiles.SelectedNode == null) return;
                if (treeViewProfiles.SelectedNode.Tag is Profile)
                {
                    SelectProfile(true);
                    EditMode = true;
                }
                else
                {
                    var oldName = treeViewProfiles.SelectedNode.Text;
                    var newName = Prompt.ShowDialog("Rename group:", "Rename Group", oldName);
                    if (!String.IsNullOrWhiteSpace(newName) && !string.Equals(newName, oldName, StringComparison.OrdinalIgnoreCase))
                    {
                        var group = _config.Groups.Where(t => t.GroupName == oldName).FirstOrDefault();
                        if (group != null)
                        {
                            group.GroupName = newName;
                        }

                        foreach (var p in _config.Profiles)
                        {
                            if (!String.IsNullOrWhiteSpace(p.GroupName) && p.GroupName == oldName)
                            {
                                p.GroupName = newName;
                        _config!.Save();
                            }
                        }

                        _config.Groups = _config.Groups.OrderBy(t => t.GroupName).ToList();
                        _config.SaveGroups();

                        PopulateTree();
                    }
                }
            });

            var deleteItem = new ToolStripMenuItem("Delete", null, (s, ev) =>
            {
                if (treeViewProfiles.SelectedNode == null) return;
                if (treeViewProfiles.SelectedNode.Tag is Profile p)
                {
                    var ok = MessageBox.Show("Delete profile '" + p.Name + "'?", "Delete", MessageBoxButtons.YesNo);
                    if (ok == DialogResult.Yes)
                    {
                        p.Delete();
                        _config.DeleteProfile(p);
                        PopulateTree();
                    }
                }
                else
                {
                    var grp = treeViewProfiles.SelectedNode.Text;
                    var ok = MessageBox.Show("Delete group '" + grp + "' and all its profiles?", "Delete Group", MessageBoxButtons.YesNo);
                    if (ok == DialogResult.Yes)
                    {
                        var items = _config.Profiles.Where(x => (x.GroupName ?? "") == grp).ToList();
                        foreach (var ip in items)
                        {
                            ip.Delete();
                            _config.DeleteProfile(ip);
                        }

            if (_config?.Groups != null)
                        {
                            var group = _config.Groups.Where(t => t.GroupName == grp).FirstOrDefault();
                            if (group != null)
                            {
                                _config.Groups.Remove(group);
                                _config.SaveGroups();
                            }
                        }

                        PopulateTree();
                    }
                }
            });

            var connectItem = new ToolStripMenuItem("Connect", null, (s, ev) =>
            {
                if (treeViewProfiles.SelectedNode != null && treeViewProfiles.SelectedNode.Tag is Profile)
                {
                    buttonConnect_Click(s ?? this, EventArgs.Empty);
                }
            });

            var cloneItem = new ToolStripMenuItem("Clone", null, (s, ev) =>
            {
                if (treeViewProfiles.SelectedNode == null || !(treeViewProfiles.SelectedNode.Tag is Profile original)) return;
                try
                {
                    var cloned = new Profile
                    {
                        Name = original.Name + " (Copy)",
                        Computer = original.Computer,
                        Username = original.Username,
                        Password = original.Password,
                        Domain = original.Domain,
                        GroupName = original.GroupName,
                        DesktopWidth = original.DesktopWidth,
                        DesktopHeight = original.DesktopHeight,
                        ScreenMode = original.ScreenMode,
                        UseMultiMon = original.UseMultiMon,
                        ColorDepth = original.ColorDepth,
                        AudioMode = original.AudioMode,
                        RedirectPrinters = original.RedirectPrinters,
                        RedirectClipboard = original.RedirectClipboard,
                        RedirectDrives = original.RedirectDrives,
                        RedirectPorts = original.RedirectPorts,
                        RedirectSmartCards = original.RedirectSmartCards,
                        PromptForCredentials = original.PromptForCredentials,
                        AuthenticationLevel = original.AuthenticationLevel,
                        EnableCredSSPSupport = original.EnableCredSSPSupport,
                        JustAdded = true,
                        Filename = ""
                    };
                    _config!.Profiles.Add(cloned);
                    PopulateTree(selectProfile: cloned);
                    SelectProfile(true);
                    EditMode = true;
                }
                catch (Exception ex)
                {
                    Logger.Error("Failed to clone profile", ex);
                    MessageBox.Show("Failed to clone profile: " + ex.Message);
                }
            });

            var moveToGroupItem = new ToolStripMenuItem("Move to Group");
            moveToGroupItem.DropDownOpening += (s, ev) =>
            {
                moveToGroupItem.DropDownItems.Clear();
                if (_config?.Groups == null) return;

                var groups = _config.Groups.OrderBy(g => g.GroupName).ToList();
                if (groups.Count == 0)
                {
                    var noGroupItem = new ToolStripMenuItem("(No groups available)") { Enabled = false };
                    moveToGroupItem.DropDownItems.Add(noGroupItem);
                    return;
                }

                foreach (var group in groups)
                {
                    var g = group;
                    var item = new ToolStripMenuItem(group.GroupName);
                    if (treeViewProfiles.SelectedNode?.Tag is Profile currentProfile && currentProfile.GroupName == group.GroupName)
                    {
                        item.Enabled = false;
                        item.Checked = true;
                    }
                    item.Click += (sender, e) =>
                    {
                        if (treeViewProfiles.SelectedNode?.Tag is Profile profile)
                        {
                            profile.GroupName = g.GroupName;
                            _config.Save();
                            PopulateTree(selectProfile: profile);
                        }
                    };
                    moveToGroupItem.DropDownItems.Add(item);
                }
            };

            cms.Items.AddRange(new ToolStripItem[] { newGroupItem, newProfileItem, sep, editItem, deleteItem, cloneItem, moveToGroupItem, connectItem });

            // Show/hide menu items based on what node is selected when the menu opens
            cms.Opening += (s, ev) =>
            {
                var sel = treeViewProfiles.SelectedNode;
                bool isProfile = sel?.Tag is Profile;

                if (sel == null)
                {
                    // blank area: only allow creating groups
                    newGroupItem.Visible = true;
                    newProfileItem.Visible = false;
                    editItem.Visible = false;
                    deleteItem.Visible = false;
                    connectItem.Visible = false;
                    cloneItem.Visible = false;
                    moveToGroupItem.Visible = false;
                }
                else if (isProfile)
                {
                    // profile node: Edit, Delete, Clone, Move to Group, Connect
                    newGroupItem.Visible = false;
                    newProfileItem.Visible = false;
                    editItem.Visible = true;
                    deleteItem.Visible = true;
                    connectItem.Visible = true;
                    cloneItem.Visible = true;
                    moveToGroupItem.Visible = true;
                }
                else
                {
                    // group node: New Group, New Profile, Edit, Delete
                    newGroupItem.Visible = true;
                    newProfileItem.Visible = true;
                    editItem.Visible = true;
                    deleteItem.Visible = true;
                    connectItem.Visible = false;
                    cloneItem.Visible = false;
                    moveToGroupItem.Visible = false;
                }
            };

            // Ensure right-click selects the node so the Opening logic sees the correct node
            treeViewProfiles.NodeMouseClick += (s, ev) =>
            {
                if (ev.Button == MouseButtons.Right)
                {
                    treeViewProfiles.SelectedNode = ev.Node;
                }
            };

            treeViewProfiles.ContextMenuStrip = cms;
        }

        public bool EditMode
        {
            get => _editMode;
            set
            {
                buttonSave.Visible = value;
                buttonCancel.Visible = value;
                buttonOptions.Enabled = !value;

                buttonConnect.Enabled = !value;

                textBoxName.Enabled = value;
                textBoxComputer.Enabled = value;
                textBoxUsername.Enabled = value;
                textBoxPassword.Enabled = value;
                textBoxDomain.Enabled = value;

                comboBoxResolution.Enabled = value;
                comboBoxScreenMode.Enabled = value;
                comboBoxColorDepth.Enabled = value;
                comboBoxAudioMode.Enabled = value;
                checkBoxRedirectPrinters.Enabled = value;
                checkBoxRedirectClipboard.Enabled = value;
                checkBoxRedirectDrives.Enabled = value;
                checkBoxRedirectPorts.Enabled = value;
                checkBoxRedirectSmartCards.Enabled = value;
                checkBoxPromptCredentials.Enabled = value;
            }
        }

        private void AddNewProfile()
        {
            var profile = new Profile();
            profile.JustAdded = true;

            string groupName = "";
            if (treeViewProfiles.SelectedNode != null)
            {
                var sel = treeViewProfiles.SelectedNode;
                if (sel.Tag is Profile)
                {
                    groupName = sel.Parent?.Text ?? "";
                }
                else
                {
                    groupName = sel.Text;
                }
            }

            profile.GroupName = groupName;
            profile.EncryptedPassword = "";

            _config!.Profiles.Add(profile);
            PopulateTree(selectProfile: profile);
            SelectProfile(true);
            EditMode = true;
        }

        private void buttonMoreOptions_Click(object sender, EventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = "mstsc.exe",
                Arguments = "/edit " + GetSelectedProfile().Filename,
            };

            try
            {
                var exeProcess = Process.Start(startInfo) ?? throw new InvalidOperationException();
                Task.Run(() =>
                {
                    try { exeProcess.WaitForExit(); } catch { }
                });
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to open mstsc options", ex);
                MessageBox.Show(ex.ToString());
            }
        }


        private void buttonConnect_Click(object sender, EventArgs e)
        {
            var profile = GetSelectedProfile();

            if (String.IsNullOrWhiteSpace(profile.Computer) || String.IsNullOrWhiteSpace(profile.Computer))
            {
                Logger.Warning("Invalid connection: Computer is empty");
                MessageBox.Show("Invalid connection");
                return;
            }

            if (!PingHost(profile.Computer))
            {
                var result = MessageBox.Show(
                    $"主机 '{profile.Computer}' 无法访问（ping 失败），是否仍然连接？",
                    "主机不可达",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            profile.PrepareRdpFile();

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = "mstsc.exe",
                Arguments = profile.Filename,
            };

            try
            {
                Logger.Info($"Connecting to {profile.Computer} with profile {profile.Name}");
                var exeProcess = Process.Start(startInfo) ?? throw new InvalidOperationException();
                Task.Run(() =>
                {
                    try
                    {
                        exeProcess.WaitForExit();
                        if (!_config!.KeepOpening)
                        {
                            try { this.BeginInvoke((Action)(() => this.Close())); } catch { }
                        }
                    }
                    catch { }
                });

            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to connect to {profile.Computer}", ex);
                MessageBox.Show(ex.ToString());
            }
        }

        private bool PingHost(string host)
        {
            try
            {
                using var ping = new System.Net.NetworkInformation.Ping();
                var reply = ping.Send(host, 2000);
                return reply.Status == System.Net.NetworkInformation.IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }

        private void treeViewProfiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // if profile node selected, show its details
            if (e.Node?.Tag is Profile)
            {
                buttonConnect.Visible = true;
                buttonOptions.Visible = true;
                SelectProfile();
            }
            else
            {
                buttonConnect.Visible = false;
                buttonOptions.Visible = false;
            }
        }

        private Profile GetSelectedProfile()
        {
            if (treeViewProfiles.SelectedNode != null && treeViewProfiles.SelectedNode.Tag is Profile p)
            {
                return p;
            }
            throw new InvalidOperationException("No profile selected");
        }

        private void SelectProfile(bool force = false)
        {
            if (treeViewProfiles.SelectedNode == null || !(treeViewProfiles.SelectedNode.Tag is Profile profile))
            {
                buttonConnect.Visible = false;
                buttonOptions.Visible = false;
                return;
            }

            // Avoid click empty area reset value
            if (profile == selectedProfile && !force)
            {
                return;
            }
            buttonConnect.Visible = true;
            buttonOptions.Visible = true;

            selectedProfile = profile;

            EditMode = profile.JustAdded;

            textBoxName.Text = profile.Name;
            textBoxComputer.Text = profile.Computer;
            textBoxUsername.Text = profile.Username;
            textBoxPassword.Text = profile.Password;
            textBoxDomain.Text = profile.Domain;

            // Load advanced settings
            LoadAdvancedSettings(profile);
        }

        private void LoadAdvancedSettings(Profile profile)
        {
            // Resolution
            string resolution = $"{profile.DesktopWidth}x{profile.DesktopHeight}";
            int resIndex = comboBoxResolution.Items.IndexOf(resolution);
            if (resIndex >= 0)
                comboBoxResolution.SelectedIndex = resIndex;
            else
                comboBoxResolution.SelectedIndex = 2; // Default 1280x720

            // Screen mode: 0=Windowed, 1=Full Screen, 2=All Monitors
            if (profile.ScreenMode == 2 && profile.UseMultiMon == 1)
                comboBoxScreenMode.SelectedIndex = 2; // All Monitors
            else if (profile.ScreenMode == 2)
                comboBoxScreenMode.SelectedIndex = 1; // Full Screen
            else
                comboBoxScreenMode.SelectedIndex = 0; // Windowed

            // Color depth: 15, 16, 24, 32
            int[] colorDepths = { 15, 16, 24, 32 };
            int cdIndex = Array.IndexOf(colorDepths, profile.ColorDepth);
            comboBoxColorDepth.SelectedIndex = cdIndex >= 0 ? cdIndex : 2; // Default 24 bit

            // Audio mode: 0=Play locally, 1=Play on remote, 2=Do not play
            comboBoxAudioMode.SelectedIndex = profile.AudioMode >= 0 && profile.AudioMode <= 2 ? profile.AudioMode : 0;

            // Redirect options
            checkBoxRedirectPrinters.Checked = profile.RedirectPrinters == 1;
            checkBoxRedirectClipboard.Checked = profile.RedirectClipboard == 1;
            checkBoxRedirectDrives.Checked = profile.RedirectDrives == 1;
            checkBoxRedirectPorts.Checked = profile.RedirectPorts == 1;
            checkBoxRedirectSmartCards.Checked = profile.RedirectSmartCards == 1;
            checkBoxPromptCredentials.Checked = profile.PromptForCredentials == 1;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditMode = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            EditMode = false;

            var profile = GetSelectedProfile();

            if (profile.JustAdded && _config!.Profiles.Count > 1)
            {
                buttonDelete_Click(this, EventArgs.Empty);
            }
            else
            {
                SelectProfile(true);
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            AddNewProfile();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show(
                "Are you sure to delete this profile?",
                "Confirm",
                MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                if (treeViewProfiles.SelectedNode != null && treeViewProfiles.SelectedNode.Tag is Profile toDelete)
                {
                    toDelete.Delete();
                    _config!.DeleteProfile(toDelete);
                    PopulateTree();

                    if (_config.Profiles.Count == 0)
                    {
                        AddNewProfile();
                        PopulateTree();
                    }
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (treeViewProfiles.SelectedNode == null || !(treeViewProfiles.SelectedNode.Tag is Profile profile))
            {
                MessageBox.Show("No profile selected to save.");
                return;
            }

            try
            {
                profile.JustAdded = false;

                profile.Name = textBoxName.Text;
                profile.Computer = textBoxComputer.Text;
                profile.Username = textBoxUsername.Text;
                profile.Password = textBoxPassword.Text;
                profile.Domain = textBoxDomain.Text;

                SaveAdvancedSettings(profile);

                // Try to prepare RDP file but do not let file I/O failures prevent DB save
                try
                {
                    profile.PrepareRdpFile();
                }
                catch (Exception ex)
                {
                    Logger.Error($"PrepareRdpFile failed for profile: {profile.Name}", ex);
                }

                // Persist to database
                _config!.Save();
                EditMode = false;

                Logger.Info($"Saved profile: {profile.Name}");

                UpdateGroupList();
                PopulateTree(selectProfile: profile);
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to save profile: {profile.Name}", ex);
                MessageBox.Show("Failed to save profile: " + ex.Message);
            }
        }

        private void SaveAdvancedSettings(Profile profile)
        {
            // Resolution
            if (comboBoxResolution.SelectedIndex >= 0)
            {
                string resolution = comboBoxResolution.SelectedItem?.ToString() ?? "";
                var parts = resolution.Split('x');
                if (parts.Length == 2)
                {
                    profile.DesktopWidth = int.Parse(parts[0]);
                    profile.DesktopHeight = int.Parse(parts[1]);
                }
            }

            // Screen mode
            switch (comboBoxScreenMode.SelectedIndex)
            {
                case 0: // Windowed
                    profile.ScreenMode = 1;
                    profile.UseMultiMon = 0;
                    break;
                case 1: // Full Screen
                    profile.ScreenMode = 2;
                    profile.UseMultiMon = 0;
                    break;
                case 2: // All Monitors
                    profile.ScreenMode = 2;
                    profile.UseMultiMon = 1;
                    break;
            }

            // Color depth
            int[] colorDepths = { 15, 16, 24, 32 };
            if (comboBoxColorDepth.SelectedIndex >= 0)
                profile.ColorDepth = colorDepths[comboBoxColorDepth.SelectedIndex];

            // Audio mode
            profile.AudioMode = comboBoxAudioMode.SelectedIndex;

            // Redirect options
            profile.RedirectPrinters = checkBoxRedirectPrinters.Checked ? 1 : 0;
            profile.RedirectClipboard = checkBoxRedirectClipboard.Checked ? 1 : 0;
            profile.RedirectDrives = checkBoxRedirectDrives.Checked ? 1 : 0;
            profile.RedirectPorts = checkBoxRedirectPorts.Checked ? 1 : 0;
            profile.RedirectSmartCards = checkBoxRedirectSmartCards.Checked ? 1 : 0;
            profile.PromptForCredentials = checkBoxPromptCredentials.Checked ? 1 : 0;
        }

        private void checkBoxKeepOpening_CheckedChanged(object sender, EventArgs e)
        {
            _config!.KeepOpening = checkBoxKeepOpening.Checked;
            _config.Save();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _config!.WindowLeft = Left;
                _config.WindowTop = Top;
                _config.WindowWidth = Width;
                _config.WindowHeight = Height;
                _config.Save();
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to save window state", ex);
            }
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog(this);
        }

        private void treeViewProfiles_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null && e.Node.Tag is Profile)
            {
                buttonConnect_Click(sender, EventArgs.Empty);
            }
        }

        private void treeViewProfiles_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitTest = treeViewProfiles.HitTest(e.Location);
                if (hitTest.Node == null)
                {
                    treeViewProfiles.SelectedNode = null;
                }
            }
        }

        private void treeViewProfiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var hitTest = treeViewProfiles.HitTest(e.Location);
            if (hitTest.Node == null)
            {
                BeginInvoke(new Action(() => treeViewProfiles.SelectedNode = null));
            }
        }

        private void treeViewProfiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Item is TreeNode node && node.Tag is Profile)
            {
                DoDragDrop(node, DragDropEffects.Move);
            }
        }

        private void treeViewProfiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(typeof(TreeNode)) == true)
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void treeViewProfiles_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(typeof(TreeNode)) == true && e.Data.GetData(typeof(TreeNode)) is TreeNode draggedNode && draggedNode.Tag is Profile profile)
            {
                var targetPoint = treeViewProfiles.PointToClient(new Point(e.X, e.Y));
                var targetNode = treeViewProfiles.GetNodeAt(targetPoint);

                if (targetNode != null && targetNode != draggedNode)
                {
                    string targetGroupName;
                    if (targetNode.Tag is Profile)
                    {
                        targetGroupName = targetNode.Parent?.Text ?? "";
                    }
                    else
                    {
                        targetGroupName = targetNode.Text;
                    }

                    if (profile.GroupName != targetGroupName)
                    {
                        profile.GroupName = targetGroupName;
                        _config?.Save();
                        PopulateTree(selectProfile: profile);
                    }
                }
            }
        }

        private void SetupTreeViewIcons()
        {
            imageListTreeView.Images.Clear();

            var folderIcon = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(folderIcon))
            {
                g.Clear(Color.Transparent);
                using var brush = new SolidBrush(Color.FromArgb(255, 200, 100));
                g.FillRectangle(brush, 1, 4, 14, 10);
                g.FillRectangle(brush, 1, 2, 6, 4);
                using var pen = new Pen(Color.FromArgb(180, 160, 60));
                g.DrawRectangle(pen, 1, 4, 14, 10);
            }
            imageListTreeView.Images.Add(folderIcon);

            var computerIcon = new Bitmap(16, 16);
            using (var g = Graphics.FromImage(computerIcon))
            {
                g.Clear(Color.Transparent);
                using var brush = new SolidBrush(Color.FromArgb(80, 140, 220));
                g.FillRectangle(brush, 2, 2, 12, 9);
                using var pen = new Pen(Color.FromArgb(60, 100, 180));
                g.DrawRectangle(pen, 2, 2, 12, 9);
                g.FillRectangle(pen.Brush, 5, 11, 6, 1);
                g.FillRectangle(pen.Brush, 4, 12, 8, 2);
            }
            imageListTreeView.Images.Add(computerIcon);
        }

        private void UpdateGroupList()
        {
            // Groups are managed by the tree view; no UI combo to update.
        }

        private void PopulateTree(Profile? selectProfile = null)
        {
            treeViewProfiles.BeginUpdate();
            treeViewProfiles.Nodes.Clear();

            var config = _config ?? throw new InvalidOperationException("Config not loaded");

            var groupSet = new HashSet<Group>();

            if (config.Groups != null)
            {
                foreach (var g in config.Groups.Where(x => !String.IsNullOrWhiteSpace(x.GroupName)))
                {
                    groupSet.Add(g);
                }
            }

            var groupNames = groupSet.OrderBy(x => x);

            foreach (var group in groupNames)
            {
                var groupNode = new TreeNode(group.GroupName)
                {
                    ImageIndex = 0,
                    SelectedImageIndex = 0
                };

                var profilesInGroup = config.Profiles.Where(p => (string.IsNullOrWhiteSpace(p.GroupName) ? "Ungrouped" : p.GroupName) == group.GroupName);
                foreach (var profile in profilesInGroup)
                {
                    var node = new TreeNode(profile.Name)
                    {
                        Tag = profile,
                        ImageIndex = 1,
                        SelectedImageIndex = 1
                    };
                    groupNode.Nodes.Add(node);
                }

                // add group node even if it has no children (shows empty group)
                treeViewProfiles.Nodes.Add(groupNode);
            }

            treeViewProfiles.EndUpdate();
            treeViewProfiles.ExpandAll();

            // restore or select node
            if (selectProfile != null)
            {
                foreach (TreeNode gnode in treeViewProfiles.Nodes)
                {
                    foreach (TreeNode pnode in gnode.Nodes)
                    {
                        if (pnode.Tag == selectProfile)
                        {
                            treeViewProfiles.SelectedNode = pnode;
                            treeViewProfiles.SelectedNode.EnsureVisible();
                            return;
                        }
                    }
                }
            }
        }


        private void buttonExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV files (*.csv)|*.csv|JSON files (*.json)|*.json|All files (*.*)|*.*";
                    sfd.FileName = "profiles.csv";
                    if (sfd.ShowDialog(this) == DialogResult.OK)
                    {
                        var ext = Path.GetExtension(sfd.FileName).ToLowerInvariant();
                        if (ext == ".csv")
                        {
                            ExportCsv(sfd.FileName);
                        }
                        else
                        {
                            ExportJson(sfd.FileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Export failed", ex);
                MessageBox.Show("Export failed: " + ex.Message);
            }
        }

        private void ExportCsv(string path)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Name,Computer,Username,Domain,GroupName,DesktopWidth,DesktopHeight,ScreenMode,ColorDepth,AudioMode,RedirectPrinters,RedirectClipboard,RedirectDrives,RedirectPorts,RedirectSmartCards,PromptForCredentials");

            foreach (Profile p in _config!.Profiles)
            {
                sb.AppendLine($"\"{EscapeCsv(p.Name)}\",\"{EscapeCsv(p.Computer)}\",\"{EscapeCsv(p.Username)}\",\"{EscapeCsv(p.Domain)}\",\"{EscapeCsv(p.GroupName)}\",{p.DesktopWidth},{p.DesktopHeight},{p.ScreenMode},{p.ColorDepth},{p.AudioMode},{p.RedirectPrinters},{p.RedirectClipboard},{p.RedirectDrives},{p.RedirectPorts},{p.RedirectSmartCards},{p.PromptForCredentials}");
            }

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            Logger.Info($"Exported {_config.Profiles.Count} profiles to CSV: {path}");
            MessageBox.Show($"Exported {_config.Profiles.Count} profiles to {path}");
        }

        private void ExportJson(string path)
        {
            var exportData = new
            {
                Groups = _config!.Groups,
                Profiles = _config.Profiles.Cast<Profile>().ToList()
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(exportData, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(path, json);
            Logger.Info($"Exported {exportData.Profiles.Count} profiles and {exportData.Groups.Count} groups to JSON: {path}");
            MessageBox.Show($"Exported {exportData.Profiles.Count} profiles and {exportData.Groups.Count} groups to {path}");
        }

        private static string EscapeCsv(string value)
        {
            if (value == null) return "";
            return value.Replace("\"", "\"\"");
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                    if (ofd.ShowDialog(this) == DialogResult.OK)
                    {
                        var json = System.IO.File.ReadAllText(ofd.FileName);
                        List<Profile>? profileList = null;
                        List<Group>? groupList = null;

                        try
                        {
                            var jo = Newtonsoft.Json.Linq.JObject.Parse(json);
                            if (jo["Profiles"] != null)
                            {
                                profileList = jo["Profiles"]?.ToObject<List<Profile>>();
                            }
                            if (jo["Groups"] != null)
                            {
                                groupList = jo["Groups"]?.ToObject<List<Group>>();
                            }
                        }
                        catch
                        {
                            try
                            {
                                profileList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Profile>>(json);
                            }
                            catch { }
                        }

                        if ((profileList == null || profileList.Count == 0) && (groupList == null || groupList.Count == 0))
                        {
                            ShowError("导入失败", "文件中没有找到有效的数据。", "请检查文件是否为有效的 JSON 格式，且包含 Profile 或 Group 列表。");
                            return;
                        }

                        var importedProfiles = 0;
                        var skippedProfiles = 0;
                        var importedGroups = 0;
                        var skippedGroups = 0;

                        if (groupList != null && groupList.Count > 0)
                        {
                            foreach (var group in groupList)
                            {
                                if (string.IsNullOrWhiteSpace(group.GroupName))
                                {
                                    skippedGroups++;
                                    continue;
                                }
                                var existing = _config!.Groups?.FirstOrDefault(g => g.GroupName.Equals(group.GroupName, StringComparison.OrdinalIgnoreCase));
                                if (existing != null)
                                {
                                    skippedGroups++;
                                }
                                else
                                {
                                    if (_config.Groups == null) _config.Groups = new List<Group>();
                                    _config.Groups.Add(group);
                                    importedGroups++;
                                }
                            }
                            _config!.SaveGroups();
                            _config.Groups = new ProfileRepository(new DatabaseContext()).GetAllGroups();
                        }

                        if (profileList != null && profileList.Count > 0)
                        {
                            var validation = ValidateProfiles(profileList);
                            if (validation.HasIssues)
                            {
                                var result = ShowValidationDialog(validation);
                                if (result != DialogResult.Yes)
                                {
                                    return;
                                }
                            }

                            foreach (var profile in profileList)
                            {
                                profile.Id = 0;
                                if (string.IsNullOrWhiteSpace(profile.Name))
                                {
                                    skippedProfiles++;
                                    continue;
                                }
                                var existing = _config!.Profiles.FirstOrDefault(p => p.Name == profile.Name && p.Computer == profile.Computer);
                                if (existing != null)
                                {
                                    skippedProfiles++;
                                }
                                else
                                {
                                    _config.Profiles.Add(profile);
                                    importedProfiles++;
                                }
                            }
                            _config!.Save();
                        }

                        UpdateGroupList();
                        PopulateTree();

                        var messages = new List<string>();
                        if (importedProfiles > 0) messages.Add($"成功导入 {importedProfiles} 个 Profile");
                        if (importedGroups > 0) messages.Add($"成功导入 {importedGroups} 个 Group");
                        if (skippedProfiles > 0) messages.Add($"跳过 {skippedProfiles} 个重复 Profile");
                        if (skippedGroups > 0) messages.Add($"跳过 {skippedGroups} 个重复 Group");

                        Logger.Info($"Imported {importedProfiles} profiles and {importedGroups} groups from {ofd.FileName}");
                        ShowSuccess("导入完成", string.Join("，", messages) + "。");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Import failed", ex);
                ShowError("导入失败", "导入过程中发生错误。", ex.Message);
            }
        }

        private ProfileValidationResult ValidateProfiles(List<Profile> profiles)
        {
            var result = new ProfileValidationResult();
            var seenNames = new HashSet<string>();
            var seenComputers = new HashSet<string>();

            foreach (var p in profiles)
            {
                if (string.IsNullOrWhiteSpace(p.Name))
                {
                    result.Warnings.Add($"第 {profiles.IndexOf(p) + 1} 项: Profile 名称为空");
                }
                else if (seenNames.Contains(p.Name))
                {
                    result.Warnings.Add($"重复的 Profile 名称: '{p.Name}'");
                }
                else
                {
                    seenNames.Add(p.Name);
                }

                if (!string.IsNullOrWhiteSpace(p.Computer))
                {
                    if (seenComputers.Contains(p.Computer))
                    {
                        result.Warnings.Add($"重复的计算机地址: '{p.Computer}'");
                    }
                    else
                    {
                        seenComputers.Add(p.Computer);
                    }
                }
            }

            return result;
        }

        private DialogResult ShowValidationDialog(ProfileValidationResult validation)
        {
            var detailForm = new Form
            {
                Width = 500,
                Height = 400,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "数据验证警告",
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var label = new Label
            {
                Left = 15,
                Top = 15,
                Width = 450,
                Height = 30,
                Text = $"发现 {validation.Warnings.Count} 个问题，是否继续导入？"
            };

            var listBox = new ListBox
            {
                Left = 15,
                Top = 50,
                Width = 450,
                Height = 240
            };
            foreach (var w in validation.Warnings)
            {
                listBox.Items.Add("⚠ " + w);
            }

            var btnContinue = new Button { Text = "继续导入", Left = 240, Width = 100, Top = 300, DialogResult = DialogResult.Yes };
            var btnCancel = new Button { Text = "取消", Left = 350, Width = 100, Top = 300, DialogResult = DialogResult.No };

            detailForm.Controls.Add(label);
            detailForm.Controls.Add(listBox);
            detailForm.Controls.Add(btnContinue);
            detailForm.Controls.Add(btnCancel);
            detailForm.AcceptButton = btnContinue;
            detailForm.CancelButton = btnCancel;

            return detailForm.ShowDialog(this);
        }

        private void ShowError(string title, string message, string detail = "")
        {
            var form = new Form
            {
                Width = 450,
                Height = detail != "" ? 250 : 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = title,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var icon = new PictureBox
            {
                Left = 15,
                Top = 20,
                Width = 32,
                Height = 32,
                Image = SystemIcons.Error.ToBitmap()
            };

            var msgLabel = new Label
            {
                Left = 60,
                Top = 20,
                Width = 350,
                Height = 30,
                Text = message
            };

            var btnOk = new Button { Text = "确定", Left = 330, Width = 80, Top = form.Height - 80, DialogResult = DialogResult.OK };
            form.Controls.Add(icon);
            form.Controls.Add(msgLabel);
            form.Controls.Add(btnOk);
            form.AcceptButton = btnOk;

            if (detail != "")
            {
                var detailBox = new TextBox
                {
                    Left = 15,
                    Top = 60,
                    Width = 400,
                    Height = form.Height - 120,
                    Multiline = true,
                    ReadOnly = true,
                    ScrollBars = ScrollBars.Vertical,
                    Text = detail,
                    BackColor = SystemColors.Control
                };
                form.Controls.Add(detailBox);
            }

            form.ShowDialog(this);
        }

        private void ShowSuccess(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Removed old ListBox draw helper after switching to TreeView

    }
}

internal static class Prompt
{
    public static string? ShowDialog(string text, string caption, string defaultValue = "")
    {
        var prompt = new Form()
        {
            Width = 400,
            Height = 150,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            Text = caption,
            StartPosition = FormStartPosition.CenterParent
        };
        var label = new Label() { Left = 10, Top = 10, Text = text, Width = 360 };
        var textBox = new TextBox() { Left = 10, Top = 35, Width = 360, Text = defaultValue };
        var confirmation = new Button() { Text = "Ok", Left = 200, Width = 80, Top = 70, DialogResult = DialogResult.OK };
        var cancel = new Button() { Text = "Cancel", Left = 290, Width = 80, Top = 70, DialogResult = DialogResult.Cancel };
        confirmation.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        cancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        prompt.Controls.Add(label);
        prompt.Controls.Add(textBox);
        prompt.Controls.Add(confirmation);
        prompt.Controls.Add(cancel);
        prompt.AcceptButton = confirmation;
        prompt.CancelButton = cancel;

        return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
    }
}
