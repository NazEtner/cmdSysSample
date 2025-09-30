using System.Collections.Generic;
using Nananami.Actors;
using Nananami.Collision;
using Nananami.CommandPatterns;
using Nananami.CommandPatterns.TableInitializers;
using Nananami.Commands;
using Nananami.Lib.CmdSys;
using Nananami.Lib.Messaging;
using UnityEngine;

namespace Nananami
{
    // 神クラスっぽくなってしまいそうですが、他はまともなコードを書いてるのでご容赦を....
    public class GameMain : MonoBehaviour
    {
        public static GameMain Instance { get; private set; }
        public CommandScheduler globalScheduler { get; private set; } = new CommandScheduler();
        public CommandPatternTable commandPatternTable { get; private set; } = new CommandPatternTable();
        public MessageTray<string> messageTray { get; private set; } = new MessageTray<string>();
        public SimpleCollider simpleCollider { get; private set; } = new SimpleCollider();
        public PrefabInstantiator prefabInstantiator { get; private set; } = new PrefabInstantiator();

        void OnEnable()
        {
            Instance = this;

            globalScheduler.EnqueueCommand(new SetInternalVariable<bool>("pauseActors", false));
            globalScheduler.Execute();

            new CommonInitializer().Initialize(commandPatternTable);
            new BasicBulletInitializer().Initialize(commandPatternTable);
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
