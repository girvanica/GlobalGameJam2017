Shader "Custom/FogOfWar" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_FogRadius("FogRadius", Float) = 1.0
		_FogMaxRadius("FogMaxRadius", Float) = 0.5
		_PlayerPos("_PlayerPos", Vector) = (0,0,0,1)
	}

	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert alpha:blend

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float2 location;
		};

        half _Glossiness;
		half _Metallic;
        fixed4     _Color;
		float     _FogRadius;
		float     _FogMaxRadius;
		float4     _PlayerPos;

		float powerForPos(float4 pos, float2 nearVertex);

		void vert(inout appdata_full vertexData, out Input outData) {
			float4 pos = mul(UNITY_MATRIX_MVP, vertexData.vertex);
			float4 posWorld = mul(unity_ObjectToWorld, vertexData.vertex);
			outData.uv_MainTex = vertexData.texcoord;
			outData.location = posWorld.xz;
		}

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			float alpha = (1.0 - (baseColor.a + powerForPos(_PlayerPos, IN.location)));

			o.Albedo = baseColor.rgb;
			o.Alpha = alpha;
		}

		float powerForPos(float4 pos, float2 nearVertex) {
			float atten = clamp(_FogRadius - length(pos.xz - nearVertex.xy), 0.0, _FogRadius);

			return (1.0 / _FogMaxRadius)*atten / _FogRadius;
		}

		ENDCG
	}

	Fallback "Transparent/VertexLit"
}
