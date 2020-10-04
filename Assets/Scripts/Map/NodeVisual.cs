using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeVisual : MonoBehaviour
{
    [SerializeField]
    GameObject _outline = null;

    [SerializeField]
    GameObject _buildingPrefab;

    [SerializeField]
    List<Transform> _buildingSlots = new List<Transform>();

    Node _node;

    public void SetNode(Node node)
    {
        _node = node;
        node.BuildingAdded += OnBuildingAdded;
    }

    void Awake()
    {
        if(_outline == null)
        {
            enabled = false;
        }
        else
        {
            _outline.SetActive(false);
        }
    }

    void OnMouseOver()
    {
        _outline.SetActive(true);
    }

    void OnMouseExit()
    {
        _outline.SetActive(false);
    }

    void OnBuildingAdded()
    {
        if(_buildingSlots.Count > 0)
        {
            Transform t = _buildingSlots[0];
            GameObject go = Instantiate(_buildingPrefab, Vector3.zero, Quaternion.identity, t);
            go.transform.localPosition = Vector3.zero;
        }
        else
        {
            Debug.LogError("No physical building positions available");
        }
    } 
}
