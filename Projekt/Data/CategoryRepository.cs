using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

public class CategoryRepository
{
    public List<Category> GetAll()
    {
        var list = new List<Category>();

        using (var conn = Db.GetConnection())
        {
            conn.Open();
            using (var cmd = new MySqlCommand("SELECT id, name FROM categories ORDER BY name;", conn))
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    list.Add(new Category
                    {
                        Id = r.GetInt32("id"),
                        Name = r.GetString("name")
                    });
                }
            }
        }

        return list;
    }

    public Category GetById(int id)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            using (var cmd = new MySqlCommand("SELECT id, name FROM categories WHERE id=@id;", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return null;

                    return new Category
                    {
                        Id = r.GetInt32("id"),
                        Name = r.GetString("name")
                    };
                }
            }
        }
    }

    public void Insert(Category c)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            using (var cmd = new MySqlCommand("INSERT INTO categories(name) VALUES(@name);", conn))
            {
                cmd.Parameters.AddWithValue("@name", c.Name);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void Update(Category c)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            using (var cmd = new MySqlCommand("UPDATE categories SET name=@name WHERE id=@id;", conn))
            {
                cmd.Parameters.AddWithValue("@name", c.Name);
                cmd.Parameters.AddWithValue("@id", c.Id);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void Delete(int id)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            using (var cmd = new MySqlCommand("DELETE FROM categories WHERE id=@id;", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
    public bool IsUsed(int id)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();

            using (var cmd = new MySqlCommand("SELECT COUNT(*) FROM quizzes WHERE category_id=@id;", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
    }
}
