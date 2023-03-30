Shader "UI/BlurWithBackground"
{
    Properties
    {
        _Radius ("Blur Radius", Range(0.0, 1.0)) = 0.1
        _MainTex ("Texture", 2D) = "white" {}
        _BackgroundTex ("Background Texture", 2D) = "white" {}
    }
 
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Opaque" }
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
 
            sampler2D _MainTex;
            sampler2D _BackgroundTex;
            float4 _MainTex_ST;
            float _Radius;
 
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = (v.uv * _MainTex_ST.xy) + _MainTex_ST.zw;
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 backgroundCol = tex2D(_BackgroundTex, i.uv);
                fixed4 sum = fixed4(0.0, 0.0, 0.0, 0.0);
                int count = 0;
 
                // Sample surrounding pixels
                for (float x = -_Radius; x <= _Radius; x += _Radius)
                {
                    for (float y = -_Radius; y <= _Radius; y += _Radius)
                    {
                        sum += tex2D(_MainTex, i.uv + float2(x, y));
                        count++;
                    }
                }
 
                return (col + (sum / count)) / 2.0 + backgroundCol;
            }
            ENDCG
        }
    }
    FallBack "UI/Default"
}