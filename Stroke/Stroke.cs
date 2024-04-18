
using System.Windows.Media;

namespace MyStroke
{
    public class Stroke
    {
        public static List<Stroke> prototypes = new List<Stroke>();
        static Stroke()
        {
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
            this.borderColor = new SolidColorBrush(stroke.borderColor.Color);
            this.thickness = stroke.thickness;
            if (stroke.fillColor != null)
            {
                this.fillColor = new SolidColorBrush(stroke.fillColor.Color);
            }
            if (stroke.strokeDashArray != null)
            {
                // Create a new DoubleCollection and copy the items
                this.strokeDashArray = new DoubleCollection();
                foreach (var dash in stroke.strokeDashArray)
                {
                    this.strokeDashArray.Add(dash);
                }
            }
            else
            {
                this.strokeDashArray = null;
            }
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

}
