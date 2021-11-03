using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotfixFrameWork
{ 
    public class GameConfig : MonoBehaviour
    {
        /// <summary>
        /// version || md5file 下载失败重试次数
        /// </summary>
        public const int DOWNLOAD_FAIL_COUNT = 3;
        /// <summary>
        /// version || md5file 下载失败延迟重试时间 秒
        /// </summary>
        public const int DOWNLOAD_FAIL_RETRY_DELAY = 2;

    }
}


