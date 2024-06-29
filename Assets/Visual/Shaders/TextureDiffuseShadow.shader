Shader "Unlit/TextureDiffuseShadow"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _TextureXOffset("Texture X Offset", float) = 0.0
        _TextureXWidth("Texture X Width", float) = 0.0
        
        _TextureYOffset("Texture Y Offset", float) = 0.0
        _TextureYScale("Texture Y Scale", float) = 0.0


        [Space]
        _DiffuseTex ("Diffuse Texture", 2D) = "white" {}

        _MainColor ("Main Color", Color) = (1,1,1,1)
        _SecondColor ("Second Color", Color) = (1,1,1,1)
        _ShadowColor ("Shadow Color", Color) = (0,0,0,1)

        _Range ("Range", Range(0,2)) = 0.5
        _ShadowStrenght ("Shadow Strenght", Range(0,01)) = 0.05
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
                float3 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            
                float3 normal : NORMAL;
                float3 tangent : TANGENT;

                float3 ws_pos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            sampler2D _DiffuseTex;
            float4 _DiffuseTex_ST;

            float _TextureXOffset;
            float _TextureXWidth;
            
            float _TextureYOffset;
            float _TextureYScale;
            
            float4 _MainColor;
            float4 _SecondColor;
            float4 _ShadowColor;

            float _Range;

            float _ShadowStrenght;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _DiffuseTex);

                o.normal = UnityObjectToWorldNormal(v.normal);
                o.tangent = UnityObjectToWorldNormal(v.tangent);

                
                o.ws_pos = mul(unity_ObjectToWorld, v.vertex).xyz;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                fixed4 NdotL = dot(_WorldSpaceLightPos0.xyz, i.normal);

                fixed3 NcrossL = cross(_WorldSpaceLightPos0.xyz, i.normal);

                fixed3 crossCheck = cross(NdotL.xyz, i.normal);
                
                float cut = (dot(NcrossL.x * _TextureXOffset + _TextureXWidth, step(NcrossL.b,0)));
                
                //return float4(cut,  NdotL.y * _TextureYScale + _TextureYOffset,0,1);

                // sample the texture
                fixed4 mainTexture = tex2D(_MainTex, fixed2(cut, NdotL.y * _TextureYScale + _TextureYOffset));
                
                fixed4 diffuseTexCol = tex2D(_DiffuseTex, i.uv);

                fixed4 shadowColor = 1 - step(_Range, NdotL + diffuseTexCol);
                
                fixed4 mainColor = step(_Range + _ShadowStrenght, NdotL + diffuseTexCol);
                
                fixed4 middleColor = (1 - (mainColor + shadowColor)) * _SecondColor;

                shadowColor *= _ShadowColor;

                mainColor *= ((1 - step(mainTexture, 0)) * mainTexture) + (step(mainTexture, 0) * _MainColor);
                
                return middleColor + mainColor + shadowColor;
            }
            ENDCG
        }
    }
}
