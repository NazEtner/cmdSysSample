using UnityEngine;
using Nananami.Commands;
using Nananami.Helpers;

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

        protected override void m_updateAfterCommandExecution()
        {
            base.m_updateAfterCommandExecution();
        }

        void OnDisable()
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            bool isGrazed = CommandVariableHelper.GetVariable<bool>(scheduler, "grazed");
            if (isGrazed)
            {
                if (instance.globalBulletStatus.grazedBulletDeleteScoreProbability <= Random.Range(0.0f, 1.0f))
                {
                    instance.messageTray.Post("ScoreControllerMessage", "BulletDeleted");
                }
            }

            if (m_damage != 0 && instance.globalBulletStatus.deletionChainRadius != 0.0f)
            {
                var deletionChain = instance.prefabInstantiator.InstantiatePrefab<DeletionChain>("Prefabs/DeletionChain");
                if (deletionChain != null)
                {
                    var param = new AutoMoveActorInitializationParameter
                    {
                        x = transform.position.x,
                        y = transform.position.y,
                        angle = 0.0f,
                        speed = 0.0f,
                        rotateOffset = 0.0f,
                        rotatable = false,
                        deletionResistance = 2147483647,
                    };
                    deletionChain.AutoMoveInitialize(param);
                }
            }
        }
    }
}