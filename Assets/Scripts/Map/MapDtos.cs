using System.Collections.Generic;
using UnityEngine;

public class MapDto
{
    public List<NodeDto> Nodes { get; set; }
    public List<RouteDto> Routes { get; set; }
}

public class NodeDto
{
    public string Name { get; set; }
    public int Id { get; set; }
    public int Type { get; set; }
    public Vector2 Coords { get; set; }
    public int Fuel { get; set; }
    public int Resources { get; set; }
    public int BuildingSpaces { get; set; }
}

public class RouteDto
{
    public int To { get; set; }
    public int From { get; set; }
    public int TravelTime { get; set; }
    public int FuelCost { get; set; }
    public int HealthCost { get; set; }
}