using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightAmbient : MonoBehaviour
{
    public static LightAmbient main;

    public Color color;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        Shader.SetGlobalColor("_LightAmbient", color);
    }
}
