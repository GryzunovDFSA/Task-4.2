using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4._2
{
    public interface ITriangleProvider
    {
        Triangle GetById(int id);
        Triangle GetBySides(double a, double b, double c);
        List<Triangle> GetAll();
        void Save(Triangle triangle);
    }
}
