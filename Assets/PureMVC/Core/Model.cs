using System;
using System.Collections.Concurrent;
using PureMVC.Interfaces;



namespace PureMVC.Core
{
    public class Model : IModel
    {

        protected readonly ConcurrentDictionary<string, IProxy> m_ProxyMap;

        protected static IModel m_Instance;

        protected const string SINGLETON_MSG = "Model Singleton already constructed!";
        public Model()
        {
            if (m_Instance != null) throw new Exception(SINGLETON_MSG);
            m_Instance = this;
            m_ProxyMap = new ConcurrentDictionary<string, IProxy>();
            InitializeModel();
        }

        public static IModel GetInstance(Func<IModel> modelFunc)
        {
            if (m_Instance == null)
            {
                m_Instance = modelFunc();
            }
            return m_Instance;
        }

        public virtual void RegisterProxy(IProxy proxy)
        {
            m_ProxyMap[proxy.ProxyName] = proxy;
            proxy.OnRegister();
        }

        public virtual IProxy RetrieveProxy(string proxyName)
        {
            return m_ProxyMap.TryGetValue(proxyName, out IProxy proxy) ? proxy : null;
        }

        public virtual IProxy RemoveProxy(string proxyName)
        {
            if (m_ProxyMap.TryRemove(proxyName, out IProxy proxy))
            {
                proxy.OnRemove();
            }
            return proxy;
        }

        public virtual bool HasProxy(string proxyName)
        {
            return m_ProxyMap.ContainsKey(proxyName);
        }
        protected virtual void InitializeModel()
        {

        }
    }
}
