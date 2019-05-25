using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // the active selected tile
    public PointController selectedTile; // is null if nothing is selected

    // Start is called before the first frame update
    void Start()
    {
        selectedTile = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Perform a mouse selection
    public void TileSelect(PointController clickedTile)
    {

        if (selectedTile != null)
        {
            selectedTile.NormalVoisin();
            selectedTile.Deselect();
        }

        if (selectedTile != null && clickedTile.positionN == selectedTile.positionN)
        {
            selectedTile = null;
        }    
         selectedTile = clickedTile;
    }

    // Verify win-loss conditions
    public void CheckEndGame(TurnManager.TurnType currentPlayer)
    {

    }
}
