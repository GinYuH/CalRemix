float radius;
float maxRadius;
float maxOpacity;
float seed;
float sizeDivisor;

float2 anchorPoint;
float2 screenPosition;
float2 screenSize;

sampler uImage0 : register(s0);
sampler uImage1 : register(s1);

float InverseLerp(float a, float b, float t)
{
    return saturate((t - a) / (b - a));
}

float4 main(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{    
    float4 textureGet = tex2D(uImage1, coords);

    float2 worldUV = screenPosition + screenSize * coords;
    coords = floor(coords * (screenSize * sizeDivisor)) / (screenSize * sizeDivisor);
    float worldDistance = distance(worldUV, anchorPoint);
    float rnd = frac(sin(dot(coords.xy, float2(12.9898,78.233)))* 422258.5453123 + seed);
	float4 staticc = float4(rnd * 0.5, rnd * 0.5, rnd * 0.5, 1);

    float opacity = 0;

    if (worldDistance > radius) 
    opacity = lerp(opacity, 0.9, InverseLerp(radius, maxRadius, worldDistance));
    
    opacity = clamp(opacity, 0, maxOpacity); 

    if (worldDistance > radius)
    return staticc * opacity;
    
    return sampleColor;



}


technique Technique1
{
    pass StaticPass
    {
        PixelShader = compile ps_3_0 main();
    }
}