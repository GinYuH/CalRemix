sampler2D uImage0 : register(s0);
float uTime;
float3 uColor;

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
	float2 uv = texCoord;
	uv -= float2(0.5, 0.5);
	
	// Do the swirl
	float dist = length(uv);
	float swirlEffect = sin(dist * 20.0 + (-uTime * 18.0)) * 0.05;
	uv -= swirlEffect * normalize(uv);
	
	float4 color = tex2D(uImage0, uv + float2(0.5, 0.5));
	
	// Glow
	color = float4(color.r * uColor.r, color.g * uColor.g, color.b * uColor.b, color.a);
	color.rgb *= 1.5 + sin(uTime * 10.0) * 0.5;
	
	// Fade at edge
	float fade = smoothstep(0.25, 0.5, dist);
	color.a *= (1.0 - fade);
	
	// Inner sun
	float circleFade = smoothstep(0.05, 0.4, dist);
	float3 circleColor = float3(1.0, 1.0, 1.0);
	color.rgb = lerp(circleColor, color.rgb, circleFade);
	
	return color;
}

technique Technique1
{
	pass AutoloadPass
	{
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}