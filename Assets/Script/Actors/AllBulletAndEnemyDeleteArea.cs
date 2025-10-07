namespace Nananami.Actors
{
    public class AllBulletAndEnemyDeleteArea : AutoMoveCollisionActor
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
            CollisionInitialize(15.0f, "Player");
        }

        public override void OnCollision(string groupName, AutoMoveCollisionActor actor)
        {
            if (groupName == "EnemyOrBullet")
            {
                ((OffScreenAutoDeletable)actor).Damage(100);
            }
        }

        protected override void m_updateAfterCommandExecution()
        {
            base.m_updateAfterCommandExecution();
            if (m_updated)
            {
                Destroy(gameObject);
            }

            m_updated = true;
        }

        private bool m_updated = false;
    }
}