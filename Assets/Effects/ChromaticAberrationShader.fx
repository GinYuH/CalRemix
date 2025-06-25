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
float2 impactPoint;

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    
    // Split colors.
    float splitDistance = uIntensity * 0.048 / (distance(coords, impactPoint) + 1);
    color.r = tex2D(uImage0, coords + float2(-0.707, -0.707) * splitDistance).r;
    color.g = tex2D(uImage0, coords + float2(0.707, -0.707) * splitDistance).g;
    color.b = tex2D(uImage0, coords + float2(0, 1) * splitDistance).b;
    
    return color;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}