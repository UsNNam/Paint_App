
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
namespace EllipseShape
{
    [Serializable]
    class EllipseShape : ShapeToDraw
    {
        Ellipse ellipse;
        public EllipseShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
            ellipse = new Ellipse();
        }

        public override void Draw()
        {

            ellipse.Stroke = this.stroke.borderColor;
            ellipse.StrokeThickness = this.stroke.thickness;
            ellipse.StrokeDashArray = this.stroke.strokeDashArray;
            ellipse.Fill = this.stroke.fillColor;


            ellipse.Width = Math.Abs(EndPoint.X - StartPoint.X);
            ellipse.Height = Math.Abs(EndPoint.Y - StartPoint.Y);
            Canvas.SetLeft(ellipse, StartPoint.X);
            Canvas.SetTop(ellipse, StartPoint.Y);
            canvas.Children.Add(ellipse);
            base.Draw();
        }
        //Create copy constructor
        public EllipseShape(EllipseShape ellipseShape) : base(ellipseShape)
        {
            this.ellipse = new Ellipse();
        }

        public override void UpdateEndPoint()
        {
            ellipse.Width = Math.Abs(EndPoint.X - StartPoint.X);
            ellipse.Height = Math.Abs(EndPoint.Y - StartPoint.Y);

            if (EndPoint.X < StartPoint.X)
            {
                Canvas.SetLeft(ellipse, EndPoint.X);
            }
            if (EndPoint.Y < StartPoint.Y)
            {
                Canvas.SetTop(ellipse, EndPoint.Y);
            }
        }
        public override string GetShapeType()
        {
            return "Ellipse";
        }
        public override ShapeToDraw Clone()
        {
            return new EllipseShape(this);
        }
        public override void Remove()
        {
            base.Remove();
            canvas.Children.Remove(ellipse);

        }
        //UpdateStartAndEndPoint
        public override void UpdateStartAndEndPoint()
        {
            base.UpdateStartAndEndPoint();
            if (stroke != null && textStyleState == false)
            {
                ellipse.Stroke = this.stroke.borderColor;
                ellipse.StrokeThickness = this.stroke.thickness;
                ellipse.StrokeDashArray = this.stroke.strokeDashArray;
                ellipse.Fill = this.stroke.fillColor;
            }

            ellipse.Width = Math.Abs(EndPoint.X - StartPoint.X);
            ellipse.Height = Math.Abs(EndPoint.Y - StartPoint.Y);

            if (EndPoint.X < StartPoint.X)
            {
                Canvas.SetLeft(ellipse, EndPoint.X);
            }
            else
            {
                Canvas.SetLeft(ellipse, StartPoint.X);
            }
            if (EndPoint.Y < StartPoint.Y)
            {
                Canvas.SetTop(ellipse, EndPoint.Y);
            }
            else
            {
                Canvas.SetTop(ellipse, StartPoint.Y);
            }
        }
        public override void Rotate(double angle)
        {
            curAngle += angle;
            base.RotateSelectedBorder();
            // Đặt điểm tâm xoay tại tâm của ellipse
            ellipse.RenderTransformOrigin = new Point(0.5, 0.5);

            // Tạo và áp dụng RotateTransform
            RotateTransform rotateTransform = new RotateTransform(curAngle);
            ellipse.RenderTransform = rotateTransform;
        }
    }

}
