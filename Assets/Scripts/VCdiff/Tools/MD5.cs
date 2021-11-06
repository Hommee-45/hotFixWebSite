using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace FileDiffTool.Tools
{
    class MD5
    {
        /// <summary>  返回文件的MD5校验咋hi
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetMD5(FileInfo file)
        {
            Byte[] fileBs =  FileTools.GetFileByte(file);
            string fileStr = System.Text.Encoding.UTF8.GetString(fileBs);

            
            System.Security.Cryptography.MD5 md5 = new MD5CryptoServiceProvider();
            //将字符串转换为字节数组
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(fileStr);
            //计算字节数组的哈希值
            byte[] toData = md5.ComputeHash(fromData);
            //使用System.Security.Cryptography自带hash计算函数

            string byteStr = "";
            for (int i = 0; i < toData.Length; i++)
            {
                byteStr += toData[i].ToString("x");
            }

            return byteStr.Substring(0, 16);//位数
        }

        public static bool CheckMD5(FileInfo file, string MD5)
        {
            if (GetMD5(file) != MD5) return false;
            return true;
        }
    }
}
