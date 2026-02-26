using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class NodeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nodeText;
    [SerializeField] private Image nodeIcon;
    private Button button;
    public GameObject activeIndicator;

    public static bool isBossNext = false;

    public void Setup(NodeType type, bool canClick)
    {
        button = GetComponent<Button>();
        nodeText.text = type.ToString();
        button.interactable = canClick;

        if (activeIndicator != null)
        {
            activeIndicator.SetActive(canClick);
        }

            if (nodeIcon != null)
            {
                setColor(type);

                if (!canClick)
                {
                    Color tempColor = nodeIcon.color;
                    tempColor.a = 0.3f;
                    nodeIcon.color = tempColor;
                }
            }
        
    }

    private void setColor(NodeType type)
    {
        switch (type)
        {
            case NodeType.Enemy: nodeIcon.color = Color.red; break;
            case NodeType.Rest: nodeIcon.color = Color.green; break;
            case NodeType.Boss: nodeIcon.color = Color.gray; break;
        }
    }

    private void Update()
    {
        if (button != null && button.interactable)
        {
            float scale = 1f + Mathf.Sin(Time.time * 5f) * 0.1f;
            transform.localScale = new Vector3(scale, scale, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }


    public void OnNodeClicked()
    {
        Debug.Log("Node clicked: " + nodeText.text);

        if(nodeText.text == "Enemy")
        {
            Debug.Log("Loading Enemy Encounter...");
            isBossNext = false;
            SceneManager.LoadScene("CombatScreen");
        }
        else if (nodeText.text == "Rest")
        {
            MapManager.currentNodeIndex++;
            Debug.Log("Loading Rest Area...");
            SceneManager.LoadScene("MapScreen");

            //TODO: Implement healing? restScreen? maybe just a pop up that heals the player and then returns to the map?
            //SceneManager.LoadScene("RestScreen");
        }
        else if (nodeText.text == "Boss")
        {
            Debug.Log("Loading Boss Fight...");
            isBossNext = true;
            SceneManager.LoadScene("CombatScreen");
        }
    }
}