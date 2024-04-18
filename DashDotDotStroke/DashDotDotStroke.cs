
using System.Windows.Media;
using MyStroke;
namespace DashDotDotStroke
{
    public class DashDotDotStroke : Stroke
    {
        public DashDotDotStroke(SolidColorBrush borderColor, double thickness, SolidColorBrush fillColor) : base(borderColor, thickness, fillColor)
        {
            strokeDashArray = new DoubleCollection() { 4, 1, 1, 1, 1, 4 };

        }
        //Create copy contructor
        public DashDotDotStroke(DashDotDotStroke stroke) : base(stroke)
        {

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
}
