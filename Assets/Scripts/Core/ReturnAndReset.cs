using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoardAgain.Core
{
    public class ReturnAndReset : MonoBehaviour
    {
        public void ResetRunAndReturn()
        {
            MapManager.currentNodeIndex = 0;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ResetGame();
            }

            SceneManager.LoadScene("TitleScreen");
        }
    }
}