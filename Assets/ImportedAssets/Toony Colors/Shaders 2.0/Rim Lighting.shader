﻿// Toony Colors Free
// (c) 2012,2016 Jean Moreno


// Want more features ? Check out Toony Colors Pro+Mobile 2 !
// http://www.jeanmoreno.com/toonycolorspro/


Shader "Toony Colors Free/Rim Lighting"
{
	Properties
	{
		//TOONY COLORS
		_Color ("Color", Color) = (0.5,0.5,0.5,1.0)
		_HColor ("Highlight Color", Color) = (0.6,0.6,0.6,1.0)
		_SColor ("Shadow Color", Color) = (0.3,0.3,0.3,1.0)

		_ArrowColor ("Arrow color", Color) = (1,1,1,1)
		_BorderColor ("Border color", Color) = (1,1,1,1)
		
		//DIFFUSE
		_MainTex ("Main Texture (RGB)", 2D) = "white" {}
		
		//TOONY COLORS RAMP
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {}
		
		//RIM LIGHT
		_RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.6)
		_RimMin ("Rim Min", Range(0,1)) = 0.5
		_RimMax ("Rim Max", Range(0,1)) = 1.0
		
		
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		
		#pragma surface surf ToonyColorsCustom
		#pragma target 2.0
		#pragma glsl
		
		
		//================================================================
		// VARIABLES
		
		fixed4 _Color;
		sampler2D _MainTex;
		fixed4 _ArrowColor;
        fixed4 _BorderColor;
		
		fixed4 _RimColor;
		fixed _RimMin;
		fixed _RimMax;
		float4 _RimDir;
		
		struct Input
		{
			half2 uv_MainTex;
			float3 viewDir;
		};
		
		//================================================================
		// CUSTOM LIGHTING
		
		//Lighting-related variables
		fixed4 _HColor;
		fixed4 _SColor;
		sampler2D _Ramp;
		
		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Alpha;
		};
		
		inline half4 LightingToonyColorsCustom (SurfaceOutputCustom s, half3 lightDir, half3 viewDir, half atten)
		{
			s.Normal = normalize(s.Normal);
			fixed ndl = max(0, dot(s.Normal, lightDir)*0.5 + 0.5);
			
			fixed3 ramp = tex2D(_Ramp, fixed2(ndl,ndl));
		#if !(POINT) && !(SPOT)
			ramp *= atten;
		#endif
			_SColor = lerp(_HColor, _SColor, _SColor.a);	//Shadows intensity through alpha
			ramp = lerp(_SColor.rgb,_HColor.rgb,ramp);
			fixed4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp;
			c.a = s.Alpha;
		#if (POINT || SPOT)
			c.rgb *= atten;
		#endif
			return c;
		}
		
		
		//================================================================
		// SURFACE FUNCTION
		
		void surf (Input IN, inout SurfaceOutputCustom o)
		{
			fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
			
			o.Albedo = mainTex.rgb * _Color.rgb;
			o.Alpha = mainTex.a * _Color.a;
			
			//Rim
			float3 viewDir = normalize(IN.viewDir);
			half rim = 1.0f - saturate( dot(viewDir, o.Normal) );
			rim = smoothstep(_RimMin, _RimMax, rim);
			o.Emission += (_RimColor.rgb * rim) * _RimColor.a;

			if (mainTex.r < 0.5f && mainTex.g < 0.5 && mainTex.b < 0.5)
			{
				o.Albedo = _ArrowColor;
			}
			else if (mainTex.r > 0.6f && mainTex.g < 0.4f && mainTex.b > 0.6f)
			{
				o.Albedo = _BorderColor;
			}
			else
			{
				o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			}
		}
		
		ENDCG
	}
	
	Fallback "Diffuse"
	CustomEditor "TCF_MaterialInspector"
}
