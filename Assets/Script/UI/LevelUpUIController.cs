using System.Collections.Generic;
using Nananami.Commands;
using Nananami.Rougelite;
using TMPro;
using UnityEngine;

namespace Nananami
{
    class LevelUpUIController : MonoBehaviour
    {
        [SerializeField] private Canvas m_canvas;
        [SerializeField] private List<Choise> m_choises;
        private LotteryMachine m_lottery;
        private List<int> m_choise_indexes = new List<int>();
        void Awake()
        {
            m_canvas.enabled = false;
        }

        void Update()
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            while (instance.messageTray.TryQuery("LevelUpUI", out string messageState))
            {
                if (messageState == "Enable")
                {
                    if (m_lottery == null)
                    {
                        m_lottery = new LotteryMachine("Data/Choises.json");
                    }
                    instance.globalScheduler.EnqueueCommand(new SetInternalVariable<bool>("pauseActors", true));
                    m_canvas.enabled = true;
                    ChangeChoisesText();
                }
            }
        }

        void ChangeChoisesText()
        {
            m_choise_indexes.Clear();
            foreach (var choise in m_choises)
            {
                choise.Clear();
                if (m_lottery.TryDrawLottery(out string text, out int index))
                {
                    m_choise_indexes.Add(index);
                    choise.SetText(text);
                    choise.index = index;
                }
            }
        }

        public void OnQuitClicked()
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            instance.globalScheduler.EnqueueCommand(new SetInternalVariable<bool>("pauseActors", false));
            m_canvas.enabled = false;
        }
    }
}