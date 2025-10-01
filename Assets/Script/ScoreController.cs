using Nananami.Actors;
using Nananami.Commands;
using Nananami.Helpers;
using UnityEngine;

namespace Nananami
{
    public class ScoreController : Actor
    {
        [SerializeField] private float m_increase_rate;
        protected override void m_update()
        {
            base.m_update();
            var instance = GameMain.Instance;
            if (instance == null) return;
            const string messageName = "ScoreControllerMessage";
            string message;
            while (instance.messageTray.TryQuery(messageName, out message))
            {
                if (message == "ScoreAddtionIncrease")
                {
                    int increaseValue = (int)((float)CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameScoreAddition") * (m_increase_rate - 1.0f));
                    instance.globalScheduler.EnqueueCommand(new AddVariable<int>("gameScoreAddition", increaseValue));
                }
            }
        }
    }
}