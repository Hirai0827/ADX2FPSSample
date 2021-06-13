Shader "Unlit/Light"
{
    Properties
    {
        _LightColorBright("LightColorBright",Color) = (0.0,0.0,0.0)
        _LightColorDark("LightColorDark",Color) = (0.0,0.0,0.0)
        _LightThreshold("LightThreshold",Float) = 0.5
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" }

        GrabPass
        {
            "_BGTex"
        }

        Pass
        {
            CGPROGRAM
           #pragma vertex vert
           #pragma fragment frag

           #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 grabPos : TEXCOORD0;
            };

            fixed4 _LightColorBright;
            fixed4 _LightColorDark;
            float _LightThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.grabPos = o.vertex;
#if !UNITY_UV_START_AT_TOP
                o.grabPos.y *= -1;
#endif
                return o;
            }

            sampler2D _BGTex;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.grabPos / i.grabPos.w * 0.5 + 0.5;
                fixed4 col = tex2D(_BGTex, uv);
                float mono = (col.x + col.y + col.z)/3.0;
                mono = mono > _LightThreshold ? 1.0 : 0.0;
                col = lerp(_LightColorDark,_LightColorBright,mono);
                return col;
            }
            ENDCG
        }
    }
}