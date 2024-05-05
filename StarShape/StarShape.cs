
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
namespace StarShape
{
    [Serializable]
    class StarShape : ShapeToDraw
    {
        PointCollection points;
        Polygon star;
        public StarShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
            Point newEndPoint1 = new Point(endPoint.X, Math.Max(startPoint.Y + (endPoint.Y - startPoint.Y) * 3 / 4, 0));
            Point newStartPoint = new Point(startPoint.X, Math.Max(startPoint.Y + (endPoint.Y - startPoint.Y) * 4 / 3, 0));
            Point newEndPoint = new Point(endPoint.X, startPoint.Y);
            this.star = new Polygon();
        }
        public StarShape(StarShape starShape) : base(starShape)
        {
            // Tính kích thước và tâm của hình chữ nhật
            double width = EndPoint.X - StartPoint.X;
            double height = EndPoint.Y - StartPoint.Y;
            Point center = new Point(StartPoint.X + width / 2, StartPoint.Y + height / 2);

            // Tạo danh sách điểm cho ngôi sao
            this.points = new PointCollection();
            for (int i = 0; i < 10; i++)
            {
                double angle = i * Math.PI / 5 - Math.PI / 2;
                double radius = (i % 2 == 0) ? Math.Min(width, height) / 2 : Math.Min(width, height) / 4;  // giảm bán kính cho các điểm trong
                double x = center.X + radius * Math.Cos(angle) * (width / height);  // Điều chỉnh theo tỷ lệ chiều rộng
                double y = center.Y + radius * Math.Sin(angle);  // Không cần điều chỉnh chiều cao
                points.Add(new Point(x, y));
            }

            // Tạo và cấu hình đối tượng Polygon
            this.star = new Polygon
            {
                Points = points,
                Stroke = Brushes.Black,
                Fill = Brushes.Yellow,
                StrokeThickness = 2
            };

            
            if (starShape.points != null)
            {
                points = new PointCollection();
                for (int i = 0; i < 10; i++)
                {
                    points.Add(new Point(starShape.points[i].X, starShape.points[i].Y));
                }
            }
            
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
            canvas.Children.Remove(star);
        }
        public override void Draw()
        {
            

            UpdateStartAndEndPoint();

            double width = EndPoint.X - StartPoint.X;
            double height = EndPoint.Y - StartPoint.Y;
            Point center = new Point(StartPoint.X + width / 2, StartPoint.Y + height / 2);


            for (int i = 0; i < 10; i++)
            {
                double angle = i * Math.PI / 5 - Math.PI / 2;
                double radius = (i % 2 == 0) ? Math.Min(width, height) / 2 : Math.Min(width, height) / 4;  // giảm bán kính cho các điểm trong
                double x = center.X + radius * Math.Cos(angle) * (width / height);  // Điều chỉnh theo tỷ lệ chiều rộng
                double y = center.Y + radius * Math.Sin(angle);  // Không cần điều chỉnh chiều cao
                points[i] = new Point(x, y);
            }



            if (stroke != null)
            {
                star.Stroke = this.stroke.borderColor;
                star.StrokeThickness = this.stroke.thickness;
                star.StrokeDashArray = this.stroke.strokeDashArray;
                star.Fill = this.stroke.fillColor;
            }

            // Thêm ngôi sao vào Canvas
            canvas.Children.Add(star);

            base.Draw();
        }
        public override void UpdateEndPoint()
        {
            if (StartPoint == EndPoint)
            {
                return;
            }
/*            triangle1.EndPoint = new Point(EndPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);
            triangle2.StartPoint = new Point(StartPoint.X, EndPoint.Y);
            triangle2.EndPoint = new Point(EndPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 1 / 4);
            line1.StartPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) / 3, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);
            line1.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 2 / 3, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);

            line2.StartPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) / 6, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            line2.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) / 3, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);

            line3.StartPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 5 / 6, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            line3.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 2 / 3, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);
*/
            /*            line1.UpdateEndPoint();
                        line1.UpdateStartPoint();
                        line2.UpdateEndPoint();
                        line2.UpdateStartPoint();
                        line3.UpdateEndPoint();
                        line3.UpdateStartPoint();
                        triangle1.UpdateEndPoint();
                        triangle2.UpdateEndPoint();*/
            double width = EndPoint.X - StartPoint.X;
            double height = EndPoint.Y - StartPoint.Y;
            Point center = new Point(StartPoint.X + width / 2, StartPoint.Y + height / 2);


            for (int i = 0; i < 10; i++)
            {
                double angle = i * Math.PI / 5 - Math.PI / 2;
                double radius = (i % 2 == 0) ? Math.Min(width, height) / 2 : Math.Min(width, height) / 4;  // giảm bán kính cho các điểm trong
                double x = center.X + radius * Math.Cos(angle) * (width / height);  // Điều chỉnh theo tỷ lệ chiều rộng
                double y = center.Y + radius * Math.Sin(angle);  // Không cần điều chỉnh chiều cao
                points[i] = new Point(x, y);
            }

        }
        //UpdateStartAndEndPoint
        public override void UpdateStartAndEndPoint()
        {
            base.UpdateStartAndEndPoint();
            if (star != null && stroke != null && textStyleState == false)
            {
                star.Stroke = this.stroke.borderColor;
                star.StrokeThickness = this.stroke.thickness;
                star.StrokeDashArray = this.stroke.strokeDashArray;
                star.Fill = this.stroke.fillColor;
            }

            if (StartPoint == EndPoint)
            {
                return;
            }


            double width = EndPoint.X - StartPoint.X;
            double height = EndPoint.Y - StartPoint.Y;
            Point center = new Point(StartPoint.X + width / 2, StartPoint.Y + height / 2);


            for (int i = 0; i < 10; i++)
            {
                double angle = i * Math.PI / 5 - Math.PI / 2;
                double radius = (i % 2 == 0) ? Math.Min(width, height) / 2 : Math.Min(width, height) / 4;  // giảm bán kính cho các điểm trong
                double x = center.X + radius * Math.Cos(angle) * (width / height);  // Điều chỉnh theo tỷ lệ chiều rộng
                double y = center.Y + radius * Math.Sin(angle);  // Không cần điều chỉnh chiều cao
                points[i] = new Point(x, y);
            }

            /*triangle1.StartPoint = StartPoint;
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
            triangle2.UpdateStartAndEndPoint();*/

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
            star.RenderTransform = rotateTransform;

/*            triangle1.Rotate(curAngle);
            triangle2.Rotate(curAngle);
            line1.Rotate(angle);
            line2.Rotate(angle);
            line3.Rotate(angle);*/
        }
    }


}
