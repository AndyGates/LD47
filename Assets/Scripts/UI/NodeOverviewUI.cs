using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeOverviewUI : MonoBehaviour
{
    [SerializeField]
    Text _name = null;

    [SerializeField]
    StatGroup _nodeStats = null;

    [SerializeField]
    StatGroup _routeStats = null;

    public GameStateData GameData{ get; set; }

    const string FuelStatName = "Fuel";
    const string ResourcesStatName = "Resources";
    const string SpacesStatName = "Spaces";

    const string TimeCostStatName = "TimeCost";
    const string HealthCostStatName = "HealthCost";
    const string FuelCostStatName = "FuelCost";

    public void SetNode(Node node)
    {
        transform.position = node.Coords;

        _name.text = node.Name;

        _nodeStats.SetStat(FuelStatName, node.Fuel);
        _nodeStats.SetStat(ResourcesStatName, node.Resources);
        _nodeStats.SetStat(SpacesStatName, node.BuildingSpaces);

        // Hide Node info if not discovered
        if(node.State != NodeState.Discovered)
        {
            _nodeStats.gameObject.SetActive(false);
        }
        else
        {
            _nodeStats.gameObject.SetActive(true);
        }

        // Hide route info if Undiscovered
        if(node.State == NodeState.Undiscovered)
        {
            _routeStats.gameObject.SetActive(false);
        }
        else
        {
            _routeStats.gameObject.SetActive(true);
        }
        
    }

    public void SetTravelCost(TravelCost cost)
    {
        if(cost != null)
        {
            _routeStats.gameObject.SetActive(true);

            _routeStats.SetStat(TimeCostStatName, cost.Time);
            _routeStats.SetStatState(TimeCostStatName, GameData.HasEnoughTime(cost.Time) ? StatState.Ok : StatState.Error);

            _routeStats.SetStat(HealthCostStatName, cost.Health);
            _routeStats.SetStatState(HealthCostStatName, GameData.HasEnoughHealth(cost.Health) ? StatState.Ok : StatState.Error);

            _routeStats.SetStat(FuelCostStatName, cost.Fuel);
            _routeStats.SetStatState(FuelCostStatName, GameData.HasEnoughFuel(cost.Fuel) ? StatState.Ok : StatState.Error);
        }
        else
        {
            _routeStats.gameObject.SetActive(false);
        }
    }
}
