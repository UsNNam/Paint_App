using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
namespace Simple_Paint
{
    public class FactoryShape
    {

        public static ShapeToDraw CreateShape(string shapeType,string strokeType, SolidColorBrush borderColor, double thickness, SolidColorBrush fillColor)
        {
            ShapeToDraw curShape = ShapeToDraw.GetPrototype(shapeType);
            Stroke stroke = Stroke.GetPrototype(strokeType);
            stroke.borderColor = borderColor;
            stroke.thickness = thickness;
            stroke.fillColor = fillColor;
            curShape.stroke = stroke;
            return curShape;
         
        }   
    }
}
