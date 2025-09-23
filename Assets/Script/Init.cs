using UnityEngine;

namespace Nananami
{
    public class Init : MonoBehaviour
    {
        void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}
