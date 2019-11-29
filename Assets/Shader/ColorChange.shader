Shader "Hidden/ColorChange"
{
	Properties
	{
		_Width("Image width",int) = 350
		_Height("Image height",int) = 100
	}
	SubShader
	{
		// No culling or depth
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
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			
			uint _Count;
			uint _Width;
			uint _Height;
			fixed4 _Color[36];

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;
				
				col = _Color[((i.uv.x) * _Width) / (_Width / _Count)];
				
				return col;
			}
			ENDCG
		}
	}
}
