using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField]
    Text _health;

    [SerializeField]
    Text _resources;

    [SerializeField]
    Text _fuel;

    [SerializeField]
    Text _timeUntilAnomaly = null;

    GameStateData _gameState = null;

    public void SetGameState(GameStateData gameState)
    {
        _gameState = gameState;
    }

    void Update()
    {
        if(_gameState != null)
        {
            // Just update the ui every frame no matter what. Maybe add some events later.
            _health.text = _gameState.Health.ToString();
            _resources.text = _gameState.Resources.ToString();
            _fuel.text = _gameState.Fuel.ToString();

            _timeUntilAnomaly.text = _gameState.OperationTime.ToString();
        }
    }
}
