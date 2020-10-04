using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontFix : MonoBehaviour
{
    [SerializeField]
    Font _font;

    void Awake()
    {
        _font.material.mainTexture.filterMode = FilterMode.Point;
    }
}
