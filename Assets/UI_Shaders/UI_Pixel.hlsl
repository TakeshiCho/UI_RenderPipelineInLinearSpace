﻿#ifndef UI_PIXEL_INCLUDE
#define UI_PIXEL_INCLUDE

#ifndef INITIALIZE_UI_IMAGE
#define INITIALIZE_UI_IMAGE InitializeUIImage
void InitializeUIImage(v2f IN,inout float4 color)
{
    color = IN.color * (SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,IN.texcoord) + _TextureSampleAdd);
}
#endif

float4 pixel(v2f IN) : SV_Target
{
    //Round up the alpha color coming from the interpolator (to 1.0/256.0 steps)
    //The incoming alpha could have numerical instability, which makes it very sensible to
    //HDR color transparency blend, when it blends with the world's texture.
    const half alphaPrecision = half(0xff);
    const half invAlphaPrecision = half(1.0/alphaPrecision);
    IN.color.a = round(IN.color.a * alphaPrecision)*invAlphaPrecision;

    half4 color;
    INITIALIZE_UI_IMAGE(IN,color);

    #ifdef UNITY_UI_CLIP_RECT
    half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
    color.a *= m.x * m.y;
    #endif

    #ifdef UNITY_UI_ALPHACLIP
    clip (color.a - 0.001);
    #endif

    color.rgb = LinearToSRGB(color.rgb);
    color.rgb *= color.a;

    return color;
}
#endif