using System.Collections.Generic;
using UnityEngine;

namespace Nananami
{
    public class PrefabInstantiator
    {
        public GameObject InstantiatePrefab(string path)
        {
            return Object.Instantiate(m_loadPrefab(path));
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
                return m_cache[path];
            }

            var res = Resources.Load<GameObject>(path);
            if (res == null)
            {
                Debug.LogError($"Prefab not found at path: {path}");
                return null;
            }
            m_cache[path] = res;
            return res;
        }

        private Dictionary<string, GameObject> m_cache = new Dictionary<string, GameObject>();
    }
}