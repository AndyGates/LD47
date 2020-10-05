using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    NotStarted,
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
    Repair,
    ViewRoutes,
}

public class GameStateData
{
    public GameState State { get; set; } = GameState.NotStarted;
    public GameAction Action{ get; set; } = GameAction.None;

    public int OperationTime { get; set; } // The amount of time available for operations
    public int Fuel { get; set; } // The amount of fuel left
    public int Resources { get; set; } // The amount of resources left
    public int Health { get; set; } // The amount of health left

    public int RepairCost{ get; set; }

    public int MaxOperationTime { get => 10; }
    public int StartFuel{  get => 30; }
    public int StartResources { get => 0; }
    public int StartHealth { get => 10; }

    public bool HasOperationTimeLeft{ get => OperationTime > 0; }
    public bool HasFuelLeft{ get => Fuel > 0; }
    public bool HasResourcesLeft{ get => Resources > 0; }
    public bool HasHealthLeft{ get => Health > 0; }

    public bool HasHealthDebt{ get => Health < 0; }
    public bool HasFuelDebt{ get => Health < 0; }

    public bool CanRepair{ get => Resources >= RepairCost; }

    public void ResetAll()
    {
        OperationTime = MaxOperationTime;
        Fuel = StartFuel;
        Health = StartHealth;
        Resources = StartResources;
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

        Debug.Log($"After travel costs we have: Time={OperationTime}, Fuel={Fuel}, Health={Health}");
    }

    public bool CanAffordTravel(TravelCost cost)
    {
        if(cost == null)
        {
            return false;
        }

        return HasEnoughTime(cost.Time) &&
            HasEnoughFuel(cost.Fuel) &&
            HasEnoughHealth(cost.Health);
    }

    public bool HasEnoughTime(int timeCost)
    {
        return OperationTime - timeCost >= 0;
    }

    public bool HasEnoughFuel(int fuelCost)
    {
        return Fuel - fuelCost >= 0;
    }

    public bool HasEnoughHealth(int healthCost)
    {
        return Health - healthCost >= 0;
    }
}
