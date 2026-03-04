using System.Collections; // Required for Coroutines
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public List<NodeType> path = new List<NodeType> { NodeType.Enemy,NodeType.Rest, NodeType.Enemy,NodeType.Rest,NodeType.Boss};
    public GameObject nodePrefab;
    public Transform contentParent;
    public ScrollRect scrollRect; // Assign this in the Inspector

    public static int currentNodeIndex = 0;

    void Start()
    {
        GenerateMap();
        // Start the centering process
        StartCoroutine(CenterOnCurrentNode());
    }

    void GenerateMap()
    {
        // Clear existing nodes if any
        foreach (Transform child in contentParent) { Destroy(child.gameObject); }

        for (int i = 0; i < path.Count; i++)
        {
            GameObject newNode = Instantiate(nodePrefab, contentParent);
            NodeUI nodeUI = newNode.GetComponent<NodeUI>();
            bool isInteractable = (i == currentNodeIndex);
            nodeUI.Setup(path[i], isInteractable);
        }
    }

    IEnumerator CenterOnCurrentNode()
    {
        // Wait for the end of the frame so UI Layout groups can calculate sizes
        yield return new WaitForEndOfFrame();

        if (path.Count > 1)
        {
            // Calculate progress (0 is bottom, 1 is top)
            // We divide the current index by the total possible steps
            float targetValue = (float)currentNodeIndex / (path.Count - 1);

            // Set the scroll position
            scrollRect.verticalNormalizedPosition = targetValue;
        }
    }
}