using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Map _map = null;

    [SerializeField]
    float _traveledTravelFactor = 1.0f;

    [SerializeField]
    float _untraveledTravelFactor = 3.0f;

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

    public bool CanTravelRoute(Route route)
    {
        return _currentNodeId == route.From || _currentNodeId == route.To;
    }

    public float TravelRoute(Route route)
    {
        _currentNodeId = route.From == _currentNodeId ? route.To : route.From;

        float travelFactor = _traveledTravelFactor;

        if(route.State == RouteState.Untraveled)
        {
            route.State = RouteState.Traveled;
            travelFactor = _untraveledTravelFactor;
        }

        float travelTime = route.TravelTime * travelFactor;

        Debug.Log($"Traveling for {travelTime}s");

        iTween.MoveTo(gameObject, iTween.Hash(
            "position", (Vector3)_map.GetNodeCoords(_currentNodeId), 
            "time", travelTime, 
            "oncomplete", nameof(OnTravelComplete),
            "easeType", _easeMethod)
        );

        return travelTime;
    }

    public bool CanTravelToNode(Node node)
    {
        return _map.IsRouteAvailble(_currentNodeId, node.Id);
    }

    public float TravelToNode(Node node)
    {
        Route route = _map.FindRoute(_currentNodeId, node.Id);
        _currentNodeId = node.Id;

        float travelFactor = _traveledTravelFactor;

        if(route.State == RouteState.Untraveled)
        {
            route.State = RouteState.Traveled;
            travelFactor = _untraveledTravelFactor;
        }

        float travelTime = route.TravelTime * travelFactor;

        Debug.Log($"Traveling for {travelTime}s");

        iTween.MoveTo(gameObject, iTween.Hash(
            "position", (Vector3)_map.GetNodeCoords(_currentNodeId), 
            "time", travelTime, 
            "oncomplete", nameof(OnTravelComplete),
            "easeType", _easeMethod)
        );

        return travelTime;
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
