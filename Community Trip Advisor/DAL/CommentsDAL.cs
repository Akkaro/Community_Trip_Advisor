using Community_Trip_Advisor.Models;
using System.Data;
using System.Data.SqlClient;

namespace Community_Trip_Advisor.DAL
{
    public class CommentsDAL
    {
        private string _connectionString = "Server=DESKTOP-84OLRP2;Database=place;User Id=sa;Password=Witcher2003";

        public List<Comment> ListComments(string searchParameter = "")
        {
            string commandText = "SELECT [CommentID], [PlaceID], [CommentDescription], [Rating], [CommentedOn], [AddedBy] FROM [dbo].[Comments]";

            commandText += searchParameter;

            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);

            List<Comment> comments = new List<Comment>();

            using (var adapter = new SqlDataAdapter(command))
            {
                var resultTable = new DataTable();
                adapter.Fill(resultTable);

                foreach (var row in resultTable.AsEnumerable())
                {
                    Comment comment = new Comment()
                    {
                        CommentID = int.Parse(row["CommentID"].ToString()),
                        PlaceID = int.Parse(row["PlaceID"].ToString()),
                        CommmentDescription = row["CommentDescription"].ToString(),
                        Rating = int.Parse(row["Rating"].ToString()),
                        CommentedOn = Convert.ToDateTime(row["CommentedOn"]),
                        AddedBy = row["AddedBy"].ToString(),
                    };
                    comments.Add(comment);
                }
            }
            return comments;
        }
        public bool DeleteCommentById(int id)
        {
            string commandText =
                               $" DELETE FROM [dbo].[Comments] WHERE CommentID = {id}";

            int result = runQuery(commandText);

            return result == 1
                ? true : false;
        }
        private int runQuery(string commandText)
        {
            return runQuery(commandText, new List<SqlParameter>());
        }
        public bool AddComment(Comment comment)
        {
            string commandText =
                $"INSERT INTO [dbo].[Comments] ([PlaceID], [GuestID], [CommentDescription], [Rating], [CommentedOn], [AddedBy])" +
                $"VALUES (@PlaceID, NewID(), @CommentDescription, @Rating, GetDate(), @AddedBy)";
            if(comment.CommmentDescription==null)
            {
                comment.CommmentDescription = "";
            }
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("CommentID",comment.CommentID),
                new SqlParameter("PlaceID",comment.PlaceID),
                new SqlParameter("CommentDescription",comment.CommmentDescription),
                new SqlParameter("Rating",comment.Rating),
                new SqlParameter("CommentedOn",comment.CommentedOn),
                new SqlParameter("AddedBy",comment.AddedBy)
            };

            int result = runQuery(commandText, parameters);

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
        public Comment GetComment(int id) =>
            this.ListComments(searchParameter: $" WHERE CommentID = {id}")
            .First();
        public List<Comment> ListCommentsWithPlaceID(string searchParameter)
        {
            string commandText = $"SELECT[CommentID], [PlaceID], [CommentDescription], [Rating], [CommentedOn], [AddedBy] FROM[dbo].[Comments] WHERE PlaceID LIKE '{searchParameter}'";
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);

            List<Comment> comments = new List<Comment>();

            using (var adapter = new SqlDataAdapter(command))
            {
                var resultTable = new DataTable();
                adapter.Fill(resultTable);

                foreach (var row in resultTable.AsEnumerable())
                {
                    Comment comment = new Comment()
                    {
                        CommentID = int.Parse(row["CommentID"].ToString()),
                        PlaceID = int.Parse(row["PlaceID"].ToString()),
                        CommmentDescription = row["CommentDescription"].ToString(),
                        Rating = int.Parse(row["Rating"].ToString()),
                        CommentedOn = Convert.ToDateTime(row["CommentedOn"]),
                        AddedBy = row["AddedBy"].ToString(),

                    };
                    comments.Add(comment);
                }
            }
            return comments;
        }
    }
}
