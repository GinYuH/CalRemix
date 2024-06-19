float time;
float resolution;
float2 screenPosition;
float2 screenSize;
texture sampleTexture;
sampler2D Texture1Sampler = sampler_state
{
    texture = <sampleTexture>;
    magfilter = LINEAR;
    minfilter = LINEAR;
    mipfilter = LINEAR;
    AddressU = wrap;
    AddressV = wrap;
};

float2 random2( float2 p ) {
    return frac(sin(float2(dot(p,float2(127.1,311.7)),dot(p,float2(269.5,183.3))))*43758.5453);
}

float4 main(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 textureGet = tex2D(Texture1Sampler, coords);
    
    
    float combined = sampleColor.r + sampleColor.g + sampleColor.b;

    float2 st = screenPosition + screenSize * coords * 0.001f;
    coords = floor(coords * (screenSize)) / (screenSize);
    
    
    //float2 st = screenPosition + screenSize;
    //float2 st = uv.xy / screenSize.xy;
    //st.x *= screenSize.x / screenSize.y;
    float3 color = float3(0.0,    .31, 0.5f);

    // Scale
    st *= 5.;

    // Tile the space
    float2 i_st = floor(st);
    float2 f_st = frac(st);

    float m_dist = 1.;  // minimum distance

    for (int y= -1; y <= 1; y++) {
        for (int x= -1; x <= 1; x++) {
            // Neighbor place in the grid
            float2 neighbor = float2(float(x),float(y));

            // Random position from current + neighbor place in the grid
            float2 pointo = random2(i_st + neighbor);

			// Animate the point
            pointo = 0.5 + 0.5*sin(time + pointo) * 2;

			// Vector between the pixel and the point
            float2 diff = neighbor + pointo - f_st;

            // Distance to the point
            float dist = length(diff);

            // Keep the closer distance
            m_dist = min(m_dist, dist);
        }
    }

    // Draw the min distance (distance field)
    color += m_dist;
    


    return float4(color - float3(0.4, 0.4, 0.4), textureGet.a * 0.8);
}

technique Technique1
{
    pass ShieldPass
    {
        PixelShader = compile ps_3_0 main();
    }
}