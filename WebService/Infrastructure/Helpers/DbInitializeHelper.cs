using System.Configuration;
using System.Data.SqlClient;

namespace UserService.Infrastructure.Helpers
{
    public static class DbInitializeHelper
    {
        public static void CreateDbInfrastructure()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["initializeConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                command.CommandText = QueryHelper.GetCreateDbExpression(Constants.dbName);
                command.ExecuteNonQuery();

                command.CommandText = QueryHelper.GetUseExpression(Constants.dbName);
                command.ExecuteNonQuery();

                command.CommandText = QueryHelper.GetCreateUserTableExpression();
                command.ExecuteNonQuery();
            }
        }
    }
}