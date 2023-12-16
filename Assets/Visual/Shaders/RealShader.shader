Shader "Unlit/RealShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

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
                float3 texcoord : TEXCOORD3;
                
                float3 normal : NORMAL;

                float3 ws_pos : TEXCOORD1;
                float3 viewDirection : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            samplerCUBE _CubeMapTex;
            half4 _CubeMapTex_HDR;

            float _LightRange;

            float4 _FresnelColor;
            float _Fresnel;
            
            float4 _CubemapColor;

            float _Roughness;
            float _Metalic;
            
            v2f vert (appdata v)
            {
                v2f o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.texcoord = v.vertex;
                
                o.normal = UnityObjectToWorldNormal(v.normal);
                
                o.viewDirection = WorldSpaceViewDir(v.vertex);
                o.ws_pos = mul(unity_ObjectToWorld, v.vertex).xyz;
                
                return o;
            }

            inline fixed4 DiffuseColor(fixed4 col, fixed3 normal, fixed3 H, float range, float shadowStrenght)
            {
                fixed4 specular = pow(saturate(dot(H, normal)), range);

                return step(shadowStrenght, specular + col);
            }


            fixed4 frag (v2f i) : SV_Target
            {
                i.normal = normalize(i.normal);
                
                fixed3 V = normalize(UnityWorldSpaceViewDir(i.ws_pos));

                fixed3 H = normalize(L + V.xyz);
                
                fixed4 NdotL = dot(L, i.normal) * 0.3 + 0.6;

                float NdotV = 1- saturate(dot(i.normal, V.xyz));

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * NdotL;


                half3 R = reflect(-V , i.normal);
                half3 ROp = reflect(V, i.normal);
                half specular = dot(R, L);
                
                half3 skydata = texCUBE (_CubeMapTex, R).rgb;

                //return fixed4(skydata * pow(NdotV, _Metalic), 1);

                
                col.rgb += NdotV * _Fresnel * _FresnelColor;

                

                col.rgb += smoothstep(0,1, specular * _Roughness - (_Roughness - 1)) * 0.1;

                col.rgb = col.rgb + skydata * _CubemapColor * pow(NdotV, _Metalic);
                return col;//fixed4(skydata, 1);
                
            }
            ENDCG
        }
    }
}
