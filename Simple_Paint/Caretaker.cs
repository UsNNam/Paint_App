using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shapes;
namespace Simple_Paint
{
    public class Caretaker
    {
        private int index = -1;
        private List<Memento> history1 = new List<Memento>();

        public Caretaker()
        {
            
        }

        public void add(Memento memento)
        {
            if(index >= 0 && index < history1.Count - 1)
            {
                history1 = history1.GetRange(0, index + 1);
            }
            history1.Add(memento);
            index = history1.Count - 1;
            if(history1.Count >= 2)
            {
                Debug.WriteLine("Count: " + history1[1].shapeList.Count);
            }
            
        }

        public void undo()
        {
            if (history1.Count >= 2)
            {
                Debug.WriteLine("Count1: " + history1[1].shapeList.Count);
            }
            foreach (ShapeToDraw shape in MainWindow.history)
            {
                shape.Remove();
            }
            if(index > 0)
            {
                index--;
            }

            if (history1.Count >= 2)
            {
                Debug.WriteLine("Count2: " + history1[1].shapeList.Count);
            }
            history1[index].restore();
            MainWindow.history.Clear();
            for (int i = 0; i < history1[index].shapeList.Count; i++)
            {
                MainWindow.history.Add(history1[index].shapeList[i]);
            }
            
            if(history1.Count >= 2)
            {
                Debug.WriteLine("Count: " + history1[1].shapeList.Count);
            }
            
            
        }
        public void redo()
        {
            if(MainWindow.history.Count > 0)
            {
                foreach (ShapeToDraw shape in MainWindow.history)
                {
                    shape.Remove();
                }
            }
            
            if (index < history1.Count - 1)
            {
                index++;
            }

            history1[index].restore();
            MainWindow.history.Clear();
            for (int i = 0; i < history1[index].shapeList.Count; i++)
            {
                MainWindow.history.Add(history1[index].shapeList[i]);
            }

        }
    }
}
