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
        Polygon collateShape;
        PointCollection points;
        public CollateShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
            this.collateShape = new Polygon();
        }

        public CollateShape(CollateShape collateShape) : base(collateShape)
        {

            this.collateShape = new Polygon();
            // Định nghĩa các điểm cho Polygon để tạo hình dạng "Collate"
            this.points = new PointCollection
            {
                StartPoint, // Góc trên bên trái
                new Point(EndPoint.X, EndPoint.Y), // Góc dưới bên phải
                new Point(StartPoint.X, EndPoint.Y), // Góc dưới bên trái
                new Point(EndPoint.X, StartPoint.Y), // Góc trên bên phải
                StartPoint // Trở lại điểm đầu tiên để đóng hình
            };
            if (collateShape.points != null  )
            {
                points = new PointCollection();
                for (int i = 0; i < 5; i++)
                {
                    points.Add(new Point(collateShape.points[i].X, collateShape.points[i].Y));
                }
            }
        }

        public override void Draw()
        {
            UpdateStartAndEndPoint();
            
            if (stroke != null)
            {
                collateShape.Stroke = this.stroke.borderColor;
                collateShape.StrokeThickness = this.stroke.thickness;
                collateShape.StrokeDashArray = this.stroke.strokeDashArray;
                collateShape.Fill = this.stroke.fillColor;
            }

            // Định nghĩa các điểm cho Polygon để tạo hình dạng "Collate"
            points = new PointCollection
            {
                StartPoint, // Góc trên bên trái
                new Point(EndPoint.X, EndPoint.Y), // Góc dưới bên phải
                new Point(StartPoint.X, EndPoint.Y), // Góc dưới bên trái
                new Point(EndPoint.X, StartPoint.Y), // Góc trên bên phải
                StartPoint // Trở lại điểm đầu tiên để đóng hình
            };

            collateShape.Points = points;
            ShapeToDraw.canvas.Children.Add(collateShape);
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
            points[0] = StartPoint;
            points[1] = EndPoint;
            points[2] = new Point(StartPoint.X, EndPoint.Y);
            points[3] = new Point(EndPoint.X, StartPoint.Y);
            points[4] = StartPoint;

        }
        public override void Remove()
        {
            base.Remove();
            canvas.Children.Remove(collateShape);
        }
        //UpdateStartAndEndPoint
        public override void UpdateStartAndEndPoint()
        {

            base.UpdateStartAndEndPoint();

            if (collateShape!= null && stroke != null && textStyleState == false)
            {
                collateShape.Stroke = this.stroke.borderColor;
                collateShape.StrokeThickness = this.stroke.thickness;
                collateShape.StrokeDashArray = this.stroke.strokeDashArray;
                collateShape.Fill = this.stroke.fillColor;
            }

            if (!(base.StartPoint == base.EndPoint))
            {
                points[0] = StartPoint;
                points[1] = EndPoint;
                points[2] = new Point(StartPoint.X, EndPoint.Y);
                points[3] = new Point(EndPoint.X, StartPoint.Y);
                points[4] = StartPoint;
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
            collateShape.RenderTransform = rotateTransform;
        }
    }


}
