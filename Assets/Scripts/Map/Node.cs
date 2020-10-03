using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int Id{ get; }

    public int Type{ get; }

    public Vector2 Coords{ get; }

    public Node(int id, int type, Vector2 coords)
    {
        Id = id;
        Type = type;
        Coords = coords;
    }

}
