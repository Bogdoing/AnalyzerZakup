using AnalyzerZakup.Data;
using AnalyzerZakup.Function;
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
        Search search = new Search();
        СheckType сheckType = new СheckType();

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

                try
                {
                    string[] NameFilestrList = dataXML.fileXml.Split('\\');
                    string NameFilestr = NameFilestrList[NameFilestrList.Length - 1];
                    dataXML.fileType = NameFilestr.Split('_')[0]; // fileType

                    List<string> dbFileType = сheckType.setNameTypeDB();
                    if (сheckType.checkTypeName(dataXML.fileType, dbFileType) == false)
                    {
                        string query_insert = @"INSERT into typeDocument(nameType) VALUES(@nameType)";
                        try
                        {
                            //MessageBox.Show("INSERT into typeDocument(nameType)");
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                SqlCommand command = new SqlCommand(query_insert, connection);
                                command.Connection.Open();
                                command.Parameters.AddWithValue("@nameType", dataXML.fileType);
                                int result = command.ExecuteNonQuery();
                                if (result < 0) MessageBox.Show("Ошибка добавления строки в базу данных INSERT nameType! " + result.ToString());
                            }
                            query_insert = @"insert into typeDocumentTag (typeDocumentId, tagId) values (@typeDocumentId, 24)";
                            //MessageBox.Show(сheckType.setTypeIDdb() + " - сheckType.setTypeIDdb()");
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                SqlCommand command = new SqlCommand(query_insert, connection);
                                command.Connection.Open();
                                command.Parameters.AddWithValue("@typeDocumentId", сheckType.setTypeIDdb());
                                int result = command.ExecuteNonQuery();
                                if (result < 0) MessageBox.Show("Ошибка добавления строки в базу данных INSERT typeDocumentTag! " + result.ToString());
                            }
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "error db INSERT nameType", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    }

                    string query_getTypeName = @"Select id from typeDocument where nameType = '" + dataXML.fileType + "'";
                    string fileTypeId = "";
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand(query_getTypeName, connection);
                            command.Connection.Open();
                            fileTypeId = command.ExecuteScalar().ToString();
                            //MessageBox.Show(fileTypeId + " * fileTypeId");
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "error db Select nameType", MessageBoxButtons.OK, MessageBoxIcon.Information); }

                    string[] test = dataXML.fileXml.Split('\\');
                    dataXML.nameFile = test[test.Length - 1]; // @name

                    string query = @"DECLARE @fileX xml;
                    SELECT @fileX = (SELECT * FROM OPENROWSET(BULK '" + dataXML.fileXml + "', SINGLE_BLOB) as [xml])" +
                        "insert into document(name, fileXml, typeDocumentId)" +
                        "values" +
                        "(@name, @fileX, @typeDocumentId)";
                    string query_Value = @"insert into documentTag (documentId, tagId, value) 
                            values 
                            (@documentId, @tagId, @value)";
                    List<string> dbFilename = search.SearchDB();
                    //MessageBox.Show(search.FiendList(dataXML.nameFile, dbFilename) + "");
                    if (search.FiendList(dataXML.nameFile, dbFilename))
                    {
                        try
                        {
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                SqlCommand command = new SqlCommand(query, connection);
                                command.Connection.Open();
                                command.Parameters.AddWithValue("@name", dataXML.nameFile);
                                command.Parameters.AddWithValue("@typeDocumentId", fileTypeId);

                                int result = command.ExecuteNonQuery();
                                if (result < 0) MessageBox.Show("Ошибка добавления строки в базу данных OPENROWSET! " + result.ToString());
                            }
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "error db insert into documen", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                        try
                        {
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                //MessageBox.Show(сheckType.setDocumentIBdb() + " - сheckType.setDocumentIBdb()");
                                SqlCommand command = new SqlCommand(query_Value, connection);
                                command.Connection.Open();
                                command.Parameters.AddWithValue("@documentId", int.Parse(сheckType.setDocumentIBdb()));
                                command.Parameters.AddWithValue("@tagId", 24);
                                command.Parameters.AddWithValue("@value", dataXML.id.ToString());
                                int result = command.ExecuteNonQuery();
                                if (result < 0) MessageBox.Show("Ошибка добавления строки в базу данных OPENROWSET! " + result.ToString());
                            }
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "error db insert into documen", MessageBoxButtons.OK, MessageBoxIcon.Information); }

                    }
                    else { MessageBox.Show("db have fhis file - " + fileEntries); }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "error db all", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "error parse contract", MessageBoxButtons.OK, MessageBoxIcon.Information); }
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

