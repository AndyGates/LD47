using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TravelCost
{
    public int Time { get; set; }
    public int Fuel { get; set; }

    public override string ToString()
    {
        return $"Time={Time}, Fuel={Fuel}";
    }
}
