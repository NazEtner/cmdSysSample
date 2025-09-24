using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Nananami.Commands;

namespace Nananami.Actors
{
    public class Player : AutoMoveActor
    {
        [SerializeField] private float m_speed;
        [SerializeField] private float m_speed_on_slowmode;
        private float m_current_speed = 0.0f;

        [SerializeField] private PlayerInput m_player_input;
        private InputAction m_slow_action;

        private Vector2 m_move_input;

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

            m_current_speed = m_speed;
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

                float eightDirAngle = Mathf.Round(rawAngle / (Mathf.PI / 4)) * (Mathf.PI / 4);

                m_scheduler.EnqueueCommand(new SetVariable<float>("angle", eightDirAngle));
                m_scheduler.EnqueueCommand(new SetVariable<float>("speed", m_current_speed));
            }
            else
            {
                m_scheduler.EnqueueCommand(new SetVariable<float>("speed", 0f));
            }
        }
    }
}