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
sampler tileSampler = sampler_state
{
	Texture = (tileMask);
};
sampler lightSampler = sampler_state 
{ 
	Texture = (lightMask); 
};

float baseLight;
float4 PixelShaderLight(float2 coords: TEXCOORD0) : COLOR0
{
  float4 color = tex2D(s0, coords);
  float4 lightColor = tex2D(lightSampler, coords);
  float4 tileColor = tex2D(tileSampler, coords);
  return color * (lightColor + baseLight);
}
technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 PixelShaderLight();
	}
}