#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler s0: register(s0);
texture lightMask;
texture tileMask;
texture miscMap;
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


float baseLight;
float tileDiffusion;
float generalDiffusion;
float4 PixelShaderLight(float2 coords: TEXCOORD0) : COLOR0
{
  float4 color = tex2D(s0, coords);
  float4 lightColor = tex2D(lightSampler, coords);
  float4 tileColor = tex2D(tileSampler, coords);
  float4 miscColor = tex2D(miscSampler, coords);
  return (color * baseLight) + (lightColor * (tileColor/ tileDiffusion + miscColor/ generalDiffusion));
}
technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderLight();
	}
}