using UnityEngine;

namespace Nananami
{
    public class Init : MonoBehaviour
    {
        void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }
}
