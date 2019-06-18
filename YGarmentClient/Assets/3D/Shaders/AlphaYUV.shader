Shader "Unlit/AlphaYUV"
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
				fixed y = c.r;
				fixed u = c.g;
				fixed v = c.b;
				//rgb.r = c.x + c.z * 1.13983;
				//rgb.g = c.x + dot(fixed2(-0.39465, -0.58060), c.yz);
				//rgb.b = c.x + c.y * 2.03211;

				rgb.r = 1.164*(y - 16. / 255.) + 1.596*(v - 128. / 255.);
				rgb.g = 1.164*(y - 16. / 255.) - 0.813*(v - 128. / 255.) - 0.391*(u - 128. / 255.);
				rgb.b = 1.164*(y - 16. / 255.) + 2.018*(u - 128. / 255.);
				return rgb;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed y = tex2D(_MainTexY, i.uv).a;
				fixed u = tex2D(_MainTexU, i.uv).a;
				fixed v = tex2D(_MainTexV, i.uv).a;
				fixed4 yuv = fixed4(y,u,v,1.0);
				//fixed4 yuv = fixed4(0,0,0, 1.0);
				fixed4 rgb = fixed4(YUVtoRGB(yuv.rgb), yuv.a);
				//fixed4 rgb = fixed4(1,0,0,1);
				
				//rgb.r = fixed(0);
				//rgb.g = fixed(0);
				//rgb.b = fixed(0);
				//rgb.a = fixed(0);
				//rgb.rgb = fixed3(1, 0, 0);
				return rgb;
			}
			ENDCG
		}
	}
}