using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeOverviewUI : MonoBehaviour
{
    [SerializeField]
    Text _availableFuel = null;

    [SerializeField]
    Text _availableResources = null;

    [SerializeField]
    GameObject _travelCostUI = null;

    [SerializeField]
    Text _timeCost = null;

    [SerializeField]
    Text _healthCost = null;

    [SerializeField]
    Text _fuelCost = null;


    public void SetNode(Node node)
    {
        transform.position = node.Coords;

        _availableFuel.text = node.Fuel.ToString();
        _availableResources.text = node.Resources.ToString();
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
