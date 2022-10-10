using ImageDomain.Models.Tags;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteInfrastructure.Tags
{
    public class SQLiteTagRepository : ITagRepository
    {
        private string _connectionString;
        private SqliteConnectionStringBuilder sqlConnectionSb;

        public SQLiteTagRepository(string connectionString)
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            _connectionString = connectionString;
            sqlConnectionSb = new SqliteConnectionStringBuilder { DataSource = connectionString, ForeignKeys = true };
        }

        public Tag Find(TagID id)
        {
            using (var connection = new SqliteConnection(sqlConnectionSb.ToString()))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT * FROM Tag where ID=@ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", id.Value);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return new Tag(id,new TagName((string)reader["Name"]));
                        }
                    }
                }
            }
            return null;
        }

        public Tag Find(TagName name)
        {
            using (var connection = new SqliteConnection(sqlConnectionSb.ToString()))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT * FROM Tag where Name=@Name", connection))
                {
                    command.Parameters.AddWithValue("@Name", name.Value);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return new Tag(new TagID((string)reader["ID"]), name);
                        }
                    }
                }
            }
            return null;

            throw new NotImplementedException();
        }

        public IReadOnlyList<Tag> FindAll()
        {
            var _tags = new List<Tag>();
            sqlConnectionSb = new SqliteConnectionStringBuilder { DataSource = _connectionString, ForeignKeys = true };

            using (var connection = new SqliteConnection(sqlConnectionSb.ToString()))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT * FROM TAG", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ID = (string)reader["ID"];
                        var Tag = (string)reader["Name"];
                        _tags.Add(new Tag(new TagID(ID), new TagName(Tag)));
                    }
                }
            }
            return _tags.AsReadOnly();
        }

        public void Save(Tag tag)
        {

            var insertQuery = @"INSERT INTO TAG(ID, Name ) VALUES(@ID ,@Name)";
            using (var connection = new SqliteConnection(sqlConnectionSb.ToString()))
            using (var command = new SqliteCommand(insertQuery, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@ID", tag.TagID.Value);
                command.Parameters.AddWithValue("@Name", tag.TagName.Value);
                command.ExecuteNonQuery();
            }
        }
    }
}
