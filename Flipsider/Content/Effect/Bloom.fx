#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif
sampler s0: register(s0);
float4 Bloom(float2 coords: TEXCOORD0) : COLOR0
{
  float4 color = tex2D(s0, coords);
  color *= 1 - distance(coords, float2(0.5f,0.5f))*2;
  return color;
}
technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 Bloom();
	}
}