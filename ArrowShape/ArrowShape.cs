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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using MyTriangle;
using Shapes;
namespace MyArrow
{
    [Serializable]
    public class ArrowShape : ShapeToDraw
    {
        Polygon arrow;
        PointCollection points;
        public ArrowShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
            arrow = new Polygon();
            double width = EndPoint.X - StartPoint.X;
            double height = EndPoint.Y - StartPoint.Y;
            points = new PointCollection
            {
                new Point(StartPoint.X + width * 0.2, StartPoint.Y + height * 0.5),  // Điểm bắt đầu cánh trái
                new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.5),  // Đầu cánh phải
                new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.2),  // Điểm giữa cánh phải
                new Point(StartPoint.X + width, StartPoint.Y + height * 0.5),        // Đỉnh của mũi tên
                new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.8),  // Điểm giữa cánh phải phía dưới
                new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.5),  // Kết thúc cánh phải
                new Point(StartPoint.X + width * 0.2, StartPoint.Y + height * 0.5)   // Kết thúc cánh trái
            };
        }

        public ArrowShape(ArrowShape arrowShape) : base(arrowShape)
        {
            arrow = new Polygon();
            double width = EndPoint.X - StartPoint.X;
            double height = EndPoint.Y - StartPoint.Y;
            points = new PointCollection
            {
                new Point(StartPoint.X + width * 0.2, StartPoint.Y + height * 0.5),  // Điểm bắt đầu cánh trái
                new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.5),  // Đầu cánh phải
                new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.2),  // Điểm giữa cánh phải
                new Point(StartPoint.X + width, StartPoint.Y + height * 0.5),        // Đỉnh của mũi tên
                new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.8),  // Điểm giữa cánh phải phía dưới
                new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.5),  // Kết thúc cánh phải
                new Point(StartPoint.X + width * 0.2, StartPoint.Y + height * 0.5)   // Kết thúc cánh trái
            };
            if (arrowShape.points != null)
            {
                points = new PointCollection();
                for (int i = 0; i < 7; i++)
                {
                    points.Add(new Point(arrowShape.points[i].X, arrowShape.points[i].Y));
                }
            }

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
            canvas.Children.Remove(arrow);
        }
        public override void Draw()
        {
            
            UpdateStartAndEndPoint();
            // Tính chiều rộng và chiều cao của hình chữ nhật
            double width = EndPoint.X - StartPoint.X;
            double height = EndPoint.Y - StartPoint.Y;

            // Tạo một polygon mới
            
            arrow.Stroke = Brushes.Black;
            arrow.Fill = Brushes.LightBlue;
            arrow.StrokeThickness = 2;

            if (stroke != null)
            {
                arrow.Stroke = this.stroke.borderColor;
                arrow.StrokeThickness = this.stroke.thickness;
                arrow.StrokeDashArray = this.stroke.strokeDashArray;
                arrow.Fill = this.stroke.fillColor;
            }

            // Định nghĩa các điểm cho Polygon theo chiều rộng và chiều cao
            points = new PointCollection
            {
                new Point(StartPoint.X + width * 0.2, StartPoint.Y + height * 0.5),  // Điểm bắt đầu cánh trái
                new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.5),  // Đầu cánh phải
                new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.2),  // Điểm giữa cánh phải
                new Point(StartPoint.X + width, StartPoint.Y + height * 0.5),        // Đỉnh của mũi tên
                new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.8),  // Điểm giữa cánh phải phía dưới
                new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.5),  // Kết thúc cánh phải
                new Point(StartPoint.X + width * 0.2, StartPoint.Y + height * 0.5)   // Kết thúc cánh trái
            };


            arrow.Points = points;

            ShapeToDraw.canvas.Children.Add(arrow);
            base.Draw();
        }

        public override void UpdateEndPoint()
        {
            // Tính chiều rộng và chiều cao của hình chữ nhật
            double width = EndPoint.X - StartPoint.X;
            double height = EndPoint.Y - StartPoint.Y;

            points[0] = new Point(StartPoint.X + width * 0.2, StartPoint.Y + height * 0.5);
            points[1] = new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.5);
            points[2] = new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.2);
            points[3] = new Point(StartPoint.X + width, StartPoint.Y + height * 0.5);
            points[4] = new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.8);
            points[5] = new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.5);
            points[6] = new Point(StartPoint.X + width * 0.2, StartPoint.Y + height * 0.5);

            //arrow.Points = points;

            /*            rectangle.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);
                        rectangle.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);
                        rectangle.UpdateEndPoint();
                        rectangle.UpdateStartPoint();
                        triangle.StartPoint = new Point(EndPoint.X, StartPoint.Y);
                        triangle.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, EndPoint.Y);
                        triangle.UpdateEndPointLandscapeOrientation();

                        line.StartPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);
                        line.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);
                        line.UpdateEndPoint();
                        line.UpdateStartPoint();*/
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

            if (arrow != null && stroke != null && textStyleState == false)
            {
                arrow.Stroke = this.stroke.borderColor;
                arrow.StrokeThickness = this.stroke.thickness;
                arrow.StrokeDashArray = this.stroke.strokeDashArray;
                arrow.Fill = this.stroke.fillColor;
            }

            if (!(base.StartPoint == base.EndPoint))
            {
                double width = EndPoint.X - StartPoint.X;
                double height = EndPoint.Y - StartPoint.Y;

                points[0] = new Point(StartPoint.X + width * 0.2, StartPoint.Y + height * 0.5);
                points[1] = new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.5);
                points[2] = new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.2);
                points[3] = new Point(StartPoint.X + width, StartPoint.Y + height * 0.5);
                points[4] = new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.8);
                points[5] = new Point(StartPoint.X + width * 0.8, StartPoint.Y + height * 0.5);
                points[6] = new Point(StartPoint.X + width * 0.2, StartPoint.Y + height * 0.5);
                base.UpdateStartAndEndPoint();
            }



            //arrow.Points = points;


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
            arrow.RenderTransform = rotateTransform;
        }
    }


}
