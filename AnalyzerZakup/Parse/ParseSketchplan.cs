using AnalyzerZakup.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AnalyzerZakup.Parse
{
    internal class ParseSketchplan
    {
        private readonly string connectionString = DataApp.TxtBoxFileDB;

        public void Parse_sketchplan(string fileEntries)
        {
            XmlDocument docXML = new XmlDocument(); // XML-документ
            docXML.Load(fileEntries); // загрузить XML

            DataXML dataXML = new DataXML();

            XmlNamespaceManager _namespaceManager = new XmlNamespaceManager(docXML.NameTable);
            _namespaceManager.AddNamespace("ns", "http://zakupki.gov.ru/oos/types/1");
            _namespaceManager.AddNamespace("ns2", "http://zakupki.gov.ru/oos/export/1");
            _namespaceManager.AddNamespace("ns4", "http://zakupki.gov.ru/oos/base/1");
            _namespaceManager.AddNamespace("ns3", "http://zakupki.gov.ru/oos/common/1");
            _namespaceManager.AddNamespace("ns6", "http://zakupki.gov.ru/oos/KOTypes/1");
            _namespaceManager.AddNamespace("ns5", "http://zakupki.gov.ru/oos/TPtypes/1");
            _namespaceManager.AddNamespace("ns8", "http://zakupki.gov.ru/oos/pprf615types/1");
            _namespaceManager.AddNamespace("ns7", "http://zakupki.gov.ru/oos/CPtypes/1");
            _namespaceManager.AddNamespace("ns13", "http://zakupki.gov.ru/oos/printform/1");
            _namespaceManager.AddNamespace("ns9", "http://zakupki.gov.ru/oos/EPtypes/1");
            _namespaceManager.AddNamespace("ns12", "http://zakupki.gov.ru/oos/EATypes/1");
            _namespaceManager.AddNamespace("ns11", "http://zakupki.gov.ru/oos/URTypes/1");
            _namespaceManager.AddNamespace("ns10", "http://zakupki.gov.ru/oos/SMTypes/1");
            _namespaceManager.AddNamespace("ns14", "http://zakupki.gov.ru/oos/control99/1");

            try
            {
                dataXML.id = int.Parse(docXML.GetElementsByTagName("id")[0].InnerText);
                //MessageBox.Show(dataXML.id + "");

                dataXML.fileXml = fileEntries;

                string query =  // ДРУГОЙ НУЖЕН
                @"DECLARE @fileXml xml; 
                        SELECT @fileXml = (SELECT * FROM OPENROWSET(BULK '" + dataXML.fileXml + "', SINGLE_BLOB) as [xml])" +
                        "insert into dataXml(fileXml, typeXml, nameFile, id)" +
                        "values (@fileXml, @typeXml, @nameFile, @id)";
                try
                {
                    if (BoolFile(fileEntries))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Connection.Open();
                            string[] test = dataXML.fileXml.Split('\\');
                            string test2 = test[test.Length - 1];
                            dataXML.fileType = test2.Split('_')[0];
                            command.Parameters.AddWithValue("@typeXml", dataXML.fileType);
                            command.Parameters.AddWithValue("@id", dataXML.id);
                            command.Parameters.AddWithValue("@nameFile", fileEntries);

                            int result = command.ExecuteNonQuery();
                            if (result < 0) MessageBox.Show("Ошибка добавления строки в базу данных! " + result.ToString());
                        }
                    }
                    else
                    {

                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "error db", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "error parse contract", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private bool BoolFile(string filename)
        {
            bool result = false;
            string sqlExpression = "SELECT nameFile FROM dataXml";
            try 
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    //SqlDataReader reader = command.ExecuteReader();

                    DataTable dt = new DataTable();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                        
                        var rstb = dt.AsEnumerable().Select(se => new NameFileClass() { NameFile = se.Field<object>(filename).ToString().Split('\\').Last() }).ToList(); //id = se.Field<int>("id"),  lastname = se.Field<string>("Фамилия")
                        rstb = dt.AsEnumerable().Select(se => new NameFileClass() { NameFile = se.Field<object>(filename).ToString().Split('\\').Last() }).ToList(); //id = se.Field<int>("id"),  lastname = se.Field<string>("Фамилия")

                        MessageBox.Show(rstb + "");
                        //if (rstb != null) result = true;
                        //else result = false;
                    }
                }
                MessageBox.Show(result + "");                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "error bool to fileName", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            return result;
        }
    }

    class NameFileClass
    {
        public string NameFile { get; set; }
    }
}


//if (reader.HasRows) // если есть данные
//{
//    // выводим названия столбцов
//    //Console.WriteLine("{0}", reader.GetName(0));

//    while (reader.Read()) // построчно считываем данные
//    {

//        List<object> id;
//        id += (List<object>)reader.GetValue(0);

//        for (int i = 0; i < reader.FieldCount; i++)
//        {
//            for (int j = 0; i < reader.FieldCount; i++)
//            {
//                if (id != filename)
//                {

//                }
//            }

//        }


//        MessageBox.Show("file name - " + id);
//    }
//}

