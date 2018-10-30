using System;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace AES
{
    public partial class Manually : Form
    {
        internal bool Encrypt;
        public Manually()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(Enum.GetNames(typeof(PaddingMode)));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
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

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                textBox1.Text = openFileDialog1.FileName;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                textBox2.Text = saveFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(Encrypt)
            {
                OpenDialog.FinEncrypt = null;
                OpenDialog.ManEncrypt = null;
            }
            else
            {
                OpenDialog.FinDecrypt = null;
                OpenDialog.ManDecrypt = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
                label6.Text = "Must fill in the paths.";
            else
            {
                try
                {
                    KeyData.FileFrom = textBox1.Text;
                    KeyData.FileTo = textBox2.Text;
                    KeyData.PM = (PaddingMode)comboBox1.SelectedIndex + 1;
                    KeyData.CM = comboBox2.SelectedIndex == 0 ? CipherMode.CBC : CipherMode.ECB;
                    int len;
                    if (radioButton3.Checked)
                        len = 32;
                    else if (radioButton2.Checked)
                        len = 24;
                    else len = 16;
                    if(Encrypt)
                    {
                        OpenDialog.SetEKeyLength(len);
                        OpenDialog.FinEncrypt.Show();
                    }
                    else
                    {
                        OpenDialog.SetDKeyLength(len);
                        OpenDialog.FinDecrypt.Show();
                    }
                }
                catch(Exception exep)
                {
                    label6.Text = exep.Message;
                }
            }
        }
    }
}
