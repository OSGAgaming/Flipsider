#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif


matrix WorldViewProjection;
texture noiseTexture;
sampler noiseSampler = sampler_state
{
	Texture = (noiseTexture);
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

float2 coordMultiplier;
float2 coordOffset;
float strength;
float progress;
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
float GetHeight(float2 Coord)
{
	return tex2D(noiseSampler, Coord).r;
}
float4 SideFallOff(VertexShaderOutput input) : COLOR
{
	return input.Color * sin(input.TextureCoordinates.y * 3.14159265);
}
float4 DirLight(VertexShaderOutput input) : COLOR
{
	float2 coords = input.TextureCoordinates;
	input.Color *= 1 - distance(float2(0.5f,0.5f),coords);
	return input.Color;
}
float4 WaterDamp(VertexShaderOutput input) : COLOR
{
	float2 coords = input.TextureCoordinates;
	float2 japanese = float2(coords.x + cos(progress) + GetHeight(coords / 2), coords.y / 4 + sin(progress) - GetHeight(coords / 2));
	input.Color *= 0.4f;

	return input.Color;
}
float4 WaterMain(VertexShaderOutput input) : COLOR
{
	if (input.TextureCoordinates.y < 0.01f + abs(GetHeight(input.TextureCoordinates)/400))
	input.Color.rgb += 0.2f;
	float2 coords = input.TextureCoordinates;
    float2 japanese = float2(coords.x + cos(progress) + GetHeight(coords / 2), coords.y/4 + sin(progress) - GetHeight(coords/2));
    input.Color.rgb -= input.TextureCoordinates.y / 2;
	input.Color *= 0.4f;
	input.Color += GetHeight(coords/4 + float2(sin(progress / 18)*0.5f, cos(progress/14)*0.5f))* GetHeight(coords/4 - float2(sin(progress / 15) * 0.5f, cos(progress / 13) * 0.5f)) *0.13f;
	return input.Color;
}
float hue2rgb(float p, float q, float t){
            if(t < 0) t += 1;
            if(t > 1) t -= 1;
            if(t < 0.166f) return p + (q - p) * 6 * t;
            if(t < 0.5f) return q;
            if(t < 0.66f) return p + (q - p) * (2/3 - t) * 6;
            return p;
        }
float4 hslToRgb(float h, float s, float l){
    float r, g, b;
        float q = l < 0.5 ? l * (1 + s) : (l + s) - (l * s);
        float p = (2 * l) - q;
        r = hue2rgb(p, q, h + 0.33f);
        g = hue2rgb(p, q, h);
        b = hue2rgb(p, q, h - 0.33f);
	return float4(r,g,b,1);
}

float4 MainPSA(VertexShaderOutput input) : COLOR
{
	input.Color *= hslToRgb(0.64f + (0.18f * (float)sin((input.TextureCoordinates.y * 4) + progress)), 1, 0.7f);
	return input.Color;
}

float4 MainPS3(VertexShaderOutput input) : COLOR
{
	input.Color.r += sin(input.TextureCoordinates.x * 5);
	return input.Color;
}

float4 AlphaFadeOff(VertexShaderOutput input) : COLOR
{
	input.Color *= input.TextureCoordinates.x;
	input.Color *= (1 + (abs((input.TextureCoordinates.y - 0.5f) * 2) * -1));
	return input.Color;
}

float4 BasicImage(VertexShaderOutput input) : COLOR
{
    float alpha = abs((1.0 - strength) + tex2D(imageSampler, coordOffset + input.TextureCoordinates * coordMultiplier).r * strength);
	return input.Color * alpha;
}

technique BasicColorDrawing
{
	pass AlphaFadeOff
	{
		PixelShader = compile ps_3_0 AlphaFadeOff();
	}
	pass DefaultPass
	{
		VertexShader = compile vs_3_0 MainVS();
	}
	pass WaterMain
	{
		PixelShader = compile ps_3_0 WaterMain();
	}
	pass WaterDamp
	{
		PixelShader = compile ps_3_0 WaterDamp();
	}
	pass AquaLightPass
	{
		PixelShader = compile ps_3_0 MainPSA();
	}
	pass SideFallOff
	{
		PixelShader = compile ps_3_0 SideFallOff();
	}
	pass DirLight
	{
		PixelShader = compile ps_3_0 DirLight();
	}
	pass BasicImagePass
	{
		VertexShader = compile vs_3_0 MainVS();
		PixelShader = compile ps_3_0 BasicImage();
	}
}
