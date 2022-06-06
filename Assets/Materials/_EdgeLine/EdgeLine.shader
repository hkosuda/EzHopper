Shader "Custom/EdgeLine"
{
    Properties
    {
		_EdgeColor ("EdgeColor", Color) = (0,1,1,1)
        _LineColor ("LineColor", Color) = (1,1,1,1)
        _MainColor ("MainColor", Color) = (0,0,0,1)

		_EdgeWidth ("EdgeWidth", Float) = 0.1
		_LineWidth ("LineWidth", Float) = 0.02

		_LatticeSize ("LatticeSize", Float) = 1

		_TileSizeX ("TileSizeX", Float) = 1
		_TileSizeZ ("TileSizeZ", Float) = 1

		_RotY ("RotY", Float) = 0

		_X ("X", Float) = 0
		_Y ("Y", Float) = 0 
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

		half _TileSizeX; 
		half _TileSizeZ;
		half _LatticeSize;

		half _RotY; 

		half _X;
		half _Y;
		half _Z; 

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
			half xx = IN.worldPos.x - _X;
			half yy = IN.worldPos.y - _Y;
			half zz = IN.worldPos.z - _Z;

			half x = zz * sin(_RotY) - xx * cos(_RotY);
			half y = yy;
			half z = zz * cos(_RotY) + xx * sin(_RotY);

			if (abs(IN.worldNormal.y) > 0.98)
			{
				half xlim = _TileSizeX - _EdgeWidth;
				half zlim = _TileSizeZ - _EdgeWidth;

				if (abs(x) > xlim || abs(z) > zlim)
				{
					o.Albedo = _EdgeColor;
				}

				else
				{
					o.Albedo = _MainColor;
				}

				return;
			}

			half nx = IN.worldNormal.z * sin(_RotY) - IN.worldNormal.x * cos(_RotY);
			half nz = IN.worldNormal.z * cos(_RotY) + IN.worldNormal.x * sin(_RotY);

			half dx = delta(x, _LatticeSize);
			half dy = delta(y, _LatticeSize);
			half dz = delta(z, _LatticeSize);

			if (abs(nx) > 0.98)
			{
				if (abs(dz) < _LineWidth || abs(dy) < _LineWidth)
				{
					o.Albedo = _LineColor;
					return;
				}
			}

			else if(abs(nz) > 0.98)
			{
				if (abs(dx) < _LineWidth || abs(dy) < _LineWidth)
				{
					o.Albedo = _LineColor;
					return;
				}
			}

			o.Albedo = _MainColor;
		}

	ENDCG
    }
    FallBack "Diffuse"
}
