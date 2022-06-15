Shader "Custom/NostalgiaLongSlope"
{
    Properties
    {
        _LineColor ("LineColor", Color) = (1,1,1,1)
        _MainColor ("MainColor", Color) = (0,0,0,1)

		_Width("Width", Float) = 0.1
        _Size ("Size", Float) = 10

		_Y ("Y", Float) = 0
		_BottomLineWidth ("BottomLineWidth", Float) = 1
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

		half _Y;
		half _BottomLineWidth;

		half delta(half pos)
		{
			return pos - round(pos / _Size) * _Size;
		}

		struct Input
		{
			half3 worldPos;
			half3 worldNormal;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			if (IN.worldNormal.y < -0.98 || abs(IN.worldNormal.y) < 0.02)
			{
				o.Albedo = _MainColor;
				return;
			}

			half dy = IN.worldPos.y - _Y;

			if (abs(dy) < _BottomLineWidth)
			{
				o.Albedo = _LineColor;
				return;
			}

			half dx = delta(IN.worldPos.x);
			half dz = delta(IN.worldPos.z);

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
