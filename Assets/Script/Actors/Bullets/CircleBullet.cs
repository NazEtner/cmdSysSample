using Nananami.Commands;

namespace Nananami.Actors.Bullets
{
    public class CircleBullet : OffScreenAutoDeletable
    {
        void Awake()
        {
            CollisionInitialize(0.03f, "EnemyOrBullet");

            scheduler.EnqueueCommand(new SetVariable<float>("offScreenDeleteRate", 30.0f));
            scheduler.EnqueueCommand(new SetVariable<bool>("grazed", false)); // 弾とはグレイズする
            scheduler.Execute();
        }
    }
}