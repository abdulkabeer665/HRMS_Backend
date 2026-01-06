using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using static HRMS_Backend.BAL.ResponseParameters;

namespace HRMS_Backend.DAL
{
    public class EncryptDecryptPassword
    {

        #region "DecryptQueryString"
        internal static string DecryptQueryString(string strQueryString)
        {
            EncryptDecryptPassword objEDQueryString = new EncryptDecryptPassword();
            return EncryptDecryptPassword.Decrypt(strQueryString, "r0b1nr0y");
        }
        #endregion

        #region "EncryptQueryString"
        internal static string EncryptQueryString(string strQueryString)
        {
            EncryptDecryptPassword objEDQueryString = new EncryptDecryptPassword();
            return EncryptDecryptPassword.Encrypt(strQueryString, "r0b1nr0y");
        }
        #endregion

        #region "Encrypt"
        public static string Encrypt(string stringToEncrypt, string SEncryptionKey)
        {
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(SEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        //internal static string DecryptQueryString(string v)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion

        #region "Decrypt"
        public static string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {

            byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion

        #region "Keys"

        public static byte[] key = { };

        public static byte[] IV = {
            0x12,
            0x34,
            0x56,
            0x78,
            0x90,
            0xab,
            0xcd,
            0xef

        };

        #endregion

        #region "License Key Encryption"

        public static bool ValidateKey(string LicKey, string SerialNo)
        {

            #region Variables

            var substringSerialNo = "";
            var Key2Validate = new long[4];
            Int32 StartPos;
            var encryptValue = "";
            string[] arr;
            string NegKey = "";
            string FinalKey = "";

            #endregion

            try
            {
                arr = LicKey.Split("-");
                substringSerialNo = SerialNo.Substring(4);
                encryptValue = Encrypt("ABTAK56", substringSerialNo, 1);
                arr[3] = arr[3].Replace("*", "-");
                //int abc123123 = Convert.ToInt32(arr[3].Replace("*", "-"));
                int abc123123 = Convert.ToInt32(arr[3]);
                int inte = (int)Convert.ToDouble(abc123123 + 197);
                string inteCStr = inte.ToString();
                if (inteCStr.Length == 1)
                {
                    string inteCtrStrRight = inteCStr.Substring(2 - 2);
                    int Cint123 = Convert.ToInt32(inteCtrStrRight);
                    StartPos = Cint123;
                }
                else
                {
                    string inteCtrStrRight = inteCStr.Substring(inteCStr.Length - 2);
                    int Cint123 = Convert.ToInt32(inteCtrStrRight);
                    StartPos = Cint123;
                }

                #region License Key Encrypt Algorithm

                #region Key 1

                string leftStr1 = encryptValue.Substring(0, 5);
                int leftStrInt1 = int.Parse(leftStr1, NumberStyles.HexNumber) / 121;
                Key2Validate[0] = leftStrInt1;

                #endregion

                #region Key 2

                string MidStr2 = encryptValue.Substring(5, 5);
                int MidStrInt2 = int.Parse(MidStr2, NumberStyles.HexNumber) / 123;
                Key2Validate[1] = MidStrInt2;

                #endregion

                #region Key 3

                string MidStr3 = encryptValue.Substring(StartPos - 1, 5);
                int MidStrInt3 = int.Parse(MidStr3, NumberStyles.HexNumber) / 127;
                Key2Validate[2] = MidStrInt3;

                #endregion

                #region Key 4

                string RightStr4 = encryptValue.Substring(encryptValue.Length - 2);
                int RightStrInt4 = int.Parse(RightStr4, NumberStyles.HexNumber);
                Key2Validate[3] = RightStrInt4;

                string KeyLongToString = Key2Validate[3].ToString();
                string StartPosStr = StartPos.ToString();
                string SetStrFunction = SetStr(StartPosStr, 2, "0", "L");
                string kabeer123 = KeyLongToString + SetStrFunction;
                int SetStrFunction197GetInt = (int)(Convert.ToInt32(kabeer123) - 197);
                Key2Validate[3] = SetStrFunction197GetInt;

                #endregion

                NegKey = Key2Validate[3] < 0 ? NegKey = "*" + Math.Abs(Key2Validate[3]) : NegKey = Convert.ToString(Math.Abs(Key2Validate[3]));

                FinalKey = Math.Abs(Key2Validate[0]) + "-" + Math.Abs(Key2Validate[1]) + "-" + Math.Abs(Key2Validate[2]) + "-" + NegKey;

                #endregion

                if (LicKey == FinalKey)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string ValidateKey2(string SerialNo)
        {

            #region Variables

            var substringSerialNo = "";
            var Key2Validate = new long[4];
            Int32 StartPos = 14;
            var encryptValue = "";
            string[] arr;
            string NegKey = "";
            string FinalKey = "";

            #endregion

            try
            {
                substringSerialNo = SerialNo.Substring(4);
                encryptValue = Encrypt("ABTAK56", substringSerialNo, 1);

                #region License Key Encrypt Algorithm

                #region Key 1

                string leftStr1 = encryptValue.Substring(0, 5);
                int leftStrInt1 = int.Parse(leftStr1, NumberStyles.HexNumber) / 121;
                Key2Validate[0] = leftStrInt1;

                #endregion

                #region Key 2

                string MidStr2 = encryptValue.Substring(5, 5);
                int MidStrInt2 = int.Parse(MidStr2, NumberStyles.HexNumber) / 123;
                Key2Validate[1] = MidStrInt2;

                #endregion

                #region Key 3

                string MidStr3 = encryptValue.Substring(StartPos - 1, 5);
                int MidStrInt3 = int.Parse(MidStr3, NumberStyles.HexNumber) / 127;
                Key2Validate[2] = MidStrInt3;

                #endregion

                #region Key 4

                string RightStr4 = encryptValue.Substring(encryptValue.Length - 2);
                int RightStrInt4 = int.Parse(RightStr4, NumberStyles.HexNumber);
                Key2Validate[3] = RightStrInt4;

                string KeyLongToString = Key2Validate[3].ToString();
                string StartPosStr = StartPos.ToString();
                string SetStrFunction = SetStr(StartPosStr, 2, "0", "L");
                string kabeer123 = KeyLongToString + SetStrFunction;
                int SetStrFunction197GetInt = (int)(Convert.ToInt32(kabeer123) - 197);
                Key2Validate[3] = SetStrFunction197GetInt;

                #endregion

                NegKey = Key2Validate[3] < 0 ? NegKey = "*" + Math.Abs(Key2Validate[3]) : NegKey = Convert.ToString(Math.Abs(Key2Validate[3]));

                FinalKey = Math.Abs(Key2Validate[0]) + "-" + Math.Abs(Key2Validate[1]) + "-" + Math.Abs(Key2Validate[2]) + "-" + NegKey;

                #endregion

                return FinalKey;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        private static string Encrypt(string CodeKey, string DataIn, int EncLevel)
        {
            Message msg = new Message();
            int longDataPtr;
            int temp;
            var tempString = "";
            string strDataOut = "";
            int intXorValue1;
            int intXorValue2;
            var aabbccSubstring = "";

            try
            {
                for (int i = 0; i < DataIn.Length; i++)
                {
                    longDataPtr = i;
                    var kabeer = DataIn.Substring(longDataPtr, 1);

                    byte[] byteArrray = Encoding.Unicode.GetBytes(kabeer);
                    char[] charArray = Encoding.ASCII.GetChars(byteArrray);

                    intXorValue1 = Convert.ToInt16(BitConverter.ToInt16(Encoding.ASCII.GetBytes(charArray)));

                    var aabbcc = longDataPtr % CodeKey.Length;
                    if (aabbccSubstring == "6")
                    {
                        aabbccSubstring = CodeKey;
                        aabbcc = -1;
                    }
                    aabbccSubstring = CodeKey.Substring(aabbcc + 1, 1);
                    byte[] byteArray2 = Encoding.Unicode.GetBytes(aabbccSubstring);
                    char[] charArray2 = Encoding.ASCII.GetChars(byteArray2);
                    intXorValue2 = (Int32)(BitConverter.ToInt16(Encoding.ASCII.GetBytes(charArray2)));

                    temp = intXorValue1 ^ intXorValue2;
                    tempString = temp.ToString("X");
                    tempString = tempString.Length == 1 ? "0" + tempString : tempString;

                    strDataOut = strDataOut + tempString;

                }
                return strDataOut;
            }
            catch (Exception ex)
            {
                msg.message = ex.Message.ToString();
                return msg.message;
            }


        }

        private static string SetStr(string str, int StrLen, string PadChr, string PadLoc)
        {
            int i;
            int ActualLen;
            int AddSpace;

            ActualLen = str.Length;

            if (ActualLen >= StrLen)
            {
                str = str.Substring(0, StrLen);
            }
            else
            {
                AddSpace = StrLen - ActualLen;
                for (i = 1; i <= AddSpace; i++)
                {
                    if (PadLoc == "R")
                    {
                        str = str + PadChr;
                    }
                    else
                    {
                        str = PadChr + str;
                    }
                }
            }
            return str;
        }

        public static string GenerateLicKey(string LicKey, string SerialNo)
        {

            #region Variables

            var substringSerialNo = "";
            var Key2Validate = new long[4];
            Int32 StartPos;
            var encryptValue = "";
            string[] arr;
            string NegKey = "";
            string FinalKey = "";

            #endregion

            try
            {
                arr = LicKey.Split("-");
                substringSerialNo = SerialNo.Substring(4);
                encryptValue = Encrypt("ABTAK56", substringSerialNo, 1);
                arr[3] = arr[3].Replace("*", "-");
                //int abc123123 = Convert.ToInt32(arr[3].Replace("*", "-"));
                int abc123123 = Convert.ToInt32(arr[3]);
                int inte = (int)Convert.ToDouble(abc123123 + 197);
                string inteCStr = inte.ToString();
                string inteCtrStrRight = inteCStr.Substring(inteCStr.Length - 2);
                int Cint123 = Convert.ToInt32(inteCtrStrRight);
                StartPos = Cint123;

                #region License Key Encrypt Algorithm

                #region Key 1

                string leftStr1 = encryptValue.Substring(0, 5);
                int leftStrInt1 = int.Parse(leftStr1, NumberStyles.HexNumber) / 121;
                Key2Validate[0] = leftStrInt1;

                #endregion

                #region Key 2

                string MidStr2 = encryptValue.Substring(5, 5);
                int MidStrInt2 = int.Parse(MidStr2, NumberStyles.HexNumber) / 123;
                Key2Validate[1] = MidStrInt2;

                #endregion

                #region Key 3

                string MidStr3 = encryptValue.Substring(StartPos - 1, 5);
                int MidStrInt3 = int.Parse(MidStr3, NumberStyles.HexNumber) / 127;
                Key2Validate[2] = MidStrInt3;

                #endregion

                #region Key 4

                string RightStr4 = encryptValue.Substring(encryptValue.Length - 2);
                int RightStrInt4 = int.Parse(RightStr4, NumberStyles.HexNumber);
                Key2Validate[3] = RightStrInt4;

                string KeyLongToString = Key2Validate[3].ToString();
                string StartPosStr = StartPos.ToString();
                string SetStrFunction = SetStr(StartPosStr, 2, "0", "L");
                string kabeer123 = KeyLongToString + SetStrFunction;
                int SetStrFunction197GetInt = (int)(Convert.ToInt32(kabeer123) - 197);
                Key2Validate[3] = SetStrFunction197GetInt;

                #endregion

                NegKey = Key2Validate[3] < 0 ? NegKey = "*" + Math.Abs(Key2Validate[3]) : NegKey = Convert.ToString(Math.Abs(Key2Validate[3]));

                FinalKey = Math.Abs(Key2Validate[0]) + "-" + Math.Abs(Key2Validate[1]) + "-" + Math.Abs(Key2Validate[2]) + "-" + NegKey;

                #endregion

                return FinalKey;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion

    }
}
