namespace Nananami.Actors.Bullets
{
    public class GlobalBulletStatus
    {
        public float grazedBulletDeleteScoreProbability { get; private set; } = 0.0f;
        public float deletionChainRadius = 0.0f;
        private float m_grazed_bullet_delete_score_probability_increase_value = 0.009f;
        private float m_deletion_chain_radius_expand = 0.30f;
        public void HandleMessages()
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            while (instance.messageTray.TryQuery("BulletEffect", out string messageState))
            {
                if (messageState == "IncreaseGrazedBulletDeleteScoreProbability")
                {
                    grazedBulletDeleteScoreProbability += m_grazed_bullet_delete_score_probability_increase_value;
                }
                if (messageState == "ExpandChainDeletionRadius")
                {
                    deletionChainRadius += m_deletion_chain_radius_expand;
                }
            }
        }
    }
}