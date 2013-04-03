float4x4 worldViewProjMatrix;
float4x4 inverseWorldMatrix;

float4 Light_Ambient;

float4 Dominant_Light_Position;
float4 Dominant_Light_Color;

vector Eye_Position;

struct VSOutput
{
    float4 Position : POSITION;
	float3 Color : COLOR0;
};

VSOutput mainVS(float4 inPos : POSITION, float4 inNormal : NORMAL, float4 inColor : COLOR0)
{
	VSOutput Out = (VSOutput)0;

	Out.Position = mul(inPos, worldViewProjMatrix);

	inNormal=normalize(inNormal);

	// -- Diffuse Lighting -- \\
	// Light Direction (transformed into model space)
	vector objectLight = mul(Dominant_Light_Position, inverseWorldMatrix);
	vector lightDirection = normalize(objectLight - inPos);

	// Lambert Diffuse Lighting
	float diffuse = max(0, dot(inNormal, lightDirection));

	// -- End Of Diffuse Lighting -- \\

	// -- Specular Lighting -- \\
	// Eye Vector (transformed into model space)
	vector objectEye = mul(Eye_Position, inverseWorldMatrix);
	vector eyeDirection = normalize(objectEye - inPos);
	vector halfVect = normalize((lightDirection+eyeDirection)/2);
	
	// Blinn-Phong Specular
	float specular = max(0, pow(dot(inNormal, halfVect), 64));
	// -- End of Specular Lighting -- \\

	// Final Lighting Calculation
	
	// Begin with vertex color
	float4 color = inColor + (inColor * specular) + (inColor * diffuse) + Dominant_Light_Color;

	Out.Color=color;

	return Out;
}


float4 mainPS(float4 inColor : COLOR0) : COLOR0
{
	return inColor;
}

//// Ambient Only Lighting \\
//VSOutput amientOnlyVS(float4 inPos : POSITION)
//{
	//VSOutput Out = (VSOutput)0;
//
	//Out.Position = mul(inPos, worldViewProjMatrix);
//
	//float4 color = Light_Ambient;
//
	//Out.Color=color;
//
	//return Out;
//}
//
//
//float4 ambientOnlyPS(float4 inColor : COLOR0) : COLOR
//{
	//return inColor;
//}
//// Ambient Only End \\

// Use All Lighting Types

technique AmbientDiffuseSpecular
{
	pass P0
	{
		vertexShader = compile vs_2_0 mainVS();
		pixelShader = compile ps_2_0 mainPS();
	}
}

//// Ambient Only Lighting
//technique AmbientOnly
//{
	//pass P0
	//{
		//vertexShader = compile vs_2_0 amientOnlyVS();
		//pixelShader = compile ps_2_0 ambientOnlyPS();
	//}
//}