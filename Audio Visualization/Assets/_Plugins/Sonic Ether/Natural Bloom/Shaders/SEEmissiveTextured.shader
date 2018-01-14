Shader "Sonic Ether/Emissive/Textured" {
Properties {
	_EmissionColor ("Emission Color", Color) = (1,1,1,1)
	_DiffuseColor ("Diffuse Color", Color) = (1, 1, 1, 1)
	_MainTex ("Diffuse Texture", 2D) = "White" {}
	_Illum ("Emission Texture", 2D) = "white" {}
	_EmissionGain ("Emission Gain", Range(0, 1)) = 0.5
}

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200
	
CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
sampler2D _Illum;
fixed4 _DiffuseColor;
fixed4 _EmissionColor;
float _EmissionGain;

struct Input {
	float2 uv_MainTex;
	float2 uv_Illum;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 c = tex * _DiffuseColor;
	o.Albedo = c.rgb;
	o.Emission = _EmissionColor * tex2D(_Illum, IN.uv_Illum).rgb * (exp(_EmissionGain * 10.0f));
	o.Alpha = c.a;
}
ENDCG
} 
FallBack "Self-Illumin/VertexLit"
}
