using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using static Xamarin.Forms.Internals.GIFBitmap;

namespace EcoTrack
{
    public class CircularProgressBar : SKCanvasView
    {
        public static readonly BindableProperty ProgressProperty =
            BindableProperty.Create(nameof(Progress), typeof(double), typeof(CircularProgressBar), 0.0);

        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            var canvas = e.Surface.Canvas;
            canvas.Clear();

            float width = e.Info.Width;
            float height = e.Info.Height;
            float lineWidth = 20;  // Adjust line width if needed
            float radius = Math.Min(width, height) / 2 - lineWidth / 2;
            float centerX = width / 2;
            float centerY = height / 2;

            using (var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = lineWidth,
                IsAntialias = true
            })
            {
                // Draw background circle
                paint.Color = SKColors.LightGray;
                canvas.DrawCircle(centerX, centerY, radius, paint);

                // Draw progress arc
                paint.Color = SKColors.Green;
                using (var path = new SKPath())
                {
                    path.AddArc(new SKRect(centerX - radius, centerY - radius, centerX + radius, centerY + radius), -90, 360 * (float)Progress);
                    canvas.DrawPath(path, paint);
                }
            }
        }
    }
}
