using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public string Name{ get; }
    public int Id{ get; }
    public int Type{ get; }
    public Vector2 Coords{ get; }
    public int Fuel { get; set; }
    public int Resources { get; set; }
    public int BuildingSpaces { get; set; }

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
    }

    public void UpdateTicks(int ticks)
    {
        int buildings = DefaultBuildingSpaces - BuildingSpaces;
    }

}
