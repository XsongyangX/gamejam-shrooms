using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PointController : MonoBehaviour
{
    public GameManager gameManager;
    public List<GameObject> ListeVoisin;
    public int positionN;
    
    bool isSelected, isHighlighted;

    // cutting algorithm
    public bool visited;

    public void CreatePoint(int n, GameManager gm)
    {
        positionN = n;
        gameManager = gm;
    }

    // Start is called before the first frame update
    void Start()
    {
        isHighlighted = false;
    }

    public void AddVoisin(GameObject aVoisin)
    {
        ListeVoisin.Add(aVoisin);
    }


    // Update is called once per frame
    void Update()
    {
    
    }

    // mouse click
    void OnMouseDown()
    {
        if(TurnManager.currentTurn == TurnManager.TurnType.MUSHROOM && !Tutorial.active)
        {
            if (isHighlighted)
            {
                //GetComponent<TileBehaviour>().type = TileBehaviour.Type.MUSHROOM;
                gameManager.ExpandMushroom(this);
                Tutorial.main.OnFirstSpread();
                //GetComponent<TileBehaviour>().type = TileBehaviour.Type.CORE;
            }
            else if(GetComponent<TileBehaviour>().type == TileBehaviour.Type.MUSHROOM || GetComponent<TileBehaviour>().type == TileBehaviour.Type.CORE)
            {
                isSelected = true;
                gameManager.TileSelect(this);
            }
        }

    }
    public void Select()
    {
        isSelected = true;
        // Selection visuelle
        GetComponent<TileBehaviour>().Highlight(Color.blue);
        if (GetComponent<TileBehaviour>().type == TileBehaviour.Type.CORE)
        {
            Tutorial.main.OnClickCore();
        }
    }

    public void Deselect()
    {
        isSelected = false;
        // Déselection visuelle
        GetComponent<TileBehaviour>().RemoveHighlight();
    }

    public void HighlightVoisin()
    {
        foreach(GameObject aTile in ListeVoisin)
        {
            if(aTile.GetComponent<TileBehaviour>().type != TileBehaviour.Type.MUSHROOM)
            {
                if (aTile.GetComponent<TileBehaviour>().actionCost <= TurnManager.mushroomActionPointsAmount)
                {
                    aTile.GetComponent<PointController>().HighlightSelf();
                    aTile.GetComponent<PointController>().isHighlighted = true;
                }
                else
                {
                    aTile.GetComponent<PointController>().HighlightSelfRed();
                    aTile.GetComponent<PointController>().isHighlighted = false;
                }
            }
        }
    }
    public void NormalVoisin()
    {
        foreach (GameObject aTile in ListeVoisin)
        {
            aTile.GetComponent<PointController>().isHighlighted = false;
            aTile.GetComponent<PointController>().NormalSelf();
        }

    }
    public void HighlightSelf()
    {
        Color hc = Color.red;
        if (TurnManager.CurrentTeamHasEnoughActionPoints(GetComponent<TileBehaviour>().actionCost)) hc = Color.cyan;
        GetComponent<TileBehaviour>().Highlight(hc);
        // Highlight
    }
    public void NormalSelf()
    {
        GetComponent<TileBehaviour>().RemoveHighlight();
        // Normal
    }
    public void HighlightSelfRed()
    {
        // Red
    }
}
