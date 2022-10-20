using MySql.Data.MySqlClient;
using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.DBContexts
{
    public class ServerContext
    {
        public string ConnectionString { get; set; }

        public ServerContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Number> GetAllNumbers()
        {
            List<Number> list = new List<Number>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM numbers;", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Number()
                        {
                            id = reader.GetInt32("id"),
                            content = reader.GetInt32("content")
                        });
                    }
                }
            }

            return list;
        }
        public Number GetNumber(int id)
        {
            Number number = null;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM numbers where id='" + id + "';", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        number = new Number()
                        {
                            id = reader.GetInt32("id"),
                            content = reader.GetInt32("content"),
                        };
                    }
                }
            }

            return number;
        }
        public Number AddNumber(int content)
        {
            Number num = new Number();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(
                    String.Format("insert into numbers (content) values('{0}');",
                    content),
                    conn);
                cmd.ExecuteNonQuery();
            }

            num.content = content;

            return num;
        }

        public Number ChangeNumber(int id, int content)
        {
            Number num = new Number();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(
                    String.Format("update numbers set content='" + content + "' where id='" + id + "';",
                    content),
                    conn);
                cmd.ExecuteNonQuery();
            }

            num.content = content;

            return num;
        }

        public bool DeleteNumber(int id)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(
                    String.Format("delete from numbers where id = '" + id + "';"),
                    conn);
                cmd.ExecuteNonQuery();
            }

            return true;
        }
    }
}
