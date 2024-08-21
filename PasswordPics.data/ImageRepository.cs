using System.Data.SqlClient;

namespace PasswordPics.data
{
    public class ImageRepository
    {
        private string _connectionString;

        public ImageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Image> GetAll()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Images";
            connection.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            List<Image> images = new List<Image>();

            while(reader.Read())
            {
                images.Add(new Image
                {
                    ID = (int)reader["ID"],
                    ImageTitle = (string)reader["ImageTitle"],
                    Password = (string)reader["Password"],
                    ImagePath = (string)reader["ImagePath"]
                });
            }

            return images;
        }

        public int Add(Image i)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Images VALUES (@title, @password, @path) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@title", i.ImageTitle);
            cmd.Parameters.AddWithValue("@password", i.Password);
            cmd.Parameters.AddWithValue("@path", i.ImagePath);
            connection.Open();
            int id = (int)(decimal)cmd.ExecuteScalar();

            return id;
        }

        public Image GetByID(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Images WHERE ID = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if(!reader.Read())
            {
                return null;
            }

            Image thisIsNew = new Image
            {
                ID = (int)reader["ID"],
                ImageTitle = (string)reader["ImageTitle"],
                Password = (string)reader["Password"],
                ImagePath = (string)reader["ImagePath"]
            };

            return thisIsNew;
        }
    }
}
