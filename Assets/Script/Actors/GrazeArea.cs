using Nananami.Commands;
using Nananami.Helpers;

namespace Nananami.Actors
{
    public class GrazeArea : AutoMoveCollisionActor
    {
        void Awake()
        {
            var param = new AutoMoveActorInitializationParameter
            {
                x = 0.0f,
                y = 0.0f,
                angle = 0.0f,
                speed = 0.0f,
                rotateOffset = 0.0f,
                rotatable = false,
                deletionResistance = 2147483647,
            };

            AutoMoveInitialize(param);

            CollisionInitialize(0.40f, "Player");
        }

        public override void OnCollision(string groupName, AutoMoveCollisionActor actor)
        {
            if (groupName == "EnemyOrBullet")
            {
                var instance = GameMain.Instance;
                if (instance == null) return;
                bool isGrazed = CommandVariableHelper.GetVariable<bool>(actor.scheduler, "grazed");
                if (isGrazed) return;

                //globalSchedulerに数フレーム使うコマンドはエンキューされない（ことが期待される）のでこれでいいはず
                int gameMoneyAddition = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameMoneyAddition");
                int gameExpAddition = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameExpAddition");
                int gameScoreAddition = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameScoreAddition");
                instance.globalScheduler.EnqueueCommand(new AddVariable<int>("gameMoney", gameMoneyAddition));
                instance.globalScheduler.EnqueueCommand(new AddVariable<int>("gameExp", gameExpAddition));
                instance.globalScheduler.EnqueueCommand(new AddVariable<int>("gameScore", gameScoreAddition));
                
                actor.scheduler.SetVariableImmediate<bool>("grazed", true);
            }
        }
    }
}