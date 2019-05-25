using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightPoint : MonoBehaviour
{
    public static List<LightPoint> all = new List<LightPoint>();

    public Color color = Color.black;
    public float intensity = 1;
    public float radius = 3;

    private void Awake()
    {
        all.Add(this);
    }

    private void Start()
    {
        LitObject.RepaintAll();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    public Vector3 GetColor()
    {
        return new Vector3(color.r, color.g, color.b);
    }
    public Vector3 GetParams()
    {
        return new Vector3(intensity, radius);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(GetPosition(), radius);
    }
}
