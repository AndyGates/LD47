using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RouteState
{
    Undiscovered,
    Untraveled,     // Discovered but untraveled
    Traveled,
}

public class Route
{
    public int From{ get; }
    public int To{ get; }

    public float TravelTime { get; } // THe nominal travel time of this route

    public float FuelCost { get; }

    RouteState _state = RouteState.Undiscovered;
    public RouteState State { get => _state; set => ChangeState(value, _state); }

    public event System.Action<Route> StateChanged;

    public Route(int from, int to, float travelTime, float fuelCost)
    {
        From = from;
        To = to;
        TravelTime = travelTime;
        FuelCost = fuelCost;
    }

    public void TravelRoute()
    {
        Debug.Log($"Travel from {From} to {To}");
    }

    void ChangeState(RouteState newState, RouteState oldState)
    {
        _state = newState;

        StateChanged?.Invoke(this);
    }
}
