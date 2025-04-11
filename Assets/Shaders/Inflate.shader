Shader "Sprites/InflateSprite"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _InflateAmount ("Inflate Amount", Range(-1, 1)) = 0
        _InflateCenter ("Inflate Center", Vector) = (0.5, 0.5, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
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
            float _InflateAmount;
            float2 _InflateCenter;

            v2f vert (appdata v)
            {
                v2f o;
                
                // Смещение вершин от центра
                float2 dir = v.uv - _InflateCenter;
                float2 offset = dir * _InflateAmount;
                
                // Применяем смещение к позиции вершины
                float4 pos = v.vertex;
                pos.xy += offset * length(dir) * 2.0; // Усиливаем эффект ближе к краям
                
                o.vertex = UnityObjectToClipPos(pos);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}