using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shapes;

namespace Simple_Paint
{
    class Memento
    {
        public List<ShapeToDraw> shapeList;

        public Memento(List<ShapeToDraw> shapes)
        {
            shapeList = new List<ShapeToDraw>();
            foreach (ShapeToDraw shape in shapes)
            {
                shapeList.Add(shape.Clone());
            }
        }

        public void restore()
        {
            foreach(ShapeToDraw shape in shapeList)
            {
                shape.Draw();
            }
        }
    }
}
