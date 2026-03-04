using UnityEngine;
using BoardAgain.Core;

public class Bootstrap : MonoBehaviour
{
    public GameObject gameManagerPrefab;

    private void Awake()
    {
        // Ensure GameManager exists in the scene
        if (GameManager.Instance == null)
        {
            Instantiate(gameManagerPrefab);
        }
    }
}
