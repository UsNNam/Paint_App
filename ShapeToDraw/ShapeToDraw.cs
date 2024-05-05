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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MyStroke;


namespace Shapes
{

    [Serializable]
    public class ShapeToDraw
    {
        public static List<ShapeToDraw> prototypes = new List<ShapeToDraw>();
        public static int rotateAngle = 45;
        public static bool textStyleState = false;
        public static SolidColorBrush borderColor = Brushes.Green;
        public static SolidColorBrush fillColor = Brushes.Blue;
        public static int fontSize = 12;
        public static string fontFamily = "Arial";
        static ShapeToDraw()
        {

        }

        //For text box
        public bool textBoxState = false;
        public string textBoxText=null;
        public double textBoxFontSize;
        public SolidColorBrush textBoxForeground;
        public SolidColorBrush textBoxBackground;
        public string textBoxFontFamily;


        public static void AddPrototype(ShapeToDraw shape)
        {
            if (shape != null)
            {
                bool isPresent = false;
                for (int i = 0; i < prototypes.Count; i++)
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
            for (int i = 0; i < prototypes.Count; i++)
            {
                if (prototypes[i].GetShapeType() == shapeType)
                {
                    return prototypes[i].Clone();
                }
            }
            return null;
        }


        public static Canvas canvas;

        public double curAngle = 0;

        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public TextBox textBox =  null;

        public Stroke stroke;
        public Stroke old;
        public int curLayer = 0;

        public RectangleShape borderSelected = null;
        public ShapeToDraw(ShapeToDraw shapeToDraw) : this(shapeToDraw.StartPoint, shapeToDraw.EndPoint)
        {
            this.StartPoint = shapeToDraw.StartPoint;
            this.EndPoint = shapeToDraw.EndPoint;
            this.stroke = shapeToDraw.stroke;
            if (shapeToDraw.textBox != null)
            {
                this.textBox = CloneTextBox(shapeToDraw.textBox);

            }
            this.curLayer = shapeToDraw.curLayer;
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
            double left = Math.Min(startPoint.X, endPoint.X);
            double top = Math.Min(startPoint.Y, endPoint.Y);
            double right = Math.Max(startPoint.X, endPoint.X);
            double bottom = Math.Max(startPoint.Y, endPoint.Y);

            // Tạo điểm góc trái trên và góc phải dưới mới
            StartPoint = new Point(left, top);
            EndPoint = new Point(right, bottom);
        }

        public void CreateBorderSelected()
        {
            if (borderSelected == null)
            {
                borderSelected = (RectangleShape)FactoryShape.CreateShape("Rectangle", "Dash", Brushes.Black, 1, null);
                borderSelected.StartPoint = this.StartPoint;
                borderSelected.EndPoint = this.EndPoint;
                borderSelected.Draw();
                borderSelected.Rotate(curAngle);
            }
        }
        public void RemoveBorderSelected()
        {
            if (borderSelected != null)
            {
                borderSelected.Remove();
                borderSelected = null;
            }
        }

        int changingState = 0;
        public int IsInBorderRectangle(Point point)
        {
            changingState = 0;
            // Vertical
            if (point.X >= borderSelected.StartPoint.X - 5 && point.X <= borderSelected.StartPoint.X + 5)
            {
                if (point.Y >= borderSelected.StartPoint.Y && point.Y <= borderSelected.EndPoint.Y)
                {
                    // Change cusor to resize vertical cursor 
                    canvas.Cursor = Cursors.SizeWE;
                    changingState = 1;




                }
            }
            else if (point.X >= borderSelected.EndPoint.X - 5 && point.X <= borderSelected.EndPoint.X + 5)
            {
                if (point.Y >= borderSelected.StartPoint.Y && point.Y <= borderSelected.EndPoint.Y)
                {
                    // Change cusor to resize vertical cursor
                    canvas.Cursor = Cursors.SizeWE;
                    changingState = 3;
                }
            }
            // Horizontal
            else if (point.Y >= borderSelected.StartPoint.Y - 5 && point.Y <= borderSelected.StartPoint.Y + 5)
            {
                if (point.X >= borderSelected.StartPoint.X && point.X <= borderSelected.EndPoint.X)
                {
                    // Change cusor to resize horizontal cursor
                    canvas.Cursor = Cursors.SizeNS;
                    changingState = 2;

                }
            }
            else if (point.Y >= borderSelected.EndPoint.Y - 5 && point.Y <= borderSelected.EndPoint.Y + 5)
            {
                if (point.X >= borderSelected.StartPoint.X && point.X <= borderSelected.EndPoint.X)
                {
                    // Change cusor to resize horizontal cursor
                    canvas.Cursor = Cursors.SizeNS;
                    changingState = 4;

                }
            }

            // Change cusor to move cursor
            if (changingState == 0)
            {
                canvas.Cursor = Cursors.Hand;
            }
            // Resize of shape





            return changingState;

        }

        public void ResizeShape(Point point)
        {

            if (changingState == 1)
            {
                StartPoint = new Point(point.X, StartPoint.Y);
                UpdateStartAndEndPoint();
            }
            else if (changingState == 2)
            {
                StartPoint = new Point(StartPoint.X, point.Y);
                UpdateStartAndEndPoint();
            }
            else if (changingState == 3)
            {
                EndPoint = new Point(point.X, EndPoint.Y);
                UpdateStartAndEndPoint();
            }
            else if (changingState == 4)
            {
                EndPoint = new Point(EndPoint.X, point.Y);
                UpdateStartAndEndPoint();
            }
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

            if (point.X >= newTopLeft.X && point.X <= newBottomRight.X && point.Y >= newTopLeft.Y && point.Y <= newBottomRight.Y)
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
            if (textBox != null)
            {
                return;
            }
            textBox = FactoryWord.CreateTextBox(backgroundColor, fontsize, color, fontFamily);

            updateTextBoxPosition();

            textBox.TextChanged += (sender, e) =>
            {
                if (textBox.Width < (EndPoint.X - StartPoint.X))
                {
                    textBox.Width = CalculateMaxLettersInLine(textBox.Text) * 10;

                }
                if (textBox.Height < (EndPoint.Y - StartPoint.Y))
                {
                    //Calculate height depending on the number of lines and font size
                    textBox.Height = Math.Max(textBox.LineCount * fontsize * 20 / 12, fontsize * 20 / 12);
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
            if (textBox != null)
            {
                updateTextBoxPosition();
                if (!canvas.Children.Contains(textBox))
                {
                    canvas.Children.Add(textBox);
                }
            }
        }
        virtual public void UpdateEndPoint()
        {

        }
        virtual public void Remove()
        {
            if (textBox != null)
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

        virtual public void Rotate(double angle)
        {

        }

        protected void RotateSelectedBorder()
        {
            if (borderSelected != null)
            {
                borderSelected.Rotate(rotateAngle);
            }
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
            if (borderSelected != null)
            {
                borderSelected.StartPoint = this.StartPoint;
                borderSelected.EndPoint = this.EndPoint;
                borderSelected.UpdateStartAndEndPoint();
            }

            if(textBox != null && textStyleState)
            {
                textBox.BorderBrush = borderColor;
                //textBox.Foreground = fillColor;
                textBox.FontFamily = new FontFamily(fontFamily);
                textBox.FontSize = fontSize;
                textBox.Background = borderColor;
                Style textBoxStyle = new Style(typeof(TextBox));

                textBoxStyle.Setters.Add(new Setter(TextBox.ForegroundProperty, fillColor));


                textBox.Style = textBoxStyle;
            }
            {
                updateTextBoxPosition();
            }
        }
        virtual public void updateTextBoxPosition()
        {
            if (textBox == null)
            {
                return;
            }

            Canvas.SetLeft(textBox, (StartPoint.X + EndPoint.X) / 2 - textBox.Width / 2);
            Canvas.SetTop(textBox, (StartPoint.Y + EndPoint.Y) / 2 - textBox.Height / 2);
        }

    }
    [Serializable]
    public class LineShape : ShapeToDraw
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

            if (stroke != null)
            {
                line.Stroke = this.stroke.borderColor;
                line.StrokeThickness = this.stroke.thickness;
                line.StrokeDashArray = this.stroke.strokeDashArray;
            }
            else
            {
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 1;
            }

            line.X1 = StartPoint.X;
            line.Y1 = StartPoint.Y;
            line.X2 = EndPoint.X;
            line.Y2 = EndPoint.Y;
            if (!canvas.Children.Contains(line))
            {
                canvas.Children.Add(line);
            }
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
            if (stroke != null && textStyleState == false)
            {
                line.Stroke = this.stroke.borderColor;
                line.StrokeThickness = this.stroke.thickness;
                line.StrokeDashArray = this.stroke.strokeDashArray;
            }

            line.X1 = StartPoint.X;
            line.Y1 = StartPoint.Y;
            line.X2 = EndPoint.X;
            line.Y2 = EndPoint.Y;
        }
        public override void Rotate(double angle)
        {
            base.Rotate(angle);
            Point center = new Point((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2);
            // Tính toán góc xoay ở dạng radian
            double radians = angle * Math.PI / 180;

            // Tính toán tọa độ mới cho StartPoint
            double newX1 = Math.Cos(radians) * (StartPoint.X - center.X) - Math.Sin(radians) * (StartPoint.Y - center.Y) + center.X;
            double newY1 = Math.Sin(radians) * (StartPoint.X - center.X) + Math.Cos(radians) * (StartPoint.Y - center.Y) + center.Y;

            // Tính toán tọa độ mới cho EndPoint
            double newX2 = Math.Cos(radians) * (EndPoint.X - center.X) - Math.Sin(radians) * (EndPoint.Y - center.Y) + center.X;
            double newY2 = Math.Sin(radians) * (EndPoint.X - center.X) + Math.Cos(radians) * (EndPoint.Y - center.Y) + center.Y;

            // Áp dụng tọa độ mới
            StartPoint = new Point(newX1, newY1);
            EndPoint = new Point(newX2, newY2);

            // Cập nhật đường thẳng trong canvas
            UpdateStartAndEndPoint();
        }

    }

    [Serializable]
    public class RectangleShape : ShapeToDraw
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
        public override void Rotate(double angle)
        {
            curAngle += (int)angle;
            base.RotateSelectedBorder();

            // Đặt điểm tâm xoay tại tâm của hình chữ nhật
            rectangle.RenderTransformOrigin = new Point(0.5, 0.5);

            // Tạo và áp dụng RotateTransform
            RotateTransform rotateTransform = new RotateTransform(curAngle);
            rectangle.RenderTransform = rotateTransform;
        }
        public override void UpdateEndPoint()
        {
            rectangle.Width = Math.Abs(EndPoint.X - StartPoint.X);
            rectangle.Height = Math.Abs(EndPoint.Y - StartPoint.Y);
            if (EndPoint.X < StartPoint.X)
            {
                Canvas.SetLeft(rectangle, EndPoint.X);
            }
            if (EndPoint.Y < StartPoint.Y)
            {
                Canvas.SetTop(rectangle, EndPoint.Y);
            }
        }
        public void UpdateStartPoint()
        {
            if (EndPoint.X < StartPoint.X)
            {
                Canvas.SetLeft(rectangle, EndPoint.X);
            }
            else
            {
                Canvas.SetLeft(rectangle, StartPoint.X);
            }
            if (EndPoint.Y < StartPoint.Y)
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
            rectangle.Width = Math.Abs(EndPoint.X - StartPoint.X);
            rectangle.Height = Math.Abs(EndPoint.Y - StartPoint.Y);

            if (stroke != null && textStyleState == false)
            {
                rectangle.Stroke = this.stroke.borderColor;
                rectangle.StrokeThickness = this.stroke.thickness;
                rectangle.StrokeDashArray = this.stroke.strokeDashArray;
                rectangle.Fill = this.stroke.fillColor;
            }
            if (EndPoint.X < StartPoint.X)
            {
                Canvas.SetLeft(this.rectangle, EndPoint.X);
            }
            else
            {
                Canvas.SetLeft(this.rectangle, StartPoint.X);
            }
            if (EndPoint.Y < StartPoint.Y)
            {
                Canvas.SetTop(this.rectangle, EndPoint.Y);
            }
            else
            {
                Canvas.SetTop(this.rectangle, StartPoint.Y);
            }
        }
    }

    public class FactoryShape
    {

        public static ShapeToDraw CreateShape(string shapeType, string strokeType, SolidColorBrush borderColor, double thickness, SolidColorBrush fillColor)
        {
            ShapeToDraw curShape = ShapeToDraw.GetPrototype(shapeType);
            Stroke stroke = Stroke.GetPrototype(strokeType);
            if (stroke != null)
            {
                stroke.borderColor = borderColor;
                stroke.thickness = thickness;
                stroke.fillColor = fillColor;
            }
            curShape.stroke = stroke;

            return curShape;

        }
    }
    public class FactoryWord
    {
        public static TextBox CreateTextBox(SolidColorBrush borderColor, int fontSize, SolidColorBrush fillColor, string fontFamily)
        {
            TextBox textBox = new TextBox();
            textBox.Text = "Center Text";
            textBox.Width = 80;
            textBox.Height = 20;
            textBox.BorderBrush = borderColor;
            //textBox.Foreground = fillColor;
            textBox.FontFamily = new FontFamily(fontFamily);
            textBox.FontSize = fontSize;
            textBox.Background = borderColor;
            Style textBoxStyle = new Style(typeof(TextBox));

            textBoxStyle.Setters.Add(new Setter(TextBox.ForegroundProperty, fillColor));


            textBox.Style = textBoxStyle;

            textBox.HorizontalAlignment = HorizontalAlignment.Center;
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.TextAlignment = TextAlignment.Center;
            return textBox;
        }
    }
}
