using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnAndReset : MonoBehaviour
{
    public void ResetRunAndReturn()
    {
        CharacterData playerAsset = Resources.Load<CharacterData>("PlayerStats");

        if (playerAsset != null)
        {
            playerAsset.bonusAbility = null;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(playerAsset);
#endif

            Debug.Log("Run data cleared: Bonus Ability reset via Resources.Load.");
        }
        else
        {
            Debug.LogError("ReturnAndReset: Could not find 'PlayerStats' in Resources folder!");
        }

        MapManager.currentNodeIndex = 0;

        SceneManager.LoadScene("TitleScreen");
    }
}