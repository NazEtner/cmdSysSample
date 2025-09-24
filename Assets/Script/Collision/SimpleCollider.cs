using System.Collections.Generic;

// Unityのコライダーは重いし使いづらいので、自前で実装する
namespace Nananami.Collision
{
    public struct CollisionData
    {
        public float x, y, radius;
        public Actors.AutoMoveCollisionActor collisionActor;
    }

    public class SimpleCollider
    {
        public void RegisterCollisionData(string groupName, CollisionData data)
        {
            if (!m_collision_groups.ContainsKey(groupName))
            {
                m_collision_groups[groupName] = new List<CollisionData>();
            }
            m_collision_groups[groupName].Add(data);
        }

        public void DetectCollision()
        {
            var keys = new List<string>(m_collision_groups.Keys);

            for (int i = 0; i < keys.Count; i++)
            {
                for (int j = i + 1; j < keys.Count; j++)
                {
                    var groupAName = keys[i];
                    var groupBName = keys[j];
                    var groupA = m_collision_groups[groupAName];
                    var groupB = m_collision_groups[groupBName];

                    for (int a = 0; a < groupA.Count; a++)
                    {
                        for (int b = 0; b < groupB.Count; b++)
                        {
                            if (m_isColliding(groupA[a], groupB[b]))
                            {
                                groupA[a].collisionActor?.OnCollision(groupBName, groupB[b].collisionActor);
                                groupB[b].collisionActor?.OnCollision(groupAName, groupA[a].collisionActor);
                            }
                        }
                    }
                }
            }

            foreach (var kv in m_collision_groups)
            {
                kv.Value.Clear();
            }
        }

        private bool m_isColliding(CollisionData a, CollisionData b)
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            float r = a.radius + b.radius;
            return dx * dx + dy * dy <= r * r;
        }

        private Dictionary<string, List<CollisionData>> m_collision_groups
            = new Dictionary<string, List<CollisionData>>();
    }
}