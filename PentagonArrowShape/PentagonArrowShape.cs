
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
using MyArrow;
using Shapes;
namespace PentagonArrowShape
{
    [Serializable]
    class PentagonArrowShape : ShapeToDraw
    {

        Polygon arrowPentagon;
        PointCollection points;
        public PentagonArrowShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {

        }
        public PentagonArrowShape(PentagonArrowShape pentagonArrowShape) : base(pentagonArrowShape)
        {
            if(pentagonArrowShape.points != null)
            {
                points = new PointCollection();
                for (int i = 0; i < 5; i++)
                {
                    points.Add(new Point(pentagonArrowShape.points[i].X, pentagonArrowShape.points[i].Y));
                }
            }
        }

        public override void Draw()
        {
            UpdateStartAndEndPoint();
            double width = EndPoint.X - StartPoint.X;
            double height = EndPoint.Y - StartPoint.Y;

            // Tạo một polygon mới
            arrowPentagon = new Polygon();
            if (stroke != null)
            {
                arrowPentagon.Stroke = this.stroke.borderColor;
                arrowPentagon.StrokeThickness = this.stroke.thickness;
                arrowPentagon.StrokeDashArray = this.stroke.strokeDashArray;
                arrowPentagon.Fill = this.stroke.fillColor;
            }

            // Định nghĩa các điểm cho Polygon để tạo hình dạng "ArrowPentagon"
            points = new PointCollection
            {
                new Point(StartPoint.X + width / 2, StartPoint.Y), // Đỉnh mũi tên trên cùng
                new Point(EndPoint.X, StartPoint.Y + height / 3), // Góc phải trên
                new Point(EndPoint.X - width / 4, StartPoint.Y + height / 2), // Góc phải dưới
                new Point(StartPoint.X + width / 4, StartPoint.Y + height / 2), // Góc trái dưới
                new Point(StartPoint.X, StartPoint.Y + height / 3) // Góc trái trên
            };

            arrowPentagon.Points = points;
            canvas.Children.Add(arrowPentagon);
            base.Draw();
        }

        public override void UpdateEndPoint()
        {
            double width = EndPoint.X - StartPoint.X;
            double height = EndPoint.Y - StartPoint.Y;
            points[0] = new Point(StartPoint.X + width / 2, StartPoint.Y);
            points[1] = new Point(EndPoint.X, StartPoint.Y + height / 3);
            points[2] = new Point(EndPoint.X - width / 4, StartPoint.Y + height / 2);
            points[3] = new Point(StartPoint.X + width / 4, StartPoint.Y + height / 2);
            points[4] = new Point(StartPoint.X, StartPoint.Y + height / 3);

        }

        public override void Remove()
        {
            base.Remove();
            canvas.Children.Remove(arrowPentagon);
        }

        public override ShapeToDraw Clone()
        {
            return new PentagonArrowShape(this);
        }
        public override string GetShapeType()
        {
            return "ArrowPentagon";
        }
        //UpdateStartAndEndPoint
        public override void UpdateStartAndEndPoint()
        {
            base.UpdateStartAndEndPoint();

            if (!(base.StartPoint == base.EndPoint))
            {
                double width = EndPoint.X - StartPoint.X;
                double height = EndPoint.Y - StartPoint.Y;
                points[0] = new Point(StartPoint.X + width / 2, StartPoint.Y);
                points[1] = new Point(EndPoint.X, StartPoint.Y + height / 3);
                points[2] = new Point(EndPoint.X - width / 4, StartPoint.Y + height / 2);
                points[3] = new Point(StartPoint.X + width / 4, StartPoint.Y + height / 2);
                points[4] = new Point(StartPoint.X, StartPoint.Y + height / 3);
                base.UpdateStartAndEndPoint();
            }
        }

        private Point CalculateCenter()
        {
            return new Point((base.StartPoint.X + base.EndPoint.X) / 2.0, (base.StartPoint.Y + base.EndPoint.Y) / 2.0);
        }
        public override void Rotate(double angle)
        {
            curAngle += angle;
            RotateSelectedBorder();
            Point center = CalculateCenter();
            RotateTransform rotateTransform = new RotateTransform(curAngle, center.X, center.Y);
            arrowPentagon.RenderTransform = rotateTransform;
        }
    }


}
