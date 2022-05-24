Shader "Custom/Tile"
{
    Properties
    {
        _LineColor ("LineColor", Color) = (1,1,1,1)
        _MainColor ("MainColor", Color) = (0,0,0,1)

		_X("X", float) = 0
		_Y("Y", float) = 0 
		_Z("Z", float) = 0

		_Width("Width", float) = 0.1
        _Size ("Size", float) = 1
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

		half _X;
		half _Y;
		half _Z;

		half _Width;
		half _Size;

		half delta(half pos, half center, half size)
		{
			half dp = pos - center;
			
			if (dp < 0)
			{
				return size + dp;
			}

			return size - dp;
		}

		struct Input
		{
			half3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			half dx = delta(IN.worldPos.x, _X, _Size);
			half dz = delta(IN.worldPos.z, _Z, _Size);

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
