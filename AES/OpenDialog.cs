using System.Windows.Forms;

namespace AES
{
    internal struct ManualFormMem
    {
        public bool Acquired;
        public string label1;
        public string label6;
        public string textBox1;
        public string textBox2;
        public int comboBox1;
        public int comboBox2;
        public byte keyLength;
        public bool checkBox1;
    }
    internal struct FinalFormMem
    {
        public bool Acquired;
        public string Message;
        public string Key;
        public string IV;
        public int KeyLen;
        public string Keyfile;
        public bool MakeKeyfile;
    }
    internal struct WithKeyFormMem
    {
        public bool Acquired;
        public string label1;
        public string label6;
        public string textBox1;
        public string textBox2;
        public string textBox3;
        public bool checkBox1;
    }
    internal static class OpenDialog
    {
        private static ManualFormMem EncryptMan;
        private static ManualFormMem DecryptMan;
        private static FinalFormMem EncryptFin;
        private static FinalFormMem DecryptFin;
        private static WithKeyFormMem EncryptWith;
        private static WithKeyFormMem DecryptWith;
        internal static Button bt;
        internal static ProgressBar pb;
        private static bool[] isOn = new bool[5];
        private static Form[] forms = new Form[5];
        internal static void SetEKeyLength(int var)
        {
            EncryptFin.KeyLen = var;
        }
        internal static void SetDKeyLength(int var)
        {
            DecryptFin.KeyLen = var;
        }
        static OpenDialog()
        {
            EncryptMan.label1 = "File to be Encrypted:";
            DecryptMan.label1 = "File to be Decrypted:";
            EncryptMan.label6 = "If you are not familiar with the algorithm, leave the selections as they are.";
            DecryptMan.label6 = "For proper decryption your selections must be " +
                    "identical to the selections specified in time of encryption.";
            EncryptMan.keyLength = DecryptMan.keyLength = 32;
            EncryptMan.comboBox1 = DecryptMan.comboBox1 = 1;
            EncryptMan.comboBox2 = DecryptMan.comboBox2 = 0;
            EncryptMan.textBox1 = DecryptMan.textBox1 = "";
            EncryptMan.textBox2 = DecryptMan.textBox2 = "";
            EncryptMan.checkBox1 = DecryptMan.checkBox1 = false;
            EncryptFin.Key = EncryptFin.IV = DecryptFin.IV = DecryptFin.Key = EncryptFin.Keyfile = "";
            EncryptFin.Message = "Making a keyfile simplifies the process of decryption. IV(initialization vector) must be also filled in.";
            DecryptFin.Message = "The proper Key and Initialization Vector are required to proceed to the decryption.";
            EncryptWith.textBox1 = EncryptWith.textBox2 = EncryptWith.textBox3 = "";
            DecryptWith.textBox1 = DecryptWith.textBox2 = DecryptWith.textBox3 = "";
            EncryptWith.label1 = "File to be Encrypted:";
            DecryptWith.label1 = "File to be Decrypted:";
            EncryptWith.label6 = "Be careful not to delete the keyfile that you'll need in time of decryption.";
            DecryptWith.label6 = "Make sure that you use the right keyfile for the decryption.";
            EncryptWith.checkBox1 = DecryptWith.checkBox1 = false;
            forms[0] = new About
            {
                TopLevel = false
            };
            forms[1] = new Manually
            {
                TopLevel = false
            };
            forms[2] = new Final
            {
                TopLevel = false
            };
            forms[3] = new MakeKey()
            {
                TopLevel = false
            };
            forms[4] = new WithKeyfile()
            {
                TopLevel = false
            };
        }
        internal static void SetParent(Form parent)
        {
            foreach (var f in forms)
                f.Parent = parent;
        }
        private static void HideAll(int ex)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i == ex)
                    continue;
                if(isOn[i])
                {
                    forms[i].Hide();
                    isOn[i] = false;
                }
            }
        }
        private static void HidePrev()
        {
            for (int i = 0; i < 5; i++)
            {
                if (isOn[i])
                {
                    forms[i].Hide();
                    break;
                }
            }
        }
        private static void ShowPrev()
        {
            for (int i = 0; i < 5; i++)
            {
                if (isOn[i])
                {
                    forms[i].Show();
                    break;
                }
            }
        }
        internal static About About
        {
            get
            {
                if (isOn[0])
                    return null;
                else
                {
                    HidePrev();
                    isOn[0] = true;
                    return (About)forms[0];
                }
            }
            set
            {
                if (value != null)
                    return;
                forms[0].Hide();
                isOn[0] = false;
                ShowPrev();
            }
        }
        internal static Manually ManEncrypt
        {
            get
            {
                if (DecryptMan.Acquired)
                {
                    Manually m = (Manually)forms[1];
                    DecryptMan.textBox1 = m.textBox1.Text;
                    DecryptMan.textBox2 = m.textBox2.Text;
                    DecryptMan.checkBox1 = m.checkBox1.Checked;
                    DecryptMan.comboBox1 = m.comboBox1.SelectedIndex;
                    DecryptMan.comboBox2 = m.comboBox2.SelectedIndex;
                    if (m.radioButton3.Checked)
                        DecryptMan.keyLength = 32;
                    else if (m.radioButton2.Checked)
                        DecryptMan.keyLength = 24;
                    else DecryptMan.keyLength = 16;
                    DecryptMan.Acquired = false;
                }
                else if(EncryptMan.Acquired)
                {
                    HideAll(1);
                    isOn[1] = true;
                    return (Manually)forms[1];
                }
                EncryptMan.Acquired = true;
                return Manually;
            }
            set
            {
                Manually = null;
                EncryptMan.Acquired = false;
                EncryptMan.keyLength = 32;
                EncryptMan.comboBox1 = 1;
                EncryptMan.comboBox2 = 0;
                EncryptMan.textBox1 = "";
                EncryptMan.textBox2 = "";
                EncryptMan.checkBox1 = false;
            }
        }
        internal static Manually ManDecrypt
        {
            get
            {
                if (EncryptMan.Acquired)
                {
                    Manually m = (Manually)forms[1];
                    EncryptMan.textBox1 = m.textBox1.Text;
                    EncryptMan.textBox2 = m.textBox2.Text;
                    EncryptMan.checkBox1 = m.checkBox1.Checked;
                    EncryptMan.comboBox1 = m.comboBox1.SelectedIndex;
                    EncryptMan.comboBox2 = m.comboBox2.SelectedIndex;
                    if (m.radioButton3.Checked)
                        EncryptMan.keyLength = 32;
                    else if (m.radioButton2.Checked)
                        EncryptMan.keyLength = 24;
                    else EncryptMan.keyLength = 16;
                    EncryptMan.Acquired = false;
                }
                else if(DecryptFin.Acquired)
                {
                    HideAll(1);
                    isOn[1] = true;
                    return (Manually)forms[1];
                }
                DecryptMan.Acquired = true;
                return Manually;
            }
            set
            {
                Manually = null;
                DecryptMan.Acquired = false;
                DecryptMan.keyLength = 32;
                DecryptMan.comboBox1 = 1;
                DecryptMan.comboBox2 = 0;
                DecryptMan.textBox1 = "";
                DecryptMan.textBox2 = "";
                DecryptMan.checkBox1 = false;
            }
        }
        private static Manually Manually
        {
            get
            {
                if (EncryptMan.Acquired)
                {
                    Manually em = (Manually)forms[1];
                    em.label1.Text = EncryptMan.label1;
                    em.label6.Text = EncryptMan.label6;
                    em.textBox1.Text = EncryptMan.textBox1;
                    em.textBox2.Text = EncryptMan.textBox2;
                    em.checkBox1.Checked = EncryptMan.checkBox1;
                    em.comboBox1.SelectedIndex = EncryptMan.comboBox1;
                    em.comboBox2.SelectedIndex = EncryptMan.comboBox2;
                    switch (EncryptMan.keyLength)
                    {
                        case 32:
                            em.radioButton3.Checked = true;
                            break;
                        case 24:
                            em.radioButton2.Checked = true;
                            break;
                        case 16:
                            em.radioButton1.Checked = true;
                            break;
                    }
                    em.Encrypt = true;
                    HideAll(1);
                    isOn[1] = true;
                    return em;
                }
                else
                {
                    Manually dm = (Manually)forms[1];
                    dm.label1.Text = DecryptMan.label1;
                    dm.label6.Text = DecryptMan.label6;
                    dm.textBox1.Text = DecryptMan.textBox1;
                    dm.textBox2.Text = DecryptMan.textBox2;
                    dm.checkBox1.Checked = DecryptMan.checkBox1;
                    dm.comboBox1.SelectedIndex = DecryptMan.comboBox1;
                    dm.comboBox2.SelectedIndex = DecryptMan.comboBox2;
                    switch (DecryptMan.keyLength)
                    {
                        case 32:
                            dm.radioButton3.Checked = true;
                            break;
                        case 24:
                            dm.radioButton2.Checked = true;
                            break;
                        case 16:
                            dm.radioButton1.Checked = true;
                            break;
                    }
                    dm.Encrypt = false;
                    HideAll(1);
                    isOn[1] = true;
                    return dm;
                }
            }
            set
            {
                forms[1].Hide();
                isOn[1] = false;
            }
        }
        internal static Final FinEncrypt
        {
            get
            {
                if (DecryptFin.Acquired)
                {
                    Final fe = (Final)forms[2];
                    DecryptFin.Key = fe.textBox1.Text;
                    DecryptFin.IV = fe.textBox2.Text;
                    DecryptFin.Acquired = false;
                }
                else if(EncryptFin.Acquired)
                {
                    Final fe = (Final)forms[2];
                    fe.keylength = EncryptFin.KeyLen;
                    fe.textBox1.MaxLength = EncryptFin.KeyLen;
                    if (fe.textBox1.TextLength > EncryptFin.KeyLen)
                    {
                        EncryptFin.Key = fe.textBox1.Text = fe.textBox1.Text.Substring(0, EncryptFin.KeyLen);
                        fe.label3.Text = "0";
                    }
                    else
                    {
                        EncryptFin.Key = fe.textBox1.Text;
                        fe.label3.Text = (EncryptFin.KeyLen - fe.textBox1.TextLength).ToString();
                    }
                    EncryptFin.IV = fe.textBox2.Text;
                    EncryptFin.Keyfile = fe.textBox3.Text;
                    EncryptFin.MakeKeyfile = fe.checkBox1.Checked;
                    HideAll(2);
                    isOn[2] = true;
                    return fe;
                }
                EncryptFin.Acquired = true;
                return Final;
            }
            set
            {
                Final = null;
                EncryptFin.Acquired = false;
                EncryptFin.IV = EncryptFin.Key = EncryptFin.Keyfile = "";
                EncryptFin.MakeKeyfile = false;
            }
        }
        internal static Final FinDecrypt
        {
            get
            {
                if (EncryptFin.Acquired)
                {
                    Final fd = (Final)forms[2];
                    EncryptFin.Key = fd.textBox1.Text;
                    EncryptFin.IV = fd.textBox2.Text;
                    EncryptFin.MakeKeyfile = fd.checkBox1.Checked;
                    EncryptFin.Keyfile = fd.textBox3.Text;
                    EncryptFin.Acquired = false;
                }
                else if(DecryptFin.Acquired)
                {
                    Final fd = (Final)forms[2];
                    fd.keylength = DecryptFin.KeyLen;
                    fd.textBox1.MaxLength = DecryptFin.KeyLen;
                    if (fd.textBox1.TextLength > DecryptFin.KeyLen)
                    {
                        DecryptFin.Key = fd.textBox1.Text = fd.textBox1.Text.Substring(0, DecryptFin.KeyLen);
                        fd.label3.Text = "0";
                    }
                    else
                    {
                        DecryptFin.Key = fd.textBox1.Text;
                        fd.label3.Text = (DecryptFin.KeyLen - fd.textBox1.TextLength).ToString();
                    }
                    DecryptFin.IV = fd.textBox2.Text;
                    HideAll(2);
                    isOn[2] = true;
                    return fd;
                }
                DecryptFin.Acquired = true;
                return Final;
            }
            set
            {
                Final = null;
                DecryptFin.Acquired = false;
                DecryptFin.IV = DecryptFin.Key = "";
            }
        }
        private static Final Final
        {
            get
            {
                if(EncryptFin.Acquired)
                {
                    Final fe = (Final)forms[2];
                    fe.button1.Visible = fe.button2.Visible = fe.button5.Visible =
                        fe.textBox3.Visible = fe.checkBox1.Visible = true;
                    fe.checkBox1.Checked = fe.textBox3.Enabled = fe.button5.Enabled =
                        EncryptFin.MakeKeyfile ? true : false;
                    fe.textBox3.Text = EncryptFin.Keyfile;
                    if (EncryptFin.Key.Length > EncryptFin.KeyLen)
                    {
                        fe.textBox1.Text = EncryptFin.Key = EncryptFin.Key.Substring(0, EncryptFin.KeyLen);
                        fe.label3.Text = "0";
                    }
                    else
                    {
                        fe.textBox1.Text = EncryptFin.Key;
                        fe.label3.Text = (EncryptFin.KeyLen - EncryptFin.Key.Length).ToString();
                    }
                    fe.textBox1.MaxLength = EncryptFin.KeyLen;
                    fe.keylength = EncryptFin.KeyLen;
                    fe.textBox2.Text = EncryptFin.IV;
                    fe.label4.Text = (16 - EncryptFin.IV.Length).ToString();
                    fe.label6.Text = EncryptFin.Message;
                    fe.button3.Text = "Encrypt";
                    fe.Encrypt = true;
                    HideAll(2);
                    isOn[2] = true;
                    return fe;
                }
                else
                {
                    Final fd = (Final)forms[2];
                    fd.button1.Visible = fd.button2.Visible = fd.button5.Visible =
                        fd.textBox3.Visible = fd.checkBox1.Visible = false;
                    if (DecryptFin.Key.Length > DecryptFin.KeyLen)
                    {
                        fd.textBox1.Text = DecryptFin.Key = DecryptFin.Key.Substring(0, DecryptFin.KeyLen);
                        fd.label3.Text = "0";
                    }
                    else
                    {
                        fd.textBox1.Text = DecryptFin.Key;
                        fd.label3.Text = (DecryptFin.KeyLen - DecryptFin.Key.Length).ToString();
                    }
                    fd.textBox1.MaxLength = DecryptFin.KeyLen;
                    fd.keylength = DecryptFin.KeyLen;
                    fd.textBox2.Text = DecryptFin.IV;
                    fd.label4.Text = (16 - DecryptFin.IV.Length).ToString();
                    fd.label6.Text = DecryptFin.Message;
                    fd.button3.Text = "Decrypt";
                    fd.Encrypt = false;
                    HideAll(2);
                    isOn[2] = true;
                    return fd;
                }
            }
            set
            {
                forms[2].Hide();
                isOn[2] = false;
            }
        }
        internal static MakeKey MakeKey
        {
            get
            {
                HideAll(3);
                isOn[3] = true;
                return (MakeKey)forms[3];
            }
            set
            {
                forms[3].Hide();
                isOn[3] = false;
                MakeKey mk = (MakeKey)forms[3];
                mk.comboBox1.SelectedIndex = 1;
                mk.comboBox2.SelectedIndex = 0;
                mk.textBox1.Text = "";
                mk.textBox2.Text = "";
                mk.radioButton3.Checked = true;
            }
        }
        internal static WithKeyfile WithEncrypt
        {
            get
            {
                if(DecryptWith.Acquired)
                {
                    WithKeyfile dw = (WithKeyfile)forms[4];
                    DecryptWith.checkBox1 = dw.checkBox1.Checked;
                    DecryptWith.textBox1 = dw.textBox1.Text;
                    DecryptWith.textBox2 = dw.textBox2.Text;
                    DecryptWith.textBox3 = dw.textBox3.Text;
                    DecryptWith.Acquired = false;
                }
                else if(EncryptWith.Acquired)
                {
                    HideAll(4);
                    isOn[4] = true;
                    return (WithKeyfile)forms[4];
                }
                EncryptWith.Acquired = true;
                return WithKeyfile;
            }
            set
            {
                WithKeyfile = null;
                EncryptWith.Acquired = false;
                EncryptWith.textBox1 = EncryptWith.textBox2 = EncryptWith.textBox3 = "";
                EncryptWith.checkBox1 = false;
            }
        }
        internal static WithKeyfile WithDecrypt
        {
            get
            {
                if (EncryptWith.Acquired)
                {
                    WithKeyfile ew = (WithKeyfile)forms[4];
                    EncryptWith.checkBox1 = ew.checkBox1.Checked;
                    EncryptWith.textBox1 = ew.textBox1.Text;
                    EncryptWith.textBox2 = ew.textBox2.Text;
                    EncryptWith.textBox3 = ew.textBox3.Text;
                    EncryptWith.Acquired = false;
                }
                else if (DecryptWith.Acquired)
                {
                    HideAll(4);
                    isOn[4] = true;
                    return (WithKeyfile)forms[4];
                }
                DecryptWith.Acquired = true;
                return WithKeyfile;
            }
            set
            {
                WithKeyfile = null;
                DecryptWith.Acquired = false;
                DecryptWith.textBox1 = DecryptWith.textBox2 = DecryptWith.textBox3 = "";
                DecryptWith.checkBox1 = false;
            }
        }
        private static WithKeyfile WithKeyfile
        {
            get
            {
                if(EncryptWith.Acquired)
                {
                    WithKeyfile ew = (WithKeyfile)forms[4];
                    ew.Encrypt = true;
                    ew.button1.Text = "Encrypt";
                    ew.label1.Text = EncryptWith.label1;
                    ew.label6.Text = EncryptWith.label6;
                    ew.checkBox1.Checked = EncryptWith.checkBox1;
                    ew.textBox1.Text = EncryptWith.textBox1;
                    ew.textBox2.Text = EncryptWith.textBox2;
                    ew.textBox3.Text = EncryptWith.textBox3;
                    HideAll(4);
                    isOn[4] = true;
                    return ew;
                }
                else
                {
                    WithKeyfile dw = (WithKeyfile)forms[4];
                    dw.Encrypt = false;
                    dw.button1.Text = "Decrypt";
                    dw.label1.Text = DecryptWith.label1;
                    dw.label6.Text = DecryptWith.label6;
                    dw.checkBox1.Checked = DecryptWith.checkBox1;
                    dw.textBox1.Text = DecryptWith.textBox1;
                    dw.textBox2.Text = DecryptWith.textBox2;
                    dw.textBox3.Text = DecryptWith.textBox3;
                    HideAll(4);
                    isOn[4] = true;
                    return dw;
                }
            }
            set
            {
                forms[4].Hide();
                isOn[4] = false;
            }
        }
    }
}