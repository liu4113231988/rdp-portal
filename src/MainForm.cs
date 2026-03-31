using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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

        [SupportedOSPlatform("windows")]
        private async void MainForm_Load(object sender, EventArgs e)
        {
            _config = await Config.GetConfigAsync();
            // Populate group combo box with existing groups
            UpdateGroupList();

            if (_config.Profiles.Count == 0)
            {
                AddNewProfile();
            }

            PopulateTree();

            checkBoxKeepOpening.Checked = _config.KeepOpening;

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
                                _config.Save();
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

            cms.Items.AddRange(new ToolStripItem[] { newGroupItem, newProfileItem, sep, editItem, deleteItem, connectItem });

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
                }
                else if (isProfile)
                {
                    // profile node: Edit, Delete, Connect
                    newGroupItem.Visible = false;
                    newProfileItem.Visible = false;
                    editItem.Visible = true;
                    deleteItem.Visible = true;
                    connectItem.Visible = true;
                }
                else
                {
                    // group node: New Group, New Profile, Edit, Delete
                    newGroupItem.Visible = true;
                    newProfileItem.Visible = true;
                    editItem.Visible = true;
                    deleteItem.Visible = true;
                    connectItem.Visible = false;
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
            //_config.Save();
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
                var groupNode = new TreeNode(group.GroupName);

                var profilesInGroup = config.Profiles.Where(p => (string.IsNullOrWhiteSpace(p.GroupName) ? "Ungrouped" : p.GroupName) == group.GroupName);
                foreach (var profile in profilesInGroup)
                {
                    var node = new TreeNode(profile.Name);
                    node.Tag = profile;
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
                    sfd.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                    sfd.FileName = "profiles.json";
                    if (sfd.ShowDialog(this) == DialogResult.OK)
                    {
                        var list = new List<Profile>();
                        foreach (Profile p in _config!.Profiles)
                        {
                            list.Add(p);
                        }

                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
                        System.IO.File.WriteAllText(sfd.FileName, json);
                        Logger.Info($"Exported {list.Count} profiles to {sfd.FileName}");
                        MessageBox.Show("Exported profiles to " + sfd.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Export failed", ex);
                MessageBox.Show("Export failed: " + ex.Message);
            }
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
                        List<Profile>? list = null;
                        try
                        {
                            list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Profile>>(json);
                        }
                        catch
                        {
                            try
                            {
                                var jo = Newtonsoft.Json.Linq.JObject.Parse(json);
                                if (jo["Profiles"] != null)
                                {
                                    list = jo["Profiles"]?.ToObject<List<Profile>>();
                                }
                            }
                            catch { }
                        }

                        if (list == null)
                        {
                            MessageBox.Show("No profiles found in file.");
                            return;
                        }

                        foreach (var profile in list)
                        {
                            profile.Id = 0;
                        }
                        _config!.ImportProfiles(list);

                        UpdateGroupList();
                        PopulateTree();

                        Logger.Info($"Imported {list.Count} profiles from {ofd.FileName}");
                        MessageBox.Show("Imported " + list.Count + " profiles.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Import failed", ex);
                MessageBox.Show("Import failed: " + ex.Message);
            }
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
