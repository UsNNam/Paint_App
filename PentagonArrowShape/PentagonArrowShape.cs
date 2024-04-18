
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
using MyArrow;
using Shapes;
namespace PentagonArrowShape
{
    [Serializable]
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
            base.UpdateStartAndEndPoint();

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


}
