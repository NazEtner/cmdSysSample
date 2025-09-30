using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nananami
{
    class GameOverUIHandler : MonoBehaviour
    {
        public void OnClickRetry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}