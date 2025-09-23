using UnityEngine;

namespace Nananami
{
    public class Init : MonoBehaviour
    {
        void Awake()
        {
            // どうして垂直同期とフレームレート指定を両立できないんですかね
            // 本当にこの仕様は意味不明
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }
    }
}
