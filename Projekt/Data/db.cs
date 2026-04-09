using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;

public static class Db
{
    public static MySqlConnection GetConnection()
    {
        var cs = ConfigurationManager.ConnectionStrings["SveznalicaConn"].ConnectionString;
        return new MySqlConnection(cs);
    }
}
