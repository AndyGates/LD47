using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    [SerializeField]
    Camera _camera = null;

    [SerializeField]
    float _scrollRatio = 0.9f; // 0.9f will be 10% of the cameras movement

    void Update()
    {
        transform.position = new Vector3(_camera.transform.position.x * _scrollRatio, 0.0f, 0.0f);
    }
}
