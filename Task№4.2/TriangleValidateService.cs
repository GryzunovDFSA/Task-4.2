using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4._2
{
    public class TriangleValidateService : ITriangleValidateService
    {
        private readonly ITriangleService triangleService;
        private readonly ITriangleProvider triangleProvider;

        public TriangleValidateService(ITriangleProvider triangleProvider, ITriangleService triangleService)
        {
            this.triangleProvider = triangleProvider;
            this.triangleService = triangleService;
        }

        public bool IsAllValid()
        {
            List<Triangle> list = triangleProvider.GetAll();
            foreach (Triangle item in list)
            {
                TriangleCheckup(item);
                if (item.IsValidType == true)
                    continue;
                else return false;
            }
            return true;
        }

        public bool IsValid(int id)
        {
            Triangle triangle = triangleProvider.GetById(id);
            TriangleCheckup(triangle);
            return triangle.IsValidType;
        }

        private void TriangleCheckup(Triangle triangle)
        {
            if (triangleService.IsValidTriangle(triangle.A, triangle.B, triangle.C) == true)
            {
                if (triangleService.GetType(triangle.A, triangle.B, triangle.C) == triangle.Type)
                {
                    if (triangleService.GetArea(triangle.A, triangle.B, triangle.C).Equals(triangle.Area))
                        triangle.IsValidType = true;
                    else triangle.IsValidType = false;
                }
                else triangle.IsValidType = false;
            }
            else triangle.IsValidType = false;
        }
    }
}
