using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatItem : MonoBehaviour
{
    [SerializeField]
    string _statName = "Stat";

    [SerializeField]
    Text _value = null;

    [SerializeField]
    Image _icon = null;

    [SerializeField]
    Color _errorColor = Color.red;

    Color _okColor = Color.white;

    public string StateName{ get => _statName; }

    void Awake()
    {
        _okColor = _value.color;
    }

    public void SetStat<T>(T value)
    {
        _value.text = value.ToString();
    }

    public void SetState(StatState state)
    {
        switch(state)
        {
            case StatState.Ok:
                SetColor(_okColor);
                break;
            case StatState.Error:
                SetColor(_errorColor);
                break;
        }
    }

    void SetColor(Color color)
    {
        _value.color = color;
        _icon.color = color;
    }
}
