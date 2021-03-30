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

texture waterMap;
sampler waterSampler = sampler_state
{
	Texture = (waterMap);
};

float GetHeight(float2 Coord)
{
	return tex2D(noiseSampler, Coord).r;
}

float2 WorldCoords(float2 coords)
{
	return coords / screenScale + screenPosition / screenSize;
}
float4 PixelShaderLight(float2 coords: TEXCOORD0) : COLOR0
{
  float4 waterMap = tex2D(waterSampler, coords);
  float2 coordsToWorld = WorldCoords(coords);
  float2 watercoords = float2(coordsToWorld.x, coordsToWorld.y);
  float4 color = tex2D(s0, coords + GetHeight(watercoords + float2(sin(Time/60),cos(Time/60)))* waterMap.a*0.01f);
  return color;
}
technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderLight();
	}
}