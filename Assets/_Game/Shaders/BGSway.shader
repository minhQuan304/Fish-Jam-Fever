// Nen co cay "song": lam gon lac ngang dung vung cay tren anh goc theo mat na (R = muc lac, 0 o goc -> 1 o ngon).
// Khong can cat cay ra lop rieng.
Shader "FishJam/BG Sway"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite", 2D) = "white" {}
        _SwayMask ("Mat na lac (R)", 2D) = "black" {}
        _Amp ("Bien do (uv)", Range(0, 0.02)) = 0.006
        _Speed ("Toc do", Range(0, 5)) = 1.3
        _Freq ("Song doc theo than", Range(0, 40)) = 9
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" "PreviewType"="Plane" }
        Cull Off ZWrite Off Lighting Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex, _SwayMask; float _Amp, _Speed, _Freq;
            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; fixed4 color : COLOR; };
            struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; fixed4 color : COLOR; };
            v2f vert (appdata v) { v2f o; o.pos = UnityObjectToClipPos(v.vertex); o.uv = v.uv; o.color = v.color; return o; }
            fixed4 frag (v2f i) : SV_Target
            {
                float m = tex2D(_SwayMask, i.uv).r;
                float t = _Time.y * _Speed;
                float2 uv = i.uv;
                uv.x += (sin(t + uv.y * _Freq + uv.x * 13.0) * 0.7 + sin(t * 0.53 + uv.x * 31.0) * 0.3) * _Amp * m;
                return tex2D(_MainTex, uv) * i.color;
            }
            ENDCG
        }
    }
}
