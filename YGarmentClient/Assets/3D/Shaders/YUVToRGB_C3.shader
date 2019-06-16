Shader "Hidden/YUVtoRGB_C3"
{
	Properties
	{
		_MainTexY("Y", 2D) = "black" {}
		_MainTexU("U", 2D) = "gray" {}
		_MainTexV("V", 2D) = "gray" {}
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
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
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTexY;
			sampler2D _MainTexU;
			sampler2D _MainTexV;


			fixed3 YUVtoRGB(fixed3 c)
			{
				fixed3 rgb;
				rgb.r = c.x + c.z * 1.13983;
				rgb.g = c.x + dot(fixed2(-0.39465, -0.58060), c.yz);
				rgb.b = c.x + c.y * 2.03211;
				return rgb;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed y = tex2D(_MainTexY, i.uv).a;
				fixed u = tex2D(_MainTexU, i.uv).a;
				fixed v = tex2D(_MainTexV, i.uv).a;
				fixed4 yuv = fixed4(y,u,v,1.0);
				fixed4 rgb = fixed4(YUVtoRGB(yuv.rgb), yuv.a);
				return rgb;
			}
			ENDCG
		}
	}
}