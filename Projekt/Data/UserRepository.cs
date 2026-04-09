using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class UserRepository
{
    public User GetByUsername(string username)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = "SELECT id, username, password_hash, role, avatar_url FROM users WHERE username=@u;";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return null;

                    return new User
                    {
                        Id = r.GetInt32("id"),
                        Username = r.GetString("username"),
                        PasswordHash = r.GetString("password_hash"),
                        Role = r.GetString("role"),
                        AvatarUrl = r.IsDBNull(r.GetOrdinal("avatar_url")) ? null : r.GetString("avatar_url")
                    };
                }
            }
        }
    }

    public User GetById(int id)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = "SELECT id, username, password_hash, role, avatar_url FROM users WHERE id=@id;";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return null;

                    return new User
                    {
                        Id = r.GetInt32("id"),
                        Username = r.GetString("username"),
                        PasswordHash = r.GetString("password_hash"),
                        Role = r.GetString("role"),
                        AvatarUrl = r.IsDBNull(r.GetOrdinal("avatar_url")) ? null : r.GetString("avatar_url")
                    };
                }
            }
        }
    }

    public List<User> GetAll()
    {
        var users = new List<User>();

        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = "SELECT id, username, password_hash, role, avatar_url FROM users ORDER BY username ASC;";
            using (var cmd = new MySqlCommand(sql, conn))
            using (var r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    users.Add(new User
                    {
                        Id = r.GetInt32("id"),
                        Username = r.GetString("username"),
                        PasswordHash = r.GetString("password_hash"),
                        Role = r.GetString("role"),
                        AvatarUrl = r.IsDBNull(r.GetOrdinal("avatar_url")) ? null : r.GetString("avatar_url")
                    });
                }
            }
        }

        return users;
    }

    public void UpdateProfile(int id, string avatarUrl)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = "UPDATE users SET avatar_url=@a WHERE id=@id;";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@a",
                    string.IsNullOrWhiteSpace(avatarUrl) ? (object)DBNull.Value : avatarUrl);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public int Create(string username, string passwordHash)
    {
        using (var conn = Db.GetConnection())
        {
            conn.Open();
            var sql = "INSERT INTO users(username, password_hash, role) VALUES(@u,@p,'User'); SELECT LAST_INSERT_ID();";
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", passwordHash);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}