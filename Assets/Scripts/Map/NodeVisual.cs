using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeVisual : MonoBehaviour
{
    [SerializeField]
    GameObject _outline;

    void Awake()
    {
        if(_outline == null)
        {
            enabled = false;
        }
        else
        {
            _outline.SetActive(false);
        }
    }

    void OnMouseOver()
    {
        _outline.SetActive(true);
    }

    void OnMouseExit()
    {
        _outline.SetActive(false);
    }
}
