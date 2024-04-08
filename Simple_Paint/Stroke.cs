using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Simple_Paint
{
    public abstract class Stroke
    {
        public SolidColorBrush borderColor { get; set; }
        public double thickness { get; set; }
        public DoubleCollection strokeDashArray;
        public SolidColorBrush fillColor { get; set; }

        public Stroke(SolidColorBrush borderColor, double thickness, SolidColorBrush fillColor)
        {
            this.borderColor = borderColor;
            this.thickness = thickness;
            this.fillColor = fillColor;

        }

        public Stroke()
        {
            this.borderColor = new SolidColorBrush(Colors.Black);
            this.thickness = 1;
        }

        public Stroke(SolidColorBrush borderColor)
        {
            this.borderColor = borderColor;
            this.thickness = 1;
        }
        public Stroke(double thickness)
        {
            this.borderColor = new SolidColorBrush(Colors.Black);
            this.thickness = thickness;
        }

        public virtual void SetStroke()
        {
            // Set the stroke thickness and color
        }
    }
    public class DashStroke : Stroke
    {
        public DashStroke(SolidColorBrush borderColor, double thickness, SolidColorBrush fillColor) : base(borderColor, thickness,fillColor)
        {
            strokeDashArray = new DoubleCollection() { 4, 2 };
        }
        public override void SetStroke()
        {
            // Set the stroke thickness and color for dash stroke
        }
    }
    public class DotStroke : Stroke
    {
        public DotStroke(SolidColorBrush borderColor, double thickness, SolidColorBrush fillColor) : base(borderColor, thickness, fillColor)
        {
            strokeDashArray = new DoubleCollection() { 1, 2 };
        }


        public override void SetStroke()
        {
            // Set the stroke thickness and color for dot stroke
        }
    }
    public class DashDotDotStroke : Stroke
    {
        public DashDotDotStroke(SolidColorBrush borderColor, double thickness, SolidColorBrush fillColor) : base(borderColor, thickness, fillColor)
        {
            strokeDashArray = new DoubleCollection() { 4, 1, 1, 1, 1, 4 };

        }
        public override void SetStroke()
        {
            // Set the stroke thickness and color for solid stroke
        }
    }
    public class SolidStroke : Stroke
    {
        public SolidStroke(SolidColorBrush borderColor, double thickness, SolidColorBrush fillColor) : base(borderColor, thickness, fillColor)
        {
            strokeDashArray = null;

        }

        public override void SetStroke()
        {
            // Set the stroke thickness and color for solid stroke
        }
    }
}

