using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AnalyzerZakup.Forms
{
    public partial class FormSerialization : Form
    {
        public FormSerialization(string fname)
        {
            InitializeComponent();

            //showXml(fname);
        }
        //public showXml(fname)
        //{
        //    OpenFileDialog dg = new OpenFileDialog();
        //    dg.Filter = "XML";
        //    dg.FileName = "";
        //    dg.ShowDialog();
        //    fname = dg.FileName;
        //    if (fname == "") return;
        //    string readfile = File.ReadAllText(fname);
        //    textBox1.Text = readfile;
        //}
    }
}
