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
    internal class ParseProtocol
    {
        private readonly string connectionString = DataApp.TxtBoxFileDB;
        #region PARSEFILE
        public void Parse_Protocol(string fileEntries)
        {
            XmlDocument docXML = new XmlDocument(); // XML-документ
            docXML.Load(fileEntries); // загрузить XML

            string _fileName = "count.txt";
            int count = 1;

            if (File.Exists(_fileName))
            {
                count = int.Parse(File.ReadAllText(_fileName));  // int.Parse(string)
            }


            DataXML dataXML = new DataXML();
            ProtocolInfo protocolInfo = new ProtocolInfo();
            CommonInfo common_Info = new CommonInfo();
            ProtocolPublisherInfo protocolPublisherInfo = new ProtocolPublisherInfo();
            CommissionMembers commissionMembers = new CommissionMembers();
            ApplicationInfo applicationInfo = new ApplicationInfo();

            protocolInfo.id = count;
            count++;
            File.WriteAllText(_fileName, count + "");
            //MessageBox.Show(protocolInfo.id + "");

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

                #region dbAddCommonInfo
                string db_commonInfo = @"insert into commonInfo(purchaseNumber, docNumber, docPublishDTInEIS)
                values
                (@purchaseNumber, @docNumber, @docPublishDTInEIS)";
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(db_commonInfo, connection);
                        command.Connection.Open();

                        command.Parameters.AddWithValue("@purchaseNumber", common_Info.commonInfo_purchaseNumber);
                        command.Parameters.AddWithValue("@docNumber", common_Info.commonInfo_docNumber);
                        command.Parameters.AddWithValue("@docPublishDTInEIS", DateTime.Parse(common_Info.commonInfo_publishDTInEIS));
                        int result = command.ExecuteNonQuery();

                        if (result < 0)
                            MessageBox.Show("Ошибка добавления строки в базу данных! " + result.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
                // c close

                #region protocolPublisherInfo
                // protocolPublisherInfo all
                protocolPublisherInfo.protocol_regNum = docXML.DocumentElement.SelectNodes(
                    "//ns9:protocolPublisherInfo/ns9:publisherOrg/ns9:regNum",
                    _namespaceManager)[0]
                    .InnerText;
                protocolPublisherInfo.protocol_fullName = docXML.DocumentElement.SelectNodes(
                    "//ns9:protocolPublisherInfo/ns9:publisherOrg/ns9:fullName",
                    _namespaceManager)[0]
                    .InnerText;
                protocolPublisherInfo.protocol_factAddress = docXML.DocumentElement.SelectNodes(
                    "//ns9:protocolPublisherInfo/ns9:publisherOrg/ns9:factAddress",
                    _namespaceManager)[0]
                    .InnerText;
                protocolPublisherInfo.protocol_INN = docXML.DocumentElement.SelectNodes(
                    "//ns9:protocolPublisherInfo/ns9:publisherOrg/ns9:INN",
                    _namespaceManager)[0]
                    .InnerText;
                protocolPublisherInfo.protocol_KPP = docXML.DocumentElement.SelectNodes(
                    "//ns9:protocolPublisherInfo/ns9:publisherOrg/ns9:KPP",
                    _namespaceManager)[0]
                    .InnerText;
                // p close
                #endregion

                #region dbAddprotocolPublisherInfo
                string db_protocolPublisherInfoo = @"insert into protocolPublisherInfo(regNum, fullName, factAddress, INN, KPP)
                            values
                            (@regNum, @fullName, @factAddress, @INN, @KPP)";
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(db_protocolPublisherInfoo, connection);
                        command.Connection.Open();
                        command.Parameters.AddWithValue("@regNum", protocolPublisherInfo.protocol_regNum);
                        command.Parameters.AddWithValue("@fullName", protocolPublisherInfo.protocol_fullName);
                        command.Parameters.AddWithValue("@factAddress", protocolPublisherInfo.protocol_factAddress);
                        command.Parameters.AddWithValue("@INN", protocolPublisherInfo.protocol_INN);
                        command.Parameters.AddWithValue("@KPP", protocolPublisherInfo.protocol_KPP);
                        int result = command.ExecuteNonQuery();

                        if (result < 0)
                            MessageBox.Show("Ошибка добавления строки в базу данных! " + result.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion

                #region applicationInfo
                // add DB applicationInfo
                string _applicationInfo_appNumber;
                string _applicationInfo_appDT;
                string _applicationInfo_finalPrice;
                var cnt_appNumber = docXML.DocumentElement.SelectNodes(
                        "//ns9:applicationsInfo/ns9:applicationInfo/ns9:commonInfo/ns9:appNumber",
                        _namespaceManager)
                        .Count;
                //MessageBox.Show(cnt_appNumber);
                if (cnt_appNumber != 0)
                {
                    applicationInfo.applicationInfo_appNumber = docXML.DocumentElement.SelectNodes(
                            "//ns9:applicationsInfo/ns9:applicationInfo/ns9:commonInfo/ns9:appNumber",
                            _namespaceManager)[cnt_appNumber - 1]
                            .InnerText;
                    applicationInfo.applicationInfo_appDT = docXML.DocumentElement.SelectNodes(
                            "//ns9:applicationsInfo/ns9:applicationInfo/ns9:commonInfo/ns9:appDT",
                            _namespaceManager)[0]
                            .InnerText;

                    var cnt_final_Prise = docXML.DocumentElement.SelectNodes(
                            "//ns9:applicationsInfo/ns9:applicationInfo/ns9:finalPrice",
                            _namespaceManager)
                            .Count.ToString();
                    if (cnt_final_Prise == "0")
                    {
                        applicationInfo.applicationInfo_finalPrice = "0.0";
                    }
                    else
                    {
                        applicationInfo.applicationInfo_finalPrice = docXML.DocumentElement.SelectNodes(
                            "//ns9:applicationsInfo/ns9:applicationInfo/ns9:finalPrice",
                            _namespaceManager)[int.Parse(cnt_final_Prise) - 1]
                            .InnerText;
                    }
                }
                else
                {
                    MessageBox.Show("ELSE appNumber");
                    applicationInfo.applicationInfo_appNumber = "0";
                    applicationInfo.applicationInfo_finalPrice = "0.0";
                    applicationInfo.applicationInfo_appDT = "01.01.1999 0:00:00"; //"01.01.2000 0:00:00"
                }

                //MessageBox.Show("add db applicationInfo| " + _applicationInfo_appNumber + "|" + _applicationInfo_appDT + "|" + ApplicationInfo.applicationInfo_finalPrice);
                #endregion

                #region dbApplicationInfo
                string query = @"insert into applicationInfo (appNumber, appDT, finalPrice) 
	                            values 
	                            (@appNumber, @appDT, @finalPrice)";
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Connection.Open();

                        command.Parameters.AddWithValue("@appNumber", int.Parse(applicationInfo.applicationInfo_appNumber));
                        command.Parameters.AddWithValue("@appDT", DateTime.Parse(applicationInfo.applicationInfo_appDT));
                        command.Parameters.AddWithValue("@finalPrice", applicationInfo.strTofloat(applicationInfo.applicationInfo_finalPrice));
                        int result = command.ExecuteNonQuery();

                        if (result < 0)
                            MessageBox.Show("Ошибка добавления строки в базу данных! " + result.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion

                #region commissionMembers
                // commissionMembers all
                var commissionMember = docXML.DocumentElement.SelectNodes(
                    "//ns3:commissionMembers/ns3:commissionMember",
                    _namespaceManager)
                    .Count.ToString();
                string _commissionMember_memberNumber = string.Empty,
                        _commissionMember_lastName = string.Empty,
                        _commissionMember_firstName = string.Empty,
                        _commissionMember_middleName = string.Empty,
                        _commissionMember_role_name = string.Empty;

                //MessageBox.Show("commissionMember" + commissionMember);
                for (int j = 0; j < int.Parse(commissionMember); j++)
                {
                    commissionMembers.commissionMember_memberNumber = docXML.DocumentElement.SelectNodes(
                        "//ns3:commissionMembers/ns3:commissionMember/ns3:memberNumber",
                        _namespaceManager)[j]
                        .InnerText;
                    commissionMembers.commissionMember_lastName = docXML.DocumentElement.SelectNodes(
                        "//ns3:commissionMembers/ns3:commissionMember/ns3:nameInfo/ns3:lastName",
                        _namespaceManager)[j]
                        .InnerText;
                    commissionMembers.commissionMember_firstName = docXML.DocumentElement.SelectNodes(
                        "//ns3:commissionMembers/ns3:commissionMember/ns3:nameInfo/ns3:firstName",
                        _namespaceManager)[j]
                        .InnerText;
                    commissionMembers.commissionMember_middleName = docXML.DocumentElement.SelectNodes(
                        "//ns3:commissionMembers/ns3:commissionMember/ns3:nameInfo/ns3:middleName",
                        _namespaceManager)[j]
                        .InnerText;
                    commissionMembers.commissionMember_role_name = docXML.DocumentElement.SelectNodes(
                        "//ns3:commissionMembers/ns3:commissionMember/ns3:role/ns3:name",
                        _namespaceManager)[j]
                        .InnerText;
                    //MessageBox.Show("Parse commissionMember" + j);
                    #region dbAddcommissionMembers
                    string db_commissionMembers =
                        @"insert into commissionMember (memberNumber, lastName, firstName, middleName, commissionRole, protocolInfo_id) 
	                        values 
	                        (@memberNumber, @lastName, @firstName, @middleName,	@commissionRole, @protocolInfo_id)";
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand(db_commissionMembers, connection);
                            command.Connection.Open();

                            command.Parameters.AddWithValue("@memberNumber", int.Parse(commissionMembers.commissionMember_memberNumber));
                            command.Parameters.AddWithValue("@lastName", commissionMembers.commissionMember_lastName);
                            command.Parameters.AddWithValue("@firstName", commissionMembers.commissionMember_firstName);
                            command.Parameters.AddWithValue("@middleName", commissionMembers.commissionMember_middleName);
                            command.Parameters.AddWithValue("@commissionRole", commissionMembers.commissionMember_role_name);
                            command.Parameters.AddWithValue("@protocolInfo_id", protocolInfo.id);
                            int result = command.ExecuteNonQuery();

                            if (result < 0)
                                MessageBox.Show("Ошибка добавления строки в базу данных! " + result.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    #endregion
                }
                //protocolInfo.commissionMembersList = (List<ProtocolInfo>)commissionMembers.protocolInfo;
                #region db DataXML
                dataXML.fileXml = fileEntries;
                query =
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
                        //D:\files\CODE\C#\Cours3_1\CouksMakovii\up\fcsPlacementResult_2400500000222000501_20375535.xml
                        string[] test = dataXML.fileXml.Split('\\');
                        string test2 = test[test.Length - 1];
                        dataXML.fileType = test2.Split('_')[0];
                        command.Parameters.AddWithValue("@typeXml", dataXML.fileType);
                        int result = command.ExecuteNonQuery();

                        if (result < 0)
                            MessageBox.Show("Ошибка добавления строки в базу данных! " + result.ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("ParseXml SelectNodes - " + ex.Message);
            }
        }
        #endregion
    }
}
