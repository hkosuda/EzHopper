Shader "Custom/SurfChess"
{
    Properties
    {
        _Color1 ("Color1", Color) = (1,1,1,1)
        _Color2 ("Color2", Color) = (0,0,0,1)

		_LineColor("LineColor", Color) = (1,1,1,1)
		_MainColor("MainColor", Color) = (0,0,0,1)

        _ChessSize ("ChessSize", float) = 5
		_LatticeSize("LatticeSize", float) = 3

		_LineWidth("LineWidth", float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
		#pragma surface surf Standard 
		#pragma target 3.0

		half4 _Color1;
		half4 _Color2;

		half4 _LineColor;
		half4 _MainColor;

		half _ChessSize;
		half _LatticeSize;

		half _LineWidth;

		half delta(half pos)
		{
			return pos - round(pos / _LatticeSize) * _LatticeSize;
		}

		struct Input
		{
			half3 worldPos;
			half3 worldNormal;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			if (IN.worldNormal.y > 0.99)
			{
				half row = round(IN.worldPos.z / _ChessSize);
				half col = round(IN.worldPos.x / _ChessSize);

				if ((row + col) % 2 == 0){
					o.Albedo = _Color1;
				}

				else{
					o.Albedo = _Color2;
				}

				return;
			}

			half dx = delta(IN.worldPos.x);
			half dy = delta(IN.worldPos.y);
			half dz = delta(IN.worldPos.z);

			if (abs(IN.worldNormal.z) > 0.99)
			{
				if (abs(dx) < _LineWidth || abs(dy) < _LineWidth)
				{
					o.Albedo = _LineColor;
					return;
				}
			}

			if (abs(IN.worldNormal.x) > 0.99)
			{
				if (abs(dy) < _LineWidth || abs(dz) < _LineWidth)
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