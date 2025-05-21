Shader "Custom/2D/DarkFog"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _FogColor ("Fog Color", Color) = (0, 0, 0, 0.35)
        _Speed ("Scroll Speed", Vector) = (0.05, 0.03, 0, 0)
        _Tiling ("Tiling", Vector) = (2, 2, 0, 0)
        _Contrast ("Contrast", Float) = 1.2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _FogColor;
            float4 _Speed;
            float4 _Tiling;
            float _Contrast;

            float noise(float2 p)
            {
                return frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453);
            }

            float smoothNoise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                float a = noise(i);
                float b = noise(i + float2(1, 0));
                float c = noise(i + float2(0, 1));
                float d = noise(i + float2(1, 1));

                float2 u = f * f * (3.0 - 2.0 * f);

                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 scrollUV = i.uv * _Tiling.xy + _Time.y * _Speed.xy;

                float fogNoise = smoothNoise(scrollUV * 10.0);
                fogNoise = pow(fogNoise, _Contrast);

                float alpha = _FogColor.a * fogNoise;

                return half4(_FogColor.rgb, alpha);
            }
            ENDHLSL
        }
    }
}