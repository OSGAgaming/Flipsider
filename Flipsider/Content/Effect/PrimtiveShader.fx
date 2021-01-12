﻿#if OPENGL
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
texture spotTexture;
sampler spotSampler = sampler_state
{
	Texture = (spotTexture);
};
texture polkaTexture;
sampler polkaSampler = sampler_state
{
	Texture = (polkaTexture);
};
texture Voronoi;
sampler VoronoiSampler = sampler_state
{
	Texture = (Voronoi);
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
float4 Web(VertexShaderOutput input) : COLOR
{
	float2 coords2 = float2(input.TextureCoordinates.x/3 + 0.1f,(input.TextureCoordinates.y/15) + 0.1f);
	float2 coords = float2(input.TextureCoordinates.x / 2, (input.TextureCoordinates.y / 10));
	float polkaColor = tex2D(polkaSampler, coords).r;
	float polkaColor2 = tex2D(polkaSampler, coords2).r;
	input.Color *= polkaColor;
	input.Color *= polkaColor2;
	return input.Color * sin(GetHeight(input.TextureCoordinates.y)) * sin(input.TextureCoordinates.y * 5);
}
float4 MainPS(VertexShaderOutput input) : COLOR
{
	input.Color.r += sin(input.TextureCoordinates.x * 4 + progress);
	input.Color.g -= cos(input.TextureCoordinates.x * 4 + progress);
	input.Color.b += cos(input.TextureCoordinates.x * 4 + progress);
	return input.Color * sin(input.TextureCoordinates.y * 3.14159265);
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
float4 Laser(VertexShaderOutput input) : COLOR
{
	float rand = GetHeight(float2(0.5f,(input.TextureCoordinates.x/2 + sin(progress / 40)*0.5f)) % 1);
	float sineInterp = (1000 * abs(rand));
	input.Color *= pow(sin(input.TextureCoordinates.y * 3.14159265),sineInterp*(1 + abs(rand)));
	input.Color.rgb *= 1 + max(0,(abs(progress / 15 % 4 - 2 - input.TextureCoordinates.x)));
	input.Color *= 1 + abs(rand)* abs(rand);
	return input.Color;
}
float4 AlphaFadeOff(VertexShaderOutput input) : COLOR
{
	input.Color *= input.TextureCoordinates.x;
	input.Color *= (1 + (abs((input.TextureCoordinates.y - 0.5f) * 2) * -1));
	return input.Color;
}
float4 Basic(VertexShaderOutput input) : COLOR
{
	float2 coords = float2(input.TextureCoordinates.x,input.TextureCoordinates.y);
	float4 spotColor = tex2D(spotSampler, coords).r;
	input.Color *= GetHeight(coords/2 + float2(sin(progress)/5 + 0.5f,cos(progress)/5 + 0.5f))*3;
	input.Color *= 1 + coords.x * abs(sin(input.TextureCoordinates.y * 20))*5;
	input.Color *= sin(input.TextureCoordinates.y * 3.14f);
	input.Color.rgb -= float3(1 + spotColor.r, 1 - spotColor.g, spotColor.b);
	input.Color *= 5 - distance(float2(1,0.5f),input.TextureCoordinates)*4;

	return input.Color;
}
float4 Basic2(VertexShaderOutput input) : COLOR
{
	float2 coords = float2(input.TextureCoordinates.x,input.TextureCoordinates.y);
	float4 spotColor = tex2D(spotSampler, coords).r;
	float lerper = pow(sin(input.TextureCoordinates.y * 3.14f - 1.2f),50 + GetHeight(input.TextureCoordinates.x)*50 - 25) + pow(sin(input.TextureCoordinates.x * 3.14f - 1.2f + (progress/3* progress/3)), 30 + GetHeight(input.TextureCoordinates.y/2 + progress) * 50 - 25);
	input.Color *= lerp(float4(0,0,0,0),float4(0.2f, 0.8f,1,0)*10, lerper);
	input.Color *= sin(input.TextureCoordinates.x * 3.14f);
	return input.Color;
}
float4 WaterPogPass(VertexShaderOutput input) : COLOR
{
	float2 coords = float2(input.TextureCoordinates.x,input.TextureCoordinates.y);
	float4 spotColor = tex2D(spotSampler, coords).r;
	input.Color *= GetHeight(coords / 2 + float2(sin(progress) / 5 + 0.5f,cos(progress) / 5 + 0.5f)) * 3;
	input.Color *= 1 + coords.x * abs(sin(input.TextureCoordinates.y * 20)) * 5;
	input.Color *= sin(input.TextureCoordinates.y * 3.14f);
	input.Color.rgb -= float3(1 + spotColor.r, 1 - spotColor.g, spotColor.b);
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
	pass RainbowLightPass
	{
		PixelShader = compile ps_3_0 MainPS();
	}
	pass Edge
	{
		PixelShader = compile ps_3_0 Basic();
	}
	pass Edge2
	{
		PixelShader = compile ps_3_0 Basic2();
	}
	pass AquaLightPass
	{
		PixelShader = compile ps_3_0 MainPSA();
	}
	pass WebPass
	{
		PixelShader = compile ps_3_0 Web();
	}
	pass SideFallOff
	{
		PixelShader = compile ps_3_0 SideFallOff();
	}
	pass Lazor
	{
		PixelShader = compile ps_3_0 Laser();
	}
	pass BasicImagePass
	{
		VertexShader = compile vs_3_0 MainVS();
		PixelShader = compile ps_3_0 BasicImage();
	}
}
