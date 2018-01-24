Shader "Custom/LevelShader" {

    Properties {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        [MaterialToggle] _Shadow("Cast Shadow", int) = 0
    }
    SubShader {
        Pass {

            Tags {"LightMode" = "ForwardBase"}
            
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            #pragma target 4.5
            
            struct Level
            {
                int index;
                int step;
                int number; 
            };
            
            int _MidStep;
            
            sampler2D _MainTex;
            float _Length;
            int _Shadow;

        #if SHADER_TARGET >= 45
            StructuredBuffer<Level> _LevelBuffer;
        #endif

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
                SHADOW_COORDS(2)
                fixed3 diff : COLOR0;
                fixed3 ambient : COLOR1;
                fixed3 color : COLOR2;
            };
            
            //アフィン変換の行列を作成する
            float4x4 transform(float3 scale, float3 position)
            {
                float4x4 mat = 0;
                
                //scaleを適応
                mat._11_22_33_44 = float4(scale, 1);
                               
                //positionの適応
                mat._14_24_34 = position;  
                
                return mat;
            }

            v2f vert (appdata_full v, uint instanceID : SV_InstanceID)
            {
            #if SHADER_TARGET >= 45
                Level b = _LevelBuffer[instanceID];
                int id = b.index;
                int sp = b.step;
                int num = b.number;
            #else
                int id = 0;
                int sp = 0;
                int num = 0;
            #endif
                
                float4x4 object2world = transform(
                    float3(1, num + 0.1, 1) / _Length * 2,
                    float3(id, num/2.0, sp - _MidStep) / _Length * 2
                );
                
                v2f o;
                o.pos = UnityObjectToClipPos(mul(object2world, v.vertex));
                o.normal = normalize(mul(object2world, v.normal));
                o.uv = v.texcoord;
                o.color = fixed3(1, (float) num / _Length, (float) num / _Length * 0.5);
                
                if(_Shadow > 0)
                {
                    half nl = max(0, dot(o.normal, _WorldSpaceLightPos0.xyz));
                    o.diff = nl * _LightColor0.rgb;
                    o.ambient = ShadeSH9(half4(o.normal.xyz, 1));
                }
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {   
                fixed4 col = tex2D(_MainTex, i.uv) * fixed4(i.color,0);
                if(_Shadow > 0)
                {
                    fixed shadow = SHADOW_ATTENUATION(i);
                    fixed3 lighting = i.diff * shadow + i.ambient;
                    col.rgb *= lighting;
                }
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }

            ENDCG
        }
        
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }

}
