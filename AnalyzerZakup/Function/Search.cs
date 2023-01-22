using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerZakup.Function
{
    class Search
    {
        public List<string> SearchDB()
        {
            List<string> columnData = new List<string>();

            using (SqlConnection connection = new SqlConnection(DataApp.TxtBoxFileDB))
            {
                connection.Open();
                string query = "select name from document";
                //int count = 0;
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //count++; //MessageBox.Show(reader.GetString(0) + "| count = " + count);
                            columnData.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return columnData;
        }
        public bool FiendList(string filename, List<string> columnData)
        {
            bool flag = true;
            for (int i = 0; i < columnData.Count; i++)
                if (filename == columnData[i])
                    flag = false;

            return flag;
        }
    }
}
