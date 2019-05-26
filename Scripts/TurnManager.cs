using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum TurnType { CITY, MUSHROOM};

    public GameObject cityPlayer, gameManager;

    public int numberOfBuiltTilesForMusic = 10;

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


    public void StartNewTurn()
    {
        if (currentTurn == TurnType.MUSHROOM)
        {
            gameManager.GetComponent<GameManager>().selectedTile.NormalVoisin();
            gameManager.GetComponent<GameManager>().selectedTile.Deselect();

            Debug.Log("City Turn");
            currentTurn = TurnType.CITY;
            cityActionPointsAmount += cityActionPointPerTurn;
            Tutorial.main.OnFirstCityTurn();
            cityPlayer.GetComponent<CityPlayer>().Play();
        }
        else
        {
            Debug.Log("Mushroom Turn");
            currentTurn = TurnType.MUSHROOM;
            mushroomActionPointsAmount += mushroomActionPointPerTurn;
            
        }
        UIManager.UpdateTurnLabel();
    }

    public void Update()
    {
        if (!CurrentTeamHasEnoughActionPoints(1)) // Or skip turn
        {
            StartNewTurn();

            // Remove Highlight
            CheckForMusic();
        }
        CheckForEndGame();
    }

    private void CheckForEndGame()
    {
        if (cityTileCount == 0)
        {
            cityActionPointsAmount = 0;
            mushroomActionPointsAmount = 5;
            UnityEngine.SceneManagement.SceneManager.LoadScene("WinScreen");
        } else if (mushroomTileCount == 0 || TileBehaviour.core == null)
        {
            cityActionPointsAmount = 0;
            mushroomActionPointsAmount = 5;
            UnityEngine.SceneManagement.SceneManager.LoadScene("DefeatScreen");
        }
    }

    private int musicTension = 0;

    private void CheckForMusic()
    {
        if (musicTension == 0)
        {
            if (cityTileCount + mushroomTileCount >= numberOfBuiltTilesForMusic)
            {
                MusicManager.FadeInChannel(2, 2);
                MusicManager.FadeOutChannel(1, 2);
                SFXManager.PlayMusicTransition(0);
                musicTension++;
            }
        } else if (musicTension == 1)
        {
            if (cityTileCount + mushroomTileCount >= numberOfBuiltTilesForMusic * 2)
            {
                MusicManager.FadeInChannel(3, 2);
                MusicManager.FadeOutChannel(2, 2);
                SFXManager.PlayMusicTransition(0);
                musicTension++;
            }
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
