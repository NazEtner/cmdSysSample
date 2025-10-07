namespace Nananami.Actors.Bullets
{
    public class DeletionChain : AutoMoveCollisionActor
    {
        void Awake()
        {
            var instance = GameMain.Instance;
            float radius = 0.0f;
            if (instance != null)
            {
                radius = instance.globalBulletStatus.deletionChainRadius;
            }
            CollisionInitialize(radius, "Player");
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