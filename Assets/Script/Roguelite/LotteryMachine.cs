using System;
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
            m_choise_data = JsonUtility.FromJson<ChoiceData>(jsonString);
        }

        public void UpdateChoiseState()
        {
            m_candidates.Clear();
            var instance = GameMain.Instance;
            if (instance == null) return;
            int level = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameLevel");
            for (int i = 0; i < m_choise_data.choices.Count; i++)
            {
                var choise = m_choise_data.choices[i];
                if (choise.levelRequired <= level && choise.maxDuplicate > 0)
                {
                    m_candidates.Add(i);
                }
            }
        }

        public bool TryDrawLottery(out string text, out int choiseIndex, out bool payable)
        {
            text = "";
            choiseIndex = -1;
            payable = false;
            if (m_candidates.Count == 0) return false;
            var index = m_candidates.ElementAt(UnityEngine.Random.Range(0, m_candidates.Count));
            m_candidates.Remove(index);
            var instance = GameMain.Instance;
            int gameMoney = 0;
            if (instance != null)
            {
                gameMoney = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameMoney");
            }
            text = $"{m_choise_data.choices[index].text}(必要資金 : {m_choise_data.choices[index].price})";
            choiseIndex = index;
            payable = gameMoney >= m_choise_data.choices[index].price;

            return true;
        }

        public void Choise(int index)
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            foreach (var message in m_choise_data.choices[index].messages)
            {
                instance.messageTray.Post(message.name, message.contents);
            }
            int gameMoney = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameMoney");
            int payed = gameMoney - m_choise_data.choices[index].price;
            instance.globalScheduler.SetVariableImmediate("gameMoney", payed);
            try
            {
                checked
                {
                    m_choise_data.choices[index].price = (int)(m_choise_data.choices[index].price * m_choise_data.choices[index].priceIncreaseRate);
                }
            }
            catch(OverflowException)
            {
                m_choise_data.choices[index].price = int.MaxValue;
            }
            m_choise_data.choices[index].maxDuplicate--;
        }

        private ChoiceData m_choise_data;
        private HashSet<int> m_candidates = new HashSet<int>();
    }
}