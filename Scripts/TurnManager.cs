using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum TurnType { CITY, MUSHROOM};

    public static TurnType currentTurn = TurnType.MUSHROOM;

    public static int mushroomActionPoints = 5;
    public static int cityActionPoints = 5;

    public static float mushroomProgress = 3;
    public static float cityProgress = 2;

    public const float cityTileWeight = 0.1f;
    public const float mushroomTileWeight = 0.2f;

    public static int mushroomTileCount = 0;
    public static int cityTileCount = 0;

    public static int GetCurrentTeamActionPoints()
    {
        if (currentTurn == TurnType.CITY) return cityActionPoints;
        else return mushroomActionPoints;
    }

    public static bool CurrentTeamHasEnoughActionPoints(int cost)
    {
        if (currentTurn == TurnType.MUSHROOM) return mushroomActionPoints >= cost;
        else return cityActionPoints >= cost;
    }

    public static void SpendActionPoints(int amount)
    {
        if (currentTurn == TurnType.MUSHROOM) mushroomActionPoints -= amount;
        else cityActionPoints -= amount;
    }

    public static int GetPointsFromProgress(float progress)
    {
        return Mathf.FloorToInt(progress);
    }

    public static void StartNewTurn()
    {
        if (currentTurn == TurnType.MUSHROOM)
        {
            currentTurn = TurnType.CITY;
            mushroomActionPoints += GetPointsFromProgress(mushroomProgress);
        }
        else
        {
            currentTurn = TurnType.MUSHROOM;
            cityActionPoints += GetPointsFromProgress(cityProgress);
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
