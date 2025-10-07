using Nananami.Commands;
using UnityEngine;

namespace Nananami
{
    class LevelUpUIController : MonoBehaviour
    {
        [SerializeField] private Canvas m_canvas;
        void Awake()
        {
            m_canvas.enabled = false;
        }

        void Update()
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            while (instance.messageTray.TryQuery("LevelUpUI", out string messageState))
            {
                if (messageState == "Enable")
                {
                    instance.globalScheduler.EnqueueCommand(new SetInternalVariable<bool>("pauseActors", true));
                    m_canvas.enabled = true;
                }
            }
        }

        public void OnQuitClicked()
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            instance.globalScheduler.EnqueueCommand(new SetInternalVariable<bool>("pauseActors", false));
            m_canvas.enabled = false;
        }
    }
}