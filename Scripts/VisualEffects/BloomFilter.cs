using System;
using UnityEngine;

namespace Game.Effects.Filters
{

    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class BloomFilter : MonoBehaviour
    {

        static Shader shader;
        static string shader_rsrc = "Hidden/BloomFilter";
        [NonSerialized]
        private Material material;
        

        private RenderTexture tempDestination = null;
        private int handleCopy;
        private int handleMain;

        public bool debug;
        [Range(1, 16)]
        public int iterations = 1;
        [Range(0, 1)]
        public float softThreshold = 0.5f;
        [Range(0, 10)]
        public float intensity = 1;

        const int BoxDownPrefilterPass = 0;
        const int BoxDownPass = 1;
        const int BoxUpPass = 2;
        const int ApplyBloomPass = 3;
        const int DebugBloomPass = 4;

        [Range(0, 10)]
        public float threshold = 1;

        void Start()
        {
            if (shader == null)
            {
                shader = Shader.Find(shader_rsrc); //Resources.Load<ComputeShader>(shader_rsrc);
            }
            if (null == shader)
            {
                Debug.Log("Shader missing.");
                enabled = false;
                return;
            }
            material = new Material(shader);

            //handleCopy = shader.FindKernel("Copy");
            //handleMain = shader.FindKernel("BloomMain");

            if (handleMain < 0)
            {
                Debug.Log("Initialization failed.");
                enabled = false;
                return;
            }
        }

        void OnDestroy()
        {
            if (null != tempDestination)
            {
                tempDestination.Release();
                tempDestination = null;
            }
        }

        RenderTexture[] textures = new RenderTexture[16];

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (shader == null)
            {
                shader = Shader.Find(shader_rsrc); //Resources.Load<ComputeShader>(shader_rsrc);
            }
            if (material == null)
            {
                material = new Material(shader);
            }
            if (null == shader || material == null || null == source)
            {
                Graphics.Blit(source, destination); // just copy
                return;
            }

            float knee = threshold * softThreshold;
            Vector4 filter;
            filter.x = threshold;
            filter.y = filter.x - knee;
            filter.z = 2f * knee;
            filter.w = 0.25f / (knee + 0.00001f);
            material.SetVector("_Filter", filter);
            material.SetFloat("_Intensity", Mathf.GammaToLinearSpace(intensity));

            int width = source.width / 2;
            int height = source.height / 2;
            RenderTextureFormat format = source.format;

            RenderTexture currentDestination = textures[0] =
                RenderTexture.GetTemporary(width, height, 0, format);

            Graphics.Blit(source, currentDestination, material, BoxDownPrefilterPass);
            RenderTexture currentSource = currentDestination;
            int i = 1;
            for (; i < iterations; i++)
            {
                width /= 2;
                height /= 2;
                if (height < 2)
                {
                    break;
                }
                currentDestination = textures[i] =
                RenderTexture.GetTemporary(width, height, 0, format);
                Graphics.Blit(currentSource, currentDestination, material, BoxDownPass);
                currentSource = currentDestination;
            }
            for (i -= 2; i >= 0; i--)
            {
                currentDestination = textures[i];
                textures[i] = null;
                Graphics.Blit(currentSource, currentDestination, material, BoxUpPass);
                RenderTexture.ReleaseTemporary(currentSource);
                currentSource = currentDestination;
            }
            if (debug)
            {
                Graphics.Blit(currentSource, destination, material, DebugBloomPass);
            }
            else
            {
                material.SetTexture("_SourceTex", source);
                Graphics.Blit(currentSource, destination, material, ApplyBloomPass);
            }
            RenderTexture.ReleaseTemporary(currentSource);
        }
    }
}