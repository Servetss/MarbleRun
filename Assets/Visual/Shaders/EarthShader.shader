Shader "Unlit/EarthShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        _GroundColor ("Ground Color", Color) = (1,1,1,1)
        _WaterColor ("Water Color", Color) = (1,1,1,1)
        
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

                float4 vertex : SV_POSITION;

                float3 normal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _GroundColor;
            float4 _WaterColor;
           
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.normal = UnityObjectToWorldNormal(v.normal);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                fixed4 NdotL = saturate(dot(i.normal, _WorldSpaceLightPos0.xyz)) * 0.9 + 0.7;

                fixed4 groundCol = step(1 - col, 0.99);
                fixed4 waterCol = 1 - groundCol;

                groundCol *= _GroundColor;
                waterCol  *= _WaterColor;

                return max(waterCol, groundCol) * NdotL;
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
