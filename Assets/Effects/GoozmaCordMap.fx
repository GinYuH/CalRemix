sampler2D uImage0 : register(s0);
matrix uTransformMatrix;
float uTime;

texture uTexture;
sampler2D tex = sampler_state
{
    texture = <uTexture>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};

texture uMap;
sampler2D map = sampler_state
{
    texture = <uMap>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = clamp;
    AddressV = clamp;
};

struct VertexShaderInput
{
    float2 Coord : TEXCOORD0;
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float2 Coord : TEXCOORD0;
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

VertexShaderOutput VertexShaderFunction(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;
    output.Color = input.Color;
    output.Coord = input.Coord;
    output.Position = mul(input.Position, uTransformMatrix);
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 img0 = tex2D(tex, input.Coord * float2(3, 1) - float2(uTime, sin(uTime * 6.28) * 0.02));
    float4 img1 = tex2D(tex, input.Coord * float2(2, 1) - float2(uTime * 3, 0));
    float4 flat = (length(img0 * img1) > 0.1 ? 1 : 0);    
    float4 baseMap = img0 + img1;
    
    float4 colors = tex2D(map, float2(input.Coord.x - baseMap.r * 0.3, 0.25));
    float4 colorsDark = tex2D(map, float2(input.Coord.x - baseMap.r * 0.1, 0.75));

    if (baseMap.g > 0.5)
        return colorsDark * flat;
    else
        return colors * flat;
}

technique Technique1
{
    pass CordMapPass
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
        VertexShader = compile vs_2_0 VertexShaderFunction();
    }
}