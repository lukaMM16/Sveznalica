using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

public class AnswerOptionRepository
{
    public List<AnswerOption> GetByQuestionId(int questionId)
    {
        var list = new List<AnswerOption>();
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            using (var cmd = new MySqlCommand(
                "SELECT id, question_id, text, is_correct FROM answer_options WHERE question_id=@qid ORDER BY id ASC;", conn))
            {
                cmd.Parameters.AddWithValue("@qid", questionId);
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new AnswerOption
                        {
                            Id = r.GetInt32("id"),
                            QuestionId = r.GetInt32("question_id"),
                            Text = r.GetString("text"),
                            IsCorrect = r.GetBoolean("is_correct")
                        });
                    }
                }
            }
        }
        return list;
    }

    public void ReplaceAll(int questionId, List<AnswerOption> options)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();

            using (var del = new MySqlCommand("DELETE FROM answer_options WHERE question_id=@qid;", conn))
            {
                del.Parameters.AddWithValue("@qid", questionId);
                del.ExecuteNonQuery();
            }

            foreach (var o in options)
            {
                using (var ins = new MySqlCommand(
                    "INSERT INTO answer_options(question_id, text, is_correct) VALUES(@qid,@t,@c);", conn))
                {
                    ins.Parameters.AddWithValue("@qid", questionId);
                    ins.Parameters.AddWithValue("@t", o.Text);
                    ins.Parameters.AddWithValue("@c", o.IsCorrect ? 1 : 0);
                    ins.ExecuteNonQuery();
                }
            }
        }
    }
}
