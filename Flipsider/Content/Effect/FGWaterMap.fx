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

texture waterShadeMap;
sampler waterShadeSampler = sampler_state
{
	Texture = (waterShadeMap);
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
float4 PixelShaderLight(float2 coords: TEXCOORD0) : COLOR0
{
  float2 coordsToWorld = WorldCoords(coords);
  float2 NoiseSample = float2(sin(Time / 560) +coordsToWorld.x, cos(Time / 560) + coordsToWorld.y - Time/60);
  float2 NoiseSample2 = float2(cos(Time / 260) - coordsToWorld.y, -sin(Time / 460) + coordsToWorld.x - Time / 120);
  float4 waterMap = tex2D(waterSampler, coords + float2(sin(coordsToWorld.y*30 - Time/2),0)*0.0025f*GetHeight(float2(0,Time/120 + coordsToWorld.y / 5)));
  float4 waterShadeMap = tex2D(waterShadeSampler, WorldCoordsScaled(coords, float2(100,100)) * GetHeight(NoiseSample) * GetHeight(NoiseSample2));
  float4 color = tex2D(s0, coords + GetHeight(coordsToWorld + float2(sin(Time / 60), cos(Time / 60)) - float2(cos(Time / 120), sin(Time / 120)) * screenScale) * waterMap.a*0.01f);
  color += waterMap * 0.5f - (waterMap.a * sin(coordsToWorld.y * 10 - Time*0.7f)*0.1f* GetHeight(float2(-Time / 240 + coordsToWorld.x,0)));
  color *=  1 + waterMap.a * 0.15f * GetHeight(float2(0, -Time / 240 + coordsToWorld.y / 5));
  return color;
}
technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderLight();
	}
}