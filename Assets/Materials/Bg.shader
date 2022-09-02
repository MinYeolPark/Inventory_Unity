Shader "Unlit/Bg"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TintColor("Tint Color", Color) = (1,1,1,1)
        //_Transparency("Transparency", Range(0.0, 1)) = 0.5
        _DotSize("DotSize", Range(0.0, 15.0)) = 0

        _BoundColor("Bound Color", Color) = (0,0.5843137254901961,1,1)
        _BgColor("Background Color", Color) = (0.1176470588235294,0,0.5882352941176471,1)
        _circleSizePercent("Circle Size Percent", Range(0, 100)) = 50
        _border("Anti Alias Border Threshold", Range(0.00001, 5)) = 0.01
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent"}
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
            };
            v2f vert(
                float4 vertex : POSITION, // vertex position input
                float2 uv : TEXCOORD0, // texture coordinate input
                out float4 outpos : SV_POSITION // clip space position output
            )
            {
                v2f o;
                o.uv = uv;
                outpos = UnityObjectToClipPos(vertex);
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;// x(=1/w), y(1/h), z(wid), w(hei)
            float4 _MainTex_ST;
            float4 _TintColor;
            //float _Transparency;
            float _DotSize;



            float _border;
            fixed4 _BoundColor;
            fixed4 _BgColor;
            float _circleSizePercent;
            float2 antialias(float radius, float borderSize, float dist)
            {
                float t = smoothstep(radius + borderSize, radius - borderSize, dist);
                return t;
            }


            fixed4 frag (v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
            {
#if false                
                //iResolution = _MainTex_TexelSize.zw;
                float2 fragCoord = i.uv * _MainTex_TexelSize.zw;
                float aspect = _MainTex_TexelSize.z / _MainTex_TexelSize.w;
                fragCoord.x *= aspect;
                float2 center = _MainTex_TexelSize.zw / 2;
                float d = 1 - length(fragCoord - center);
                return float4 (_TintColor.rgb, _TintColor.a * clamp(1- (_DotSize / 2.) - d, 0., 1.));
#endif          

                float4 col;
                float2 center = _ScreenParams.xy / 2;

                float maxradius = length(center);

                float radius = maxradius * (_circleSizePercent / 100);

                float dis = distance(screenPos.xy, center);

                if (dis > radius) {
                    float aliasVal = antialias(radius, _border, dis);
                    //col = lerp(_BoundColor, _BgColor, aliasVal); //NOT needed but incluse just incase
                }
                else {
                    float aliasVal = antialias(radius, _border, dis);
                    col = lerp(_BoundColor, _BgColor, aliasVal);
                }
                return col;
            }
            ENDCG
        }
    }
}
