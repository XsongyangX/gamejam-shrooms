using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public Vector3 targetPosition;
    public Vector3 startPosition;
    public Vector3 targetScale;
    public float duration = 2;
    public float spawnDuration = 2;

    private float timer;
    
    private void UpdateMove()
    {
        if (timer >= duration)
        {
            timer = 0;
            Invoke("Shrink", 0);
        } else
        {
            timer += Time.deltaTime;
            float t = timer / spawnDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            Invoke("UpdateMove", 0);

        }
    }

    private void Grow()
    {
        if (timer >= spawnDuration)
        {
            timer = 0;
            Invoke("UpdateMove", 0);
        } else
        {
            timer += Time.deltaTime;
            float t = timer / spawnDuration;
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
            Invoke("Grow", 0);
        }
    }

    private void Shrink()
    {
        if (timer >= spawnDuration)
        {
            timer = 0;
            //Invoke("UpdateMove", 0);
            Destroy(gameObject);
        }
        else
        {
            timer += Time.deltaTime;
            float t = timer / spawnDuration;
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, 1-t);
            Invoke("Shrink", 0);
        }
    }

    public static void Spawn(Worker worker, Vector3 start, Vector3 end)
    {
        Worker w = Instantiate(worker.gameObject).GetComponent<Worker>();
        w.startPosition = start;
        w.targetPosition = end;

        Vector3 toTarget = end - start;
        w.transform.rotation = Quaternion.LookRotation(toTarget, Vector3.back);
        w.targetScale = w.transform.localScale;
        w.Invoke("Grow", 0);
    }
}
