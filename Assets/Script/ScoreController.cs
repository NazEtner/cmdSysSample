using Nananami.Actors;
using Nananami.Commands;
using Nananami.Helpers;
using UnityEngine;

namespace Nananami
{
    public class ScoreController : Actor
    {
        [SerializeField] private float m_increase_rate;

        void Start()
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            instance.globalScheduler.EnqueueCommand(new SetVariable<int>("gameScore", 0));
            instance.globalScheduler.EnqueueCommand(new SetVariable<int>("gameScoreAddition", 100));
        }

        protected override void m_update()
        {
            base.m_update();
            var instance = GameMain.Instance;
            if (instance == null) return;
            while (instance.messageTray.TryQuery("ScoreControllerMessage", out string message))
            {
                if (message == "Grazed")
                {
                    int gameScoreAddition = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameScoreAddition");
                    instance.globalScheduler.EnqueueCommand(new AddVariable<int>("gameScore", gameScoreAddition));
                }
                if (message == "ScoreAddtionIncrease")
                {
                    int increaseValue = (int)(CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameScoreAddition") * (m_increase_rate - 1.0f));
                    instance.globalScheduler.EnqueueCommand(new AddVariable<int>("gameScoreAddition", increaseValue));
                }
            }
        }
    }
}