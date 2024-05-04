using Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            shapeToDraws.ForEach(shape => shape.Draw());
        }

        public void Remove()
        {
            shapeToDraws.ForEach(shape => shape.Remove());
        }
    }
}
