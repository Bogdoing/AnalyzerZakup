using AnalyzerZakup.Data;
using AnalyzerZakup.Forms;
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
        private const string connectionString =
                "Data Source=DESKTOP-432U1GM\\SQLEXPRESS;Initial Catalog=AnalizeXML;Integrated Security=True;MultipleActiveResultSets=True;"; //AnalizeXML

        public FormList()
        {
            InitializeComponent();
            
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

        private void FormList_Load(object sender, EventArgs e)
        {
            string query_all = @"select * from commissionMember";
            query_all = @"select
	                    distinct(p.fullName) as 'Полное наименование', 
	                    pr.commissionName as 'Название комиссии', 
	                    a.finalPrice as 'Цена',  
	                    a.appDT as 'Дата публикации', 
	                    c.docPublishDTInEIS as 'Дата оплаты',  
	                    p.INN, p.KPP, 
	                    p.factAddress as 'Фактический адрес', 
	                    c.purchaseNumber as 'Номер документа', 
	                    c.docNumber as 'Вид документа', 
	                    p.regNum as 'Рег. номер'
                    from applicationInfo a, commonInfo c, protocolPublisherInfo p, protocolInfo pr
                    where c.id = a.id and c.id = p.id and c.id = pr.id";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand comm = new SqlCommand(query_all, connection);
                    DataTable table = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(comm);
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                    dataGridView1.Columns[0].Visible = true;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    connection.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            string query_cmb_d1 = @"select distinct(rtrim(appDT)) as appDT
                            from applicationInfo  union all (select '' as appDT) 
                            order by appDT";
            string query_cmb_d2 = @"select distinct(rtrim(docPublishDTInEIS)) as docPublishDTInEIS 
                            from commonInfo union all (select '' as docPublishDTInEIS) 
                            order by docPublishDTInEIS";
            string query_cmb_d3 = @"select distinct(rtrim(docNumber)) as docNumber
                            from commonInfo  union all (select '' as docNumber) 
                            order by docNumber";
            string query_cmb_d4 = @"select distinct(typeXml) from dataXml
                            union all (select '' as typeXml)
                            order by typeXml";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand comm = new SqlCommand(query_cmb_d1, connection);
                    DataTable stable = new DataTable();
                    SqlDataAdapter sadapter = new SqlDataAdapter(comm);
                    sadapter.Fill(stable);
                    comboBox1.DataSource = stable;
                    comboBox1.DisplayMember = "appDT";
                    //comboBox1.ValueMember = "id";
                    connection.Close();
                }
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand comm = new SqlCommand(query_cmb_d2, connection);
                    DataTable stable = new DataTable();
                    SqlDataAdapter sadapter = new SqlDataAdapter(comm);
                    sadapter.Fill(stable);
                    comboBox2.DataSource = stable;
                    comboBox2.DisplayMember = "docPublishDTInEIS";
                    //comboBox2.ValueMember = "id";
                    connection.Close();
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand comm = new SqlCommand(query_cmb_d3, connection);
                    DataTable stable = new DataTable();
                    SqlDataAdapter sadapter = new SqlDataAdapter(comm);
                    sadapter.Fill(stable);
                    comboBox3.DataSource = stable;
                    comboBox3.DisplayMember = "docNumber";
                    //comboBox3.ValueMember = "id";
                    connection.Close();
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand comm = new SqlCommand(query_cmb_d4, connection);
                    DataTable stable = new DataTable();
                    SqlDataAdapter sadapter = new SqlDataAdapter(comm);
                    sadapter.Fill(stable);
                    comboBox4.DataSource = stable;
                    comboBox4.DisplayMember = "typeXml";
                    //comboBox3.ValueMember = "id";
                    connection.Close();
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information); }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query_xml_serializ = @"select fileXml from dataXml";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query_xml_serializ, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows) // если есть данные
                    {
                        // выводим названия столбцов
                        Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));

                        while (reader.Read()) // построчно считываем данные
                        {
                            object id = reader.GetValue(0);
                            object name = reader.GetValue(1);
                            object age = reader.GetValue(2);

                            Console.WriteLine("{0} \t{1} \t{2}", id, name, age);
                        }
                    }
                    reader.Close();
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            //FormSerialization fromSerializatio = new FormSerialization(); //fname

            //fromSerializatio.ShowDialog();
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query_cmb_sort = "";
            if (comboBox1.Text == string.Empty)
            {
                if (comboBox2.Text == string.Empty)
                {
                    query_cmb_sort = @"select
	                    distinct(p.fullName) as 'Полное наименование', 
	                    pr.commissionName as 'Название комиссии', 
	                    a.finalPrice as 'Цена',  
	                    a.appDT as 'Дата публикации', 
	                    c.docPublishDTInEIS as 'Дата оплаты',  
	                    p.INN, p.KPP, 
	                    p.factAddress as 'Фактический адрес', 
	                    c.purchaseNumber as 'Номер документа', 
	                    c.docNumber as 'Вид документа', 
	                    p.regNum as 'Рег. номер'
                    from applicationInfo a, commonInfo c, protocolPublisherInfo p, protocolInfo pr
                    where c.id = a.id and c.id = p.id and c.id = pr.id";
                }
                else if (comboBox2.Text != string.Empty)
                {
                    query_cmb_sort = @"select
                distinct(p.fullName) as 'Полное наименование', pr.commissionName as 'Название комиссии', a.finalPrice as 'Цена',  a.appDT as 'Дата публикации', c.docPublishDTInEIS as 'Дата оплаты',
                p.INN, p.KPP, p.factAddress as 'Фактический адрес', c.purchaseNumber as 'Номер документа', c.docNumber as 'Вид документа', p.regNum as 'Рег. номер'
                from applicationInfo a, commonInfo c, protocolPublisherInfo p, protocolInfo pr
                where c.id = a.id and c.id = p.id and c.id = pr.id and c.docPublishDTInEIS between '1999/01/01' and '" + comboBox2.Text + "'";
                }
            }
            else
            if (comboBox1.Text != string.Empty)
            {
                if (comboBox2.Text != string.Empty)
                {
                    query_cmb_sort =
                        @"select distinct(p.fullName) as 'Полное наименование', pr.commissionName as 'Название комиссии',
                            a.finalPrice as 'Цена',  a.appDT as 'Дата публикации',
                            c.docPublishDTInEIS as 'Дата оплаты',
                            p.INN, p.KPP, p.factAddress as 'Фактический адрес', 
                            c.purchaseNumber as 'Номер документа', 
                            c.docNumber as 'Вид документа', p.regNum as 'Рег. номер'
                                from applicationInfo a, commonInfo c, protocolPublisherInfo p, protocolInfo pr
                                where c.id = a.id and c.id = p.id and c.id = pr.id and a.appDT 
between '" + comboBox1.Text + "' and '" + comboBox2.Text + "' and c.docPublishDTInEIS between '" + comboBox1.Text + "' and '" + comboBox2.Text + "'";
                }
                else
                {
                    query_cmb_sort = @"select
                distinct(p.fullName) as 'Полное наименование', pr.commissionName as 'Название комиссии', a.finalPrice as 'Цена',  a.appDT as 'Дата публикации', c.docPublishDTInEIS as 'Дата оплаты',
                p.INN, p.KPP, p.factAddress as 'Фактический адрес', c.purchaseNumber as 'Номер документа', c.docNumber as 'Вид документа', p.regNum as 'Рег. номер'
                from applicationInfo a, commonInfo c, protocolPublisherInfo p, protocolInfo pr
                where c.id = a.id and c.id = p.id and c.id = pr.id and a.appDT between '" + comboBox1.Text + "' and '2052.30.12'";
                }
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand comm = new SqlCommand(query_cmb_sort, connection);
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

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            string query_cmb3_sort = "";
            if (comboBox3.Text != "")
            {
                query_cmb3_sort = @"select
                    distinct(p.fullName) as 'Полное наименование', pr.commissionName as 'Название комиссии', a.finalPrice as 'Цена',  a.appDT as 'Дата публикации', c.docPublishDTInEIS as 'Дата оплаты',
                    p.INN, p.KPP, p.factAddress as 'Фактический адрес', c.purchaseNumber as 'Номер документа', c.docNumber as 'Вид документа', p.regNum as 'Рег. номер'
                    from applicationInfo a, commonInfo c, protocolPublisherInfo p, protocolInfo pr
                    where c.id = a.id and c.id = p.id and c.id = pr.id and c.docNumber = '" + comboBox3.Text + "'";
            }
            else
            {
                query_cmb3_sort = @"select
	                    distinct(p.fullName) as 'Полное наименование', 
	                    pr.commissionName as 'Название комиссии', 
	                    a.finalPrice as 'Цена',  
	                    a.appDT as 'Дата публикации', 
	                    c.docPublishDTInEIS as 'Дата оплаты',  
	                    p.INN, p.KPP, 
	                    p.factAddress as 'Фактический адрес', 
	                    c.purchaseNumber as 'Номер документа', 
	                    c.docNumber as 'Вид документа', 
	                    p.regNum as 'Рег. номер'
                    from applicationInfo a, commonInfo c, protocolPublisherInfo p, protocolInfo pr
                    where c.id = a.id and c.id = p.id and c.id = pr.id";
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand comm = new SqlCommand(query_cmb3_sort, connection);
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            try
            {
                if (dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText.ToString() ==
                    "Название комиссии")
                {
                    FormReference formReference = new FormReference(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    formReference.Show();
                }
                else
                if (dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].HeaderText.ToString() ==
                    "Название комиссии")
                {
                    FormReference formReference = new FormReference(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    formReference.Show();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("cell content exp | " + exception);
                throw;
            }
        }

        private void comboBox4_TextChanged(object sender, EventArgs e)
        {
            string query_cmb4_sort = "";
            if (comboBox4.Text != "")
            {
                query_cmb4_sort = @"select
                    distinct(x.typeXml) as 'Тип протокола', p.fullName as 'Полное наименование', pr.commissionName as 'Название комиссии', a.finalPrice as 'Цена', 
                    a.appDT as 'Дата публикации', c.docPublishDTInEIS as 'Дата оплаты',
                    p.factAddress as 'Фактический адрес', c.docNumber as 'Вид документа'
                        from applicationInfo a, commonInfo c, protocolPublisherInfo p, protocolInfo pr, dataXml x
                        where c.id = a.id and c.id = p.id and c.id = pr.id and x.typeXml = '" + comboBox4.Text + "'";
            }
            else
            {
                query_cmb4_sort = @"select
	                    distinct(p.fullName) as 'Полное наименование', 
	                    pr.commissionName as 'Название комиссии', 
	                    a.finalPrice as 'Цена',  
	                    a.appDT as 'Дата публикации', 
	                    c.docPublishDTInEIS as 'Дата оплаты',  
	                    p.INN, p.KPP, 
	                    p.factAddress as 'Фактический адрес', 
	                    c.purchaseNumber as 'Номер документа', 
	                    c.docNumber as 'Вид документа', 
	                    p.regNum as 'Рег. номер'
                    from applicationInfo a, commonInfo c, protocolPublisherInfo p, protocolInfo pr
                    where c.id = a.id and c.id = p.id and c.id = pr.id";
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand comm = new SqlCommand(query_cmb4_sort, connection);
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
