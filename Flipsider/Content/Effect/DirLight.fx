#if OPENGL
#define SV_POSITION POSITION
#define PS_SHADERMODEL ps_3_0
#else
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

struct VertexShaderOutput
{
	float2 TextureCoordinates : TEXCOORD0;
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
};

float4 DirLight(VertexShaderOutput input) : COLOR
{
	float2 coords = input.TextureCoordinates;
	input.Color *= 1 - distance(float2(0.5f, 0.5f), coords);
	return input.Color;
}

technique BasicColorDrawing
{
	pass DirLight
	{
		PixelShader = compile PS_SHADERMODEL DirLight();
	}
}