using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotfixFrameWork
{

    public class GlobalVariable
    {
        //全局list 保存热更新配置文件中 有变动的文件项
        public static List<FileDiffTool.Tools.DiffConfig> g_FileInfoList = new List<FileDiffTool.Tools.DiffConfig>();
        //当前客户端 版本
        public static Version g_LocalVersion;
        //当前DESKey
        public static string g_DESKey = "";
        //差分字符串 文件夹名称
        public static string VERISION_DIFF_FILENAME = "";
    }

    //public class DiffConfig
    //{
    //    public enum Mark
    //    {
    //        MODIFY = 0,
    //        ADD = 1,
    //    }

    //    public const int DATA_LENGTH = 7;

    //    public string MD5NAME
    //    {
    //        get {return m_MD5Name;}
    //        private set {m_MD5Name = value;}
    //    }

    //    public string RelativePath
    //    {
    //        get {return m_RelativePath;}
    //        private set {m_RelativePath = value;}
    //    }

    //    public string DownloadCRC
    //    {
    //        get {return m_DownloadCRC;}
    //        private set {m_DownloadCRC = value;}
    //    }

    //    public string FileSize
    //    {
    //        get {return m_FileSize;}
    //        private set {m_FileSize = value;}
    //    }

    //    public string UpzippedFileCRC
    //    {
    //        get {return m_UpzippedFileCRC;}
    //        private set {m_UpzippedFileCRC = value;}
    //    }

    //    public string OriginFileCRC
    //    {
    //        get {return m_OriginFileCRC;}
    //        private set {m_OriginFileCRC = value;}
    //    }

    //    public Mark FileType
    //    {
    //        get {return (Mark)int.Parse(m_FileType);}
    //        private set { m_FileType = value.ToString();}
    //    }

    //    //以md5命名的文件(加密压缩后的
    //    private string m_MD5Name;
    //    //相对路径
    //    private string m_RelativePath;
    //    //差分文件(压缩，加密)的CRC
    //    private string m_DownloadCRC;
    //    //网络下载文件的大小(用于统计流量消耗)
    //    private string m_FileSize;
    //    //差分文件(解压后， 解密后)的CRC
    //    private string m_UpzippedFileCRC;
    //    //新版本(合并后)文件的CRC
    //    private string m_OriginFileCRC;
    //    //标记位(0 - 修改  1-新增)
    //    private string m_FileType;

    //    public DiffConfig(string str)
    //    {
    //        Parse(str);
    //    }

    //    private void Parse(string data)
    //    {

    //        var val = data.Split('|');
    //        if (val.Length < DATA_LENGTH)
    //        {
    //            Debug.LogError("illegal string format, now it just count " + val.Length + " ,target count is: " + DATA_LENGTH);
    //            return;
    //        }
    //        m_MD5Name = val[0];
    //        m_RelativePath = val[1];
    //        m_DownloadCRC = val[2];
    //        m_FileSize = val[3];
    //        m_UpzippedFileCRC = val[4];
    //        m_OriginFileCRC = val[5];
    //        m_FileType = val[6];
    //    }

    //}

}
