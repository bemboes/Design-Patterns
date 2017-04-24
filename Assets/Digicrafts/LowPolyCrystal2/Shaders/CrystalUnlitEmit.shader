// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Digicrafts/Unlit/CrystalEmission"
{
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Opacity ("Opacity", Range(0.0,1.0)) = 1.0
		_MainTex ("Main", 2D) = "white" {}
		_NormalMap ("Normal", 2D) = "white" {}
		[HDR] _EmissionColor ("Color", Color) = (1,1,1,1)
		_EmissionTex ("Emission", 2D) = "white" {}
		_Emission ("Emission Strength", Range(0.0,2.0)) = 0.0
		_ReflectionStrength ("Reflection Strength", Range(0.0,1.0)) = 0.5
		_FresnelStrength ("Fresnel Strength", Range(0.0,1.0)) = 0.5
		_RefractionStrength ("Refraction Strength", Range(0.0,1.0)) = 0.5
		[NoScaleOffset] _RefractTex ("Refraction Texture", Cube) = "" {}
		_AmbientLight ("Ambient Light", Range(0.0,2.0)) = 1.0

	}
	SubShader {		

// lightmap
		Pass
		{
			Name "Meta"
			Tags 
			{
				"LightMode"="Meta"
			}
			Cull Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#define UNITY_PASS_META 1
			#define _GLOSSYENV 1
			#include "UnityCG.cginc"
			#include "UnityPBSLighting.cginc"
			#include "UnityStandardBRDF.cginc"
			#include "UnityMetaPass.cginc"
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_fog
			#pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
			#pragma target 3.0
			uniform half4 _EmissionColor;
			uniform fixed _Emission;
			uniform sampler2D _EmissionTex;
			struct VertexInput 
			{
				half4 vertex : POSITION;
				half2 texcoord0 : TEXCOORD0;
				half2 texcoord1 : TEXCOORD1;
				half2 texcoord2 : TEXCOORD2;
			};
			struct VertexOutput 
			{
				half4 pos : SV_POSITION;
				half2 uv0 : TEXCOORD0;
			};
			VertexOutput vert (VertexInput v) 
			{
				VertexOutput o = (VertexOutput)0;
				o.uv0 = v.texcoord0;
				o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
				return o;
			}
			half4 frag(VertexOutput i) : SV_Target 
			{
				/////// Vectors:
				UnityMetaInput o;
				UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
				float4 e = Luminance(tex2D (_EmissionTex, i.uv0));

				o.Emission = e.rgb*_EmissionColor*_Emission;//half3(2,2,2);//((_EmissionColor.rgb*_EmissionMap_var.rgb)*_EmissionBakeIntensity);
				o.Albedo = half3(0,0,0);

				return UnityMetaFragment( o );
			}

			#pragma shader_feature _EMISSION
            #pragma shader_feature _METALLICGLOSSMAP
            #pragma shader_feature ___ _DETAIL_MULX2

			ENDCG
		}

		Tags {
			"Queue" = "Transparent" 
			"RenderType"="Transparent"		
			"IgnoreProjector"="True"
		}

// back
		Pass {

			Cull Front
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
        
			struct v2f {
				float4 pos : SV_POSITION;
				float3 uv : TEXCOORD0;
				float2 uv_MainTex : TEXCOORD1;
			};

			v2f vert (float4 v : POSITION, float3 normal : NORMAL, float2 uv_MainTex: TEXCOORD1)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v);

				// TexGen CubeReflect:
				// reflect view direction along the normal, in view space.
				float3 viewDir = normalize(ObjSpaceViewDir(v));
				o.uv = -reflect(viewDir, normal);
				o.uv = mul(unity_ObjectToWorld, float4(o.uv,0.0f));
				o.uv_MainTex = uv_MainTex;

				return o;
			}

			uniform fixed4 _Color;
			uniform half _Opacity;
			uniform half _Emission;
			uniform fixed4 _EmissionColor;

			uniform sampler2D _MainTex;
			uniform sampler2D _EmissionTex;

			samplerCUBE _RefractTex;
			half _RefractionStrength;


			half4 frag (v2f i) : SV_Target
			{				
				float4 c = Luminance(tex2D (_MainTex, i.uv_MainTex));
				float4 e = Luminance(tex2D (_EmissionTex, i.uv_MainTex));
				half3 refraction = texCUBE(_RefractTex, i.uv).rgb*_RefractionStrength*_Color.rgb;

				// Calculate Colors
				float3 cc =  (c.rgb*_Color.rgb)  + e.rgb*refraction.rgb*_Color;
				return half4(cc.rgb, _Opacity*e.a);
			}
			ENDCG 
		}

