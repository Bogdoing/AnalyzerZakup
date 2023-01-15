using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using System.Xml.Linq;
using AnalyzerZakup;
using AnalyzerZakup.Data;
using static System.Net.Mime.MediaTypeNames;

namespace AnalyzerZakup
{
    internal class ParseXML
    {
        private const string connectionString =
                "Data Source=DESKTOP-432U1GM\\SQLEXPRESS;Initial Catalog=AnalizeXML;Integrated Security=True;MultipleActiveResultSets=True;"; //AnalizeXML
        private DataTable CreateTable()
        {
            //создаём таблицу
            DataTable dt = new DataTable("Data");
            //создаём три колонки
            DataColumn colID = new DataColumn("Id", typeof(Int32));
            DataColumn externalId = new DataColumn("externalId", typeof(String));
            DataColumn foundationDocNumber = new DataColumn("foundationDocNumber", typeof(Int32));
            //добавляем колонки в таблицу
            dt.Columns.Add(colID);
            dt.Columns.Add(externalId);
            dt.Columns.Add(foundationDocNumber);
            return dt;
        }

        public void Parse_XML()
        {
            string[] fileEntriesAll = Directory.GetFiles(DataApp.TxtBoxfilepath);
            string[] fileEntriesProtocol = Directory.GetFiles(DataApp.TxtBoxfilepath, "epProtocol*");
            MessageBox.Show("Parsing" + fileEntriesProtocol);

            // Parse epProtocol
            for (int i = 1; i < fileEntriesProtocol.Length; i++) // !!! i = 0 !!!
            {
                Parse_Protocol(fileEntriesProtocol[i]);
                MessageBox.Show("Parsing" + fileEntriesProtocol[i]);   
            }
            MessageBox.Show("End parsing all");
        }
        #region PARSEFILE
        public void Parse_Protocol(string fileEntries)
        {
            XmlDocument docXML = new XmlDocument(); // XML-документ
            docXML.Load(fileEntries); // загрузить XML

            //XmlTextReader reader = new XmlTextReader(fileEntries[i]);
            //XmlNamespaceManager nsmanager = new XmlNamespaceManager(reader.NameTable);
            // инициализация пространства имён
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
                //if (fileEntries[i].StartsWith("f")) // (person.ToUpper().StartsWith("T"))
                //{
                //    MessageBox.Show("f - " + fileEntries[i]);
                //}
                //MessageBox.Show("any file - " + fileEntries[i]);
                var _id = docXML.DocumentElement.SelectNodes("//ns9:id", _namespaceManager)[0].InnerText;

                var _foundationDocNumber = docXML.DocumentElement.SelectNodes(
                    "//ns9:foundationDocInfo/ns9:foundationDocNumber",
                    _namespaceManager)[0]
                    .InnerText;

                // commonInfo all
                var _commonInfo = docXML.DocumentElement.SelectNodes(
                    "//ns9:commonInfo",
                    _namespaceManager)[0]
                    .InnerText;

                var _commonInfo_purchaseNumber = docXML.DocumentElement.SelectNodes(
                    "//ns9:commonInfo/ns9:purchaseNumber",
                    _namespaceManager)[0]
                    .InnerText;

                var _commonInfo_docNumber = docXML.DocumentElement.SelectNodes(
                    "//ns9:commonInfo/ns9:docNumber",
                    _namespaceManager)[0]
                    .InnerText;

                var _commonInfo_procedureDT = docXML.DocumentElement.SelectNodes(
                    "//ns9:commonInfo/ns9:publishDTInETP",
                    _namespaceManager)[0]
                    .InnerText;
                // c close
                // protocolPublisherInfo all
                var _protocol_regNum = docXML.DocumentElement.SelectNodes(
                    "//ns9:protocolPublisherInfo/ns9:publisherOrg/ns9:regNum",
                    _namespaceManager)[0]
                    .InnerText;
                var _protocol_fullName = docXML.DocumentElement.SelectNodes(
                    "//ns9:protocolPublisherInfo/ns9:publisherOrg/ns9:fullName",
                    _namespaceManager)[0]
                    .InnerText;
                var _protocol_factAddress = docXML.DocumentElement.SelectNodes(
                    "//ns9:protocolPublisherInfo/ns9:publisherOrg/ns9:factAddress",
                    _namespaceManager)[0]
                    .InnerText;
                var _protocol_INN = docXML.DocumentElement.SelectNodes(
                    "//ns9:protocolPublisherInfo/ns9:publisherOrg/ns9:INN",
                    _namespaceManager)[0]
                    .InnerText;
                var _protocol_KPP = docXML.DocumentElement.SelectNodes(
                    "//ns9:protocolPublisherInfo/ns9:publisherOrg/ns9:KPP",
                    _namespaceManager)[0]
                    .InnerText;
                // p close
                // commissionMembers all
                var commissionMember = docXML.DocumentElement.SelectNodes(
                    "//ns3:commissionMembers/ns3:commissionMember",
                    _namespaceManager)
                    .Count.ToString();
                //MessageBox.Show("commissionMember" + commissionMember);
                for (int j = 0; j < int.Parse(commissionMember); j++)
                {
                    var _commissionMember_memberNumber = docXML.DocumentElement.SelectNodes(
                        "//ns3:commissionMembers/ns3:commissionMember/ns3:memberNumber",
                        _namespaceManager)[j]
                        .InnerText;
                    var _commissionMember_lastName = docXML.DocumentElement.SelectNodes(
                        "//ns3:commissionMembers/ns3:commissionMember/ns3:nameInfo/ns3:lastName",
                        _namespaceManager)[j]
                        .InnerText;
                    var _commissionMember_firstName = docXML.DocumentElement.SelectNodes(
                        "//ns3:commissionMembers/ns3:commissionMember/ns3:nameInfo/ns3:firstName",
                        _namespaceManager)[j]
                        .InnerText;
                    var _commissionMember_middleName = docXML.DocumentElement.SelectNodes(
                        "//ns3:commissionMembers/ns3:commissionMember/ns3:nameInfo/ns3:middleName",
                        _namespaceManager)[j]
                        .InnerText;
                    //var _commissionMember_role_code = docXML.DocumentElement.SelectNodes(
                    //    "//ns3:commissionMembers/ns3:commissionMember/ns3:role/ns3:code",
                    //    _namespaceManager)[j]
                    //    .InnerText;
                    var _commissionMember_role_name = docXML.DocumentElement.SelectNodes(
                        "//ns3:commissionMembers/ns3:commissionMember/ns3:role/ns3:name",
                        _namespaceManager)[j]
                        .InnerText;
                    //MessageBox.Show("Parse commissionMember" + j);
                }
                //
                string _applicationInfo_appNumber;
                string _applicationInfo_appDT;
                string _applicationInfo_finalPrice;
                var cnt_appNumber = docXML.DocumentElement.SelectNodes(
                        "//ns9:applicationsInfo/ns9:applicationInfo/ns9:commonInfo/ns9:appNumber",
                        _namespaceManager)
                        .Count.ToString();
                //MessageBox.Show(cnt_appNumber);
                if (cnt_appNumber != "0")
                {
                    _applicationInfo_appNumber = docXML.DocumentElement.SelectNodes(
                            "//ns9:applicationsInfo/ns9:applicationInfo/ns9:commonInfo/ns9:appNumber",
                            _namespaceManager)[0]
                            .InnerText;
                    _applicationInfo_appDT = docXML.DocumentElement.SelectNodes(
                            "//ns9:applicationsInfo/ns9:applicationInfo/ns9:commonInfo/ns9:appDT",
                            _namespaceManager)[0]
                            .InnerText;

                    var cnt_final_Prise = docXML.DocumentElement.SelectNodes(
                            "//ns9:applicationsInfo/ns9:applicationInfo/ns9:finalPrice",
                            _namespaceManager)
                            .Count.ToString();
                    if (cnt_final_Prise == "0")
                    {

                        ApplicationInfo.applicationInfo_finalPrice = "0";
                    }
                    else
                    {
                        _applicationInfo_finalPrice = docXML.DocumentElement.SelectNodes(
                            "//ns9:applicationsInfo/ns9:applicationInfo/ns9:finalPrice",
                            _namespaceManager)[int.Parse(cnt_final_Prise) - 1]
                            .InnerText;
                        ApplicationInfo.applicationInfo_finalPrice = _applicationInfo_finalPrice;
                    }
                }
                else
                {
                    MessageBox.Show("ELSE appNumber");
                    _applicationInfo_appNumber = "0";
                    _applicationInfo_appDT = "0";
                }

                //MessageBox.Show("add db applicationInfo| " + _applicationInfo_appNumber + "|" + _applicationInfo_appDT + "|" + ApplicationInfo.applicationInfo_finalPrice);

                //string query = "INSERT INTO sport (surname,kind_sport,place,id_country) VALUES (@surname,@kind_sport,@place, @id_country)";
                string query = @"insert into applicationInfo (appNumber, appDT, finalPrice) 
	                            values 
	                            (@appNumber, @appDT, @finalPrice)";
                //query = @"insert into test (name) 
                //        values
                //        (@name)"; 
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Connection.Open();

                        command.Parameters.AddWithValue("@appNumber", _applicationInfo_appNumber);
                        command.Parameters.AddWithValue("@appDT", DateTime.Parse(_applicationInfo_appDT));
                        command.Parameters.AddWithValue("@finalPrice", float.Parse(ApplicationInfo.applicationInfo_finalPrice));
                        int result = command.ExecuteNonQuery();

                        if (result < 0)
                            MessageBox.Show("Ошибка добавления строки в базу данных! " + result.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ParseXml SelectNodes - " + ex.Message);
            }
        }
        #endregion

