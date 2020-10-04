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

    Node _currentNode;

    public void Awake()
    {
        _actionList.ActionSelected += OnActionSelected;
    }

    public void Show(List<ActionData> actions, Node node)
    {
        _actionList.SetActions(actions);
        _planetDetails.SetData(node.Fuel, node.Resources, node.BuildingSpaces, node.Name);
    }

    public void OnActionSelected(ActionData action)
    {
        ActionSelected?.Invoke(action.ActionType);
    }
}
