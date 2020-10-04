using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInteraction : MonoBehaviour
{
    public Node Node{ get; set; }
    public event System.Action<Node> NodeSelected;
    public event System.Action<Node, bool> NodeHover;

    void OnMouseUpAsButton()
    {
        NodeSelected?.Invoke(Node);
    }

    void OnMouseOver()
    {
        if(Node.State == NodeState.Discovered)
        {
            NodeHover?.Invoke(Node, false);
        }
    }

    void OnMouseExit()
    {
        if(Node.State == NodeState.Discovered)
        {
            NodeHover?.Invoke(Node, true);
        }
    }
}
