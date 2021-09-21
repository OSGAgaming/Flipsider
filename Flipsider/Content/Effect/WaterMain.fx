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

float4 WaterMain(VertexShaderOutput input) : COLOR
{
	if (input.TextureCoordinates.y < 0.01f + abs(GetHeight(input.TextureCoordinates) / 400))
		input.Color.rgb += 0.2f;
	float2 coords = input.TextureCoordinates;
	float2 japanese = float2(coords.x + cos(progress) + GetHeight(coords / 2), coords.y / 4 + sin(progress) - GetHeight(coords / 2));
	input.Color.rgb -= input.TextureCoordinates.y / 2;
	input.Color *= 0.4f;
	input.Color += GetHeight(coords / 4 + float2(sin(progress / 18) * 0.5f, cos(progress / 14) * 0.5f)) * GetHeight(coords / 4 - float2(sin(progress / 15) * 0.5f, cos(progress / 13) * 0.5f)) * 0.13f;
	return input.Color;
}

technique BasicColorDrawing
{
	pass WaterMain
	{
		PixelShader = compile PS_SHADERMODEL WaterMain();
	}
}