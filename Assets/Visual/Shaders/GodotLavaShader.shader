Shader "Unlit/GodotLavaShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
                

            
//float localTime() {
//	return _Time * timeScaleFactor;
//}

            
//            float flow(float2 p) 
//            {
//	            float z = 2.;
//	            float rz = 0.;
//	            float2 bp = p;
	
//	            for(float i = 1.; i < 7.; ++i) 
//                {
//		            p += localTime() * .6;
//		            bp += localTime() * 1.9;
//		            float2 gr = gradn(i * p * .34 + localTime() * 1.);
//		            gr *= makem2(localTime() * 6. - (.05 * p.x + 0.03 * p.y) * 40.);
//		            p += gr * .5;
//		            rz += (sin(noise(p) * 7.) *.5 + .5) / z;
//		            p = mix(bp, p, .77);
//		            z *= 1.4;
//		            p *= 2.;
//		            bp *= 1.9;
//	            }
//	            return rz;
//            }
    
            

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv); // Sample the input texture
                half lum = dot(col.rgb, fixed3(0.3, 0.59, 0.11)); // Color to luminosity
                col = tex2D(_LookUpTex, float2(lum, 0)); // Get value from color ramp
                return col;
                
	            //float2 resolution = 1. / SCREEN_PIXEL_SIZE;
	            //float2 fragCoord = FRAGCOORD.xy;
	            //fragCoord = vec2(fragCoord.x, resolution.y - fragCoord.y);
	            //float2 p = fragCoord / resolution - 0.5;
	
	            //p.x *= resolution.x / resolution.y;
	            //p *= 3.;
	
	            //float rz = flow(p);
	            //float3 col = vec3(.2, .07, .01) / rz;
	            //col = pow(col, vec3(1.4));
	
	            //return fixed4(col, 1);
            }

            
//uniform float timeScaleFactor = .04;

//// Make sure to use a seamless noise texture
//uniform sampler2D noise_texture: repeat_enable, filter_linear;

//mat2 makem2(float theta) {
//	float c = cos(theta);
//	float s = sin(theta);
//	return mat2(vec2(c, -s), vec2(s, c));
//}

//float noise(vec2 x) {
//	return texture(noise_texture, x * .01).x;
//}

//vec2 gradn(vec2 p) {
//	float ep = .09;
//	float gradx = noise(vec2(p.x + ep, p.y)) - noise(vec2(p.x - ep, p.y));
//	float grady = noise(vec2(p.x, p.y + ep)) - noise(vec2(p.x, p.y - ep));
//	return vec2(gradx, grady);
//}
        ENDCG
        }
    }
}
