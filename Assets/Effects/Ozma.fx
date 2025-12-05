sampler baseTexture : register(s0);
sampler noiseTexture : register(s1);

float time : register(C0);
float rotation;
float2 zoom = (1,1);

// Marble appearance controls
float blowUpPower = 2.5; // How spherical it looks
float blowUpSize = 0.25; // Strength of the spherical distortion
float noiseScale = 0.75; // Scale of the noise pattern
float scrollSpeed = 0.005; // How fast the pattern flows
float noiseContrast = 1.75;
float noiseRotation = 1.5708;
float pixelation = 128; // Pixelation amount (0 = none, higher = more pixelated)

// Expanded Ozma-inspired color palette
float4 color1 = float4(1.0, 0.1, 0.1, 1.0); // red
float4 color2 = float4(0.9, 0.9, 0.9, 1.0); // White/Light Gray
float4 color3 = float4(0.8, 0.2, 1.0, 1.0); // Purple/Magenta
float4 color4 = float4(0.4, 0.2, 0.8, 1.0); // Deep Purple
float4 color5 = float4(0.2, 0.6, 1.0, 1.0); // Cyan/Blue
float4 color6 = float4(0.9, 0.9, 0.9, 1.0); // White/Light Gray
float4 color7 = float4(0.9, 0.9, 0.9, 1.0); // White/Light Gray
float4 color8 = float4(1.0, 0.9, 0.2, 1.0); // Yellow

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
    // Apply pixelation if enabled
    if (pixelation > 0.0)
    {
        float pixelSize = 1.0 / pixelation;
        coords.x = floor(coords.x / pixelSize) * pixelSize;
        coords.y = floor(coords.y / pixelSize) * pixelSize;
    }
    
    float4 baseColor = tex2D(baseTexture, coords);
    if (!any(baseColor))
        return baseColor;
    
    float2 centeredCoords = coords - 0.5;
    
    // Apply rotation
    float cosR = cos(rotation);
    float sinR = sin(rotation);
    float2 rotatedCoords = float2(
        centeredCoords.x * cosR - centeredCoords.y * sinR,
        centeredCoords.x * sinR + centeredCoords.y * cosR
    ) + 0.5;
    
    float2 uv = rotatedCoords;
    
    // Distance from center for various effects
    float distanceFromCenter = length(uv - 0.5) * 2.0;
    
    // Rotate UVs FIRST, before any distortion
    float cosNR = cos(noiseRotation);
    float sinNR = sin(noiseRotation);
    float2 centeredUV = uv - 0.5;
    float2 rotatedUV = float2(
        centeredUV.x * cosNR - centeredUV.y * sinNR,
        centeredUV.x * sinNR + centeredUV.y * cosNR
    ) + 0.5;
    
    // === "Blow up" effect to make it look spherical ===
    float blownUpUVX = pow(abs(rotatedUV.x - 0.5) * 2.0, blowUpPower);
    float blownUpUVY = pow(abs(rotatedUV.y - 0.5) * 2.0, blowUpPower);
    
    float2 blownUpUV = float2(
        -blownUpUVY * blowUpSize * 0.5 + rotatedUV.x * (1.0 + blownUpUVY * blowUpSize),
        -blownUpUVX * blowUpSize * 0.5 + rotatedUV.y * (1.0 + blownUpUVX * blowUpSize)
    );
    
    // Scale the noise
    blownUpUV *= noiseScale;
    
    // Scroll effect
    blownUpUV.y = blownUpUV.y + time * scrollSpeed;
    
    // Sample noise for primary pattern
    float combinedNoise = tex2D(noiseTexture, blownUpUV).r;

    combinedNoise = ((combinedNoise - 0.5) * noiseContrast) + 0.5;
    
    // Add spherical shading (fresnel-like)
    combinedNoise += pow(distanceFromCenter, 6.0) * 0.3;
    
    // Clamp noise to 0-1 range
    combinedNoise = saturate(combinedNoise);
    
    // Map brightness to expanded gradient colors (8 colors instead of 4)
    float4 marbleColor;
    if (combinedNoise < 0.125)
        marbleColor = lerp(color1, color2, combinedNoise * 8.0);
    else if (combinedNoise < 0.25)
        marbleColor = lerp(color2, color3, (combinedNoise - 0.125) * 8.0);
    else if (combinedNoise < 0.375)
        marbleColor = lerp(color3, color4, (combinedNoise - 0.25) * 8.0);
    else if (combinedNoise < 0.5)
        marbleColor = lerp(color4, color5, (combinedNoise - 0.375) * 8.0);
    else if (combinedNoise < 0.625)
        marbleColor = lerp(color5, color6, (combinedNoise - 0.5) * 8.0);
    else if (combinedNoise < 0.75)
        marbleColor = lerp(color6, color7, (combinedNoise - 0.625) * 8.0);
    else if (combinedNoise < 0.875)
        marbleColor = lerp(color7, color8, (combinedNoise - 0.75) * 8.0);
    else
        marbleColor = lerp(color8, color1, (combinedNoise - 0.875) * 8.0);
    
    // Add rim highlight for glossy sphere look
    float rimLight = pow(1.0 - distanceFromCenter, 3.0);
    marbleColor.rgb -= rimLight * 0.25;
    
    // Circular mask with harder edge for more solid appearance
    float circleMask = 1.0 - smoothstep(0.95, 1.0, distanceFromCenter);
    
    marbleColor.a = baseColor.a * circleMask;
    
    if (marbleColor.a < 0.01)
        return float4(0, 0, 0, 0);
    
    return marbleColor;
}

technique Technique1
{
    pass MarblePass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}