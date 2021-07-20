// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/pruebaShader" {
	SubShader {

		Pass
		{
			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members uv)
#pragma exclude_renderers d3d11
			#pragma vertex vert
			#pragma fragment frag

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXTCOOR0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float uv _ TEXCOOR0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				return float4(i.uv.r, i.uv.g, 0, 1);
			}

			ENDCG
		}
	}
}
