sampler2D uImage0 : register(s0);
sampler uImage1 : register(s1);
float uTime;
float3 uColor;
float opacity;

float4 PixelShaderFunction(float2 tex_coords : TEXCOORD0) : COLOR0
{
	float4 c = tex2D(uImage0, tex_coords);
	float2 uv = tex_coords;
	uv -= float2(0.5, 0.5);
	uv *= 6;
    
	float2 movingCoords = float2(tex_coords.x + uTime * 0.5, tex_coords.y) * 2;
	float4 noise = tex2D(uImage1, movingCoords);
    
	float2 movingCoords2 = float2(tex_coords.x - uTime * 0.5, tex_coords.y - 50) * 2;
	float4 noise2 = tex2D(uImage1, movingCoords2);
	
	noise.rgb *= uColor * 1;
	noise.rgb += noise2.rgb * 1;
	c.rgb *= noise.rgb;
	c.a = opacity;
	c.a *= smoothstep(0, 0.5, 1 - length(uv));
	
	return c;
}

technique Technique1
{
	pass AutoloadPass
	{
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}