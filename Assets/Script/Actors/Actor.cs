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
                m_scheduler.Execute();
            }
        }

        protected abstract void m_update();
        protected CommandScheduler m_scheduler = new CommandScheduler();
    }
}