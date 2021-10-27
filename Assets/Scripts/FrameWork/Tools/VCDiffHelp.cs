using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VCDiff.Includes;
using VCDiff.Decoders;
using VCDiff.Shared;
using System.IO;

namespace HotfixFrameWork
{

    public class VCDiffHelp
    {
        /// <summary>
        /// 解码合并
        /// </summary>
        /// <param name="dictFile">源文件路径</param>
        /// <param name="diffFile">差分文件路径</param>
        /// <param name="outputFile">目标文件路径</param>
        /// <returns></returns>
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
