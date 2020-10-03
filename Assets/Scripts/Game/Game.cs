using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    enum GameState
    {
        SelectingNode, // Waiting for the user to select a node to travel
        Traveling,  // We are traveling a route
        SelectingAction, // Waiting on the user to select a build/mine/collect/upgrade ship action
        RetractingHome, // Rang out of travel time so retract home has been triggered
        Complete,
    }

    [SerializeField]
    TextAsset _mapData = null;

    [SerializeField]
    Map _map = null;

    [SerializeField]
    Player _player = null;

    GameState State { get; set; } = GameState.SelectingNode;

    float _travelTime = 0.0f;
    float _maxTravelTime = 6.0f;

    void Awake()
    {
        _map.LoadMap(_mapData);
        _map.RouteSelected += OnRouteSelected;
        _map.NodeSelected += OnNodeSelected;

        _player.SetAtNodeId(0);
        _player.TravelComplete += OnPlayerTravelComplete;
        _player.RetractHomeComplete += OnPlayerRetractHomeComplete;
    }

    void Update()
    {
        // HACKS
        if(State == GameState.SelectingAction)
        {
            Debug.Log("Skipping user select action");
            ApplyAction();
        }
    }

    void OnRouteSelected(Route route)
    {
        if(State == GameState.SelectingNode && _player.CanTravelRoute(route))
        {
            // TODO: Validate if we have enough time left to do this travel
             _travelTime += _player.TravelRoute(route);
            State = GameState.Traveling;
        }
    }

    void OnNodeSelected(Node node)
    {
        if(State == GameState.SelectingNode && node.Id != _player.GetCurrentNodeId() && _player.CanTravelToNode(node))
        {
            // TODO: Validate if we have enough time left to do this travel
             _travelTime += _player.TravelToNode(node);
            State = GameState.Traveling;
        }
    }

    void OnPlayerTravelComplete()
    {
        State = GameState.SelectingAction;

        Debug.Log($"Travel completed. {_maxTravelTime - _travelTime}s of travel time left");

        if(_map.GetNodeType(_player.GetCurrentNodeId()) == 1)
        {
            Debug.Log("Well done you escaped the loop");
            State = GameState.Complete;
        }
    }

    void RetractHome()
    {
        _player.RetractToHome();
        State = GameState.RetractingHome;
    }

    void OnPlayerRetractHomeComplete()
    {
        State = GameState.SelectingNode;
        _travelTime = 0.0f;
    }

    void ApplyAction()
    {
        // TODO Do action stuff

        // Action completed now allow user to select node or retract to home
        if(_travelTime >= _maxTravelTime)
        {
            Debug.Log("Ran out of travel time. Retracting to home.");
            RetractHome();
        }
        else
        {
            State = GameState.SelectingNode;
        }
    }
}
