Shader "Custom/Dirt" {
     Properties {
         _MainTex ("Base (RGB)", 2D) = "white" {}
         _DetailTex ("Detail (RGB)", 2D) = "white" {}
         _Guide ("Guide (RGB)", 2D) = "white" {}
         _Wetness("Wetness",Range(0,1))=0
         _BumpMap ("Bump Map", 2D) = "bump" {}
     }
     SubShader {
         Tags { "RenderType"="Opaque" }
         LOD 200
         
         CGPROGRAM
         #pragma surface surf Lambert
 
         sampler2D _MainTex;
         sampler2D _DetailTex;
         sampler2D _Guide;
         sampler2D _BumpMap;
         float _Wetness;
 
         struct Input {
             float2 uv_MainTex;
             float2 uv_DetailTex;
             float2 uv_Guide;
             float2 uv_BumpMap;
         };
 
         void surf (Input IN, inout SurfaceOutput o) {
             half4 c = tex2D (_MainTex, IN.uv_MainTex);
             half4 d = tex2D (_DetailTex, IN.uv_DetailTex);
             half4 g = tex2D (_Guide, IN.uv_Guide);
             if((g.r+g.g+g.b)*0.33333f<_Wetness)
                 o.Albedo = d.rgb;
                     else
                         o.Albedo = c.rgb;
             o.Alpha = c.a;
             o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
         }
         ENDCG
     } 
     FallBack "Diffuse"
 }
