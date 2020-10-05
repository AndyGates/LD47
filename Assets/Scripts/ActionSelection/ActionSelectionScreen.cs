using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionScreen : MonoBehaviour
{
    public event System.Action<ActionData> ActionSelected;

    [SerializeField]
    ActionListView _actionList;

    [SerializeField]
    PlanetDetailsPanel _planetDetails;

    [SerializeField]
    Transform _nodeSpawn;

    [SerializeField]
    MapAssets _mapAssets;

    Node _currentNode;

    Vector3 _nodeSpawnOffset;
    GameObject _nodeGO;

    public void Awake()
    {
        _actionList.ActionSelected += OnActionSelected;
        _nodeSpawnOffset = _nodeSpawn.position;
    }

    public void Show(List<ActionData> actions, Node node)
    {
        _actionList.SetActions(actions);
        _planetDetails.SetData(node.Fuel, node.Resources, node.BuildingSpaces, node.Name);

        _nodeSpawn.position = _nodeSpawnOffset + Camera.main.transform.position;

        if(_nodeGO != null)
        {
            Object.Destroy(_nodeGO);
        }

        GameObject go = Instantiate(_mapAssets.GetZoomedNodePrefab(node.Type), Vector3.zero, Quaternion.identity, _nodeSpawn);
        go.transform.localPosition = Vector3.zero;
        _nodeGO = go;
        
        NodeVisual nv = go.GetComponent<NodeVisual>();
        if(nv != null)
        {
            for(int i = 0; i < node.ActiveBuildings; i++)
            {
                nv.OnBuildingAdded();
            }
        }

    }

    public void OnActionSelected(ActionData action)
    {
        ActionSelected?.Invoke(action);
    }
}
