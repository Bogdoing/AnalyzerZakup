using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerZakup.Data
{
    internal class DataXML
    {
    }
    static class ProtocolInfo
    {
        public static string id { get; set;}
        public static string foundationDocNumber { get; set; }
    }
    static class CommonInfo
    {
        public static string commonInfo { get; set; }
        public static string commonInfo_purchaseNumber { get; set; }
        public static string commonInfo_docNumber { get; set; }
        public static string commonInfo_procedureDT { get; set; }
    }
    static class ProtocolPublisherInfo
    {
        public static string _protocol_regNum { get; set; }
        public static string _protocol_fullName { get; set; }
        public static string _protocol_factAddress { get; set; }
        public static string _protocol_INN { get; set; }
        public static string _protocol_KPP { get; set; }
    }
    static class CommissionMembers
    {
        public static string commissionMember_memberNumber { get; set; }
        public static string commissionMember_lastName { get; set; }
        public static string commissionMember_firstName { get; set; }
        public static string commissionMember_middleName { get; set; }
        public static string commissionMember_role_name { get; set; }
    }
    static class ApplicationInfo
    {
        public static string applicationInfo_appNumber { get; set; }
        public static string applicationInfo_appDT { get; set; }
        public static string applicationInfo_finalPrice { get; set; }

        public static float strTofloat()
        {
            return float.Parse(applicationInfo_finalPrice.Replace(".", ","));
        }
    }

}
