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
texture lightMask;
texture tileMask;
texture miscMap;
texture waterMap;
texture noiseTexture;
sampler noiseSampler = sampler_state
{
	Texture = (noiseTexture);
};
sampler tileSampler = sampler_state
{
	Texture = (tileMask);
};
sampler lightSampler = sampler_state 
{ 
	Texture = (lightMask); 
};
sampler miscSampler = sampler_state
{
	Texture = (miscMap);
};
sampler waterSampler = sampler_state
{
	Texture = (waterMap);
};
float GetHeight(float2 Coord)
{
	return tex2D(noiseSampler, Coord).r;
}
float baseLight;
float tileDiffusion;
float generalDiffusion;
float4 PixelShaderLight(float2 coords: TEXCOORD0) : COLOR0
{
  
  float4 lightColor = tex2D(lightSampler, coords);
  float4 tileColor = tex2D(tileSampler, coords);
  float4 miscColor = tex2D(miscSampler, coords);
  float4 waterMap = tex2D(waterSampler, coords);
  float water = waterMap.g > 0 ? 1 : 0;
  float tile = tileColor.r > 0 ? 1 : 0;
  float2 japanese = float2(coords.x + cos(Time/60) + GetHeight(coords / 2), coords.y / 4 + sin(Time / 60) - GetHeight(coords / 2));
  float4 color = tex2D(s0, coords + tileColor.b * float2(GetHeight(japanese)/700,GetHeight(japanese)/700));
  return (color * baseLight) + (lightColor * ((tileColor.r + tileColor.b)/ tileDiffusion + miscColor/ generalDiffusion));
}
technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderLight();
	}
}