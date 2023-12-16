Shader "Unlit/LavaShader"
{
    Properties
    {
        _LavaTex ("Lava Texture", 2D) = "white" {}
        _NoiseMap ( "Noise Map", 2D) = "white" {}
        
        _Color( "Color", Color ) = (1,1,1,1)
        _GlassBlickColor ("Glass Blick Color", Color) = (1,1,1,1)
        
        _Scale( "Scale", Range(0, 40)) = 1
        _Extrude( "Extrude", Range(0, 1)) = 0.5
        
        // Remap Variables //
        [Space]
        _InMinMax1 ("In Min Max 1", Range(0,1)) = 0
        _InMinMax2 ("In Min Max 2", Range(0,1)) = 0
    
        _OutMinMax1 ("Out Min Max 1", Range(0,1)) = 0
        _OutMinMax2 ("Out Min Max 2", Range(0,1)) = 0
    
        [Space]
        _Speed ("Speed", float) = 0
        _DistortionAmount("Distortion Amount", float) = 0
        
        [Space(30)]
        _Roughtness ("Roughtness", Range(0, 5)) = 0.5
        _ShadowStrenght ("Shadow Strenght", Range(0,5)) = 0.5
        
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha 


        Tags { "RenderType"="Opaque"}
        LOD 100

        Pass
        {
            Cull Back
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

                float2 c : TEXCOORD1;

                float3 normal : NORMAL;
                float3 ws_pos : TEXCOORD2;
            };

            sampler2D _LavaTex;
            float4 _LavaTex_ST;

            sampler2D _NoiseMap;
            float4 _NoiseMap_ST;

            float4 _Color;
            float4 _GlassBlickColor;

            float _Scale;
            float _Extrude;

            float _InMinMax1;
            float _InMinMax2;
    
            float _OutMinMax1;
            float _OutMinMax2;
            
            float _Speed;
            float _DistortionAmount;
            
            float _Roughtness;
            float _ShadowStrenght;

            v2f vert (appdata v)
            {
                v2f o;
                
                o.uv = TRANSFORM_TEX(v.uv, _LavaTex);
                

                float offset = saturate(1 - (_Scale * pow((v.uv.y - 0.5), 2)));
                v.vertex -= fixed4(v.normal * offset * _Extrude, 0);
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                o.ws_pos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.normal = UnityObjectToWorldNormal(v.normal);
                
                return o;
            }

            inline fixed Unity_Remap_float4(fixed In, float2 InMinMax, float2 OutMinMax)
            {
                return OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
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
                
                float speed = _Time.x * _Speed;
                float noiseSpeed = _Time.x * _Speed;
                
                fixed noise = tex2D(_NoiseMap, float2(i.uv.x, i.uv.y + noiseSpeed) * 0.4).r * _DistortionAmount;
                fixed noise2 = tex2D(_NoiseMap, float2(i.uv.x, i.uv.y - noiseSpeed * 0.05) * 1).r * 0.15;
                
                fixed2 uvOffsetAnim = fixed2(i.uv.x, i.uv.y + noise);
                
                fixed4 diffuseColor = DiffuseColor(fixed4(1,1,1,1), i.normal, H, _Roughtness, _ShadowStrenght);

                float NdotV = 1 - saturate(dot(i.normal, V.xyz));
                
                
                // sample the texture
                fixed lavaMoveCol = tex2D(_LavaTex, float2(i.uv.x + sin(i.uv.y * 3.14 * 4) * 0.02, i.uv.y + speed)).r + noise2;
                fixed lavaMiddleCol = tex2D(_LavaTex, uvOffsetAnim).g;
                fixed lavaBorderCol = tex2D(_LavaTex, uvOffsetAnim).b;
                 
                fixed col = (lavaMoveCol * lavaMiddleCol) + lavaBorderCol;
                float2 inMinMax = float2(_InMinMax1,_InMinMax2);
                float2 outMinMax = float2(_OutMinMax1,_OutMinMax2);
                            
                col = Unity_Remap_float4(col, inMinMax, outMinMax);
                
                
                
                fixed alpha = step(0.99, col);
                fixed4 col4 = col;
                
                col4 *= _Color;
                col4.a = alpha;

                float offset = 1 - (_Scale * pow((i.uv.y - 0.5), 2));
                
                //return col4;
            
                return max(NdotV * _GlassBlickColor, col4);
            }
            ENDCG
        }
    }
}
