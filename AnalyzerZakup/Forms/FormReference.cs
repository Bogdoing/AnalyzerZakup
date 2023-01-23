using AnalyzerZakup.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnalyzerZakup.Forms
{
    public partial class FormReference : Form
    {
        private readonly string connectionString = DataApp.TxtBoxFileDB;
        //"Data Source=DESKTOP-432U1GM\\SQLEXPRESS;Initial Catalog=AnalizeXML;Integrated Security=True;MultipleActiveResultSets=True;"; //AnalizeXML

        string contentTable = "";
        public FormReference(string content)
        {
            InitializeComponent();
            contentTable = content;
        }

        protected override void OnResizeBegin(EventArgs e)
        {
            SuspendLayout();
            base.OnResizeBegin(e);
        }
        protected override void OnResizeEnd(EventArgs e)
        {
            ResumeLayout();
            base.OnResizeEnd(e);
        }
        private void FormReference_Load(object sender, EventArgs e)
        {
            textBox1.Text = contentTable;

            string sq = @"select distinct(cm.memberNumber), cm.lastName, cm.firstName, cm.middleName, cm.commissionRole
                    from commissionMember cm, protocolInfo p
                    where p.id = cm.protocolInfo_id and p.commissionName = '" + contentTable + "'";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand comm = new SqlCommand(sq, connection);
                    DataTable table = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(comm);
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                    dataGridView1.Columns[0].Visible = true;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    connection.Close();
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "using error", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }
    }
}
