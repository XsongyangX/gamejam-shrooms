Shader "LOCAL/Emit"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_MainColor("Color", color) = (1,1,1,1)
		_SpecularSmoothness("Smoothness", range(0,1)) = 0.5
		_SpecularIntensity("Specular", range(0,1)) = 0.7
		_EmitTex ("Emission Map", 2D) = "white" {}
		_EmitColor ("Color", color) = (1,1,1,1)
		_EmitIntensity ("Intensity", float) = 1
		_EmitFresnelWeight("Fresnel Weight", range(0,1)) = 0
		_EmitFresnelSize ("Fresnel Size", range(0,5)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

		UsePass "LOCAL/DefaultLit/Lit"

        Pass
        {
			Name "Emit"

			Blend One One
			ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD1;
				float3 normal : NORMAL;
            };

            sampler2D _EmitTex;
			float3 _EmitColor;
			float _EmitIntensity;
			float _EmitFresnelWeight;
			float _EmitFresnelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				float3 center = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
				o.normal = o.worldPos - center;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_EmitTex, i.uv);

				float3 view = i.worldPos - _WorldSpaceCameraPos;
				float fresnel = saturate(dot(normalize(i.normal), normalize(-view)));
				fresnel = pow(fresnel, _EmitFresnelSize);

				fresnel = saturate(lerp(1, fresnel, _EmitFresnelWeight));

				col.rgb *= _EmitColor;
                return col * _EmitIntensity * fresnel;
            }
            ENDCG
        }
    }
}
