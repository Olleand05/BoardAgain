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

    [SerializeField] private Sprite enemyIsland;
    [SerializeField] private Sprite restIsland;
    [SerializeField] private Sprite bossIsland;

    public static bool isBossNext = false;

    private Vector3 baseScale;

    private NodeType currentNodeType;

    private void Awake()
    {
        baseScale = transform.localScale;
    }

    public void Setup(NodeType type, bool canClick)
    {
        currentNodeType = type;
        button = GetComponent<Button>();
        //nodeText.text = type.ToString();
        button.interactable = canClick;

        switch (type)
        {
            case NodeType.Enemy:
                baseScale = Vector3.one * 1.0f;
                if (nodeIcon != null)
                    nodeIcon.sprite = enemyIsland;
                break;

            case NodeType.Rest:
                baseScale = Vector3.one * 1.1f;
                if (nodeIcon != null)
                    nodeIcon.sprite = restIsland;
                break;

            case NodeType.Boss:
                baseScale = Vector3.one * 1.4f;
                if (nodeIcon != null)
                    nodeIcon.sprite = bossIsland;
                break;
        }
        transform.localScale = baseScale;

        if (activeIndicator != null)
        {
            activeIndicator.SetActive(canClick);
        }

            if (nodeIcon != null)
            {
                //setColor(type);

                if (!canClick)
                {
                    Color tempColor = nodeIcon.color;
                    tempColor.a = 1f;
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
            float pulse = 1f + Mathf.Sin(Time.time * 5f) * 0.1f;
            transform.localScale = baseScale*pulse;
        }
        else
        {
            transform.localScale = baseScale;
        }
    }


    public void OnNodeClicked()
    {

        if(currentNodeType==NodeType.Enemy)
        {
            Debug.Log("Loading Enemy Encounter...");
            isBossNext = false;
            SceneManager.LoadScene("CombatScreen");
        }
        else if (currentNodeType==NodeType.Rest)
        {
            MapManager.currentNodeIndex++;
            Debug.Log("Loading Rest Area...");
            SceneManager.LoadScene("MapScreen");

            //TODO: Implement healing? restScreen? maybe just a pop up that heals the player and then returns to the map?
            //SceneManager.LoadScene("RestScreen");
        }
        else if (currentNodeType==NodeType.Boss)
        {
            Debug.Log("Loading Boss Fight...");
            isBossNext = true;
            SceneManager.LoadScene("CombatScreen");
        }
    }
}