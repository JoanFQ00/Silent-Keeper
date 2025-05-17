Shader "Custom/BlackOnlyAlphaShader_Offset"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold ("White Threshold", Range(0,1)) = 0.5
        _Offset ("UV Offset", Vector) = (0,0,0,0)
        _Scale ("UV Scale", Vector) = (1,1,0,0)
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

            float _Threshold;
            float2 _Offset;
            float2 _Scale;

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
                // Transformamos las UV: escalamos y luego desplazamos
                float2 uvTransformed = (IN.uv - 0.5) / _Scale + 0.5 + _Offset;

                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvTransformed);

                // Calculamos luminancia para decidir transparencia
                float luminance = dot(col.rgb, float3(0.299, 0.587, 0.114));

                if (luminance > _Threshold)
                {
                    // Blanco (zona blanca): transparente
                    return half4(0, 0, 0, 0);
                }
                else
                {
                    // Negro o borde: visible negro
                    return half4(0, 0, 0, 1);
                }
            }
            ENDHLSL
        }
    }
}