using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MyTriangle;
using Shapes;
namespace MyArrow
{
    [Serializable]
    public class ArrowShape : ShapeToDraw
    {
        public RectangleShape rectangle;
        public TriangleShape triangle;
        public LineShape line;
        public ArrowShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
            rectangle = new RectangleShape(startPoint, endPoint);
            triangle = new TriangleShape(startPoint, endPoint);
            line = new LineShape(new Point(0, 0), new Point(0, 0));

        }

        public ArrowShape(ArrowShape arrowShape) : base(arrowShape)
        {
            this.rectangle = (RectangleShape?)arrowShape.rectangle.Clone();
            this.triangle = (TriangleShape?)arrowShape.triangle.Clone();
            this.line = (LineShape?)arrowShape.line.Clone();
            //UpdateStartAndEndPoint();
        }


        public override string GetShapeType()
        {
            return "Arrow";
        }
        public override ShapeToDraw Clone()
        {
            return new ArrowShape(this);
        }
        public override void Remove()
        {
            base.Remove();
            rectangle.Remove();
            triangle.Remove();
            line.Remove();
        }
        public override void Draw()
        {

            rectangle.stroke = this.stroke;
            triangle.stroke = this.stroke;
            line.stroke = this.stroke;

            UpdateStartAndEndPoint();

            rectangle.Draw();
            triangle.Draw();
            line.Draw();

            base.Draw();
        }

        public override void UpdateEndPoint()
        {


            rectangle.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);
            rectangle.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);
            rectangle.UpdateEndPoint();
            rectangle.UpdateStartPoint();
            triangle.StartPoint = new Point(EndPoint.X, StartPoint.Y);
            triangle.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, EndPoint.Y);
            triangle.UpdateEndPointLandscapeOrientation();

            line.StartPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);
            line.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);
            line.UpdateEndPoint();
            line.UpdateStartPoint();
        }
        //UpdateStartAndEndPoint
        public override void UpdateStartAndEndPoint()
        {

            /*rectangle.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);
            rectangle.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);
            rectangle.isDragging = this.isDragging;
            rectangle.dragStartPoint = this.dragStartPoint;
            rectangle.curDragPoint = this.curDragPoint;
            rectangle.Drag(curDragPoint)*/
            base.UpdateStartAndEndPoint();

            rectangle.stroke = this.stroke;
            triangle.stroke = this.stroke;
            line.stroke = this.stroke;

            rectangle.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);
            rectangle.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);
            rectangle.UpdateEndPoint();
            rectangle.UpdateStartPoint();
            triangle.StartPoint = new Point(EndPoint.X, StartPoint.Y);
            triangle.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, EndPoint.Y);
            triangle.UpdateEndPointLandscapeOrientation();

            line.StartPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);
            line.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);
            line.UpdateEndPoint();
            line.UpdateStartPoint();

            rectangle.UpdateStartAndEndPoint();
            line.UpdateStartAndEndPoint();

            base.UpdateStartAndEndPoint();

        }
    }


}
