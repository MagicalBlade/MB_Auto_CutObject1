using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Tekla.Structures.Geometry3d;
using TSG = Tekla.Structures.Geometry3d;

namespace MB_Auto_CutObject1.Classes
{
    internal class DifferencePoints : IComparable
    {
        private TSG.Point _point1;
        private TSG.Point _point2;
        private double _difference;

        public TSG.Point Point1 { get => _point1; set => _point1 = value; }

        public TSG.Point Point2 { get => _point2; set => _point2 = value; }

        public double Difference { get => _difference; set => _difference = value; }

        public DifferencePoints(Point point1, Point point2)
        {
            Point1 = point1;
            Point2 = point2;
            Difference = Math.Abs((Point1.X - Point2.X) + (Point1.Y - Point2.Y) + (Point1.Z - Point2.Z));
        }

        public int CompareTo(object obj)
        {
            if (obj is DifferencePoints differencePoints)
            {
                return Difference.CompareTo(differencePoints.Difference);
            }
            else { throw new ArgumentException("Некорректное значение параметра"); }
            
        }
    }
}
