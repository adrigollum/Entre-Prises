Shader "Custom/TMP_PointSpotLight"
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

        _LightPos("Light Position", Vector) = (0,0,0,1)
        _LightColor("Light Color", Color) = (1,1,1,1)
        _LightIntensity("Light Intensity", Float) = 1.0
        _LightRange("Light Range", Float) = 10.0
        _LightSpotAngle("Spot Angle (degrees)", Float) = 30.0
        _LightIsSpot("Is Spot Light", Float) = 0.0
        _LightDirection("Light Direction", Vector) = (0,0,-1,0)
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

            float3 _LightPos;
            float4 _LightColor;
            float _LightIntensity;
            float _LightRange;
            float _LightSpotAngle;      // en degrés
            float _LightIsSpot;         // 0 = point, 1 = spot
            float3 _LightDirection;     // direction du spot (normalisée)

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
                // Sample alpha from SDF texture
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

                // Calcul direction lumière
                float3 lightVec = _LightPos - IN.posWS;
                float distLight = length(lightVec);
                if (distLight > 0)
                    lightVec /= distLight;
                else
                    lightVec = float3(0,0,0);

                // Atténuation inverse quadratique
                float atten = saturate(1.0 - distLight / _LightRange);
                atten *= atten;

                // Si spot, appliquer le cône
                if (_LightIsSpot > 0.5)
                {
                    float spotCos = dot(-lightVec, normalize(_LightDirection));
                    float spotAngleCos = cos(radians(_LightSpotAngle * 0.5));
                    if (spotCos < spotAngleCos)
                        atten = 0.0; // hors cône spot
                    else
                    {
                        // Atténuation douce au bord du cône
                        float spotAtten = smoothstep(spotAngleCos, spotAngleCos + 0.1, spotCos);
                        atten *= spotAtten;
                    }
                }

                // Calcul éclairage diffuse + speculaire
                float NdotL = saturate(dot(normal, lightVec));
                float3 halfDir = normalize(lightVec + viewDir);
                float NdotH = saturate(dot(normal, halfDir));
                float3 diffuse = baseColor.rgb * _LightColor.rgb * _LightIntensity * NdotL * atten;
                float spec = pow(NdotH, 32.0) * _Smoothness * _LightIntensity * atten;
                float3 specular = spec * _Metallic * _LightColor.rgb;

                float3 lighting = diffuse + specular;

                return float4(lighting, alpha);
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/InternalErrorShader"
}
