using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectionAlgorithm
{

    // Returns a list of disconnected tiles from the source given a cut
    public static List<GameObject> Disconnect(GameObject source, GameObject cut)
    {
        // get all nodes assuming they are not visited
        
        // DFS on all the nodes
        Stack<GameObject> stack = new Stack<GameObject>();
        stack.Push(source);

        while (!stack.isEmpty())
        {

        }

        return null;
    }
}
