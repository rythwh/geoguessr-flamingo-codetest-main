Shader "Custom/URPGridShader"
{
    Properties
    {
        _GridColor ("Grid Color", Color) = (1,1,1,1)
        _BackgroundColor ("Background Color", Color) = (0,0,0,1)
        _LineThickness ("Line Thickness", Range(0.01, 0.1)) = 0.02
        _GridScale ("Grid Scale", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        Pass
        {
            Name "ForwardLit"
            Blend One Zero
            Cull Off
            ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 worldPos : TEXCOORD0; // Keep as float2 for grid calculations
            };

            float4 _GridColor;
            float4 _BackgroundColor;
            float _LineThickness;
            float _GridScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.position = TransformObjectToHClip(v.vertex);

                // Get world position as float3
                float3 worldPos = TransformObjectToWorld(v.vertex);
                o.worldPos = worldPos.xz; // Extract X and Z for grid calculations

                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Scale grid to maintain 1x1 units in world space
                float2 scaledUV = i.worldPos / _GridScale;

                // Calculate grid UVs
                float2 gridUV = abs(frac(scaledUV) - 0.5);

                // Determine if the pixel lies within a grid line
                float on_line = step(gridUV.x, _LineThickness) + step(gridUV.y, _LineThickness);

                // Blend between grid and background colors
                float4 color = lerp(_BackgroundColor, _GridColor, on_line);

                return color;
            }
            ENDHLSL
        }
    }
}