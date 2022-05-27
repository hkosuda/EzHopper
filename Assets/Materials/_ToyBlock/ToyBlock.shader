Shader "Custom/ToyBlock"
{
    Properties
    {
        _MainColor ("MainColor", Color) = (1,1,1,1)
		_LineColor ("LineColor", Color) = (1,1,1,1)

		_X("X", float) = 0
		_Y("Y", float) = 0
		_Z("Z", float) = 0
		_RotY("RotY", float) = 0

		_TileSize("TileSize", float) = 5.0
		_OuterLineWidth("OuterLineWidth", float) = 0.1

		_Width("Width", Float) = 0.1
        _Size("Size", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
		#pragma surface surf Standard 
		#pragma target 3.0

		half4 _MainColor;
		half4 _LineColor;

		half _X;
		half _Y;
		half _Z;
		half _RotY;

		half _TileSize;

		half _Width;
		half _Size;

		half delta(half pos)
		{
			return pos - round(pos / _Size) * _Size;
		}

		half getAlpha(half d)
		{
			half add = 0.3;
			if (d > 0){ add = 0.6; }

			half val = abs(d) / _Width + add;

			if (val > 1.0)
			{
				return 1.0;
			}

			return val;
		}

		half minValue(half vx, half vy, half vz)
		{
			half v = 1.0;

			if (vx < v){ v = vx; }
			if (vy < v){ v = vy; }
			if (vz < v){ v = vz; }

			return v;
		}

		half minValue2(half vx, half vz)
		{
			if (vx < vz){ return vx; }
			return vz;
		}

		half checkOutline(half halfSize, half pos)
		{
			if (pos > 0)
			{
				half d = pos - halfSize;
				if (abs(d) < _Width) { return 1.0; }
			}

			else
			{
				half d = halfSize + pos;
				if (abs(d) < _Width) { return 1.0; }
			}

			return -1.0;
		}

		struct Input
		{
			half3 worldPos;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			half halfSize = 0.5 * _Size;
			half halfTileSize = 0.5 * _TileSize;

			half _x = IN.worldPos.x;
			half _z = IN.worldPos.z;

			half x = _x * cos(_RotY) - _z * sin(_RotY) - _X;
			half y = IN.worldPos.y;
			half z = _x * sin(_RotY) + _z * cos(_RotY) - _Z;

			if (checkOutline(halfTileSize, x) > 0.0 || checkOutline(halfTileSize, z) > 0.0)
			{
				o.Albedo = _LineColor;
				return;
			}

			half xx = x + halfSize;
			half zz = z + halfSize;

			half dx = delta(xx);
			half dz = delta(zz);

			half vx = getAlpha(dx);
			half vz = getAlpha(dz);

			o.Albedo = _MainColor * minValue2(vx, vz);
		}

	ENDCG
    }
    FallBack "Diffuse"
}

