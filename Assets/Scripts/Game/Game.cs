using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Game : MonoBehaviour
{
    [SerializeField]
    List<ActionData> _actionDefinitions;

    [SerializeField]
    TextAsset _mapData = null;

    [SerializeField]
    Map _map = null;

    [SerializeField]
    Player _player = null;

    [SerializeField]
    int _goalNodeType = 5;

	[SerializeField]
    GameObject _routeSelectionScreen = null; 

    [SerializeField]
    ActionSelectionScreen _actionSelectionScreen = null;

    [SerializeField]
    NodeOverviewUI _nodeOverviewUI = null;

    [SerializeField]
    PlayerHUD _playerHUD = null;

    [SerializeField]
    GameObject _startScreen = null;

    [SerializeField]
    GameObject _gameOverScreen = null;

    [SerializeField]
    GameObject _winnersScreen = null;

    int _startNodeId = 0;

    GameStateData GameData { get; set; } = new GameStateData();
        
    Dictionary<GameAction, ActionData> _actionMap; 

    void Awake()
    {
        _actionMap = _actionDefinitions.ToDictionary(x => x.ActionType, x => x);

        _map.LoadMap(_mapData);
        _map.NodeSelected += OnNodeSelected;
        _map.NodeHover += OnNodeHover;

        _player.SetAtNodeId(_startNodeId);
        _player.TravelComplete += OnPlayerTravelComplete;
        _player.RetractHomeComplete += OnPlayerAnomalyComplete;

        _map.MarkNodeRoutesAsDiscovered(_startNodeId);

        _actionSelectionScreen.ActionSelected += ApplyAction;

        _playerHUD.SetGameState(GameData);
    }

    public void StartGame()
    {
        GameData.ResetAll();

        GameData.State = GameState.ConfiguringAction;
        GameData.Action = GameAction.Travel;

        _playerHUD.gameObject.SetActive(true);
        _routeSelectionScreen.gameObject.SetActive(true);
        _actionSelectionScreen.gameObject.SetActive(false);
        _startScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
        _winnersScreen.SetActive(false);

        _map.ResetAll();
        _player.SetAtNodeId(_startNodeId);
        _map.MarkNodeRoutesAsDiscovered(_startNodeId);
    }

    public void RestartGame()
    {
        StartGame();
    }

    public void GameOver()
    {
        _playerHUD.gameObject.SetActive(false);
        _routeSelectionScreen.gameObject.SetActive(false);
        _actionSelectionScreen.gameObject.SetActive(false);
        _startScreen.SetActive(false);
        _gameOverScreen.SetActive(true);
        _winnersScreen.SetActive(false);
    }

    public void Win()
    {
        _playerHUD.gameObject.SetActive(false);
        _routeSelectionScreen.gameObject.SetActive(false);
        _actionSelectionScreen.gameObject.SetActive(false);
        _startScreen.SetActive(false);
        _gameOverScreen.SetActive(false);
        _winnersScreen.SetActive(true);
    }

    void OnNodeSelected(Node node)
    {
        if(GameData.State == GameState.ConfiguringAction && GameData.Action == GameAction.Travel)
        {
            bool canTravel = _player.CanTravelToNode(node);
            TravelCost cost = _player.CalculateTravelCost(node);
            bool canAffordTravel = GameData.CanAffordTravel(cost);
            if(canTravel && canAffordTravel)
            {
                _player.TravelToNode(node);
                GameData.ApplyTravelCost(cost);
                GameData.State = GameState.RunningAction;

                _map.TickNodes(cost.Time);

                Debug.Log($"Traveling to {node.Name} with cost {cost.ToString()}");
            }
            else
            {
                Debug.Log($"Not traveling to {node.Name} CanTravel={canTravel}, CanAffordTravel={canAffordTravel}");
            }
        }
    }

    void OnNodeHover(Node node, bool exited)
    {
        _nodeOverviewUI.gameObject.SetActive(!exited);
        _nodeOverviewUI.SetNode(node);

        TravelCost cost = _player.CalculateTravelCost(node);
        _nodeOverviewUI.SetTravelCost(cost);
    }

    void OnPlayerTravelComplete()
    {
        Debug.Log($"Travel completed. {GameData.OperationTime}s of travel time left");

        if(_map.GetNodeType(_player.CurrentNodeId) == _goalNodeType)
        {
            Debug.Log("Well done you escaped the loop");
            GameData.State = GameState.Complete;

            Win();
        }
        else
        {
            GameData.State = GameState.ChoosingAction;
            ShowActionSelectionScreen();
        }

        _map.MarkNodeRoutesAsDiscovered(_player.CurrentNodeId);
    }

    List<ActionData> GetAvailableActions(List<GameAction> nodeActions)
    {
        //Can always leave
        List<ActionData> actions = new List<ActionData>();

        foreach(GameAction ga in nodeActions)
        {
            if(_actionMap.ContainsKey(ga))
            {
                ActionData data = _actionMap[ga];

                //Only add the action if the player has time
                if(GameData.OperationTime >= data.Time)
                {
                    switch(ga)
                    {
                        //Only add these actions if the player has the resources
                        case GameAction.Build:
                        case GameAction.Upgrade:
                            if(GameData.Resources >= -data.Resources)
                            {
                                actions.Add(data);
                            }
                            break;

                        default:
                            actions.Add(data);
                            break;
                    }
                }
            }
            else
            {
                Debug.LogError($"Could not find action rule for {ga}");
            }
        }

        actions.Add(_actionMap[GameAction.Travel]);
        return actions;
    }

    void ShowActionSelectionScreen()
    {              
        //Probably need a better way of doing this
        _routeSelectionScreen.SetActive(false);
        _actionSelectionScreen.gameObject.SetActive(true);

        //This probably shouldn't be done here...
        //Get the actions available from the node 
        List<GameAction> nodeActions = _map.GetAvailableActions(_player.CurrentNodeId);

        //Combine these with the actions the player can currently do and get the action data 
        List<ActionData> availableActions = GetAvailableActions(nodeActions);

        _actionSelectionScreen.Show(availableActions);
    }

    void ShowRouteSelectionScreen()
    {
        //Probably need a better way of doing this
        _actionSelectionScreen.gameObject.SetActive(false);
        _routeSelectionScreen.SetActive(true);
    }

    void StartAnomaly()
    {
        _player.StartAnomaly();
        GameData.State = GameState.RetractingHome;
    }

    void OnPlayerAnomalyComplete()
    {
        GameData.Action = GameAction.Travel;
        GameData.State = GameState.ConfiguringAction;
        GameData.ResetAnomaly();

        Debug.Log("Anomaly completed");
    }

    void ApplyAction(GameAction action)
    {
        if(_actionMap.ContainsKey(action))
        {
            ActionData data = _actionMap[action];
            GameData.Action = action;
            // TODO Do action stuff
            Debug.Log($"Doing action {data.ToString()}");

            GameData.OperationTime -= data.Time;
            _map.TickNodes(data.Time);

            switch(action)
            {
                case GameAction.Mine:
                    DoMineAction(data);
                    break;
                case GameAction.Collect:
                    DoCollectFuel(data);
                    break;
                case GameAction.Build:
                    DoBuildRefinery(data);
                    break;
            }
        }

        // Action completed now allow user to select node, retract to home or game over if they cannot travel
        if(false == _player.TravelActionValid(GameData))
        {
            GameOver();
        }
        else if(false == GameData.HasOperationTimeLeft)
        {
            Debug.Log("Ran out of travel time. Anomaly starting...");
            ShowRouteSelectionScreen();
            StartAnomaly();
        }
        else if(action == GameAction.Travel)
        {
            GameData.Action = GameAction.Travel;
            GameData.State = GameState.ConfiguringAction;

            ShowRouteSelectionScreen();
        }
        else
        {
            ShowActionSelectionScreen();
        }
    }

    void DoMineAction(ActionData data)
    {
        GameData.Resources += data.Resources;
        _player.CurrentNode.Resources -= data.Resources;
    }

    void DoCollectFuel(ActionData data)
    {
        GameData.Fuel += data.Fuel;
        _player.CurrentNode.Fuel -= data.Fuel;
    }

    void DoBuildRefinery(ActionData data)
    {
        if(_player.CurrentNode.AddBuilding())
        {
            GameData.Resources += data.Resources;
        }
    }
}
