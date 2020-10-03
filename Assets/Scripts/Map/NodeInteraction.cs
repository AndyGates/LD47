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
        NodeHover?.Invoke(Node, false);
    }

    void OnMouseExit()
    {
        NodeHover?.Invoke(Node, true);
    }
}
