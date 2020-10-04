using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Map _map = null;

    [SerializeField]
    int _traveledTravelFactor = 1;

    [SerializeField]
    int _untraveledTravelFactor = 3;

    [SerializeField]
    float _retractHomeTravelSpeed = 2.0f;

    public event System.Action TravelComplete;
    public event System.Action RetractHomeComplete;

    int _currentNodeId = 0; // Id of the node this player is at or is traveling to
    int _homeNodeId = 0;

    iTween.EaseType _easeMethod = iTween.EaseType.linear;

    public int GetCurrentNodeId()
    {
        return _currentNodeId;
    }

    public void SetAtNodeId(int nodeId)
    {
        _currentNodeId = _homeNodeId = nodeId;
        transform.position = _map.GetNodeCoords(nodeId);
        transform.right = Vector3.right;
    }

    public bool CanTravelToNode(Node node)
    {
        return node.Id != _currentNodeId && _map.IsRouteAvailable(_currentNodeId, node.Id);
    }

    public int CalculateTravelTime(Route route)
    {
        int travelFactor = _traveledTravelFactor;
        if(route.State == RouteState.Untraveled)
        {
            travelFactor = _untraveledTravelFactor;
        }

        return route.TravelTime * travelFactor;
    }

    public TravelCost CalculateTravelCost(Node node)
    {
        Route route = _map.FindRoute(_currentNodeId, node.Id);

        TravelCost cost = null;
        if(route != null)
        {
            cost =  new TravelCost()
            {
                Time = CalculateTravelTime(route),
                Fuel = route.FuelCost,
                Health = route.HealthCost
            };
        }
        return cost;
    }

    public TravelCost TravelToNode(Node node)
    {
        Route route = _map.FindRoute(_currentNodeId, node.Id);

        int travelTime = CalculateTravelTime(route);

        Vector3 to = _map.GetNodeCoords(node.Id);

        transform.right = (to - transform.position).normalized;

        iTween.MoveTo(gameObject, iTween.Hash(
            "position", (Vector3)to, 
            "time", travelTime, 
            "oncomplete", nameof(OnTravelComplete),
            "easeType", _easeMethod)
        );

        _currentNodeId = node.Id;
        route.State = RouteState.Traveled;

        return new TravelCost()
        {
            Time = travelTime,
            Fuel = route.FuelCost,
            Health = route.HealthCost
        };
    }

    public void StartAnomaly()
    {
        _currentNodeId = _homeNodeId;

        iTween.MoveTo(gameObject, iTween.Hash(
            "position", (Vector3)_map.GetNodeCoords(_currentNodeId), 
            "speed", _retractHomeTravelSpeed, 
            "oncomplete", nameof(OnAnomalyComplete),
            "easeType", _easeMethod)
        );
    }

    public void OnTravelComplete()
    {
        TravelComplete.Invoke();
    }

    public void OnAnomalyComplete()
    {
        RetractHomeComplete.Invoke();
    }

    public bool TravelActionValid(GameStateData gameData)
    {
        Node currentNode = _map.FindNode(GetCurrentNodeId());

        // Make sure we have fuel and time
        if(false == gameData.HasFuelLeft)
        {
            Debug.Log("Ran out of fuel");
            return false;
        }

        // See if we can travel to another node
        List<Route> routes = _map.FindLinkedRoutes(GetCurrentNodeId());
        if(routes.Count > 0)
        {
            foreach(Route route in routes)
            {
                TravelCost cost = CalculateTravelCost(currentNode);
                if(false == gameData.CanAffordTravel(cost))
                {
                    return true; // We can travel to atleast 1 node
                }
            }
        }
        else
        {
            Debug.Log("No routes to check");
            return false;
        }

        Debug.Log("Cannot afford to travel");
        return false;
    }
}
