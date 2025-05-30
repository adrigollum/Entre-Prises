Shader "Custom/TMP_URP_Lit"
{
    Properties
    {
        _MainTex("Font Atlas", 2D) = "white" {}
        _FaceColor("Face Color", Color) = (1,1,1,1)
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Range(0, 1)) = 0.05
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "AlphaTest" }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 posWS : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _FaceColor;
            float4 _OutlineColor;
            float _OutlineWidth;
            float _Cutoff;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs posInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = posInputs.positionCS;
                OUT.posWS = posInputs.positionWS;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float distance = tex2D(_MainTex, IN.uv).a;

                // Alpha clip (text shape)
                float outlineEdge = 0.5 - _OutlineWidth * 0.5;
                float alpha = smoothstep(outlineEdge - 0.01, outlineEdge + 0.01, distance);
                clip(alpha - _Cutoff);

                // Lighting
                float3 normal = normalize(IN.normalWS);
                float3 lightDir = normalize(_MainLightPosition.xyz);
                float NdotL = saturate(dot(normal, lightDir));
                float3 litColor = _FaceColor.rgb * _MainLightColor.rgb * NdotL;

                return float4(litColor, alpha);
            }

            ENDHLSL
        }
    }

    FallBack "Hidden/InternalErrorShader"
}
