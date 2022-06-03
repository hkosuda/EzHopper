Shader "Custom/Surface"
{
    Properties
    {
        _SurfaceColor ("SurfaceColor", Color) = (1,0,0,1)
		_BodyColor("BodyColor", Color) = (1,1,1,1)
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

		struct Input
		{
			half3 worldNormal;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			if(IN.worldNormal.y > 0.99)
			{
				o.Albedo = _SurfaceColor;
				return;
			}

			o.Albedo = _BodyColor;
		}

	ENDCG
    }
    FallBack "Diffuse"
}
