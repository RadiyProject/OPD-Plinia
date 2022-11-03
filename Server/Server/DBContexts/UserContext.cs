using MySql.Data.MySqlClient;
using Server.Models;
using System;
using System.Collections.Generic;

namespace Server.DBContexts
{
    public class UserContext
    {
        public string ConnectionString { get; set; }

        public UserContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<User> GetAllUsers()
        {
            List<User> list = new List<User>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM users;", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new User()
                        {
                            id = reader.GetInt32("id"),
                            name = reader.GetString("name"),
                            password = reader.GetString("password")

                        });
                    }
                }
            }

            return list;
        }
        public User GetUser(int id)
        {
            User user = null;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM users where id='" + id + "';", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new User()
                        {
                            id = reader.GetInt32("id"),
                            name = reader.GetString("name"),
                            password = reader.GetString("password")
                        };
                    }
                }
            }

            return user;
        }
        public User AddUser(string name, string password)
        {
            User user = new User();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(
                    String.Format("insert into users (name, password) values('{0}', '{1}');",
                    name, password),
                    conn);
                cmd.ExecuteNonQuery();
            }

            user.name = name;
            user.password = password;

            return user;
        }

        public User ChangeUser(int id, string name, string password)
        {
            User user = new User();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd1 = new MySqlCommand(
                    String.Format("update users set name='" + name + "' where id='" + id + "';",
                    name),
                    conn);
                MySqlCommand cmd2 = new MySqlCommand(
                    String.Format("update users set password='" + password + "' where id='" + id + "';",
                    password),
                    conn);
                cmd1.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
            }

            user.name = name;
            user.password = password;

            return user;
        }

        public bool DeleteUser(int id)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(
                    String.Format("delete from users where id = '" + id + "';"),
                    conn);
                cmd.ExecuteNonQuery();
            }

            return true;
        }
    }
}
