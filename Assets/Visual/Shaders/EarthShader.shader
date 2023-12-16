Shader "Unlit/EarthShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaterNormalMap ("Water Normal Map", 2D) = "white" {}
        _GroundNormalMap ("Ground Normal Map", 2D) = "white" {}
        
        _GroundColor ("Ground Color", Color) = (1,1,1,1)
        _WaterColor ("Water Color", Color) = (1,1,1,1)
        
        [Space]
        _Range ("Range", float) = 0.5
        _ShadowStrenght ("Shadow Strenght", float) = 0.5
        
        [Space]
        _RangeGround ("Range Ground", float) = 0.5
        _ShadowStrenghtGround ("Shadow Strenght Ground", float) = 0.5

        [Space(20)]
        _CloudsTex("Clous Tex", 2D) = "white" {}
        _CloudColor ("Cloud Color", Color) = (1,1,1,1)

        _CloudDistance ("Cloud Distance", Range(0,1)) = 0.5
        _CloudStrenght ("Cloud Strenght", Range(0,1)) = 0.5
        _CloudSpeed ("Cloud Speed", float) = 1
        
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 normalUV : TEXCOORD02;
                float2 groundNormalUV : TEXCOORD03;

                float4 vertex : SV_POSITION;

                float3 normal : NORMAL;
                
                float3 ws_pos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _WaterNormalMap;
            float4 _WaterNormalMap_ST;

            sampler2D _GroundNormalMap;
            float4 _GroundNormalMap_ST;
            
            float4 _GroundColor;
            float4 _WaterColor;
            
            float _Range;
            float _ShadowStrenght;

            float _RangeGround;
            float _ShadowStrenghtGround;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normalUV = TRANSFORM_TEX(v.uv, _WaterNormalMap);
                o.groundNormalUV =TRANSFORM_TEX(v.uv, _GroundNormalMap);
                
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.ws_pos = mul(unity_ObjectToWorld, v.vertex).xyz;
                
                return o;
            }

            inline fixed4 DiffuseColor(fixed4 col, fixed3 normal, fixed3 H, float range, float shadowStrenght)
            {
                fixed4 specular = pow(saturate(dot(H, normal)), range);

                return smoothstep(0, shadowStrenght, specular);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 V = normalize(UnityWorldSpaceViewDir(i.ws_pos));

                fixed3 H = normalize(_WorldSpaceLightPos0.xyz + V.xyz);
                
                fixed3 waterNormal = i.normal * tex2D(_WaterNormalMap, i.normalUV);
                
                fixed3 groundNormal = i.normal * tex2D(_GroundNormalMap, i.groundNormalUV);
                
                waterNormal = normalize(waterNormal);

                groundNormal = normalize(groundNormal);

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                //return col;

                fixed4 groundCol = step(1 - col, 0.99);
                fixed4 waterCol = groundCol;

                fixed4 firstWater = 1 - waterCol;

                fixed NdotL = dot(waterNormal, _WorldSpaceLightPos0);

                fixed NdotLGround = dot(groundNormal, _WorldSpaceLightPos0);
                
                waterCol = saturate(DiffuseColor(waterCol, waterNormal, H, _Range, _ShadowStrenght) * NdotL * 0.4 + _WaterColor);
               
                waterCol -= groundCol;
                
                groundCol = saturate(DiffuseColor(groundCol, groundNormal, H, _RangeGround, _ShadowStrenghtGround) * NdotLGround + _GroundColor);

                groundCol -= firstWater;
               
                
                //groundCol *= _GroundColor;

                //return groundCol;
                
                return max(waterCol, groundCol);
            }
            ENDCG
        }

         Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha 

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

                float4 objectSpacePos : TEXCOORD1;
            };

            sampler2D _CloudsTex;
            float4 _CloudsTex_ST;

            float4 _CloudColor;

            float _CloudDistance;
            float _CloudStrenght;

            float _CloudSpeed;

            v2f vert (appdata v)
            {
                v2f o;

                v.vertex.xyz += v.normal * _CloudDistance;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _CloudsTex);

                o.objectSpacePos = v.vertex;

                o.normal = UnityObjectToWorldNormal(v.normal);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                   
                fixed4 V = fixed4(normalize(ObjSpaceViewDir(i.objectSpacePos)), 1) * 0.6;

                float r = i.uv.r;

                float g = i.uv.g * 1;

                float2 animatedUV1 = float2(r - _Time.x * _CloudSpeed, g + _Time.x * _CloudSpeed);
                float2 animatedUV2 = float2(r + _Time.x * _CloudSpeed, g - _Time.x * _CloudSpeed);

                // sample the texture
                fixed4 col1 = tex2D(_CloudsTex, animatedUV1);
                
                fixed4 col2 = tex2D(_CloudsTex, animatedUV2);

                fixed4 col = col2 * col1;
 
                col.a = smoothstep(0.2, _CloudStrenght, col).r;

                col.rgb = (1,1,1);
                
                return col;
            }
            ENDCG
        }
    }
}
