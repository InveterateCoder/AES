using System.Security.Cryptography;
using System.IO;
using System;
using System.Windows.Forms;

namespace AES
{
    public enum Direction { Encrypt, Decrypt };
    internal static class KeyData
    {
        internal static bool Cancel, Exit = true;
        internal static int val;
        internal static Direction direction;
        internal static byte[] Key;
        internal static byte[] IV;
        internal static PaddingMode PM;
        internal static CipherMode CM;
        private static string _FileFrom;
        private static string _FileTo;
        private static string _KeyfileWrite;
        private static string _KeyfileRead;
        internal static string FileFrom
        {
            get => _FileFrom;
            set
            {
                if (!Path.IsPathRooted(value))
                    throw new Exception("The specified input path is not rooted.");
                if ((Path.GetPathRoot(value)).Length != 3)
                    throw new Exception("The input path's root is corrupted.");
                if (value.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                    throw new Exception("Invalid path characters are detected in the input.");
                if ((Path.GetFileName(value)).IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                    throw new Exception("Invalid file name characters are detected in the input.");
                FileInfo fi = new FileInfo(value);
                if (!fi.Exists)
                    throw new Exception("The input file doesn't exist.");
                _FileFrom = value;
            }
        }
        internal static string FileTo
        {
            get => _FileTo;
            set
            {
                if (!Path.IsPathRooted(value))
                    throw new Exception("The specified output path is not rooted.");
                if ((Path.GetPathRoot(value)).Length != 3)
                    throw new Exception("The output path's root is corrupted.");
                if (value.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                    throw new Exception("Invalid path characters are detected in the output.");
                if ((Path.GetFileName(value)).IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                    throw new Exception("Invalid file name characters are detected in the output.");
                FileInfo fi = new FileInfo(Path.GetDirectoryName(value) + "\\testingAESabilitytosavefile");
                (fi.Create()).Close();
                fi.Delete();
                _FileTo = value;
            }
        }
        internal static string KeyfileRead
        {
            get => _KeyfileRead;
            set
            {
                if (value == null)
                {
                    _KeyfileRead = value;
                    return;
                }
                KeyfileCheck(value);
                FileInfo fi = new FileInfo(value);
                if (!fi.Exists)
                    throw new Exception("The specified keyfile doesn't exist.");
                _KeyfileRead = value;
            }
        }
        internal static string KeyfileWrite
        {
            get => _KeyfileWrite;
            set
            {
                if (value == null)
                {
                    _KeyfileWrite = value;
                    return;
                }
                KeyfileCheck(value);
                _KeyfileWrite = value;
                FileInfo fi = new FileInfo(_KeyfileWrite);
                if (fi.Exists)
                    (fi.OpenWrite()).Close();
                else
                {
                    FileStream fs = fi.OpenWrite();
                    fs.Close();
                    fi.Delete();
                }
            }
        }
        private static void KeyfileCheck(string str)
        {
            if (str.Length == 0)
                throw new Exception("The keyfile path cannot be empty.");
            if (!Path.IsPathRooted(str))
                throw new Exception("The keyfile path is not rooted.");
            if (str.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new Exception("Invalid path characters are detected.");
            if ((Path.GetFileName(str)).IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                throw new Exception("Invalid file name characters are detected.");
            if (FileTo.Equals(str, StringComparison.OrdinalIgnoreCase))
                throw new Exception("The paths of the keyfile and the output file cannot be same.");
        }
        internal static void ProcessData(object obj)
        {
            Exit = false;
            Cancel = false;
            FileInfo fileTo = null, keyfile = null;
            FileStream streamFrom = null, streamTo = null;
            try
            {
                FileInfo fileFrom = new FileInfo(FileFrom);
                fileTo = new FileInfo(FileTo + "AESOutputTemporaryName");
                streamFrom = fileFrom.OpenRead();
                streamTo = fileTo.OpenWrite();
                streamTo.SetLength(0);
                AesManaged aes = new AesManaged();
                aes.Key = Key;
                aes.IV = IV;
                aes.Padding = PM;
                aes.Mode = CM;
                byte[] chunk = new byte[4096];
                int count = 0;
                double relation = (double)100 / (double)fileFrom.Length;
                long moved = 0;
                val = 0;
                if (direction == Direction.Encrypt)
                {
                    using (CryptoStream cs = new CryptoStream(streamTo, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        do
                        {
                            if (Cancel)
                            {
                                cs.Close();
                                streamFrom.Close();
                                if (fileTo.Exists)
                                    fileTo.Delete();
                                Cancel = false;
                                val = 100;
                                return;
                            }
                            count = streamFrom.Read(chunk, 0, 4096);
                            if (count != 0)
                            {
                                cs.Write(chunk, 0, count);
                                moved += count;
                                val = (int)Math.Round(relation * moved);
                            }
                        } while (count != 0);
                    }
                    streamFrom.Close();
                    if (_KeyfileWrite != null)
                    {
                        keyfile = new FileInfo(_KeyfileWrite);
                        _KeyfileWrite = null;
                        BinaryWriter bw = new BinaryWriter(keyfile.OpenWrite());
                        bw.BaseStream.SetLength(0);
                        bw.Write((byte)aes.Key.Length);
                        bw.Write(Key);
                        bw.Write(IV);
                        bw.Write((byte)CM);
                        bw.Write((byte)PM);
                        bw.Close();
                        keyfile.Attributes = FileAttributes.ReadOnly;
                    }
                }
                else if (direction == Direction.Decrypt)
                {
                    using (CryptoStream cs = new CryptoStream(streamFrom, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        do
                        {
                            if (Cancel)
                            {
                                cs.Close();
                                streamTo.Close();
                                if (fileTo.Exists)
                                    fileTo.Delete();
                                Cancel = false;
                                val = 100;
                                return;
                            }
                            count = cs.Read(chunk, 0, 4096);
                            if (count != 0)
                            {
                                streamTo.Write(chunk, 0, count);
                                moved += count;
                                val = (int)Math.Round(relation * moved);
                            }
                        } while (count != 0);
                    }
                    streamTo.Close();
                }
                else
                    throw new Exception("Direction must be specify");
                if (FileFrom.Equals(FileTo, System.StringComparison.OrdinalIgnoreCase))
                    fileFrom.Delete();
                else
                {
                    FileInfo fi = new FileInfo(FileTo);
                    if (fi.Exists)
                        fi.Delete();
                }
                fileTo.MoveTo(FileTo);
                val = 100;
            }
            catch(Exception exc)
            {
                streamFrom.Close();
                streamTo.Close();
                val = 100;
                if (keyfile != null)
                    keyfile.Delete();
                if (fileTo.Exists)
                    fileTo.Delete();
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
