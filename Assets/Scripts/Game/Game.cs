using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    TextAsset _mapData = null;

    [SerializeField]
    Map _map = null;

    [SerializeField]
    Player _player = null;

    float _operationTime = 0.0f;
    float _maxOperationTime = 6.0f;

    GameStateData GameData { get; set; } = new GameStateData();

    void Awake()
    {
        _map.LoadMap(_mapData);
        _map.NodeSelected += OnNodeSelected;

        _player.SetAtNodeId(0);
        _player.TravelComplete += OnPlayerTravelComplete;
        _player.RetractHomeComplete += OnPlayerRetractHomeComplete;
    }

    void Update()
    {
        // HACKS
        if(GameData.State == GameState.ChoosingAction)
        {
            Debug.Log("Skipping user select action");
            ApplyAction();
        }
    }

    void OnNodeSelected(Node node)
    {
        if(GameData.State == GameState.ConfiguringAction && GameData.Action == GameAction.Travel && 
            _player.CanTravelToNode(node))
        {
            // TODO: Validate if we have enough time/fuel left to do this travel
            TravelCost cost = _player.TravelToNode(node);
            GameData.ApplyTravelCost(cost);
            GameData.State = GameState.RunningAction;
        }
    }

    void OnPlayerTravelComplete()
    {
        Debug.Log($"Travel completed. {_maxOperationTime - _operationTime}s of travel time left");

        if(_map.GetNodeType(_player.GetCurrentNodeId()) == 1)
        {
            Debug.Log("Well done you escaped the loop");
            GameData.State = GameState.Complete;
        }
        else
        {
            GameData.State = GameState.ChoosingAction;
        }
    }

    void RetractHome()
    {
        _player.RetractToHome();
        GameData.State = GameState.RetractingHome;
    }

    void OnPlayerRetractHomeComplete()
    {
        GameData.State = GameState.ChoosingAction;
        GameData.Reset();
    }

    void ApplyAction()
    {
        // TODO Do action stuff

        // Action completed now allow user to select node or retract to home
        if(GameData.HasOperationTimeLeft)
        {
            Debug.Log("Ran out of travel time. Retracting to home.");
            RetractHome();
        }
        else
        {
            GameData.Action = GameAction.Travel;
            GameData.State = GameState.ConfiguringAction;
        }
    }
}
