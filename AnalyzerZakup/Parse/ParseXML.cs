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
        public void Parse_XML()
        {
            string[] fileEntriesAll = Directory.GetFiles(DataApp.TxtBoxfilepath);
            

            //MessageBox.Show("Parsing" + fileEntriesProtocol);
            // IEOK           
            // Parse epProtocol IEF
            string[] fileEntriesProtocol = Directory.GetFiles(DataApp.TxtBoxfilepath, "epProtocol*.xml");            
            ParseProtocol parseProtocol = new ParseProtocol();
            foreach (string file in fileEntriesProtocol) 
            {
                //MessageBox.Show(file + "");
                //parseProtocol.Parse_Protocol(file);
            }

            // Parse Contract
            string[] fileEntriesContract = Directory.GetFiles(DataApp.TxtBoxfilepath, "contract*.xml"); //contract*.xml            
            ParseContract parseContract = new ParseContract();
            foreach (string file in fileEntriesContract) 
            {
                if (file.IndexOf("Cancel") >= 1)//(file == "contractProcedureCancel*.xml")
                {
                    //MessageBox.Show("Cansel - " + file);
                    //parseContract.Parse_contracts_Cansel(file);
                }
                else // Parse contract proces
                {
                    //MessageBox.Show("Else - " + file);
                    parseContract.Parse_contracts(file);
                }
                //MessageBox.Show("Parsing" + file);  
                //MessageBox.Show(file + ""); 
            }

            // Parse Cusromer 
            string[] fileEntriesCusromer = Directory.GetFiles(DataApp.TxtBoxfilepath, "fcsCustomer*.xml");            
            ParseCustomerreport parseCustomerreport = new ParseCustomerreport();
            foreach (string file in fileEntriesCusromer) 
            {
                //MessageBox.Show(file + ""); 
                parseCustomerreport.Parse_customerreport(file);
            }

            // Parse Purchasedoc
            string[] fileEntriesPurchasedoc = Directory.GetFiles(DataApp.TxtBoxfilepath, "fcsPurchase*.xml");            
            ParsePurchasedoc parsePurchasedoc = new ParsePurchasedoc();
            foreach (string file in fileEntriesPurchasedoc)
            {
                //MessageBox.Show(file + ""); 
                parsePurchasedoc.Parse_purchasedoc(file);
            }

            // Parse SkathcPlan
            string[] fileEntriesSkathcPlan = Directory.GetFiles(DataApp.TxtBoxfilepath, "SketchPlan*.xml");            
            ParseSketchplan parseSketchplan = new ParseSketchplan();
            foreach (string file in fileEntriesSkathcPlan) 
            {
                //MessageBox.Show(file + "");
                parseSketchplan.Parse_sketchplan(file);
            }

            ParseNotification parseNotification = new ParseNotification();
            string[] fileEntriesNotification = Directory.GetFiles(DataApp.TxtBoxfilepath, "epNotificationE*.xml");
            foreach (string file in fileEntriesNotification) 
            {
                //MessageBox.Show(file + "");
                parseNotification.Parse_Notification(file);
            }
            string[] fileEntriesClarification = Directory.GetFiles(DataApp.TxtBoxfilepath, "epClarification*.xml");
            foreach (string file in fileEntriesClarification)
            {
                //MessageBox.Show(file + "");
                //parseNotification.Parse_Clarification(file);   
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
