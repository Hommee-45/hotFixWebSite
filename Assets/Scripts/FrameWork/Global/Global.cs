using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotfixFrameWork
{

    public class GlobalVariable
    {
        //全局list 保存热更新配置文件中 有变动的文件项
        public static List<DiffFileInfo> g_FileInfoList = new List<DiffFileInfo>();
        //当前客户端 版本
        public static Version g_LocalVersion;        
        //差分字符串 文件夹名称
        public static string VERISION_DIFF_FILENAME = "";
    }


}
