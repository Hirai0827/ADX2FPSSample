Shader "Unlit/NoiseSprite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent"  }
        LOD 100

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed2x2 genRot(float t)
            {
                return fixed2x2(cos(t),-sin(t),sin(t),cos(t));
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed2 uv = i.uv;
                float div = 20.0;
                float width = 0.2;
                float shakeLine = frac(_Time.y / (1.0 + width*8.0)) * (1.0 + width*8.0) - width*4.0;
                float effectAmount = pow(max(width-abs(shakeLine - floor(uv.y*div)/div),0.0)/width,3.0);
                float width2 = 0.4;
                float shakeLine2 = frac(-_Time.y/2.0 / (1.0 + width2 + width2)) * (1.0 + width2 + width2) - width2;
                float effectAmount2 = pow(max(width2-abs(shakeLine2 - uv.y),0.0)/width2,3.0);
                effectAmount = sin(sin(effectAmount * 1.57) * 1.57);
                float signedItem = sign(shakeLine - floor(uv.y*div)/div);
                //uv.x += sin(uv.y * 200.0 + _Time.y*100.0) * effectAmount;
                fixed2 grid = floor(i.uv * div);
                fixed2 st = mul((frac(i.uv * div) - 0.5) / max(1.0 -effectAmount*1.0,0.0) , genRot(signedItem*1.0*effectAmount)) + 0.5;
                uv = (grid + st)/div;
                uv.x += sin(uv.y * 100.0f + _Time.y*1500.0) * 0.0125*tan(effectAmount2*1.57)*tan(effectAmount2*1.57);
                fixed4 col = tex2D(_MainTex, uv);
                col = 0.0 < st.x && st.x < 1.0 && 0.0 < st.y && st.y < 1.0 ? col : 0.0;
                col = sin(uv.y * 400.0 + _Time.y*10.0) < 0.5 ? col : 0.0; 
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                col.xyz *= 2.0;
                return col;
            }
            ENDCG
        }
    }
}
