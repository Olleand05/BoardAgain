using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<NodeType> path = new List<NodeType> 
    { 
        NodeType.Enemy,
        NodeType.Rest, 
        NodeType.Enemy, 
        NodeType.Rest, 
        NodeType.Boss
    };

    public GameObject nodePrefab;
    public Transform contentParent;

    public static int currentNodeIndex = 0;

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int i = 0; i < path.Count; i++)
        {
            GameObject newNode = Instantiate(nodePrefab, contentParent);
            NodeUI nodeUI = newNode.GetComponent<NodeUI>();
            bool isInteractable = (i == currentNodeIndex);
            nodeUI.Setup(path[i], isInteractable);
            
        }
    }
}
