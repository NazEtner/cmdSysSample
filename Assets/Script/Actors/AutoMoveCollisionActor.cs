using System;
using Nananami.Collision;
using Nananami.Commands;
using Unity.VisualScripting;

namespace Nananami.Actors
{
    public class AutoMoveCollisionActor : AutoMoveActor
    {
        public void CollisionInitialize(float radius, string groupName)
        {
            m_scheduler.EnqueueCommand(new SetVariable<float>("radius", radius));
            m_group_name = groupName;

            m_scheduler.Execute();

            m_is_initialized = true;
        }

        public virtual void OnCollision(string groupName)
        {

        }

        protected override void m_updateAfterCommandExecution()
        {
            base.m_updateAfterCommandExecution();
            if (!m_is_initialized)
            {
                throw new InvalidOperationException($"This auto move collision actor is not initialized.");
            }

            CollisionData collisionData = new CollisionData
            {
                x = transform.position.x,
                y = transform.position.y,
                radius = m_getVariable<float>("radius"),
                collisionActor = this,
            };

            var instance = GameMain.Instance;
            if (instance != null)
            {
                instance.simpleCollider.RegisterCollisionData(m_group_name, collisionData);
            }
        }

        private bool m_is_initialized = false;
        private string m_group_name;
    }
}