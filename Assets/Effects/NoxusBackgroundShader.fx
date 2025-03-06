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

float luminanceThreshold;
float scrollSpeed;
float noiseZoom;

float2 flashCoordsOffset;
float2 flashPosition;
float flashNoiseZoom;
float flashIntensity;

float2x2 fbmMatrix = float2x2(1.63, 1.2, -1.2, 1.63);

float turbulentNoise(float2 coords)
{
    float2 currentCoords = coords;
    
    // Approximate, somewhat basic FBM equations with time included.
    float result = 0.5 * tex2D(uImage1, (currentCoords + float2(0, uTime * scrollSpeed * -0.3)) * noiseZoom);
    currentCoords = mul(currentCoords, fbmMatrix);
    currentCoords.y += uTime * scrollSpeed * 0.25;
    
    result += 0.25 * tex2D(uImage1, currentCoords * noiseZoom);
    currentCoords = mul(currentCoords, fbmMatrix);
    currentCoords.x += uTime * scrollSpeed * 0.4;
    
    return result * 1.0666667;
}

float3 swirl(float2 coords)
{
    // Start by using turbulence as a base for the background.
    float3 result = uColor * (turbulentNoise(coords * 4) + turbulentNoise(coords * 12)) * 0.75;
    
    return pow(result, 2.75);
}

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    if (luminanceThreshold > 0)
    {
        float4 color = tex2D(uImage0, coords);
        float luminance = dot(color.rgb, float3(0.299, 0.587, 0.114));
        if (luminance < luminanceThreshold)
            return 0;
    }
    
    float2 worldOffset = uScreenPosition * 0.00006;
    float4 finalColor = float4(swirl(coords + worldOffset), 1) * sampleColor * uIntensity;
    
    // Apply the flash effect.
    float flashDissipation = distance(coords, flashPosition) * 3 + 1;
    float flash = tex2D(uImage2, coords * noiseZoom + flashCoordsOffset) * uIntensity * flashIntensity / pow(flashDissipation, 8.1);
    finalColor = finalColor * (1 + flash) + flash * 0.02;
    
    return finalColor;
}
technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}