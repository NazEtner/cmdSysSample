using System;
using Nananami.Actors;
using Nananami.Commands;
using Nananami.Helpers;
using UnityEngine;

namespace Nananami
{
    public class MoneyController : Actor
    {
        [SerializeField] private float m_money_addition_increase_rate;
        [SerializeField] private float m_money_addition_increase_rate_decrease_rate;
        private float m_money_addition = 1.0f;

        void Start()
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            instance.globalScheduler.EnqueueCommand(new SetVariable<int>("gameMoney", 0));
        }
        protected override void m_update()
        {
            base.m_update();
            var instance = GameMain.Instance;
            if (instance == null) return;
            try
            {
                while (instance.messageTray.TryQuery("MoneyControllerMessage", out string messageState))
                {
                    if (messageState == "Grazed")
                    {
                        instance.globalScheduler.EnqueueCommand(new AddVariable<int>("gameMoney", (int)Mathf.Ceil(m_money_addition)));
                    }
                    if (messageState == "MoneyAdditionIncrease")
                    {
                        m_money_addition *= m_money_addition_increase_rate;
                    }
                    if (messageState == "DecreaseMoneyAdditionIncreaseRate")
                    {
                        m_money_addition_increase_rate *= m_money_addition_increase_rate_decrease_rate;
                    }
                }
            }
            catch (Exception)
            {
                Debug.LogWarning("Failed to get any status.");
            }
        }
    }
}