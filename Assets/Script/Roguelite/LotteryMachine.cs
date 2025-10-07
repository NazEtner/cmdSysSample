using System.Collections.Generic;
using System.Linq;
using Nananami.Helpers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Nananami.Rougelite
{
    public class LotteryMachine
    {
        public LotteryMachine(string path)
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(path);
            var asset = handle.WaitForCompletion();
            if (asset == null)
            {
                Debug.LogError($"Choises json file not found at path: {path}");
                return;
            }

            var jsonString = asset.ToString();
            m_choise_data = JsonUtility.FromJson<ChoiseData>(jsonString);

            UpdateChoiseState();
        }

        public void UpdateChoiseState()
        {
            m_candidates.Clear();
            var instance = GameMain.Instance;
            if (instance == null) return;
            int level = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameLevel");
            for (int i = 0; i < m_choise_data.choises.Count; i++)
            {
                var choise = m_choise_data.choises[i];
                if (choise.levelRequired <= level && choise.maxDuplicate > 0)
                {
                    m_candidates.Add(i);
                }
            }
        }

        public bool TryDrawLottery(out string text, out int choiseIndex)
        {
            text = "";
            choiseIndex = -1;
            if (m_candidates.Count == 0) return false;
            var index = m_candidates.ElementAt(Random.Range(0, m_candidates.Count));
            m_candidates.Remove(index);
            text = m_choise_data.choises[index].text;
            choiseIndex = index;
            return true;
        }

        public void Choise(int index)
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            foreach (var message in m_choise_data.choises[index].messages)
            {
                instance.messageTray.Post(message.name, message.contents);
            }

            m_choise_data.choises[index].maxDuplicate--;
        }

        private ChoiseData m_choise_data;
        private HashSet<int> m_candidates = new HashSet<int>();
    }
}