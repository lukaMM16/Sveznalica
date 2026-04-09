using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

public class QuestionRepository
{
    public List<Question> GetAll()
    {
        var list = new List<Question>();
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = @"
                SELECT q.id, q.quiz_id, q.text, q.points, z.title AS quiz_title
                FROM questions q
                JOIN quizzes z ON z.id = q.quiz_id
                ORDER BY q.id DESC;";
            using (var cmd = new MySqlCommand(sql, conn))
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    list.Add(new Question
                    {
                        Id = r.GetInt32("id"),
                        QuizId = r.GetInt32("quiz_id"),
                        Text = r.GetString("text"),
                        Points = r.GetInt32("points"),
                        QuizTitle = r.GetString("quiz_title")
                    });
                }
            }
        }
        return list;
    }

    public List<Question> GetByQuizId(int quizId)
    {
        var list = new List<Question>();
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = @"SELECT id, quiz_id, text, points FROM questions WHERE quiz_id=@qid ORDER BY id ASC;";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@qid", quizId);
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new Question
                        {
                            Id = r.GetInt32("id"),
                            QuizId = r.GetInt32("quiz_id"),
                            Text = r.GetString("text"),
                            Points = r.GetInt32("points")
                        });
                    }
                }
            }
        }
        return list;
    }

    public Question GetById(int id)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = @"
                SELECT q.id, q.quiz_id, q.text, q.points, z.title AS quiz_title
                FROM questions q
                JOIN quizzes z ON z.id = q.quiz_id
                WHERE q.id=@id;";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return null;
                    return new Question
                    {
                        Id = r.GetInt32("id"),
                        QuizId = r.GetInt32("quiz_id"),
                        Text = r.GetString("text"),
                        Points = r.GetInt32("points"),
                        QuizTitle = r.GetString("quiz_title")
                    };
                }
            }
        }
    }

    public int Insert(Question q)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = "INSERT INTO questions(quiz_id, text, points) VALUES(@qid,@t,@p); SELECT LAST_INSERT_ID();";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@qid", q.QuizId);
                cmd.Parameters.AddWithValue("@t", q.Text);
                cmd.Parameters.AddWithValue("@p", q.Points);
                return System.Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }

    public void Update(Question q)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            using (var cmd = new MySqlCommand(
                "UPDATE questions SET quiz_id=@qid, text=@t, points=@p WHERE id=@id;", conn))
            {
                cmd.Parameters.AddWithValue("@qid", q.QuizId);
                cmd.Parameters.AddWithValue("@t", q.Text);
                cmd.Parameters.AddWithValue("@p", q.Points);
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

            // prvo obriši odgovore
            using (var cmd = new MySqlCommand("DELETE FROM answer_options WHERE question_id=@id;", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            using (var cmd = new MySqlCommand("DELETE FROM questions WHERE id=@id;", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
