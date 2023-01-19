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

        DataXML dataXML = new DataXML();
        bool flag = true;

        public void Parse_sketchplan(string fileEntries)
        {
            XmlDocument docXML = new XmlDocument(); // XML-документ
            docXML.Load(fileEntries); // загрузить XML

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
                    string[] NameFilestrList = dataXML.fileXml.Split('\\');
                    string NameFilestr = NameFilestrList[NameFilestrList.Length - 1];
                    dataXML.fileType = NameFilestr.Split('_')[0];

                    string[] test = dataXML.fileXml.Split('\\');
                    dataXML.nameFile = test[test.Length - 1];


                    List<string> dbFilename = SearchDB(); //!
                    //FiendList(fileEntries, dbFilename);
                    MessageBox.Show(fileEntries + "");
                    if (FiendList(dataXML.nameFile, dbFilename))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Connection.Open();
                            //MessageBox.Show("db1 namefile" + dataXML.nameFile);
                            command.Parameters.AddWithValue("@typeXml", dataXML.fileType);
                            command.Parameters.AddWithValue("@id", dataXML.id);
                            command.Parameters.AddWithValue("@nameFile", dataXML.nameFile);

                            int result = command.ExecuteNonQuery();
                            if (result < 0) MessageBox.Show("Ошибка добавления строки в базу данных! " + result.ToString());
                        }
                    }
                    else { MessageBox.Show("db have fhis file - " + fileEntries); }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "error db", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "error parse contract", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }
        private List<string> SearchDB()
        {
            List<string> columnData = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "select nameFile from dataXml"; // "SELECT Column1 FROM Table1"
                //int count = 0;
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //count++;
                            //MessageBox.Show(reader.GetString(0) + "| count = " + count);
                            columnData.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return columnData;
        }
        private bool FiendList(string filename, List<string> columnData)
        {
            bool flag = true;
            for (int i = 0; i < columnData.Count; i++)
            {
                if (filename == columnData[i]) { flag = false; }
            }
            return flag;
        }

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

