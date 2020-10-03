using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public string Name{ get; }
    public int Id{ get; }
    public int Type{ get; }
    public Vector2 Coords{ get; }
    public int Fuel { get; }
    public int Resources { get; }
    public int BuildingSpaces { get; }

    public Node(string name, int id, int type, Vector2 coords, int fuel, int resources, int buildingSpaces)
    {
        Name = name;
        Id = id;
        Type = type;
        Coords = coords;
        Fuel = fuel;
        Resources = resources;
        BuildingSpaces = buildingSpaces;
    }

}
