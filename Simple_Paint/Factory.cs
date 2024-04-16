using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
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
    public class FactoryWord
    {
        public static TextBox CreateTextBox(SolidColorBrush borderColor, int fontSize, SolidColorBrush fillColor, string fontFamily)
        {
            TextBox textBox = new TextBox();
            textBox.Text = "Center Text";
            textBox.Width = 80;
            textBox.Height = 20;
            textBox.BorderBrush = borderColor;
            //textBox.Foreground = fillColor;
            textBox.FontFamily = new FontFamily(fontFamily);
            textBox.FontSize = fontSize;
            textBox.Background = borderColor;
            Style textBoxStyle = new Style(typeof(TextBox));

            textBoxStyle.Setters.Add(new Setter(TextBox.ForegroundProperty, fillColor));


            textBox.Style = textBoxStyle;

            textBox.HorizontalAlignment = HorizontalAlignment.Center;
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.TextAlignment = TextAlignment.Center;
            return textBox;
        }
    }
}
