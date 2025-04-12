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
float2 layerSize;
float2 layerOffset;
float4 edgeColor;
float2 singleFrameScreenOffset;

float2 convertToScreenCoords(float2 coords)
{
    return coords * screenSize;
}

float2 convertFromScreenCoords(float2 coords)
{
    return coords / screenSize;
}

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    // Calculate the base color. This is the calculated from the raw objects in the metaball render target.
    float4 baseColor = tex2D(uImage0, coords);
    
    // Check if there are any empty pixels nearby. If there are, that means this pixel is at an edge, and should be colored accordingly.
    if (baseColor.a > 0)
    {
        float left = tex2D(uImage0, convertFromScreenCoords(convertToScreenCoords(coords) + float2(-2, 0))).a;
        if (left <= 0)
            return edgeColor;
        
        float right = tex2D(uImage0, convertFromScreenCoords(convertToScreenCoords(coords) + float2(2, 0))).a;
        if (right <= 0)
            return edgeColor;
        
        float top = tex2D(uImage0, convertFromScreenCoords(convertToScreenCoords(coords) + float2(0, -2))).a;
        if (top <= 0)
            return edgeColor;
        
        float bottom = tex2D(uImage0, convertFromScreenCoords(convertToScreenCoords(coords) + float2(0, 2))).a;
        if (bottom <= 0)
            return edgeColor;
    }
    
    float4 layerColor = tex2D(uImage1, (coords + layerOffset + singleFrameScreenOffset) * screenSize / layerSize);
    return layerColor * tex2D(uImage0, coords);
}
technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}