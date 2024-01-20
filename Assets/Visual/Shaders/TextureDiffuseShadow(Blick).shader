Shader "Unlit/TextureDiffuseShadow(Blick)"
{
    Properties
    {
        _MainColor("Main Color", Color) = (1,1,1,1)
        
        _DiffuseColor("Diffuse Color", Color) = (1,1,1,1)
        _DiffuseTex ("Diffuse Texture", 2D) = "white" {}

        _RimLightColor("Rim Light Color", Color) = (1,1,1,1)
        
        _FresnelRange ("Fresnel Range", Range(0,25)) = 1
        
        _LightRange ("Light Range", Range(0,25)) = 1

        _Range ("Range", Range(0,25)) = 0.5
        _ShadowStrenght ("Shadow Strenght", Range(0,5)) = 0.05
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
            //#include "Helper.cginc"

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
                float3 viewDirection : TEXCOORD2;
            };

            sampler2D _DiffuseTex;
            float4 _DiffuseTex_ST;

            fixed4 _MainColor;

            fixed4 _DiffuseColor;

            fixed4 _RimLightColor;

            fixed _FresnelRange;
            
            fixed _LightRange ;
            
            fixed _Range;

            fixed _ShadowStrenght;

            v2f vert (appdata v)
            {
                v2f o;
                o.normal = UnityObjectToWorldNormal(v.normal);

                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _DiffuseTex);
                
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

                fixed3 H = normalize(_WorldSpaceLightPos0.xyz + V.xyz);

                // sample the texture
                fixed4 NdotL = max(min(dot(_WorldSpaceLightPos0.xyz, i.normal) + _LightRange, 1), 0);

                fixed4 NdotL2 = max(min(dot(_WorldSpaceLightPos0.xyz, i.normal) + _LightRange, 1), 0.8);
                
                fixed4 diffuseColor = tex2D(_DiffuseTex, i.uv);
                
                diffuseColor = DiffuseColor(diffuseColor, i.normal, H, _Range, _ShadowStrenght);
                
                fixed4 rimLight = ((0.5 + dot(i.normal, -V)) * _FresnelRange) * NdotL;

                fixed4 col = (1 - max(rimLight, diffuseColor)) * _MainColor * NdotL2;
                
                return max(rimLight * _RimLightColor, diffuseColor * _DiffuseColor) + col;   //diffuseColor + _MainColor;//col1 + col2;
            }
            ENDCG
        }
    }
}