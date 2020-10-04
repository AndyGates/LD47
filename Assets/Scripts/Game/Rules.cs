using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules {
    public int RefineryResourcesPerTick { get; set; } = 1;
    public int RefineryFuelPerTick { get; set; } = 1;

    public Rules(int refineryResourcesPerTick, int refineryFuelPerTick)
    {
        RefineryResourcesPerTick = refineryResourcesPerTick;
        RefineryFuelPerTick = refineryFuelPerTick;
    }
}
