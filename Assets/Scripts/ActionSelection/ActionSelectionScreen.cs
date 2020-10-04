using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionScreen : MonoBehaviour
{
    public event System.Action<GameAction> ActionSelected;

    [SerializeField]
    ActionListView _actionList;

    [SerializeField]
    PlanetDetailsPanel _planetDetails;

    [SerializeField]
    Transform _nodeSpawn;

    [SerializeField]
    MapAssets _mapAssets;

    Node _currentNode;

    public void Awake()
    {
        _actionList.ActionSelected += OnActionSelected;
    }

    public void Show(List<ActionData> actions, Node node)
    {
        _actionList.SetActions(actions);
        _planetDetails.SetData(node.Fuel, node.Resources, node.BuildingSpaces, node.Name);
        
        foreach(Transform t in _nodeSpawn)
        {
            Object.Destroy(t.gameObject);
        }

        GameObject go = Instantiate(_mapAssets.GetZoomedNodePrefab(node.Type), Vector3.zero, Quaternion.identity, _nodeSpawn);
        go.transform.localPosition = Vector3.zero;
        
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
        ActionSelected?.Invoke(action.ActionType);
    }
}
