using UnityEngine;
using UnityEngine.SceneManagement;

namespace BoardAgain.Core
{
    public class QuitGame : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void QuitGameButton()
        {
            Application.Quit();
        }
    }
}

