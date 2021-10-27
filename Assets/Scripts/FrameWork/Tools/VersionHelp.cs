using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HotfixFrameWork
{

    public class VersionHelp
    {

        private static Version m_Version;

        public static Version version
        {
            get
            {
                if (m_Version == null)
                {
                    m_Version = GetLocalVersionForApp();
                }
                return m_Version;
            }
        }

        public static Version GetLocalVersionForModule(string module)
        {
            string versionFilePath = Path.Combine(Application.streamingAssetsPath, GamePathConfig.ANDROID_VERSION_FILENAME);

            if (!File.Exists(versionFilePath))
            {
                return null;
            }

            try
            {
                string text = File.ReadAllText(versionFilePath);
                return StringForVersion(text);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
                return null;
            }


        }



        public static Version JsonForVersion(string json)
        {
            try
            {
                Version version = JsonUtility.FromJson<Version>(json);
                return version;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
                return null;
            }
        }

        public static Version StringForVersion(string text)
        {
            Version version = new Version();
            version.version = text.Trim();
            return version;
        }

        public static Version GetLocalVersionForApp()
        {
            string versionFilePath = Path.Combine(Application.streamingAssetsPath, GamePathConfig.ANDROID_VERSION_FILENAME);
            if (!File.Exists(versionFilePath)) return null;
            string text = File.ReadAllText(versionFilePath);
            if (string.IsNullOrEmpty(text)) return null;
            return JsonForVersion(text);
        }

        public static void WriteLocalVersionFile(Version version)
        {
            string versionFilePath = Path.Combine(Application.streamingAssetsPath, GamePathConfig.ANDROID_VERSION_FILENAME);
            //创建根目录
            if (!Directory.Exists(Path.GetDirectoryName(versionFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(versionFilePath));
            }

            //删除旧文件
            if (File.Exists(versionFilePath))
            {
                File.Delete(versionFilePath);
            }
            string json = JsonUtility.ToJson(version);

            try
            {
                File.WriteAllText(versionFilePath, json);
                m_Version = version;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
                throw;
            }
        }
    }

}
