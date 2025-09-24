using Nananami.Lib.CmdSys;
using UnityEngine;

namespace Nananami.Actors
{
    public abstract class Actor : MonoBehaviour
    {
        void Update()
        {
            var instance = GameMain.Instance;
            if (instance != null)
            {
                bool shouldUpdate = true;
                var pauseActors = instance.globalScheduler.GetInternalVariable("pauseActors");
                pauseActors.Match(
                    intCase: null,
                    stringCase: null,
                    floatCase: null,
                    boolCase: b => shouldUpdate = !b
                );

                if (!shouldUpdate) return;
                m_update();
                scheduler.Execute();
                m_updateAfterCommandExecution();
            }
        }

        protected virtual void m_update() { }
        protected virtual void m_updateAfterCommandExecution(){ }
        public CommandScheduler scheduler = new CommandScheduler();
    }
}