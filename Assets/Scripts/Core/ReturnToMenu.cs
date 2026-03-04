using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoardAgain.Core
{
    public class ReturnToMenu : MonoBehaviour
    {
        public void ExitToTitleScreen()
        {
            MapManager.currentNodeIndex = 0;
            SceneManager.LoadScene("TitleScreen");
            GameManager.Instance.ResetGame();
        }
    }
}