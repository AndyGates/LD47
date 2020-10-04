using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeOverviewUI : MonoBehaviour
{
    [SerializeField]
    Text _name = null;

    [SerializeField]
    Text _availableFuel = null;

    [SerializeField]
    Text _availableResources = null;

    [SerializeField]
    Text _availableSpaces = null;

    [SerializeField]
    GameObject _travelCostUI = null;

    [SerializeField]
    Text _timeCost = null;

    [SerializeField]
    Text _healthCost = null;

    [SerializeField]
    Text _fuelCost = null;

    [SerializeField]
    GameObject _nodeStats = null;

    [SerializeField]
    GameObject _routeStats = null;

    public void SetNode(Node node)
    {
        transform.position = node.Coords;

        _name.text = node.Name;

        _availableFuel.text = node.Fuel.ToString();
        _availableResources.text = node.Resources.ToString();
        _availableSpaces.text = node.BuildingSpaces.ToString();

        // Hide Node info if not discovered
        if(node.State != NodeState.Discovered)
        {
            _nodeStats.SetActive(false);
        }
        else
        {
            _nodeStats.SetActive(true);
        }

        // Hide route info if Undiscovered
        if(node.State == NodeState.Undiscovered)
        {
            _routeStats.SetActive(false);
        }
        else
        {
            _routeStats.SetActive(true);
        }
        
    }

    public void SetTravelCost(TravelCost cost)
    {
        if(cost != null)
        {
            _travelCostUI.SetActive(true);
            _timeCost.text = cost.Time.ToString();
            _healthCost.text = cost.Health.ToString();
            _fuelCost.text = cost.Fuel.ToString();
        }
        else
        {
            _travelCostUI.SetActive(false);
        }
    }
}
