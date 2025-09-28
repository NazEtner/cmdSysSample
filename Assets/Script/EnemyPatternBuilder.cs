using System.Collections.Generic;
using UnityEngine;

namespace Nananami {
    public class EnemyPatternBuilder
    {
        public void Set(string name, List<string> candidates)
        {
            m_candidates[name] = candidates;
        }

        public List<string> Build(string start, string end)
        {
            var list = new List<string>();
            Build(0, list, start, end);
            return list;
        }

        public void Build(uint recursion, List<string> list, string start, string end)
        {
            const uint recursionLimit = 50;
            ++recursion;
            list.Add(start);
            if (start == end) return;
            if (m_candidates.ContainsKey(start) && recursion < recursionLimit)
            {
                var next = m_candidates[start][Random.Range(0, m_candidates[start].Count)];
                Build(recursion, list, next, end);
                return;
            }

            Build(recursion, list, end, end);
        }

        private Dictionary<string, List<string>> m_candidates = new Dictionary<string, List<string>>();
    }
}