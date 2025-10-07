using UnityEngine;
using Nananami.Helpers;
using Nananami.Commands;

namespace Nananami.Actors
{
    public class GrazeArea : AutoMoveCollisionActor
    {
        private float m_bullet_deletion_probability = 0.0f;
        private float m_all_bullet_deletion_probability = 0.0f;
        private float m_graze_probability = 1.0f;
        [SerializeField] private float m_bullet_deletion_probability_increase_value = 0.08f;
        [SerializeField] private float m_all_bullet_deletion_probability_increase_value = 0.008f;
        [SerializeField] private float m_graze_ignore_probability_increase_value = 0.038f;
        [SerializeField] private float m_radius_expand_rate = 1.09f;

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

        protected override void m_updateAfterCommandExecution()
        {
            base.m_updateAfterCommandExecution();
            m_handleMessages();
        }

        public override void OnCollision(string groupName, AutoMoveCollisionActor actor)
        {
            if (groupName == "EnemyOrBullet")
            {
                var instance = GameMain.Instance;
                if (instance == null) return;
                bool isGrazed = CommandVariableHelper.GetVariable<bool>(actor.scheduler, "grazed");
                if (isGrazed) return;

                instance.messageTray.Post("ScoreControllerMessage", "Grazed");
                instance.messageTray.Post("LevelControllerMessage", "Grazed");
                instance.messageTray.Post("MoneyControllerMessage", "Grazed");

                if (m_graze_probability >= Random.Range(0.0f, 1.0f))
                {
                    actor.scheduler.SetVariableImmediate("grazed", true);
                }

                if (m_bullet_deletion_probability >= Random.Range(0.0f, 1.0f))
                {
                    ((OffScreenAutoDeletable)actor).Damage(1000);
                }

                if (m_all_bullet_deletion_probability >= Random.Range(0.0f, 1.0f))
                {
                    instance.prefabInstantiator.InstantiatePrefab("Prefabs/AllBulletAndEnemyDeleteArea");
                }
            }
        }

        private void m_handleMessages()
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            while (instance.messageTray.TryQuery("GrazeEffect", out string messageState))
            {
                if (messageState == "IncreaseBulletDeletionProbability")
                {
                    m_bullet_deletion_probability += m_bullet_deletion_probability_increase_value;
                }
                if (messageState == "ExpandGrazeRadius")
                {
                    float expanded = CommandVariableHelper.GetVariable<float>(scheduler, "radius") * m_radius_expand_rate;
                    scheduler.EnqueueCommand(new SetVariable<float>("radius", expanded));
                }
                if (messageState == "IncreaseGrazeIgnoreProbability")
                {
                    m_graze_probability -= m_graze_ignore_probability_increase_value;
                }
                if (messageState == "IncreaseAllBulletDeletionProbability")
                {
                    m_all_bullet_deletion_probability += m_all_bullet_deletion_probability_increase_value;
                }
            }
        }
    }
}