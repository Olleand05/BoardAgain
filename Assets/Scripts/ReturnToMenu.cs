using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    public void ExitToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
