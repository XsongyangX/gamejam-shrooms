using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityPlayer
{

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

    public static void Decide()
    {
        // no territory, exception
        if (territory.Count == 0) throw new System.Exception();

        // choose a random tile to expand from
        // repeatedly search if the tile has no possible expansion
        while (true)
        {
            int newMoveIndex = random.Next(territory.Count);
            Debug.Log("Territory " + territory.Count);
            PointController chosenPoint = territory[newMoveIndex];
            Debug.Log(chosenPoint.ListeVoisin);

            foreach (GameObject element in chosenPoint.ListeVoisin)
            {
                TileBehaviour tile = element.GetComponent<TileBehaviour>();

                // if the tile is not a city, expand
                if (tile.type != TileBehaviour.Type.CITY)
                {
                    gameManager.ExpandCity(tile, chosenPoint.GetComponent<TileBehaviour>());
                    return;
                }
            }
        }

        throw new System.Exception("AI failed to decided");

    }
}
