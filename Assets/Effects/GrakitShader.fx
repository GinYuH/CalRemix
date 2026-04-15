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

float4 PixelShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 color = tex2D(uImage0, coords);
    
	float4 color1 = float4(0.39, 0.56, 0.94, color.a);
	float4 color2 = float4(0.45, 0.44, 0.66, color.a);
	float4 color3 = float4(0.31, 0.29, 0.53, color.a);
	float4 color4 = float4(0.20, 0.18, 0.41, color.a);
	float4 color5 = float4(0.06, 0.05, 0.14, color.a);
	
	
	if (color.r == 0 && color.g == 0 && color.b == 0)
		return color;
    
	float comp = (color.r + color.g + color.b) / 3;
	
	if (comp < 0.2)
		return color5;
	else if (comp < 0.4)
		return color4;
	else if (comp < 0.6)
		return color3;
	else if (comp < 0.8)
		return color2;
	else
		return color1;
}

technique Technique1
{
    pass DyePass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}