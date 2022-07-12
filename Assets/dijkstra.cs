using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dijkstra : MonoBehaviour
{
    grid grid;
    public Transform user;
    public Transform target;

    struct item
    {
        public node n;
        public int dist;
    }

    private void Start()
    {
        grid = GetComponent<grid>();
    }

    private void Update()
    {
        findpath(user.position, target.position);
    }
    void findpath (Vector3 start, Vector3 end)
    {
        node startnode = grid.nodefromworldpoint(start);
        node endnode = grid.nodefromworldpoint(end);

        List<node> visited = new List<node>();
        List<item> minPQ = new List<item>() ;

        item sta = new item();
        sta.n = startnode;
        sta.dist = 0;
        

        minPQ.Add(sta);

        while(minPQ.Count>0)
        {
            item currentnode = new item();
            currentnode = minPQ[0];        

            for(int i = 1; i<minPQ.Count; i++)
            {
                if(minPQ[i].dist < currentnode.dist)
                {
                    currentnode.dist = minPQ[i].dist;
                    currentnode.n = minPQ[0].n;
                }
            }

            minPQ.Remove(currentnode);
            visited.Add(currentnode.n);

            if(currentnode.n == endnode)
            {
                retracepath(startnode, endnode);
                return;
            }

            foreach(node node in grid.neighbournodes(currentnode.n))
            {
                bool inlist = false;
                if(!visited.Contains(node) && node.walkable)
                {
                    item neighbour = new item();
                    neighbour.n = node;
                    node.parent = currentnode.n;
                    neighbour.dist = currentnode.dist + Getdistance(currentnode.n, node);
                    for(int i = 0; i<minPQ.Count; i++)
                    {
                        if(neighbour.n == minPQ[i].n)
                        {
                            inlist = true;
                            if(minPQ[i].dist > neighbour.dist)
                            {
                                minPQ.Remove(minPQ[i]);
                                minPQ.Add(neighbour);
                            }
                        }
                    }
                    if(inlist == false)
                    {
                        minPQ.Add(neighbour);
                    }
                    
                }
            }


        }     
    }

    int Getdistance(node a, node b)
    {
        int distx = Mathf.Abs(a.x - b.x);
        int disty = Mathf.Abs(a.y - b.y);

        if (distx > disty)
            return 14 * disty + 10 * (distx - disty);
        return 14 * distx + 10 * (disty - distx);
    }

    void retracepath(node a, node b)
    {
        List<node> path = new List<node>();
        node currentnode1 = b;

        while (currentnode1 != a)
        {
            path.Add(currentnode1);
            currentnode1 = currentnode1.parent;
        }
        path.Reverse();

        grid.path = path;

    }




}
