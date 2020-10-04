using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionScreen : MonoBehaviour
{
    public event System.Action<ActionData> ActionSelected;

    [SerializeField]
    ActionListView _actionList;

    Node _currentNode;

    public void Awake()
    {
        _actionList.ActionSelected += OnActionSelected;
    }

    public void Show(List<ActionData> actions)
    {
        _actionList.SetActions(actions);
    }

    public void OnActionSelected(ActionData action)
    {
        ActionSelected?.Invoke(action);
    }
}
