using System.Windows.Media;
using MyStroke;
namespace DotStroke
{
    public class DotStroke : Stroke
    {
        public DotStroke(SolidColorBrush borderColor, double thickness, SolidColorBrush fillColor) : base(borderColor, thickness, fillColor)
        {
            strokeDashArray = new DoubleCollection() { 1, 2 };
        }
        //Create copy contructor
        public DotStroke(DotStroke stroke) : base(stroke)
        {

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

}
