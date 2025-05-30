Shader "Custom/TMP_URP_AdvancedLit"
{
    Properties
    {
        _MainTex("Font Atlas", 2D) = "white" {}
        _FaceColor("Face Color", Color) = (1,1,1,1)
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth("Outline Width", Range(0,1)) = 0.1
        _FaceDilate("Face Dilate", Range(-1,1)) = 0.0
        _Softness("Softness", Range(0,1)) = 0.0
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _Smoothness("Smoothness", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "Queue" = "AlphaTest" }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" "UniversalMaterialType" = "Lit" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHTS

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #define _ADDITIONAL_LIGHTS
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
            float _FaceDilate;
            float _Softness;
            float _Cutoff;
            float _Metallic;
            float _Smoothness;

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
                // === Sample alpha from SDF texture ===
                float dist = tex2D(_MainTex, IN.uv).a;
                float d = dist + _FaceDilate;
                float softness = _Softness * 0.01;
                float outlineEdge = 0.5 - _OutlineWidth * 0.5;

                float alpha = smoothstep(outlineEdge - softness, outlineEdge + softness, d);
                clip(alpha - _Cutoff);

                float blend = smoothstep(0.5 - _OutlineWidth, 0.5 + _OutlineWidth, d);
                float4 baseColor = lerp(_OutlineColor, _FaceColor, blend);

                float3 normal = normalize(IN.normalWS);
                float3 viewDir = normalize(GetWorldSpaceViewDir(IN.posWS));

                // === Main light ===
                Light mainLight = GetMainLight();
                float3 lightDir = normalize(mainLight.direction);
                float3 halfDir = normalize(lightDir + viewDir);
                float NdotL = saturate(dot(normal, lightDir));
                float NdotH = saturate(dot(normal, halfDir));
                float3 diffuse = baseColor.rgb * mainLight.color * NdotL;
                float spec = pow(NdotH, 32.0) * _Smoothness;
                float3 specular = spec * _Metallic;

                float3 lighting = diffuse + specular;

                // === Additional lights ===
                
                int lightCount = GetAdditionalLightsCount();
                for (int i = 0; i < lightCount; ++i)
                {
                    Light light = GetAdditionalLight(i, IN.posWS);
                
                    float3 lightDirAdd = normalize(light.direction);
                    float3 halfDirAdd = normalize(lightDirAdd + viewDir);
                    float nl = saturate(dot(normal, lightDirAdd));
                    float nh = saturate(dot(normal, halfDirAdd));
                
                    float3 diffAdd = baseColor.rgb * light.color * nl;
                    float3 specAdd = pow(nh, 32.0) * light.color * _Metallic;
                
                    lighting += diffAdd + specAdd;
                }
                return float4(lighting, alpha);
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/InternalErrorShader"
}
