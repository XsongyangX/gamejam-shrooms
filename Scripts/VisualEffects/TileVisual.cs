using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVisual : MonoBehaviour
{
    private Vector3 targetScale;
    private Vector3 targetPosition;
    public const float growthTime = 2;
    public float timer;

    private void Grow()
    {
        if (timer < growthTime)
        {
            float t = timer / growthTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
            transform.localPosition = Vector3.Lerp(Vector3.zero, targetPosition, t);
            timer += Time.deltaTime;
            Invoke("Grow", 0);
        }
        else
        {
            CancelInvoke("Grow");
        }
    }
    
    void Start()
    {
        timer = -3;
        targetScale = transform.localScale;
        targetPosition = transform.localPosition;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Random.Range(0, 180), 90, 90);
        Invoke("Grow", 0);
    }
}
