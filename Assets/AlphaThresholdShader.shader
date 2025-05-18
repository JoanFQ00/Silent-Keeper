Shader "Custom/FearAuraShaderBlackBlend"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Offset ("UV Offset", Vector) = (0,0,0,0)
        _Scale ("UV Scale", Vector) = (1,1,0,0)
        _AuraColor ("Aura Color", Color) = (1, 0, 0, 1) // Color del aura (ej: rojo)
        _AuraThickness ("Aura Thickness", Range(0.001, 0.2)) = 0.05
        _Threshold ("White Threshold", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float2 _Offset;
            float2 _Scale;
            float4 _AuraColor;
            float _AuraThickness;
            float _Threshold;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // UV transform
                float2 uvTransformed = (IN.uv - 0.5) / _Scale + 0.5 + _Offset;
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvTransformed);

                // Descarta lo blanco
                float luminance = dot(col.rgb, float3(0.299, 0.587, 0.114));
                if (luminance > _Threshold)
                {
                    discard;
                }

                // Distancia a bordes
                float2 distToEdge = min(IN.uv, 1.0 - IN.uv);
                float minDist = min(distToEdge.x, distToEdge.y);

                float auraFactor = smoothstep(_AuraThickness, 0.0, minDist);

                // Color resultante: mezcla de negro con el aura
                float3 blended = lerp(float3(0, 0, 0), _AuraColor.rgb, auraFactor);

                return half4(blended, 1.0); // SIEMPRE opaco
            }
            ENDHLSL
        }
    }
}