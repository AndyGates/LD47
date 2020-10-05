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
    GameOverScreen _gameOverScreen = null;

    [SerializeField]
    GameObject _winnersScreen = null;

    int _startNodeId = 0;
    TravelCost _activeTravelCost = null;
    Route _activeRoute = null;

    GameStateData GameData { get; set; } = new GameStateData();
        
    Dictionary<GameAction, ActionData> _actionMap; 

    void Awake()
    {
        _actionMap = _actionDefinitions.ToDictionary(x => x.ActionType, x => x);
        GameData.RepairCost = -_actionMap[GameAction.Repair].Resources; // Will be negative if it is a cost

        _map.LoadMap(_mapData);
        _map.NodeSelected += OnNodeSelected;
        _map.NodeHover += OnNodeHover;

        _player.SetAtNodeId(_startNodeId);
        _player.TravelComplete += OnPlayerTravelComplete;
        _player.RetractHomeComplete += OnPlayerAnomalyComplete;

        _map.MarkNodeRoutesAsDiscovered(_startNodeId);

        _actionSelectionScreen.ActionSelected += ApplyAction;

        _playerHUD.GameState = GameData;
        _nodeOverviewUI.GameData = GameData;
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
        _gameOverScreen.gameObject.SetActive(false);
        _winnersScreen.SetActive(false);

        _map.ResetAll();
        _player.SetAtNodeId(_startNodeId);
        _map.MarkNodeRoutesAsDiscovered(_startNodeId);
    }

    public void RestartGame()
    {
        StartGame();
    }

    public void GameOver(string reason)
    {
        Debug.Log($"GameOver: {reason}");

        _playerHUD.gameObject.SetActive(false);
        _routeSelectionScreen.gameObject.SetActive(false);
        _actionSelectionScreen.gameObject.SetActive(false);
        _startScreen.SetActive(false);
        _gameOverScreen.gameObject.SetActive(true);
        _winnersScreen.SetActive(false);

        _gameOverScreen.SetReason(reason);
    }

    public void Win()
    {
        _playerHUD.gameObject.SetActive(false);
        _routeSelectionScreen.gameObject.SetActive(false);
        _actionSelectionScreen.gameObject.SetActive(false);
        _startScreen.SetActive(false);
        _gameOverScreen.gameObject.SetActive(false);
        _winnersScreen.SetActive(true);
    }

    void OnNodeSelected(Node node)
    {
        if(GameData.State == GameState.ConfiguringAction && GameData.Action == GameAction.Travel)
        {
            // Dont travel to a node we are already on just open the action screen
            if(node.Id == _player.CurrentNodeId)
            {
                GameData.State = GameState.ChoosingAction;
                ShowActionSelectionScreen();
            }
            else
            {
                TravelCost cost = _player.CalculateTravelCost(node);

                _activeRoute = _player.TravelToNode(node, cost.Time);
                _activeTravelCost = cost;
                
                GameData.State = GameState.RunningAction;

                _map.TickNodes(cost.Time);

                Debug.Log($"Traveling to {node.Name} with cost {cost.ToString()}");
            }
        }
    }

    void OnNodeHover(Node node, bool exited)
    {
        _nodeOverviewUI.gameObject.SetActive(!exited);
        _nodeOverviewUI.SetNode(node);

        if(_activeRoute == null)
        {
            TravelCost cost = _player.CalculateTravelCost(node);
            _nodeOverviewUI.SetTravelCost(cost);
        }
        else
        {
            _nodeOverviewUI.SetTravelCost(null);
        }
    }

    void OnPlayerTravelComplete()
    {
        Debug.Log($"Travel completed. {GameData.OperationTime}s of travel time left");

        if(_map.GetNodeType(_player.CurrentNodeId) == _goalNodeType)
        {
            // Make sure we could afford the travel before winning
            if(GameData.CanAffordTravel(_activeTravelCost))
            {
                Debug.Log("Well done you escaped the loop");
                GameData.State = GameState.Complete;
                Win();
            }
            else
            {
                if(false == GameData.HasEnoughHealth(_activeTravelCost.Health))
                {
                    GameOver(GameOverReasons.NoHealth);
                }
                else if(false == GameData.HasEnoughFuel(_activeTravelCost.Fuel))
                {
                    GameOver(GameOverReasons.NoFuel);
                }
                if(false == GameData.HasEnoughTime(_activeTravelCost.Time))
                {
                    Debug.Log("So close but the anomaly got you. Anomaly starting...");
                    ShowRouteSelectionScreen();
                    StartAnomaly();
                }
            }
        }
        else
        {
            if(GameData.HasEnoughTime(_activeTravelCost.Time))
            {
                GameData.ApplyTravelCost(_activeTravelCost);
                _activeTravelCost = null;
                _map.MarkNodeRoutesAsDiscovered(_player.CurrentNodeId);

                _activeRoute.State = RouteState.Traveled;
                _player.OnDiscoverNode(_player.CurrentNode);
            }
            else
            {
                GameData.OperationTime = 0;
            }

            _activeRoute = null;

            OnActionCompleted(GameAction.Travel);
        }
    }

    List<ActionData> GetAvailableActions(List<GameAction> nodeActions, Node node)
    {
        nodeActions.Add(GameAction.Repair);

        //Can always leave
        List<ActionData> actions = new List<ActionData>();

        foreach(GameAction ga in nodeActions)
        {
            if(_actionMap.ContainsKey(ga))
            {
                ActionData data = new ActionData(_actionMap[ga]);

                //Only add the action if the player has time
                if(GameData.OperationTime >= data.Time)
                {
                    bool shouldAdd = true;

                    switch(ga)
                    {
                        //Only add these actions if the player has the resources
                        case GameAction.Build:
                            shouldAdd = GameData.Resources >= -data.Resources;
                            break;

                        case GameAction.Collect:
                            data.Fuel = Mathf.Min(data.Fuel, node.Fuel);
                            break;

                        case GameAction.Repair:
                            shouldAdd = GameData.Resources >= -data.Resources && GameData.Health < GameData.StartHealth;
                            break;

                        case GameAction.Mine:
                            shouldAdd = node.Resources >= data.Resources;
                            break;
                    }
                    
                    if(shouldAdd)
                    {
                        actions.Add(data);
                    }
                }
            }
            else
            {
                Debug.LogError($"Could not find action rule for {ga}");
            }
        }

        actions.Add(_actionMap[GameAction.ViewRoutes]);
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
        List<ActionData> availableActions = GetAvailableActions(nodeActions, _player.CurrentNode);

        _actionSelectionScreen.Show(availableActions, _player.CurrentNode);
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
        if(_actionMap.ContainsKey(action) && action != GameAction.ViewRoutes)
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
                case GameAction.Repair:
                    DoRepair(data);
                    break;
            }

            OnActionCompleted(action);
        }
        else if(action == GameAction.ViewRoutes)
        {
            ShowRouteSelectionScreen();

            GameData.Action = GameAction.Travel;
            GameData.State = GameState.ConfiguringAction;
        }
    }
    void OnActionCompleted(GameAction action)
    {
        Debug.Log($"Action {action} completed");

        // Action completed now allow user to select node, retract to home or game over if they cannot travel
        string gameOverReason = _player.CanContinue(GameData);
        if(false == string.IsNullOrEmpty(gameOverReason))
        {
            GameOver(gameOverReason);
        }
        else if(false == GameData.HasOperationTimeLeft)
        {
            Debug.Log("Ran out of travel time. Anomaly starting...");
            ShowRouteSelectionScreen();
            StartAnomaly();
        }
        else
        {
            GameData.State = GameState.ChoosingAction;
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

    void DoRepair(ActionData data)
    {
        GameData.Resources += data.Resources;
        GameData.Health = Mathf.Min(GameData.Health + data.Health, GameData.MaxHealth);
    }
}
