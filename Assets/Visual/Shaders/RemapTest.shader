Shader "Unlit/RemapTest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _InMinMax1 ("In Min Max 1", Range(0,1)) = 0
        _InMinMax2 ("In Min Max 2", Range(0,1)) = 0
    
        _OutMinMax1 ("Out Min Max 1", Range(0,1)) = 0
        _OutMinMax2 ("Out Min Max 2", Range(0,1)) = 0
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
            // make fog work
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _InMinMax1;
            float _InMinMax2;
    
            float _OutMinMax1;
            float _OutMinMax2;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            inline fixed4 Unity_Remap_float4(fixed4 In, float2 InMinMax, float2 OutMinMax, fixed4 Out)
            {
                return OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

                float2 inMinMax = float2(_InMinMax1,_InMinMax2);
                float2 outMinMax = float2(_OutMinMax1,_OutMinMax2);


                col = Unity_Remap_float4(col, inMinMax, outMinMax);

                
                return Out;
            }
            ENDCG
        }
    }
}
