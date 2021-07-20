// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Custom/VoxelShader"
{
	SubShader
	{
		Cull Back

		Pass
		{
			Tags {"LightMode" = "ForwardBase"}

			CGPROGRAM

			#pragma target 4.0
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "PointInfo.cginc"
			#include "LightInfo.cginc"

			uniform int cubeNum;

			uniform StructuredBuffer<PolyInfo> polyBuffer;
			
			struct V2G {

				float3 v1 : TEXCOORD0;
				float3 v2 : TEXCOORD1;
				float3 v3 : TEXCOORD2;
				float3 normal : TEXCOORD3;
			};
			struct G2F {

				float4 vertex : SV_POSITION;
				float3 normal : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
			};

			V2G vert(uint id : SV_VertexID)
			{
				V2G output;

				PolyInfo curPt = polyBuffer[id];

				output.v1 = curPt.v1;
				output.v2 = curPt.v2;
				output.v3 = curPt.v3;

				output.normal = curPt.facetNormal;

				return output;
			}


			[maxvertexcount(3)]
			void geom(point V2G IN[1], inout TriangleStream<G2F> OUT)
			{
				G2F output;

				output.worldPos = IN[0].v1;
				output.vertex = UnityObjectToClipPos(float4(IN[0].v1, 1));
				output.normal = IN[0].normal;
				OUT.Append(output);

				output.worldPos = IN[0].v2;
				output.vertex = UnityObjectToClipPos(float4(IN[0].v2, 1));
				output.normal = IN[0].normal;
				OUT.Append(output);

				output.worldPos = IN[0].v3;
				output.vertex = UnityObjectToClipPos(float4(IN[0].v3, 1));
				output.normal = IN[0].normal;
				OUT.Append(output);
			}


			float4 frag(G2F input) : COLOR
			{
				float3 worldLightDir = normalize(input.worldPos - worldLightPos);

				float3 diffuse = saturate(dot(input.normal, -worldLightDir));
				
				return float4(diffuse + 0.2,1);
			}
			ENDCG
		}
	}
}
