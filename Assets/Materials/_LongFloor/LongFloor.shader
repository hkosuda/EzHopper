Shader "Custom/LongFloor"
{
    Properties
    {
        _LineColor ("LineColor", Color) = (1,1,1,1)
        _MainColor ("MainColor", Color) = (0,0,0,1)

		_Width("Width", Float) = 0.1
        _Size ("Size", Float) = 10
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

		half _Width;
		half _Size;

		half delta(half pos)
		{
			return pos - round(pos / _Size) * _Size;
		}

		struct Input
		{
			half3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			half dx = delta(IN.worldPos.x);
			half dy = delta(IN.worldPos.y);
			half dz = delta(IN.worldPos.z);

			if (abs(dx) < _Width && abs(dz) < _Width)
			{
				o.Albedo = _LineColor;
			}

			else if (abs(dx) < _Width && abs(dy) < _Width)
			{
				o.Albedo = _LineColor;
			}

			else if (abs(dy) < _Width && abs(dz) < _Width)
			{
				o.Albedo = _LineColor;
			}



			else
			{
				o.Albedo = _MainColor;
			}
		}

	ENDCG
    }
    FallBack "Diffuse"
}
