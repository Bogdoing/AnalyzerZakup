using AnalyzerZakup.Data;
using AnalyzerZakup.Function;
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
        Search search = new Search();
        DataXML dataXML = new DataXML();
        СheckType сheckType = new СheckType();
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
                //ns9:id
                dataXML.id = int.Parse(docXML.DocumentElement.SelectNodes(
                       "//ns9:id",
                       _namespaceManager)[0]
                       .InnerText);

                dataXML.fileXml = fileEntries;

                query = @"DECLARE @fileXml xml; 
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
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("ParseXml SelectNodes - " + ex.Message);
            }
        }
        #endregion

        public void Parse_Protocol2(string fileEntries)
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
                dataXML.id = int.Parse(docXML.DocumentElement.SelectNodes(
                    "//ns9:id",
                    _namespaceManager)[0]
                    .InnerText);

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
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                SqlCommand command = new SqlCommand(query_insert, connection);
                                command.Connection.Open();
                                command.Parameters.AddWithValue("@nameType", dataXML.fileType);
                                int result = command.ExecuteNonQuery();
                                if (result < 0) MessageBox.Show("Ошибка добавления строки в базу данных INSERT nameType! " + result.ToString());
                            }
                            query_insert = @"insert into typeDocumentTag (typeDocumentId, tagId) values (@typeDocumentId, 24)";
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

                        #region Parse
                        ProtocolInfo protocolInfo = new ProtocolInfo();
                        CommonInfo common_Info = new CommonInfo();
                        ProtocolPublisherInfo protocolPublisherInfo = new ProtocolPublisherInfo();
                        CommissionMembers commissionMembers = new CommissionMembers();
                        ApplicationInfo applicationInfo = new ApplicationInfo();

                        var cnt_commissionname = docXML.DocumentElement.SelectNodes(
                            "//ns9:commissionInfo/ns3:commissionName",
                            _namespaceManager).Count;
                        if (cnt_commissionname != 0)
                            protocolInfo.commissionName = docXML.DocumentElement.SelectNodes(
                            "//ns9:commissionInfo/ns3:commissionName",
                            _namespaceManager)[0]
                            .InnerText;
                        else protocolInfo.commissionName = "Не определена";
                        //
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
                        //
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
                        //
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

                        var commissionMember = docXML.DocumentElement.SelectNodes(
                            "//ns3:commissionMembers/ns3:commissionMember",
                            _namespaceManager)
                            .Count.ToString();


                        addDB(fileEntries, dataXML.id.ToString(), 24);
                        addDB(fileEntries, protocolInfo.commissionName.ToString(), 6);
                        addDB(fileEntries, common_Info.commonInfo_purchaseNumber.ToString(), 12);
                        addDB(fileEntries, common_Info.commonInfo_publishDTInEIS.ToString(), 14);
                        addDB(fileEntries, protocolPublisherInfo.protocol_regNum.ToString(), 11);
                        addDB(fileEntries, protocolPublisherInfo.protocol_fullName.ToString(), 7);
                        addDB(fileEntries, protocolPublisherInfo.protocol_factAddress.ToString(), 8);
                        addDB(fileEntries, protocolPublisherInfo.protocol_INN.ToString(), 9);
                        addDB(fileEntries, protocolPublisherInfo.protocol_KPP.ToString(), 10);
                        addDB(fileEntries, applicationInfo.applicationInfo_appNumber.ToString(), 18);
                        addDB(fileEntries, applicationInfo.applicationInfo_appDT.ToString(), 19);
                        addDB(fileEntries, applicationInfo.applicationInfo_finalPrice.ToString(), 20);

                        #endregion
                    }
                    else { /*MessageBox.Show("db have fhis file - " + fileEntries);*/ }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "error db all", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "error parse contract", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }


        private void addDB(string fileEntries, string value, int tagId)
        {
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
            catch (Exception ex) { MessageBox.Show(ex.Message, "error db Select nameType addDB", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            string query = @"DECLARE @fileX xml;
                    SELECT @fileX = (SELECT * FROM OPENROWSET(BULK '" + dataXML.fileXml + "', SINGLE_BLOB) as [xml])" +
                        "insert into document(name, fileXml, typeDocumentId)" +
                        "values" +
                        "(@name, @fileX, @typeDocumentId)";
            string query_Value = @"insert into documentTag (documentId, tagId, value) 
                            values 
                            (@documentId, @tagId, @value)";

            List<string> dbFilename = search.SearchDB();
            if (search.FiendList(dataXML.nameFile, dbFilename))
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Connection.Open();
                        command.Parameters.AddWithValue("@name", dataXML.nameFile);
                        command.Parameters.AddWithValue("@typeDocumentId", 1);

                        int result = command.ExecuteNonQuery();
                        if (result < 0) MessageBox.Show("Ошибка добавления строки в базу данных OPENROWSET addDB! " + result.ToString());
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "addDB error db insert into documen", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                //MessageBox.Show("test db insert value");
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        //MessageBox.Show(сheckType.setDocumentIBdb() + " - сheckType.setDocumentIBdb() - /  " + value);
                        SqlCommand command = new SqlCommand(query_Value, connection);
                        command.Connection.Open();
                        command.Parameters.AddWithValue("@documentId", 1); //int.Parse(сheckType.setDocumentIBdb())
                        command.Parameters.AddWithValue("@tagId", tagId);
                        command.Parameters.AddWithValue("@value", value);
                        int result = command.ExecuteNonQuery();
                        if (result < 0) MessageBox.Show("addDB Ошибка добавления строки в базу данных OPENROWSET! " + result.ToString());
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "addDB error db insert into documen", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            }
            else { /*MessageBox.Show("addDB db have fhis file - " + fileEntries);*/ }

        }
    }
}
