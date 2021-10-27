using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotfixFrameWork
{
    public class Util
    {
        public static T GetInstance<T>(ref T instance, string name, bool isDontDestroy = true) where T : UnityEngine.Object
        {
            if (instance != null) return instance;
            if (GameObject.FindObjectOfType<T>() != null)
            {
                return instance;
            }
            GameObject go = new GameObject(name, typeof(T));
            if (isDontDestroy)
            {
                UnityEngine.Object.DontDestroyOnLoad(go);
            }
            instance = go.GetComponent(typeof(T)) as T;
            return instance;
        }
    }

}
