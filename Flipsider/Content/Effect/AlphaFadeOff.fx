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

float4 AlphaFadeOff(VertexShaderOutput input) : COLOR
{
	input.Color *= input.TextureCoordinates.x;
	input.Color *= (1 + (abs((input.TextureCoordinates.y - 0.5f) * 2) * -1));
	return input.Color;
}

technique BasicColorDrawing
{
	pass AlphaFadeOff
	{
		PixelShader = compile PS_SHADERMODEL AlphaFadeOff();
	}
}