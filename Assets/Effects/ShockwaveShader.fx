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

float2 screenSize;
float2 projectilePosition;
float explosionDistance;
float shockwaveOpacityFactor;

float InverseLerp(float min, float max, float x)
{
    return saturate((x - min) / (max - min));
}

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = 0;
    
    float uvOffsetAngle = tex2D(uImage0, coords) * 8 + uTime * 10;
    float2 uvOffset = float2(sin(uvOffsetAngle + 1.57), sin(uvOffsetAngle)) * 0.0023;
    
    // Calculate the distance of the UV coordinate from the explosion center.
    // This has a small offset based on noise so that the shape of the overall explosion isn't a perfect circle.
    float offsetFromProjectile = length((coords + uvOffset) * screenSize - projectilePosition);
    
    // Calculate how close the distance is to the explosion line.
    float signedDistanceFromExplosion = (offsetFromProjectile - explosionDistance) / screenSize.x;
    float distanceFromExplosion = abs(signedDistanceFromExplosion);
    
    // Make the shockwave's intensity dissipate at the outer edges.
    distanceFromExplosion += InverseLerp(0.01, 0.15, signedDistanceFromExplosion);
    
    // Make colors very bright near the explosion line.
    color += float4(uColor, 1) * 0.041 * shockwaveOpacityFactor / distanceFromExplosion;
    
    return color * sampleColor;
}
technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}