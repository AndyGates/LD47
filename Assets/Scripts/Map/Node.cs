using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    Undiscovered, // Undiscovered and cannot travel too
    CanTravel, // Undiscovered but can travel too
    Discovered,
}

public class Node
{
    public event System.Action BuildingAdded;

    public string Name{ get; }
    public int Id{ get; }
    public int Type{ get; }
    public Vector2 Coords{ get; }
    public int Fuel { get; set; }
    public int Resources { get; set; }
    public int BuildingSpaces { get; set; }

    public int ActiveBuildings { get { return DefaultBuildingSpaces - BuildingSpaces; } }

    public NodeState State { get; set; }

    public NodeVisual Visual { get; set; }

    public bool HasFuel{ get => Fuel > 0; }

    readonly int DefaultFuel = 0;
    readonly int DefaultResources = 0;
    readonly int DefaultBuildingSpaces = 0;

    public Node(string name, int id, int type, Vector2 coords, int fuel, int resources, int buildingSpaces)
    {
        Name = name;
        Id = id;
        Type = type;
        Coords = coords;
        DefaultFuel = Fuel = fuel;
        DefaultResources = Resources = resources;
        DefaultBuildingSpaces = BuildingSpaces = buildingSpaces;
    }

    public void ResetAll()
    {
        Fuel = DefaultFuel;
        Resources = DefaultResources;
        BuildingSpaces = DefaultBuildingSpaces;
        State = NodeState.Undiscovered;

        if(Visual != null)
        {
            Visual.ResetAll();
        }
    }

    public void OnCanTravelTo()
    {
        if(Visual != null)
        {
            Visual.CanOutline = true;
        }

        if(State == NodeState.Undiscovered)
        {
            State = NodeState.CanTravel;
        }
    }

    public bool AddBuilding()
    {
        if(BuildingSpaces > 0)
        {
            BuildingSpaces--;
            BuildingAdded?.Invoke();
        }
        else
        {
            Debug.LogError("Cannot add building with no spaces...");
            return false;
        }

        return true;
    }

}
