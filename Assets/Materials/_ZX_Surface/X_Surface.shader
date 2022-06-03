Shader "Custom/X_Surface"
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
			if(abs(IN.worldNormal.z) > 0.99 || abs(IN.worldNormal.y) > 0.99)
			{
				o.Albedo = _BodyColor;
				return;
			}

			o.Albedo = _SurfaceColor;
		}

	ENDCG
    }
    FallBack "Diffuse"
}
