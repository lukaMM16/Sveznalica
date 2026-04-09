using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class TeamMemberRepository
{
    public void Insert(int teamId, int userId)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();

            string sql = @"
                INSERT INTO team_members (team_id, user_id)
                VALUES (@teamId, @userId);";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@teamId", teamId);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public List<TeamMember> GetByTeamId(int teamId)
    {
        var list = new List<TeamMember>();

        using (var conn = Db.GetConnection())
        {
            conn.Open();

            string sql = @"
                SELECT tm.id, tm.team_id, tm.user_id, tm.joined_at, u.username
                FROM team_members tm
                INNER JOIN users u ON u.id = tm.user_id
                WHERE tm.team_id = @teamId
                ORDER BY tm.joined_at ASC;";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@teamId", teamId);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new TeamMember
                        {
                            Id = r.GetInt32("id"),
                            TeamId = r.GetInt32("team_id"),
                            UserId = r.GetInt32("user_id"),
                            JoinedAt = r.GetDateTime("joined_at"),
                            Username = r.GetString("username")
                        });
                    }
                }
            }
        }

        return list;
    }

    public bool Exists(int teamId, int userId)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();

            string sql = @"
                SELECT COUNT(*)
                FROM team_members
                WHERE team_id = @teamId AND user_id = @userId;";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@teamId", teamId);
                cmd.Parameters.AddWithValue("@userId", userId);

                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
    }
}