        #region TESTCODE
        public DataTable ReadXml()
        {
            DataTable dt = null;
            string[] fileEntries = Directory.GetFiles(DataApp.TxtBoxfilepath);
            for(int i = 0; i < fileEntries.Length; i++)
            {
                try
                {
                    //загружаем xml файл
                    //XDocument xDoc = XDocument.Load(DataApp.TxtBoxfilepath + "testXML.xml");
                    XDocument xDoc = XDocument.Load(fileEntries[i]);

                    //создаём таблицу
                    dt = CreateTable();
                    DataRow newRow = null;
                    //получаем все узлы в xml файле
                    foreach (XElement elm in xDoc.Descendants("friend"))
                    {
                        //создаём новую запись
                        newRow = dt.NewRow();
                        //проверяем наличие атрибутов (если требуется)
                        if (elm.HasAttributes)
                        {
                            //проверяем наличие атрибута id
                            if (elm.Attribute("id") != null)
                            {
                                //получаем значение атрибута
                                newRow["id"] = int.Parse(elm.Attribute("id").Value);
                            }
                        }
                        //проверяем наличие xml элемента name
                        if (elm.Element("name") != null)
                        {
                            //получаем значения элемента name
                            newRow["name"] = elm.Element("name").Value;
                        }
                        if (elm.Element("age") != null)
                        {
                            newRow["age"] = int.Parse(elm.Element("age").Value);
                        }
                        //добавляем новую запись в таблицу
                        dt.Rows.Add(newRow);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }
        #endregion

    }
}

