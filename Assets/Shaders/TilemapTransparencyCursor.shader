Shader "Custom/TilemapTransparencyCursor"
{
    Properties
    {
        _MainTex ("Tilemap Texture", 2D) = "white" {}
        _CursorPosition ("Cursor Position", Vector) = (0, 0, 0, 0)
        _Radius ("Transparency Radius", Float) = 5.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _CursorPosition; // World-space cursor position
            float _Radius; // Radius for transparency

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // Get world position
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Calculate the distance between the current tile's world position and the cursor
                float distance = length(i.worldPos.xy - _CursorPosition.xy); // Ignore Z axis for 2D

                // Apply transparency if the tile is within the radius
                if (distance < _Radius)
                {
                    col.a = col.a * 0.5; // Adjust transparency (can be changed)
                }

                return col;
            }
            ENDCG
        }
    }
}
