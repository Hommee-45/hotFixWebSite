using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDiffTool.Tools
{
    class CRC
    {
        /// <summary> 根据节流计算得到CRC
        /// </summary>
        /// <param name="tagetBytes"> 需要计算CRC的目标字节流</param>
        public static int GetCRC(byte[] tagetBytes)
        {
            return CalculationCRC(tagetBytes);
        }

        /// <summary> 获取目标文件字节流,对其计算得到CRC
        /// </summary>
        /// <param name="tagetFile"> 需要计算CRC的目标文件</param>
        public static int GetCRC(FileInfo tagetFile)
        {
            byte[] bsTarget = FileTools.GetFileByte(tagetFile);
            return CalculationCRC(bsTarget);
        }

        /// <summary> 对字节流进行CRC16校验
        /// 重新计算CRC与获取的CRC进行比对
        /// </summary>
        /// <param name="srcBytes">源字节流</param>
        /// <param name="CRC">获取的CRC校验码</param>
        /// <returns>校验是否成功</returns>
        public static bool CheckCRC(byte[] srcBytes, int CRC)
        {
            int temp = GetCRC(srcBytes);

            if (temp != CRC) return false;

            return true;
        }

        /// <summary> 对文件进行CRC16校验
        /// 重新计算CRC与获取的CRC进行比对
        /// </summary>
        /// <param name="tagetFile">源文件</param>
        /// <param name="CRC">获取的CRC校验码</param>
        /// <returns>校验是否成功</returns>
        public static bool CheckCRC(FileInfo tagetFile, int CRC)
        {
            int temp = GetCRC(tagetFile);

            if (temp != CRC) return false;

            return true;
        } 

        /// <summary>  计算CRC
        /// </summary>
        /// <param name="bs">目标字节流</param>
        /// <returns></returns>
        private static int CalculationCRC(byte[] bs)
        {
            int CRC = 0x0000ffff;
            int len = bs.Length;
            int POLYNOMIAL = 0x0000a001;//CRC多项式
            int i, j;
            for (i = 0; i < len; i++)
            {
                CRC ^= ((int)bs[i] & 0x000000ff);
                for (j = 0; j < 8; j++)
                {
                    if ((CRC & 0x00000001) != 0)
                    {
                        CRC >>= 1;
                        CRC ^= POLYNOMIAL;
                    }
                    else
                    {
                        CRC >>= 1;
                    }
                }
            }
            return CRC;//返回crc校验码
        }
    }
}
