using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectionAlgorithm
{
    public static List<GameObject> listMushrooms = new List<GameObject>();

    // Returns a list of disconnected tiles from the source given a cut
    // uses the list of mushrooms above as a way to know what is now no longer connected to the source
    public static List<GameObject> Disconnect()
    {

        if (listMushrooms.Count == 0) throw new System.Exception("Empty mushroom list");

        Debug.Log("Disconnect begin");
        // get all nodes assuming they are not visited
        
        // DFS on all the nodes
        Stack<GameObject> stack = new Stack<GameObject>();
        stack.Push(listMushrooms[0]); // source

        while (stack.Count != 0)
        {
            Debug.Log("Looping in Disconnect");
            GameObject element = stack.Pop();
            PointController pc = element.GetComponent<PointController>();
            pc.visited = true;

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

        foreach (GameObject node in listMushrooms)
        {
            PointController pc = node.GetComponent<PointController>();
            if (pc.visited == false)
            {
                disconnected.Add(node);
            }
            else
                pc.visited = false;
        }


        Debug.Log("Disconnect end");
        return disconnected;
    }
}
