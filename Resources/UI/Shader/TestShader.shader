Shader "Unlit/TestShader"
{
Properties
{
_speed("speed", Float) = 1
_size("size", Float) = 7
[NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
_Radius("Radius", Float) = 0.5
}
SubShader
{
Tags
{
// RenderPipeline: <None>
"RenderType"="Opaque"
"Queue"="Geometry"
// DisableBatching: <None>
"ShaderGraphShader"="true"
}
Pass
{
    // Name: <None>
    Tags
    {
        // LightMode: <None>
    }

    // Render State
    // RenderState: <None>

    // Debug
    // <None>

    // --------------------------------------------------
    // Pass
         ZWrite On
     ZTest Always  
     Cull Off
    HLSLPROGRAM

    // Pragmas
    #pragma vertex vert
#pragma fragment frag

    // Keywords
    // PassKeywords: <None>
    // GraphKeywords: <None>

    // Defines
    #define ATTRIBUTES_NEED_TEXCOORD0
    #define VARYINGS_NEED_POSITION_WS
    #define VARYINGS_NEED_TEXCOORD0
    /* WARNING: $splice Could not find named fragment 'PassInstancing' */
    #define SHADERPASS SHADERPASS_PREVIEW
#define SHADERGRAPH_PREVIEW 1

    // Includes
    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Packing.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/NormalSurfaceGradient.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/EntityLighting.hlsl"
#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariables.hlsl"
#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"

    // --------------------------------------------------
    // Structs and Packing

    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

    struct Attributes
{
 float3 positionOS : POSITION;
 float4 uv0 : TEXCOORD0;
#if UNITY_ANY_INSTANCING_ENABLED
 uint instanceID : INSTANCEID_SEMANTIC;
#endif
};
struct Varyings
{
 float4 positionCS : SV_POSITION;
 float3 positionWS;
 float4 texCoord0;
#if UNITY_ANY_INSTANCING_ENABLED
 uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
};
struct SurfaceDescriptionInputs
{
 float3 WorldSpacePosition;
 float4 ScreenPosition;
 float4 uv0;
 float3 TimeParameters;
};
struct VertexDescriptionInputs
{
};
struct PackedVaryings
{
 float4 positionCS : SV_POSITION;
 float4 texCoord0 : INTERP0;
 float3 positionWS : INTERP1;
#if UNITY_ANY_INSTANCING_ENABLED
 uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
};

    PackedVaryings PackVaryings (Varyings input)
{
PackedVaryings output;
ZERO_INITIALIZE(PackedVaryings, output);
output.positionCS = input.positionCS;
output.texCoord0.xyzw = input.texCoord0;
output.positionWS.xyz = input.positionWS;
#if UNITY_ANY_INSTANCING_ENABLED
output.instanceID = input.instanceID;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
output.cullFace = input.cullFace;
#endif
return output;
}

Varyings UnpackVaryings (PackedVaryings input)
{
Varyings output;
output.positionCS = input.positionCS;
output.texCoord0 = input.texCoord0.xyzw;
output.positionWS = input.positionWS.xyz;
#if UNITY_ANY_INSTANCING_ENABLED
output.instanceID = input.instanceID;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
output.cullFace = input.cullFace;
#endif
return output;
}


    // --------------------------------------------------
    // Graph

    // Graph Properties
    CBUFFER_START(UnityPerMaterial)
float _speed;
float _size;
float4 _MainTex_TexelSize;
float _Radius;
CBUFFER_END


// Object and Global properties
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);

    // Graph Includes
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Hashes.hlsl"

    // -- Property used by ScenePickingPass
    #ifdef SCENEPICKINGPASS
    float4 _SelectionID;
    #endif

    // -- Properties used by SceneSelectionPass
    #ifdef SCENESELECTIONPASS
    int _ObjectId;
    int _PassValue;
    #endif

    // Graph Functions
    
void Unity_Multiply_float_float(float A, float B, out float Out)
{
Out = A * B;
}

float2 Unity_Voronoi_RandomVector_Deterministic_float (float2 UV, float offset)
{
Hash_Tchou_2_2_float(UV, UV);
return float2(sin(UV.y * offset), cos(UV.x * offset)) * 0.5 + 0.5;
}

void Unity_Voronoi_Deterministic_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
{
float2 g = floor(UV * CellDensity);
float2 f = frac(UV * CellDensity);
float t = 8.0;
float3 res = float3(8.0, 0.0, 0.0);
for (int y = -1; y <= 1; y++)
{
for (int x = -1; x <= 1; x++)
{
float2 lattice = float2(x, y);
float2 offset = Unity_Voronoi_RandomVector_Deterministic_float(lattice + g, AngleOffset);
float d = distance(lattice + offset, f);
if (d < res.x)
{
res = float3(d, offset.x, offset.y);
Out = res.x;
Cells = res.y;
}
}
}
}

void Unity_Power_float(float A, float B, out float Out)
{
    Out = pow(A, B);
}

void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
{
Out = A * B;
}

void Unity_Add_float4(float4 A, float4 B, out float4 Out)
{
    Out = A + B;
}

void Unity_Distance_float2(float2 A, float2 B, out float Out)
{
    Out = distance(A, B);
}

void Unity_Step_float(float Edge, float In, out float Out)
{
    Out = step(Edge, In);
}

    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

    // Graph Vertex
    // GraphVertex: <None>

    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreSurface' */

    // Graph Pixel
    struct SurfaceDescription
{
float4 Out;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
SurfaceDescription surface = (SurfaceDescription)0;
float _Property_f23f3b86025c49f1944c23359627ad49_Out_0_Float = _speed;
float _Multiply_466fd2ab3aec442487b735508d027a6e_Out_2_Float;
Unity_Multiply_float_float(IN.TimeParameters.x, _Property_f23f3b86025c49f1944c23359627ad49_Out_0_Float, _Multiply_466fd2ab3aec442487b735508d027a6e_Out_2_Float);
float _Property_b10c08665d27413d9398616862fab699_Out_0_Float = _size;
float _Voronoi_10d76ba2081c436aab002cfd603a1e0d_Out_3_Float;
float _Voronoi_10d76ba2081c436aab002cfd603a1e0d_Cells_4_Float;
Unity_Voronoi_Deterministic_float(IN.uv0.xy, _Multiply_466fd2ab3aec442487b735508d027a6e_Out_2_Float, _Property_b10c08665d27413d9398616862fab699_Out_0_Float, _Voronoi_10d76ba2081c436aab002cfd603a1e0d_Out_3_Float, _Voronoi_10d76ba2081c436aab002cfd603a1e0d_Cells_4_Float);
float _Power_054163c29dc048f691ea39d62b73934a_Out_2_Float;
Unity_Power_float(_Voronoi_10d76ba2081c436aab002cfd603a1e0d_Out_3_Float, 2, _Power_054163c29dc048f691ea39d62b73934a_Out_2_Float);
float4 Color_a9d804b91eaa4b228522889e9b84c299 = IsGammaSpace() ? float4(0.6603774, 0.1624873, 0.1214845, 0) : float4(SRGBToLinear(float3(0.6603774, 0.1624873, 0.1214845)), 0);
float4 _Multiply_851b03c20cc248e88cef98c7788bca70_Out_2_Vector4;
Unity_Multiply_float4_float4((_Power_054163c29dc048f691ea39d62b73934a_Out_2_Float.xxxx), Color_a9d804b91eaa4b228522889e9b84c299, _Multiply_851b03c20cc248e88cef98c7788bca70_Out_2_Vector4);
float4 Color_d5c10f16c2e24ec4835ba92db1da1aff = IsGammaSpace() ? float4(0.5471698, 0.1264685, 0.1264685, 0) : float4(SRGBToLinear(float3(0.5471698, 0.1264685, 0.1264685)), 0);
float4 _Add_7249e4a9f19c479fba92b6ecdcae62b2_Out_2_Vector4;
Unity_Add_float4(_Multiply_851b03c20cc248e88cef98c7788bca70_Out_2_Vector4, Color_d5c10f16c2e24ec4835ba92db1da1aff, _Add_7249e4a9f19c479fba92b6ecdcae62b2_Out_2_Vector4);
float4 _ScreenPosition_a6c31f55f05847bfbbfa34965d3fa7d8_Out_0_Vector4 = IN.ScreenPosition;
float _Split_b739a3a9c10042f797f50f24d5b78799_R_1_Float = _ScreenPosition_a6c31f55f05847bfbbfa34965d3fa7d8_Out_0_Vector4[0];
float _Split_b739a3a9c10042f797f50f24d5b78799_G_2_Float = _ScreenPosition_a6c31f55f05847bfbbfa34965d3fa7d8_Out_0_Vector4[1];
float _Split_b739a3a9c10042f797f50f24d5b78799_B_3_Float = _ScreenPosition_a6c31f55f05847bfbbfa34965d3fa7d8_Out_0_Vector4[2];
float _Split_b739a3a9c10042f797f50f24d5b78799_A_4_Float = _ScreenPosition_a6c31f55f05847bfbbfa34965d3fa7d8_Out_0_Vector4[3];
float2 _Vector2_e5504c4046464f20ae457430c599df5d_Out_0_Vector2 = float2(_Split_b739a3a9c10042f797f50f24d5b78799_R_1_Float, _Split_b739a3a9c10042f797f50f24d5b78799_G_2_Float);
float _Distance_8aee746bf09a4d7582e6db1eb8d519c8_Out_2_Float;
Unity_Distance_float2(_Vector2_e5504c4046464f20ae457430c599df5d_Out_0_Vector2, float2(0.5, 0.5), _Distance_8aee746bf09a4d7582e6db1eb8d519c8_Out_2_Float);
float _Property_69435c2a57fe4a2aa8612cf6a0bbda4a_Out_0_Float = _Radius;
float _Step_8c8efd61db7d423fb2923305e16b8606_Out_2_Float;
Unity_Step_float(_Distance_8aee746bf09a4d7582e6db1eb8d519c8_Out_2_Float, _Property_69435c2a57fe4a2aa8612cf6a0bbda4a_Out_0_Float, _Step_8c8efd61db7d423fb2923305e16b8606_Out_2_Float);
float4 _Multiply_1215584e84a94712b056e93113ab4636_Out_2_Vector4;
Unity_Multiply_float4_float4(_Add_7249e4a9f19c479fba92b6ecdcae62b2_Out_2_Vector4, (_Step_8c8efd61db7d423fb2923305e16b8606_Out_2_Float.xxxx), _Multiply_1215584e84a94712b056e93113ab4636_Out_2_Vector4);
surface.Out = all(isfinite(_Multiply_1215584e84a94712b056e93113ab4636_Out_2_Vector4)) ? half4(_Multiply_1215584e84a94712b056e93113ab4636_Out_2_Vector4.x, _Multiply_1215584e84a94712b056e93113ab4636_Out_2_Vector4.y, _Multiply_1215584e84a94712b056e93113ab4636_Out_2_Vector4.z, 1.0) : float4(1.0f, 0.0f, 1.0f, 1.0f);
return surface;
}

    // --------------------------------------------------
    // Build Graph Inputs

    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

    /* WARNING: $splice Could not find named fragment 'CustomInterpolatorCopyToSDI' */





    output.WorldSpacePosition =                         input.positionWS;
    output.ScreenPosition =                             ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);

    #if UNITY_UV_STARTS_AT_TOP
    #else
    #endif


    output.uv0 =                                        input.texCoord0;
    output.TimeParameters =                             _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/PreviewVaryings.hlsl"
#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/PreviewPass.hlsl"

    ENDHLSL
}
}
CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
FallBack "Hidden/Shader Graph/FallbackError"
}