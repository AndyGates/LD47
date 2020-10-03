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

    [SerializeField]
    int _goalNodeType = 5;

    int _startNodeId = 0;

    GameStateData GameData { get; set; } = new GameStateData();

    void Awake()
    {
        _map.LoadMap(_mapData);
        _map.NodeSelected += OnNodeSelected;

        _player.SetAtNodeId(_startNodeId);
        _player.TravelComplete += OnPlayerTravelComplete;
        _player.RetractHomeComplete += OnPlayerRetractHomeComplete;

        _map.MarkNodeRoutesAsDiscovered(_startNodeId);

        GameData.ResetAll();
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
        if(GameData.State == GameState.ConfiguringAction && GameData.Action == GameAction.Travel)
        {
            bool canTravel = _player.CanTravelToNode(node);
            TravelCost cost = _player.CalculateTravelCost(node);
            bool canAffordTravel = GameData.CanTakeTravelCost(cost);
            if(canTravel && canAffordTravel)
            {
                _player.TravelToNode(node);
                GameData.ApplyTravelCost(cost);
                GameData.State = GameState.RunningAction;

                Debug.Log($"Traveling to {node.Name} with cost {cost.ToString()}");
            }
            else
            {
                Debug.Log($"Not traveling to {node.Name} CanTravel={canTravel}, CanAffordTravel={canAffordTravel}");
            }
        }
    }

    void OnPlayerTravelComplete()
    {
        Debug.Log($"Travel completed. {GameData.OperationTime}s of travel time left");

        if(_map.GetNodeType(_player.GetCurrentNodeId()) == _goalNodeType)
        {
            Debug.Log("Well done you escaped the loop");
            GameData.State = GameState.Complete;
        }
        else
        {
            GameData.State = GameState.ChoosingAction;
        }

        _map.MarkNodeRoutesAsDiscovered(_player.GetCurrentNodeId());
    }

    void RetractHome()
    {
        _player.RetractToHome();
        GameData.State = GameState.RetractingHome;
    }

    void OnPlayerRetractHomeComplete()
    {
        GameData.State = GameState.ChoosingAction;
        GameData.ResetAnomaly();
    }

    void ApplyAction()
    {
        // TODO Do action stuff

        // Action completed now allow user to select node or retract to home
        if(false == GameData.HasOperationTimeLeft)
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
