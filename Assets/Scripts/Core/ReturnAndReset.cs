using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoardAgain.Core
{
    public class ReturnAndReset : MonoBehaviour
    {
        public void ResetRunAndReturn()
        {

            MapManager.currentNodeIndex = 0;

            SceneManager.LoadScene("TitleScreen");

            GameManager.Instance.ResetGame();
        }
    }
}