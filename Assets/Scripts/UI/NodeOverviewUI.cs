using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeOverviewUI : MonoBehaviour
{
    public void SetNode(Node node)
    {
        transform.position = node.Coords;
    }
}
