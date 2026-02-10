using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnTOMenu : MonoBehaviour
{
    public void ExitToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
