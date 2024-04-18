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
namespace CollateShape
{
    [Serializable]
    class CollateShape : ShapeToDraw
    {
        TriangleShape triangle1, triangle2;
        public CollateShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
            triangle1 = new TriangleShape(StartPoint, EndPoint);
            triangle2 = new TriangleShape(StartPoint, EndPoint);

        }

        public CollateShape(CollateShape collateShape) : base(collateShape)
        {
            this.triangle1 = (TriangleShape?)collateShape.triangle1.Clone();
            this.triangle2 = (TriangleShape?)collateShape.triangle2.Clone();
        }

        public override void Draw()
        {
            UpdateStartAndEndPoint();
            triangle1.StartPoint = StartPoint;
            triangle1.EndPoint = EndPoint;

            triangle2.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            triangle2.EndPoint = new Point(EndPoint.X, EndPoint.Y);


            triangle1.stroke = this.stroke;
            triangle2.stroke = this.stroke;

            triangle1.Draw();
            triangle2.Draw();

            base.Draw();
        }
        public override string GetShapeType()
        {
            return "Collate";
        }
        public override ShapeToDraw Clone()
        {
            return new CollateShape(this);
        }
        public override void UpdateEndPoint()
        {
            triangle1.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            triangle1.EndPoint = new Point(EndPoint.X, StartPoint.Y);
            triangle1.UpdateEndPoint();
            triangle2.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            triangle2.EndPoint = EndPoint;
            triangle2.UpdateEndPoint();

        }
        public override void Remove()
        {
            base.Remove();
            triangle1.Remove();
            triangle2.Remove();
        }
        //UpdateStartAndEndPoint
        public override void UpdateStartAndEndPoint()
        {
            base.UpdateStartAndEndPoint();

            triangle1.stroke = this.stroke;
            triangle2.stroke = this.stroke;

            triangle1.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            triangle1.EndPoint = new Point(EndPoint.X, StartPoint.Y);
            triangle1.UpdateStartAndEndPoint();
            triangle2.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            triangle2.EndPoint = EndPoint;
            triangle2.UpdateStartAndEndPoint();

            base.UpdateStartAndEndPoint();
        }
    }


}
