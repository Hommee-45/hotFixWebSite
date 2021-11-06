using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileDiffTool.Tools
{
    class DES
    {
        /// <summary>
        /// 加密文件
        /// </summary>
        /// <param name="filePath">源文件</param>
        /// <param name="savePath">保存文件</param>
        /// <param name="keyString">密钥</param>
        public static void EncryptFile(string filePath, string savePath, string keyString)
        {
            var des = GetDesCryptoServiceProvider(keyString);
            //创建加密器
            ICryptoTransform cryptoTransform = des.CreateEncryptor();
            CryptoFileContent(filePath, savePath, cryptoTransform);
        }

        /// <summary>
        /// 解密文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="savePath"></param>
        /// <param name="keyString"></param>
        public static void DecryptFile(string filePath, string savePath, string keyString)
        {
            var des = GetDesCryptoServiceProvider(keyString);
            //创建解密器
            ICryptoTransform cryptoTransform = des.CreateDecryptor();
            CryptoFileContent(filePath, savePath, cryptoTransform);
        }

        /// <summary>
        /// 获取加密服务提供者
        /// </summary>
        /// <param name="keyStr"></param>
        /// <returns></returns>
        private static DESCryptoServiceProvider GetDesCryptoServiceProvider(string keyStr)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(keyStr);
            //计算指定字节组指定区域哈希值
            SHA1 sha = new SHA1Managed();
            byte[] hash = sha.ComputeHash(keyBytes);
            //加密密钥数组
            byte[] key = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                key[i] = hash[i];
            }
            //DES加密
            DESCryptoServiceProvider des = new DESCryptoServiceProvider { Key = key, IV = key };
            return des;
        }

        /// <summary>
        /// 加密或解密文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="savePath"></param>
        /// <param name="cryptoTransform"></param>
        private static void CryptoFileContent(string filePath, string savePath, ICryptoTransform cryptoTransform)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream =
                    new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    var inputByteArray = FileTools.GetFileByte(filePath);
                    cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
                    cryptoStream.FlushFinalBlock();
                    SaveFile(savePath, memoryStream);
                }
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="memoryStream"></param>
        private static void SaveFile(string savePath, MemoryStream memoryStream)
        {
            using (FileStream fileStream = File.OpenWrite(savePath))
            {
                memoryStream.WriteTo(fileStream);
            }
        }

    }
}
