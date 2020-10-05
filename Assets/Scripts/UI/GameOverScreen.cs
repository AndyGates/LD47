using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    Text _reason;

    public void SetReason(string reason)
    {
        _reason.text = reason;
    }
}
