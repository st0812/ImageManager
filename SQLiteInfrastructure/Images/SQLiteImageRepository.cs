using ImageDomain.Models.Images;
using ImageDomain.Models.Tags;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteInfrastructure.Images
{
    public class SQLiteImageRepository : IImageRepository
    {
        private string _connectionString;
        private string _imageFolderPath;
        private SqliteConnectionStringBuilder sqlConnectionSb;

        public SQLiteImageRepository(string connectionString, string imageFolderPath)
        {
            _connectionString = connectionString;
            _imageFolderPath = imageFolderPath;
            sqlConnectionSb = new SqliteConnectionStringBuilder { DataSource = _connectionString, ForeignKeys = true };
        }


        public Image Find(ImageID id)
        {
            Image image = null;
            using (var connection = new SqliteConnection(sqlConnectionSb.ToString()))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT * FROM ENTRY where ID=@ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", id.Value);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            image = new Image(id, new ImagePath(Path.Combine(_imageFolderPath, (string)reader["Filepath"])));
                            break;
                        }
                    }
                }
            }
            if (image == null) return null;


            var _tags = new List<Tag>();
            var selectQuery
                = @"SELECT 
                    TAG.ID, TAG.NAME
                    FROM ENTRY LEFT OUTER JOIN TAG_MAP ON ENTRY.ID=TAG_MAP.ENTRY_ID
                               LEFT OUTER JOIN TAG ON TAG.ID=TAG_MAP.TAG_ID
                    WHERE ENTRY.ID=@entry_id";
            using (var connection = new SqliteConnection(sqlConnectionSb.ToString()))
            {
                connection.Open();
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@entry_id", id.Value);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read() && !reader.IsDBNull(0))
                        {
                            var ID = (string)reader["ID"];
                            image.AttachTag(new TagID(ID));
                        }
                    }
                }
            }
            return image;
        }

        public IReadOnlyList<Image> FindAll()
        {


            var _entries = new List<Image>();
            using (var connection = new SqliteConnection(sqlConnectionSb.ToString()))
            {
                connection.Open();
                using (var command = new SqliteCommand("SELECT * FROM ENTRY", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ID = (string)reader["ID"];
                        var filepath = Path.Combine(_imageFolderPath, (string)reader["Filepath"]);
                        _entries.Add(new Image(new ImageID(ID), new ImagePath(filepath)));
                    }
                }
            }

            var _tags = new List<Tag>();
            var selectQuery
                = @"SELECT 
                    * FROM TAG_MAP";
            using (var connection = new SqliteConnection(sqlConnectionSb.ToString()))
            {
                connection.Open();
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entry_ID = new ImageID((string)reader["Entry_ID"]);
                            var tag_ID = new TagID((string)reader["Tag_ID"]);
                            _entries.FirstOrDefault(entry => entry.ID.Value == entry_ID.Value)?.AttachTag(tag_ID);
                        }
                    }
                }
            }
            return _entries.AsReadOnly();
        }

        public void Save(Image image)
        {
            if (Find(image.ID) == null)
            {
                var insertImageQuery = @"INSERT INTO ENTRY(ID, Filepath) VALUES(@ID, @Filepath)";

                var newFilePath = Path.Combine(_imageFolderPath, Guid.NewGuid().ToString() + image.ImagePath.Extension);
                File.Move(image.ImagePath.FilePath, newFilePath);
                image.ImagePath = new ImagePath(newFilePath);


                using (var connection = new SqliteConnection(sqlConnectionSb.ToString()))
                using (var command = new SqliteCommand(insertImageQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@ID", image.ID.Value);
                    command.Parameters.AddWithValue("@Filepath", Path.GetFileName(image.ImagePath.FilePath));
                    command.ExecuteNonQuery();
                }
            }

            var deleteQuery = @"DELETE FROM TAG_MAP where ENTRY_ID=@Entry_ID";
            using (var connection = new SqliteConnection(sqlConnectionSb.ToString()))
            using (var command = new SqliteCommand(deleteQuery, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Entry_ID", image.ID.Value);
                command.ExecuteNonQuery();
            }


            foreach (var tag in image.AttachedTags)
            {
                var insertQuery = @"INSERT INTO TAG_MAP(ENTRY_ID,TAG_ID ) VALUES(@Entry_ID ,@Tag_ID)";
                var Entry_ID = image.ID;
                var Tag_ID = tag;
                using (var connection = new SqliteConnection(sqlConnectionSb.ToString()))
                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Entry_ID", Entry_ID.Value);
                    command.Parameters.AddWithValue("@Tag_ID", Tag_ID.Value);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
