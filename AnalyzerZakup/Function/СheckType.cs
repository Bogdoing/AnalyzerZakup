using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalyzerZakup.Function
{
    class СheckType
    {

        public List<string> setNameTypeDB()
        {
            List<string> columnData = new List<string>();

            using (SqlConnection connection = new SqlConnection(DataApp.TxtBoxFileDB))
            {
                connection.Open();
                string query = "SELECT nameType FROM typeDocument";
                //int count = 0;
                try
                {
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
                 catch (Exception ex) { MessageBox.Show(ex.Message, "error db SELECT nameType FROM typeDocument td", MessageBoxButtons.OK, MessageBoxIcon.Information); }                
            }
            return columnData;
        }
        public string setTypeIDdb()
        {
            string query = "Select MAX(id) from typeDocument";
            string fileTypeId = "";
            using (SqlConnection connection = new SqlConnection(DataApp.TxtBoxFileDB))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                fileTypeId = command.ExecuteScalar().ToString();
                //MessageBox.Show(fileTypeId + " * fileTypeId");
            }
            return fileTypeId;
        }

        public string setDocumentIBdb()
        {
            string query = "SELECT MAX(id) from document";
            string documentIB = "";
            using (SqlConnection connection = new SqlConnection(DataApp.TxtBoxFileDB))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                documentIB = command.ExecuteScalar().ToString();
                //MessageBox.Show(fileTypeId + " * fileTypeId");
            }
            return documentIB;
        }

        public bool checkTypeName(string fileType, List<string> columnData)
        {
            return columnData.Contains(fileType);
        }

        public bool FiendType(string filename, List<string> columnData)
        {
            bool flag = true;
            for (int i = 0; i < columnData.Count; i++)
                if (filename == columnData[i])
                    flag = false;

            return flag;
        }
    }
}
