Shader "Custom/Checkpoint"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)

        _Height("Height", float) = 1
        _Lines("Number of Lines", Float) = 10

        _Y ("Y", Float) = 0
        _InScene ("InScene", Float) = 1
    }
        SubShader
        {
            Tags { "RenderType"="Transparent" "Queue"="Transparent" }
            LOD 200
            Cull off

            CGPROGRAM
            #pragma surface surf Standard alpha:fade
            #pragma target 3.0

            
            half _Height;
            half _Lines;

            half _Y;
            half _InScene;

            half4 _Color;

            half clip(half value, half min, half max)
            {
                if (value > max) { return max; }
                if (value < min) { return min; }
                return value;
            }

            half offset(half value, half threshold, half exp)
            {
                if (value < threshold) { return 0; }

                half v = (value - threshold) / (1.0 - threshold);

                return pow(v, exp);
            }

            struct Input
            {
                half3 worldPos;
                half3 worldNormal;
            };

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                if (_InScene > 0)
                {
                    o.Albedo = _Color;
                    o.Alpha = 0.2;
                    return;
                }

                if (abs(IN.worldNormal.y) > 0.98)
                {
                    o.Alpha = 0.0;
                    return;
                }

                half y = IN.worldPos.y - _Y;

                if (y < 0 || y > _Height)
                {
                    o.Alpha = 0.0; 
                    return;
                }

                y = y / _Height;

                half value = 0.5 + 0.5 * sin(-2 * 3.1415 * pow(y, 2) * _Lines + _Time * 100);
                value = clip(value, 0.0, 1.0);
                value = offset(value, 0.95, 0.5);

                o.Albedo = _Color;
                o.Alpha = value * 0.5;
            }
        ENDCG
        }
    FallBack "Diffuse"
}
