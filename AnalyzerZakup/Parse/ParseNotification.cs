using AnalyzerZakup.Data;
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
    internal class ParseNotification
    {
        private readonly string connectionString = DataApp.TxtBoxFileDB;
        // epNotificationCancel
        // epClarificationDoc
        public void Parse_Notification(string fileEntries)
        {
            XmlDocument docXML = new XmlDocument(); // XML-документ
            docXML.Load(fileEntries); // загрузить XML

            DataXML dataXML = new DataXML();
            ProtocolInfo protocolInfo = new ProtocolInfo();
            CommonInfo common_Info = new CommonInfo();
            ProtocolPublisherInfo protocolPublisherInfo = new ProtocolPublisherInfo();
            CommissionMembers commissionMembers = new CommissionMembers();
            ApplicationInfo applicationInfo = new ApplicationInfo();

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
                var cnt_commissionname = docXML.DocumentElement.SelectNodes(
                "//ns9:commissionInfo/ns3:commissionName",
                _namespaceManager).Count;
                if (cnt_commissionname != 0)
                    protocolInfo.commissionName = docXML.DocumentElement.SelectNodes(
                    "//ns9:commissionInfo/ns3:commissionName",
                    _namespaceManager)[0]
                    .InnerText;
                else protocolInfo.commissionName = "Не определена";
                string db_commissionName = @"insert into protocolInfo (commissionName ) values (@commissionName)";
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(db_commissionName, connection);
                        command.Connection.Open();
                        command.Parameters.AddWithValue("@commissionName", protocolInfo.commissionName);
                        int result = command.ExecuteNonQuery();
                        if (result < 0) MessageBox.Show("Ошибка добавления строки в базу данных! " + result.ToString());
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information); }


                #region CommonInfo
                common_Info.commonInfo_purchaseNumber = docXML.DocumentElement.SelectNodes(
                    "//ns9:commonInfo/ns9:purchaseNumber",
                    _namespaceManager)[0]
                    .InnerText;

                common_Info.commonInfo_docNumber = docXML.DocumentElement.SelectNodes(
                    "//ns9:commonInfo/ns9:docNumber",
                    _namespaceManager)[0]
                    .InnerText;

                common_Info.commonInfo_publishDTInEIS = docXML.DocumentElement.SelectNodes(
                    "//ns9:commonInfo/ns9:publishDTInEIS",
                    _namespaceManager)[0]
                    .InnerText;
                //MessageBox.Show(_commonInfo_purchaseNumber + " " + _commonInfo_docNumber + " " + _commonInfo_publishDTInEIS);
                #endregion
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information); }

        }

        public void Parse_Clarification(string fileEntries)
        {

        }
    }
}
