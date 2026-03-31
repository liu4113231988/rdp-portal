using System;
using System.Drawing;
using System.Windows.Forms;

namespace RDP_Portal {
    public partial class About : Form {
        public About() {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e) {
            if (Owner == null) return;

            Location = new Point(Owner.Location.X + Owner.Width / 2 - ClientSize.Width / 2,
                Owner.Location.Y + Owner.Height / 2 - ClientSize.Height / 2);

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion ?? "1.0.0.0";

            labelName.Text = "RDP Portal v" + version;
        }

        private void buttonOK_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
        }
    }
}
