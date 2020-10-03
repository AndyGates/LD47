using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteVisual : MonoBehaviour
{    
    [SerializeField]
    float _width = 0.2f;

    [SerializeField]
    Color _untraveledColor = Color.grey;

    [SerializeField]
    Color _traveledColor = Color.white;

    Route _route = null;
    public Route Route{ get => _route; set => ChangeRoute(value, _route); }

    Renderer _renderer = null;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void Set(Vector2 From, Vector2 To)
    {
        Vector2 dir = To - From;
        float dist =  dir.magnitude;
        Vector2 pos = From + (dir.normalized * (dist / 2.0f));

        transform.position = pos;
        transform.right = dir.normalized;
        transform.localScale = new Vector3(dist, _width, 1.0f);
    }

    void ChangeRoute(Route newRoute, Route oldRoute)
    {
        _route = newRoute;

        if(oldRoute != null)
        {
            oldRoute.StateChanged -= OnStateChanged;
        }
        newRoute.StateChanged += OnStateChanged;

        UpdateColor();
    }

    void OnStateChanged(Route route)
    {
        if(route == Route)
        {
            UpdateColor();
        }
    }

    void UpdateColor()
    {
        _renderer.material.color = Route.State == RouteState.Traveled ? _traveledColor : _untraveledColor;
    }
}
