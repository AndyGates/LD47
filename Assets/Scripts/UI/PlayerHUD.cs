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

    public GameStateData GameState{ get; set; }

    void Update()
    {
        if(GameState != null)
        {
            // Just update the ui every frame no matter what. Maybe add some events later.
            _health.text = GameState.Health.ToString();
            _resources.text = GameState.Resources.ToString();
            _fuel.text = GameState.Fuel.ToString();

            _timeUntilAnomaly.text = GameState.OperationTime.ToString();
        }
    }
}
