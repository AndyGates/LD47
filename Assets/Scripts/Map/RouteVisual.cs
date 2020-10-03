using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteVisual : MonoBehaviour
{    
    [SerializeField]
    Color _untraveledColor = Color.grey;

    [SerializeField]
    Color _traveledColor = Color.white;

    public Route Route{ get; set; }

    LineRenderer _renderer = null;

    Vector2 _from;
    Vector2 _to;

    void Awake()
    {
        _renderer = FindObjectOfType<LineRenderer>(); // HACKS
    }

    void Update()
    {
        if(Route.State != RouteState.Undiscovered)
        {
            Color color = Route.State == RouteState.Traveled ? _traveledColor : _untraveledColor;
            _renderer.RenderLine(_from, _to, color);
        }
    }

    public void Set(Vector2 from, Vector2 to)
    {
        _from = from;
        _to = to;
    }
}
