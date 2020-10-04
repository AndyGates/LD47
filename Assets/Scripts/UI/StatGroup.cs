using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatState
{
    Ok,
    Error,
}

public class StatGroup : MonoBehaviour
{
    [SerializeField]
    List<StatItem> _items;

    Dictionary<string, StatItem> _itemMap = new Dictionary<string, StatItem>();

    void Awake()
    {
        foreach(StatItem item in _items)
        {
            _itemMap[item.StateName] = item;
        }
    }

    public StatItem GetItem(string statName)
    {
        if(_itemMap.ContainsKey(statName))
        {
            return _itemMap[statName];
        }
        Debug.LogWarning($"Failed to find stat {statName} in {name}");
        return null;
    }

    public void SetStat<T>(string statName, T value)
    {
        GetItem(statName)?.SetStat(value);
    }

    public void SetStatState(string statName, StatState state)
    {
        GetItem(statName)?.SetState(state);
    }
}
