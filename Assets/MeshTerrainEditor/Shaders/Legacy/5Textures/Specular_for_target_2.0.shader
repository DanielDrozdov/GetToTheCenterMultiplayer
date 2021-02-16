﻿Shader "MTE/Legacy/5 Textures/Specular_for_target_2.0"
{
	Properties
	{
		_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess("Shininess", Range(0.03, 1)) = 0.078125

		_Control ("Control (RGBA)", 2D) = "red" {}
		_ControlExtra ("Control Extra (R)", 2D) = "black" {}
		_Splat0 ("Layer 1", 2D) = "white" {}
		_Splat1 ("Layer 2", 2D) = "white" {}
		_Splat2 ("Layer 3", 2D) = "white" {}
		_Splat3 ("Layer 4", 2D) = "white" {}
		_Splat4 ("Layer 5", 2D) = "white" {}

		_Normal0 ("Normalmap 1", 2D) = "bump" {}
		_Normal1 ("Normalmap 2", 2D) = "bump" {}
		_Normal2 ("Normalmap 3", 2D) = "bump" {}
		_Normal3 ("Normalmap 4", 2D) = "bump" {}
		_Normal4 ("Normalmap 5", 2D) = "bump" {}

		[Toggle(ENABLE_NORMAL_INTENSITY)] ENABLE_NORMAL_INTENSITY ("Normal Intensity", Float) = 0
		_NormalIntensity0 ("Normal Intensity 0", Range(0.01, 10)) = 1.0
		_NormalIntensity1 ("Normal Intensity 1", Range(0.01, 10)) = 1.0
		_NormalIntensity2 ("Normal Intensity 2", Range(0.01, 10)) = 1.0
		_NormalIntensity3 ("Normal Intensity 3", Range(0.01, 10)) = 1.0
		_NormalIntensity4 ("Normal Intensity 4", Range(0.01, 10)) = 1.0
	}

	CGINCLUDE
		#pragma surface surf BlinnPhong vertex:MTE_SplatmapVert finalcolor:MTE_SplatmapFinalColor finalprepass:MTE_SplatmapFinalPrepass finalgbuffer:MTE_SplatmapFinalGBuffer
		#pragma multi_compile_fog
		#pragma shader_feature ENABLE_NORMAL_INTENSITY
		
		struct Input
		{
			float4 tc;
			UNITY_FOG_COORDS(0)
		};

		sampler2D _Control,_ControlExtra;
		float4 _Control_ST,_ControlExtra_ST;
		sampler2D _Splat0,_Splat1,_Splat2,_Splat3,_Splat4;
		float4 _Splat0_ST,_Splat1_ST,_Splat2_ST,_Splat3_ST,_Splat4_ST;
		sampler2D _Normal0,_Normal1,_Normal2,_Normal3,_Normal4;
#ifdef ENABLE_NORMAL_INTENSITY
		fixed _NormalIntensity0, _NormalIntensity1, _NormalIntensity2, _NormalIntensity3,
			_NormalIntensity4;
#endif

		#include "../../MTE Common.cginc"

		half _Shininess;

		void MTE_SplatmapVert(inout appdata_full v, out Input data)
		{
			UNITY_INITIALIZE_OUTPUT(Input, data);
			data.tc = v.texcoord;
			float4 pos = UnityObjectToClipPos (v.vertex);
			UNITY_TRANSFER_FOG(data, pos);

			v.tangent.xyz = cross(v.normal, float3(0,0,1));
			v.tangent.w = -1;
		}

		void MTE_SplatmapMix(Input IN, out half weight, out fixed4 mixedDiffuse, inout fixed3 mixedNormal)
		{
			float2 uvControl = TRANSFORM_TEX(IN.tc.xy, _Control);
			float2 uvSplat0 = TRANSFORM_TEX(IN.tc.xy, _Splat0);
			float2 uvSplat1 = TRANSFORM_TEX(IN.tc.xy, _Splat1);
			float2 uvSplat2 = TRANSFORM_TEX(IN.tc.xy, _Splat2);
			float2 uvSplat3 = TRANSFORM_TEX(IN.tc.xy, _Splat3);
			float2 uvSplat4 = TRANSFORM_TEX(IN.tc.xy, _Splat4);

			half4 splat_control = tex2D(_Control, uvControl);
			half4 splat_control_extra = tex2D(_ControlExtra, uvControl);
			weight = dot(splat_control, half4(1, 1, 1, 1));
			weight += dot(splat_control_extra.r, half(1));
			splat_control /= (weight + 1e-3f);
			splat_control_extra /= (weight + 1e-3f);

			mixedDiffuse = 0;
			mixedDiffuse += splat_control.r * tex2D(_Splat0, uvSplat0);
			mixedDiffuse += splat_control.g * tex2D(_Splat1, uvSplat1);
			mixedDiffuse += splat_control.b * tex2D(_Splat2, uvSplat2);
			mixedDiffuse += splat_control.a * tex2D(_Splat3, uvSplat3);
			mixedDiffuse += splat_control_extra.r * tex2D(_Splat4, uvSplat4);
			
#ifdef ENABLE_NORMAL_INTENSITY
			fixed3 nrm0 = UnpackNormal(tex2D(_Normal0, uvSplat0));
			fixed3 nrm1 = UnpackNormal(tex2D(_Normal1, uvSplat1));
			fixed3 nrm2 = UnpackNormal(tex2D(_Normal2, uvSplat2));
			fixed3 nrm3 = UnpackNormal(tex2D(_Normal3, uvSplat3));
			fixed3 nrm4 = UnpackNormal(tex2D(_Normal4, uvSplat4));
			nrm0 = splat_control.r * MTE_NormalIntensity_fixed(nrm0, _NormalIntensity0);
			nrm1 = splat_control.g * MTE_NormalIntensity_fixed(nrm1, _NormalIntensity1);
			nrm2 = splat_control.b * MTE_NormalIntensity_fixed(nrm2, _NormalIntensity2);
			nrm3 = splat_control.a * MTE_NormalIntensity_fixed(nrm3, _NormalIntensity3);
			nrm4 = splat_control_extra.r * MTE_NormalIntensity_fixed(nrm4, _NormalIntensity4);
			mixedNormal = normalize(nrm0 + nrm1 + nrm2 + nrm3 + nrm4);
#else
			fixed4 nrm = 0.0f;
			nrm += splat_control.r * tex2D(_Normal0, uvSplat0);
			nrm += splat_control.g * tex2D(_Normal1, uvSplat1);
			nrm += splat_control.b * tex2D(_Normal2, uvSplat2);
			nrm += splat_control.a * tex2D(_Normal3, uvSplat3);
			nrm += splat_control_extra.r * tex2D(_Normal4, uvSplat4);
			mixedNormal = UnpackNormal(nrm);
#endif
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 mixedDiffuse;
			half weight;
			MTE_SplatmapMix(IN, weight, mixedDiffuse, o.Normal);
			o.Albedo = mixedDiffuse.rgb;
			o.Alpha = weight;
			o.Gloss = mixedDiffuse.a;
			o.Specular = _Shininess;
		}
	ENDCG

	Category
	{
		Tags
		{
			"Queue" = "Geometry-99"
			"RenderType" = "Opaque"
		}
		SubShader//for target 2.5
		{
			CGPROGRAM
				#pragma target 2.5
			ENDCG
		}
		SubShader//for target 2.0
		{
			CGPROGRAM
				#pragma target 2.0
			ENDCG
		}
	}

	Fallback "MTE/Legacy/5 Textures/Bumped/Fog"
	CustomEditor "MTE.MTEShaderGUI"
}
