Shader "Custom/Sq2Bar"
{
    Properties
    {
		_EdgeColor ("EdgeColor", Color) = (1,1,0,1)
        _LineColor ("LineColor", Color) = (1,1,1,1)
        _MainColor ("MainColor", Color) = (0,0,0,1)

		_EdgeWidth ("EdgeWidth", Float) = 0.1
		_LineWidth ("LineWidth", Float) = 0.02

		_LatticeSize ("LatticeSize", Float) = 1

		_PileSizeX ("PileSizeX", Float) = 1
		_PileSizeY ("PileSizeY", Float) = 1
		_PileSizeZ ("PileSizeZ", Float) = 1

		_X ("X", Float) = 0
		_Y ("Y", Float) = 0 
		_Z ("Z", Float) = 0

		_Direction("Direction", Float) = 0
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
		half _PileSizeY;
		half _PileSizeZ;

		half _LatticeSize;

		half _X;
		half _Y;
		half _Z;
		
		half _Direction;

		half delta(half pos, half size)
		{
			return pos - round(pos / size) * size;
		}

		struct Input
		{
			half3 worldPos;
			half3 worldNormal;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			half x = IN.worldPos.x - _X;
			half z = IN.worldPos.z - _Z;

			half borderX = _PileSizeX - _EdgeWidth;
			half borderZ = _PileSizeZ - _EdgeWidth;

			if (abs(IN.worldNormal.y) > 0.98)
			{
				if (abs(x) > borderX || abs(z) > borderZ)
				{
					o.Albedo = _EdgeColor;
				}

				else
				{
					o.Albedo = _MainColor;
				}

				return;
			}

			

			if (_Direction < 1)
			{
				if (z > borderZ && abs(x) > borderX)
				{
					o.Albedo = _EdgeColor;
					return;
				}
			}

			else if (_Direction < 2)
			{
				if (x > borderX && abs(z) > borderZ)
				{
					o.Albedo = _EdgeColor;
					return; 
				}
			}

			else if (_Direction < 3)
			{
				if (-z > borderZ && abs(x) > borderX)
				{
					o.Albedo = _EdgeColor;
					return;
				}
			}

			else if (_Direction < 4)
			{
				if (-x > borderX && abs(z) > borderZ)
				{
					o.Albedo = _EdgeColor;
					return; 
				}
			}

			else 
			{
				if (abs(z) > borderZ && abs(x) > borderX)
				{
					o.Albedo = _EdgeColor;
					return;
				}
			}

			half y = IN.worldPos.y - _Y;
			half borderY = _PileSizeY - _EdgeWidth;

			if (abs(y) > borderY)
			{
				o.Albedo = _EdgeColor;
				return;
			}


			half dy = delta(IN.worldPos.y, _LatticeSize);

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
