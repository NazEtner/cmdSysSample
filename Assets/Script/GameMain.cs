using System.Collections.Generic;
using Mono.Cecil;
using Nananami.Actors;
using Nananami.Collision;
using Nananami.CommandPatterns;
using Nananami.CommandPatterns.TableInitializers;
using Nananami.Commands;
using Nananami.Lib.CmdSys;
using UnityEngine;

namespace Nananami
{
    // 神クラスっぽくなってしまいそうですが、他はまともなコードを書いてるのでご容赦を....
    public class GameMain : MonoBehaviour
    {
        public static GameMain Instance { get; private set; }
        public CommandScheduler globalScheduler = new CommandScheduler();
        public CommandPatternTable commandPatternTable = new CommandPatternTable();
        public SimpleCollider simpleCollider = new SimpleCollider();
        public PrefabInstantiator prefabInstantiator = new PrefabInstantiator();

        void OnEnable()
        {
            Instance = this;

            globalScheduler.EnqueueCommand(new SetInternalVariable<bool>("pauseActors", false));
            globalScheduler.Execute();

            new CommonInitializer().Initialize(commandPatternTable);

            var tInit = new AutoMoveActorInitializationParameter
            {
                x = 0,
                y = 0,
                angle = -Mathf.PI / 2,
                speed = 0.1f,
                deletionResistance = 100,
            };
            globalScheduler.EnqueueCommand(new CreateAutoMoveActor("Prefabs/Enemy", tInit, new List<string>() { "LongDeceleration", "GoRight" }));
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this)
            {
                simpleCollider.DetectCollision();
                globalScheduler.Execute();
            }
        }
    }
}
