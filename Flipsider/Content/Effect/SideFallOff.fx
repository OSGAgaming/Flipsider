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

float4 SideFallOff(VertexShaderOutput input) : COLOR
{
	return input.Color * sin(input.TextureCoordinates.y * 3.14159265);
}

technique BasicColorDrawing
{
	pass SideFallOff
	{
		PixelShader = compile PS_SHADERMODEL SideFallOff();
	}
}