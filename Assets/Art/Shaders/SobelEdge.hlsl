//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

void Sobel_float(Texture2D image, SamplerState ss, float2 uv, float2 texelSize, float3x3 horizontal, float3x3 vertical, out float4 RGBA) {

	float4 hr = float4(0, 0, 0, 0);
	float4 vt = float4(0, 0, 0, 0);

	hr += image.Sample(ss, (uv + float2(-1.0, -1.0) * texelSize)) * 1.0;
	hr += image.Sample(ss, (uv + float2(0.0, -1.0) * texelSize)) * 0.0;
	hr += image.Sample(ss, (uv + float2(1.0, -1.0) * texelSize)) * -1.0;
	hr += image.Sample(ss, (uv + float2(-1.0, 0.0) * texelSize)) * 2.0;
	hr += image.Sample(ss, (uv + float2(0.0, 0.0) * texelSize)) * 0.0;
	hr += image.Sample(ss, (uv + float2(1.0, 0.0) * texelSize)) * -2.0;
	hr += image.Sample(ss, (uv + float2(-1.0, 1.0) * texelSize)) * 1.0;
	hr += image.Sample(ss, (uv + float2(0.0, 1.0) * texelSize)) * 0.0;
	hr += image.Sample(ss, (uv + float2(1.0, 1.0) * texelSize)) * -1.0;

	vt += image.Sample(ss, (uv + float2(-1.0, -1.0) * texelSize)) * 1.0;
	vt += image.Sample(ss, (uv + float2(0.0, -1.0) * texelSize)) * 2.0;
	vt += image.Sample(ss, (uv + float2(1.0, -1.0) * texelSize)) * 1.0;
	vt += image.Sample(ss, (uv + float2(-1.0, 0.0) * texelSize)) * 0.0;
	vt += image.Sample(ss, (uv + float2(0.0, 0.0) * texelSize)) * 0.0;
	vt += image.Sample(ss, (uv + float2(1.0, 0.0) * texelSize)) * 0.0;
	vt += image.Sample(ss, (uv + float2(-1.0, 1.0) * texelSize)) * -1.0;
	vt += image.Sample(ss, (uv + float2(0.0, 1.0) * texelSize)) * -2.0;
	vt += image.Sample(ss, (uv + float2(1.0, 1.0) * texelSize)) * -1.0;

	hr = sqrt((hr.r * hr.r) + (hr.g * hr.g) + (hr.b * hr.b));
	vt = sqrt((vt.r * vt.r) + (vt.g * vt.g) + (vt.b * vt.b));

	RGBA = sqrt(hr * hr + vt * vt);
}

#endif //MYHLSLINCLUDE_INCLUDED