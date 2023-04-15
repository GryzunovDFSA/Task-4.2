using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using Task_4._2;

namespace UnitTest4._2
{
    [TestClass]
    public class TestTriangles
    {
        private List<Triangle> trueTriangles = new List<Triangle>
        {
            new Triangle (1, 14, 9, 14, TriangleType.Isosceles|TriangleType.Acute, 59.657, true),
            new Triangle (2, 2.3, 3.2, 4.2, TriangleType.Scalene|TriangleType.Obtuse, 5.071, true),
            new Triangle (3, 14, 18, 23, TriangleType.Scalene|TriangleType.Obtuse, 125.980, true),
            new Triangle (4, 9.14, 9.14, 9.14, TriangleType.Equilateral|TriangleType.Acute,36.174, true)
        };

        private List<Triangle> falseTriangles = new List<Triangle>
        {
            new Triangle (1, 14, 9, 14, TriangleType.Isosceles|TriangleType.Acute, 59.657, false),
            new Triangle (2, 2.3, 3.2, 4.2, TriangleType.Scalene|TriangleType.Obtuse, 5.071, false),
            new Triangle (3, 14, 18, 23, TriangleType.Scalene|TriangleType.Obtuse, 125.980, false),
            new Triangle (4, 9.14, 9.14, 9.14, TriangleType.Equilateral|TriangleType.Acute,36.174, false)
        };

        private List<Triangle> Triangles_2Invalid_2Valid = new List<Triangle>
        {
            new Triangle (1, 14, 9, 14, TriangleType.Scalene|TriangleType.Obtuse, 59.657),
            new Triangle (2, 2.3, 3.2, 4.2, TriangleType.Isosceles|TriangleType.Acute, 5.071),
            new Triangle (3, 14, 18, 23, TriangleType.Scalene|TriangleType.Obtuse, 125.980),
            new Triangle (4, 9.14, 9.14, 9.14, TriangleType.Equilateral|TriangleType.Acute,36.174)
        };

        private List<Triangle> Triangles_Invalid_Valid = new List<Triangle>
        {
            new Triangle (3, 14, 18, 23, TriangleType.Scalene|TriangleType.Acute, 0),
            new Triangle (4, 9.14, 9.14, 9.14, TriangleType.Equilateral|TriangleType.Acute,36.174)
        };

        private ITriangleProvider triangleProvider;
        private ITriangleService triangleService;
        private ITriangleValidateService triangleValidateService;

        [TestInitialize]
        public void TestInitialize()
        {
            string connectionstr = "Data Source=C:\\Users\\Пользователь\\Desktop\\ПТ ПМ\\Task№4.2\\DBtriangles.db";
            SQLiteConnection connection = new SQLiteConnection(connectionstr);

            triangleService = new TriangleService();
            triangleProvider = new TriangleProvider(connectionstr);
            triangleValidateService = new TriangleValidateService(triangleProvider, triangleService);
        }

