sampler2D uImage0 : register(s0);
sampler uImage1 : register(s1);
float uTime;
float3 uColor;
float4 rectangle;
float2 topLeft;
float2 bottomRight;
float opacity;

float4 PixelShaderFunction(float2 coords : SV_Position, float2 tex_coords : TEXCOORD0) : COLOR0
{
	float2 local_coords = coords - rectangle.xy;
    
	float4 c = tex2D(uImage0, tex_coords);
    
	float2 movingCoords = float2(coords.x + uTime * 40, coords.y) / 1000;
	float4 noise = tex2D(uImage1, movingCoords);
    
	float2 movingCoords2 = float2(coords.x - uTime * 45, coords.y - 50) / 1000;
	float4 noise2 = tex2D(uImage1, movingCoords2);
	
	noise.rgb *= uColor * 0.75;
	noise.rgb += noise2.rgb * 0.75;	
	
	float2 center = rectangle.xy + rectangle.zw / 2.0;
	
	// top. bottom is a non issue
	float1 topNoise = smoothstep(topLeft.y, topLeft.y - 200, local_coords.y);
	
	// left
	float1 leftNoise = smoothstep(topLeft.x, topLeft.x - 200, local_coords.x);
	
	// right
	float1 rightNoise = smoothstep(bottomRight.x, bottomRight.x + 200, local_coords.x);
	
	noise *= opacity;
	
	// top. bottom is a non issue
	if (local_coords.x > topLeft.x && local_coords.x < bottomRight.x)
		noise *= topNoise;
	
	// left
	if (local_coords.x < topLeft.x && local_coords.y > topLeft.y)
		noise *= leftNoise;
	
	// right
	if (local_coords.x > bottomRight.x && local_coords.y > topLeft.y)
		noise *= rightNoise;
	
	// top left
	if (local_coords.x < topLeft.x && local_coords.y < topLeft.y)
		noise *= max(leftNoise, topNoise);
	
	// top right
	if (local_coords.x > bottomRight.x && local_coords.y < topLeft.y)
		noise *= max(rightNoise, topNoise);
    
	if (local_coords.x < topLeft.x || local_coords.x > bottomRight.x || local_coords.y < topLeft.y || local_coords.y > bottomRight.y)
		return noise;
    
	return c;
}

technique Technique1
{
	pass AutoloadPass
	{
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}