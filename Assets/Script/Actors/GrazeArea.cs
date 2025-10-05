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

                // TODO: MoneyManagerを実装し、messageTrayを使う実装にする
                int gameMoneyAddition = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameMoneyAddition");
                instance.globalScheduler.EnqueueCommand(new AddVariable<int>("gameMoney", gameMoneyAddition));

                instance.messageTray.Post("ScoreControllerMessage", "Grazed");
                instance.messageTray.Post("LevelControllerMessage", "Grazed");
                
                actor.scheduler.SetVariableImmediate("grazed", true);
            }
        }
    }
}