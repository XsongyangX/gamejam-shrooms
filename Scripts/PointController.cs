using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PointController : MonoBehaviour
{
    public GameManager gameManager;
    public List<GameObject> ListeVoisin;
    public int positionN;
    
    bool isSelected, isHighlighted;

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
        isSelected = true;
        

        gameManager.TileSelect(this);
        HighlightVoisin();

    }

    public void Deselect()
    {
        isSelected = false;
    }

    public void HighlightVoisin()
    {
        foreach(GameObject aTile in ListeVoisin)
        {
            aTile.GetComponent<PointController>().isHighlighted = true;
            aTile.GetComponent<PointController>().HighlightSelf();
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
        // Highlight
    }
    public void NormalSelf()
    {
        // Normal
    }

}
