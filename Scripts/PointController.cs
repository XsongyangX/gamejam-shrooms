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
        if(isHighlighted)
        {
            gameManager.ExpandMushroom(this);
        }
        else if(GetComponent<TileBehaviour>().type == TileBehaviour.Type.MUSHROOM)
        {
            isSelected = true;
            Debug.Log("Mooshroom");
            gameManager.TileSelect(this);
        }

    }
    public void Select()
    {
        isSelected = true;
        // Selection visuelle
        GetComponent<TileBehaviour>().Highlight(Color.blue);
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
                aTile.GetComponent<PointController>().isHighlighted = true;
                aTile.GetComponent<PointController>().HighlightSelf();
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
        GetComponent<TileBehaviour>().Highlight(Color.cyan);
        // Highlight
    }
    public void NormalSelf()
    {
        GetComponent<TileBehaviour>().RemoveHighlight();
        // Normal
    }

}
