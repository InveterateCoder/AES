using System;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace AES
{
    public partial class Final : Form
    {
        internal bool Encrypt;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private RandomNumberGenerator rn;
        internal int keylength;
        private bool switcher;
        public Final()
        {
            InitializeComponent();
            rn = RandomNumberGenerator.Create();
            timer.Interval = 350;
            timer.Tick += TimerTick;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Encrypt)
                OpenDialog.ManEncrypt.Show();
            else
                OpenDialog.ManDecrypt.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label3.Text = (keylength - textBox1.TextLength).ToString();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label4.Text = (16 - textBox2.TextLength).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] bt = new byte[keylength];
            do
            {
                rn.GetBytes(bt);
                textBox1.Text = Encoding.Default.GetString(bt);
            } while (textBox1.TextLength != keylength);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] bt = new byte[16];
            do
            {
                rn.GetBytes(bt, 0, 16);
                textBox2.Text = Encoding.Default.GetString(bt);
            } while (textBox2.TextLength != 16);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Enabled ^= true;
            button5.Enabled ^= true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog()==DialogResult.OK)
            {
                textBox3.Text = saveFileDialog1.FileName;
                saveFileDialog1.FileName = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (label3.Text != "0" || label4.Text != "0")
                    throw new Exception("The Key and Initialization Vector fields must be filled in.");
                KeyData.Key = Encoding.Default.GetBytes(textBox1.Text.ToCharArray());
                KeyData.IV = Encoding.Default.GetBytes(textBox2.Text.ToCharArray());
                switcher = true;
                ((Form1)Parent).menuStrip1.Enabled = false;
                Thread thread = new Thread(KeyData.ProcessData);
                if (Encrypt)
                {
                    KeyData.direction = Direction.Encrypt;
                    if (checkBox1.Checked)
                        KeyData.KeyfileWrite = textBox3.Text;
                    OpenDialog.FinEncrypt = null;
                    OpenDialog.ManEncrypt = null;
                }
                else
                {
                    KeyData.direction = Direction.Decrypt;
                    OpenDialog.FinDecrypt = null;
                    OpenDialog.ManDecrypt = null;
                }
                thread.Start();
                timer.Start();
            }
            catch(Exception exc)
            {
                label6.Text = exc.Message;
            }
        }
        private void TimerTick(object sender, EventArgs e)
        {
            if (KeyData.val == 100)
            {
                timer.Stop();
                if (!switcher)
                {
                    OpenDialog.pb.Visible = false;
                    OpenDialog.bt.Visible = false;
                }
                ((Form1)Parent).menuStrip1.Enabled = true;
                KeyData.Exit = true;
            }
            else
            {
                OpenDialog.pb.Value = KeyData.val;
                if(switcher)
                {
                    switcher = false;
                    OpenDialog.pb.Visible = true;
                    OpenDialog.bt.Visible = true;
                }
            }
        }
    }
}
