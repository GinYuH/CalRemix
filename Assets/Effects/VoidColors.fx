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


float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    
	if (color.r == 1 && color.g == 0 && color.b == 0)
		return color;
	if (color.r + color.g + color.b > 0.6)
		return float4(1, 0, 1, color.a);
	return float4(0, 0, 0, 1);
}

technique Technique1
{
    pass VoidPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}