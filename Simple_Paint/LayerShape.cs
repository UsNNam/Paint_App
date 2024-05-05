using Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Simple_Paint
{
    public class LayerShape
    {
        public List<ShapeToDraw> shapeToDraws;
        public Caretaker caretaker { get; set; }
        public LayerShape()
        {
            shapeToDraws = new List<ShapeToDraw>();
            caretaker = new Caretaker();
        }

        public void Draw()
        {
            foreach(ShapeToDraw shape in shapeToDraws)
            {
                shape.Draw();
                if(shape.textBoxState == true)
                {
                    shape.attachTextBox(shape.textBoxForeground, shape.textBoxBackground, (int)shape.textBoxFontSize, shape.textBoxFontFamily);
                    shape.textBox.Text = shape.textBoxText;
                }
                shape.UpdateStartAndEndPoint();

            }
        }

        public void Remove()
        {
            shapeToDraws.ForEach(shape => shape.Remove());
        }
    }
}
