Shader "Custom/TileShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

		_Grass("Grass", 2D) = "white" {}
		_Earth("Earth", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Grass;
		uniform float4 _Grass_ST;
		sampler2D _Earth;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Grass;
			float2 uv_Earth;
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			float2 uv = TRANSFORM_TEX(IN.worldPos.xz, _Grass);
			fixed4 c1 = tex2D(_Grass, uv) * _Color;
			fixed4 c2 = tex2D(_Earth, uv) * _Color;
			
			fixed4 c = lerp(c1, c2, clamp(IN.worldPos.y / 5, 0, 1));

			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
