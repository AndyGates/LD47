﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionListView : MonoBehaviour
{
    public event System.Action<Action> ActionSelected;

    [SerializeField]
    int _itemHeight;

    [SerializeField]
    int _offset;

    [SerializeField]
    ActionListItem _itemPrefab;

    public void SetActions(List<Action> actions)
    {
        Debug.Log("Setting list actions");

        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < actions.Count; i++)
        {
            Vector3 pos = new Vector3(0, (i*_itemHeight) + _offset, 0);
            ActionListItem item = GameObject.Instantiate<ActionListItem>(_itemPrefab, Vector3.zero, Quaternion.identity, transform);
            item.transform.localPosition = pos;

            Action act = actions[i];
            item.SetData(act.Text, act.Time, act.Fuel, act.Resources);
        }
    }
}
