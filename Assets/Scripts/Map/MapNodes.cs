using UnityEngine;

public enum NodeType
{
    Enemy,
    Rest,
    Boss
}

public class MapNodes
{
    public NodeType type;
    public string nodeName;
}
