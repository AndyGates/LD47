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

    public Node CurrentNode { get; private set; }
    public int CurrentNodeId { get => CurrentNode.Id; }
    int _homeNodeId = 0;

    Route _activeRoute = null;

    iTween.EaseType _easeMethod = iTween.EaseType.linear;

    public void SetAtNodeId(int nodeId)
    {
        CurrentNode = _map.FindNode(nodeId);
        _homeNodeId = nodeId;
        transform.position = _map.GetNodeCoords(nodeId);
        transform.right = Vector3.right;

        OnDiscoverNode(_map.FindNode(nodeId));
    }

    public bool CanTravelToNode(Node node)
    {
        return node.Id != CurrentNodeId && _map.IsRouteAvailable(CurrentNodeId, node.Id);
    }

    public int CalculateTravelTime(Route route)
    {
        if(route == null)
        {
            return -1;
        }

        int travelFactor = _traveledTravelFactor;
        if(route.State == RouteState.Untraveled)
        {
            travelFactor = _untraveledTravelFactor;
        }

        return route.TravelTime * travelFactor;
    }

    public TravelCost CalculateTravelCost(Node node)
    {
        Route route = _map.FindRoute(CurrentNodeId, node.Id);

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
        Route route = _map.FindRoute(CurrentNodeId, node.Id);

        int travelTime = CalculateTravelTime(route);

        Vector3 to = _map.GetNodeCoords(node.Id);

        transform.right = (to - transform.position).normalized;

        iTween.MoveTo(gameObject, iTween.Hash(
            "position", (Vector3)to, 
            "time", travelTime, 
            "oncomplete", nameof(OnTravelComplete),
            "easeType", _easeMethod)
        );

        CurrentNode = node;
        _activeRoute = route;
                
        return new TravelCost()
        {
            Time = travelTime,
            Fuel = route.FuelCost,
            Health = route.HealthCost
        };
    }

    void OnDiscoverNode(Node node)
    {
        node.State = NodeState.Discovered;
        node.Visual.CanOutline = true;

        List<Route> linkedRoutes = _map.FindLinkedRoutes(node.Id);
        foreach(Route linkedRoute in linkedRoutes)
        {
            Node toNode = _map.FindNode(linkedRoute.To);
            toNode.OnCanTravelTo();

            Node fromNode = _map.FindNode(linkedRoute.From);
            fromNode.OnCanTravelTo();
        }
    }

    public void StartAnomaly()
    {
        CurrentNode = _map.FindNode(_homeNodeId);

        iTween.MoveTo(gameObject, iTween.Hash(
            "position", (Vector3)CurrentNode.Coords, 
            "speed", _retractHomeTravelSpeed, 
            "oncomplete", nameof(OnAnomalyComplete),
            "easeType", _easeMethod)
        );
    }

    public void OnTravelComplete()
    {
        TravelComplete.Invoke();

        OnDiscoverNode(CurrentNode);

        _activeRoute.State = RouteState.Traveled;
        _activeRoute = null;
    }

    public void OnAnomalyComplete()
    {
        RetractHomeComplete.Invoke();
    }

    public string CanContinue(GameStateData gameData)
    {
        if(gameData.HasHealthDebt && gameData.HasFuelDebt)
        {
            return GameOverReasons.NoFuelDiedInTransit;
        }

        if(gameData.HasHealthDebt)
        {
            return GameOverReasons.DiedInTransit;
        }

        if(gameData.HasFuelDebt)
        {
            return GameOverReasons.NoFuelInTransit;
        }

        // Make sure we have fuel or can get some
        if(false == gameData.HasFuelLeft && false == CurrentNode.HasFuel)
        {
            return GameOverReasons.NoFuel;
        }

        // Make sure we have health or can repair
        if(false == gameData.HasHealthLeft && false == gameData.CanRepair && CurrentNode.Resources < gameData.RepairCost)
        {
            return GameOverReasons.NoHealth;
        }

        return null;

        // See if we can travel to another node
        /*List<Route> routes = _map.FindLinkedRoutes(CurrentNodeId);
        if(routes.Count > 0)
        {
            foreach(Route route in routes)
            {
                TravelCost cost = CalculateTravelCost(CurrentNode);
                if(false == gameData.CanAffordTravel(cost))
                {
                    return null; // We can travel to atleast 1 node
                }
            }
        }
        else
        {
            // How did we get here is there are not routes?
            return GameOverReasons.NoRoutes;
        }

        // Generic reason
        return GameOverReasons.Cost;*/
    }
}
