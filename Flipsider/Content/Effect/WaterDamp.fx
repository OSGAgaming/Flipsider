#if OPENGL
#define SV_POSITION POSITION
#define PS_SHADERMODEL ps_3_0
#else
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

texture noiseTexture;
sampler noiseSampler = sampler_state
{
	Texture = (noiseTexture);
};

struct VertexShaderOutput
{
	float2 TextureCoordinates : TEXCOORD0;
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
};

float progress;

float GetHeight(float2 Coord)
{
	return tex2D(noiseSampler, Coord).r;
}

float4 WaterDamp(VertexShaderOutput input) : COLOR
{
	float2 coords = input.TextureCoordinates;
	float2 japanese = float2(coords.x + cos(progress) + GetHeight(coords / 2), coords.y / 4 + sin(progress) - GetHeight(coords / 2));
	input.Color *= 0.4f;

	return input.Color;
}

technique BasicColorDrawing
{
	pass WaterDamp
	{
		PixelShader = compile PS_SHADERMODEL WaterDamp();
	}
}