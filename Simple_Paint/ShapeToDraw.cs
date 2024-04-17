using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Simple_Paint
{
    public class ShapeToDraw
    {
        public static List<ShapeToDraw> prototypes = new List<ShapeToDraw>();

        static ShapeToDraw(){
                   prototypes.Add(new LineShape(new Point(0, 0), new Point(0, 0)));
                   prototypes.Add(new EllipseShape(new Point(0, 0), new Point(0, 0)));
                   prototypes.Add(new RectangleShape(new Point(0, 0), new Point(0, 0)));
                   prototypes.Add(new TriangleShape(new Point(0, 0), new Point(0, 0)));
                   prototypes.Add(new StarShape(new Point(0, 0), new Point(0, 0)));
                   prototypes.Add(new ArrowShape(new Point(0, 0), new Point(0, 0)));
                   prototypes.Add(new CollateShape(new Point(0, 0), new Point(0, 0)));
                   prototypes.Add(new PentagonArrowShape(new Point(0, 0), new Point(0, 0)));
        }

        public static void AddPrototype(ShapeToDraw shape)
        {
            if(shape!=null)
            {
                bool isPresent = false;
                for(int i = 0; i < prototypes.Count; i++)
                {
                    if (prototypes[i].GetType() == shape.GetType())
                    {
                        isPresent = true;
                        break;
                    }
                }
                if (!isPresent)
                {
                    prototypes.Add(shape);
                }
            }
        }
        public static ShapeToDraw GetPrototype(string shapeType)
        {
            for(int i = 0; i < prototypes.Count; i++)
            {
                if (prototypes[i].GetShapeType() == shapeType)
                {
                    return prototypes[i].Clone();
                }
            }
            return null;
        }
        



        public static Canvas canvas;

        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public TextBox textBox;

        public Stroke stroke;
        public Stroke old;
        public ShapeToDraw(ShapeToDraw shapeToDraw)
        {
            this.StartPoint = shapeToDraw.StartPoint;
            this.EndPoint = shapeToDraw.EndPoint;
            this.stroke = shapeToDraw.stroke;
            if (shapeToDraw.textBox != null) {
                this.textBox = CloneTextBox(shapeToDraw.textBox);
                
            } 

        }

        public TextBox CloneTextBox(TextBox textBox)
        {
            TextBox newTextBox = new TextBox();
            newTextBox.Text = textBox.Text;
            newTextBox.Width = textBox.Width;
            newTextBox.Height = textBox.Height;
            newTextBox.BorderBrush = textBox.BorderBrush;
            Style textBoxStyle = new Style(typeof(TextBox));

            textBoxStyle.Setters.Add(new Setter(TextBox.ForegroundProperty, textBox.Foreground));
            newTextBox.Style = textBoxStyle;
            newTextBox.BorderThickness = new Thickness(0);
            newTextBox.FontFamily = textBox.FontFamily;
            newTextBox.FontSize = textBox.FontSize;
            newTextBox.Background = textBox.Background;
            newTextBox.HorizontalAlignment = HorizontalAlignment.Center;
            newTextBox.VerticalAlignment = VerticalAlignment.Center;
            newTextBox.TextAlignment = TextAlignment.Center;

            //Add textChanged and keyDown event
            newTextBox.TextChanged += (sender, e) =>
            {
                if (newTextBox.Width < (EndPoint.X - StartPoint.X))
                {
                    newTextBox.Width = CalculateMaxLettersInLine(newTextBox.Text) * 10;

                }
                if (newTextBox.Height < (EndPoint.Y - StartPoint.Y))
                {
                    //Calculate height depending on the number of lines and font size
                    newTextBox.Height = newTextBox.LineCount * (int)newTextBox.FontSize * 20 / 12;
                }
                updateTextBoxPosition();
            };

            newTextBox.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    // Insert a new line character
                    newTextBox.Text += "\n";

                    // Move the caret to the end of the text
                    newTextBox.CaretIndex = newTextBox.Text.Length;
                }
            };

            return newTextBox;
        }

        public ShapeToDraw(Point startPoint, Point endPoint) 
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            
        }
        public virtual void attachMouseRightClickEvent()
        {
            
        }
        public bool IsPointInShape(Point point)
        {
            // Tính toán lại để đảm bảo đúng cặp điểm góc trái trên và góc phải dưới
            double left = Math.Min(StartPoint.X, EndPoint.X);
            double top = Math.Min(StartPoint.Y, EndPoint.Y);
            double right = Math.Max(StartPoint.X, EndPoint.X);
            double bottom = Math.Max(StartPoint.Y, EndPoint.Y);

            // Tạo điểm góc trái trên và góc phải dưới mới
            Point newTopLeft = new Point(left, top);

            Point newBottomRight = new Point(right, bottom);

            if(point.X >= newTopLeft.X && point.X <= newBottomRight.X && point.Y >= newTopLeft.Y && point.Y <= newBottomRight.Y)
            {
                return true;
            }
            return false;

        }

        public static int CalculateMaxLettersInLine(string paragraph)
        {
            string[] lines = paragraph.Split('\n');
            int maxLetters = lines.Max(line => line.Length);
            return maxLetters;
        }

        virtual public void attachTextBox(SolidColorBrush color, SolidColorBrush backgroundColor, int fontsize, string fontFamily)
        {
            if(textBox != null)
            {
               return;
            }
            textBox = FactoryWord.CreateTextBox(backgroundColor, fontsize, color, fontFamily);


            updateTextBoxPosition();

            textBox.TextChanged += (sender, e) =>
            {
                if(textBox.Width < (EndPoint.X - StartPoint.X))
                {
                    textBox.Width = CalculateMaxLettersInLine(textBox.Text) * 10;
                    
                }
                if(textBox.Height < (EndPoint.Y - StartPoint.Y))
                {
                    //Calculate height depending on the number of lines and font size
                    textBox.Height = textBox.LineCount * fontsize * 20 / 12;
                }
                

                updateTextBoxPosition();
            };

            textBox.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    // Insert a new line character
                    textBox.Text += "\n";

                    // Move the caret to the end of the text
                    textBox.CaretIndex = textBox.Text.Length;
                }
            };

            canvas.Children.Add(textBox);

        }

       
        virtual public void Draw()
        {
            if(textBox != null)
            {
                updateTextBoxPosition();
                canvas.Children.Add(textBox);
            }
        }
        virtual public void UpdateEndPoint()
        {
            
        }
        virtual public void Remove()
        {
            if(textBox != null)
            {
                canvas.Children.Remove(textBox);
            }
        }   
        virtual public string GetShapeType()
        {
            return "";
        }
        virtual public ShapeToDraw Clone()
        {
            return (ShapeToDraw)this.MemberwiseClone();
        }


        public bool isDragging;
        public Point dragStartPoint;
        public Point curDragPoint;

        public bool StartDrag(Point startPoint)
        {
            // Save the starting point of the drag
            dragStartPoint = startPoint;

            // Check if the start point is within the shape
            if (IsPointInShape(startPoint))
            {
                isDragging = true;
                return true;
            }
            else
            {
                Drop();
                return false;
            }
        }

        public void Drag(Point currentPoint)
        {
            if (isDragging)
            {
                curDragPoint = currentPoint;
                // Calculate the offset from the start point
                double offsetX = currentPoint.X - dragStartPoint.X;
                double offsetY = currentPoint.Y - dragStartPoint.Y;

                // Update the start and end points of the shape
                StartPoint = new Point(StartPoint.X + offsetX, StartPoint.Y + offsetY);
                EndPoint = new Point(EndPoint.X + offsetX, EndPoint.Y + offsetY);

                // Update the drag start point for the next call
                dragStartPoint = currentPoint;

                // Redraw the shape at its new location (you may need to call a method to refresh the canvas here)
                UpdateStartAndEndPoint();
            }
        }

        public void Drop()
        {
            // End the dragging operation
            isDragging = false;
        }
        virtual public void UpdateStartAndEndPoint()
        {
            updateTextBoxPosition();
        }
        virtual public void updateTextBoxPosition()
        {
            if(textBox == null)
            {
                return;
            }
            
            Canvas.SetLeft(textBox, (StartPoint.X + EndPoint.X) / 2 - textBox.Width / 2);
            Canvas.SetTop(textBox, (StartPoint.Y + EndPoint.Y) / 2 - textBox.Height / 2);
        }

    }

    class LineShape : ShapeToDraw
    {
        public Line line;
       public LineShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
       {
            line = new Line();

       }
        // create copy constructor
        public LineShape(LineShape lineShape) : base(lineShape)
        {
            line = new Line();
        }

        public override void Draw()
       {
            // Draw the line in canvas with start point and end point
            
            line.Stroke = this.stroke.borderColor;
            line.StrokeThickness = this.stroke.thickness;
            line.StrokeDashArray = this.stroke.strokeDashArray;

            line.X1 = StartPoint.X;
            line.Y1 = StartPoint.Y;
            line.X2 = EndPoint.X;
            line.Y2 = EndPoint.Y;
            canvas.Children.Add(line);
            base.Draw();
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
        public override string GetShapeType()
        {
            return "Line";
        }
        public override ShapeToDraw Clone()
        {
            return new LineShape(this);
        }
        public override void Remove()
        {
            base.Remove();
            canvas.Children.Remove(line);
        }
        //UpdateStartAndEndPoint
        public override void UpdateStartAndEndPoint()
        {
            base.UpdateStartAndEndPoint();
            line.Stroke = this.stroke.borderColor;
            line.StrokeThickness = this.stroke.thickness;
            line.StrokeDashArray = this.stroke.strokeDashArray;


            line.X1 = StartPoint.X;
            line.Y1 = StartPoint.Y;
            line.X2 = EndPoint.X;
            line.Y2 = EndPoint.Y;
        }

    }

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

            if(EndPoint.X < StartPoint.X)
            {
                Canvas.SetLeft(ellipse, EndPoint.X);
            }
            if(EndPoint.Y < StartPoint.Y)
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
            ellipse.Stroke = this.stroke.borderColor;
            ellipse.StrokeThickness = this.stroke.thickness;
            ellipse.StrokeDashArray = this.stroke.strokeDashArray;
            ellipse.Fill = this.stroke.fillColor;
            if(EndPoint.X < StartPoint.X)
            {
                Canvas.SetLeft(ellipse, EndPoint.X);
            }
            else
            {
                Canvas.SetLeft(ellipse, StartPoint.X);
            }
            if(EndPoint.Y < StartPoint.Y)
            {
                Canvas.SetTop(ellipse, EndPoint.Y);
            }
            else
            {
                Canvas.SetTop(ellipse, StartPoint.Y);
            }
        }
    }

    class RectangleShape : ShapeToDraw
    {
        public Rectangle rectangle;
        
        public RectangleShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
            this.rectangle = new System.Windows.Shapes.Rectangle();

        }
        public RectangleShape(RectangleShape rectangleShape) : base(rectangleShape)
        {
            this.rectangle = new System.Windows.Shapes.Rectangle();
        }


        public override void Draw()
        {
            
            if (!canvas.Children.Contains(rectangle))
            {

                if (this.stroke != null)
                {
                    rectangle.Stroke = this.stroke.borderColor;
                    rectangle.StrokeThickness = this.stroke.thickness;
                    rectangle.StrokeDashArray = this.stroke.strokeDashArray;
                    rectangle.Fill = this.stroke.fillColor;
                }
                rectangle.Width = Math.Abs(EndPoint.X - StartPoint.X);
                rectangle.Height = Math.Abs(EndPoint.Y - StartPoint.Y);
                Canvas.SetLeft(rectangle, StartPoint.X);
                Canvas.SetTop(rectangle, StartPoint.Y);
                canvas.Children.Add(rectangle);
            }
            base.Draw();
        }
        public override string GetShapeType()
        {
            return "Rectangle";
        }
        public override ShapeToDraw Clone()
        {
            return new RectangleShape(this);
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
        public override void Remove()
        {
            base.Remove();
            if (canvas.Children.Contains(rectangle))
            { 
                canvas.Children.Remove(this.rectangle);
                canvas.InvalidateVisual();
                canvas.UpdateLayout();
            }
        }
        //UpdateStartAndEndPoint
        public override void UpdateStartAndEndPoint()
        {
            base.UpdateStartAndEndPoint();
            rectangle.Stroke = this.stroke.borderColor;
            rectangle.StrokeThickness = this.stroke.thickness;
            rectangle.StrokeDashArray = this.stroke.strokeDashArray;
            rectangle.Fill = this.stroke.fillColor;
            if(EndPoint.X < StartPoint.X)
            {
                Canvas.SetLeft(this.rectangle, EndPoint.X);
            }
            else
            {
                Canvas.SetLeft(this.rectangle, StartPoint.X);
            }
            if(EndPoint.Y < StartPoint.Y)
            {
                Canvas.SetTop(this.rectangle, EndPoint.Y);
            }
            else
            {
                Canvas.SetTop(this.rectangle, StartPoint.Y);
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
        public TriangleShape(TriangleShape triangleShape) : base(triangleShape)
        {
            // Copy constructor
            this.triangle = new Polygon();
            this.p1 = new Point(triangleShape.p1.X,triangleShape.p1.Y);
            this.p2 = new Point(triangleShape.p2.X, triangleShape.p2.Y);
            this.p3 = new Point(triangleShape.p3.X, triangleShape.p3.Y);

            this.points = new PointCollection();
            points.Add(p1);
            points.Add(p2);
            points.Add(p3);
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
            if (EndPoint.Equals(new Point(0,0)))
            {
                EndPoint = StartPoint;
            }
            triangle.Stroke = this.stroke.borderColor;
            triangle.StrokeThickness = this.stroke.thickness;
            triangle.StrokeDashArray = this.stroke.strokeDashArray;
            triangle.Fill = this.stroke.fillColor;

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

    class StarShape : ShapeToDraw
    {
        TriangleShape triangle1, triangle2;
        LineShape line1, line2, line3;
        public StarShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
            Point newEndPoint1 = new Point(endPoint.X, Math.Max(startPoint.Y + (endPoint.Y - startPoint.Y) * 3/4,0));
            Point newStartPoint = new Point(startPoint.X, Math.Max(startPoint.Y + (endPoint.Y - startPoint.Y) * 4 / 3,0));
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
            if(StartPoint == EndPoint)
            {
                return;
            }
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
        //UpdateStartAndEndPoint
        public override void UpdateStartAndEndPoint()
        {
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
    }

    class ArrowShape : ShapeToDraw {
        public RectangleShape rectangle;
        public TriangleShape triangle;
        public LineShape line;
        public ArrowShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
            rectangle = new RectangleShape(startPoint, endPoint);
            triangle = new TriangleShape(startPoint, endPoint);
            line = new LineShape(new Point(0, 0), new Point(0, 0));

        }
        public ArrowShape(ArrowShape arrowShape) : base(arrowShape)
        {
            this.rectangle = (RectangleShape?)arrowShape.rectangle.Clone();
            this.triangle = (TriangleShape?)arrowShape.triangle.Clone();
            this.line = (LineShape?)arrowShape.line.Clone();
            //UpdateStartAndEndPoint();
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
            rectangle.Remove();
            triangle.Remove();
            line.Remove();
        }
        public override void Draw()
        {
            
            rectangle.stroke = this.stroke;
            triangle.stroke = this.stroke;
            line.stroke = this.stroke;

            UpdateStartAndEndPoint();

            rectangle.Draw();
            triangle.Draw();
            line.Draw();

            base.Draw();
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
        //UpdateStartAndEndPoint
        public override void UpdateStartAndEndPoint()
        {

            /*rectangle.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 4);
            rectangle.EndPoint = new Point(StartPoint.X + (EndPoint.X - StartPoint.X) * 3 / 4, StartPoint.Y + (EndPoint.Y - StartPoint.Y) * 3 / 4);
            rectangle.isDragging = this.isDragging;
            rectangle.dragStartPoint = this.dragStartPoint;
            rectangle.curDragPoint = this.curDragPoint;
            rectangle.Drag(curDragPoint)*/

            rectangle.stroke = this.stroke;
            triangle.stroke = this.stroke;
            line.stroke = this.stroke;

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

            rectangle.UpdateStartAndEndPoint();
            line.UpdateStartAndEndPoint();

            base.UpdateStartAndEndPoint();

        }
    }

    class PentagonArrowShape : ArrowShape
    {
        public PentagonArrowShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
           
        }
        public PentagonArrowShape(PentagonArrowShape pentagonArrowShape) : base(pentagonArrowShape)
        {
            
        }
        public override void UpdateEndPoint()
        {
            rectangle.stroke = this.stroke;
            triangle.stroke = this.stroke;
            line.stroke = this.stroke;

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

            rectangle.stroke = this.stroke;
            triangle.stroke = this.stroke;
            line.stroke = this.stroke;

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

            rectangle.UpdateStartAndEndPoint();
            line.UpdateStartAndEndPoint();

            base.UpdateStartAndEndPoint();
        }
    }

    class CollateShape : ShapeToDraw
    {
        TriangleShape triangle1, triangle2;
        public CollateShape(Point startPoint, Point endPoint) : base(startPoint, endPoint)
        {
            triangle1 = new TriangleShape(StartPoint, EndPoint);
            triangle2 = new TriangleShape(StartPoint, EndPoint);

        }
        public CollateShape(CollateShape collateShape) : base(collateShape)
        {
            this.triangle1 = (TriangleShape?)collateShape.triangle1.Clone();
            this.triangle2 = (TriangleShape?)collateShape.triangle2.Clone();
        }

        public override void Draw()
        {
            UpdateStartAndEndPoint();
            triangle1.StartPoint = StartPoint;
            triangle1.EndPoint = EndPoint;

            triangle2.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            triangle2.EndPoint = new Point(EndPoint.X, EndPoint.Y);
            

            triangle1.stroke = this.stroke;
            triangle2.stroke = this.stroke;

            triangle1.Draw();
            triangle2.Draw();

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
            triangle1.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            triangle1.EndPoint = new Point(EndPoint.X, StartPoint.Y);
            triangle1.UpdateEndPoint();
            triangle2.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            triangle2.EndPoint = EndPoint;
            triangle2.UpdateEndPoint();

        }
        public override void Remove()
        {
            base.Remove();
            triangle1.Remove();
            triangle2.Remove();
        }
        //UpdateStartAndEndPoint
        public override void UpdateStartAndEndPoint()
        {
            triangle1.stroke = this.stroke;
            triangle2.stroke = this.stroke;

            triangle1.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            triangle1.EndPoint = new Point(EndPoint.X, StartPoint.Y);
            triangle1.UpdateStartAndEndPoint();
            triangle2.StartPoint = new Point(StartPoint.X, StartPoint.Y + (EndPoint.Y - StartPoint.Y) / 2);
            triangle2.EndPoint = EndPoint;
            triangle2.UpdateStartAndEndPoint();

            base.UpdateStartAndEndPoint();
        }
    }

}
