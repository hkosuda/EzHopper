Shader "Custom/Sq2Pile"
{
    Properties
    {
		_EdgeColor ("EdgeColor", Color) = (0,1,1,1)
        _LineColor ("LineColor", Color) = (1,1,1,1)
        _MainColor ("MainColor", Color) = (0,0,0,1)

		_EdgeWidth ("EdgeWidth", Float) = 0.1
		_LineWidth ("LineWidth", Float) = 0.02

		_LatticeSize ("LatticeSize", Float) = 1

		_PileSizeX ("PileSizeX", Float) = 1
		_PileSizeZ ("PileSizeZ", Float) = 1

		_X ("X", Float) = 0
		_Z ("Z", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
		#pragma surface surf Standard 
		#pragma target 3.0

		half4 _EdgeColor;
		half4 _LineColor;
		half4 _MainColor;

		half _EdgeWidth;
		half _LineWidth;

		half _PileSizeX; 
		half _PileSizeZ;
		half _LatticeSize;

		half _RotY; 

		half _X;
		half _Z; 

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
			half x = IN.worldPos.x - _X;
			half y = IN.worldPos.y;
			half z = IN.worldPos.z - _Z;

			half borderX = _PileSizeX - _EdgeWidth;
			half borderZ = _PileSizeZ - _EdgeWidth;

			if (abs(x) > borderX && abs(z) > borderZ)
			{
				o.Albedo = _EdgeColor;
				return;
			}

			half dy = delta(y, _LatticeSize);

			if (abs(dy) < _LineWidth)
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
