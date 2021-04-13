#if OPENGL
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
	float4 color = tex2D(s0, coords);
	float2 toWorld = WorldCoords(coords);
	float noiseStrip = GetHeight(float2(0, toWorld.x + Time/500)) * GetHeight(float2(0, toWorld.x - Time / 500));
	color.rgb += abs(sin(noiseStrip + toWorld.x*2))*max(0,(1 - toWorld.y/2));
	return color;
}
technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderLight();
	}
}