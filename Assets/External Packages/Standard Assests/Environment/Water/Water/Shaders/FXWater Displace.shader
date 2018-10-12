// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Water/HQ Water Displace" {
Properties {
	_WaveScale ("Wave scale", Range (0.001,0.3)) = 0.063
	_ReflDistort ("Reflection distort", Range (0,1.5)) = 0.44
	_RefrDistort ("Refraction distort", Range (0,1.5)) = 0.40
	_RefrColor ("Refraction color", COLOR)  = ( .34, .85, .92, 1)
	[NoScaleOffset] _BumpMap ("Normalmap ", 2D) = "bump" {}
    _BumpStrength ("Normal Strength", Float) = 1
    [NoScaleOffset] _Fresnel ("Fresnel (A) ", 2D) = "gray" {}
    _FresnelCutoff ("Fresnel Cutoff", Float) = .02
	WaveSpeed ("Wave speed (map1 x,y; map2 x,y)", Vector) = (19,9,-16,-7)
    _ReflectionBias ("Reflection bias", Float) = 4
	[NoScaleOffset] _ReflectiveColor ("Reflective color (RGB) fresnel (A) ", 2D) = "" {}
	_HorizonColor ("Simple water horizon color", COLOR)  = ( .172, .463, .435, 1)
    [HideInInspector] _ReflectionTex0 ("Internal Reflection", 2D) = "" {}
    [HideInInspector] _ReflectionTex1 ("Internal Reflection", 2D) = "" {}
    [HideInInspector] _RefractionTex0 ("Internal Refraction", 2D) = "" {}
    [HideInInspector] _RefractionTex1("Internal Refraction", 2D) = "" {}

    _DispTex("Disp Texture", 2D) = "gray" {}
    _Displacement("Displacement", Range(0, 1.0)) = 0.3

    [KeywordEnum(Uniform, Edge)]
    _Tessellation("Tessellation Type", Float) = 0
    _TessellationUniform ("Tessellation Uniform", Range(1, 64)) = 1
    _TessellationEdgeLength ("Tessellation Edge Length", Range(5, 100)) = 50
}


// -----------------------------------------------------------
// Fragment program cards


