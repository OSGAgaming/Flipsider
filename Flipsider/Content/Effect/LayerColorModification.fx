#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif
sampler s0 : register(s0);

float saturationValue;
float4 colorModification;
float pixelationFactor;
float2 resolution;
float2 transform;

float4 AdjustSaturation(float4 color, float saturation)
{
	// The constants 0.3, 0.59, and 0.11 are chosen because the
	// human eye is more sensitive to green light, and less to blue.
    float grey = dot(color, float3(0.3, 0.59, 0.11));

    return lerp(float4(grey, grey, grey, color.a), color, saturation);
}
float4 LayerEffect(float2 coords : TEXCOORD0, float4 color : COLOR0) : COLOR0
{
    float2 perc = (transform % 1) / resolution;
    float2 roundedcoords = float2((int) (coords.x * resolution.x) / resolution.x, (int) (coords.y * resolution.y) / resolution.y);

    color *= tex2D(s0, pixelationFactor >= 1 ? roundedcoords : coords);
	
  // Combine the two images.
    return AdjustSaturation(color * colorModification, saturationValue);
}
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 LayerEffect();
    }
}