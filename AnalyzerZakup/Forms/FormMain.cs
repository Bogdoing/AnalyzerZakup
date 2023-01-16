using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZakupAnaliser;

namespace AnalyzerZakup.Forms
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            tabControl1.Visible = false;
			panel2.Visible = true;

            textBox1.Text = 
@"create table applicationInfo (
	id int not null identity,
	appNumber int,
	appDT dateTime,
	finalPrice float,
	primary key(id)
)

create table commonInfo(
	id int not null identity,
	purchaseNumber varchar(max),
	docNumber varchar(max),
	docPublishDTInEIS dateTime,
	primary key(id),
)

create table protocolPublisherInfo(
	id int not null identity,
	regNum varchar(max),
	fullName varchar(max),
	factAddress varchar(max),
	INN varchar(max),
	KPP varchar(max),
	primary key(id)
)

create table dataXml (
	id int not null identity,
	fileXml xml,
	typeXml varchar(max),
	primary key(id)
)


create table protocolInfo (
	id int not null identity,
	commissionName varchar(max),
	primary key(id)
)

create table commissionMember (
	id int not null identity,
	memberNumber int,
	lastName varchar(max),
	firstName varchar(max),
	middleName varchar(max),
	commissionRole varchar(max),
	protocolInfo_id int,
	primary key(id),
	foreign key (protocolInfo_id) references protocolInfo(id)
)";
			textBox2.Text =
@"select distinct(p.fullName) as 'Полное наименование', 
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
		where c.id = a.id and c.id = p.id and c.id = pr.id

select distinct(rtrim(appDT)) as appDT
        from applicationInfo  union all (select '' as appDT) 
        order by appDT;
select distinct(rtrim(docPublishDTInEIS)) as docPublishDTInEIS 
        from commonInfo union all (select '' as docPublishDTInEIS) 
        order by docPublishDTInEIS;
select distinct(rtrim(docNumber)) as docNumber
        from commonInfo  union all (select '' as docNumber) 
        order by docNumber;
select distinct(typeXml) from dataXml
        union all (select '' as typeXml)
        order by typeXml;

select distinct(p.fullName) as 'Полное наименование', pr.commissionName as 'Название комиссии', 
	a.finalPrice as 'Цена',  a.appDT as 'Дата публикации', c.docPublishDTInEIS as 'Дата оплаты',
	p.INN, p.KPP, p.factAddress as 'Фактический адрес', c.purchaseNumber as 'Номер документа', 
	c.docNumber as 'Вид документа', p.regNum as 'Рег. номер'
		from applicationInfo a, commonInfo c, protocolPublisherInfo p, protocolInfo pr
		where c.id = a.id and c.id = p.id and c.id = pr.id and c.docPublishDTInEIS 
		between '1999/01/01' and '"" + comboBox2.Text + ""'"";

select distinct(p.fullName) as 'Полное наименование', pr.commissionName as 'Название комиссии',
    a.finalPrice as 'Цена',  a.appDT as 'Дата публикации',
    c.docPublishDTInEIS as 'Дата оплаты',
    p.INN, p.KPP, p.factAddress as 'Фактический адрес', 
    c.purchaseNumber as 'Номер документа', 
    c.docNumber as 'Вид документа', p.regNum as 'Рег. номер'
        from applicationInfo a, commonInfo c, protocolPublisherInfo p, protocolInfo pr
        where c.id = a.id and c.id = p.id and c.id = pr.id and a.appDT 
		between '"" + comboBox1.Text + ""' and '"" + comboBox2.Text + ""' and 
		c.docPublishDTInEIS between '"" + comboBox1.Text + ""' and '"" + comboBox2.Text + ""'""

select distinct(p.fullName) as 'Полное наименование', pr.commissionName as 'Название комиссии', 
	a.finalPrice as 'Цена',  a.appDT as 'Дата публикации', c.docPublishDTInEIS as 'Дата оплаты',
	p.INN, p.KPP, p.factAddress as 'Фактический адрес', c.purchaseNumber as 'Номер документа', 
	c.docNumber as 'Вид документа', p.regNum as 'Рег. номер'
		from applicationInfo a, commonInfo c, protocolPublisherInfo p, protocolInfo pr
		where c.id = a.id and c.id = p.id and c.id = pr.id 
		and a.appDT between '"" + comboBox1.Text + ""' and '2052.30.12'""

select distinct(p.fullName) as 'Полное наименование', pr.commissionName as 'Название комиссии',
	a.finalPrice as 'Цена',  a.appDT as 'Дата публикации', c.docPublishDTInEIS as 'Дата оплаты',
	p.INN, p.KPP, p.factAddress as 'Фактический адрес', c.purchaseNumber as 'Номер документа', 
	c.docNumber as 'Вид документа', p.regNum as 'Рег. номер'
		from applicationInfo a, commonInfo c, protocolPublisherInfo p, protocolInfo pr
		where c.id = a.id and c.id = p.id and c.id = pr.id and c.docNumber = '"" + comboBox3.Text + ""'""

select distinct(x.typeXml) as 'Тип протокола', p.fullName as 'Полное наименование', 
	pr.commissionName as 'Название комиссии', a.finalPrice as 'Цена', 
	a.appDT as 'Дата публикации', c.docPublishDTInEIS as 'Дата оплаты',
	p.factAddress as 'Фактический адрес', c.docNumber as 'Вид документа'
		from applicationInfo a, commonInfo c, protocolPublisherInfo p, protocolInfo pr, dataXml x
		where c.id = a.id and c.id = p.id and c.id = pr.id and x.typeXml = '"" + comboBox4.Text + ""'""
";
			textBox3.Text =
@"insert into applicationInfo (appNumber, appDT, finalPrice) 
	values 
	(@appNumber, @appDT, @finalPrice)


insert into commonInfo (purchaseNumber , docNumber , docPublishDTInEIS) 
	values 
	(@purchaseNumber , @docNumber , @docPublishDTInEIS)

insert into protocolPublisherInfo (regNum, fullName, factAddress, INN, KPP) 
	values 
	(@regNum, @fullName, @factAddress, @INN, @KPP)


insert into commissionMember (memberNumber, lastName, firstName, middleName,
		[commissionRole]) 
	values 
	(@memberNumber, @lastName, @firstName, @middleName, @[commissionRole])


insert into protocolInfo (commissionName, commissionMember_id ) 
	values 
	(@commissionName, @commissionMember_id )

DECLARE @fileXml xml;
SELECT @fileXml  = (SELECT * FROM OPENROWSET (BULK 'НАШ ПУТЬ К ФАЙЛУ ИЗ C#', SINGLE_BLOB) as [xml])

insert into dataXml (fileXml, typeXml) 
	values 
	(@fileXml, @typeXml)

";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1= new Form1();  
            form1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormList formList = new FormList();
            formList.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tabControl1_VisibleChanged(object sender, EventArgs e)
        {
			//if (tabControl1.Visible == true) tabControl1.Visible = false;
			//else tabControl1.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
			if (tabControl1.Visible == true) { 
				tabControl1.Visible = false; 
				panel2.Visible = true;
			}
			else { 
				tabControl1.Visible = true; 
				panel2.Visible = false;
			}
        }
    }
}
