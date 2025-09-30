using System;
using Nananami.Actors;
using Nananami.Commands;
using Nananami.Helpers;
using UnityEngine;

namespace Nananami
{
    public class LevelController : Actor
    {
        [SerializeField] private int m_level_up_exp;
        [SerializeField] private float m_level_up_exp_increase_rate;
        protected override void m_update()
        {
            base.m_update();
            var instance = GameMain.Instance;
            if (instance == null) return;
            try
            {
                int gameExp = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameExp");
                if (m_level_up_exp <= gameExp)
                {
                    instance.globalScheduler.EnqueueCommand(new AddVariable<int>("gameLevel", 1));
                    instance.globalScheduler.EnqueueCommand(new AddVariable<int>("gameExp", -m_level_up_exp));
                    m_level_up_exp = (int)(m_level_up_exp * m_level_up_exp_increase_rate);
                    instance.messageTray.Post("ScoreControllerMessage", "ScoreAddtionIncrease");
                }
            }
            catch (Exception)
            {
                Debug.LogWarning("Failed to get any status.");
            }
        }
    }
}