Subshader {
	Tags { "WaterMode"="Refractive" "RenderType"="Opaque" }
	Pass {
CGPROGRAM
#pragma vertex tessVert
#pragma fragment frag
#pragma hull MyHullProgram
#pragma domain MyDomainProgram
#pragma multi_compile_fog
#pragma multi_compile _TESSELLATION_UNIFORM _TESSELLATION_EDGE
#pragma multi_compile WATER_REFRACTIVE WATER_REFLECTIVE WATER_SIMPLE

#pragma multi_compile KINO_FOG_LINEAR KINO_FOG_EXP KINO_FOG_EXP2
#pragma multi_compile _ KINO_FOG_RADIAL_DIST
#pragma multi_compile _ KINO_FOG_USE_SKYBOX

#if defined (WATER_REFLECTIVE) || defined (WATER_REFRACTIVE)
#define HAS_REFLECTION 1
#endif
#if defined (WATER_REFRACTIVE)
#define HAS_REFRACTION 1
#endif

#include "UnityCG.cginc"
#include "UnityLightingCommon.cginc" // for _LightColor0
#include "Assets/External Packages/Kino/Fog/Shader/Fog.cginc"

uniform float4 _WaveScale4;
uniform float4 _WaveOffset;

#if HAS_REFLECTION
uniform float _ReflDistort;
#endif
#if HAS_REFRACTION
uniform float _RefrDistort;
#endif

// Displacement params
float _Displacement;
sampler2D _DispTex;
float4 _DispTex_TexelSize;
float _TessellationUniform;
float _TessellationEdgeLength;

#if defined (WATER_REFLECTIVE) || defined (WATER_REFRACTIVE)
sampler2D _ReflectionTex0;
sampler2D _ReflectionTex1;
#endif
#if defined (WATER_REFLECTIVE) || defined (WATER_SIMPLE)
sampler2D _ReflectiveColor;
#endif
#if defined (WATER_REFRACTIVE)
sampler2D _Fresnel;
sampler2D _RefractionTex0;
sampler2D _RefractionTex1;
uniform float4 _RefrColor;
#endif
#if defined (WATER_SIMPLE)
uniform float4 _HorizonColor;
#endif
sampler2D _BumpMap;
float _BumpStrength;
float _FresnelCutoff;
float _ReflectionBias;

struct TessellationControlPoint {
    float4 vertex : INTERNALTESSPOS;
    float3 normal : NORMAL;
    float4 tangent : TANGENT;
    float2 uv : TEXCOORD0;
    float2 uv1 : TEXCOORD1;
    float2 uv2 : TEXCOORD2;
};

struct appdata {
	float4 vertex : POSITION;
	float3 normal : NORMAL;
    float4 tangent : TANGENT;
    float2 uv : TEXCOORD0;
    float2 uv1 : TEXCOORD1;
    float2 uv2 : TEXCOORD2;
};

struct v2f {
	float4 pos : SV_POSITION;
    float2 uv_DispTex : TEXCOORD0;
	#if defined(HAS_REFLECTION) || defined(HAS_REFRACTION)
		float4 ref : TEXCOORD1;
		float4 bumpuv : TEXCOORD2;
        float2 eyeIndexAndDisp : TEXCOORD3;
		float3 viewDir : TEXCOORD4;
	#else
		float4 bumpuv : TEXCOORD1;
        float2 eyeIndexAndDisp : TEXCOORD2;
		float3 viewDir : TEXCOORD3;
	#endif
	float3 dispNormal : TEXCOORD5;
	float3 dispTangent : TEXCOORD6;
	float3 dispBinormal : TEXCOORD7;
};

TessellationControlPoint tessVert(appdata v)
{
    TessellationControlPoint p;
    p.vertex = v.vertex;
    p.normal = v.normal;
    p.tangent = v.tangent;
    p.uv = v.uv;
    p.uv1 = v.uv1;
    p.uv2 = v.uv2;
    return p;
}

v2f vert(appdata v)
{
	v2f o;

    // scroll bump waves
    float4 temp;
    float4 wpos = mul (unity_ObjectToWorld, v.vertex);
    temp.xyzw = wpos.xzxz * _WaveScale4 + _WaveOffset;
    o.bumpuv.xy = temp.xy;
    o.bumpuv.zw = temp.wz;

    // vertex displacement
    float d = tex2Dlod(_DispTex, float4(v.uv.xy, 0, 0)).r * _Displacement;
    v.vertex.xyz += normalize(v.normal) * d;
	o.uv_DispTex = v.uv.xy;

    float3 duv = float3(_DispTex_TexelSize.xy, 0);
    half v1 = tex2Dlod(_DispTex, float4(v.uv.xy - duv.xz, 0, 0)).y;
    half v2 = tex2Dlod(_DispTex, float4(v.uv.xy + duv.xz, 0, 0)).y;
    half v3 = tex2Dlod(_DispTex, float4(v.uv.xy - duv.zy, 0, 0)).y;
    half v4 = tex2Dlod(_DispTex, float4(v.uv.xy + duv.zy, 0, 0)).y;
    o.dispNormal = normalize(float3(v1 - v2, v3 - v4, 0.06));
	o.dispTangent = normalize(cross(float3(0,1,0), o.dispNormal));
	o.dispBinormal = normalize(cross(o.dispNormal, o.dispTangent));

    // displacement by normals; didn't work super well
    half3 bump1 = UnpackNormal(tex2Dlod( _BumpMap, float4(o.bumpuv.xy, 0, 0) )).rgb;
    half3 bump2 = UnpackNormal(tex2Dlod( _BumpMap, float4(o.bumpuv.zw, 0, 0) )).rgb;
    half3 bump = (bump1 + bump2) * 0.5;
    bump *= _BumpStrength;
    v.vertex.y += bump.y * .05;

    o.pos = UnityObjectToClipPos(v.vertex);
    o.eyeIndexAndDisp.y = (float3)d * 4; // const mult to emphasize the effect
	
	// object space view direction (will normalize per pixel)
	o.viewDir.xzy = WorldSpaceViewDir(v.vertex);
	
	#if defined(HAS_REFLECTION) || defined(HAS_REFRACTION)
	o.ref = ComputeNonStereoScreenPos(o.pos);

	#ifdef UNITY_SINGLE_PASS_STEREO
		o.eyeIndexAndDisp.x = unity_StereoEyeIndex;
	#else
	// When not using single pass stereo rendering, eye index must be determined by testing the
	// sign of the horizontal skew of the projection matrix.
	if (unity_CameraProjection[0][2] > 0) {
		o.eyeIndexAndDisp.x = 1.0;
	} else {
		o.eyeIndexAndDisp.x = 0.0;
	}
	#endif
	#endif

    return o;
}


// Tessellation stuff taken from catlikecoding.com

struct TessellationFactors {
    float edge[3] : SV_TessFactor;
    float inside : SV_InsideTessFactor;
};

TessellationControlPoint MyTessellationVertexProgram (appdata v) {
    TessellationControlPoint p;
    p.vertex = v.vertex;
    p.normal = v.normal;
    p.tangent = v.tangent;
    p.uv = v.uv;
    p.uv1 = v.uv1;
    p.uv2 = v.uv2;
    return p;
}

float TessellationEdgeFactor (float3 p0, float3 p1) {
#if defined(_TESSELLATION_EDGE)
    float edgeLength = distance(p0, p1);

    float3 edgeCenter = (p0 + p1) * 0.5;
    float viewDistance = distance(edgeCenter, _WorldSpaceCameraPos);

    return edgeLength * _ScreenParams.y /
        (_TessellationEdgeLength * viewDistance);
#else
    return _TessellationUniform;
#endif
}

TessellationFactors MyPatchConstantFunction (
    InputPatch<TessellationControlPoint, 3> patch
) {
    float3 p0 = mul(unity_ObjectToWorld, patch[0].vertex).xyz;
    float3 p1 = mul(unity_ObjectToWorld, patch[1].vertex).xyz;
    float3 p2 = mul(unity_ObjectToWorld, patch[2].vertex).xyz;
    TessellationFactors f;
    f.edge[0] = TessellationEdgeFactor(p1, p2);
    f.edge[1] = TessellationEdgeFactor(p2, p0);
    f.edge[2] = TessellationEdgeFactor(p0, p1);
    f.inside =
        (TessellationEdgeFactor(p1, p2) +
            TessellationEdgeFactor(p2, p0) +
            TessellationEdgeFactor(p0, p1)) * (1 / 3.0);
    return f;
}

[UNITY_domain("tri")]
[UNITY_outputcontrolpoints(3)]
[UNITY_outputtopology("triangle_cw")]
[UNITY_partitioning("fractional_odd")]
[UNITY_patchconstantfunc("MyPatchConstantFunction")]
TessellationControlPoint MyHullProgram (
    InputPatch<TessellationControlPoint, 3> patch,
    uint id : SV_OutputControlPointID
) {
    return patch[id];
}

[UNITY_domain("tri")]
v2f MyDomainProgram (
    TessellationFactors factors,
    OutputPatch<TessellationControlPoint, 3> patch,
    float3 barycentricCoordinates : SV_DomainLocation
) {
    appdata data;

#define MY_DOMAIN_PROGRAM_INTERPOLATE(fieldName) data.fieldName = \
		patch[0].fieldName * barycentricCoordinates.x + \
		patch[1].fieldName * barycentricCoordinates.y + \
		patch[2].fieldName * barycentricCoordinates.z;

    MY_DOMAIN_PROGRAM_INTERPOLATE(vertex)
        MY_DOMAIN_PROGRAM_INTERPOLATE(normal)
        MY_DOMAIN_PROGRAM_INTERPOLATE(tangent)
        MY_DOMAIN_PROGRAM_INTERPOLATE(uv)
        MY_DOMAIN_PROGRAM_INTERPOLATE(uv1)
        MY_DOMAIN_PROGRAM_INTERPOLATE(uv2)

        return vert(data);
}

// Tessellation stuff taken from catlikecoding.com



half4 frag( v2f i ) : SV_Target
{
	float3 viewDir = -i.viewDir.xzy;
    i.viewDir = normalize(i.viewDir);

    half3 n = normalize(i.dispNormal);
	half3 t = normalize(i.dispTangent);
	half3 b = normalize(i.dispBinormal);

	// combine two scrolling bumpmaps into one
	half3 bump1 = UnpackNormal(tex2D( _BumpMap, i.bumpuv.xy )).rgb;
	half3 bump2 = UnpackNormal(tex2D( _BumpMap, i.bumpuv.zw )).rgb;
	half3 bump = ((bump1 + bump2) * 0.5);

    //bump = lerp(normalize(n), bump, 1 - saturate(abs(i.eyeIndexAndDisp.y)));
	bump = t * bump.x + b * bump.y + n * bump.z;
    bump *= _BumpStrength;
	
	// fresnel factor
	half fresnelFac = abs(dot( i.viewDir, bump ));
    fresnelFac += step(fresnelFac, _FresnelCutoff) * _FresnelCutoff;
	
	// perturb reflection/refraction UVs by bumpmap, and lookup colors
	
	#if HAS_REFLECTION
	float4 uv1 = i.ref; uv1.xy += bump * _ReflDistort;
	half4 refl0 = tex2Dproj( _ReflectionTex0, UNITY_PROJ_COORD(uv1) );
	half4 refl1 = tex2Dproj( _ReflectionTex1, UNITY_PROJ_COORD(uv1) );
	half4 refl = lerp(refl0, refl1, i.eyeIndexAndDisp.x);
	#endif
	#if HAS_REFRACTION
	float4 uv2 = i.ref; uv2.xy -= bump * _RefrDistort;
	half4 refr0 = tex2Dproj(_RefractionTex0, UNITY_PROJ_COORD(uv2)) * _RefrColor;
	half4 refr1 = tex2Dproj(_RefractionTex1, UNITY_PROJ_COORD(uv2)) * _RefrColor;
	half4 refr = lerp(refr0, refr1, i.eyeIndexAndDisp.x);
	#endif
	
	// final color is between refracted and reflected based on fresnel
	half4 color;
	
	#if defined(WATER_REFRACTIVE)
	half fresnel = UNITY_SAMPLE_1CHANNEL( _Fresnel, float2(fresnelFac,fresnelFac) );
	color = lerp( refr, refl, saturate(_ReflectionBias * (fresnel * (.5 + abs(i.eyeIndexAndDisp.y) * 5) )));
	#endif
	
	#if defined(WATER_REFLECTIVE)
	half4 water = tex2D( _ReflectiveColor, float2(fresnelFac,fresnelFac) );
	color.rgb = lerp( water.rgb, refl.rgb, water.a );
	color.a = refl.a * water.a;
	#endif
	
	#if defined(WATER_SIMPLE)
	half4 water = tex2D( _ReflectiveColor, float2(fresnelFac,fresnelFac) );
	color.rgb = lerp( water.rgb, _HorizonColor.rgb, water.a );
	color.a = _HorizonColor.a;
	#endif

    //half nl = max(0, dot(normalize(float3(v1 - v2, v3 - v4, 0.3)), _WorldSpaceLightPos0.xyz));

    //color *= nl * _LightColor0;

	// hack to modulate saturation based on displacement
    color *= lerp(1, .3, clamp(-i.eyeIndexAndDisp.y * 2, 0, 1) );
    color *= lerp(1, 5, clamp(i.eyeIndexAndDisp.y, 0, 1) );

	#if defined(KINO_FOG_USE_SKYBOX)

	float dist = length(viewDir);
	float3 skyColor = SkyColor(normalize(viewDir.xyz));
	float fogFactor = 1 - ComputeFogFactor(dist);
	color.rgb = lerp(color, skyColor, saturate(fogFactor));

	#endif

    return color;
}
ENDCG

	}
}

}
