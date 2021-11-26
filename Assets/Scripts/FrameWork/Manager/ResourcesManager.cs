using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourcesManager : MonoBehaviour
{
    public Dictionary<string, UnityEngine.Object> m_ResourcesDict = new Dictionary<string, Object>();

    private static ResourcesManager m_Instance;
    public static ResourcesManager Instance
    {
        get { return HotfixFrameWork.Util.GetInstance(ref m_Instance, "_ResourcesMgr"); }
    }

    
    public GameObject LoadPrefab(string prefabName)
    {
        GameObject tempObj = null;
        if (!m_ResourcesDict.ContainsKey(prefabName))
        {
            m_ResourcesDict[prefabName] = Resources.Load<GameObject>("Prefab/" + prefabName);
        }
        tempObj = Instantiate(m_ResourcesDict[prefabName] as GameObject);
        tempObj.name = prefabName;
        return tempObj;
    }

    public Sprite LoadSprite(string spriteName)
    {
        Sprite tempSprite = null;
        if (!m_ResourcesDict.ContainsKey(spriteName))
        {
            m_ResourcesDict[spriteName] = Resources.Load<Sprite>("Sprite/" + spriteName);
        }
        tempSprite = m_ResourcesDict[spriteName] as Sprite;
        return tempSprite;
    }

    public AudioClip LoadAudioClip(string audioClipName)
    {
        AudioClip tempAudioClip = null;
        if (!m_ResourcesDict.ContainsKey(audioClipName))
        {
            m_ResourcesDict[audioClipName] = Resources.Load<AudioClip>("AudioClip/" + audioClipName);
        }
        tempAudioClip = m_ResourcesDict[audioClipName] as AudioClip;
        return tempAudioClip;
    }
}
