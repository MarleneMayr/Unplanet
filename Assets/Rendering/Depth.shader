Shader "FullScreen/Depth"
{
	Properties{
_VisRange("Visibility Range", float) = 1
	}

		HLSLINCLUDE

#pragma vertex Vert

#pragma target 4.5
#pragma only_renderers d3d11 playstation xboxone vulkan metal switch

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

		// The PositionInputs struct allow you to retrieve a lot of useful information for your fullScreenShader:
		// struct PositionInputs
		// {
		//     float3 positionWS;  // World space position (could be camera-relative)
		//     float2 positionNDC; // Normalized screen coordinates within the viewport    : [0, 1) (with the half-pixel offset)
		//     uint2  positionSS;  // Screen space pixel coordinates                       : [0, NumPixels)
		//     uint2  tileCoord;   // Screen tile coordinates                              : [0, NumTiles)
		//     float  deviceDepth; // Depth from the depth buffer                          : [0, 1] (typically reversed)
		//     float  linearDepth; // View space Z coordinate                              : [Near, Far]
		// };

		// To sample custom buffers, you have access to these functions:
		// But be careful, on most platforms you can't sample to the bound color buffer. It means that you
		// can't use the SampleCustomColor when the pass color buffer is set to custom (and same for camera the buffer).
		// float4 SampleCustomColor(float2 uv);
		// float4 LoadCustomColor(uint2 pixelCoords);
		// float LoadCustomDepth(uint2 pixelCoords);
		// float SampleCustomDepth(float2 uv);

		// There are also a lot of utility function you can use inside Common.hlsl and Color.hlsl,
		// you can check them out in the source code of the core SRP package.


		float _VisRange;

	float4 FullScreenPass(Varyings varyings) : SV_Target
	{
		UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(varyings);
		float depth = LoadCameraDepth(varyings.positionCS.xy);
		PositionInputs posInput = GetPositionInput(varyings.positionCS.xy, _ScreenSize.zw, depth, UNITY_MATRIX_I_VP, UNITY_MATRIX_V);
		float3 viewDirection = GetWorldSpaceNormalizeViewDir(posInput.positionWS);
		float4 color = float4(0.0, 0.0, 0.0, 0.0);

		// Load the camera color buffer at the mip 0 if we're not at the before rendering injection point
		if (_CustomPassInjectionPoint != CUSTOMPASSINJECTIONPOINT_BEFORE_RENDERING)
			color = float4(CustomPassLoadCameraColor(varyings.positionCS.xy, 0), 1);

		// Add your custom pass code here
		depth *= 50;
		float depthfactor = -2 * depth + 1;
		depthfactor = clamp(depth, 0, 1) * _VisRange;
		/*if (depth < 0.5) {
			color = float4(depth, depth, depth, 1);
		}*/

		// Fade value allow you to increase the strength of the effect while the camera gets closer to the custom pass volume
		//float f = 1 - abs(_FadeValue * 2 - 1);
		//return float4((color.rgb + f), color.a);

		float3 col = (color.rgb * (depthfactor)) + (float3(0, 0.05, 0.4) * (1 - depthfactor));
		return float4(col, 1) *depthfactor * 2;
	}

		ENDHLSL

		SubShader
	{
		Pass
		{
			Name "Custom Pass 0"

			ZWrite Off
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off

			HLSLPROGRAM
				#pragma fragment FullScreenPass
			ENDHLSL
		}
	}
	Fallback Off
}
