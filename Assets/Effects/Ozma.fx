sampler2D uImage0 : register(s0);
sampler2D uImage1 : register(s1);
sampler2D uImage2 : register(s2);
float uTime;
float3 uRotation;
float uRotSpeed;
float uSwirlStrength;
float uSaturation;
float uPixelSize;
float3 uOutlineColor;

float4x4 rotationMatrix(float3 axis, float angle)
{
    axis = normalize(axis);
    float s = sin(angle);
    float c = cos(angle);
    float oc = 1.0 - c;
    
    return float4x4(
        oc * axis.x * axis.x + c, oc * axis.x * axis.y - axis.z * s, oc * axis.z * axis.x + axis.y * s, 0.0,
        oc * axis.x * axis.y + axis.z * s, oc * axis.y * axis.y + c, oc * axis.y * axis.z - axis.x * s, 0.0,
        oc * axis.z * axis.x - axis.y * s, oc * axis.y * axis.z + axis.x * s, oc * axis.z * axis.z + c, 0.0,
        0.0, 0.0, 0.0, 1.0
    );
}

float3 rotatePoint(float3 p, float3 rotation)
{
    float4x4 rotX = rotationMatrix(float3(1, 0, 0), rotation.x);
    float4x4 rotY = rotationMatrix(float3(0, 1, 0), rotation.y);
    float4x4 rotZ = rotationMatrix(float3(0, 0, 1), rotation.z);
    
    p = mul(float4(p, 1.0), rotX).xyz;
    p = mul(float4(p, 1.0), rotY).xyz;
    p = mul(float4(p, 1.0), rotZ).xyz;
    
    return p;
}

float3 saturateColor(float3 color, float saturation)
{
    float gray = dot(color, float3(0.299, 0.587, 0.114));
    return lerp(float3(gray, gray, gray), color, saturation);
}

float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0
{
    float2 pixelatedCoord = texCoord;
    if (uPixelSize > 1.0)
    {
        pixelatedCoord = floor(texCoord * uPixelSize) / uPixelSize;
    }
    
    float2 uv = pixelatedCoord * 2.0 - 1.0;
    
    float dist = length(uv);
    
    if (dist >= 1.0)
        return float4(0, 0, 0, 0);
    
    float z = sqrt(1.0 - dist * dist);
    
    float3 spherePoint = float3(uv.x, uv.y, z);
    
    spherePoint = rotatePoint(spherePoint, uRotation);
    
    bool isLightHalf = spherePoint.y < 0.0;
    
    float hemisphereRotation = uTime * uRotSpeed * 2.0;
    if (isLightHalf)
        spherePoint = mul(float4(spherePoint, 1.0), rotationMatrix(float3(0, 1, 0), -hemisphereRotation)).xyz;
    else
        spherePoint = mul(float4(spherePoint, 1.0), rotationMatrix(float3(0, 1, 0), hemisphereRotation)).xyz;
    
    float phi = atan2(spherePoint.z, spherePoint.x);
    float theta = asin(clamp(spherePoint.y, -1.0, 1.0));
    
    float radialDist;
    if (isLightHalf)
        radialDist = 1.0 + (theta / (3.14159265359 / 2.0));
    else
        radialDist = 1.0 - (theta / (3.14159265359 / 2.0));
    
    float swirlAmount = radialDist;
    if(isLightHalf)
        swirlAmount *= uSwirlStrength;
    else
        swirlAmount *= -uSwirlStrength;
    
    float swirledPhi = phi + swirlAmount;
    
    float2 textureUV;
    textureUV.x = 0.5 + radialDist * cos(swirledPhi) * 0.5;
    textureUV.y = 0.5 + radialDist * sin(swirledPhi) * 0.5;
    
    float4 color;
    if (isLightHalf)
        color = tex2D(uImage1, textureUV);
    else
        color = tex2D(uImage2, textureUV);
    
    color.rgb = saturateColor(color.rgb, uSaturation);
    
    float equatorDist = abs(spherePoint.y);
    
    float coreWidth = 0.03;
    float coreStrength = smoothstep(coreWidth, 0.0, equatorDist);
    
    float gradientWidth = 0.15;
    float gradientStrength = smoothstep(gradientWidth, 0.0, equatorDist);
    gradientStrength = pow(gradientStrength, 2.0);
    
    float totalOutline = max(coreStrength * 0.8, gradientStrength * 0.4);
    color.rgb = lerp(color.rgb, uOutlineColor, totalOutline);
    
    return color;
}

technique Technique1
{
    pass AutoloadPass
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
}