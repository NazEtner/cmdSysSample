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

        void Start()
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            instance.globalScheduler.EnqueueCommand(new SetVariable<int>("gameLevel", 1));
            instance.globalScheduler.EnqueueCommand(new SetVariable<int>("gameExp", 0));
            instance.globalScheduler.EnqueueCommand(new SetVariable<int>("gameExpAddition", 10));
        }
        protected override void m_update()
        {
            base.m_update();
            var instance = GameMain.Instance;
            if (instance == null) return;
            try
            {
                while (instance.messageTray.TryQuery("LevelControllerMessage", out string messageState))
                {
                    if (messageState == "Grazed")
                    {
                        int gameExpAddition = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameExpAddition");
                        int gameExp = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameExp") + gameExpAddition;
                        instance.globalScheduler.SetVariableImmediate("gameExp", gameExp);
                        if (m_level_up_exp <= gameExp)
                        {
                            instance.globalScheduler.EnqueueCommand(new AddVariable<int>("gameExp", -m_level_up_exp));
                            instance.messageTray.Post("LevelControllerMessage", "LevelUp");
                            break;
                        }
                    }
                    if (messageState == "LevelUp")
                    {
                        instance.globalScheduler.EnqueueCommand(new AddVariable<int>("gameLevel", 1));
                        m_level_up_exp = (int)(m_level_up_exp * m_level_up_exp_increase_rate);
                        instance.messageTray.Post("ScoreControllerMessage", "ScoreAddtionIncrease");
                        instance.messageTray.Post("LevelUpUI", "Enable");
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