using Nananami.Commands;
using Nananami.Helpers;
using TMPro;
using UnityEngine;

namespace Nananami {
    public class GameOverController : MonoBehaviour
    {
        [SerializeField] private Canvas m_canvas;
        [SerializeField] private TextMeshProUGUI m_score_text;
        void Start()
        {
            m_canvas.enabled = false;
        }

        void Update()
        {
            var instance = GameMain.Instance;
            if (instance == null) return;
            const string messageName = "GameOverControllerMessage";
            string message;
            while (instance.messageTray.TryQuery(messageName, out message))
            {
                if (message == "GameOver")
                {
                    m_canvas.enabled = true;
                    instance.globalScheduler.EnqueueCommand(new SetInternalVariable<bool>("pauseActors", true));

                    int score = CommandVariableHelper.GetVariable<int>(instance.globalScheduler, "gameScore");
                    m_score_text.text = $"スコア : {score}";
                }
            }
        }
    }
}
