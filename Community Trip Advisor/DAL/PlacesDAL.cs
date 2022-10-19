using Community_Trip_Advisor.Models;
using System.Data;
using System.Data.SqlClient;

namespace Community_Trip_Advisor.DAL
{
    public class PlacesDAL
    {
        private string _connectionString = "Server=DESKTOP-84OLRP2;Database=place;User Id=sa;Password=Witcher2003";

        public List<Place> ListPlaces(string searchParameter = "")
        {
            string commandText = "SELECT [Name], [ID], [Type], [Description], [ImgPath], [ImgTitle] FROM [dbo].[place]";

            commandText += searchParameter;

            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);

            List<Place> places = new List<Place>();

            using (var adapter = new SqlDataAdapter(command))
            {
                var resultTable = new DataTable();
                adapter.Fill(resultTable);

                foreach (var row in resultTable.AsEnumerable())
                {
                    Place place = new Place()
                    {
                        ID = int.Parse(row["ID"].ToString()),
                        Name = row["Name"].ToString(),
                        Description = row["Description"].ToString(),
                        Type = row["Type"].ToString(),
                        ImgPath = row["ImgPath"].ToString(),
                        ImgTitle = row["ImgTitle"].ToString(),
                        
                    };
                    places.Add(place);
                }
            }
            return places;
        }
        public bool AddPlace(Place place)
        {
            string commandText =
                $"INSERT INTO [dbo].[place] ([Name],[Type],[Description],[ImgPath],[ImgTitle])" +
                $"VALUES (@Name, @Type, @Description, @ImgPath, @ImgTitle)";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("Name",place.Name),
                new SqlParameter("Type",place.Type),
                new SqlParameter("Description",place.Description),
                new SqlParameter("ImgPath",place.ImgPath ?? string.Empty),
                new SqlParameter("ImgTitle",place.ImgTitle ?? string.Empty)
            };

            int result = runQuery(commandText, parameters);

            return result == 1
                ? true : false;
        }
        public bool DeletePlace(Place place)
        {
            return DeletePlaceById(place.ID);
        }
        private int runQuery(string commandText)
        {
            return runQuery(commandText, new List<SqlParameter>());
        }
        public bool DeletePlaceById(int id)
        {
            string commandText =
                               $" DELETE FROM [dbo].[place] WHERE ID = {id}";

            int result = runQuery(commandText);

            return result == 1
                ? true : false;
        }
        private int runQuery(string commandText, List<SqlParameter> parameters)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);

            parameters.ForEach(parameter => command.Parameters.Add(parameter));

            connection.Open();
            int result = command.ExecuteNonQuery();
            connection.Close();

            return result;

        }
        public Place GetPlace(int id) =>
            this.ListPlaces(searchParameter: $" WHERE ID = {id}")
            .First();

        public List<Place> Search(string searchParameter)
        {
            string commandText = $"SELECT [Name], [ID], [Type], [Description], [ImgPath], [ImgTitle] FROM [dbo].[place] WHERE Name LIKE '%{searchParameter}%'";
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);

            List<Place> places = new List<Place>();

            using (var adapter = new SqlDataAdapter(command))
            {
                var resultTable = new DataTable();
                adapter.Fill(resultTable);

                foreach (var row in resultTable.AsEnumerable())
                {
                    Place place = new Place()
                    {
                        ID = int.Parse(row["ID"].ToString()),
                        Name = row["Name"].ToString(),
                        Description = row["Description"].ToString(),
                        Type = row["Type"].ToString(),
                        ImgPath = row["ImgPath"].ToString(),
                        ImgTitle = row["ImgTitle"].ToString(),

                    };
                    places.Add(place);
                }
            }
            return places;
        }

        public bool UpdatePlace(Place place)
        {
            string commandText =
                   $" UPDATE[dbo].[place] " +
                   $" SET [Name] = @Name " +
                   $"     ,[Type] = @Type " +
                   $"     ,[Description] = @Description " +
                   $"     ,[ImgTitle] = @ImgTitle  " +
                   $"     ,[ImgPath] = @ImgPath " +
                   $" WHERE ID = @ID ";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("Name",place.Name),
                new SqlParameter("Type",place.Type),
                new SqlParameter("Description",place.Description),
                new SqlParameter("ImgPath",place.ImgPath),
                new SqlParameter("ImgTitle",place.ImgTitle),
                new SqlParameter("ID",place.ID)
            };

            int result = runQuery(commandText, parameters);

            return result == 1
                ? true : false;
        }
    }

}
