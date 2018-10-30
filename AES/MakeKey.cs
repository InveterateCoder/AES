using System;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace AES
{
    public partial class MakeKey : Form
    {
        private RandomNumberGenerator rn;
        public MakeKey()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(Enum.GetNames(typeof(PaddingMode)));
            comboBox1.SelectedIndex = 1;
            comboBox2.SelectedIndex = 0;
            rn = RandomNumberGenerator.Create();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.MaxLength = 16;
            if (textBox1.TextLength > 16)
                textBox1.Text = textBox1.Text.Substring(0, 16);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.MaxLength = 24;
            if (textBox1.TextLength > 24)
                textBox1.Text = textBox1.Text.Substring(0, 24);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.MaxLength = 32;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int len = textBox1.MaxLength;
            byte[] bt = new byte[len];
            do
            {
                rn.GetBytes(bt);
                textBox1.Text = Encoding.Default.GetString(bt);
            } while (textBox1.TextLength != len);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] bt = new byte[16];
            do
            {
                rn.GetBytes(bt);
                textBox2.Text = Encoding.Default.GetString(bt);
            } while (textBox2.TextLength != 16);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenDialog.MakeKey = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.TextLength != textBox1.MaxLength || textBox2.TextLength != 16)
                    throw new Exception("The Key and IV fields must be properly filled in.");
                saveFileDialog1.FileName = "";
                if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                    return;
                FileInfo fi = new FileInfo(saveFileDialog1.FileName);
                BinaryWriter bw = new BinaryWriter(fi.OpenWrite());
                bw.BaseStream.SetLength(0);
                bw.Write((byte)textBox1.TextLength);
                bw.Write(Encoding.Default.GetBytes(textBox1.Text.ToCharArray()));
                bw.Write(Encoding.Default.GetBytes(textBox2.Text.ToCharArray()));
                bw.Write((byte)(comboBox2.SelectedIndex == 0 ? CipherMode.CBC : CipherMode.ECB));
                bw.Write((byte)(comboBox1.SelectedIndex + 1));
                bw.Close();
                fi.Attributes = FileAttributes.ReadOnly;
                OpenDialog.MakeKey = null;
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
