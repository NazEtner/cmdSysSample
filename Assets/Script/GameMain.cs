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

        void OnEnable()
        {
            Instance = this;
            globalScheduler.EnqueueCommand(new SetInternalVariable<bool>("pauseActors", false));
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
