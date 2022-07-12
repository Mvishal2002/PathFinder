using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid : MonoBehaviour
{

    node[,] Grid;
    public LayerMask unwalkable;
    public Vector2 gridworldsize;
    public float noderadius;
    float nodedia;
    int gridsizex;
    int gridsizey;

    private void Start()
    {
        nodedia = noderadius * 2;
        gridsizex = (int)(gridworldsize.x / nodedia);
        gridsizey = (int)(gridworldsize.y / nodedia);
    }

    private void Update()
    {
        creategrid();
    }

    void creategrid()
    {
        Grid = new node[gridsizex, gridsizey];
        Vector3 bottomleft = transform.position - Vector3.right * gridworldsize.x / 2 - Vector3.forward * gridworldsize.y / 2;

        for(int i = 0; i<gridsizex; i++)
        {
            for(int j = 0; j<gridsizey; j++)
            {
                Vector3 worldpoint = bottomleft + Vector3.right * (i * nodedia + noderadius) + Vector3.forward * (j * nodedia + noderadius);
                bool walkable = !(Physics.CheckSphere(worldpoint, noderadius, unwalkable)) ;
                Grid[i, j] = new node(walkable, worldpoint, i, j);
            }
        }
    }

    public List<node> neighbournodes(node current)
    {
        List<node> neighbours = new List<node>();

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                if (current.x + i >= 0 && current.x + i < gridsizex && current.y + j >= 0 && current.y + j < gridsizey)
                {
                    neighbours.Add(Grid[current.x + i, current.y + j]);
                }
            }
        }
        return neighbours;
    }

    public node nodefromworldpoint(Vector3 position)
    {
        float percentx = (position.x + gridworldsize.x / 2) / gridworldsize.x;
        float percenty = (position.z + gridworldsize.y / 2) / gridworldsize.y;

        percentx = Mathf.Clamp01(percentx);
        percenty = Mathf.Clamp01(percenty);

        int x = (int)(gridsizex * percentx);
        int y = (int)(gridsizey * percenty);
        return Grid[x, y];
    }

    public List<node> path;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridworldsize.x, 1, gridworldsize.y));
        if(Grid != null)
        {
            
            foreach (node n in Grid)
            {              
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                {
                    if(path.Contains(n))
                    { 
                        Gizmos.color = Color.cyan;
                    }
                    
                }
                Gizmos.DrawCube(n.worldpos, Vector3.one * (nodedia));
                
            }       
        }
    }


}
