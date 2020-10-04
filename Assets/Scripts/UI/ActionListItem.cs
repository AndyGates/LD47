using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionListItem : MonoBehaviour
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
        Selected?.Invoke();
    }
}
