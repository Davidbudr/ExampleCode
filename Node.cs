using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2 position;
    public float StartCost; //horizontal dist + vertical dist
    public float EndCost; // horizontal dist ^2 + Vertical dist ^2
    public float TotalCost; // startcost + endcost
    public Node MotherNode; // which node is its parent
}
