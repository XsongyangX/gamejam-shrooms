using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public enum Type { EMPTY, CITY, MUSHROOM, CORE, POWERUP};

    public PointController point;

    public Type type = Type.EMPTY;

    public static TileBehaviour core;

    public Worker mushroomWoorker;
    public GameObject visualEmpty;
    public GameObject visualMushroom;
    public GameObject visualCity;
    public GameObject visualCore;
    public GameObject visualPowerup;
    public GameObject vfxSpore;
    public GameObject vfxSporePowerup;

    public GameObject cityPlayer;

    private GameObject currentVisual;

    public int actionCost
    {
        get
        {
            switch (type)
            {
                case Type.EMPTY:
                    return 1;
                case Type.POWERUP:
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
                newVisual = visualCore;
                break;
            case Type.POWERUP:
                newVisual = visualPowerup;
                break;
            default:
                newVisual = visualEmpty;
                break;
        }
        RefreshVisual(newVisual);
        /*
        currentVisual = Instantiate(newVisual);
        currentVisual.transform.SetParent(transform);
        currentVisual.transform.position = transform.position;
        */
    }

    public void RefreshVisual(GameObject newVisual)
    {
        if (currentVisual != null) Destroy(currentVisual);
        currentVisual = Instantiate(newVisual);
        currentVisual.transform.SetParent(transform);
        currentVisual.transform.position = transform.position;
    }

    public void SetType(Type newType)
    {
        if (type == Type.CITY)
        {

            TurnManager.cityTileCount--;
            cityPlayer.GetComponent<CityPlayer>().RemoveTerritory(point);
        }
        else if (type == Type.MUSHROOM || type == Type.CORE)
        {

            TurnManager.mushroomTileCount--;
            cityPlayer.GetComponent<CityPlayer>().listMushrooms.Remove(gameObject);

        }
        else if (type == Type.POWERUP)
        {
            SpawnSporePowerupVFX();
            UIManager.ShowPowerup("More Action Points !!!", 3);
            Tutorial.main.OnFirstPowerUp();
            if (TurnManager.currentTurn == TurnManager.TurnType.MUSHROOM) TurnManager.mushroomActionPointPerTurn += TurnManager.powerupTileWeight;
            else TurnManager.cityActionPointPerTurn += TurnManager.powerupTileWeight;
        }

        if (newType == Type.CORE && core != this)
        {
            if (core == null) core = this;
            else newType = Type.MUSHROOM;
        }

        type = newType;

        if (newType == Type.CITY)
        {
            TurnManager.cityTileCount++;            
        }
        else if (newType == Type.MUSHROOM || type == Type.CORE)
        {
            TurnManager.mushroomTileCount++;
            SpawnSporeVFX();
            cityPlayer.GetComponent<CityPlayer>().listMushrooms.Add(gameObject);
        }

        // Update Visuals here
        RefreshVisual();
        //Highlight(Color.cyan);
    }

    public void Colonise(TileBehaviour from)
    {
        /*
        if (from.point.ListeVoisin.Contains(gameObject)) // Ils sont voisins
        {
            if (TurnManager.CurrentTeamHasEnoughActionPoints(actionCost)) // Assez de points d'action
            {
                TurnManager.SpendActionPoints(actionCost);
                SetType(from.type);
                //Transition de une tuile a une autre
            }
        }
        */

        TurnManager.SpendActionPoints(actionCost);
        SetType(from.type);

        // from a mushroom tile
        if (from.type == Type.MUSHROOM || from.type == Type.CORE) {
            Worker.Spawn(mushroomWoorker, from.transform.position, transform.position);
            SFXManager.Play(SFXManager.Style.COLONISE_SHROOM);
        }
        // from a human tile
        else
            SFXManager.Play(SFXManager.Style.COLONISE_HUMAN);

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

    private void SpawnVFX(GameObject vfx)
    {
        GameObject obj = Instantiate(vfx);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        //obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.identity;
    }

    public void SpawnSporeVFX()
    {
        SpawnVFX(vfxSpore);
    }

    public void SpawnSporePowerupVFX()
    {
        SpawnVFX(vfxSporePowerup);
    }
}
