// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Water v-2"
{
	Properties
	{
		_Color("Water Color", Color) = (1, 1, 1, 1)
		_EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
		_DepthFactor("Depth factor", float) = 1.0
		_WaveSpeed("Wave Speed", float) = 1.0 
		_WaveAmp("Wave Amplitude", float) = 1.0		
		_DepthMapTexture("Ramp Texture", 2D) = "white" {}
		_MainTex("Main Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "Queue"="Transparent" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			
			float4 _Color;
			float4 _EdgeColor;
			float _DepthFactor;
			float _WaveSpeed;
			float _WaveAmp;
			sampler2D _NoiseTex;
			sampler2D _CameraDepthTexture;
			sampler2D _MainTex;
						
			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 texCoord : TEXCOORD0;
			};
			
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 screenPos : TEXCOORD1;
				float3 texCoord : TEXCOORD0;
			};
			
			vertexOutput vert(vertexInput input)
			{
				vertexOutput output;
				output.pos = UnityObjectToClipPos(input.vertex);
				// convert to camera clip space
				output.pos = UnityObjectToClipPos(input.vertex);
				// apply wave animation
				float noiseSample = tex2Dlod(_NoiseTex, float4(input.texCoord.xy, 0, 0));
				output.pos.y += sin(_Time[2]*_WaveSpeed*noiseSample)*_WaveAmp;
				output.pos.x += cos(_Time[2]*_WaveSpeed*noiseSample)*_WaveAmp;
				output.screenPos = ComputeScreenPos(output.pos);
				output.texCoord = input.texCoord;
				return output;
			}
			
			float4 frag(vertexOutput input) : COLOR
			{
				float4 depthSample = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, input.screenPos);
				float depth = LinearEyeDepth(depthSample).r;
				// apply the DepthFactor to be able to tune at what depth values
				// the foam line actually starts
				float foamLine = 1 - saturate(_DepthFactor * (depth - input.screenPos.w));
				// multiply the edge color by the foam factor to get the edge,
				// then add that to the color of the water
				
				float4 albedo = tex2D(_MainTex, input.texCoord.xy);
				
				float4 col = _Color + foamLine * _EdgeColor * albedo;
				return col;
			}
			
			ENDCG
		}
	}
}
