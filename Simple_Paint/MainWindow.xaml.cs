using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simple_Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ShapeToDraw curShape;
        bool isDraw = false;
        public static SolidColorBrush fillColorMain { get; set; }

        public static SolidColorBrush borderColorMain { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ShapeToDraw.canvas = canvas;
            curShape = new LineShape(new Point(0, 0), new Point(0, 0));
        }

        private void LineButton_Click(object sender, RoutedEventArgs e)
        {
            curShape = new LineShape(new Point(0, 0), new Point(0, 0));
            if(fillColorMain == null)
            {
                ShapeToDraw.fillColor = Brushes.Black;
            }
            else {                 
                ShapeToDraw.fillColor = fillColorMain;
                       
            }
           
            if(borderColorMain == null)
            {
                ShapeToDraw.borderColor = Brushes.Black;
            }
            else
            {
                ShapeToDraw.borderColor = borderColorMain;
            }
        
        }
        private void EllipseButton_Click(object sender, RoutedEventArgs e)
        {
            curShape = new EllipseShape(new Point(0, 0), new Point(0, 0));
            if (fillColorMain == null)
            {
                ShapeToDraw.fillColor = Brushes.Black;
            }
            else
            {
                ShapeToDraw.fillColor = fillColorMain;

            }

            if(borderColorMain == null)
            {
                ShapeToDraw.borderColor = Brushes.Black;
            }
            else
            {
                ShapeToDraw.borderColor = borderColorMain;
            }
        }
        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            curShape = new RectangleShape(new Point(0, 0), new Point(0, 0));
            if (fillColorMain == null)
            {
                ShapeToDraw.fillColor = Brushes.Black;
            }
            else
            {
                ShapeToDraw.fillColor = fillColorMain;

            }

            if(borderColorMain == null)
            {
                ShapeToDraw.borderColor = Brushes.Black;
            }
            else
            {
                ShapeToDraw.borderColor = borderColorMain;
            }
        }

        private void TriangleButton_Click(object sender, RoutedEventArgs e)
        {
            curShape = new TriangleShape(new Point(0, 0), new Point(0, 0));
            if (fillColorMain == null)
            {
                ShapeToDraw.fillColor = Brushes.Black;
            }
            else
            {
                ShapeToDraw.fillColor = fillColorMain;

            }

            if(borderColorMain == null)
            {
                ShapeToDraw.borderColor = Brushes.Black;
            }
            else
            {
                ShapeToDraw.borderColor = borderColorMain;
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed && isDraw == false)
            {
                
                Point point = e.GetPosition(canvas);
                if(point != null) {
                    curShape.StartPoint = point;
                    curShape.EndPoint = point;
                    curShape.Draw();
                    isDraw = true;
                }
                
            }
            

        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Released && isDraw)
            {
                curShape.EndPoint = e.GetPosition(canvas);
                isDraw = false;
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraw)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (curShape is EllipseShape || curShape is RectangleShape)
                    {
                        // I want the endpoint make the shape into circle

                        Point curPoint = e.GetPosition(canvas);
                        // Tính toán độ dài cạnh của hình vuông
                        double sideLength = Math.Min(Math.Abs(curPoint.X - curShape.StartPoint.X), Math.Abs(curPoint.Y - curShape.StartPoint.Y));

                        // Tính toán tọa độ của endPoint
                        double endX = curShape.StartPoint.X + (curPoint.X > curShape.StartPoint.X ? sideLength : -sideLength);
                        double endY = curShape.StartPoint.Y + (curPoint.Y > curShape.StartPoint.Y ? sideLength : -sideLength);

                        // Tạo ra endPoint từ tọa độ tính toán được
                        Point endPoint = new Point(endX, endY);

                        curShape.EndPoint = endPoint;
                        curShape.UpdateEndPoint();


                    }

                }
                else
                {
                    curShape.EndPoint = e.GetPosition(canvas);
                    curShape.UpdateEndPoint();
                }
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FillColor_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.IsChecked == true)
            {
                ShapeToDraw.fillColor = (SolidColorBrush)radioButton.Tag;
                fillColorMain = (SolidColorBrush)radioButton.Tag;
            }
            if (curShape != null)
            {
                
            }
        }

        private void BorderColor_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.IsChecked == true)
            {
                ShapeToDraw.borderColor = (SolidColorBrush)radioButton.Tag;
                borderColorMain = (SolidColorBrush)radioButton.Tag;
            }
            if (curShape != null)
            {
                
            }
        }
    }


}