using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionData
{
    public GameAction ActionType;
    public string Text;
    public int Time;
    public int Fuel;
    public int Resources;

    public override string ToString()
    {
        return $"{ActionType}, Text={Text}, Time={Time}, Fuel={Fuel}, Resources={Resources}";
    }
}
