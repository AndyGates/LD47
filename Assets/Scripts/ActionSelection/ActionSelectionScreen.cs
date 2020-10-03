using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionScreen : MonoBehaviour
{
    public event System.Action<Action> ActionSelected;

    [SerializeField]
    ActionListView _actionList;

    Node _currentNode;

    public void Awake()
    {
        _actionList.ActionSelected += OnActionSelected;
    }

    public void Show(Node node)
    {
        //Generate actions from node
        _actionList.SetActions(new List<Action>{
            new Action() { Text = "MINE RESOURCES", Time = 1, Resources = 10, Fuel = 0 },
            new Action() { Text = "MINE FUEL", Time = 2, Resources = 0, Fuel = 10 },
            new Action() { Text = "BUILD REFINERY", Time = 1, Resources = -25, Fuel = 0 },
            new Action() { Text = "UPGRADE SHIP", Time = 3, Resources = -10, Fuel = 0 },
            new Action() { Text = "LEAVE", Time = 0, Resources = 0, Fuel = 0 }
         });
    }

    public void OnActionSelected(Action action)
    {
        ActionSelected?.Invoke(action);
    }
}
