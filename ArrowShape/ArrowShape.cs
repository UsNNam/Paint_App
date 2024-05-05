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
            isPolygon = true;


            arrow = new Polygon();

            arrow.RenderTransform = transformGroup;
            transformGroup.Children.Add(translateTransform);

            transformGroup.Children.Add(rotateTransform);

            double width = EndPoint.X - StartPoint.X;
            double height = EndPoint.Y - StartPoint.Y;
            points = new PointCollection
            {
                new Point(StartPoint.X, StartPoint.Y + height * 0.5),     // Điểm bắt đầu cánh trái
                new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.5),  // Đầu cánh phải
                new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.2),  // Điểm giữa cánh phải trên
                new Point(StartPoint.X + width, StartPoint.Y + height * 0.5),         // Đỉnh của mũi tên
                new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.8),  // Điểm giữa cánh phải dưới
                new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.5),  // Kết thúc cánh phải
                new Point(StartPoint.X, StartPoint.Y + height * 0.5)                  // Kết thúc cánh trái
            };
        }

        public ArrowShape(ArrowShape arrowShape) : base(arrowShape)
        {


            isPolygon = true;

            arrow = new Polygon();
            double width = EndPoint.X - StartPoint.X;
            double height = EndPoint.Y - StartPoint.Y;
            points = new PointCollection
            {
                new Point(StartPoint.X, StartPoint.Y + height * 0.5),     // Điểm bắt đầu cánh trái
                new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.5),  // Đầu cánh phải
                new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.2),  // Điểm giữa cánh phải trên
                new Point(StartPoint.X + width, StartPoint.Y + height * 0.5),         // Đỉnh của mũi tên
                new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.8),  // Điểm giữa cánh phải dưới
                new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.5),  // Kết thúc cánh phải
                new Point(StartPoint.X, StartPoint.Y + height * 0.5)                  // Kết thúc cánh trái
            };

            arrow.RenderTransform = transformGroup;
            this.rotateTransform = CopyRotateTransform(arrowShape.rotateTransform);
            this.translateTransform = CopyTranslateTransform(arrowShape.translateTransform);
            this.transformGroup.Children.Add(translateTransform);
            this.transformGroup.Children.Add(rotateTransform);

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
                new Point(StartPoint.X, StartPoint.Y + height * 0.5),     // Điểm bắt đầu cánh trái
                new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.5),  // Đầu cánh phải
                new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.2),  // Điểm giữa cánh phải trên
                new Point(StartPoint.X + width, StartPoint.Y + height * 0.5),         // Đỉnh của mũi tên
                new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.8),  // Điểm giữa cánh phải dưới
                new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.5),  // Kết thúc cánh phải
                new Point(StartPoint.X, StartPoint.Y + height * 0.5)                  // Kết thúc cánh trái
            };


            arrow.Points = points;

            ShapeToDraw.canvas.Children.Add(arrow);
            base.Draw();
        }

        public override void UpdateEndPoint()
        {
            base.UpdateEndPoint(); // Gọi implementation cơ bản nếu có.
            UpdateArrowPoints(); // Cập nhật điểm dựa trên StartPoint và EndPoint hiện tại.
        }

        public override void UpdateStartAndEndPoint()
        {
            base.UpdateStartAndEndPoint(); // Gọi implementation cơ bản nếu có.

            if (arrow != null && stroke != null)
            {
                arrow.Stroke = this.stroke.borderColor;
                arrow.StrokeThickness = this.stroke.thickness;
                arrow.StrokeDashArray = this.stroke.strokeDashArray;
                arrow.Fill = this.stroke.fillColor;
            }


            UpdateArrowPoints(); // Cập nhật điểm sau khi cập nhật stroke và fill.

            // Không cần kiểm tra base.StartPoint == base.EndPoint vì không rõ mục đích.
        }

        private void UpdateArrowPoints()
        {
            if (points != null)
            {
                double width = EndPoint.X - StartPoint.X;
                double height = EndPoint.Y - StartPoint.Y;

                points[0] = new Point(StartPoint.X, StartPoint.Y + height * 0.5);
                points[1] = new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.5);
                points[2] = new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.2);
                points[3] = new Point(StartPoint.X + width, StartPoint.Y + height * 0.5);
                points[4] = new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.8);
                points[5] = new Point(StartPoint.X + width * 0.75, StartPoint.Y + height * 0.5);
                points[6] = new Point(StartPoint.X, StartPoint.Y + height * 0.5);

                arrow.Points = points; // Đảm bảo các điểm được cập nhật cho arrow.
            }
        }

        public override void MovePolygon(double deltaX, double deltaY)
        {
            translateTransform.X += deltaX;
            translateTransform.Y += deltaY;
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
            // Thay đổi góc xoay
            rotateTransform.Angle = curAngle;

            // Thay đổi tâm xoay
            rotateTransform.CenterX = center.X;
            rotateTransform.CenterY = center.Y;
        }

    }


}
