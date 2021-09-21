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

float progress;

float hue2rgb(float p, float q, float t)
{
	if (t < 0)
		t += 1;
	if (t > 1)
		t -= 1;
	if (t < 0.166f)
		return p + (q - p) * 6 * t;
	if (t < 0.5f)
		return q;
	if (t < 0.66f)
		return p + (q - p) * (2 / 3 - t) * 6;
	return p;
}

float4 hslToRgb(float h, float s, float l)
{
	float r, g, b;
	float q = l < 0.5 ? l * (1 + s) : (l + s) - (l * s);
	float p = (2 * l) - q;
	r = hue2rgb(p, q, h + 0.33f);
	g = hue2rgb(p, q, h);
	b = hue2rgb(p, q, h - 0.33f);
	return float4(r, g, b, 1);
}

float4 MainPSA(VertexShaderOutput input) : COLOR
{
	input.Color *= hslToRgb(0.64f + (0.18f * (float) sin((input.TextureCoordinates.y * 4) + progress)), 1, 0.7f);
	return input.Color;
}

technique BasicColorDrawing
{
	pass AquaLightPass
	{
		PixelShader = compile PS_SHADERMODEL MainPSA();
	}
}