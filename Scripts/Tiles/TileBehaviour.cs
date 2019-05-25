using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public enum Type { EMPTY, CITY, MUSHROOM, CORE};

    public PointController point;

    public Type type = Type.EMPTY;

    public GameObject visualEmpty;
    public GameObject visualMushroom;
    public GameObject visualCity;
    public GameObject visualCore;

    private GameObject currentVisual;

    public int actionCost
    {
        get
        {
            switch (type)
            {
                case Type.EMPTY:
                    return 1;
                case Type.CITY:
                    return 3;
                case Type.MUSHROOM:
                    return 4;
                case Type.CORE:
                    return 4;
                default:
                    return 1;
            }
        }
    }
    
    void Start()
    {
        point = GetComponent<PointController>();

        RefreshVisual();
        //Highlight(Color.cyan);
    }

    private void RefreshVisual()
    {
        if (currentVisual != null) Destroy(currentVisual);
        GameObject newVisual;
        switch (type)
        {
            case Type.EMPTY:
                newVisual = visualEmpty;
                break;
            case Type.CITY:
                newVisual = visualCity;
                break;
            case Type.MUSHROOM:
                newVisual = visualMushroom;
                break;
            case Type.CORE:
                newVisual = visualMushroom;
                break;
            default:
                newVisual = visualEmpty;
                break;
        }
        currentVisual = Instantiate(newVisual);
        currentVisual.transform.SetParent(transform);
        currentVisual.transform.position = transform.position;
    }

    public void SetType(Type newType)
    {
        if (type == Type.CITY)
        {
            TurnManager.cityProgress -= TurnManager.cityTileWeight;
        }
        else if (type == Type.MUSHROOM)
        {
            TurnManager.mushroomProgress -= TurnManager.mushroomTileWeight;
        }

        type = newType;

        if (newType == Type.CITY)
        {
            TurnManager.cityProgress += TurnManager.cityTileWeight;
        }
        else if (newType == Type.MUSHROOM)
        {
            TurnManager.mushroomProgress += TurnManager.mushroomTileWeight;
        }

        // Update Visuals here
        RefreshVisual();
        //Highlight(Color.cyan);
    }

    public void Colonise(TileBehaviour from)
    {
        if (from.point.ListeVoisin.Contains(gameObject)) // Ils sont voisins
        {
            if (TurnManager.CurrentTeamHasEnoughActionPoints(actionCost)) // Assez de points d'action
            {
                TurnManager.SpendActionPoints(actionCost);
                SetType(from.type);
                //Transition de une tuile a une autre
            }
        }
    }

    public void Highlight(Color color)
    {
        LitObject[] obj = GetComponentsInChildren<LitObject>();
        foreach (var i in obj)
        {
            i.ApplyHighlight(color);
        }
    }

    public void RemoveHighlight()
    {
        LitObject[] obj = GetComponentsInChildren<LitObject>();
        foreach (var i in obj)
        {
            i.RemoveHighlight();
        }
    }
}
