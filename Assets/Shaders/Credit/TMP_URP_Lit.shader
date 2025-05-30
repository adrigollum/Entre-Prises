Shader "Custom/TMP_DynamicLit"
{
    Properties
    {
        _MainTex("Font Atlas", 2D) = "white" {}
        _FaceColor("Face Color", Color) = (1,1,1,1)
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Range(0, 1)) = 0.05
        _FaceDilate("Face Dilate", Range(-1, 1)) = 0.0
        _Softness("Softness", Range(0, 1)) = 0.0
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "AlphaTest" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade

        sampler2D _MainTex;
        float4 _FaceColor;
        float4 _OutlineColor;
        float _OutlineWidth;
        float _FaceDilate;
        float _Softness;
        float _Cutoff;
        float _Glossiness;
        float _Metallic;

        struct Input
        {
            float2 uv_MainTex;
        };

        inline float GetDistanceFieldAlpha(float d, float width, float softness)
        {
            float edge = 0.5 - width * 0.5;
            float alpha = smoothstep(edge - softness, edge + softness, d);
            return alpha;
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float4 tex = tex2D(_MainTex, IN.uv_MainTex);

            // Distance Field threshold
            float distance = tex.a;

            float width = _OutlineWidth;
            float softness = _Softness * 0.01;
            float alpha = GetDistanceFieldAlpha(distance, width, softness);

            // Apply alpha cutoff
            clip(alpha - _Cutoff);

            // Blend face and outline colors based on distance
            float outlineBlend = smoothstep(0.5 - width, 0.5 + width, distance);
            float4 face = _FaceColor;
            float4 outline = _OutlineColor;
            float4 finalColor = lerp(outline, face, outlineBlend);

            o.Albedo = finalColor.rgb;
            o.Alpha = alpha;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG
    }

    FallBack "Diffuse"
}

