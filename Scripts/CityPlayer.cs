using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityPlayer : MonoBehaviour
{

    private List<PointController> territory = new List<PointController>();
    public GameObject visualSkyscraper;

    public GameObject gameManager;

    private System.Random random = new System.Random();

    public void AddTerritory(PointController point)
    {
        territory.Add(point);
    }

    public void RemoveTerritory(PointController point)
    {
        territory.Remove(point);
    }

    public void Play()
    {
        StartCoroutine("DecideAll");
    }

     IEnumerator DecideAll()
    {
        while (TurnManager.cityActionPointsAmount > 0)
        {
            Decide();
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void Decide()
    {
        // no territory, exception
        if (territory.Count == 0) throw new System.Exception();

        // list all possible neighbors to expand to
        List<GameObject> possibilities = new List<GameObject>();
        foreach(PointController element in territory)
        {
            bool citySurround = true;

            foreach (GameObject neighbor in element.ListeVoisin)
            {
                bool visited = neighbor.GetComponent<PointController>().visited;
                if (neighbor.GetComponent<TileBehaviour>().type != TileBehaviour.Type.CITY)
                    citySurround = false;
                if (true == visited) continue;

                if (neighbor.GetComponent<TileBehaviour>().type != TileBehaviour.Type.CITY)
                {
                    possibilities.Add(neighbor);
                    neighbor.GetComponent<PointController>().visited = true;
                }
            }
            if (citySurround && !element.GetComponent<PointController>().isSkyscraper)
            {
                element.GetComponent<TileBehaviour>().RefreshVisual(visualSkyscraper);
                element.GetComponent<PointController>().isSkyscraper = true;
            }
            //Method to change appearence

        }

        // unvisit
        foreach (PointController element in territory)
        {
            foreach (GameObject neighbor in element.ListeVoisin)
            {
                neighbor.GetComponent<PointController>().visited = false;
            }
        }

        if (possibilities.Count == 0) throw new System.Exception("possibilities are empty");

        int randomIndex = random.Next(possibilities.Count);
        gameManager.GetComponent<GameManager>().ExpandCity(possibilities[randomIndex].GetComponent<TileBehaviour>(), territory[0].GetComponent<TileBehaviour>());
        
    }

    // disconnection alg

    public List<GameObject> listMushrooms = new List<GameObject>();
    

    // Returns a list of disconnected tiles from the source given a cut
    // uses the list of mushrooms above as a way to know what is now no longer connected to the source
    public List<GameObject> Disconnect()
    {

        if (listMushrooms.Count == 0) throw new System.Exception("Empty mushroom list");



        // get all nodes assuming they are not visited

        // DFS on all the nodes
        Stack<GameObject> stack = new Stack<GameObject>();
        stack.Push(listMushrooms[0]); // source

        while (stack.Count != 0)
        {
            GameObject CurrentTile = stack.Pop();
            CurrentTile.GetComponent<PointController>().visited = true;
            foreach(GameObject aVoisin in CurrentTile.GetComponent<PointController>().ListeVoisin)
            {
                if(aVoisin.GetComponent<TileBehaviour>().type == TileBehaviour.Type.MUSHROOM && !aVoisin.GetComponent<PointController>().visited)
                {
                    stack.Push(aVoisin);
                }
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


        return disconnected;
    }
}
