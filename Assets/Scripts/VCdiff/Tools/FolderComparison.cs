using FileDiffTool.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCDiff.Decoders;
using VCDiff.Encoders;
using VCDiff.Includes;

namespace FileDiffTool
{
    
    class FolderComparison
    {
        public static void Comparison(Dictionary<string, DiffConfig> cfgs, DirectoryInfo target, DirectoryInfo source, DirectoryInfo output)
        {
            Dictionary<string, FileInfo> tarFiles = target.GetFiles().ToDictionary(key => key.Name, fileobj => fileobj);
            Dictionary<string, FileInfo> srcFiles = source.GetFiles().ToDictionary(key => key.Name, fileobj => fileobj);
            string outputPath = output.FullName;
            foreach (string fileName in tarFiles.Keys)
            {
                DiffConfig cfg = new DiffConfig();
                string relativePath = tarFiles[fileName].FullName.Replace(target.FullName, target.Name);
                cfg.Set_RelativePath(relativePath);
                cfg.Set_OrgFileCRC(CRC.GetCRC(tarFiles[fileName]));

                string filePath = outputPath + @"\" + fileName;
                if (!srcFiles.Keys.Contains(fileName))
                {
                    //新增(源文件集中不存在)
                    //直接拷贝至目标文件夹
                    tarFiles[fileName].CopyTo(filePath);
                    cfg.Set_FileType(DiffConfig.Mark.ADD);
                }
                else
                {
                    //存在相同文件
                    //（判断是否不同？）
                    //文件差分
                    DoEncode(srcFiles[fileName].FullName, tarFiles[fileName].FullName, filePath);
                    cfg.Set_FileType(DiffConfig.Mark.MODIFY);
                }
                //配置记录

                cfg.Set_UzpFileCRC(CRC.GetCRC(new FileInfo(filePath)));
                cfgs.Add(fileName, cfg);
            }
        }

        public static int DeComparison(DiffConfigs cfgs, DirectoryInfo source, DirectoryInfo diff, DirectoryInfo output)
        {
            Dictionary<string, FileInfo> srcFiles = source.GetFiles().ToDictionary(key => key.Name, fileobj => fileobj);
            Dictionary<string, FileInfo> diffFiles = diff.GetFiles().ToDictionary(key => key.Name, fileobj => fileobj);
            
            foreach (string fileMD5 in diffFiles.Keys)
            {
                DiffConfig cfg = cfgs.Cfg_MD5(fileMD5);
                string outputPath = output.FullName.Replace(output.Name, "") + cfg.Get_RelativePath();
                //若为新增文件
                if (cfg.Get_FileType() == DiffConfig.Mark.ADD)
                {
                    diffFiles[fileMD5].CopyTo(outputPath);
                }
                else
                {
                    string fileName = cfg.Get_RelativePath().Replace(source.Name + @"\", "");
                    DoDecode(srcFiles[fileName].FullName, diffFiles[fileMD5].FullName, outputPath);
                }
                //CRC校验
                if (!CRC.CheckCRC(new FileInfo(outputPath), cfg.Get_OrgFileCRC()))
                {
                    return -1;
                }
            }

            return 1;
        }

        public static void DoEncode(string dictFile, string targetFile, string outputFile)
        {
            using (FileStream output = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (FileStream dict = new FileStream(dictFile, FileMode.Open, FileAccess.Read))
            using (FileStream target = new FileStream(targetFile, FileMode.Open, FileAccess.Read))
            {
                VCCoder coder = new VCCoder(dict, target, output);
                VCDiffResult result = coder.Encode(); //encodes with no checksum and not interleaved
                if (result != VCDiffResult.SUCCESS)
                {
                    Console.Write("Fail");
                    //error was not able to encode properly
                }

            }
        }

        public static bool DoDecode(string dictFile, string diffFile, string outputFile)
        {
            using (FileStream output = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
            using (FileStream dict = new FileStream(dictFile, FileMode.Open, FileAccess.Read))
            using (FileStream target = new FileStream(diffFile, FileMode.Open, FileAccess.Read))
            {
                VCDecoder decoder = new VCDecoder(dict, target, output);

                //You must call decoder.Start() first. The header of the delta file must be available before calling decoder.Start()

                VCDiffResult result = decoder.Start();

                if (result != VCDiffResult.SUCCESS)
                {
                    return false;
                    //error abort
                }

                long bytesWritten = 0;
                result = decoder.Decode(out bytesWritten);

                if (result != VCDiffResult.SUCCESS)
                {
                    return false;
                    //error decoding
                }
                return true;
                //if success bytesWritten will contain the number of bytes that were decoded
            }
        }

    }
}
