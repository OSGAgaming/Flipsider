#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float Time;

sampler s0: register(s0);

float2 screenPosition;

float2 screenSize;

float screenScale;

texture noiseMap;

float BloomIntensity;
float BloomSaturation;
float2 Dims;
#define SAMPLE_COUNT 20
float Offsets[SAMPLE_COUNT];
float Weights[SAMPLE_COUNT];

sampler noiseSampler = sampler_state
{
	Texture = (noiseMap);
};

texture Map;
sampler MapSampler = sampler_state
{
	Texture = (Map);
};

float GetHeight(float2 Coord)
{
	return tex2D(noiseSampler, Coord).r;
}

float2 WorldCoords(float2 coords)
{
	return coords / screenScale + screenPosition / screenSize;
}

float2 WorldCoordsScaled(float2 coords, float2 scale)
{
	return (coords * scale) / screenScale + screenPosition / screenSize;
}

float4 AdjustSaturation(float4 color, float saturation)
{
	// The constants 0.3, 0.59, and 0.11 are chosen because the
	// human eye is more sensitive to green light, and less to blue.
	float grey = dot(color, float3(0.3, 0.59, 0.11));

	return lerp(grey, color, saturation);
}

float4 H(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = 0;

	for (int i = 0; i < SAMPLE_COUNT; i++)
	{
		color += tex2D(MapSampler, coords + float2(Offsets[i], 0) * float2(1 / Dims.x, 1 / Dims.y)) * Weights[i];
	}

	return color;
}

float4 V(float2 coords: TEXCOORD0) : COLOR0
{
	float4 color = 0;

	for (int i = 0; i < SAMPLE_COUNT; i++)
	{
		color += tex2D(MapSampler, coords + float2(0, Offsets[i]) * float2(1 / Dims.x, 1 / Dims.y)) * Weights[i];
	}

	return color;
}

float BaseSaturation;
float BaseIntensity;

float4 Combine(float2 coords : TEXCOORD0) : COLOR0
{
	// Look up the bloom and original base image colors.
	float4 bloom = tex2D(MapSampler, coords);
	float4 base = tex2D(s0, coords);

	// Adjust color saturation and intensity.
	bloom = AdjustSaturation(bloom, BloomSaturation) * BloomIntensity;
	base = AdjustSaturation(base, BaseSaturation) * BaseIntensity;

	// Darken down the base image in areas where there is a lot of bloom,
	// to prevent things looking excessively burned-out.
	base *= (1 - saturate(bloom));

	// Combine the two images.
	return base + bloom;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 H();
	}

	pass Pass2
	{
		PixelShader = compile ps_3_0 V();
	}

	pass Pass3
	{
		PixelShader = compile ps_3_0 Combine();
	}
}