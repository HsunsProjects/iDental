/// <class>GrayScaleEffect</class>
/// <description>An effect that turns the input into gray scale shades.</description>

sampler2D  inputSampler : register(S0);


float4 main(float2 uv : TEXCOORD) : COLOR
{
   float4 srcColor = tex2D(inputSampler, uv);
   float3 rgb = srcColor.rgb;
   // use the ICC grayscale ratios
   float3 luminance1 = dot(rgb, float3(0.222, 0.707, 0.071));

   return float4(luminance1, srcColor.a);
}