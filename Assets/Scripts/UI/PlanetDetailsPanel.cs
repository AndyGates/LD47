using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetDetailsPanel : MonoBehaviour
{
    [SerializeField]
    Text _fuelText = null;

    [SerializeField]
    Text _resourcesText = null;

    [SerializeField]
    Text _buildingSpacesText = null;

    [SerializeField]
    Text _nameText = null;

    public void SetData(int fuel, int resources, int buildingSpaces, string name)
    {
        _fuelText.text = fuel.ToString();
        _resourcesText.text = resources.ToString();
        _buildingSpacesText.text = buildingSpaces.ToString();
        _nameText.text = name;
    }
}
