using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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
            //foreach (string file in fileEntriesProtocol) MessageBox.Show(file + "");
            ParseProtocol parseProtocol = new ParseProtocol();
            for (int i = 1; i < fileEntriesProtocol.Length; i++) // !!! i = 0 !!!
            {
                parseProtocol.Parse_Protocol(fileEntriesProtocol[i]);
                //MessageBox.Show("Parsing" + fileEntriesProtocol[i]);   
            }

            // Parse Contract
            string[] fileEntriesContract = Directory.GetFiles(DataApp.TxtBoxfilepath, "contract*.xml");
            //foreach (string file in fileEntriesContract) MessageBox.Show(file + "");
            ParseContract parseContract = new ParseContract();
            for (int i = 1; i < fileEntriesContract.Length; i++) // !!! i = 0 !!!
            {
                if (fileEntriesContract[i].IndexOf("Cancel") >= 1)//(fileEntriesContract[i] == "contractProcedureCancel*.xml")
                {
                    MessageBox.Show("Cansel - " + fileEntriesContract[i]);
                    parseContract.Parse_contracts_Cansel(fileEntriesContract[i]);
                }
                else // Parse contract proces
                {
                    MessageBox.Show("Else - " + fileEntriesContract[i]);
                    parseContract.Parse_contracts(fileEntriesContract[i]);
                }            
                //MessageBox.Show("Parsing" + fileEntriesProtocol[i]);   
            }

            // Parse Cusromer 
            string[] fileEntriesCusromer = Directory.GetFiles(DataApp.TxtBoxfilepath, "fcsCustomer*.xml");
            //foreach (string file in fileEntriesCusromer) MessageBox.Show(file + "");
            ParseCustomerreport parseCustomerreport = new ParseCustomerreport();
            for (int i = 1; i < fileEntriesCusromer.Length; i++) // !!! i = 0 !!!
            {
                parseCustomerreport.Parse_customerreport(fileEntriesCusromer[i]);
                //MessageBox.Show("Parsing" + fileEntriesProtocol[i]);   
            }

            // Parse Purchasedoc
            string[] fileEntriesPurchasedoc = Directory.GetFiles(DataApp.TxtBoxfilepath, "fcsPurchase*.xml");
            //foreach (string file in fileEntriesPurchasedoc) MessageBox.Show(file + "");
            ParsePurchasedoc parsePurchasedoc = new ParsePurchasedoc();
            for (int i = 1; i < fileEntriesPurchasedoc.Length; i++) // !!! i = 0 !!!
            {
                parsePurchasedoc.Parse_purchasedoc(fileEntriesPurchasedoc[i]);
                //MessageBox.Show("Parsing" + fileEntriesProtocol[i]);   
            }

            // Parse SkathcPlan
            string[] fileEntriesSkathcPlan = Directory.GetFiles(DataApp.TxtBoxfilepath, "SketchPlan*.xml");
            //foreach (string file in fileEntriesSkathcPlan) MessageBox.Show(file + "")
            ParseSketchplan parseSketchplan = new ParseSketchplan();
            for (int i = 1; i < fileEntriesSkathcPlan.Length; i++) // !!! i = 0 !!!
            {
                parseSketchplan.Parse_sketchplan(fileEntriesSkathcPlan[i]);
                //MessageBox.Show("Parsing" + fileEntriesProtocol[i]);   
            }

            ParseNotification parseNotification = new ParseNotification();
            string[] fileEntriesNotification = Directory.GetFiles(DataApp.TxtBoxfilepath, "epNotificationE*.xml");
            //foreach (string file in fileEntriesNotification) MessageBox.Show(file + "");
            for (int i = 1; i < fileEntriesNotification.Length; i++) // !!! i = 0 !!!
            {
                parseNotification.Parse_Notification(fileEntriesSkathcPlan[i]);   
            }
            string[] fileEntriesClarification = Directory.GetFiles(DataApp.TxtBoxfilepath, "epClarification*.xml");
            //foreach (string file in fileEntriesClarification) MessageBox.Show(file + "");
            for (int i = 1; i < fileEntriesNotification.Length; i++) // !!! i = 0 !!!
            {
                parseNotification.Parse_Clarification(fileEntriesClarification[i]);   
            }

            MessageBox.Show("End parsing all");
        }
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
