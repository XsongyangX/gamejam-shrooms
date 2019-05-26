using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum TurnType { CITY, MUSHROOM};

    public static TurnType currentTurn = TurnType.MUSHROOM;

    public static int mushroomActionPointsAmount = 5;
    public static int cityActionPointsAmount = 0;

    public static int mushroomActionPointPerTurn = 5;
    public static int cityActionPointPerTurn = 5;

    public const int powerupTileWeight = 2;

    public static int mushroomTileCount = 0;
    public static int cityTileCount = 0;

    public static int GetCurrentTeamActionPoints()
    {
        if (currentTurn == TurnType.CITY) return cityActionPointsAmount;
        else return mushroomActionPointsAmount;
    }

    public static bool CurrentTeamHasEnoughActionPoints(int cost)
    {
        if (currentTurn == TurnType.MUSHROOM) return mushroomActionPointsAmount >= cost;
        else return cityActionPointsAmount >= cost;
    }

    public static void SpendActionPoints(int amount)
    {
        if (currentTurn == TurnType.MUSHROOM) mushroomActionPointsAmount -= amount;
        else cityActionPointsAmount -= amount;
    }


    public static void StartNewTurn()
    {
        if (currentTurn == TurnType.MUSHROOM)
        {
            Debug.Log("City Turn");
            currentTurn = TurnType.CITY;
            cityActionPointsAmount += cityActionPointPerTurn;
            Tutorial.main.OnFirstCityTurn();
            CityPlayer.Play();
        }
        else
        {
            Debug.Log("Mushroom Turn");
            currentTurn = TurnType.MUSHROOM;
            mushroomActionPointsAmount += mushroomActionPointPerTurn;
            
        }
        UIManager.UpdateTurnLabel();
    }

    private void Update()
    {
        CheckForNextTurn();
    }

    private void CheckForNextTurn()
    {
        if (!CurrentTeamHasEnoughActionPoints(1)) // Or skip turn
        {
            StartNewTurn();
            // Remove Highlight
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical(GUI.skin.box);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Skip all tutorial: ");
        Tutorial.skipAll = GUILayout.Toggle(Tutorial.skipAll, "");
        GUILayout.EndHorizontal();
        
        GUILayout.Label("Current Team Turn: " + currentTurn);
        GUILayout.Label("Remaining Action Points: " + GetCurrentTeamActionPoints());
        if (GUILayout.Button("Skip Turn"))
        {
            StartNewTurn();
        }
        GUILayout.Space(10);
        GUILayout.Label("Mushroom Tile Count: " + mushroomTileCount);
        GUILayout.Label("City Tile Count: " + cityTileCount);


        GUILayout.EndVertical();
    }
}
