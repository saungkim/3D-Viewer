Shader "Unlit/Spine Outline Dest Alpha"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _MaskColor ("Mask Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(0, 4)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        ZWrite Off
        Blend OneMinusDstAlpha DstAlpha
        ColorMask RGB
 
        CGINCLUDE
        #include "UnityCG.cginc"
 
        sampler2D _MainTex;
        float4 _MainTex_ST;
 
        fixed4 _OutlineColor;
            fixed4 _MaskColor;
        float _OutlineWidth;
 
        struct v2fOutline
        {
            float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
        };
 
        v2fOutline vertOutline (appdata_base v, float2 offset)
        {
            v2fOutline o;
            o.pos = UnityObjectToClipPos(v.vertex);
            o.pos.xy += offset * 2 * o.pos.w * _OutlineWidth / _ScreenParams.xy;
            o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
            return o;
        }
 
        fixed4 fragOutlineMask (v2fOutline i) : SV_Target
        {
            fixed alpha = tex2D(_MainTex, i.uv).a;
            fixed4 col = _MaskColor;
            col.a = 0;
            return col;
        }
 
        fixed4 fragOutline (v2fOutline i) : SV_Target
        {
            fixed alpha = tex2D(_MainTex, i.uv).a;
            fixed4 col = _OutlineColor;
            col.a *= alpha;
            return col;
        }
        ENDCG
 
        Pass
        {
            Name "OUTLINEALPHA"
            BlendOp Min
            ColorMask A
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragOutlineMask
 
            v2fOutline vert (appdata_base v)
            {
                return vertOutline(v, float2( 1, 1));
            }
            ENDCG
        }
 
        Pass
        {
            Name "OUTLINEALPHA"
            BlendOp Min
            ColorMask A
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragOutlineMask
 
            v2fOutline vert (appdata_base v)
            {
                return vertOutline(v, float2(-1, 1));
            }
            ENDCG
        }
 
        Pass
        {
            Name "OUTLINEALPHA"
            BlendOp Min
            ColorMask A
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragOutlineMask
 
            v2fOutline vert (appdata_base v)
            {
                return vertOutline(v, float2( 1,-1));
            }
            ENDCG
        }
 
        Pass
        {
            Name "OUTLINEALPHA"
            BlendOp Min
            ColorMask A
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragOutlineMask
 
            v2fOutline vert (appdata_base v)
            {
                return vertOutline(v, float2(-1,-1));
            }
            ENDCG
        }
 
        Pass
{
    Name "CENTERMASK"
    ColorMask RGB
    Blend SrcAlpha OneMinusSrcAlpha
    ZWrite On
    Offset 5,5
    CGPROGRAM
    #pragma vertex vert
    #pragma exclude_renderers gles xbox360 ps3
    #pragma exclude_renderers gles xbox360 ps3
    #pragma exclude_renderers gles xbox360 ps3
    #pragma fragment frag

    #include "UnityCG.cginc"

    struct v2f
    {
        float4 pos : POSITION;
        float2 uv : TEXCOORD0;
    };

    v2f vert(appdata_full v)
    {
        v2f o;
        o.pos = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
        return o;
    }

    fixed4 frag(v2f i) : COLOR
    {
        return fixed4(1, 1, 1, 1); // White color
    }
    ENDCG
}
 
        Pass
        {
            Name "OUTLINECOLOR"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragOutline
 
            v2fOutline vert (appdata_base v)
            {
                return vertOutline(v, float2( 1, 1));
            }
            ENDCG
        }
 
        Pass
        {
            Name "OUTLINECOLOR"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragOutline
 
            v2fOutline vert (appdata_base v)
            {
                return vertOutline(v, float2(-1, 1));
            }
            ENDCG
        }
 
        Pass
        {
            Name "OUTLINECOLOR"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragOutline
 
            v2fOutline vert (appdata_base v)
            {
                return vertOutline(v, float2( 1,-1));
            }
            ENDCG
        }
 
        Pass
        {
            Name "OUTLINECOLOR"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragOutline
 
            v2fOutline vert (appdata_base v)
            {
                return vertOutline(v, float2(-1,-1));
            }
            ENDCG
        }

         Pass
        {
            Name "OUTLINECOLOR"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment fragOutlineMask
 
            v2fOutline vert (appdata_base v)
            {
                return vertOutline(v, float2(0,0));
            }
            ENDCG
        }
    }
}