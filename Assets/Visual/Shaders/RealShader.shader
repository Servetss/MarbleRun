Shader "Unlit/RealShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor ("Color", Color) = (1,1,1,1)

        _LightRange ("Light Range", Range(0.0, 5)) = 0.5

        [Space(30)]
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        _Fresnel("Fresnel", Range(0,1)) = 0


        [Space]
        [NoScaleOffset] _CubeMapTex ("Cubemap Texture", Cube) = "" {}
        [HDR]_CubemapColor ("Cubemap Color", Color) = (1,1,1,1)
        

        [Space(30)]
        _Roughness ("Roughness", Range(0.0, 30)) = 0.5

        _Metalic ("Metalic", Range(0.0, 4)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            #define L _WorldSpaceLightPos0.xyz
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                
                float3 normal : NORMAL;

                float3 ws_pos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            samplerCUBE _CubeMapTex;
            float4 _CubeMapTex_HDR;

            fixed4 _MainColor;

            fixed _LightRange;

            fixed4 _FresnelColor;
            fixed _Fresnel;
            
            fixed4 _CubemapColor;

            fixed _Roughness;
            fixed _Metalic;
            
            v2f vert (appdata v)
            {
                v2f o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.normal = UnityObjectToWorldNormal(v.normal);
                
                o.ws_pos = mul(unity_ObjectToWorld, v.vertex).xyz;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.normal = normalize(i.normal);
                
                fixed3 V = normalize(UnityWorldSpaceViewDir(i.ws_pos));
                
                fixed4 NdotL = dot(L, i.normal) * 0.3 + 0.6;

                fixed NdotV = 1 - saturate(dot(i.normal, V.xyz));

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * max(NdotL,_LightRange) * _MainColor;


                fixed3 R = reflect(-V, i.normal);
                fixed specular = dot(R, L);
                
                fixed3 skydata = texCUBE(_CubeMapTex, R).rgb;
                
                col.rgb += NdotV * _Fresnel * _FresnelColor;
                
                col.rgb += smoothstep(0,1, specular * _Roughness - (_Roughness - 1)) * 0.1;

                col.rgb += skydata * _CubemapColor * pow(NdotV, _Metalic);
                return col;
                
            }
            ENDCG
        }
    }
}
