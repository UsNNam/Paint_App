
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
namespace StarShape
{
    [Serializable]
    class StarShape : ShapeToDraw
    {
        TriangleShape triangle1, triangle2;
        LineShape line1, line2, line3;
        public StarShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
            Point newEndPoint1 = new Point(endPoint.X, Math.Max(startPoint.Y + (endPoint.Y - startPoint.Y) * 3 / 4, 0));
            Point newStartPoint = new Point(startPoint.X, Math.Max(startPoint.Y + (endPoint.Y - startPoint.Y) * 4 / 3, 0));
            Point newEndPoint = new Point(endPoint.X, startPoint.Y);

            triangle1 = new TriangleShape(new Point(0, 0), new Point(0, 0));
            triangle2 = new TriangleShape(new Point(0, 0), new Point(0, 0));

            line1 = new LineShape(new Point(0, 0), new Point(0, 0));
            line2 = new LineShape(new Point(0, 0), new Point(0, 0));
            line3 = new LineShape(new Point(0, 0), new Point(0, 0));
        }
        public StarShape(StarShape starShape) : base(starShape)
        {
            this.triangle1 = (TriangleShape?)starShape.triangle1.Clone();

            this.triangle2 = (TriangleShape?)starShape.triangle2.Clone();
            triangle1.StartPoint = starShape.StartPoint;
            triangle1.EndPoint = starShape.StartPoint;
            triangle2.StartPoint = starShape.StartPoint;
            triangle2.EndPoint = starShape.StartPoint;
            this.line1 = (LineShape?)starShape.line1.Clone();
            this.line2 = (LineShape?)starShape.line2.Clone();
            this.line3 = (LineShape?)starShape.line3.Clone();
        }
        public override string GetShapeType()
        {
            return "Star";
        }
        public override ShapeToDraw Clone()
        {
            return new StarShape(this);
        }
        public override void Remove()
        {
            base.Remove();
            triangle1.Remove();
            triangle2.Remove();
            line1.Remove();
            line2.Remove();
            line3.Remove();
        }
        public override void Draw()
        {

            triangle1.stroke = this.stroke;
            triangle2.stroke = this.stroke;
            line1.stroke = this.stroke;
            line2.stroke = this.stroke;
            line3.stroke = this.stroke;

            UpdateStartAndEndPoint();

            triangle1.StartPoint = StartPoint;
            triangle1.Draw();
            triangle2.Draw();
            line1.Draw();
            line2.Draw();
            line3.Draw();

            base.Draw();
        }
        public override void UpdateEndPoint()
        {
            if (StartPoint == EndPoint)
            {
                return;
            }
            triangle1.EndPoint = new Point(EndPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);
            triangle2.StartPoint = new Point(StartPoint.X, EndPoint.Y);
            triangle2.EndPoint = new Point(EndPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 1 / 4);
            line1.StartPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) / 3, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);
            line1.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 2 / 3, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);

            line2.StartPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) / 6, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            line2.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) / 3, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);

            line3.StartPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 5 / 6, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            line3.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 2 / 3, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);

            line1.UpdateEndPoint();
            line1.UpdateStartPoint();
            line2.UpdateEndPoint();
            line2.UpdateStartPoint();
            line3.UpdateEndPoint();
            line3.UpdateStartPoint();
            triangle1.UpdateEndPoint();
            triangle2.UpdateEndPoint();

        }
        //UpdateStartAndEndPoint
        public override void UpdateStartAndEndPoint()
        {
            base.UpdateStartAndEndPoint();

            triangle1.stroke = this.stroke;
            triangle2.stroke = this.stroke;
            line1.stroke = this.stroke;
            line2.stroke = this.stroke;
            line3.stroke = this.stroke;

            if (StartPoint == EndPoint)
            {
                return;
            }
            triangle1.StartPoint = StartPoint;
            triangle1.EndPoint = new Point(EndPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);
            triangle2.StartPoint = new Point(StartPoint.X, EndPoint.Y);
            triangle2.EndPoint = new Point(EndPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 1 / 4);



            line1.StartPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) / 3, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);
            line1.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 2 / 3, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);

            line2.StartPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) / 6, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            line2.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) / 3, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);

            line3.StartPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 5 / 6, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            line3.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 2 / 3, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);


            line1.UpdateStartAndEndPoint();
            line2.UpdateStartAndEndPoint();
            line3.UpdateStartAndEndPoint();
            triangle1.UpdateStartAndEndPoint();
            triangle2.UpdateStartAndEndPoint();

            base.UpdateStartAndEndPoint();
        }
        private Point CalculateCenter()
        {
            return new Point((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2);
        }
        public override void Rotate(double angle)
        {
            curAngle += angle;
            base.RotateSelectedBorder();
            Point center = CalculateCenter();
            RotateTransform rotateTransform = new RotateTransform(curAngle, center.X, center.Y);

            triangle1.Rotate(curAngle);
            triangle2.Rotate(curAngle);
            line1.Rotate(angle);
            line2.Rotate(angle);
            line3.Rotate(angle);
        }
    }


}
