using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

public class AttemptAnswerRepository
{
    public void SaveAnswer(int attemptId, int questionId, int? selectedOptionId, bool isCorrect)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();

            // da ne podupla ako netko refresha 
            var sql = @"
                INSERT INTO attempt_answers(attempt_id, question_id, selected_option_id, is_correct)
                VALUES(@aid, @qid, @sid, @ok)
                ON DUPLICATE KEY UPDATE
                    selected_option_id = VALUES(selected_option_id),
                    is_correct = VALUES(is_correct);";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@aid", attemptId);
                cmd.Parameters.AddWithValue("@qid", questionId);
                cmd.Parameters.AddWithValue("@sid", (object)selectedOptionId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ok", isCorrect ? 1 : 0);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public List<AttemptAnswerRow> GetAttemptReview(int attemptId)
    {
        var list = new List<AttemptAnswerRow>();

        using (var conn = Db.GetConnection())
        {
            conn.Open();

            var sql = @"
                SELECT
                    q.id AS question_id,
                    q.text AS question_text,
                    q.points,
                    aa.selected_option_id,
                    so.text AS selected_text,
                    co.id AS correct_option_id,
                    co.text AS correct_text,
                    aa.is_correct
                FROM attempt_answers aa
                JOIN questions q ON q.id = aa.question_id
                LEFT JOIN answer_options so ON so.id = aa.selected_option_id
                JOIN answer_options co ON co.question_id = q.id AND co.is_correct = 1
                WHERE aa.attempt_id = @aid
                ORDER BY q.id;";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@aid", attemptId);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new AttemptAnswerRow
                        {
                            QuestionId = r.GetInt32("question_id"),
                            QuestionText = r.GetString("question_text"),
                            Points = r.GetInt32("points"),
                            SelectedOptionId = r.IsDBNull(r.GetOrdinal("selected_option_id")) ? (int?)null : r.GetInt32("selected_option_id"),
                            SelectedText = r.IsDBNull(r.GetOrdinal("selected_text")) ? "(nije odabrano)" : r.GetString("selected_text"),
                            CorrectOptionId = r.GetInt32("correct_option_id"),
                            CorrectText = r.GetString("correct_text"),
                            IsCorrect = r.GetInt32("is_correct") == 1
                        });
                    }
                }
            }
        }

        return list;
    }
}
