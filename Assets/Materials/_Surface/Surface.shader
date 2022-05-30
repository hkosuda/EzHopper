Shader "Custom/Surface"
{
    Properties
    {
        _SurfaceColor ("SurfaceColor", Color) = (1,0,0,1)
		_BodyColor("BodyColor", Color) = (0,0,0,1)

		_Y("Y", float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
		#pragma surface surf Standard 
		#pragma target 3.0

		half4 _SurfaceColor;
		half4 _BodyColor;

		half _Y;

		struct Input
		{
			half3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			half dy = IN.worldPos.y - _Y;

			if (dy > -0.01)
			{
				o.Albedo = _SurfaceColor;	
			}

			else
			{
				o.Albedo = _BodyColor;
			}
		}

	ENDCG
    }
    FallBack "Diffuse"
}
