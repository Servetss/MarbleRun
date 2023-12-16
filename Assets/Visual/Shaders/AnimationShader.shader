Shader "Unlit/AnimationShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _ColorPattern1 ("Color Pattern 1", Color) = (1,1,1,1)
        _RangePattern1 ("Range Pattern 1", Range(0, 1)) = 0.5
        
        _ColorPattern2 ("Color Pattern 2", Color) = (1,1,1,1)
        _RangePattern2 ("Range Pattern 2", Range(0, 1)) = 0.5
        
        _ColorPattern3 ("Color Pattern 3", Color) = (1,1,1,1)

        [Space(30)]
        _DiffuseColor ("Diffuse Color", Color) = (1,1,1,1)
        _ShadowStrenght ("Shadow Strenght", float ) = 1
        _Roughness ("Roughness", Range(0,1)) = 0.5
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
            
                float3 view : TEXCOORD1;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float _RangePattern1;
            float _RangePattern2;
            
            float4 _ColorPattern1;
            float4 _ColorPattern2;
            float4 _ColorPattern3;

            float4 _DiffuseColor;
            float _ShadowStrenght;
            float _Roughness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                o.normal = UnityObjectToWorldNormal(v.normal);
                
                float4 ws_pos = mul(unity_ObjectToWorld, v.vertex);
                o.view = normalize(UnityWorldSpaceViewDir(ws_pos));
                
                return o;
            }

            inline fixed4 DiffuseColor(fixed4 col, fixed3 normal, fixed3 H, float range, float shadowStrenght)
            {
                fixed4 specular = pow(saturate(dot(H, normal)), range);

                return step(shadowStrenght, specular + col);
            }


            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col1 = tex2D(_MainTex, i.uv + sin(_Time.x * 3.14 * 0.5) * 0.01);
                
                fixed4 col2 = tex2D(_MainTex, i.uv + sin(_Time.x * 3.14* 0.5) * 0.01);
                
                fixed3 H = normalize(_WorldSpaceLightPos0.xyz + i.view);

                fixed4 NdotL = max(0, dot(i.normal, _WorldSpaceLightPos0));
                
                float NdotV = 1 - saturate(dot(i.normal, i.view));
                
                fixed4 diffuseColor = DiffuseColor(_DiffuseColor, i.normal, H, _Roughness, _ShadowStrenght);

                _RangePattern1 = 0.65 + sin(_Time.x * 3.14) * 0.1;

                _RangePattern2 = 0.85 + sin(_Time.x * 3.14) * 0.07;
                
                float4 pattern1 = step(col1.r, _RangePattern1) ;

                float4 pattern2 = (1 - step(col2.r, _RangePattern2)) ;

                float4 pattern3 =  (1 - (pattern2 + pattern1)) * _ColorPattern3;

                pattern2 *= _ColorPattern2;

                pattern1 *= _ColorPattern1;

                return pattern3 + pattern2 + pattern1;
            }
            ENDCG
        }
    }
}
