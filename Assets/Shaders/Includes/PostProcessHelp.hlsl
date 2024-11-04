SAMPLER(sampler_point_clamp);

// Gaussian kernel
static const float kernel[3][3] = {
    { 0.06136, 0.12511, 0.06136 },
    { 0.12511, 0.24420, 0.12511 },
    { 0.06136, 0.12511, 0.06136 }
};

void GaussianBlur_float(float2 uv, float2 texelSize, out float3 result)
{
	float3 sum = float3(0, 0, 0);
	float3 sample = float3(0, 0, 0);
	float2 offset = float2(0, 0);
	for (int i = 0; i < 3; i++)
	{
		for (int j = 0; j < 3; j++)
		{
			offset = float2(i - 1, j - 1);
			sample = SAMPLE_TEXTURE2D(_MainTex, sampler_point_clamp, uv + offset * texelSize).rgb;
			sum += sample * kernel[i][j];
		}
	}
	result = sum;
	//result = SAMPLE_TEXTURE2D(_MainTex, sampler_point_clamp, uv).rgb;
}
