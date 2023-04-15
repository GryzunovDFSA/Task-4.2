using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4._2
{
    public class TriangleService : ITriangleService
    {
        public double GetArea(double a, double b, double c)
        {
            double p = (a + b + c) / 2;
            return Math.Round(Math.Sqrt(p * (p - a) * (p - b) * (p - c)), 3);
        }

        public bool IsValidTriangle(double a, double b, double c)
        {
            if (a > 0 && b > 0 && c > 0)
            {
                if (a < b + c && b < a + c && c < a + b)
                    return true;
                else return false;
            }
            else return false;
        }

        public TriangleType GetType(double a, double b, double c)
        {
            TriangleType resultSEI, resultROA;

            if (a != b && b != c && a != c)
                resultSEI = TriangleType.Scalene;
            else
            {
                if (a == b && b == c && a == c)
                    resultSEI = TriangleType.Equilateral;
                else resultSEI = TriangleType.Isosceles;
            }

            if (a * a == (b * b + c * c) || b * b == (a * a + c * c) || c * c == (b * b + a * a))
                resultROA = TriangleType.Right;
            else
            {
                if (a * a > (b * b + c * c) || b * b > (a * a + c * c) || c * c > (b * b + a * a))
                    resultROA = TriangleType.Obtuse;
                else resultROA = TriangleType.Acute;
            }

            return resultSEI | resultROA;
        }

        public void Save(double a, double b, double c, TriangleType type, double area)
        {
            int count = 0;
            Triangle triangle = new Triangle(count, a, b, c, type, area);
        }
    }
}
