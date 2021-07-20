#ifndef __POINT_INFO_INCLUDED__
#define __POINT_INFO_INCLUDED__

struct Voxel {
	float3 pos;
	float value;
};

struct PolyInfo {

	float3 v1, v2, v3;
	float3 facetNormal;
};
#endif