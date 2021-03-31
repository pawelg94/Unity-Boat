Shader "Unlit/Sail5Shader"
{
	Properties
	{
		[NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
		_AnimationSpeed("Animation Speed", Range(0, 3)) = 0
		_OffsetSize("Offset Size", Range(0, 10)) = 0
	}
		SubShader
	{
		Pass
		{
			Tags {"LightMode" = "ForwardBase"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

		// compile shader into multiple variants, with and without shadows
		// (we don't care about any lightmaps yet, so skip these variants)
		#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
		// shadow helper functions and macros
		#include "AutoLight.cginc"

		struct v2f
		{
			float2 uv : TEXCOORD0;
			SHADOW_COORDS(1) // put shadows data into TEXCOORD1
			fixed3 diff : COLOR0;
			fixed3 ambient : COLOR1;
			float4 pos : SV_POSITION;
		};

		float _AnimationSpeed;
		float _OffsetSize;

		v2f vert(appdata_base v)
		{
			v2f o;
			if (v.vertex.y < 14.5 && v.vertex.y > 5)
			{
				v.vertex.z += abs(sin((_Time.y-1)  * _AnimationSpeed + v.vertex.y * _OffsetSize));
				//v.vertex.x += sin((_Time.y / 2) * _AnimationSpeed + v.vertex.y * _OffsetSize) / 6;
			}
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord;

			half3 worldNormal = UnityObjectToWorldNormal(v.normal);
			half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
			o.diff = nl * _LightColor0.rgb;
			o.ambient = ShadeSH9(half4(worldNormal,1));
			// compute shadows data
			TRANSFER_SHADOW(o)				
			return o;
		}

		sampler2D _MainTex;

		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 col = tex2D(_MainTex, i.uv);
		// compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
		fixed shadow = SHADOW_ATTENUATION(i);
		// darken light's illumination with shadow, keep ambient intact
		fixed3 lighting = i.diff * shadow + i.ambient;
		col.rgb *= lighting;
		return col;
	}
	ENDCG
}

// shadow casting support
UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}
