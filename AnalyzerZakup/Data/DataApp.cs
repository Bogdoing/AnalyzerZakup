using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerZakup
{
    static class DataApp
    {
        public static string TxtBoxfilepath { get; set; }            

        public static string TxtBoxFileDB = 
        "Data Source=DESKTOP-432U1GM\\SQLEXPRESS;Initial Catalog=AnalizeXml2;Integrated Security=True;MultipleActiveResultSets=True;"; //AnalizeXML

        public static bool checkBox1 { get; set; }
        public static bool checkBox2 { get; set; }
        public static bool checkBox3 { get; set; }
        public static bool checkBox4 { get; set; }
        public static bool checkBox5 { get; set; }
        public static bool checkBox6 { get; set; }

        public static string region { get; set; }
    }
}
