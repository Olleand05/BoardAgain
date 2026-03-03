using UnityEngine;
using UnityEngine.SceneManagement;


public class ReturnAndReset : MonoBehaviour
{
    [Header("Run Reset Settings")]
    public CharacterData playerAsset;


    public void ResetRunAndReturn()
    {
        if (playerAsset != null)
        {
            playerAsset.bonusAbility = null;
            Debug.Log("Run data cleared: Bonus Ability reset to null.");
        }
        else
        {
            Debug.LogWarning("ReturnToMenu: No playerAsset assigned! Data was not cleared.");
        }

        MapManager.currentNodeIndex = 0;

        SceneManager.LoadScene("TitleScreen");
    }
}