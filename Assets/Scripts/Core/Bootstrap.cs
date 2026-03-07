using UnityEngine;
using BoardAgain.Core;

public class Bootstrap : MonoBehaviour
{
    public GameObject gameManagerPrefab;

    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            Instantiate(gameManagerPrefab);
        }
    }
}
