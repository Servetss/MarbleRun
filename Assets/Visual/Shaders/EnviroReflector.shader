Shader "Unlit/EnviroReflector"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ReflectTex("Reflect Texture", 2D) = "white" {}
        
        _CameraViewVector("Camera View Vector", Vector) = (1,1,1,1)
        _ReflectColor ("Reflect Color", Color) = (1,1,1,1)
        _ReflectIntensity ("Reflect Intensity", Range(0,2)) = 1
       
        _LightRange ("Light Range", Range(0.0, 5)) = 0.5

        [Space(30)]
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        _Fresnel("Fresnel", Range(0,1)) = 0


        [Space]
        [NoScaleOffset] _CubeMapTex ("Cubemap Texture", Cube) = "" {}
        [HDR]_CubemapColor ("Cubemap Color", Color) = (1,1,1,1)
        

        [Space(30)]
        _Roughness ("Roughness", Range(0.0, 30)) = 0.5

        _Metalic ("Metalic", Range(0, 5)) = 0
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

            sampler2D _ReflectTex;
            float4 _ReflectTex_ST;
            
            samplerCUBE _CubeMapTex;
            half4 _CubeMapTex_HDR;

            float _LightRange;

            float4 _CameraViewVector;
            
            float4 _ReflectColor;
            float _ReflectIntensity;
            
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


                o.texcoord = mul(UNITY_MATRIX_IT_MV, v.vertex).xyz;//v.vertex;
                
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

            inline float2 Rotate(float2 v, float angleRad)
            {
                float ca = sin(angleRad);
                float sa = cos(angleRad);

                return float2(ca * v.x - sa * v.y, sa * v.x + ca * v.y);
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                //i.uv = Rotate(i.uv, _Time.y * 0.1);

                i.normal = normalize(i.normal);
                
                fixed3 V = normalize(UnityWorldSpaceViewDir(i.ws_pos));
                //fixed3 VConst = normalize(_CameraViewVector.xyz - i.texcoord);
                fixed3 VConst = normalize(_CameraViewVector.xyz);

                
                fixed3 H = normalize(L + V.xyz);
                
                fixed4 NdotL = dot(L, i.normal) * 0.5 + 0.5;

                float NdotV = 1- saturate(dot(i.normal, V.xyz));

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * NdotL;
                

                half3 R = reflect(-V , i.normal);
                half3 ROp = reflect(-V, i.normal);
                half specular = dot(R, L);
                
                float2 reflectUV = saturate(float2(ROp.b * 0.5 + 0.5, -ROp.y));
                
                float reflectRenderOffset = (-i.texcoord.y + 0.2);

                half3 skydata = texCUBE(_CubeMapTex, R).rgb;
                half4 reflectRoad = saturate(tex2D(_ReflectTex, reflectUV) * reflectRenderOffset * _ReflectIntensity);// + _ReflectColor; // 


                col.rgb += NdotV * _Fresnel * _FresnelColor;

                

                //col.rgb += smoothstep(0,1, specular * _Roughness - (_Roughness - 1)) * 0.1;

                col.rgb = col.rgb + reflectRoad + skydata * _CubemapColor * pow(NdotV, _Metalic);
                return reflectRoad;
                
            }
            ENDCG
        }
    }
}
