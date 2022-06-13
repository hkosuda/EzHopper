Shader "Custom/Ghost"
{
    Properties
    {
        _Color ("Color", Color) = (0,1,0,1)

		_Width ("Width", Float) = 0.1
        _Height ("Height", Float) = 0.9
		_Radius ("Radius", Float) = 0.5
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Opaque" }
        LOD 200
		Cull Off

        CGPROGRAM
		#pragma surface surf Standard alpha:fade
		#pragma target 3.0

		half4 _Color;

		half _Width;
		half _Height;
		half _Radius;

		half delta(half pos, half size)
		{
			return pos - round(pos / size) * size;
		}

		half disp(half x, half z)
		{
			return pow((pow(x, 2) + pow(z, 2)), 0.5);
		}

		struct Input
		{
			half3 worldPos;
			half3 worldNormal;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			half3 localPos = IN.worldPos - mul(unity_ObjectToWorld, half4(0,0,0,1)).xyz;

			half x = localPos.x;
			half y = localPos.y;
			half z = localPos.z;

			if (abs(IN.worldNormal.y) > 0.98)
			{
				half border = _Radius - _Width;
				half d = disp(x, z);

				if (d > border)
				{
					o.Albedo = _Color;
					o.Alpha = 1;
				}

				else
				{
					o.Alpha = 0.0;
				}
			}

			else
			{
				half angleWidth = 0.5 * _Width / _Radius;
				half angle = atan2(z, x);
				half angleDelta = delta(angle, 1.0472);

				if (abs(angleDelta) < angleWidth)
				{
					o.Albedo = _Color; 
					o.Alpha = 1; 
					return; 
				}

				half border = _Height - _Width;
				
				if (abs(y) > border || abs(y) < _Width)
				{
					o.Albedo = _Color;
					o.Alpha = 1;
				}

				else
				{
					o.Alpha = 0.0;
				}
			}
		}

	ENDCG
    }

    FallBack "Diffuse"
}
