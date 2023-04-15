using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Task_4._2
{
    public class TriangleProvider : ITriangleProvider
    {

        private readonly string connectionstr;

        public TriangleProvider(string connectionstr)
        {
            this.connectionstr = connectionstr;
        }

        public Triangle GetById(int id)
        {
            using (var connection = new SQLiteConnection(connectionstr))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM triangles WHERE [id] = @id";
                command.Parameters.Add(new SQLiteParameter("@id", id));
                SQLiteDataReader reader = command.ExecuteReader();

                using (command)
                using (reader)
                {
                    reader.Read();

                    var a = reader.GetDouble(1);
                    var b = reader.GetDouble(2);
                    var c = reader.GetDouble(3);
                    var type = (TriangleType)Enum.Parse(typeof(TriangleType), reader.GetString(4));
                    var area = reader.GetDouble(5);
                    var isValid = reader.GetBoolean(6);

                    return new Triangle(id, a, b, c, type, area, isValid);
                }
            }
        }

        public Triangle GetBySides(double a, double b, double c)
        {
            using (var connection = new SQLiteConnection(connectionstr))
            {
                connection.Open();

                using (var command = new SQLiteCommand($"SELECT * FROM triangles WHERE [a] = {a} AND [b] = {b} AND [c] = {c}", connection))
                using (var reader = command.ExecuteReader())
                {
                    reader.Read();

                    var id = reader.GetInt32(0);
                    var type = (TriangleType)Enum.Parse(typeof(TriangleType), reader.GetString(4));
                    var area = reader.GetDouble(5);
                    var isValid = reader.GetBoolean(6);

                    return new Triangle(id, a, b, c, type, area, isValid);
                }
            }
        }

        public List<Triangle> GetAll()
        {
            var triangles = new List<Triangle>();

            using (var connection = new SQLiteConnection(connectionstr))
            {
                connection.Open();

                using (var command = new SQLiteCommand("SELECT * FROM triangles", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var a = reader.GetDouble(1);
                        var b = reader.GetDouble(2);
                        var c = reader.GetDouble(3);
                        var type = (TriangleType)Enum.Parse(typeof(TriangleType), reader.GetString(4));
                        var area = reader.GetDouble(5);
                        var isValid = reader.GetBoolean(6);

                        triangles.Add(new Triangle(id, a, b, c, type, area, isValid));
                    }
                }
            }

            return triangles;
        }

        public void Save(Triangle triangle)
        {
            using (var connection = new SQLiteConnection(connectionstr))
            {
                connection.Open();

                using (var selectCommand = new SQLiteCommand($"SELECT COUNT(*) FROM triangles WHERE [id] = {triangle.id}", connection))
                {
                    var count = (long)selectCommand.ExecuteScalar();
                    if (count == 0)
                    {
                        using (var insertCommand = new SQLiteCommand("INSERT INTO triangles (id, a, b, c, type, area, isValid) VALUES (@id, @a, @b, @c, @type, @area, @isValid)", connection))
                        {
                            insertCommand.Parameters.AddWithValue("@id", triangle.id);
                            insertCommand.Parameters.AddWithValue("@a", triangle.a);
                            insertCommand.Parameters.AddWithValue("@b", triangle.b);
                            insertCommand.Parameters.AddWithValue("@c", triangle.c);
                            insertCommand.Parameters.AddWithValue("@type", triangle.type.ToString());
                            insertCommand.Parameters.AddWithValue("@area", triangle.area);
                            insertCommand.Parameters.AddWithValue("@isValid", triangle.isValid);

                            insertCommand.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (var updateCommand = new SQLiteCommand("UPDATE triangles SET a = @a, b = @b, c = @c, type = @type, area = @area, isValid = @isValid WHERE id = @id", connection))
                        {
                            updateCommand.Parameters.AddWithValue("@id", triangle.id);
                            updateCommand.Parameters.AddWithValue("@a", triangle.a);
                            updateCommand.Parameters.AddWithValue("@b", triangle.b);
                            updateCommand.Parameters.AddWithValue("@c", triangle.c);
                            updateCommand.Parameters.AddWithValue("@type", triangle.type.ToString());
                            updateCommand.Parameters.AddWithValue("@area", triangle.area);
                            updateCommand.Parameters.AddWithValue("@isValid", triangle.isValid);

                            updateCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
