using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatPopUp : MonoBehaviour
{
    public float animationSpeed = 0.1f;

    void OnEnable()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(PopIn());
    }

    IEnumerator PopIn()
    {
        float timer = 0;
        while (timer <= 1)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer);
            timer += Time.unscaledDeltaTime * (1 / animationSpeed);
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

    public void BoardAgain()
    {
        Time.timeScale = 1f;

        MapManager.currentNodeIndex = 0;
        SceneManager.LoadScene("TitleScreen");
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen");
    }
}
