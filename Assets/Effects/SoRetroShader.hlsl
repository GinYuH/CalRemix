sampler uImage0 : register(s0); // screen
sampler uImage1 : register(s1); // lut

float2 uScreenResolution; // uImage0 res
float2 uImageSize1; // lut res

float4 quantize(float4 color, int levels)
{
    color.r = floor(color.r * (float)(levels - 1)) / (float)(levels - 1);
    color.g = floor(color.g * (float)(levels - 1)) / (float)(levels - 1);
    color.b = floor(color.b * (float)(levels - 1)) / (float)(levels - 1);

    return color;
}

float softLight( float s, float d )
{
    return (s < 0.5) ? d - (1.0 - 2.0 * s) * d * (1.0 - d) 
        : (d < 0.25) ? d + (2.0 * s - 1.0) * d * ((16.0 * d - 12.0) * d + 3.0) 
                     : d + (2.0 * s - 1.0) * (sqrt(d) - d);
}

float3 softLight( float3 s, float3 d )
{
    float3 c;
    c.x = softLight(s.x,d.x);
    c.y = softLight(s.y,d.y);
    c.z = softLight(s.z,d.z);
    return c;
}


float4 main(float2 coords : TEXCOORD0) : COLOR0
{
    // float2 uv = coords.xy/uScreenResolution;
    float2 uv = coords;

    float2 adjustedUV = tex2D(uImage1, uv).rg;
    float4 color = tex2D(uImage0, adjustedUV);

    color = lerp(color, float4(softLight(float3(1.,1.,1.) - color.rgb, color.rgb), 1.0), 0.25);
    color = quantize(color, 16);

    return color;
}

technique Technique1
{
    pass SoRetro
    {
        PixelShader = compile ps_3_0 main();
    }
}