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
	float4 c = tex2D(uImage0, coords);
	
	coords *= 300;
    
	float2 movingCoords = float2(coords.x + uTime * 20, coords.y + 20 * sin(uTime * 0.3 + 5)) / 1000;
	float4 noise = tex2D(uImage1, movingCoords);
    
	float2 movingCoords2 = float2(coords.x - uTime * 25, coords.y - 50 + 20 * sin(uTime * 0.3)) / 1000;
	float4 noise2 = tex2D(uImage1, movingCoords2);
	
	float4 baseColor = float4(0.2, 0.2, 0.2, 1);
	
	noise.rgb /= 2;
	noise.rgb += noise2.rgb * 0.5;
	
	c.rgb = lerp(c.rgb, noise.rgb, uOpacity);
	
	return c;
}

technique Technique1
{
	pass AutoloadPass
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}