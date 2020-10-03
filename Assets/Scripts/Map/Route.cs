using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RouteState
{
    Untraveled,
    Traveled,
}

public class Route
{
    public int From{ get; }
    public int To{ get; }

    public float TravelTime { get; } // THe nominal travel time of this route

    RouteState _state = RouteState.Untraveled;
    public RouteState State { get => _state; set => ChangeState(value, _state); }

    public event System.Action<Route> StateChanged;

    public Route(int from, int to, float travelTime)
    {
        From = from;
        To = to;
        TravelTime = travelTime;
    }

    public void TravelRoute()
    {
        Debug.Log($"Travel from {From} to {To}");
    }

    void ChangeState(RouteState newState, RouteState oldState)
    {
        _state = newState;

        StateChanged.Invoke(this);
    }
}
