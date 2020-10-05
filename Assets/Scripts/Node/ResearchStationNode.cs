using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchStationNode : MonoBehaviour
{
    [SerializeField]
    Animator _anim;

    [SerializeField]
    float _maxAnimTime = 30;

    [SerializeField]
    float _minAnimTime = 10;

    float _nextAnimTime = 0;

    void UpdateAnimTime()
    {
        _nextAnimTime = Time.time + Random.Range(_minAnimTime, _maxAnimTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateAnimTime();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > _nextAnimTime)
        {
            _anim.SetTrigger("OneShot");
            UpdateAnimTime();
        }
    }
}
