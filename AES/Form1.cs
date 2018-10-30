using System;
using System.Windows.Forms;

namespace AES
{
    public partial class Form1 : Form
    {
        private Timer timer = new Timer();
        public Form1()
        {
            InitializeComponent();
            OpenDialog.SetParent(this);
            OpenDialog.pb = progressBar1;
            OpenDialog.bt = button1;
            Controls.SetChildIndex(pictureBox1, -1);
            timer.Interval = 200;
            timer.Tick += HandleTick;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = OpenDialog.About;
            if (about != null)
                about.Show();
        }

        private void newKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDialog.ManEncrypt.Show();
        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenDialog.ManDecrypt.Show();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                pictureBox1.Visible ^= true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Do you really want to cancel the process?",
                "Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
                KeyData.Cancel = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (KeyData.Exit)
                return;
            e.Cancel = true;
            DialogResult dr = MessageBox.Show("You're about to cancel the current processing and exit. Do you consent?",
                "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                KeyData.Cancel = true;
                timer.Start();
            }
        }

        private void HandleTick(object sender, EventArgs e)
        {
            if(KeyData.Exit)
            {
                timer.Stop();
                Close();
            }
        }

        private void makeNewKeyfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakeKey mk = OpenDialog.MakeKey;
            mk.Parent = this;
            mk.Show();
        }

        private void usingKeyFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDialog.WithEncrypt.Show();
        }

        private void messageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDialog.WithDecrypt.Show();
        }
    }
}
