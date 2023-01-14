using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace AnalyzerZakup
{
    public partial class FormList : Form
    {
        NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=5432;User Id=custom_adm;Password=61488871;Database=custom_test;");
        public FormList()
        {
            InitializeComponent();
            
        }
        //public`void TestRead()
        //{

        //    OleDbConnection conn = new OleDbConnection(conn);
        //    OleDbCommand myCommand = new OleDbCommand();
        //    myCommand.Connection = conn;
        //    myCommand.CommandText = commandText;
        //    OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
        //    dataAdapter.SelectCommand = myCommand;
        //    DataSet ds = new DataSet();

        //    dataAdapter.TableMappings.Add("Table", "TOUR");

        //    OleDbCommand myCommand2 = new OleDbCommand();
        //    myCommand2.Connection = conn;
        //    myCommand2.CommandText = commandText2;
        //    OleDbDataAdapter dataAdapter2 = new OleDbDataAdapter();
        //    dataAdapter2.SelectCommand = myCommand2;
        //    dataAdapter2.TableMappings.Add("Table", "SEASON");

        //    conn.Open();
        //    dataAdapter.Fill(ds);
        //    dataAdapter2.Fill(ds);
        //    conn.Close();
        //    dataGrid1.DataSource = ds;
        //}
    }
}
