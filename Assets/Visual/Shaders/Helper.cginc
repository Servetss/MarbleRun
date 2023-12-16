            inline fixed4 DiffuseColor(fixed4 col, fixed3 normal, fixed3 H, float range, float shadowStrenght)
            {
                fixed4 specular = pow(saturate(dot(H, normal)), range);

                return step(shadowStrenght, specular + col);
            }
