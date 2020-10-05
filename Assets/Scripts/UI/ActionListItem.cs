using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionListItem : MonoBehaviour, IPointerEnterHandler
{
    public event System.Action Selected;

    [SerializeField]
    Image _fuelIcon = null;

    [SerializeField]
    Text _fuelText = null;

    [SerializeField]
    Image _resourcesIcon = null; 

    [SerializeField]
    Text _resourcesText = null;

    [SerializeField]
    Text _timeText = null;

    [SerializeField]
    Text _nameText = null;

    [SerializeField]
    AudioClip _hoverClip;

    [SerializeField]
    AudioClip _selectClip;

    AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GameObject.FindGameObjectWithTag("OneShotAudio").GetComponent<AudioSource>();
    }

    public void SetData(string name, int time, int fuel, int resources)
    {       
        _fuelIcon.enabled = fuel != 0;
        _fuelText.enabled = fuel != 0;
        _fuelText.text = fuel.ToString();

        _resourcesIcon.enabled = resources != 0;
        _resourcesText.enabled = resources != 0;
        _resourcesText.text = resources.ToString();

        _timeText.text = time.ToString();
        _nameText.text = name;
    }

    public void OnClicked()
    {
        if(_selectClip != null)
        {
            _audioSource.PlayOneShot(_selectClip);
        }

        Selected?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_hoverClip != null)
        {
            _audioSource.PlayOneShot(_hoverClip);
        }
    }
}
