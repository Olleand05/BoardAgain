using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoardAgain.Core
{
    public class StartGame : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartGameButton()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MapScreen");
        }
    }
}