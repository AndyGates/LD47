using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject _target = null;

    [SerializeField]
    float _scrollTrigger = 2.5f;

    Camera _camera = null;

    Vector3 _defaultPos = Vector3.zero;

    void Awake()
    {
        _camera = GetComponent<Camera>();

        _defaultPos = transform.position;
    }

    void Update()
    {
        if(_target.transform.position.x > _scrollTrigger)
        {
            transform.position = new Vector3(_defaultPos.x + (_target.transform.position.x - _scrollTrigger), _defaultPos.y, _defaultPos.z);
        }
        else
        {
            // HACKS to handle player reset
            transform.position = _defaultPos;
        }
    }
}
