using Nananami.Commands;
using UnityEngine;

namespace Nananami.Actors
{
    public class Enemy : OffScreenAutoDeletable
    {
        void Awake()
        {
            CollisionInitialize(0.20f, "EnemyOrBullet");

            scheduler.EnqueueCommand(new SetVariable<float>("offScreenDeleteRate", 30.0f));
            scheduler.EnqueueCommand(new SetVariable<bool>("grazed", true)); // 敵とはグレイズしない
            scheduler.Execute();
        }

        protected override void m_updateAfterCommandExecution()
        {
            base.m_updateAfterCommandExecution();
        }
    }
}