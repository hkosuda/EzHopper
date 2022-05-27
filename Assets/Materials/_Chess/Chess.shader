Shader "Custom/Chess"
{
    Properties
    {
        _Color1 ("Color1", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (0,0,0,1)
        _Size ("Size", Float) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
		#pragma surface surf Standard 
		#pragma target 3.0

		half4 _Color1;
		half4 _Color2;
		half _Size;

		struct Input
		{
			half3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			half row = round(IN.worldPos.z / _Size);
			half col = round(IN.worldPos.x / _Size);

			if ((row + col) % 2 == 0){
				o.Albedo = _Color1;
				//o.Emission = _Color1 * 1;
			}

			else{
				o.Albedo = _Color2;
				//o.Emission = _Color2 * 1;
			}
		}

	ENDCG
    }
    FallBack "Diffuse"
}