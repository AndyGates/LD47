using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInteraction : MonoBehaviour
{
    public Node Node{ get; set; }
    public event System.Action<Node> NodeSelected;
    public event System.Action<Node, bool> NodeHover;

    bool _active = false;

    void OnMouseUpAsButton()
    {
        NodeSelected?.Invoke(Node);
    }

    void OnMouseOver()
    {
        NodeHover?.Invoke(Node, false);
        _active = true;
    }

    void OnMouseExit()
    {
        NodeHover?.Invoke(Node, true);
        _active = false;
    }

    void OnEnable()
    {
        if(_active)
        {
            OnMouseExit();
        }
    }
}
