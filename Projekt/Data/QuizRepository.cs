using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;


public class QuizRepository
{
    public List<Quiz> GetAll()
    {
        var list = new List<Quiz>();
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = @"
                SELECT q.id, q.title, q.category_id, q.difficulty, q.time_limit_sec, c.name AS category_name
                FROM quizzes q
                JOIN categories c ON c.id = q.category_id
                ORDER BY q.id DESC;";
            using (var cmd = new MySqlCommand(sql, conn))
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    list.Add(new Quiz
                    {
                        Id = r.GetInt32("id"),
                        Title = r.GetString("title"),
                        CategoryId = r.GetInt32("category_id"),
                        Difficulty = r.GetInt32("difficulty"),
                        TimeLimitSec = r.GetInt32("time_limit_sec"),
                        CategoryName = r.GetString("category_name")
                    });
                }
            }
        }
        return list;
    }

    public Quiz GetById(int id)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = @"
                SELECT q.id, q.title, q.category_id, q.difficulty, q.time_limit_sec, c.name AS category_name
                FROM quizzes q
                JOIN categories c ON c.id = q.category_id
                WHERE q.id=@id;";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return null;
                    return new Quiz
                    {
                        Id = r.GetInt32("id"),
                        Title = r.GetString("title"),
                        CategoryId = r.GetInt32("category_id"),
                        Difficulty = r.GetInt32("difficulty"),
                        TimeLimitSec = r.GetInt32("time_limit_sec"),
                        CategoryName = r.GetString("category_name")
                    };
                }
            }
        }
    }

    public void Insert(Quiz q)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            using (var cmd = new MySqlCommand(
                "INSERT INTO quizzes(title, category_id, difficulty, time_limit_sec) VALUES(@t,@c,@d,@tl);", conn))
            {
                cmd.Parameters.AddWithValue("@t", q.Title);
                cmd.Parameters.AddWithValue("@c", q.CategoryId);
                cmd.Parameters.AddWithValue("@d", q.Difficulty);
                cmd.Parameters.AddWithValue("@tl", q.TimeLimitSec);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void Update(Quiz q)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            using (var cmd = new MySqlCommand(
                "UPDATE quizzes SET title=@t, category_id=@c, difficulty=@d, time_limit_sec=@tl WHERE id=@id;", conn))
            {
                cmd.Parameters.AddWithValue("@t", q.Title);
                cmd.Parameters.AddWithValue("@c", q.CategoryId);
                cmd.Parameters.AddWithValue("@d", q.Difficulty);
                cmd.Parameters.AddWithValue("@tl", q.TimeLimitSec);
                cmd.Parameters.AddWithValue("@id", q.Id);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void Delete(int id)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();

            using (var tx = conn.BeginTransaction())
            {
                try
                {
                    // 1 Obriši odgovore (answer_options) za sva pitanja 
                    using (var cmd = new MySqlCommand(@"
                    DELETE ao
                    FROM answer_options ao
                    JOIN questions q ON q.id = ao.question_id
                    WHERE q.quiz_id = @qid;", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@qid", id);
                        cmd.ExecuteNonQuery();
                    }

                    // 2 Obriši pitanja za taj kviz
                    using (var cmd = new MySqlCommand(
                        "DELETE FROM questions WHERE quiz_id = @qid;", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@qid", id);
                        cmd.ExecuteNonQuery();
                    }

                    // 3) obriši pokušaje (attempts) za taj kviz 
                    using (var cmd = new MySqlCommand(
                        "DELETE FROM quiz_attempts WHERE quiz_id = @qid;", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@qid", id);
                        cmd.ExecuteNonQuery();
                    }

                    // 4)  sad briši kviz
                    using (var cmd = new MySqlCommand(
                        "DELETE FROM quizzes WHERE id = @qid;", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@qid", id);
                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }

}
