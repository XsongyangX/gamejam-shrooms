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
            cityActionPoints += GetPointsFromProgress(cityProgress);
        } else
        {
            currentTurn = TurnType.MUSHROOM;
            mushroomActionPoints += GetPointsFromProgress(mushroomProgress);
        }
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
}
