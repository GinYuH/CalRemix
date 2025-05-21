float4 frame;
float2 textureResolution;
float4 screenBlendColor;

texture sampleTexture;
sampler2D samplerTex = sampler_state
{
    texture = <sampleTexture>;
    magfilter = POINT;
    minfilter = POINT;
    mipfilter = POINT;
    AddressU = wrap;
    AddressV = wrap;
};


struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    output.Position = input.Position;
    
    output.Color = input.Color;
    output.TextureCoordinates = input.TextureCoordinates;

    return output;
}

float3 screen(float3 base, float3 scrn)
{
    return float3(1, 1, 1) - (float3(1, 1, 1) - base) * (float3(1, 1, 1) - scrn);
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float2 frameUV = (input.TextureCoordinates * frame.zw + frame.xy) / textureResolution;
    float4 color = tex2D(samplerTex, frameUV);

    float3 screenedRgb = screen(color.rgb, screenBlendColor.rgb);
    color.rgb = lerp(color.rgb, screenedRgb, screenBlendColor.a);

    return color * input.Color * color.a;
}

technique Technique1
{
    pass NormalDrawPass
    {
        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}