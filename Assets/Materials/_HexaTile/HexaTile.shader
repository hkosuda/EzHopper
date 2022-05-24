Shader "Custom/HexaTile"
{
    Properties
    {
        _MainColor ("MainColor", Color) = (1,1,1,1)
		_LineColor ("LineColor", Color) = (0,1,1,1)

		_TileSize ("TileSize", float) = 1.0
		_LineWidth ("LineWidth", float) = 0.3

		_PositionX ("PositionX", float) = 0.0
		_PosiitonZ ("PositionZ", float) = 0.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard 
		#pragma target 3.0

		half4 _MainColor;
		half4 _LineColor;

		half _TileSize;
		half _LineWidth;

		half _PositionX;
		half _PositionZ;

		struct Input
		{
			half3 worldPos;
		};

		// function
		half GetRotDeg(half deg){
			half n = floor((abs(deg) + 30) / 60);

			if (deg >= 0){
				return - n * 60;
			}

			else{
				return n * 60;
			}
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			half rad2deg = 57.29579143;
			half deg2rad = 0.017453292;

			half x = IN.worldPos.x - _PositionX;
			half z = IN.worldPos.z - _PositionZ;

			half deg = atan2(x, z) * rad2deg;
			half rotDeg = GetRotDeg(deg);
			
			half rotRad = rotDeg * deg2rad;

			half zz = z * cos(rotRad) - x * sin(rotRad);
			//half _x = z * sin(rotRad) + x * cos(rotRad);

			if (abs(_TileSize - zz) < _LineWidth){
				o.Albedo = _LineColor;
				o.Emission = _LineColor * 1.0;
			}

			else{
				o.Albedo = _MainColor;
				o.Emission = _MainColor * 1.0;
			}
		}

	ENDCG
    }
    FallBack "Diffuse"
}
