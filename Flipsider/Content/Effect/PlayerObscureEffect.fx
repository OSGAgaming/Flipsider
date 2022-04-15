#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

float Time;

sampler s0 : register(s0);

float2 screenPosition;
float2 screenSize;
float screenScale;
texture noiseMap;

sampler noiseSampler = sampler_state
{
    Texture = (noiseMap);
};

texture Map;
sampler MapSampler = sampler_state
{
    Texture = (Map);
};

texture PlayerMap;
sampler PlayerSampler = sampler_state
{
    Texture = (PlayerMap);
};

float GetHeight(float2 Coord)
{
    return tex2D(noiseSampler, Coord).r;
}

float2 WorldCoords(float2 coords)
{
    return coords + (screenPosition / screenSize) * screenScale;
}

float2 WorldCoordsScaled(float2 coords, float2 scale)
{
    return (coords * scale) / screenScale + screenPosition / screenSize;
}

float2 Round(float2 coords, float accuracy)
{
    float pixelX = screenSize.x / accuracy;
    float pixelY = screenSize.y / accuracy;
    return float2(floor(coords.x * pixelX) / pixelX, floor(coords.y * pixelY) / pixelY);
}

float Round(float coords, float accuracy)
{
    float pixelX = screenSize.x / accuracy;
    return floor(coords.x * pixelX) / pixelX;
}

float4 PixelShaderLight(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(s0, coords);

    float4 fgColor = tex2D(PlayerSampler, coords);
    float4 mapColor = tex2D(MapSampler, coords);
    
    float2 resolution = 2.0f / float2(1980, 1080);
    
    float outline = 0;
    float2 fulrescoords = float2((int) (1980 * coords.x), (int) (1080 * coords.y));
    
    outline -= tex2D(PlayerSampler, coords + float2(resolution.x, 0)).a - fgColor.a;
    outline -= tex2D(PlayerSampler, coords + float2(-resolution.x, 0)).a - fgColor.a;
    outline -= tex2D(PlayerSampler, coords + float2(0, resolution.x)).a - fgColor.a;
    outline -= tex2D(PlayerSampler, coords + float2(0, -resolution.x)).a - fgColor.a;
    
    
    outline = clamp(outline, 0, 1);
    
    return color + (fgColor.a * sign(outline) * (fulrescoords.x % 2 * fulrescoords.y % 2)) * mapColor.a;
}
technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 PixelShaderLight();
    }
}