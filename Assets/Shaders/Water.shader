Shader "Unlit/Water"
{
    Properties
    {
    _Color ("Color", Color) = (1.0,1.0,1.0,1.0)
    _WaveColor("WaveColor",Color) = (1.0,1.0,1.0,1.0)
    _ShadowColor ("ShadowColor", Color) = (1.0,1.0,1.0,1.0)
    _Speed("Speed",Vector) = (1.0,1.0,1.0,1.0)
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
                float4 wPos : TEXCOORD2;
                float4 pos : SV_POSITION; // posに変更する！！TRANSFER_SHADOWがposとうい名前でないと受け付けない。
                SHADOW_COORDS(1)
            };

            float4 _Color;
            float4 _WaveColor;
            float4 _ShadowColor;
            float2 _Speed;
            float2 random2( float2 p ) {
                return frac(sin(float2(dot(p,float2(127.1,311.7)),dot(p,float2(269.5,183.3))))*43758.5453);
            }

            v2f vert (appdata v)
            {
                v2f o;
                // ここの左辺もposに変更
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                o.wPos = mul(unity_ObjectToWorld,v.vertex);
                half NdotL = saturate(dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = NdotL * _LightColor0;
                TRANSFER_SHADOW(o)

                return o;
            }

            float2 dist(float2 uv)
            {
                
                float2 st = uv;
                st *= 10.;

                // Tile the space
                float2 i_st = floor(st);
                float2 f_st = frac(st);

                float m_dist = 1.;
                float n_dist = 1.;// minimum distance

                for (int y= -1; y <= 1; y++) {
                    for (int x= -1; x <= 1; x++) {
                        // Neighbor place in the grid
                        float2 neighbor = float2(float(x),float(y));

                        float2 po = random2(i_st + neighbor);

                        po = 0.5 + 0.5 * float2(sin(_Time.y + 6.2831*po.x),sin(_Time.y + 6.2831*po.y));

                        float2 diff = neighbor + po - f_st;

                        float dist = length(diff);

                        if(m_dist > dist)
                        {
                            n_dist = m_dist;
                            m_dist = dist;
                            
                        }else if(n_dist > dist)
                        {
                            n_dist = dist;
                        }
                    }
                }
                return float2(m_dist,n_dist);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color;

                float3 color = float3(0.0,0.0,0.0);

                // Scale
                float2 uv = float2(i.wPos.x + i.wPos.y,i.wPos.z) / 5.0 + _Speed * _Time.y;
                uv.x += sin(uv.y*10.0+_Time.y)*0.05;
                uv.y += sin(uv.x*10.0+_Time.y)*0.05;
                float2 c_dist = dist(uv);

            // Draw the min distance (distance field)
                color += c_dist.x;

                col = float4(c_dist.y - c_dist.x > 0.05 ? _Color.xyz:_WaveColor.xyz,1.0);
            //col.xyz = color;
                
            // 影を計算
                fixed4 shadow = SHADOW_ATTENUATION(i);
                col = i.diff * shadow > 0.1 ? col : _ShadowColor ;
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
