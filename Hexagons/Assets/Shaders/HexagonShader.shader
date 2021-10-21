Shader "Unlit/Hexagon"{
	Properties{
		_Color ("Tint", Color) = (1, 1, 1, 1)
		_OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
		_OutlineWidth("Outline Width", Float) = 0.05
		_OutlineBlendOuter("Outline Blend Outer", Float) = 0.01
		_OutlineBlendInner("Outline Blend Inner", Float) = 0.01
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader{
		Tags{ 
			"RenderType"="Transparent" 
			"Queue"="Transparent"
		}

		Blend One OneMinusSrcAlpha


		ZWrite off
		Cull off

		Pass
		{
			CGPROGRAM

			#include "UnityCG.cginc"
			#include "HexagonHelp.cginc"

			#pragma vertex vert
			#pragma fragment frag

			fixed4 _OutlineColor;
			float _OutlineWidth;
			float _OutlineBlendOuter;
			float _OutlineBlendInner;

			fixed4 frag(v2f i) : SV_TARGET{
				float dist = hex(i.uv - 0.5);
				float outline = min(smoothstep(0.0, _OutlineBlendInner, dist * 3), smoothstep(_OutlineWidth, _OutlineWidth - _OutlineBlendOuter, dist * 3));

				return outline * _OutlineColor;
			}

			ENDCG
		}
		
		Pass
		{
			CGPROGRAM

			#include "UnityCG.cginc"
			#include "HexagonHelp.cginc"

			#pragma vertex vert
			#pragma fragment frag

			fixed4 _Color;
			float _OutlineBlendInner;

			fixed4 frag(v2f i) : SV_TARGET{
				float dist = hex(i.uv - 0.5);
				dist = smoothstep(_OutlineBlendInner, 0, dist);

				return dist * _Color;
			}

			ENDCG
		}
	}
}