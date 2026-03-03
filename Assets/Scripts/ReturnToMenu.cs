using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    [Header("Run Reset Settings")]
    public CharacterData playerAsset;
    public void ExitToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
