using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

public class AttemptRepository
{
    // Kreira novi pokušaj kviza
    public int CreateAttempt(int quizId, int? userId)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = @"
                INSERT INTO quiz_attempts(quiz_id, user_id, score)
                VALUES(@qid, @uid, 0);
                SELECT LAST_INSERT_ID();";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@qid", quizId);
                cmd.Parameters.AddWithValue("@uid", (object)userId ?? DBNull.Value);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }

    // Završava pokušaj i sprema bodove
    public void FinishAttempt(int attemptId, int score)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = "UPDATE quiz_attempts SET score=@s, finished_at=NOW() WHERE id=@id;";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@s", score);
                cmd.Parameters.AddWithValue("@id", attemptId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public QuizAttempt GetById(int attemptId)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = @"
                SELECT a.id, a.quiz_id, a.user_id, a.score, a.started_at, a.finished_at,
                       q.title AS quiz_title,
                       COALESCE(u.username, 'Anonimno') AS username
                FROM quiz_attempts a
                JOIN quizzes q ON q.id = a.quiz_id
                LEFT JOIN users u ON u.id = a.user_id
                WHERE a.id = @id;";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", attemptId);
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return null;

                    int? uid = r.IsDBNull(r.GetOrdinal("user_id")) ? (int?)null : r.GetInt32("user_id");

                    return new QuizAttempt
                    {
                        Id = r.GetInt32("id"),
                        QuizId = r.GetInt32("quiz_id"),
                        UserId = uid ?? 0,
                        Username = r.GetString("username"),
                        Score = r.GetInt32("score"),
                        StartedAt = r.GetDateTime("started_at"),
                        FinishedAt = r.IsDBNull(r.GetOrdinal("finished_at")) ? (DateTime?)null : r.GetDateTime("finished_at"),
                        QuizTitle = r.GetString("quiz_title")
                    };
                }
            }
        }
    }

    // ljestvica (top rezultati po kvizu) + username
    public List<QuizAttempt> GetTopResults(int quizId, int limit = 10)
    {
        var list = new List<QuizAttempt>();

        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = @"
                SELECT a.id, a.quiz_id, a.user_id, a.score, a.started_at, a.finished_at,
                       q.title AS quiz_title,
                       COALESCE(u.username, 'Anonimno') AS username
                FROM quiz_attempts a
                JOIN quizzes q ON q.id = a.quiz_id
                LEFT JOIN users u ON u.id = a.user_id
                WHERE a.quiz_id = @qid
                  AND a.finished_at IS NOT NULL
                ORDER BY a.score DESC, a.finished_at ASC
                LIMIT @lim;";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@qid", quizId);
                cmd.Parameters.AddWithValue("@lim", limit);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        int? uid = r.IsDBNull(r.GetOrdinal("user_id")) ? (int?)null : r.GetInt32("user_id");

                        list.Add(new QuizAttempt
                        {
                            Id = r.GetInt32("id"),
                            QuizId = r.GetInt32("quiz_id"),
                            UserId = uid ?? 0,
                            Username = r.GetString("username"),
                            Score = r.GetInt32("score"),
                            StartedAt = r.GetDateTime("started_at"),
                            FinishedAt = r.IsDBNull(r.GetOrdinal("finished_at")) ? (DateTime?)null : r.GetDateTime("finished_at"),
                            QuizTitle = r.GetString("quiz_title")
                        });
                    }
                }
            }
        }

        return list;
    }

    // kratka helper metoda za profil
    public List<QuizAttempt> GetByUser(int userId)
    {
        return GetByUserId(userId, 1000);
    }

    // MOJI REZULTATI (svi pokušaji od usera)
    public List<QuizAttempt> GetByUserId(int userId, int limit = 50)
    {
        var list = new List<QuizAttempt>();

        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = @"
                SELECT a.id, a.quiz_id, a.user_id, a.score, a.started_at, a.finished_at,
                       q.title AS quiz_title, u.username
                FROM quiz_attempts a
                JOIN quizzes q ON q.id = a.quiz_id
                JOIN users u ON u.id = a.user_id
                WHERE a.user_id = @uid
                  AND a.finished_at IS NOT NULL
                ORDER BY a.finished_at DESC
                LIMIT @lim;";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.Parameters.AddWithValue("@lim", limit);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new QuizAttempt
                        {
                            Id = r.GetInt32("id"),
                            QuizId = r.GetInt32("quiz_id"),
                            UserId = r.GetInt32("user_id"),
                            Username = r.GetString("username"),
                            Score = r.GetInt32("score"),
                            StartedAt = r.GetDateTime("started_at"),
                            FinishedAt = r.IsDBNull(r.GetOrdinal("finished_at")) ? (DateTime?)null : r.GetDateTime("finished_at"),
                            QuizTitle = r.GetString("quiz_title")
                        });
                    }
                }
            }
        }

        return list;
    }
}