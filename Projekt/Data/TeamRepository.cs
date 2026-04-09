using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class TeamRepository
{
    public int Insert(Team model)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();

            string sql = @"
                INSERT INTO teams (name, owner_id)
                VALUES (@name, @ownerId);
                SELECT LAST_INSERT_ID();";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@name", model.Name);
                cmd.Parameters.AddWithValue("@ownerId", model.OwnerId);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }

    public Team GetById(int id)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();

            string sql = @"
                SELECT t.id, t.name, t.owner_id, t.created_at, u.username AS OwnerUsername
                FROM teams t
                INNER JOIN users u ON u.id = t.owner_id
                WHERE t.id = @id;";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return null;

                    return new Team
                    {
                        Id = r.GetInt32("id"),
                        Name = r.GetString("name"),
                        OwnerId = r.GetInt32("owner_id"),
                        CreatedAt = r.GetDateTime("created_at"),
                        OwnerUsername = r.GetString("OwnerUsername")
                    };
                }
            }
        }
    }

    public List<Team> GetByOwnerId(int ownerId)
    {
        var list = new List<Team>();

        using (var conn = Db.GetConnection())
        {
            conn.Open();

            string sql = @"
                SELECT t.id, t.name, t.owner_id, t.created_at, u.username AS OwnerUsername
                FROM teams t
                INNER JOIN users u ON u.id = t.owner_id
                WHERE t.owner_id = @ownerId
                ORDER BY t.created_at DESC;";

            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ownerId", ownerId);

                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new Team
                        {
                            Id = r.GetInt32("id"),
                            Name = r.GetString("name"),
                            OwnerId = r.GetInt32("owner_id"),
                            CreatedAt = r.GetDateTime("created_at"),
                            OwnerUsername = r.GetString("OwnerUsername")
                        });
                    }
                }
            }
        }

        return list;
    }
}