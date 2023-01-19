using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using System.Xml.Linq;
using AnalyzerZakup;
using AnalyzerZakup.Data;
using AnalyzerZakup.Parse;
using static System.Net.Mime.MediaTypeNames;

namespace AnalyzerZakup
{
    internal class ParseXML
    {
        private readonly string connectionString = DataApp.TxtBoxFileDB;
                //"Data Source=DESKTOP-432U1GM\\SQLEXPRESS;Initial Catalog=AnalizeXml2;Integrated Security=True;MultipleActiveResultSets=True;"; //AnalizeXML
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
            

            //MessageBox.Show("Parsing" + fileEntriesProtocol);
            // IEOK           
            // Parse epProtocol IEF
            string[] fileEntriesProtocol = Directory.GetFiles(DataApp.TxtBoxfilepath, "epProtocol*.xml");
            ParseProtocol parseProtocol = new ParseProtocol();
            for (int i = 1; i < fileEntriesProtocol.Length; i++) // !!! i = 0 !!!
            {
                parseProtocol.Parse_Protocol(fileEntriesProtocol[i]);
                //MessageBox.Show("Parsing" + fileEntriesProtocol[i]);   
            }

            // Parse Contract
            string[] fileEntriesContract = Directory.GetFiles(DataApp.TxtBoxfilepath, "contract*.xml");
            ParseContract parseContract = new ParseContract();
            for (int i = 1; i < fileEntriesContract.Length; i++) // !!! i = 0 !!!
            {
                parseContract.Parse_contracts(fileEntriesContract[i]);                
                //MessageBox.Show("Parsing" + fileEntriesProtocol[i]);   
            }

            // Parse Cusromer 
            string[] fileEntriesCusromer = Directory.GetFiles(DataApp.TxtBoxfilepath, "fcsCustomer*.xml");
            ParseCustomerreport parseCustomerreport = new ParseCustomerreport();
            for (int i = 1; i < fileEntriesCusromer.Length; i++) // !!! i = 0 !!!
            {
                parseCustomerreport.Parse_customerreport(fileEntriesCusromer[i]);
                //MessageBox.Show("Parsing" + fileEntriesProtocol[i]);   
            }

            // Parse Purchasedoc
            string[] fileEntriesPurchasedoc = Directory.GetFiles(DataApp.TxtBoxfilepath, "fcsPurchase*.xml");
            ParsePurchasedoc parsePurchasedoc = new ParsePurchasedoc();
            for (int i = 1; i < fileEntriesPurchasedoc.Length; i++) // !!! i = 0 !!!
            {
                parsePurchasedoc.Parse_purchasedoc(fileEntriesPurchasedoc[i]);
                //MessageBox.Show("Parsing" + fileEntriesProtocol[i]);   
            }

            // Parse SkathcPlan
            string[] fileEntriesSkathcPlan = Directory.GetFiles(DataApp.TxtBoxfilepath, "SketchPlan*.xml");
            ParseSketchplan parseSketchplan = new ParseSketchplan();
            for (int i = 1; i < fileEntriesSkathcPlan.Length; i++) // !!! i = 0 !!!
            {
                parseSketchplan.Parse_sketchplan(fileEntriesSkathcPlan[i]);
                //MessageBox.Show("Parsing" + fileEntriesProtocol[i]);   
            }

            ParseNotification parseNotification = new ParseNotification();
            string[] fileEntriesNotification = Directory.GetFiles(DataApp.TxtBoxfilepath, "epNotificationE*.xml");            
            for (int i = 1; i < fileEntriesNotification.Length; i++) // !!! i = 0 !!!
            {
                parseNotification.Parse_Notification(fileEntriesSkathcPlan[i]);   
            }
            string[] fileEntriesClarification = Directory.GetFiles(DataApp.TxtBoxfilepath, "epClarification*.xml");
            for (int i = 1; i < fileEntriesNotification.Length; i++) // !!! i = 0 !!!
            {
                parseNotification.Parse_Clarification(fileEntriesClarification[i]);   
            }

            MessageBox.Show("End parsing all");
        }

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
//if (fileEntries[i].StartsWith("f")) // (person.ToUpper().StartsWith("T"))
//{
//    MessageBox.Show("f - " + fileEntries[i]);
//}
//MessageBox.Show("any file - " + fileEntries[i]);
//var _id = docXML.DocumentElement.SelectNodes("//ns9:id", _namespaceManager)[0].InnerText;

//var _foundationDocNumber = docXML.DocumentElement.SelectNodes(
//    "//ns9:foundationDocInfo/ns9:foundationDocNumber",
//    _namespaceManager)[0]
//    .InnerText;

// commonInfo all
//common_Info.commonInfo = docXML.DocumentElement.SelectNodes(
//    "//ns9:commonInfo",
//    _namespaceManager)[0]
//    .InnerText;
