Shader "Custom/MinimapUnlitOutline"

{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.2, 0.6, 1, 1)
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth ("Outline Width", Range(0.001, 0.1)) = 0.02
        _MinHeight ("Min Height", Float) = 0.0
        _MaxHeight ("Max Height", Float) = 20.0
    }

    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Opaque" }

        // ===== Pass 1: Outline Pass =====
        Pass
        {
            Name "OUTLINE"
            Cull Front
            ZWrite On
            ColorMask RGB
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _OutlineWidth;
            fixed4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 worldNormal = normalize(v.normal);
                float3 offset = worldNormal * _OutlineWidth;
                o.pos = UnityObjectToClipPos(v.vertex + float4(offset, 0));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }

        // ===== Pass 2: Fill Pass with Height-based Color =====
        Pass
        {
            Name "FILL"
            Cull Back
            ZWrite On
            ColorMask RGB
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _BaseColor;
            float _MinHeight;
            float _MaxHeight;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float height : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.height = v.vertex.y;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float h = saturate((i.height - _MinHeight) / (_MaxHeight - _MinHeight));
                return _BaseColor * (0.5 + h * 0.5); // ‘Ω∏ﬂ‘Ω¡¡
            }
            ENDCG
        }
    }

    FallBack "Unlit/Color"
}
