﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    Animator _anim;

    [SerializeField]
    Map _map = null;

    [SerializeField]
    int _traveledTravelFactor = 1;

    [SerializeField]
    int _untraveledTravelFactor = 3;

    [SerializeField]
    float _retractHomeTravelSpeed = 2.0f;

    [SerializeField]
    AudioClip _anomalySound;

    [SerializeField]
    AudioClip _respawnSound;

    AudioSource _engineAudio;
    AudioSource _oneShotAudio;

    public event System.Action TravelComplete;
    public event System.Action RetractHomeComplete;

    public Node CurrentNode { get; private set; }
    public int CurrentNodeId { get => CurrentNode.Id; }
    int _homeNodeId = 0;

    iTween.EaseType _easeMethod = iTween.EaseType.linear;

    void Awake()
    {
        //Sorry
        _engineAudio = GameObject.FindGameObjectWithTag("EngineAudio").GetComponent<AudioSource>();
        _oneShotAudio = GameObject.FindGameObjectWithTag("OneShotAudio").GetComponent<AudioSource>();
    }

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
        return route.TravelTime * CalculateTravelFactor(route);
    }

    public int CalculateDamageCost(Route route)
    {
        return route.HealthCost * CalculateTravelFactor(route);
    }

    public int CalculateTravelFactor(Route route)
    {
        if(route == null)
        {
            return 1;
        }

        int travelFactor = _traveledTravelFactor;
        if(route.State == RouteState.Untraveled)
        {
            travelFactor = _untraveledTravelFactor;
        }

        return travelFactor;
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
                Health = CalculateDamageCost(route)
            };
        }
        return cost;
    }

    public Route TravelToNode(Node node, float travelTime)
    {
        Route route = _map.FindRoute(CurrentNodeId, node.Id);

        Vector3 to = _map.GetNodeCoords(node.Id);

        transform.right = (to - transform.position).normalized;

        iTween.MoveTo(gameObject, iTween.Hash(
            "position", (Vector3)to, 
            "time", travelTime, 
            "oncomplete", nameof(OnTravelComplete),
            "easeType", _easeMethod)
        );

        CurrentNode = node;
        _anim.SetBool("EnginesOn", true);
        iTween.AudioTo(_engineAudio.gameObject, 1, 1, 0.5f);
                
        return route;
    }

    public void OnDiscoverNode(Node node)
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

    public void Respawn()
    {
        if(_oneShotAudio != null && _respawnSound != null)
        {
            _oneShotAudio.PlayOneShot(_respawnSound);
        }

        _anim.SetTrigger("Respawn");
    }

    public void StartAnomaly()
    {
        CurrentNode = _map.FindNode(_homeNodeId);
        
        if(_oneShotAudio != null && _anomalySound != null)
        {
            _oneShotAudio.PlayOneShot(_anomalySound);
        }

        _anim.SetTrigger("Anomaly");

        iTween.MoveTo(gameObject, iTween.Hash(
            "delay", 0.5f,
            "position", (Vector3)CurrentNode.Coords, 
            "speed", _retractHomeTravelSpeed, 
            "oncomplete", nameof(OnAnomalyComplete),
            "easeType", _easeMethod)
        );
    }

    public void OnTravelComplete()
    {
        _anim.SetBool("EnginesOn", false);
        iTween.AudioTo(_engineAudio.gameObject, 0, 1, 0.5f);

        transform.localRotation = Quaternion.identity;
        TravelComplete.Invoke();
    }

    public void OnAnomalyComplete()
    {
        Respawn();
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
    }
}
