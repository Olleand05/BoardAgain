using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoardAgain.Core
{
    public class StartGame : MonoBehaviour
    {
        public void StartGameButton()
        {
            GameManager.Instance.ResetGame();
            GameManager.Instance.InitalizePlayer();

            Time.timeScale = 1f;
            SceneManager.LoadScene("MapScreen");
        }

    }
}