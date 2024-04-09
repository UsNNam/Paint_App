using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Simple_Paint
{
    public class Stroke
    {
        public static List<Stroke> prototypes = new List<Stroke>();
        static Stroke()
        {
            prototypes.Add(new DashStroke(new SolidColorBrush(Colors.Black), 1, new SolidColorBrush(Colors.White)));
            prototypes.Add(new DotStroke(new SolidColorBrush(Colors.Black), 1, new SolidColorBrush(Colors.White)));
            prototypes.Add(new DashDotDotStroke(new SolidColorBrush(Colors.Black), 1, new SolidColorBrush(Colors.White)));
            prototypes.Add(new SolidStroke(new SolidColorBrush(Colors.Black), 1, new SolidColorBrush(Colors.White)));
        }
        public static Stroke GetPrototype(string type)
        {
            foreach (Stroke prototype in prototypes)
            {
                string test = prototype.GetStrokeType();
                if (prototype.GetStrokeType() == type)
                {
                    return prototype.Clone();
                }
            }
            return null;
        }



        public SolidColorBrush borderColor { get; set; }
        public double thickness { get; set; }
        public DoubleCollection strokeDashArray;
        public SolidColorBrush fillColor { get; set; }

        //Create copy contructor 
        public Stroke(Stroke stroke)
        {
            this.borderColor = stroke.borderColor;
            this.thickness = stroke.thickness;
            this.fillColor = stroke.fillColor;
        }

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
        virtual public string GetStrokeType()
        {
            return "Stroke";
        }
        virtual public Stroke Clone()
        {
            return null;
        }
    }
    public class DashStroke : Stroke
    {
        public DashStroke(SolidColorBrush borderColor, double thickness, SolidColorBrush fillColor) : base(borderColor, thickness,fillColor)
        {
            strokeDashArray = new DoubleCollection() { 4, 2 };
        }
        //Create copy contructor
        public DashStroke(DashStroke stroke) : base(stroke)
        {
            this.strokeDashArray = stroke.strokeDashArray;
        }
        public override void SetStroke()
        {
            // Set the stroke thickness and color for dash stroke
        }
        public override string GetStrokeType()
        {
            return "Dash";
        }
        public override Stroke Clone()
        {
            return new DashStroke(this);
        }
    }
    public class DotStroke : Stroke
    {
        public DotStroke(SolidColorBrush borderColor, double thickness, SolidColorBrush fillColor) : base(borderColor, thickness, fillColor)
        {
            strokeDashArray = new DoubleCollection() { 1, 2 };
        }
        //Create copy contructor
        public DotStroke(DotStroke stroke) : base(stroke)
        {
            this.strokeDashArray = stroke.strokeDashArray;
        }
        public override string GetStrokeType()
        {
            return "Dot";
        }
        public override Stroke Clone()
        {
            return new DotStroke(this);
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
        //Create copy contructor
        public DashDotDotStroke(DashDotDotStroke stroke) : base(stroke)
        {
            this.strokeDashArray = stroke.strokeDashArray;
        }
        public override void SetStroke()
        {
            // Set the stroke thickness and color for solid stroke
        }
        public override string GetStrokeType()
        {
            return "DashDotDot";
        }
        public override Stroke Clone()
        {
            return new DashDotDotStroke(this);
        }
    }
    public class SolidStroke : Stroke
    {
        public SolidStroke(SolidColorBrush borderColor, double thickness, SolidColorBrush fillColor) : base(borderColor, thickness, fillColor)
        {
            strokeDashArray = null;

        }
        //Create copy contructor
        public SolidStroke(SolidStroke stroke) : base(stroke)
        {
            this.strokeDashArray = stroke.strokeDashArray;
        }

        public override void SetStroke()
        {
            // Set the stroke thickness and color for solid stroke
        }
        public override string GetStrokeType()
        {
            return "Solid";
        }
        public override Stroke Clone()
        {
            return new SolidStroke(this);
        }
    }
}

