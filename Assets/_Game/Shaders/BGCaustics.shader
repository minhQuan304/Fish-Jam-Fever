// Anh sang to ong tren cat: 2 lop caustic truot nguoc chieu, chi hien trong vung mask (sprite = anh mask), hoa tron screen.
Shader "FishJam/BG Caustics"
{
    Properties
    {
        [PerRendererData] _MainTex ("Mask (alpha)", 2D) = "white" {}
        _CausticTex ("Caustic (alpha)", 2D) = "white" {}
        _Color ("Mau", Color) = (1, 0.96, 0.8, 1)
        _Intensity ("Do sang", Range(0, 2)) = 0.6
        _Tile1 ("Lap lop 1 (xy)", Vector) = (5, 5, 0, 0)
        _Tile2 ("Lap lop 2 (xy)", Vector) = (3.5, 3.5, 0, 0)
        _Speed1 ("Toc do lop 1 (xy)", Vector) = (0.03, 0.02, 0, 0)
        _Speed2 ("Toc do lop 2 (xy)", Vector) = (-0.025, 0.015, 0, 0)
        _Wobble ("Gon song", Range(0, 0.05)) = 0.012
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "PreviewType"="Plane" }
        Cull Off ZWrite Off Lighting Off
        Blend OneMinusDstColor One
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex, _CausticTex;
            fixed4 _Color; float _Intensity, _Wobble; float4 _Tile1, _Tile2, _Speed1, _Speed2;
            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; fixed4 color : COLOR; };
            struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; fixed4 color : COLOR; };
            v2f vert (appdata v) { v2f o; o.pos = UnityObjectToClipPos(v.vertex); o.uv = v.uv; o.color = v.color; return o; }
            fixed4 frag (v2f i) : SV_Target
            {
                float t = _Time.y;
                float2 w = float2(sin(i.uv.y * 23 + t * 1.3), cos(i.uv.x * 19 + t * 1.1)) * _Wobble;
                float a = tex2D(_CausticTex, i.uv * _Tile1.xy + _Speed1.xy * t + w).a;
                float b = tex2D(_CausticTex, i.uv * _Tile2.xy + _Speed2.xy * t - w).a;
                float c = saturate(a * 0.75 + b * 0.55 + a * b * 0.8);
                float m = tex2D(_MainTex, i.uv).a * i.color.a;
                return fixed4(_Color.rgb * c * m * _Intensity, 1);
            }
            ENDCG
        }
    }
}
