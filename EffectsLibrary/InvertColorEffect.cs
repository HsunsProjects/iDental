using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace EffectsLibrary
{
    public class InvertColorEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(InvertColorEffect), 0);
        public InvertColorEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(@"pack://application:,,,/EffectsLibrary;component/InvertColorEffect.ps");
            this.PixelShader = pixelShader;

            this.UpdateShaderValue(InputProperty);
        }
        public Brush Input
        {
            get
            {
                return ((Brush)(this.GetValue(InputProperty)));
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }
    }
}
