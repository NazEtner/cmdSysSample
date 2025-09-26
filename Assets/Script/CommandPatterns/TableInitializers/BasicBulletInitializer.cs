using System.Collections.Generic;
using Nananami.CommandPatterns.Bullet;
using UnityEngine;

namespace Nananami.CommandPatterns.TableInitializers
{
    public class BasicBulletInitializer : InitializerBase
    {
        struct Difficulty
        {
            public string Name;
            public uint BaseWay;
            public float Speed;
            public float LevelRate;

            public Difficulty(string name, uint baseWay, float speed, float levelRate)
            {
                Name = name;
                BaseWay = baseWay;
                Speed = speed;
                LevelRate = levelRate;
            }
        }

        struct Range
        {
            public string Name;
            public float Value;

            public Range(string name, float value)
            {
                Name = name;
                Value = value;
            }
        }

        public override void Initialize(CommandPatternTable table)
        {
            var colors = new List<string> { "White", "Red", "Purple", "Blue", "Sky", "Gleen", "Yellow", "Orange" };
            var difficulties = new List<Difficulty>
            {
                new Difficulty("Easy", 4, 0.3f, 1.3f),
                new Difficulty("Normal", 6, 0.4f, 1.5f),
                new Difficulty("Hard", 8, 0.6f, 1.9f)
            };
            var ranges = new List<Range>
            {
                new Range("Narrow", Mathf.PI / 8),
                new Range("Middle", Mathf.PI / 6),
                new Range("Large", Mathf.PI / 4),
                new Range("Circle", Mathf.PI * 2)
            };

            for (int colorIndex = 0; colorIndex < colors.Count; colorIndex++)
            {
                string color = colors[colorIndex];
                foreach (var range in ranges)
                {
                    foreach (var difficulty in difficulties)
                    {
                        string patternName = $"{range.Name}{difficulty.Name}NWayToPlayer{color}";
                        table.AddCommandPattern(patternName,
                            new NWayToPlayer(difficulty.BaseWay, difficulty.LevelRate, false, range.Value, difficulty.Speed, (uint)colorIndex));
                    }
                }
            }
        }
    }
}