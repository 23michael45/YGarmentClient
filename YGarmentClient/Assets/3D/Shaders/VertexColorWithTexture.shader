﻿Shader "Unlit/VertexColorWithTexture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Blend ("Blend", Range (0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

		Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float4 color: Color; // Vertex color
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float4 color: Color; // Vertex color
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _Blend;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = i.color * _Blend + tex2D(_MainTex, i.uv) * (1 - _Blend);
                return col;
            }
            ENDCG
        }
    }
}
