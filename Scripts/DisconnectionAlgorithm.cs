using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectionAlgorithm
{

    // Returns a list of disconnected tiles from the source given a cut
    public static List<GameObject> Disconnect(List<GameObject> listNodes, GameObject source, GameObject cut)
    {
        // get all nodes assuming they are not visited
        
        // DFS on all the nodes
        Stack<GameObject> stack = new Stack<GameObject>();
        stack.Push(source);

        while (stack.Count != 0)
        {
            GameObject element = stack.Pop();
            PointController pc = element.GetComponent<PointController>();
            pc.visited = true;

            // check if the visited is the cut
            if (element == cut) break;

            foreach(GameObject neighbor in pc.ListeVoisin)
            {
                // push only if it is a mushroom tile
                TileBehaviour tb = neighbor.GetComponent<TileBehaviour>();
                if (tb.type == TileBehaviour.Type.MUSHROOM)
                    stack.Push(neighbor);
            }
        }

        // gather all unvisited nodes and set all nodes back to unvisited
        List<GameObject> disconnected = new List<GameObject>();

        foreach (GameObject node in listNodes)
        {
            PointController pc = node.GetComponent<PointController>();
            if (!pc.visited)
            {
                disconnected.Add(node);
            }
            else
                pc.visited = false;
        }
        
        
        return disconnected;
    }
}
