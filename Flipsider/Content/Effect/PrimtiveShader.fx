#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


matrix WorldViewProjection;

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

float2 coordMultiplier;
float2 coordOffset;
float strength;
float progress2;
//custom passes
texture imageTexture;
sampler imageSampler = sampler_state
{
    Texture = (imageTexture);
    AddressU = Wrap;
    AddressV = Wrap;
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
//float hue2rgb(float p, float q, float t){
//            if(t < 0) t += 1;
//            if(t > 1) t -= 1;
//            if(t < 0.166f) return p + (q - p) * 6 * t;
//            if(t < 0.5f) return q;
//            if(t < 0.66f) return p + (q - p) * (2/3 - t) * 6;
//            return p;
//}

//float4 hslToRgb(float h, float s, float l){
//    float r, g, b;
//        float q = l < 0.5 ? l * (1 + s) : (l + s) - (l * s);
//        float p = (2 * l) - q;
//        r = hue2rgb(p, q, h + 0.33f);
//        g = hue2rgb(p, q, h);
//        b = hue2rgb(p, q, h - 0.33f);
//	return float4(r,g,b,1);
//}

//float4 MainPS3(VertexShaderOutput input) : COLOR
//{
//	input.Color.r += sin(input.TextureCoordinates.x * 5);
//	return input.Color;
//}

float4 BasicImage(VertexShaderOutput input) : COLOR
{
    float alpha = abs((1.0 - strength) + tex2D(imageSampler, coordOffset + input.TextureCoordinates * coordMultiplier).r * strength);
	return input.Color * alpha;
}

technique BasicColorDrawing
{
	pass DefaultPass
	{
		VertexShader = compile vs_3_0 MainVS();
	}
	pass BasicImagePass
	{
		VertexShader = compile vs_3_0 MainVS();
		PixelShader = compile ps_3_0 BasicImage();
	}
}
