#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif
sampler s0: register(s0);
float2 Dims;
float NoOfFramesX;
float NoOfFramesY;
float FrameX;
float FrameY;
float BloomIntensity;
float BloomSaturation;
#define SAMPLE_COUNT 10
float Offsets[SAMPLE_COUNT];
float Weights[SAMPLE_COUNT];

float4 AdjustSaturation(float4 color, float saturation)
{
	// The constants 0.3, 0.59, and 0.11 are chosen because the
	// human eye is more sensitive to green light, and less to blue.
	float grey = dot(color, float3(0.3, 0.59, 0.11));

	return lerp(grey, color, saturation);
}
float4 Bloom(float2 coords: TEXCOORD0) : COLOR0
{

  float2 samplecoords = float2((coords.x + FrameX) / NoOfFramesX,(coords.y + FrameY) / NoOfFramesY);
  samplecoords.y = clamp(samplecoords.y,(FrameY + 0.1f) / NoOfFramesY, (FrameY+1) / NoOfFramesY);
  float4 color = 0;

  float4 colorBuffer = tex2D(s0, clamp(samplecoords,0,1));

  for (int i = 0; i < SAMPLE_COUNT; i++)
  {
	  color += tex2D(s0,samplecoords + float2(Offsets[i],0)* float2(1/Dims.x,1/Dims.y)) * Weights[i];
  }

  colorBuffer = AdjustSaturation(colorBuffer, 1);

  color = AdjustSaturation(color, BloomSaturation)* BloomIntensity;

  colorBuffer *= (1 - saturate(color));

  // Combine the two images.
  return colorBuffer + color;
}
technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_3_0 Bloom();
	}
}