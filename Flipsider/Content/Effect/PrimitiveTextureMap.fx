#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

texture imageTexture;
sampler imageSampler = sampler_state
{
    Texture = (imageTexture);
    AddressU = Wrap;
    AddressV = Wrap;
};

matrix WorldViewProjection;

texture textureMap;
sampler texSampler = sampler_state
{
	Texture = (textureMap);
};

struct VertexShaderInput
{
	float2 TextureCoordinates : TEXCOORD0;
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float2 TextureCoordinates : TEXCOORD0;
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;
    float4 pos = mul(input.Position, WorldViewProjection);
    output.Position = pos;

    output.Color = input.Color;

	output.TextureCoordinates = input.TextureCoordinates;

    return output;
}

float4 Method(VertexShaderOutput input) : COLOR
{
	return tex2D(texSampler, input.TextureCoordinates);
}

technique BasicColorDrawing
{
	pass DefaultPass
	{
		VertexShader = compile vs_3_0 MainVS();
	}
	pass Pass
	{
		PixelShader = compile ps_3_0 Method();
	}
}
