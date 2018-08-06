Shader "Lit/Diffuse With Ambient"
{
	Properties
	{
		[NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
	}
	
	SubShader
	{
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 diff : COLOR0;
				float4 vertex : SV_POSITION;
				float3 amb : AMB;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				o.diff = nl * _LightColor0;
				o.amb = ShadeSH9(half4(worldNormal,1));
				return o;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				if (length(i.diff) < 0.5) i.diff = 0;
				else if (length(i.diff) < 1) i.diff = 0.1;
				else i.diff = 0.2;
				col *= (i.diff + float4(i.amb, 0));
				return col;
			}
			ENDCG
		}
	}
}