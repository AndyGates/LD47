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
    public int Health;

    public ActionData(){}

    public ActionData(ActionData data)
    {
        ActionType = data.ActionType;
        Text = data.Text;
        Time = data.Time;
        Fuel = data.Fuel;
        Resources = data.Resources;
        Health = data.Health;
    }

    public override string ToString()
    {
        return $"{ActionType}, Text={Text}, Time={Time}, Fuel={Fuel}, Resources={Resources}";
    }
}
