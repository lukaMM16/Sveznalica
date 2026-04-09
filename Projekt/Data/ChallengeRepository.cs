using MySql.Data.MySqlClient;
using System.Collections.Generic;

public class ChallengeRepository
{
    public void Insert(Challenge model)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();

            string sql = @"
                INSERT INTO challenges (quiz_id, from_user_id, to_user_id, status)
                VALUES (@quizId, @fromUserId, @toUserId, @status)";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@quizId", model.QuizId);
                cmd.Parameters.AddWithValue("@fromUserId", model.FromUserId);
                cmd.Parameters.AddWithValue("@toUserId", model.ToUserId);
                cmd.Parameters.AddWithValue("@status", model.Status ?? "Pending");
                cmd.ExecuteNonQuery();
            }
        }
    }

    public List<Challenge> GetReceivedByUserId(int userId)
    {
        var list = new List<Challenge>();

        using (var conn = Db.GetConnection())
        {
            conn.Open();

            string sql = @"
                SELECT c.id, c.quiz_id, c.from_user_id, c.to_user_id, c.status, c.created_at,
                       q.title AS QuizTitle,
                       u.username AS FromUsername
                FROM challenges c
                INNER JOIN quizzes q ON q.id = c.quiz_id
                INNER JOIN users u ON u.id = c.from_user_id
                WHERE c.to_user_id = @userId
                ORDER BY c.created_at DESC";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@userId", userId);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new Challenge
                        {
                            Id = r.GetInt32("id"),
                            QuizId = r.GetInt32("quiz_id"),
                            FromUserId = r.GetInt32("from_user_id"),
                            ToUserId = r.GetInt32("to_user_id"),
                            Status = r.GetString("status"),
                            CreatedAt = r.GetDateTime("created_at"),
                            QuizTitle = r.GetString("QuizTitle"),
                            FromUsername = r.GetString("FromUsername")
                        });
                    }
                }
            }
        }

        return list;
    }

    public Challenge GetById(int id)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();

            string sql = @"
                SELECT c.id, c.quiz_id, c.from_user_id, c.to_user_id, c.status, c.created_at,
                       q.title AS QuizTitle
                FROM challenges c
                INNER JOIN quizzes q ON q.id = c.quiz_id
                WHERE c.id = @id";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                using (var r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        return new Challenge
                        {
                            Id = r.GetInt32("id"),
                            QuizId = r.GetInt32("quiz_id"),
                            FromUserId = r.GetInt32("from_user_id"),
                            ToUserId = r.GetInt32("to_user_id"),
                            Status = r.GetString("status"),
                            CreatedAt = r.GetDateTime("created_at"),
                            QuizTitle = r.GetString("QuizTitle")
                        };
                    }
                }
            }
        }

        return null;
    }

    public void UpdateStatus(int id, string status)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();

            string sql = "UPDATE challenges SET status = @status WHERE id = @id";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}