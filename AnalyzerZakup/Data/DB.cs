using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerZakup.Data
{
    internal class DB
    {
        //public const string connectionString =
        //    @"Data Source=DESKTOP-432U1GM\\SQLEXPRESS;Initial Catalog=AnalizeXML;Integrated Security=True;MultipleActiveResultSets=True;"; //AnalizeXML
        public const string connectionString = @"Data Source=Bogdoing\SQLEXPRESS;Initial Catalog=AnalizeXML;Integrated Security=True;MultipleActiveResultSets=True;";

        private SqlConnection connection;

        public DB()
        {
        }

        public SqlConnection GetConnection()
        {
            connection = connection ?? new SqlConnection(connectionString);
            connection.Open();

            return connection;
        }

        public void Dispose()
        {
            if (connection != null)
                connection.Close();
        }
    }
}
