using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeVisual : MonoBehaviour
{
    [SerializeField]
    GameObject _outline = null;

    public bool CanOutline { get; set; } = false;

    [SerializeField]
    GameObject _buildingPrefab;

    [SerializeField]
    List<Transform> _buildingSlots = new List<Transform>();

    Node _node;

    List<GameObject> _buildings = new List<GameObject>();

    public void SetNode(Node node)
    {
        _node = node;
        node.BuildingAdded += OnBuildingAdded;
    }

    public void ResetAll()
    {
        foreach(GameObject go in _buildings)
        {
            Destroy(go);
        }
        _buildings.Clear();

        CanOutline = false;
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

    void OnEnable()
    {
        _outline.SetActive(false);
    }

    void OnMouseOver()
    {
        if(CanOutline)
        {
            _outline.SetActive(true);
        }
    }

    void OnMouseExit()
    {
        _outline.SetActive(false);
    }

    public void OnBuildingAdded()
    {
        if(_buildingSlots.Count > 0)
        {
            Transform t = _buildingSlots[0];
            GameObject go = Instantiate(_buildingPrefab, Vector3.zero, Quaternion.identity, t);
            go.transform.localPosition = Vector3.zero;

            _buildings.Add(go);
        }
        else
        {
            Debug.LogError("No physical building positions available");
        }
    } 
}
