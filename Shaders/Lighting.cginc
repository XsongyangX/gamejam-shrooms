// Upgrade NOTE: replaced 'defined LIGHTING_INCLUDED' with 'defined (LIGHTING_INCLUDED)'

#if defined(LIGHTING_INCLUDED) 
#else

float3 _LightPosition[8];
float3 _LightColor[8];
float3 _LightParams[8]; // x = intensity, y = radius

float3 _LightAmbient;

float _SpecularSmoothness;
float _SpecularIntensity;

float3 GetLighting(float3 position, float3 normal) {
	
	float3 result = float3(0, 0, 0);

	result += _LightAmbient;

	for (int i = 0; i < 8; i++)
	{
		float3 dir = position - _LightPosition[i];
		if (length(dir) <= _LightParams[i].y) {
			float ndotl = saturate(dot(normalize(-dir), normalize(normal)));
			float fade = 1 - length(dir) / _LightParams[i].y;

			float3 view = position - _WorldSpaceCameraPos;
			float3 reflectDir = reflect(-dir, normal);
			float rdv = saturate(dot(normalize(reflectDir), normalize(view)));
			float spec = pow(rdv, _SpecularSmoothness * 100);

			result += _LightColor[i] * ndotl * fade + _LightColor[i] * spec * _SpecularIntensity * ndotl * fade;
		}
	}

	return result;
}

#define LIGHTING_INCLUDED

#endif