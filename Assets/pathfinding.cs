using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathfinding : MonoBehaviour
{
    public Transform seeker, player;
    grid grid;

    private void Start()
    {
        grid = GetComponent<grid>();
    }

    private void Update()
    {
        findpath(seeker.position, player.position);
    }
    void findpath(Vector3 start, Vector3 end)
    {
        node startnode = grid.nodefromworldpoint(start);
        node endnode = grid.nodefromworldpoint(end);

        List<node> openset = new List<node>();
        HashSet<node> closedset = new HashSet<node>();

        openset.Add(startnode);

        while(openset.Count > 0)
        {
            node currentnode = openset[0];
            for(int i = 1; i<openset.Count; i++)
            {
                if(openset[i].fcost < currentnode.fcost)
                {
                    currentnode = openset[i];
                }
            }
            openset.Remove(currentnode);
            closedset.Add(currentnode);

            if(currentnode == endnode)
            {
                retrace(startnode, endnode);
                return;
            }
            foreach(node n in grid.neighbournodes(currentnode))
            {
                if (!n.walkable || closedset.Contains(n))
                {
                    continue;
                }
                int movcost = currentnode.gcost + Getdistance(currentnode, n);
                if(movcost < n.gcost || !openset.Contains(n))
                {
                    n.gcost = movcost;
                    n.hcost = Getdistance(n, endnode);
                    n.parent = currentnode;
                    if (!openset.Contains(n))
                    {
                        openset.Add(n);
                    }
                }

            }
        }
    }

    void retrace(node a, node b)
    {
        List<node> path = new List<node>();
        node currentnode = b;
        
        while(currentnode != a)
        {
            path.Add(currentnode);
            currentnode = currentnode.parent;
        }
        path.Reverse();

        grid.path = path; 

    }

    int Getdistance(node a, node b)
    {
        int distx = Mathf.Abs(a.x - b.x);
        int disty = Mathf.Abs(a.y - b.y);

        if (distx > disty)
            return 14 * disty + 10 * (distx - disty);
        return 14 * distx + 10 * (disty - distx);
    }
}
