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

		Blend SrcAlpha OneMinusSrcAlpha

		ZWrite off
		Cull off

		Pass{

			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			float4 _MainTex_ST;

			fixed4 _Color;
			fixed4 _OutlineColor;
			float _OutlineWidth;
			float _OutlineBlendOuter;
			float _OutlineBlendInner;

			struct appdata{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			v2f vert(appdata v){
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}
			
			float greaterthan(float x, float y) {
                return max(sign(x - y), 0.0);
            }
            
            float lesserthan(float x, float y) {
                return max(sign(y - x), 0.0);
            }
			
            float hex(in float2 p){
                // Rotate 90 degrees
                float angle = 90 * (3.14159265 * 2 / 360);
                float rc = cos(angle);
                float rs = sin(angle);    
                p = float2(p.x * rc - p.y * rs, p.x * rs + p.y * rc);
                
                const float hexSize = (0.5 / 2);// * 0.866f; // 0.866 is the relationship between the circumradius and inradius
                const float2 s = float2(1, 1.7320508);
                
                p = abs(p);
                return (max(dot(p, s*.5), p.x) - hexSize);
            }

			fixed4 frag(v2f i) : SV_TARGET{
				float dist = hex((i.uv - 0.5));
				float outline = min(smoothstep(0.0, _OutlineBlendInner, dist * 3), smoothstep(_OutlineWidth, _OutlineWidth - _OutlineBlendOuter, dist * 3));
				dist = smoothstep(_OutlineBlendInner, 0, dist);
				//dist = lesserthan(_OutlineWidth, dist);

				return dist * _Color + outline * _OutlineColor;
			}

			ENDCG
		}
	}
}