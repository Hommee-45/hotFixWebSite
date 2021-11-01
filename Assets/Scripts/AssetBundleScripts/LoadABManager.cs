using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadABManager
{
    //用来存储已经加载的AB包， 键是AB包的名字， 值就是AB包
    public Dictionary<string, AssetBundle> m_ABDict;
    //用来存储单一的ab包
    public AssetBundle m_SingleAB;
    //单一的构建清单， 所有的ab包依赖全部从这获取
    public AssetBundleManifest m_SingleManifest;
    //存储ab包的路径
    public string m_ABPath;
    //单一的ab包的名字
    public string m_SingleABName;


    #region 单例
    public static LoadABManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new LoadABManager();
            }
            return m_Instance;
        }
    }
    private static LoadABManager m_Instance;

    private LoadABManager()
    {
        m_ABDict = new Dictionary<string, AssetBundle>();
        m_ABPath = HotfixFrameWork.GamePathConfig.LOCAL_ASSETBUNDLES_PATH;
        m_SingleABName = "AssetBundles";
    }
    #endregion


    /// <summary>
    /// 加载指定的ab包， 并且返回该AB包
    /// </summary>
    /// <param name="abName"></param>
    /// <returns></returns>
    public AssetBundle LoadAssetBundle(string abName)
    {
        if (abName == null)
            return null;

        LoadAllDependencies(abName);
        //加载指定的ab包
        //加载前先判断是否已经加载过， 如果加载过，把加载过的ab包返回
        //如果未加载过，就加载该ab包
        AssetBundle ab = null;
        if (!m_ABDict.TryGetValue(abName, out ab))
        {//如果没有这个包名
            //重新从文件夹加载
            ab = AssetBundle.LoadFromFile(m_ABPath + abName);
            //把加载进来的ab包添加到字典中
            m_ABDict[abName] = ab;
        }
        return ab;
    }

    /// <summary>
    /// 加载指定的ab包中的指定名字的指定类型的资源
    /// </summary>
    /// <typeparam name="T">指定的类型</typeparam>
    /// <param name="abName">ab包名</param>
    /// <param name="assetName">资源名字</param>
    /// <returns></returns>
    public T LoadAssetByAB<T>(string abName, string assetName) where T : Object
    {
        //先获取指定的ab包
        AssetBundle ab = LoadAssetBundle(abName);
        if (ab != null)
        {
            return ab.LoadAsset<T>(assetName);
        }
        else
        {
            Debug.LogError("指定的ab包名字有误");
        }
        return null;
    }

    /// <summary>
    /// 卸载指定的ab包
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="unloadAllloadedObjects"></param>
    public void UnloadAssetBundle(string abName, bool isUnloadAllloadedObjects)
    {
        AssetBundle ab = null;
        if (m_ABDict.TryGetValue(abName, out ab))
        {
            //卸载ab包
            ab.Unload(isUnloadAllloadedObjects);
            //从容器中删除该ab包
            m_ABDict.Remove(abName);
        }
    }

    public void UnloadAllAssetBundle(bool isUnloadAllloadedObjects)
    {
        //遍历每一个ab包，调用ab包的卸载方法
        //遍历键， 通过键去获取值进行卸载
        //foreach (var item in m_ABDict.Keys)
        //{
        //    m_ABDict[item].Unload(isUnloadAllloadedObjects);
        //}
        //直接遍历值去卸载
        foreach (var item in m_ABDict.Values)
        {
            item.Unload(isUnloadAllloadedObjects);
        }
        m_ABDict.Clear();
    }


    /// <summary>
    /// 加载单一的ab包， 和单一的构建清单
    /// </summary>
    private void LoadSingleAssetBundle()
    {
        //每次加载单一的ab包需要判断是否加载过
        //m_SingAB为NULL即没加载过
        if (m_SingleAB == null)
        {
            m_SingleAB = AssetBundle.LoadFromFile(m_ABPath + m_SingleABName);
        }
        //从单一的ab包中加载单一的构建清单
        if (m_SingleManifest == null)
        {
            m_SingleManifest = m_SingleAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
    }

    /// <summary>
    /// 加载指定ab包的所有依赖项
    /// </summary>
    /// <param name="abName"></param>
    private void LoadAllDependencies(string abName)
    {
        LoadSingleAssetBundle();
        //首先获取指定的这个ab包的所有依赖项
        //从单一的构建清单中获取
        string[] deps = m_SingleManifest.GetDirectDependencies(abName);
        //遍历去加载依赖项
        for (int i = 0; i < deps.Length; i++)
        {
            //加载依赖项前，先判断之前有没有加载过
            //就是判断存储ab包字典中有没有这个ab包
            if (!m_ABDict.ContainsKey(deps[i]))
            {
                //如果未加载过， 需要加载
                AssetBundle ab = AssetBundle.LoadFromFile(m_ABPath + deps[i]);
                //ab包加载完之后，把加载来的ab包存储在字典里
                m_ABDict[deps[i]] = ab;
            }
        }
    }


    
    



}
