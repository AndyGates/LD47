using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeVisual : MonoBehaviour
{
    [SerializeField]
    Color _color = Color.yellow;

    [SerializeField]
    Color _hoverColor = Color.cyan;

    SpriteRenderer _renderer = null;

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

        _color = _renderer.color;
    }

    void OnMouseOver()
    {
        _renderer.color = _hoverColor;
        Debug.Log("Mopuse ");
    }

    void OnMouseExit()
    {
        _renderer.color = _color;
    }
}
