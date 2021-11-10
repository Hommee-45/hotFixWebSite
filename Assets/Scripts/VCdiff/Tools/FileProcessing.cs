using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace FileDiffTool.Tools
{
    public class FileProcessing
    {

        public static void Processing(string srcPath, string tarPath, string outputPath)
        {
            //源文件夹（旧版本文件）
            DirectoryInfo srcFolder    = new DirectoryInfo(srcPath);
            //新文件夹（新版本文件）
            DirectoryInfo tarFolder    = new DirectoryInfo(tarPath);
            //差分文件储存位置
            DirectoryInfo outputFolder = new DirectoryInfo(outputPath);
            //临时文件夹
            DirectoryInfo tempFolder;
            //配置文件列表
            Dictionary<string, DiffConfig> diffCfgs = new Dictionary<string, DiffConfig>();
            //DES密钥
            string desKey = "1145141919810abcd";

            //创建临时文件夹
            string tempPath = outputPath + @"\temp";
            string diffPath = tempPath + @"\diff";
            string desPath = tempPath + @"\des";
            string gzipPath = tempPath + @"\gzip";
            if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
            tempFolder = new DirectoryInfo(tempPath);
            //temp-diff文件夹
            if (!Directory.Exists(diffPath)) Directory.CreateDirectory(diffPath);
            DirectoryInfo diffFolder = new DirectoryInfo(diffPath);
            //temp-des文件夹
            if (!Directory.Exists(desPath)) Directory.CreateDirectory(desPath);
            DirectoryInfo desFolder = new DirectoryInfo(desPath);
            //temp-gzip文件夹
            if (!Directory.Exists(gzipPath)) Directory.CreateDirectory(gzipPath);
            DirectoryInfo gzipFolder = new DirectoryInfo(gzipPath);

            //todo 文件差分，生成diff文件
            FolderComparison.Comparison(diffCfgs, tarFolder, srcFolder, diffFolder);

            //todo 差分文件加密
            FileInfo[] files = diffFolder.GetFiles();
            foreach (FileInfo file in files)
            {
                DES.EncryptFile(file.FullName, desFolder + @"\" + file.Name, desKey);
            }

            //todo 差分文件压缩
            files = desFolder.GetFiles();
            foreach (FileInfo file in files)
            {
                GZIP.CompressFile(file.FullName, gzipFolder + @"\" + file.Name);
            }
            
            //todo 重命名为MD5名称
            files = gzipFolder.GetFiles();
            foreach (FileInfo file in files)
            {
                string md5 = MD5.GetMD5(file);
                diffCfgs[file.Name].Set_MD5(md5);
                diffCfgs[file.Name].Set_DownLoadCRC(CRC.GetCRC(file));
                diffCfgs[file.Name].Set_FileSize(FileTools.WithoutCountSize(file.Length));
                file.CopyTo(outputPath + @"\" + md5);
            }
            
            //todo 配置文件保存
            List<string> Infos = new List<string>();
            foreach (string key in diffCfgs.Keys)
            {
                Infos.Add(diffCfgs[key].Get_ConfigDesc());
            }
            File.WriteAllLines(outputPath + @"\Congfigs.txt", Infos.ToArray());

            //todo 密钥保存
            File.WriteAllText(outputPath + @"\DESKey.txt", desKey);

            //todo 清理临时文件
            tempFolder.Delete(true);

            //（todo 整包压缩）
        }

        public static int ReverseProcessing(string srcPath, string diffsPath, string outputPath)
        {
            //源文件夹（旧版本文件）
            DirectoryInfo srcFolder = new DirectoryInfo(srcPath);
            //差分文件文件夹
            DirectoryInfo diffsFolder = new DirectoryInfo(diffsPath);
            //文件输出位置
            DirectoryInfo outputFolder = new DirectoryInfo(outputPath);
            //临时文件夹
            DirectoryInfo tempFolder;

            //todo  创建临时文件夹
            string tempPath = outputPath + @"\temp";
            string DecodeMD5Path = tempPath + @"\md5";
            string gzipPath = tempPath + @"\gzip";
            string diffPath = tempPath + @"\diff";
            //temp文件夹
            if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
            tempFolder = new DirectoryInfo(tempPath);
            //temp-gzip文件夹
            if (!Directory.Exists(gzipPath)) Directory.CreateDirectory(gzipPath);
            DirectoryInfo gzipFolder = new DirectoryInfo(gzipPath);
            //temp-diff文件夹
            if (!Directory.Exists(diffPath)) Directory.CreateDirectory(diffPath);
            DirectoryInfo diffFolder = new DirectoryInfo(diffPath);


            //todo  读取配置文件
            DiffConfigs diffCfgs = new DiffConfigs(DiffConfig.Get_Congfigs(File.ReadAllLines(diffsPath + @"\Congfigs.txt")));
            //todo  读取密钥
            string desKey = File.ReadAllText(diffsPath + @"\DESKey.txt");

            //todo  MD5校验 + 第一次CRC校验 + 解压
            FileInfo[] files = diffsFolder.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Name == "Congfigs.txt" || file.Name == "DESKey.txt")
                {
                    continue;
                }
                //MD5校验
                if (!MD5.CheckMD5(file, file.Name))
                {
                    //出错：MD5检验失败
                    return -1;
                }
                //CRC校验
                if (!CRC.CheckCRC(file, diffCfgs.Cfg_MD5(file.Name).Get_DownLoadCRC()))
                {
                    //出错：第一次CRC检验失败
                    return -2;
                }
                //解压
                GZIP.DecompressFile(file.FullName, gzipFolder + @"\" + file.Name);
            }

            //todo  解密
            files = gzipFolder.GetFiles();
            foreach (FileInfo file in files)
            {
                DES.DecryptFile(file.FullName, diffFolder + @"\" + file.Name, desKey);
            }

            //todo  第二次CRC校验
            files = diffFolder.GetFiles();
            foreach (FileInfo file in files)
            {
                if (!CRC.CheckCRC(file, diffCfgs.Cfg_MD5(file.Name).Get_UzpFileCRC()))
                {
                    //第二次CRC校验失败
                    return -3;
                }
            }

            //todo  还原文件
            if (FolderComparison.DeComparison(diffCfgs, srcFolder, diffFolder, outputFolder) != 1)
            {
                return -4;
            }

            //todo  第三次CRC校验
            files = outputFolder.GetFiles();
            foreach (FileInfo file in files)
            {
                if (!CRC.CheckCRC(file, diffCfgs.Cfg_MD5(file.Name).Get_OrgFileCRC()))
                {
                    //第三次CRC校验失败
                    return -5;
                }
            }

            //todo  临时文件清理
            //tempFolder.Delete(true);
            return 1;
        }

        public static int SingleRP(DiffConfig cfg, string desKey, string srcPath, string diffPath, string outputPath, string tempPath)
        {
            FileInfo srcFile  = new FileInfo(srcPath);
            FileInfo diffFile = new FileInfo(diffPath);

            //临时文件路径
            string tGzipPath = tempPath + @"\gzip";
            string tDiffPath = tempPath + @"\diff";
            //temp-gzip文件夹
            if (!Directory.Exists(tGzipPath)) Directory.CreateDirectory(tGzipPath);
            if (!Directory.Exists(tDiffPath)) Directory.CreateDirectory(tDiffPath);
            DirectoryInfo gzipFolder = new DirectoryInfo(tGzipPath);
            DirectoryInfo diffFolder = new DirectoryInfo(tDiffPath);

            //MD5校验
            if (!MD5.CheckMD5(diffFile, diffFile.Name))
            {
                //出错：MD5检验失败
                Debug.Log("MD5-NAME: " + diffFile.Name);
                return -1;
            }

            //第一次CRC校验
            if (!CRC.CheckCRC(diffFile, cfg.Get_DownLoadCRC()))
            {
                //出错：第一次CRC检验失败
                return -2;
            }

            //解压
            string tGzipFilePath = tGzipPath + @"\" + diffFile.Name;
            GZIP.DecompressFile(diffPath, tGzipFilePath);

            //解密
            string tDiffFilePath = tDiffPath + @"\" + diffFile.Name;
            DES.DecryptFile(tGzipFilePath, tDiffFilePath, desKey);

            //第二次CRC校验
            FileInfo tdiffFile = new FileInfo(tDiffFilePath);
            if (!CRC.CheckCRC(tdiffFile, cfg.Get_UzpFileCRC()))
            {
                //第二次CRC校验失败
                return -3;
            }

            //文件还原
            if (!FolderComparison.DoDecode(srcPath, tDiffFilePath, outputPath))
            {

                //文件还原失败
                return -4;
            }

            //第三次CRC校验
            FileInfo outputFile = new FileInfo(outputPath);
            if (!CRC.CheckCRC(outputFile, cfg.Get_OrgFileCRC()))
            {
                //第三次CRC校验失败
                return -5;
            }

            return 1;
        }
    }



    public class DiffConfig
    {
        public const int DATA_LENGTH = 7;
        public enum Mark { MODIFY = 0, ADD = 1 };
        private string MD5_NAME;
        private string RELATIVE_NAME;
        private string DOWNLOAD_CRC;
        private string DOWNLOAD_FILESIZE;
        private string UZPFILE_CRC;
        private string ORGFIlE_CRC;
        private string FILETYPE;
        private string m_LocalPath;
        private string m_TargetPath;

        public void Parse(string data)
        {
            Debug.Log(data);
            var val = data.Split('|');
            if (val.Length < DATA_LENGTH)
            {
                Debug.LogError("illegal string format, now it just count " + val.Length + " ,target count is: " + DATA_LENGTH);
                return;
            }
            MD5_NAME = val[0];
            RELATIVE_NAME = val[1];
            DOWNLOAD_CRC = val[2];
            DOWNLOAD_FILESIZE = val[3];
            UZPFILE_CRC = val[4];
            ORGFIlE_CRC = val[5];
            FILETYPE = val[6];
        }

        public string GetLocalPath()
        {
            return m_LocalPath;
        }
        public string GetTargetPath()
        {
            return m_TargetPath;
        }
        public string GetFileName()
        {
            string[] vector = Get_RelativePath().Split('\\');

            return vector[vector.Length - 1];
        }
        public string Get_MD5()
        {
            return MD5_NAME;
        }
        public void   Set_MD5(string md5)
        {
            MD5_NAME = md5;
        }

        public string Get_RelativePath()
        {
            return RELATIVE_NAME;
        }
        public void SetLocalPath(string localPath)
        {
            m_LocalPath = localPath;
        }
        public void SetTargetPath(string targetPath)
        {
            m_TargetPath = targetPath;
        }
        public void   Set_RelativePath(string path)
        {
            RELATIVE_NAME = path;
        }

        public int    Get_DownLoadCRC()
        {
            return int.Parse(DOWNLOAD_CRC);
        }
        public void   Set_DownLoadCRC(int crc)
        {
            DOWNLOAD_CRC = crc.ToString();
        }

        public string Get_FileSize()
        {
            return DOWNLOAD_FILESIZE;
        }
        public void   Set_FileSize(string size)
        {
            DOWNLOAD_FILESIZE = size;
        }

        public int    Get_UzpFileCRC()
        {
            return int.Parse(UZPFILE_CRC);
        }
        public void   Set_UzpFileCRC(int crc)
        {
            UZPFILE_CRC = crc.ToString();
        }

        public int    Get_OrgFileCRC()
        {
            return int.Parse(ORGFIlE_CRC);
        }
        public void   Set_OrgFileCRC(int crc)
        {
            ORGFIlE_CRC = crc.ToString();
        }

        public Mark   Get_FileType()
        {
            return (Mark)int.Parse(FILETYPE);
        }
        public void   Set_FileType(Mark type)
        {
            FILETYPE = type.GetHashCode().ToString();
        }

        public string Get_ConfigDesc()
        {
            string desc = "";
            desc = MD5_NAME;
            desc += "|" + RELATIVE_NAME;
            desc += "|" + DOWNLOAD_CRC;
            desc += "|" + DOWNLOAD_FILESIZE;
            desc += "|" + UZPFILE_CRC;
            desc += "|" + ORGFIlE_CRC;
            desc += "|" + FILETYPE;

            return desc;
        }

        public static DiffConfig[] Get_Congfigs(string[] cfgStr)
        {
            DiffConfig[] cfgs = new DiffConfig[cfgStr.Length];
            for (int i = 0; i < cfgStr.Length; i++)
            {
                string[] cfg = cfgStr[i].Split('|');
                cfgs[i] = new DiffConfig() {
                    MD5_NAME = cfg[0],
                    RELATIVE_NAME = cfg[1],
                    DOWNLOAD_CRC = cfg[2],
                    DOWNLOAD_FILESIZE = cfg[3],
                    UZPFILE_CRC = cfg[4],
                    ORGFIlE_CRC = cfg[5],
                    FILETYPE = cfg[6],
                };
            }
            return cfgs;
        }

        public void Clear()
        {
            MD5_NAME = "";
            RELATIVE_NAME = "";
            DOWNLOAD_CRC = "";
            DOWNLOAD_FILESIZE = "";
            UZPFILE_CRC = "";
            ORGFIlE_CRC = "";
            FILETYPE = "";
        }
    }

    class DiffConfigs
    {
        private DiffConfig[] diffConfigs;

        public DiffConfigs(DiffConfig[] cfgs)
        {
            diffConfigs = cfgs;
        }

        public DiffConfig Cfg_MD5(string md5)
        {
            foreach (DiffConfig cfg in diffConfigs)
            {
                if (cfg.Get_MD5() == md5)
                {
                    return cfg;
                }
            }
            return null;
        }
    }
}
