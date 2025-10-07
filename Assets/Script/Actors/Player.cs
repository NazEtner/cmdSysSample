using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Nananami.Commands;

namespace Nananami.Actors
{
    public class Player : AutoMoveCollisionActor
    {
        [SerializeField] private float m_speed;
        [SerializeField] private float m_speed_on_slowmode;
        private float m_current_speed = 0.0f;

        [SerializeField] private PlayerInput m_player_input;
        private InputAction m_slow_action;

        private Vector2 m_move_input;
        private int m_hit_permit_count = 0;

        public override void OnCollision(string groupName, AutoMoveCollisionActor actor)
        {
            if (groupName == "EnemyOrBullet")
            {
                ((OffScreenAutoDeletable)actor).Damage(100);
                if (m_hit_permit_count <= 0)
                {
                    var instance = GameMain.Instance;
                    instance.messageTray.Post("GameOverControllerMessage", "GameOver");
                }
                else
                {
                    ((OffScreenAutoDeletable)actor).Damage(1000); // 無敵時間を実装するのは面倒なので、敵をほぼ確実に倒すようにする(合計1,100ダメージ)
                }
            }
        }

        public void OnMove(InputValue value)
        {
            m_move_input = value.Get<Vector2>();
        }

        void Awake()
        {
            if (m_player_input == null)
            {
                throw new InvalidOperationException("This actor has not player input.");
            }

            m_slow_action = m_player_input.actions.FindAction("Slow");
        }

        void Start()
        {
            var param = new AutoMoveActorInitializationParameter
            {
                x = 0.0f,
                y = -3.0f,
                angle = 0.0f,
                speed = 0.0f,
                rotateOffset = 0.0f,
                rotatable = false,
                deletionResistance = 1,
            };

            AutoMoveInitialize(param);

            CollisionInitialize(0.08f, "Player");

            m_current_speed = m_speed;

            var instance = GameMain.Instance;
            if (instance != null)
            {
                var globalScheduler = instance.globalScheduler;
            }
            else
            {
                Debug.LogError("GameMain instance is null.");
            }

            scheduler.Execute();
        }

        protected override void m_update()
        {
            base.m_update();

            if (m_slow_action == null)
            {
                throw new InvalidOperationException("Cannot to use slow action.");
            }

            if (m_slow_action.WasPressedThisFrame())
            {
                m_current_speed = m_speed_on_slowmode;
            }

            if (m_slow_action.WasReleasedThisFrame())
            {
                m_current_speed = m_speed;
            }

            if (m_move_input != Vector2.zero)
            {
                Vector2 direction = m_move_input.normalized;
                float rawAngle = Mathf.Atan2(direction.y, direction.x);

                // 角度を45度刻みに丸める
                float eightDirAngle = Mathf.Round(rawAngle / (Mathf.PI / 4)) * (Mathf.PI / 4);

                scheduler.EnqueueCommand(new SetVariable<float>("angle", eightDirAngle));
                scheduler.EnqueueCommand(new SetVariable<float>("speed", m_current_speed));
            }
            else
            {
                scheduler.EnqueueCommand(new SetVariable<float>("speed", 0f));
            }
        }

        protected override void m_updateAfterCommandExecution()
        {
            base.m_updateAfterCommandExecution();
            m_handleMessages();
            m_limitPosition();
        }

        private void m_handleMessages()
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            while (instance.messageTray.TryQuery("PlayerEffect", out string messageState))
            {
                if (messageState == "IncrementHitPermitCount")
                {
                    m_hit_permit_count++;
                }
            }
        }

        private void m_limitPosition()
        {
            const float leftLimit = -8.0f;
            const float rightLimit = 8.0f;
            const float upLimit = 4.5f;
            const float downLimit = -4.5f;

            var pos = transform.localPosition;
            if (pos.x < leftLimit) pos.x = leftLimit;
            else if (pos.x > rightLimit) pos.x = rightLimit;

            if (pos.y < downLimit) pos.y = downLimit;
            else if (pos.y > upLimit) pos.y = upLimit;

            transform.localPosition = pos;
        }
    }
}