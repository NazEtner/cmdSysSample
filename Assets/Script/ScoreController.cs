using Nananami.Actors;
using Nananami.Commands;
using Nananami.Helpers;

namespace Nananami
{
    public class ScoreController : Actor
    {
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
                    const float increaseRate = 1.7f;
                    int increaseValue = (int)((float)CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameScoreAddition") * (increaseRate - 1.0f));
                    instance.globalScheduler.EnqueueCommand(new AddVariable<int>("gameScoreAddition", increaseValue));
                }
            }
        }
    }
}