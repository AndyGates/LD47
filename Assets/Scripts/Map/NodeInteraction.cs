using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInteraction : MonoBehaviour
{
    public Node Node{ get; set; }
    public event System.Action<Node> NodeSelected;

    void OnMouseUpAsButton()
    {
        NodeSelected.Invoke(Node);
    }
}
