sampler TextureSampler : register(s0);

float Intensity = 1.0;

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(TextureSampler, coords);
    
    // These weights account for human eye sensitivity to different colors
    float grey = dot(color.rgb, float3(0.299, 0.587, 0.114));
    float3 greyColor = float3(grey, grey, grey);
    float3 finalColor = lerp(color.rgb, greyColor, Intensity);
    
    return float4(finalColor, color.a);
}

technique Greyscale
{
    pass AutoloadPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}