Shader "Custom/Hightlight Scaling"
{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}
		_HighlightScaleFactor("Highlight Scale Factor", float) = 1.05
		_HighlightColor("Highlight Color", Color) = (1,1,1,1)
	}

	SubShader
	{
		Pass
		{ 
			Stencil {
				Ref 1
				Comp always
				Pass replace
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			fixed4 _Color;
			sampler2D _MainTex;

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			float4 _MainTex_ST;

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 texcol = tex2D(_MainTex, i.uv);
				return fixed4((texcol * _Color).xyz, 0);
			}
			ENDCG
		}

		Pass
		{
			Stencil {
				Ref 0
				Comp GEqual
				Pass replace
			}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			float _HighlightScaleFactor;
			float4 _HighlightColor;

			float4 vert(float3 inVertex : POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos(inVertex * _HighlightScaleFactor);
			}

			fixed4 frag(float4 highlightVertex : SV_POSITION) : SV_TARGET 
			{	
				return _HighlightColor;
			}
			ENDCG
		}
	}
}
