using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityPlayer : MonoBehaviour
{
    public static CityPlayer main;

    private void Awake()
    {
        main = this;
    }
    private static List<PointController> territory = new List<PointController>();

    public static GameManager gameManager;

    private static System.Random random = new System.Random();

    public static void AddTerritory(PointController point)
    {
        territory.Add(point);
        Debug.Log("Added " + point.positionN);
    }

    public static void RemoveTerritory(PointController point)
    {
        territory.Remove(point);
        Debug.Log("Removed " + point.positionN);
    }

    public static void Play()
    {
        main.StartCoroutine("DecideAll");
    }

     IEnumerator DecideAll()
    {
        while (TurnManager.cityActionPointsAmount > 0)
        {
            Decide();
            Debug.Log("One Coroutine");
            yield return new WaitForSeconds(0.3f);
        }
    }

    public static void Decide()
    {
        // no territory, exception
        if (territory.Count == 0) throw new System.Exception();

        // list all possible neighbors to expand to
        List<GameObject> possibilities = new List<GameObject>();
        foreach(PointController element in territory)
        {
            foreach(GameObject neighbor in element.ListeVoisin)
            {
                bool visited = neighbor.GetComponent<PointController>().visited;
                if (visited) continue;

                if (neighbor.GetComponent<TileBehaviour>().type != TileBehaviour.Type.CITY)
                {
                    possibilities.Add(neighbor);
                    neighbor.GetComponent<PointController>().visited = true;
                }
            }
        }

        int randomIndex = random.Next(possibilities.Count);
        gameManager.ExpandCity(possibilities[randomIndex].GetComponent<TileBehaviour>(), territory[0].GetComponent<TileBehaviour>());
        
        foreach(GameObject p in possibilities)
        {
            p.GetComponent<PointController>().visited = false;
        }
    }
}
