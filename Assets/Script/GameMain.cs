using Nananami.CommandPatterns;
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

        void OnEnable()
        {
            Instance = this;
            globalScheduler.EnqueueCommand(new SetInternalVariable<bool>("pauseActors", false));
            globalScheduler.Execute();
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this)
            {
                globalScheduler.Execute();
            }
        }
    }
}
