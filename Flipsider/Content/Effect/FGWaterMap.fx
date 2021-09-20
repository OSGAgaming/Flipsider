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
sampler waterSampler = sampler_state
{
	Texture = (Map);
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

float2 Round(float2 coords, float accuracy)
{
	float pixelX = screenSize.x / accuracy;
	float pixelY = screenSize.y / accuracy;
	return float2(floor(coords.x * pixelX) / pixelX, floor(coords.y * pixelY) / pixelY);
}

float Round(float coords, float accuracy)
{
	float pixelX = screenSize.x / accuracy;
	return floor(coords.x * pixelX) / pixelX;
}

float4 PixelShaderLight(float2 coords: TEXCOORD0) : COLOR0
{

  float2 coordsToWorld = WorldCoords(coords);

  float2 RTW = Round(coordsToWorld, 2);

  float2 NoiseSample = float2(sin(Time / 560) + RTW.x, cos(Time / 560) + RTW.y - Time/60);
  float2 NoiseSample2 = float2(cos(Time / 260) - RTW.y, -sin(Time / 460) + RTW.x - Time / 120);
  float2 WaterSampleCoords = float2(sin(RTW.y * 30 - Time * 0.5f), 0) * 0.0025f * GetHeight(float2(0, Time / 120 + RTW.y / 5));
  float4 waterMap = tex2D(waterSampler, coords + WaterSampleCoords);
  float4 color = tex2D(s0, coords + GetHeight(coordsToWorld + float2(sin(Time / 60), cos(Time / 60)) - float2(cos(Time / 120), sin(Time / 120)) * screenScale) * waterMap.a*0.01f);
  color += waterMap * 0.5f - (waterMap.a * sin(RTW.y * 10 - Time*0.7f)*0.1f* GetHeight(float2(-Time / 240 + RTW.x,0)));

  float sampleCoords = GetHeight(float2(RTW.x * 5 - Time / 140, RTW.y/3 - Time / 40))* GetHeight(float2(RTW.x * 5 + Time / 140, RTW.y/3 - Time / 40));
  if (sampleCoords > 0.5f)
  {
	  color += 0.3f * waterMap.a;
  }
  else if (sampleCoords < 0.3f)
  {
	  color.rgb -= 0.08f * waterMap.a;
  }
  if (sampleCoords < 0.2f)
  {
	  color.rgb += 0.08f * waterMap.a;
  }
  if (sampleCoords > 0.6f)
  {
	  color.rgb += 0.48f * waterMap.a;
  }
  color += waterMap.a*pow(sin(RTW.y * 3 - Time * 0.1f + GetHeight(float2(RTW.x - Time / 240, 0))*0.3f),2)*0.2f;
  
  //float4 left = tex2D(waterSampler, coords + WaterSampleCoords + float2(-1/100,0));
  //color += (1 - left.a) * waterMap.a;
  return color;
}
technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderLight();
	}
}