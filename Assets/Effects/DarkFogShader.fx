sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;
float4 uShaderSpecificData;
float fogTravelDistance;

float InverseLerp(float from, float to, float x)
{
    return saturate((x - from) / (to - from));
}

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    // Use FBM to calculate complex warping offsets in the noise texture that will be used to create the fog noise.
    float2 noiseOffset = 0;
    float noiseAmplitude = 0.03;
    float2 noiseZoom = 3.187;
    for (float i = 0; i < 2; i++)
    {
        float2 scrollOffset = float2(uTime * 0.285 - i * 0.338, uTime * -1.233 + i * 0.787) * noiseZoom.y * 0.04;
        noiseOffset += (tex2D(uImage1, coords * noiseZoom + scrollOffset) - 0.5) * noiseAmplitude;
        noiseZoom *= 2;
        noiseAmplitude *= 0.5;
    }
    
    // Calculate how far the fog should be going.
    float sourceDistanceWarp = tex2D(uImage1, coords * 3 - float2(0, uTime * 0.33));
    float distanceFromSource = distance(coords + noiseOffset, uTargetPosition) + (sourceDistanceWarp - 0.5) * 0.13;
    float distanceFadeInterpolant = InverseLerp(fogTravelDistance, fogTravelDistance * 0.8, distanceFromSource);
    
    noiseOffset.x += uTime * 0.04;
    float noise = tex2D(uImage1, coords * 2 + noiseOffset) * distanceFadeInterpolant;
    return sampleColor * noise * 1.26;
}
technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}