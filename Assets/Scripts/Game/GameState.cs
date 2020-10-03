using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    ChoosingAction,     // Waiting for the user to choose their next action
    ConfiguringAction,  // Waiting for the user to configure the action. Choose node to travel to, building to build...
    RunningAction,      // We are traveling a route
    RetractingHome,     // Rang out of travel time so retract home has been triggered
    Complete,
}

public enum GameAction
{
    None,
    Travel,
    Build,
    Mine,
    Collect,
    Upgrade,
}

public class GameStateData
{
    public GameState State { get; set; } = GameState.ChoosingAction;
    public GameAction Action{ get; set; } = GameAction.None;
    public float OperationTime { get; set; } // The amount of time availble for operations
    public float Fuel { get; set; } // The amount of fuel left

    public float MaxOperationTime{ get => 10.0f; }
    public float MaxFuel{ get => 100.0f; }

    public bool HasOperationTimeLeft{ get => OperationTime >= MaxOperationTime; }

    public void Reset()
    {
        OperationTime = MaxOperationTime;
        Fuel = MaxFuel;
    }

    public void ApplyTravelCost(TravelCost cost)
    {
        OperationTime -= cost.Time;
        Fuel -= cost.Fuel;
    }
}
