sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float2 uTargetPosition;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;

float2 InverseLerp(float2 start, float2 end, float2 x)
{
    return saturate((x - start) / (end - start));
}

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 framedCoords = (coords * uImageSize0 - uSourceRect.xy) / uSourceRect.zw;
    float4 color = tex2D(uImage0, coords);
    
    float time = sin(uTime) * 0.25 + 0.125;
    
    if (framedCoords.x < 0.45 + sin((coords.y + uTime) * 22) * 0.2)
        return float4(uColor, 1) * sampleColor * color;
    if (framedCoords.x > 0.55 + sin((coords.y + uTime) * 22) * 0.2)
        return float4(uColor, 1) * sampleColor * color;
    else
        return float4(uSecondaryColor, 1) * sampleColor * color;
    
    return color;
}
technique Technique1
{
    pass DyePass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}