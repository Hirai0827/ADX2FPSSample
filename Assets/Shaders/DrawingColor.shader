Shader "Unlit/DrawingColor"
{
    Properties
    {
        _Color ("Color", Color) = (1.0,1.0,1.0,1.0)
        _ShadowColor ("Color", Color) = (1.0,1.0,1.0,1.0)
        _ShadowThr("ShadowThreshold",Range(-1.0,1.0)) = 0.1
        [Toggle]_CalcShadow("CalcShadow",int) = 1.0
    }
    SubShader
    {
        Pass
        {
            Tags { "LightMode"="ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            #include "UnityCG.cginc" 
            #include "UnityLightingCommon.cginc"
            #include "Lighting.cginc" 
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 diff : COLOR0;
                float4 pos : SV_POSITION; // posに変更する！！TRANSFER_SHADOWがposとうい名前でないと受け付けない。
                SHADOW_COORDS(1)
            };

            float4 _Color;
            float4 _ShadowColor;
            float _ShadowThr;
            int _CalcShadow;
            

            v2f vert (appdata v)
            {
                v2f o;
                // ここの左辺もposに変更
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half NdotL = saturate(dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = NdotL * _LightColor0;
                TRANSFER_SHADOW(o)

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color;
                // 影を計算
                fixed4 shadow = SHADOW_ATTENUATION(i);
                col = ((i.diff * (_CalcShadow == 1 ? shadow : 1.0)) > _ShadowThr) ? _Color : _ShadowColor ;
                return col;
            }
            ENDCG
        }

        Pass
        {
            Tags{ "LightMode"="ShadowCaster" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                V2F_SHADOW_CASTER;
            };

            v2f vert (appdata v)
            {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }

    }
}