// front
		Pass {
			Tags {"LightMode" = "ForwardBase"}
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
//			#include "Lighting.cginc"
        	// compile shader into multiple variants, with and without shadows
            // (we don't care about any lightmaps yet, so skip these variants)
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            // shadow helper functions and macros
            #include "AutoLight.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				float3 uv : TEXCOORD0;
//				SHADOW_COORDS(1)
				half fresnel : TEXCOORD1;
                half3 tspace0 : TEXCOORD2; // tangent.x, bitangent.x, normal.x
                half3 tspace1 : TEXCOORD3; // tangent.y, bitangent.y, normal.y
                half3 tspace2 : TEXCOORD4; // tangent.z, bitangent.z, normal.z
                float3 worldPos : TEXCOORD5;
                half3 worldViewDir : TEXCOORD6;
			};


			v2f vert (float4 v : POSITION, float3 normal : NORMAL, float4 tangent : TANGENT, float3 uv: TEXCOORD)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v);
				o.worldPos = mul(unity_ObjectToWorld, v).xyz;

				// frensnel and uv
				float3 viewDir = normalize(ObjSpaceViewDir(v));
				o.fresnel = 1.0 - saturate(dot(normal,viewDir));
				o.uv = uv;

				// Calculate normals
				half3 wNormal = UnityObjectToWorldNormal(normal);
                half3 wTangent = UnityObjectToWorldDir(tangent.xyz);
                // compute bitangent from cross product of normal and tangent
                half tangentSign = tangent.w * unity_WorldTransformParams.w;
                half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
                // output the tangent space matrix
                o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
                o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
                o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);

                // rest the same as in previous shader
                o.worldViewDir = normalize(UnityWorldSpaceViewDir(o.worldPos));
//                o.worldRefl = reflect(-worldViewDir, worldNormal);

//                TRANSFER_SHADOW(o)				

				return o;
			}

			uniform fixed4 _Color;
			uniform half _Opacity;
			uniform half _AmbientLight;
			uniform half _Emission;
			uniform fixed4 _EmissionColor;

			uniform sampler2D _MainTex;
			uniform sampler2D _EmissionTex;
			uniform sampler2D _NormalMap;

			uniform half _RefractionStrength;
			uniform half _ReflectionStrength;
			uniform half _FresnelStrength;

			half4 frag (v2f i) : SV_Target
			{	

				// sample the normal map, and decode from the Unity encoding
                half3 tnormal = UnpackNormal(tex2D(_NormalMap, i.uv));
                // transform normal from tangent to world space
                half3 worldNormal;
                worldNormal.x = dot(i.tspace0, tnormal);
                worldNormal.y = dot(i.tspace1, tnormal);
                worldNormal.z = dot(i.tspace2, tnormal);
//
//                // rest the same as in previous shader
                half3 worldRefl = reflect(-i.worldViewDir, worldNormal);
                half4 skyData = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, worldRefl);
                half3 skyColor = DecodeHDR (skyData, unity_SpecCube0_HDR);

                // Calculate Colors
				float4 c = Luminance(tex2D (_MainTex, i.uv));
				float4 e = Luminance(tex2D (_EmissionTex, i.uv));

				// compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
//                fixed shadow = SHADOW_ATTENUATION(i);

				// Calculate Colors
				float3 cc = c.rgb*_Color.rgb + e.rgb*_EmissionColor*_Emission + e.rgb* skyColor.rgb * _ReflectionStrength * (1 - (1-i.fresnel)*_FresnelStrength);
				return half4(cc,(1-(1-e.a)*_RefractionStrength*0.5)*_Opacity);

			}
			ENDCG

		}

// shadow
		Pass
        {
            Tags {"LightMode"="ShadowCaster"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f { 
                V2F_SHADOW_CASTER;
            };

            v2f vert(appdata_base v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }	

	}
}
