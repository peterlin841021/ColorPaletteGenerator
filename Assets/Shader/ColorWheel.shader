Shader "Hidden/ColorWheel"
{
	Properties
	{
		_Radius("Palette radius",float) = 0.45
		//_BGColor("Background Color", Color) = (1,1,1,1)
		//_CircleSizePercent("Circle Size Percent", Range(0, 100)) = 50
		//_Border("Anti Alias Border Threshold", Range(0.00001, 5)) = 0.01		
	}
	SubShader
	{		
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);				
				o.uv =  v.uv - 0.5;								
				return o;
			}
			uint _Count;
			fixed4 _Color[36];
			//fixed4 _BGColor;
			float _Radius;
			//float _CircleSizePercent;
			//float _Border;
			float radius = 0.45;
			float PI = 3.14;
			
			/*float gray(float4 color) 
			{
				return 0.2125* color.r + 0.7154*color.g + 0.0721*color.b;
			}
			float Sobel(v2f i, float4 color)
			{
				const float Gx[9] = { -1,-2,-1,
									0, 0, 0,
									1, 2, 1 };

				const float Gy[9] = { -1, 0, 1,
								   -2, 0, 2,
								   -1, 0, 1 };
				float texColor;
				float edgeX = 0;
				float edgeY = 0;
				for (int it = 0; it < 9; it++) 
				{					
					texColor = gray(color);
					edgeX += texColor * Gx[it];
					edgeY += texColor * Gy[it];
				}
				float edge = 1 - abs(edgeX) - abs(edgeY);				
				return edge;
			}			*/
			
			/*float2 side = float2(0.5,0);
			float2 antialias(float radius, float borderSize, float dist)
			{
				float t = smoothstep(radius + borderSize, radius - borderSize, dist);
				return t;
			}
			float circle(float2 uv, float2 center)
			{
				return smoothstep(0.1,0.1, length(uv - center));
			}*/
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;
				float dist = length(i.uv);
				if (dist < 0.3)discard;				
				float theta = degrees(atan2(i.uv.y,i.uv.x));
								
				float pwidth = length(float2(ddx(dist), ddy(dist)));				
				float alpha = smoothstep(0.5, 0.5 - pwidth * 1.5, dist);
				if (alpha == 0)discard;

				if (theta < 0)
					theta = 360 + theta;				
				col = _Color[(theta) / (360. / _Count)];				
				return col;
			}
			ENDCG
		}
	}
}