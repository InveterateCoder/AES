using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace AES
{
    public partial class WithKeyfile : Form
    {
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        internal bool Encrypt;
        private bool switcher;
        public WithKeyfile()
        {
            InitializeComponent();
            timer.Interval = 350;
            timer.Tick += TimerTick;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog()==DialogResult.OK)
                textBox1.Text = openFileDialog1.FileName;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            if (saveFileDialog1.ShowDialog()==DialogResult.OK)
                textBox2.Text = saveFileDialog1.FileName;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                textBox3.Text = openFileDialog1.FileName;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.Text = textBox1.Text;
                textBox2.Enabled = button4.Enabled = false;
            }
            else
                textBox2.Enabled = button4.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.Text = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Encrypt)
                OpenDialog.WithEncrypt = null;
            else
                OpenDialog.WithDecrypt = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.TextLength == 0 || textBox2.TextLength == 0 || textBox3.TextLength == 0)
                {
                    label6.Text = "None of the paths can be empty.";
                    return;
                }
                KeyData.KeyfileWrite = null;
                KeyData.FileFrom = textBox1.Text;
                KeyData.FileTo = textBox2.Text;
                KeyData.KeyfileRead = textBox3.Text;
                BinaryReader br = new BinaryReader(File.OpenRead(KeyData.KeyfileRead));
                long keyfilelen = br.BaseStream.Length;
                if (keyfilelen < 35 || keyfilelen > 51)
                {
                    br.Close();
                    throw new Exception("The keyfile is either corrupted or not a keyfile.");
                }
                byte keylen = br.ReadByte();
                switch (keylen)
                {
                    case 16:
                        if (keyfilelen != 35)
                        {
                            br.Close();
                            throw new Exception("The keyfile is either corrupted or not a keyfile.");
                        }
                        break;
                    case 24:
                        if (keyfilelen != 43)
                        {
                            br.Close();
                            throw new Exception("The keyfile is either corrupted or not a keyfile.");
                        }
                        break;
                    case 32:
                        if (keyfilelen != 51)
                        {
                            br.Close();
                            throw new Exception("The keyfile is either corrupted or not a keyfile.");
                        }
                        break;
                    default:
                        br.Close();
                        throw new Exception("The keyfile is either corrupted or not a keyfile.");
                }
                byte[] key = new byte[keylen];
                if (br.Read(key, 0, keylen) != keylen)
                {
                    br.Close();
                    throw new Exception("Failed to read the key from the keyfile.");
                }
                byte[] iv = new byte[16];
                if (br.Read(iv, 0, 16) != 16)
                {
                    br.Close();
                    throw new Exception("Failed to read the IV from the keyfile.");
                }
                byte CM = br.ReadByte();
                byte PM = br.ReadByte();
                br.Close();
                KeyData.Key = key;
                KeyData.IV = iv;
                KeyData.CM = (System.Security.Cryptography.CipherMode)CM;
                KeyData.PM = (System.Security.Cryptography.PaddingMode)PM;
                switcher = true;
                ((Form1)Parent).menuStrip1.Enabled = false;
                Thread thread = new Thread(KeyData.ProcessData);
                if (Encrypt)
                {
                    KeyData.direction = Direction.Encrypt;
                    OpenDialog.WithEncrypt = null;
                }
                else
                {
                    KeyData.direction = Direction.Decrypt;
                    OpenDialog.WithDecrypt = null;
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
                if (switcher)
                {
                    switcher = false;
                    OpenDialog.pb.Visible = true;
                    OpenDialog.bt.Visible = true;
                }
            }
        }
    }
}
