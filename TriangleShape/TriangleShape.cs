
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

using Shapes;
namespace MyTriangle
{
    [Serializable]
    public class TriangleShape : ShapeToDraw
    {
        Polygon triangle;
        Point p1, p2, p3;
        PointCollection points;
        public TriangleShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {

        }
        public TriangleShape(TriangleShape triangleShape) : base(triangleShape)
        {
            // Copy constructor
            this.triangle = new Polygon();
            this.p1 = new Point(triangleShape.p1.X, triangleShape.p1.Y);
            this.p2 = new Point(triangleShape.p2.X, triangleShape.p2.Y);
            this.p3 = new Point(triangleShape.p3.X, triangleShape.p3.Y);

            this.points = new PointCollection();
            points.Add(p1);
            points.Add(p2);
            points.Add(p3);
        }
        public override void Rotate(double angle)
        {
            curAngle += angle;
            base.RotateSelectedBorder();
            // Tính tâm của tam giác để đặt điểm xoay
            /*double centerX = (p1.X + p2.X + p3.X) / 3;
            double centerY = (p1.Y + p2.Y + p3.Y) / 3;
*/
            double centerX = (StartPoint.X + EndPoint.X) / 2;
            double centerY = (StartPoint.Y + EndPoint.Y) / 2;

            // Đặt điểm tâm xoay
            triangle.RenderTransformOrigin = new Point(0, 0);

            // Tạo và áp dụng RotateTransform
            RotateTransform rotateTransform = new RotateTransform(curAngle, centerX, centerY);
            triangle.RenderTransform = rotateTransform;
        }
        public override string GetShapeType()
        {
            return "Triangle";
        }
        public override ShapeToDraw Clone()
        {
            return new TriangleShape(this);
        }
        public override void Remove()
        {
            base.Remove();
            canvas.Children.Remove(triangle);

        }


        public override void Draw()
        {
            if (EndPoint.Equals(new Point(0, 0)))
            {
                EndPoint = StartPoint;
            }
            if (stroke != null)
            {
                triangle.Stroke = this.stroke.borderColor;
                triangle.StrokeThickness = this.stroke.thickness;
                triangle.StrokeDashArray = this.stroke.strokeDashArray;
                triangle.Fill = this.stroke.fillColor;
            }
            p1 = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) / 2, StartPoint.Y);
            p2 = new Point(StartPoint.X, EndPoint.Y);
            p3 = new Point(EndPoint.X, EndPoint.Y);

            triangle.Points = points;
            canvas.Children.Add(triangle);
            base.Draw();
        }

        public override void UpdateEndPoint()
        {
            p1.X = StartPoint.X + (EndPoint.X - StartPoint.X) / 2;
            p1.Y = StartPoint.Y;
            p2.X = StartPoint.X;
            p2.Y = EndPoint.Y;
            p3.X = EndPoint.X;
            p3.Y = EndPoint.Y;
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
            //triangle.Points = points;
        }

        public void UpdateEndPointLandscapeOrientation()
        {
            base.UpdateStartAndEndPoint();


            p1.X = StartPoint.X;
            p1.Y = StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2;
            p2.X = EndPoint.X;
            p2.Y = StartPoint.Y;
            p3.X = EndPoint.X;
            p3.Y = EndPoint.Y;
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
            triangle.Stroke = this.stroke.borderColor;
            triangle.StrokeThickness = this.stroke.thickness;
            triangle.StrokeDashArray = this.stroke.strokeDashArray;
            triangle.Fill = this.stroke.fillColor;
            //triangle.Points = points;
        }
        //UpdateStartAndEndPoint
        public override void UpdateStartAndEndPoint()
        {
            base.UpdateStartAndEndPoint();


            triangle.Stroke = this.stroke.borderColor;
            triangle.StrokeThickness = this.stroke.thickness;
            triangle.StrokeDashArray = this.stroke.strokeDashArray;
            triangle.Fill = this.stroke.fillColor;
            p1.X = StartPoint.X + (EndPoint.X - StartPoint.X) / 2;
            p1.Y = StartPoint.Y;
            p2.X = StartPoint.X;
            p2.Y = EndPoint.Y;
            p3.X = EndPoint.X;
            p3.Y = EndPoint.Y;
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
            //triangle.Points = points;
            base.UpdateStartAndEndPoint();
        }

    }

}
