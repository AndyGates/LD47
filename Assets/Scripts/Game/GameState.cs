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

    public int OperationTime { get; set; } // The amount of time availble for operations
    public int Fuel { get; set; } // The amount of fuel left
    public int Resources { get; set; } // The amount of resources left
    public int Health { get; set; } // The amount of health left

    public int MaxOperationTime { get => 10; }
    public int MaxFuel{  get => 100; }
    public int MaxResources { get => 25; }
    public int MaxHealth { get => 10; }

    public bool HasOperationTimeLeft{ get => OperationTime > 0; }
    public bool HasFuelLeft{ get => Fuel > 0; }
    public bool HasResourcesLeft{ get => Resources > 0; }
    public bool HasHealthLeft{ get => Health > 0; }

    public void ResetAll()
    {
        OperationTime = MaxOperationTime;
        Fuel = MaxFuel;
        Health = MaxHealth;
        Resources = MaxResources;
    }

    public void ResetAnomaly()
    {
        OperationTime = MaxOperationTime;
    }

    public void ApplyTravelCost(TravelCost cost)
    {
        OperationTime -= cost.Time;
        Fuel -= cost.Fuel;
        Health -= cost.Health;
    }

    public bool CanTakeTravelCost(TravelCost cost)
    {
        return OperationTime - cost.Time >= 0 &&
            Fuel - cost.Fuel >= 0 &&
            Health - cost.Health >= 0;
    }
}
