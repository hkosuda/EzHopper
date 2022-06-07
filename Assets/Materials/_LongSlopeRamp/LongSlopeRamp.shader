Shader "Custom/LongSlopeRamp"
{
    Properties
    {
		_CenterColor ("CenterColor", Color) = (0,1,0,1)
        _AxisColor ("AxisColor", Color) = (1,0,0,1)
		_TickColor ("TickColor", Color) = (0,0,0,1)
        _MainColor ("MainColor", Color) = (0,0,0,1)


		_CenterLineWidth("CenterLineWidth", Float) = 0.1
		_AxisLineWidth("AxisLineWidth", Float) = 0.1
		_TickLineWidth("TickLineWidth", Float) = 0.1

        _AxisInterval ("AxisInterval", Float) = 100
		_TickInterval ("TickInterval", Float) = 10 
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
		#pragma surface surf Standard 
		#pragma target 3.0

		half4 _CenterColor;
		half4 _AxisColor;
		half4 _TickColor;
		half4 _MainColor;

		half _CenterLineWidth;
		half _AxisLineWidth;
		half _TickLineWidth;

		half _AxisInterval;
		half _TickInterval;

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
			if (abs(IN.worldPos.x) < _CenterLineWidth)
			{
				o.Albedo = _CenterColor;
				return;
			}

			half dz = delta(IN.worldPos.z, _AxisInterval);

			if (abs(dz) < _AxisLineWidth)
			{
				o.Albedo = _AxisColor;
				return;
			}

			if (abs(dz) < _TickLineWidth)
			{
				o.Albedo = _MainColor;
				return;
			}

			half dz2 = delta(IN.worldPos.z, _TickInterval);

			if (abs(dz2) < _TickLineWidth)
			{
				o.Albedo = _TickColor;
				return;
			}

			o.Albedo = _MainColor;
		}

	ENDCG
    }
    FallBack "Diffuse"
}
