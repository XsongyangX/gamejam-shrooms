Shader "LOCAL/DefaultLit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_MainColor ("Color", color) = (1,1,1,1)
		_SpecularSmoothness ("Smoothness", range(0,1)) = 0.5
		_SpecularIntensity ("Specular", range(0,1)) = 0.7
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
			Name "Lit"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			#include "UnityCG.cginc"
			#include "Lighting.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _MainColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.normal = mul((float3x3)unity_ObjectToWorld, v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _MainColor;

				float3 light = GetLighting(i.worldPos, i.normal);

				col.rgb *= light;

                return col;
            }
            ENDCG
        }

		Pass
		{
			Name "Highlight"

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				float3 normal : NORMAL;
			};

			float4 _HighlightColor;
			float _HighlightFactor = 0;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.normal = mul((float3x3)unity_ObjectToWorld, v.normal);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = fixed4(0,0,0,0);

				float3 view = i.worldPos - _WorldSpaceCameraPos;
				float fresnel = saturate(dot(normalize(i.normal), normalize(-view)));
				fresnel = 1 - pow(fresnel, 2);

				col.rgb = _HighlightColor.rgb;
				col.a = fresnel * _HighlightFactor;

				return col;
			}
			ENDCG
		}
    }
}
