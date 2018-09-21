// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//
// KinoFog - Deferred fog effect
//
// Copyright (C) 2015 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
Shader "Hidden/Kino/Fog"
{
    Properties
    {
        _MainTex ("-", 2D) = "" {}
        _FogColor ("-", Color) = (0, 0, 0, 0)
    }
    CGINCLUDE

    #include "UnityCG.cginc"
	#include "Fog.cginc"

	#pragma multi_compile KINO_FOG_LINEAR KINO_FOG_EXP KINO_FOG_EXP2
	#pragma multi_compile _ KINO_FOG_RADIAL_DIST
	#pragma multi_compile _ KINO_FOG_USE_SKYBOX

    sampler2D _MainTex;
    float4 _MainTex_TexelSize;

    sampler2D_float _CameraDepthTexture;

    struct v2f
    {
        float4 pos : SV_POSITION;
        float2 uv : TEXCOORD0;
        float2 uv_depth : TEXCOORD1;
        float3 ray : TEXCOORD2;
    };

    v2f vert(appdata_full v)
    {
        v2f o;

        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv = v.texcoord.xy;
        o.uv_depth = v.texcoord.xy;
        o.ray = v.texcoord1.xyz;

    #if UNITY_UV_SkyStartS_AT_TOP
        if (_MainTex_TexelSize.y < 0.0) o.uv.y = 1.0 - o.uv.y;
    #endif

        return o;
    }

    half4 frag(v2f i) : SV_Target
    {
        half4 sceneColor = tex2D(_MainTex, i.uv);

        // Reconstruct world space position & direction towards this screen pixel.
        float zsample = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv_depth);
        float depth = Linear01Depth(zsample * (zsample < 1.0));
		return half4(depth, depth, depth, 1);

        // Compute fog amount.
        float g = ComputeDistance(i.ray, depth);
        half fog = ComputeFogFactor(g);

        // Look up the skybox color.
        half4 skyColor = SkyColor(normalize(i.ray).xyz);
        // Lerp between source color to fog color with the fog amount.
        return lerp(_FogColor, sceneColor, fog);
    }

    ENDCG
    SubShader
    {
        ZTest Always Cull Off ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
    Fallback off
}
