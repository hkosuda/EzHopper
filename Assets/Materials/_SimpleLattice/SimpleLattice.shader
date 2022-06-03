Shader "Custom/SimpleLattice"
{
    Properties
    {
        _LineColor ("LineColor", Color) = (1,1,1,1)
        _MainColor ("MainColor", Color) = (0,0,0,1)

		_IntervalZ("IntervalZ", float) = 10
		_IntervalX("IntervalX", float) = 5

		_Width("Width", float) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
		#pragma surface surf Standard 
		#pragma target 3.0

		half4 _LineColor;
		half4 _MainColor;

		half _IntervalZ;
		half _IntervalX;

		half _Width;

		half delta(half pos, half size)
		{
			return pos - round(pos / size) * size;
		}

		struct Input
		{
			half3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			half dx = delta(IN.worldPos.x, _IntervalX);
			half dz = delta(IN.worldPos.z, _IntervalZ);

			if (abs(dx) < _Width || abs(dz) < _Width)
			{
				o.Albedo = _LineColor;
				return;
			}

			o.Albedo = _MainColor;
		}

	ENDCG
    }
    FallBack "Diffuse"
}
