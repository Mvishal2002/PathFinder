using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class node 
{
    public bool walkable;
    public Vector3 worldpos;
    public int gcost, hcost ;
    public int x, y;
    public node parent; 
    public node(bool _walkable, Vector3 _worldpos, int _x, int _y)
    {
        walkable = _walkable;
        worldpos = _worldpos;
        x = _x;
        y = _y;
    }

    public int fcost
    {
        get
        {
            return gcost + hcost ;
        }
         
    }
}
