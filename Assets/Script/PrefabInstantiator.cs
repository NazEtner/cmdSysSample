using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Nananami
{
    public class PrefabInstantiator
    {
        public void Destruct()
        {
            foreach (var resource in m_cache)
            {
                resource.Value.Item1.Release();
            }

            m_cache.Clear();
        }

        public GameObject InstantiatePrefab(string path)
        {
            return UnityEngine.Object.Instantiate(m_loadPrefab(path));
        }

        public T InstantiatePrefab<T>(string path) where T : Component
        {
            var gameObject = InstantiatePrefab(path);
            return gameObject.GetComponent<T>();
        }

        private GameObject m_loadPrefab(string path)
        {
            if (m_cache.ContainsKey(path))
            {
                return m_cache[path].Item2;
            }

            var handle = Addressables.LoadAssetAsync<GameObject>(path);
            // メインスレッドをブロックするから重いオブジェクトは別のメソッドを作る方が吉
            var res = handle.WaitForCompletion();
            if (res == null)
            {
                Debug.LogError($"Prefab not found at path: {path}");
                return null;
            }
            m_cache[path] = (handle, res);
            return res;
        }

        private Dictionary<string, (AsyncOperationHandle<GameObject>, GameObject)> m_cache
            = new Dictionary<string, (AsyncOperationHandle<GameObject>, GameObject)>();
    }
}