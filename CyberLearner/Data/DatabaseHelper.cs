using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CyberLearner.Data
{
    public static class DatabaseHelper
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["CyberLearnerDB"].ConnectionString;

        public static DataTable GetTable(string sql, params SqlParameter[] parameters)
        {
            DataTable table = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddRange(parameters);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
            }

            return table;
        }

        public static int Execute(string sql, params SqlParameter[] parameters)
        {
            int rows;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddRange(parameters);

                con.Open();
                rows = cmd.ExecuteNonQuery();
            }

            return rows;
        }

        public static object GetValue(string sql, params SqlParameter[] parameters)
        {
            object value;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddRange(parameters);

                con.Open();
                value = cmd.ExecuteScalar();
            }

            return value;
        }
    }
}
