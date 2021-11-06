using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDiffTool.Tools
{
    class FileTools
    {
        /// <summary>  计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="Size">初始文件大小</param>
        /// <returns></returns>
        public static string CountSize(long Size)
        {

            string m_strSize = "";
            long FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }

        public static string WithoutCountSize(long Size)
        {

            string m_strSize = "";
            long FactSize = 0;
            FactSize = Size;
            m_strSize = FactSize.ToString("F2");
            return m_strSize;
        }

        /// <summary> 获取指定文件的二进制流
        /// </summary>
        /// <param name="filePath">目标文件路径</param>
        /// <returns></returns>
        public static byte[] GetFileByte(FileInfo file)
        {
            string filePath = file.FullName;
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(fs);
                byte[] fileArray = r.ReadBytes((int)fs.Length);
                //读取文件数据流储存在fileArray中

                fs.Dispose();
                return fileArray;//返回文件数据
            }
            catch (IOException)
            {
                Console.WriteLine("Error:FileStream error");
            }

            //如出现文件流错误，则返回null
            return null;

        }
        
         /// <summary> 获取指定路径的文件二进制流
         /// </summary>
         /// <param name="filePath">目标文件路径</param>
         /// <returns></returns>
        public static byte[] GetFileByte(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(fs);
                byte[] fileArray = r.ReadBytes((int)fs.Length);
                //读取文件数据流储存在fileArray中

                fs.Dispose();
                return fileArray;//返回文件数据
            }
            catch (IOException)
            {
                Console.WriteLine("Error:FileStream error");
            }

            //如出现文件流错误，则返回null
            return null;

        }


        
    }
}
