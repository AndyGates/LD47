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
    }

    public bool CanTravelToNode(Node node)
    {
        return node.Id != _currentNodeId && _map.IsRouteAvailble(_currentNodeId, node.Id);
    }

    public TravelCost TravelToNode(Node node)
    {
        Route route = _map.FindRoute(_currentNodeId, node.Id);
        _currentNodeId = node.Id;

        int travelFactor = _traveledTravelFactor;

        if(route.State == RouteState.Untraveled)
        {
            route.State = RouteState.Traveled;
            travelFactor = _untraveledTravelFactor;
        }

        int travelTime = route.TravelTime * travelFactor;

        Vector3 to = _map.GetNodeCoords(_currentNodeId);

        transform.right = (to - transform.position).normalized;

        Debug.Log($"Traveling for {travelTime}s");

        iTween.MoveTo(gameObject, iTween.Hash(
            "position", (Vector3)to, 
            "time", travelTime, 
            "oncomplete", nameof(OnTravelComplete),
            "easeType", _easeMethod)
        );

        return new TravelCost()
        {
            Time = travelTime,
            Fuel = route.FuelCost
        };
    }

    public void RetractToHome()
    {
        _currentNodeId = _homeNodeId;

        iTween.MoveTo(gameObject, iTween.Hash(
            "position", (Vector3)_map.GetNodeCoords(_currentNodeId), 
            "speed", _retractHomeTravelSpeed, 
            "oncomplete", nameof(OnRetractHomeComplete),
            "easeType", _easeMethod)
        );
    }

    public void OnTravelComplete()
    {
        TravelComplete.Invoke();
    }

    public void OnRetractHomeComplete()
    {
        RetractHomeComplete.Invoke();
    }
}
