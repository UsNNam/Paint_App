using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using MyStroke;
namespace Simple_Paint
{
    public class DashStroke : Stroke
    {
        public DashStroke(SolidColorBrush borderColor, double thickness, SolidColorBrush fillColor) : base(borderColor, thickness,fillColor)
        {
            strokeDashArray = new DoubleCollection() { 4, 2 };
        }
        //Create copy contructor
        public DashStroke(DashStroke stroke) : base(stroke)
        {

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
   
    public class SolidStroke : Stroke
    {
        public SolidStroke(SolidColorBrush borderColor, double thickness, SolidColorBrush fillColor) : base(borderColor, thickness, fillColor)
        {
            strokeDashArray = null;

        }
        //Create copy contructor
        public SolidStroke(SolidStroke stroke) : base(stroke)
        {

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
