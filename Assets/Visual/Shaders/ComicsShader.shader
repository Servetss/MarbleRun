Shader "Unlit/ComicsShader"
{
    Properties
    {
        _ToonRampTex ("Toon Ramp Texture", 2D) = "white" {}
        _ShadowTexture ("Shadow Texture", 2D) = "white" {}
        
        _Scale ("Scale", Range(0, 1)) = 1
        _Scale2 ("Scale 2", Range(0, 1)) = 1
        
        _ColorLight ("Light Color", Color) = (1,1,1,1)
        _ColorShadow ("Shadow Color", Color) = (1,1,1,1)

        [Space(30)]
        _OutlaneScale ("Outlane Scale", Range(0, 1)) = 0
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        

        [Space(30)]
        _Roughtness ("_Roughtness", Range(0,2)) = 0.5
        _ShadowStrenght ("Shadow Strenght", Range(0,01)) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
                
         Pass
        {
            Cull Front
            
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
            };

            sampler2D _ToonRampTex;
            float4 _ToonRampTex_ST;
            
            float _OutlaneScale;
            float4 _OutlineColor;

            v2f vert (appdata v)
            {
                v2f o;

                v.vertex += float4(v.normal * _OutlaneScale, 1);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _ToonRampTex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_ToonRampTex, i.uv);

                return _OutlineColor;
            }
            ENDCG
        }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
          
            #define L _WorldSpaceLightPos0
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uvShadow : TEXCOORD1;
                
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uvShadow : TEXCOORD2;
                
                float4 vertex : SV_POSITION;
            
                float3 normal : NORMAL;
                float3 ws_pos : TEXCOORD1;
            };

            sampler2D _ToonRampTex;
            float4 _ToonRampTex_ST;

            sampler2D _ShadowTexture;
            float4 _ShadowTexture_ST;

            float _Scale;
            float _Scale2;

            float4 _ColorLight;
            float4 _ColorShadow;

            float _Roughtness;
            float _ShadowStrenght;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _ToonRampTex);
                o.uvShadow = TRANSFORM_TEX(v.uvShadow, _ShadowTexture);
                
                o.normal = UnityObjectToWorldNormal(v.vertex);
                o.ws_pos = mul(unity_ObjectToWorld, v.vertex);
                
                return o;
            }

            
            inline fixed4 DiffuseColor(fixed4 col, fixed3 normal, fixed3 H, float range, float shadowStrenght)
            {
                fixed4 specular = pow(saturate(dot(H, normal)), range);

                return 1 - smoothstep(0, specular, shadowStrenght) + col;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed3 V = normalize(UnityWorldSpaceViewDir(i.ws_pos));
                
                fixed3 H = normalize(_WorldSpaceLightPos0.xyz + V.xyz);

                // sample the texture
                fixed4 col = tex2D(_ToonRampTex, i.uv);
                fixed4 colShadow = tex2D(_ShadowTexture, i.uvShadow);
                
                fixed3 NdotLLight = saturate((dot(L, i.normal) + _Scale2) + 0.5);
                fixed3 NdotLShadow = saturate((dot(L, i.normal) + _Scale2) + 0.1);
                fixed3 NdotLDotsShadow = saturate(dot(L, i.normal) + 0.5) + colShadow + NdotLLight;
                     

                // Calculate the dot product between the surface normal and the light direction
                float diff = fixed4((pow(1 - NdotLShadow, 2)), 1) + fixed4((1 - NdotLDotsShadow), 1);

                // Convert the dot product value to a range between 0 and 1
                float h = (diff * 0.5 + 0.5) * _Scale;

                // Create a float2 vector using the converted dot product value
                float2 rh = h;

                // Use the ramp texture to determine the shading value based on the dot product value
                float4 ramp = tex2D(_ToonRampTex, rh);

                // Combine the albedo, light color, and shading value to create the final color
                //fixed4 c = _ColorLight * ramp;

           
                fixed4 diffuseColor = DiffuseColor(colShadow, i.normal, H, _Roughtness, _ShadowStrenght);

                fixed4 finalCol = lerp(_ColorLight, _ColorShadow, saturate(diff));
                
                return finalCol;//saturate(diff);// + (diff));
            }
            ENDCG
        }
    }
}
