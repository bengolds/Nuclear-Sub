Shader "Custom/ButtonShader" {
	Properties {
		_BaseColor ("Base Color", Color) = (1,1,1,1)
		_MarkingColor ("Marking Color", Color) = (0, 0, 0, 0)
		_MainTex ("Base Texture", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_EmissionColor("Emission", Color) = (0,0,0,1)
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

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _BaseColor;
		fixed4 _MarkingColor;
		fixed3 _EmissionColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			c = lerp(_MarkingColor, _BaseColor, c.r);
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Emission = step(0.5, c.r) * _EmissionColor;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
