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

        private Config _config;
        private bool _editMode = false;
        private Profile selectedProfile = null;
        private TreeNode lastSelectedNode = null;

        public MainForm()
        {
            InitializeComponent();
            _config = Config.GetConfig();
        }

        [SupportedOSPlatform("windows")]
        private void MainForm_Load(object sender, EventArgs e)
        {
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

            // create context menu for tree
            var cms = new ContextMenuStrip();
            cms.Items.Add("New Group", null, (s, ev) =>
            {
                var name = Prompt.ShowDialog("Group name:", "New Group");
                if (!String.IsNullOrWhiteSpace(name))
                {
                    if (_config.Groups == null) _config.Groups = new List<string>();
                    if (!_config.Groups.Contains(name))
                    {
                        _config.Groups.Add(name);
                        _config.Save();
                        PopulateTree();
                        MessageBox.Show("Group '" + name + "' created.");
                    }
                    else
                    {
                        MessageBox.Show("Group '" + name + "' already exists.");
                    }
                }
            });
            cms.Items.Add("New Profile", null, (s, ev) =>
            {
                var groupName = "";
                if (treeViewProfiles.SelectedNode != null && !(treeViewProfiles.SelectedNode.Tag is Profile))
                {
                    groupName = treeViewProfiles.SelectedNode.Text;
                }
                var p = new Profile() { JustAdded = true, Group = groupName };
                _config.Profiles.Add(p);
                _config.Save();
                PopulateTree(selectProfile: p);
            });
            cms.Items.Add(new ToolStripSeparator());
            cms.Items.Add("Edit", null, (s, ev) =>
            {
                if (treeViewProfiles.SelectedNode == null) return;
                if (treeViewProfiles.SelectedNode.Tag is Profile)
                {
                    // select and enter edit mode
                    SelectProfile(true);
                    EditMode = true;
                }
                else
                {
                    // group rename
                    var oldName = treeViewProfiles.SelectedNode.Text;
                    var newName = Prompt.ShowDialog("Rename group:", "Rename Group", oldName);
                    if (!String.IsNullOrWhiteSpace(newName) && newName != oldName)
                    {
                        // rename in profiles
                        foreach (var p in _config.Profiles.Where(x => (x.Group ?? "") == oldName))
                        {
                            p.Group = newName;
                        }

                        // rename in persisted groups list
                        if (_config.Groups == null) _config.Groups = new List<string>();
                        if (_config.Groups.Contains(oldName))
                        {
                            if (!_config.Groups.Contains(newName))
                            {
                                var idx = _config.Groups.IndexOf(oldName);
                                _config.Groups[idx] = newName;
                            }
                            else
                            {
                                // remove oldName if newName already exists
                                _config.Groups.Remove(oldName);
                            }
                        }
                        else
                        {
                            if (!_config.Groups.Contains(newName)) _config.Groups.Add(newName);
                        }

                        _config.Save();
                        PopulateTree();
                    }
                }
            });
            cms.Items.Add("Delete", null, (s, ev) =>
            {
                if (treeViewProfiles.SelectedNode == null) return;
                if (treeViewProfiles.SelectedNode.Tag is Profile p)
                {
                    var ok = MessageBox.Show("Delete profile '" + p.Name + "'?", "Delete", MessageBoxButtons.YesNo);
                    if (ok == DialogResult.Yes)
                    {
                        p.Delete();
                        _config.Profiles.Remove(p);
                        _config.Save();
                        PopulateTree();
                    }
                }
                else
                {
                    var grp = treeViewProfiles.SelectedNode.Text;
                    var ok = MessageBox.Show("Delete group '" + grp + "' and all its profiles?", "Delete Group", MessageBoxButtons.YesNo);
                    if (ok == DialogResult.Yes)
                    {
                        var items = _config.Profiles.Where(x => (x.Group ?? "") == grp).ToList();
                        foreach (var ip in items)
                        {
                            ip.Delete();
                            _config.Profiles.Remove(ip);
                        }

                        if (_config.Groups != null && _config.Groups.Contains(grp))
                        {
                            _config.Groups.Remove(grp);
                        }

                        _config.Save();
                        PopulateTree();
                    }
                }
            });
            cms.Items.Add("Connect", null, (s, ev) =>
            {
                if (treeViewProfiles.SelectedNode != null && treeViewProfiles.SelectedNode.Tag is Profile)
                {
                    buttonConnect_Click(s, EventArgs.Empty);
                }
            });

            treeViewProfiles.ContextMenuStrip = cms;
        }

        public bool EditMode
        {
            get => _editMode;
            set
            {
                buttonEdit.Visible = !value;
                buttonSave.Visible = value;
                buttonCancel.Visible = value;
                buttonOptions.Enabled = !value;

                buttonConnect.Enabled = !value;

                textBoxName.Enabled = value;
                textBoxComputer.Enabled = value;
                textBoxUsername.Enabled = value;
                textBoxPassword.Enabled = value;
                textBoxDomain.Enabled = value;
            }
        }

        private void AddNewProfile()
        {
            var profile = new Profile();
            profile.JustAdded = true;
            profile.Group = "";
            profile.EncryptedPassword = "";
            _config.Profiles.Add(profile);
            _config.Save();
            PopulateTree(selectProfile: profile);
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
                exeProcess.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void buttonConnect_Click(object sender, EventArgs e)
        {
            var profile = GetSelectedProfile();

            if (String.IsNullOrWhiteSpace(profile.Computer) || String.IsNullOrWhiteSpace(profile.Computer))
            {
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
                var exeProcess = Process.Start(startInfo) ?? throw new InvalidOperationException();
                exeProcess.WaitForExit();

                if (!_config.KeepOpening)
                {
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void treeViewProfiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // if profile node selected, show its details
            if (e.Node?.Tag is Profile)
            {
                SelectProfile();
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
                return;
            }

            // Avoid click empty area reset value
            if (profile == selectedProfile && !force)
            {
                return;
            }

            selectedProfile = profile;

            EditMode = profile.JustAdded;

            textBoxName.Text = profile.Name;
            textBoxComputer.Text = profile.Computer;
            textBoxUsername.Text = profile.Username;
            textBoxPassword.Text = profile.Password;
            textBoxDomain.Text = profile.Domain;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditMode = true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            EditMode = false;

            var profile = GetSelectedProfile();

            if (profile.JustAdded && _config.Profiles.Count > 1)
            {
                buttonDelete_Click(null, null);
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
            // show confirm dialog
            var confirmResult = MessageBox.Show(
                "Are you sure to delete this profile?",
                "Confirm",
                MessageBoxButtons.YesNo);

            // if confirm delete
            if (confirmResult == DialogResult.Yes)
            {
                if (treeViewProfiles.SelectedNode != null && treeViewProfiles.SelectedNode.Tag is Profile toDelete)
                {
                    toDelete.Delete();
                    _config.Profiles.Remove(toDelete);
                    _config.Save();
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

            profile.JustAdded = false;

            profile.Name = textBoxName.Text;
            profile.Computer = textBoxComputer.Text;
            profile.Username = textBoxUsername.Text;
            profile.Password = textBoxPassword.Text;
            profile.Domain = textBoxDomain.Text;

            profile.PrepareRdpFile();

            _config.Save();
            EditMode = false;

            // refresh group list in case user added a new group
            UpdateGroupList();

            // refresh tree and keep selection
            PopulateTree(selectProfile: profile);
        }

        private void checkBoxKeepOpening_CheckedChanged(object sender, EventArgs e)
        {
            _config.KeepOpening = checkBoxKeepOpening.Checked;
            _config.Save();
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

        private void UpdateGroupList()
        {
            // Groups are managed by the tree view; no UI combo to update.
        }

        private void PopulateTree(Profile selectProfile = null)
        {
            treeViewProfiles.BeginUpdate();
            treeViewProfiles.Nodes.Clear();

            // Build group set from profiles and persisted groups so empty groups show up
            var groupSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var p in _config.Profiles)
            {
                var gname = String.IsNullOrWhiteSpace(p.Group) ? "Ungrouped" : p.Group;
                groupSet.Add(gname);
            }

            if (_config.Groups != null)
            {
                foreach (var g in _config.Groups.Where(x => !String.IsNullOrWhiteSpace(x)))
                {
                    groupSet.Add(g);
                }
            }

            var groupNames = groupSet.OrderBy(x => x);

            foreach (var gname in groupNames)
            {
                var groupNode = new TreeNode(gname);

                var profilesInGroup = _config.Profiles.Where(p => (String.IsNullOrWhiteSpace(p.Group) ? "Ungrouped" : p.Group) == gname);
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
                        foreach (Profile p in _config.Profiles)
                        {
                            list.Add(p);
                        }

                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);
                        System.IO.File.WriteAllText(sfd.FileName, json);
                        MessageBox.Show("Exported profiles to " + sfd.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
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
                        var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Profile>>(json);
                        if (list == null)
                        {
                            MessageBox.Show("No profiles found in file.");
                            return;
                        }

                        // Replace current profiles with imported ones
                        _config.Profiles = new BindingList<Profile>(list);
                        _config.Save();

                        UpdateGroupList();
                        PopulateTree();

                        MessageBox.Show("Imported " + list.Count + " profiles.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Import failed: " + ex.Message);
            }
        }

        // Removed old ListBox draw helper after switching to TreeView

    }
}

internal static class Prompt
{
    public static string ShowDialog(string text, string caption, string defaultValue = "")
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
