Shader "Custom/Transition" {
     Properties {
         _MainTex ("Default (RGB)", 2D) = "white" {}
         _DisplayTex ("Display (RGB)", 2D) = "white" {}
         _Guide ("Guide (RGB)", 2D) = "white" {}
         _Threshold("Threshold",Range(0,1))=0
         _ScaleWidth("Scale Width", Float)=0
         _ScaleHeight("Scale Height", Float)=0
     }
     SubShader {
         Tags { "RenderType"="Opaque" }
         LOD 200
         
         CGPROGRAM
         #pragma surface surf Lambert
 
         sampler2D _MainTex;
         sampler2D _DisplayTex;
         sampler2D _Guide;
         float _Threshold;
         float _ScaleWidth;
         float _ScaleHeight;
		 float4 _MainTex_TexelSize;
		 float4 _DisplayTex_TexelSize;
		 float4 _Guide_TexelSize;
 
         float2 SquareUVs(float4 size, float2 oldUVs) {
		     //Scales the image up to match width or height (whichever is smaller)
			 // and clips off the rest, centering it and keeping aspect ratio.

			 float width = _ScaleWidth / size.z;
			 float height = _ScaleHeight / size.w;
			 float newU = oldUVs.x;
			 float newV = oldUVs.y;
			 float aspect;
			 if(width < height) {
			     //height is big- keep the height and make it wider
			     aspect = height / width;
				 newU = newU / aspect + 0.5f - 0.5f / aspect;
			 }
			 else {
                 //width is big- keep the width and make it taller
			     aspect = width / height;
				 newV = newV / aspect + 0.5f - 0.5f / aspect;
			 }
			 return float2(newU, newV);
		 }

         struct Input {
             float2 uv_MainTex;
             float2 uv_DisplayTex;
             float2 uv_Guide;
         };
 
         void surf (Input IN, inout SurfaceOutput o) {
		     float2 mainUVs = SquareUVs(_MainTex_TexelSize, IN.uv_MainTex);
             half4 c = tex2D (_MainTex, mainUVs);

			 float2 displayUVs = SquareUVs(_DisplayTex_TexelSize, IN.uv_DisplayTex);
			 //displayUVs = float2(1 - displayUVs.x, 1 - displayUVs.y); //rotate the image 180 degrees for some reason
             half4 d = tex2D (_DisplayTex, displayUVs);

			 float2 guideUVs = SquareUVs(_Guide_TexelSize, IN.uv_Guide);
             half4 g = tex2D (_Guide, guideUVs);
             if((g.r+g.g+g.b)*0.33333f<_Threshold)
                 o.Albedo = d.rgb;
             else
                 o.Albedo = c.rgb;
             o.Alpha = c.a;
         }
         ENDCG
     } 
     FallBack "Diffuse"
 }