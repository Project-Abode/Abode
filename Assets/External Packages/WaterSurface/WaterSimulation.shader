Shader "Water/Simulation"
{

Properties
{
    _S2("PhaseVelocity^2", Range(0.0, 0.5)) = 0.2
    [PowerSlider(0.01)]
    _Atten("Attenuation", Range(0.0, 1.0)) = 0.999
    _DeltaUV("Delta UV", Float) = 3
    _Limit("Displacement Limit", Range(0.0, 1.0)) = .1
    _HitSpread("Hit Spread Curve", Float) = .2
}

CGINCLUDE

#include "UnityCustomRenderTexture.cginc"

half _S2;
half _Atten;
float _DeltaUV;
float _Limit;
float _HitSpread;

int _ArrayLength;
float4 _Array[1000];

float4 frag(v2f_customrendertexture i) : SV_Target
{
    float2 uv = i.globalTexcoord;

    float3 duv = float3(1.0 / _CustomRenderTextureWidth, 1.0 / _CustomRenderTextureHeight, 0) * _DeltaUV;

    float y0 = (uv - duv.zy).y > .1;
    float y1 = (uv + duv.zy).y < .9;
    float x0 = (uv - duv.xz).x > .1;
    float x1 = (uv + duv.xz).x < .9;
    float inbounds = y0 * y1 * x0 * x1;
    float sum = y0 + y1 + x0 + x1;

    float2 c = tex2D(_SelfTexture2D, uv);

    float p = (2 * c.r -  c.g + _S2 * (
        tex2D(_SelfTexture2D, uv - duv.zy).r +
        tex2D(_SelfTexture2D, uv + duv.zy).r +
        tex2D(_SelfTexture2D, uv - duv.xz).r +
        tex2D(_SelfTexture2D, uv + duv.xz).r - 4 * c.r)) * _Atten;

    return float4(p, c.r, 0, 0);
}

float4 frag_amp(v2f_customrendertexture i) : SV_Target
{
    float4 dir = (float4)0; // dir.xz are unused, used to be for defining ripple shape (didn't work well)
    int j;
    for (j = 0; j < _ArrayLength - 1; j += 2)
    {
        dir += lerp((float4)0, _Array[j], pow(max(_Array[j + 1].z - length(i.globalTexcoord.xy - _Array[j + 1].xy), 0) / _Array[j + 1].z, _HitSpread));
    }

    if (abs(dir.w) <= 1e-6) discard;

    float t = saturate(1 - (length(i.localTexcoord.xy - .5))); // circle from hitpoint
    float h = dir.w * (1 + abs(dir.y)); // scalar with vertical movement

    float temp = h * t + tex2D(_SelfTexture2D, i.globalTexcoord).r * (1 - t);

    return float4(clamp(temp, -_Limit, _Limit), tex2D(_SelfTexture2D, i.globalTexcoord).g * (1 - t), 0, 0);
}

ENDCG

SubShader
{
    Cull Off ZWrite Off ZTest Always

    Pass
    {
        Name "Update"
        CGPROGRAM
        #pragma vertex CustomRenderTextureVertexShader
        #pragma fragment frag
        ENDCG
    }

    Pass
    {
        CGPROGRAM
        #pragma vertex CustomRenderTextureVertexShader
        #pragma fragment frag_amp
        ENDCG
    }
}

}