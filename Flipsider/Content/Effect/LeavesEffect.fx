﻿#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float Time;

sampler s0: register(s0);

float2 screenPosition;

float2 screenSize;

float screenScale;

texture noiseMap;
sampler noiseSampler = sampler_state
{
	Texture = (noiseMap);
};

texture Map;
sampler MapSampler = sampler_state
{
	Texture = (Map);
};

float GetHeight(float2 Coord)
{
	return tex2D(noiseSampler, Coord).r;
}

float2 WorldCoords(float2 coords)
{
	return coords / screenScale + screenPosition / screenSize;
}

float2 WorldCoordsScaled(float2 coords, float2 scale)
{
	return (coords * scale) / screenScale + screenPosition / screenSize;
}

float2 Round(float2 coords, int accuracy)
{
	float pixelX = screenSize.x / accuracy;
	float pixelY = screenSize.y / accuracy;
	return float2(floor(coords.x * pixelX) / pixelX, floor(coords.y * pixelY) / pixelY);
}
float4 PixelShaderLight(float2 coords: TEXCOORD0) : COLOR0
{
	float pixelX = screenSize.x / 6;
	float pixelY = screenSize.y / 6;

	float Mag = 3;

	for (int i = 0; i < 10; i++)
	{
		Mag -= tex2D(MapSampler, Round(coords,2) + float2(0, i * -(2/screenSize.y))).a * 0.5f;
	}
	int PMag = max(Mag, 0);
	float2 WC = WorldCoords(coords);
	float dispa = sin(Time/(12))* PMag *0.3f;
	float2 SC = (coords + float2(dispa, dispa) * 0.003f);
	float4 MapColor = tex2D(MapSampler, SC);
	float4 color = tex2D(s0, coords);
	color += MapColor;
	return color;
}
technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderLight();
	}
}