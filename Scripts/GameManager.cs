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
        selectedTile.Select();
        if (TurnManager.mushroomActionPoints != 0)
        {
            selectedTile.HighlightVoisin();
        }
    }

    public void ExpandMushroom(PointController clickedTile)
    {
        Debug.Log("Test");
        //clickedTile.GetComponent<TileBehaviour>().type = TileBehaviour.Type.MUSHROOM;
        selectedTile.NormalVoisin();
        selectedTile.Deselect();
        clickedTile.NormalVoisin();
        clickedTile.Deselect();
        clickedTile.gameObject.GetComponent<TileBehaviour>().Colonise(selectedTile.gameObject.GetComponent<TileBehaviour>());
    }

    // End game transition
    public void EndGame(TurnManager.TurnType winningPlayer)
    {
        switch (winningPlayer)
        {
            // LOSS
            case TurnManager.TurnType.CITY:

                break;
            
            // WIN
            case TurnManager.TurnType.MUSHROOM:

                break;
        }
    }
}
