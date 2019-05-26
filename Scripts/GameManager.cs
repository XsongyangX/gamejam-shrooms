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

    void Awake()
    {
        CityPlayer.gameManager = this;
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
        else
        {
            selectedTile = clickedTile;
            selectedTile.Select();
        
            if (TurnManager.mushroomActionPointsAmount != 0)
            {
                selectedTile.HighlightVoisin();
            }
        }
    }

    public void ExpandMushroom(PointController clickedTile)
    {
        selectedTile.NormalVoisin();
        selectedTile.Deselect();
        clickedTile.NormalVoisin();
        clickedTile.Deselect();
        clickedTile.gameObject.GetComponent<TileBehaviour>().Colonise(selectedTile.gameObject.GetComponent<TileBehaviour>());
    }

    public void ExpandCity(TileBehaviour chosenTile, TileBehaviour expandingFrom)
    {
        Debug.Log("Expanding enemy");
        chosenTile.Colonise(expandingFrom);
        CityPlayer.AddTerritory(chosenTile.GetComponent<PointController>());
        //algo
        List<GameObject> removeList = DisconnectionAlgorithm.Disconnect();
        foreach(GameObject aTile in removeList)
        {
            Debug.Log("A removal");
            aTile.GetComponent<TileBehaviour>().SetType(TileBehaviour.Type.EMPTY);
        }
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
