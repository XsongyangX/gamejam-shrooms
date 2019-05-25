Shader "LOCAL/SporeParticule"
{
    Properties
    {
		_MainTex("Texture", 2D) = "white" {}
		_GradientTex("Gradient", 2D) = "white" {}
		_MainColor("Color Start", color) = (1,1,1,1)
		_EndColor("Color End", color) = (0,0,0,1)
		_Intensity("Intensity", float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent"}
        LOD 100
		ZWrite Off
		Blend One One

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
				float3 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float3 color : COLOR;
				float id : TEXCOORD1;
			};

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _MainColor;
			float4 _EndColor;
			float _Intensity;
			sampler2D _GradientTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv.xy;
				o.color = v.color;
				o.id = v.uv.z;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				if (col.a < 0.5) discard;
				if (col.r > i.color.r) discard;
				if (col.g < i.color.g) discard;
				float t = i.color.b;
				col.rgb = tex2D(_GradientTex, float2(i.id,0)).rgb * _Intensity;
                return col;
            }
            ENDCG
        }
    }
}
