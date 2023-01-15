using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerZakup.Data
{
    internal class DataXML
    {
        public int id { get; set; }
        public string fileXml { get; set; }
        public string fileType { get; set; }
    }
    class ProtocolInfo
    {
        public int id { get; set;}
        public string foundationDocNumber { get; set; }

        public string protocolPublisherInfoId { get; set; }
        
        [ForeignKey("ProtocolInfo")]
        public CommissionMembers commissionMembers { get; set; }
    }
    class CommonInfo
    {
        public int id { get; set; }
        public  string commonInfo { get; set; }
        public  string commonInfo_purchaseNumber { get; set; }
        public  string commonInfo_docNumber { get; set; }
        public  string commonInfo_procedureDT { get; set; }
    }
     class ProtocolPublisherInfo
    {
        public int id { get; set; }
        public string _protocol_regNum { get; set; }
        public string _protocol_fullName { get; set; }
        public string _protocol_factAddress { get; set; }
        public string _protocol_INN { get; set; }
        public string _protocol_KPP { get; set; }
    }
    class CommissionMembers
    {
        public int id { get; set; }
        public string commissionMember_memberNumber { get; set; }
        public string commissionMember_lastName { get; set; }
        public string commissionMember_firstName { get; set; }
        public string commissionMember_middleName { get; set; }
        public string commissionMember_role_name { get; set; }

        
        public List<ProtocolInfo> protocolInfo { get; set; }

        [ForeignKey("DataXML")]
        public List<DataXML> dataXML { get; set; }
    }
    class ApplicationInfo
    {
        public int id { get; set; }
        public string applicationInfo_appNumber { get; set; }
        public string applicationInfo_appDT { get; set; }
        public string applicationInfo_finalPrice { get; set; }

        public float strTofloat()
        {
            return float.Parse(applicationInfo_finalPrice.Replace(".", ","));
        }
    }

}
