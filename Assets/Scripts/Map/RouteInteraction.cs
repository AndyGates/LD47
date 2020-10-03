using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteInteraction : MonoBehaviour
{
    public Route Route{ get; set; }
    public event System.Action<Route> RouteSelected;

    void OnMouseUpAsButton()
    {
        RouteSelected?.Invoke(Route);
    }
}
