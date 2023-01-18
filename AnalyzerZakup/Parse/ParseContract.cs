using AnalyzerZakup.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AnalyzerZakup.Parse
{
    internal class ParseContract
    {
        private readonly string connectionString = DataApp.TxtBoxFileDB;
        //contractProcedure
        //contract
        //contractProcedureCancel
        //contractAvailableForElAct
        public void Parse_contracts(string fileEntries)
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
                        "insert into dataXml(fileXml, typeXml)" +
                        "values (@fileXml, @typeXml)";
                try
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

                        int result = command.ExecuteNonQuery();
                        if (result < 0) MessageBox.Show("Ошибка добавления строки в базу данных! " + result.ToString());
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "error db", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "error parse contract", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }
    }
}
