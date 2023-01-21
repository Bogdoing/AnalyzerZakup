using AnalyzerZakup.Data;
using AnalyzerZakup.Function;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AnalyzerZakup.Parse
{
    internal class ParseCustomerreport
    {
        private readonly string connectionString = DataApp.TxtBoxFileDB;
        //fcsCustomerReportSmallScaleBusiness
        //fcsCustomerReportRusProductsPurchasesVolume
        //fcsCustomerReportSingleContractor
        Search search = new Search();
        DataXML dataXML = new DataXML();
        public void Parse_customerreport(string fileEntries)
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

                string query = @"DECLARE @fileXml xml; 
                        SELECT @fileXml = (SELECT * FROM OPENROWSET(BULK '" + dataXML.fileXml + "', SINGLE_BLOB) as [xml])" +
                        "insert into dataXml(fileXml, typeXml, nameFile, idDoc)" +
                        "values (@fileXml, @typeXml, @nameFile, @idDoc)";
                try
                {
                    string[] NameFilestrList = dataXML.fileXml.Split('\\');
                    string NameFilestr = NameFilestrList[NameFilestrList.Length - 1];
                    dataXML.fileType = NameFilestr.Split('_')[0];

                    string[] test = dataXML.fileXml.Split('\\');
                    dataXML.nameFile = test[test.Length - 1];

                    List<string> dbFilename = search.SearchDB();
                    //MessageBox.Show(fileEntries + "");
                    if (search.FiendList(dataXML.nameFile, dbFilename))
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Connection.Open();
                            //MessageBox.Show("db1 namefile" + dataXML.nameFile);
                            command.Parameters.AddWithValue("@typeXml", dataXML.fileType);
                            command.Parameters.AddWithValue("@idDoc", dataXML.id);
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
    }
}

