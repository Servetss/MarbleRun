Shader "Custom/Track"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
		_ArrowColor ("Arrow color", Color) = (1,1,1,1)
		_BorderColor ("Border color", Color) = (1,1,1,1)

        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

		struct Fragment
		{
			float4 vertex : POSITION;
			float2 uv_MainTex : TEXCOORD0;
			float2 uv2 : TEXCOORD1;
		};

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _ArrowColor;
        fixed4 _BorderColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);

			if (c.r < 0.5f && c.g < 0.5 && c.b < 0.5)
			{
				o.Albedo = _ArrowColor;
			}
			else if (c.r > 0.6f && c.g < 0.4f && c.b > 0.6f)
			{
				o.Albedo = _BorderColor;
			}
			else
			{
				o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			}

            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            //o.Alpha = c.a;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
