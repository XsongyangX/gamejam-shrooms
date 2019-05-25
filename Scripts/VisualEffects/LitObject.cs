using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LitObject : MonoBehaviour
{
    private static List<LitObject> all = new List<LitObject>();

    public LightPoint[] lights;

    public Color currentHighlightColor = Color.cyan;
    private float highlightFactor = 0;

    private MaterialPropertyBlock mpb;
    private MeshRenderer[] renderers;

    private void Awake()
    {
        all.Add(this);

        lights = new LightPoint[8];
        //GetNearbyLights();
        renderers = GetComponentsInChildren<MeshRenderer>();
        mpb = new MaterialPropertyBlock();
        //ApplyLights();
    }

    void Start()
    {
        Repaint();
    }

    public void ApplyLights()
    {
        Vector4[] positions = new Vector4[8];
        Vector4[] colors = new Vector4[8];
        Vector4[] parameters = new Vector4[8];
        for (int i = 0; i < 8; i++)
        {
            if (lights[i] != null)
            {
                positions[i] = lights[i].GetPosition();
                colors[i] = lights[i].GetColor();
                parameters[i] = lights[i].GetParams();
            }
        }
        mpb.SetVectorArray("_LightPosition", positions);
        mpb.SetVectorArray("_LightColor", colors);
        mpb.SetVectorArray("_LightParams", parameters);

        RefreshRenderers();
    }

    private void RefreshRenderers()
    {
        foreach (var i in renderers)
        {
            i.SetPropertyBlock(mpb);
        }
    }

    public void GetNearbyLights()
    {
        for (int i = 0; i < LightPoint.all.Count; i++)
        {
            if (LightPoint.all[i] == null)
            {
                LightPoint.all.RemoveAt(i);
                i--;
            }
        }

        List<LightPoint> list = LightPoint.all;
        
        list.Sort(Comparer);
        for (int i = 0; i < 8; i++)
        {
            if (i >= list.Count)
            {
                lights[i] = null;
            } else
            {
                lights[i] = list[i];
            }

        }
    }

    public int Comparer(LightPoint a, LightPoint b)
    {
        if (Vector3.Distance(a.GetPosition(), transform.position) > Vector3.Distance(b.GetPosition(), transform.position))
        {
            return 1;
        } else
        {
            return -1;
        }
    }

    public void ApplyHighlight(Color color)
    {
        mpb.SetColor("_HighlightColor", color);
        mpb.SetFloat("_HighlightFactor", 1);
        RefreshRenderers();
    }

    public void RemoveHighlight()
    {
        mpb.SetFloat("_HighlightFactor", 0);
        RefreshRenderers();
    }

    private void OnDestroy()
    {
        all.Remove(this);
        RepaintAll();
    }

    public void Repaint()
    {
        GetNearbyLights();
        ApplyLights();
    }

    public static void RepaintAll()
    {
        foreach (var i in all)
        {
            if (i != null)
            {
                i.Repaint();
            }
        }
    }
}
