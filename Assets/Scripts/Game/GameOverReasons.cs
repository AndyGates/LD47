using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverReasons
{
    public const string NoFuel = "You cannot go very far without any fuel";
    public const string NoFuelInTransit = "You ran out of fuel whilst traveling";
    public const string DiedInTransit = "Your ship failed you whilst traveling";
    public const string NoFuelDiedInTransit = "That was ambitious! Your ship failed and ran out of fuel.";
    public const string NoHealth = "Your ship has taken a beating and has failed";
    public const string NoRoutes = "No routes to travel"; // This should never happen
    public const string Cost = "Cannot afford to travel";
}