        [TestMethod]
        public void TriangleProvider_IsAllValid_True()
        {
            foreach (Triangle triangle in trueTriangles)
            {
                triangleProvider.Save(triangle);
            }

            var result = triangleValidateService.IsAllValid();

            Assert.IsTrue(triangleProvider.GetById(1).isValid);
            Assert.IsTrue(triangleProvider.GetById(2).isValid);
            Assert.IsTrue(triangleProvider.GetById(3).isValid);
            Assert.IsTrue(triangleProvider.GetById(4).isValid);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TriangleProvider_IsAllValid_False()
        {
            foreach (Triangle triangle in falseTriangles)
            {
                triangleProvider.Save(triangle);
            }

            var result = triangleValidateService.IsAllValid();

            Assert.IsFalse(triangleProvider.GetById(1).isValid);
            Assert.IsFalse(triangleProvider.GetById(2).isValid);
            Assert.IsFalse(triangleProvider.GetById(3).isValid);
            Assert.IsFalse(triangleProvider.GetById(4).isValid);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TriangleProvider_Is1Valid_True()
        {
            triangleProvider.Save(trueTriangles[0]);
            var result = triangleValidateService.IsValid(1);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TriangleProvider_Is2Valid_False()
        {
            triangleProvider.Save(falseTriangles[1]);
            var result = triangleValidateService.IsValid(2);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TriangleProvider_IsAllValid_Invalid_Valid()
        {
            foreach (Triangle triangle in Triangles_Invalid_Valid)
            {
                triangleProvider.Save(triangle);
            }

            var result = triangleValidateService.IsAllValid();

            Assert.IsTrue(Triangles_Invalid_Valid[0].isValid);
            Assert.IsFalse(Triangles_Invalid_Valid[1].isValid);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TriangleProvider_IsAllValid_2Invalid_2Valid()
        {
            foreach (Triangle triangle in Triangles_2Invalid_2Valid)
            {
                triangleProvider.Save(triangle);
            }

            var result = triangleValidateService.IsAllValid();

            Assert.IsFalse(Triangles_2Invalid_2Valid[0].isValid);
            Assert.IsFalse(Triangles_2Invalid_2Valid[1].isValid);
            Assert.IsTrue(Triangles_2Invalid_2Valid[2].isValid);
            Assert.IsTrue(Triangles_2Invalid_2Valid[3].isValid);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TriangleProvider_IsValid_True_False_True()
        {
            triangleProvider.Save(trueTriangles[2]);
            Assert.IsTrue(triangleValidateService.IsValid(3));
            Assert.IsTrue(triangleProvider.GetById(3).IsValidType);

            Triangle triangleOld = triangleProvider.GetById(3);
            Triangle triangleNew = triangleProvider.GetById(3);
            triangleNew.a = 45.234;

            triangleProvider.Save(triangleNew);
            Assert.IsFalse(triangleValidateService.IsValid(3));
            Assert.IsFalse(triangleProvider.GetById(3).IsValidType);

            triangleProvider.Save(triangleOld);
            Assert.IsTrue(triangleValidateService.IsValid(3));
            Assert.IsTrue(triangleProvider.GetById(3).IsValidType);
        }

        [TestMethod]
        public void TriangleProvider_IsValid_False_True_False()
        {
            triangleProvider.Save(falseTriangles[0]);
            Assert.IsFalse(triangleValidateService.IsValid(1));
            Assert.IsFalse(triangleProvider.GetById(1).IsValidType);

            Triangle triangleOld = triangleProvider.GetById(1);
            Triangle triangleNew = triangleProvider.GetById(1);
            triangleNew.area = triangleService.GetArea(triangleNew.a, triangleNew.b, triangleNew.c);

            triangleProvider.Save(triangleNew);
            Assert.IsTrue(triangleValidateService.IsValid(1));
            Assert.IsTrue(triangleProvider.GetById(1).IsValidType);

            triangleProvider.Save(triangleOld);
            Assert.IsFalse(triangleValidateService.IsValid(1));
            Assert.IsFalse(triangleProvider.GetById(1).IsValidType);
        }

        [TestMethod]
        public void TriangleProvider_IsAllValid_True_False_True()
        {
            foreach (Triangle triangle in trueTriangles)
            {
                triangleProvider.Save(triangle);
            }
            Assert.IsTrue(triangleValidateService.IsAllValid());

            var lastA1 = triangleProvider.GetById(1).a;
            var lastA2 = triangleProvider.GetById(2).a;
            var lastA3 = triangleProvider.GetById(3).a;
            var lastA4 = triangleProvider.GetById(4).a;
            var triangle1 = triangleProvider.GetById(1);
            var triangle2 = triangleProvider.GetById(2);
            var triangle3 = triangleProvider.GetById(3);
            var triangle4 = triangleProvider.GetById(4);
            triangle1.a = triangle2.a = triangle3.a = triangle4.a = 0;
            triangleProvider.Save(triangle1);
            triangleProvider.Save(triangle2);
            triangleProvider.Save(triangle3);
            triangleProvider.Save(triangle4);
            Assert.IsFalse(triangleValidateService.IsAllValid());

            triangle1.a = lastA1;
            triangle2.a = lastA2;
            triangle3.a = lastA3;
            triangle4.a = lastA4;
            triangleProvider.Save(triangle1);
            triangleProvider.Save(triangle2);
            triangleProvider.Save(triangle3);
            triangleProvider.Save(triangle4);
            Assert.IsTrue(triangleValidateService.IsAllValid());
        }

        [TestMethod]
        public void TriangleProvider_IsAllValid_False_True_False()
        {
            foreach (Triangle triangle in falseTriangles)
            {
                triangleProvider.Save(triangle);
            }
            Assert.IsFalse(triangleValidateService.IsAllValid());

            var lastType1 = triangleProvider.GetById(1).type;
            var lastType2 = triangleProvider.GetById(2).type;
            var lastArea3 = triangleProvider.GetById(3).area;
            var lastA4 = triangleProvider.GetById(4).a;
            var triangle1 = triangleProvider.GetById(1);
            var triangle2 = triangleProvider.GetById(2);
            var triangle3 = triangleProvider.GetById(3);
            var triangle4 = triangleProvider.GetById(4);

            triangle1.type = triangleService.GetType(triangle1.a, triangle1.b, triangle1.c);
            triangle2.type = triangleService.GetType(triangle2.a, triangle2.b, triangle2.c);
            triangle3.area = triangleService.GetArea(triangle3.a, triangle3.b, triangle3.c);
            triangle4.a = 2.4;
            triangleProvider.Save(triangle1);
            triangleProvider.Save(triangle2);
            triangleProvider.Save(triangle3);
            triangleProvider.Save(triangle4);
            Assert.IsTrue(triangleValidateService.IsAllValid());

            triangle1.type = lastType1;
            triangle2.type = lastType2;
            triangle3.area = lastArea3;
            triangle4.a = lastA4;
            triangleProvider.Save(triangle1);
            triangleProvider.Save(triangle2);
            triangleProvider.Save(triangle3);
            triangleProvider.Save(triangle4);
            Assert.IsFalse(triangleValidateService.IsAllValid());
        }

        [TestCleanup]
        public void TestCleanup()
        {
            string connectionstr = "Data Source=C:\\Users\\Пользователь\\Desktop\\ПТ ПМ\\Task№4.2\\DBtriangles.db";
            SQLiteConnection connection = new SQLiteConnection(connectionstr);

            string deleteAllQuery = "DELETE FROM triangles";

            using (SQLiteCommand command = new SQLiteCommand(deleteAllQuery, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
