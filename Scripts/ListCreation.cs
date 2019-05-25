using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListCreation : MonoBehaviour
{
    public GameObject pointPrefab;
    public GameObject gameManager;
    public List<GameObject> listePoint;
    public List<GameObject> listeVoisinTemp;

    public float distanceX, distanceY;
    public int tilesPerRow, numberOfRow, tileAmount;
    
    [System.Serializable]
    public struct StartPoints
    {
        public int index;
        public TileBehaviour.Type type;
    }

    public StartPoints[] startPoints;

    // Start is called before the first frame update
    void Start()
    {
        float row = 0;
        float height = 0;
        bool pairRow = true;
        tileAmount = tilesPerRow * numberOfRow;
        for (int i = 1; i < tileAmount + 1; i++)
        {

            GameObject aPoint = Instantiate(pointPrefab, new Vector3(row * distanceX, height * distanceY, 0),Quaternion.identity);
            aPoint.GetComponent<PointController>().CreatePoint(i,gameManager.GetComponent<GameManager>());
            listePoint.Add(aPoint);

            TileBehaviour tb = aPoint.GetComponent<TileBehaviour>();

            for (int k = 0; k < startPoints.Length; k++)
            {
                if (startPoints[k].index == i)
                {
                    tb.SetType(startPoints[k].type);
                }
            }

            height++;
            if (i % tilesPerRow == 0)
            {
                row++;
                height = 0f;
                if (pairRow)
                    height = 0.5f;
                pairRow = !pairRow;
            }
        }


        for (int i = 0; i < tileAmount; i++)
        {
            double targetRow;
            double result = listePoint[i].GetComponent<PointController>().positionN  / (double)tilesPerRow;

            double currentRow = Math.Ceiling(result);

            // case n - 1
            if (i - 1 >= 0)
            {
                result = listePoint[i - 1].GetComponent<PointController>().positionN / (double)tilesPerRow;
                targetRow = Math.Ceiling(result);
                if(currentRow == targetRow)
                {
                    listePoint[i].GetComponent<PointController>().AddVoisin(listePoint[i - 1]);
                }
            }
            // case n + 1
            if (i + 1 < tileAmount)
            {
                result = listePoint[i + 1].GetComponent<PointController>().positionN / (double)tilesPerRow;
                targetRow = Math.Ceiling(result);
                if (currentRow == targetRow)
                {
                    listePoint[i].GetComponent<PointController>().AddVoisin(listePoint[i + 1]);
                }
            }
            //case n - k && n - k + 1
            
            if(currentRow % 2 == 0)
            {
                if (i - tilesPerRow >= 0)
                {
                    // case n - k
                    result = listePoint[i - tilesPerRow].GetComponent<PointController>().positionN / (double)tilesPerRow;
                    targetRow = Math.Ceiling(result);
                    if (currentRow - 1 == targetRow)
                    {
                        listePoint[i].GetComponent<PointController>().AddVoisin(listePoint[i - tilesPerRow]);
                    }
                    // case n - k + 1
                    result = listePoint[i - tilesPerRow + 1].GetComponent<PointController>().positionN / (double)tilesPerRow;
                    targetRow = Math.Ceiling(result);

                    if (currentRow - 1 == targetRow)
                    {
                        listePoint[i].GetComponent<PointController>().AddVoisin(listePoint[i - tilesPerRow + 1]);
                    }
                }
            }
            else
            {
                if (i - tilesPerRow >= 0)
                {
                    // case n - k
                    result = listePoint[i - tilesPerRow -1].GetComponent<PointController>().positionN / (double)tilesPerRow;
                    targetRow = Math.Ceiling(result);
                    if (currentRow - 1 == targetRow)
                    {
                        listePoint[i].GetComponent<PointController>().AddVoisin(listePoint[i - tilesPerRow - 1]);
                    }

                    // case n - k + 1
                    result = listePoint[i - tilesPerRow].GetComponent<PointController>().positionN / (double)tilesPerRow;
                    targetRow = Math.Ceiling(result);

                    if (currentRow - 1 == targetRow)
                    {
                        listePoint[i].GetComponent<PointController>().AddVoisin(listePoint[i - tilesPerRow]);
                    }
                }
            }
            if (currentRow % 2 == 0)
            {
                if (i + tilesPerRow < tileAmount)
                {
                    // case n + k
                    result = listePoint[i + tilesPerRow].GetComponent<PointController>().positionN / (double)tilesPerRow;
                    targetRow = Math.Ceiling(result);

                    if (currentRow + 1 == targetRow)
                    {
                        listePoint[i].GetComponent<PointController>().AddVoisin(listePoint[i + tilesPerRow]);
                    }

                    // case n - k + 1
                    result = listePoint[i + tilesPerRow + 1].GetComponent<PointController>().positionN / (double)tilesPerRow;
                    targetRow = Math.Ceiling(result);

                    if (currentRow + 1 == targetRow)
                    {
                        listePoint[i].GetComponent<PointController>().AddVoisin(listePoint[i + tilesPerRow + 1]);
                    }
                }
            }
            else
            {
                if (i + tilesPerRow < tileAmount)
                {
                    // case n + k
                    result = listePoint[i + tilesPerRow - 1].GetComponent<PointController>().positionN / (double)tilesPerRow;
                    targetRow = Math.Ceiling(result);
                    if (currentRow + 1 == targetRow)
                    {
                        listePoint[i].GetComponent<PointController>().AddVoisin(listePoint[i + tilesPerRow - 1]);
                    }

                    // case n - k + 1
                    result = listePoint[i + tilesPerRow].GetComponent<PointController>().positionN / (double)tilesPerRow;
                    targetRow = Math.Ceiling(result);

                    if (currentRow + 1 == targetRow)
                    {
                        listePoint[i].GetComponent<PointController>().AddVoisin(listePoint[i + tilesPerRow]);
                    }
                }
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
