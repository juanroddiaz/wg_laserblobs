
Shader "Custom/OverlayBlend"
{
    Properties {
        _MainTex ("Texture", any) = "" {}
        _Color ("Blend Color", Color) = (0.2, 0.3, 1 ,1)
    }
 
    SubShader {
 
        Tags { "ForceSupported" = "True" "RenderType"="Overlay" }
       
        Lighting Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        Fog { Mode Off }
        ZTest Always
       
        Pass { 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
 
            #include "UnityCG.cginc"
 
            struct appdata_t {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };
 
            struct v2f {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };
 
            sampler2D _MainTex;
 
            uniform float4 _MainTex_ST;
            uniform float4 _Color;
           
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                return o;
            }
 
            fixed4 frag (v2f i) : COLOR
            {
                // Get the raw texture value
                float4 texColor = tex2D(_MainTex, i.texcoord);
                // Calculate the luminance of the texture color
                float luminance =  dot(texColor, fixed4(0.2126, 0.7152, 0.0722, 0));
                // Declare the output structure
                fixed4 output = 0;
               
                // The actual Overlay/High Light method is based on the shader
                if (luminance < 0.5) {
                    output = 2.0 * texColor * _Color;
                } else {
                    // We need a white base for a lot of the algorith,
                    fixed4 white = fixed4(1,1,1,1);
                    output = white - 2.0 * (white - texColor) * (white - _Color);
                }
               
                // The alpha can actually just be a simple blend of the two- makes things nicely controllable in both
                // texture and color
                output.a  = texColor.a * _Color.a;
                return output;
            }
            ENDCG
        }
    }  
 
   
    SubShader {
 
        Tags { "ForceSupported" = "True" "RenderType"="Overlay" }
 
        Lighting Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        Fog { Mode Off }
        ZTest Always
       
        BindChannels {
            Bind "vertex", vertex
            Bind "color", color
            Bind "TexCoord", texcoord
        }
       
        Pass {
            SetTexture [_MainTex] {
                combine primary * texture DOUBLE, primary * texture DOUBLE
            }
        }
    }
 
    Fallback off
}
 