using UnityEngine;

namespace Nananami.CommandPatterns.TableInitializers
{
    public class CommonInitializer : InitializerBase
    {
        public override void Initialize(CommandPatternTable table)
        {
            table.AddCommandPattern("ShortDeceleration", new Deceleration(20));
            table.AddCommandPattern("MiddleDeceleration", new Deceleration(50));
            table.AddCommandPattern("LongDeceleration", new Deceleration(80));

            table.AddCommandPattern("ShortAcceleration", new Acceleration(20));
            table.AddCommandPattern("MiddleAcceleration", new Acceleration(50));
            table.AddCommandPattern("LongAcceleration", new Acceleration(80));

            table.AddCommandPattern("GoRight", new TurnAndAcceleration(Mathf.PI / 2, 2.0f, 120));
            table.AddCommandPattern("GoLeft", new TurnAndAcceleration(-Mathf.PI / 2, 2.0f, 120));

            for (uint i = 0; i <= 30; ++i)
            {
                table.AddCommandPattern($"Wait{i}", new Wait(i));
            }
        }
    }
}