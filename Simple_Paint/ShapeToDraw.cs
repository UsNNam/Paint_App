using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        public Line line;
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
            line.StrokeThickness = 5;
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

        public void UpdateStartPoint()
        {
            line.X2 = StartPoint.X;
            line.Y2 = StartPoint.Y;
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
        public void UpdateStartPoint()
        {
            if(EndPoint.X < StartPoint.X)
            {
                Canvas.SetLeft(rectangle, EndPoint.X);
            }
            else
            {
                Canvas.SetLeft(rectangle, StartPoint.X);
            }
            if(EndPoint.Y < StartPoint.Y)
            {
                Canvas.SetTop(rectangle, EndPoint.Y);
            }
            else
            {
                Canvas.SetTop(rectangle, StartPoint.Y);
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

        public void UpdateEndPointLandscapeOrientation()
        {

            p1.X = StartPoint.X;
            p1.Y = StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2;
            p2.X = EndPoint.X;
            p2.Y = StartPoint.Y;
            p3.X = EndPoint.X;
            p3.Y = EndPoint.Y;
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
            //triangle.Points = points;
        }

    }

    class StarShape : ShapeToDraw
    {
        TriangleShape triangle1, triangle2;
        LineShape line1, line2, line3;
        public StarShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
            Point newEndPoint1 = new Point(endPoint.X, startPoint.Y + (endPoint.Y - startPoint.Y) * 3/4);
            triangle1 = new TriangleShape(startPoint, newEndPoint1);
            Point newStartPoint = new Point(startPoint.X, startPoint.Y + (endPoint.Y - startPoint.Y) * 4/3 );
            Point newEndPoint = new Point(endPoint.X, startPoint.Y );
            triangle2 = new TriangleShape(newStartPoint, newEndPoint);
            line1 = new LineShape(new Point(0, 0), new Point(0, 0));
            line2 = new LineShape(new Point(0, 0), new Point(0, 0));
            line3 = new LineShape(new Point(0, 0), new Point(0, 0));
        }

        public override void Draw()
        {
            triangle1.StartPoint = StartPoint;
            triangle1.Draw();
            triangle2.Draw();
            line1.Draw();
            line2.Draw();
            line3.Draw();
        }
        public override void UpdateEndPoint()
        {
            triangle1.EndPoint= new Point(EndPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3/4);
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
    }

    class ArrowShape : ShapeToDraw {
        public RectangleShape rectangle;
        public TriangleShape triangle;
        public LineShape line;
        public ArrowShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
            rectangle = new RectangleShape(startPoint, endPoint);
            triangle = new TriangleShape(startPoint, endPoint);
        }

        public override void Draw()
        {
            
            rectangle.Draw();
            triangle.Draw();
            line = new LineShape(new Point(0, 0), new Point(0, 0));
            line.Draw();
        }

        public override void UpdateEndPoint()
        {

            rectangle.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);
            rectangle.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);
            rectangle.UpdateEndPoint();
            rectangle.UpdateStartPoint();
            triangle.StartPoint = new Point(EndPoint.X, StartPoint.Y);
            triangle.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, EndPoint.Y);
            triangle.UpdateEndPointLandscapeOrientation();

            line.StartPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);
            line.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);
            line.UpdateEndPoint();
            line.UpdateStartPoint();
        }
    }

    class PentagonArrowShape : ArrowShape
    {
        public PentagonArrowShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
           
        }
        public override void UpdateEndPoint()
        {

            rectangle.StartPoint = new Point(StartPoint.X, StartPoint.Y);
            rectangle.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, EndPoint.Y);
            rectangle.UpdateEndPoint();
            rectangle.UpdateStartPoint();
            triangle.StartPoint = new Point(EndPoint.X, StartPoint.Y);
            triangle.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, EndPoint.Y);
            triangle.UpdateEndPointLandscapeOrientation();

            line.StartPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, StartPoint.Y);
            line.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, EndPoint.Y);
            line.UpdateEndPoint();
            line.UpdateStartPoint();
        }
    }

    class CollateShape : ShapeToDraw
    {
        TriangleShape triangle1, triangle2;
        public CollateShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {

        }

        public override void Draw()
        {
            triangle1 = new TriangleShape(StartPoint, EndPoint);
            triangle1.Draw();
            triangle2 = new TriangleShape(new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2), new Point(EndPoint.X, EndPoint.Y));
            triangle2.Draw();
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
    }

}
