using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Simple_Paint
{
    class ShapeToDraw
    {
        public static Canvas canvas;


        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public static SolidColorBrush fillColor { get; set;}

        public static SolidColorBrush borderColor { get; set; }

        public ShapeToDraw(Point startPoint, Point endPoint) 
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

       
        virtual public void Draw()
        {
            
        }
        virtual public void UpdateEndPoint()
        {

        }

    }

    class LineShape : ShapeToDraw
    {
        Line line;
       public LineShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
       {

       }
       public override void Draw()
       {
            // Draw the line in canvas with start point and end point
            line = new Line();
            if(fillColor == null)
            {
                fillColor = Brushes.Black;
            }
            line.Stroke = fillColor;
            line.X1 = StartPoint.X;
            line.Y1 = StartPoint.Y;
            line.X2 = EndPoint.X;
            line.Y2 = EndPoint.Y;
            canvas.Children.Add(line);


       }
        public override void UpdateEndPoint()
        {
            line.X1 = EndPoint.X;
            line.Y1 = EndPoint.Y;
        }

    }

    class EllipseShape : ShapeToDraw
    {
        Ellipse ellipse;
        public EllipseShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {

        }
        public override void Draw()
        {
            
            ellipse = new Ellipse();
            ellipse.Stroke = borderColor;
            ellipse.Fill = fillColor;
            ellipse.StrokeThickness = 2;
            ellipse.Width = Math.Abs(EndPoint.X - StartPoint.X);
            ellipse.Height = Math.Abs(EndPoint.Y - StartPoint.Y);
            Canvas.SetLeft(ellipse, StartPoint.X);
            Canvas.SetTop(ellipse, StartPoint.Y);
            canvas.Children.Add(ellipse);

        }

        public override void UpdateEndPoint()
        {
            ellipse.Width = Math.Abs(EndPoint.X - StartPoint.X);
            ellipse.Height = Math.Abs(EndPoint.Y - StartPoint.Y);

            if(EndPoint.X < StartPoint.X)
            {
                Canvas.SetLeft(ellipse, EndPoint.X);
            }
            if(EndPoint.Y < StartPoint.Y)
            {
                Canvas.SetTop(ellipse, EndPoint.Y);
            }
        }
    }

    class RectangleShape : ShapeToDraw
    {
        Rectangle rectangle;
        public RectangleShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {

        }

        public override void Draw()
        {
            rectangle = new System.Windows.Shapes.Rectangle();
            rectangle.Stroke = borderColor;
            rectangle.Fill = fillColor;
            rectangle.StrokeThickness = 2;
            rectangle.Width = Math.Abs(EndPoint.X - StartPoint.X);
            rectangle.Height = Math.Abs(EndPoint.Y - StartPoint.Y);
            Canvas.SetLeft(rectangle, StartPoint.X);
            Canvas.SetTop(rectangle, StartPoint.Y);
            canvas.Children.Add(rectangle);
        }

        public override void UpdateEndPoint()
        {
            rectangle.Width = Math.Abs(EndPoint.X - StartPoint.X);
            rectangle.Height = Math.Abs(EndPoint.Y - StartPoint.Y);
            if(EndPoint.X < StartPoint.X)
            {
                Canvas.SetLeft(rectangle, EndPoint.X);
            }
            if(EndPoint.Y < StartPoint.Y)
            {
                Canvas.SetTop(rectangle, EndPoint.Y);
            }

        }
    }

    class TriangleShape : ShapeToDraw
    {
        Polygon triangle;
        Point p1, p2, p3;
        PointCollection points;
        public TriangleShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {

        }

        public override void Draw()
        {
            triangle = new Polygon();
            triangle.Stroke = borderColor;
            triangle.Fill = fillColor;
            triangle.StrokeThickness = 2;
            points = new PointCollection();
            p1 = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) / 2, StartPoint.Y);
            p2 = new Point(StartPoint.X, EndPoint.Y);
            p3 = new Point(EndPoint.X, EndPoint.Y);
            points.Add(p1);
            points.Add(p2);
            points.Add(p3);
            triangle.Points = points;
            canvas.Children.Add(triangle);
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

    }
}
