using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml;
using Npgsql;
using BytesRoad.Net.Ftp;
using AnalyzerZakup;
using System.Xml.Linq;
using System.Threading;
using System.Data.SqlTypes;

namespace ZakupAnaliser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            DataApp.TxtBoxfilepath = textBox_filepath.Text;
            DataApp.TxtBoxFileZip = textBox_filezip.Text;


            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //LoadFTP lftp = new LoadFTP();
            //lftp.ftp_load(textBox_filepath.Text, textBox_ftp.Text, textBox_login.Text, textBox_password.Text, textBox1, dateTimePicker1.Value);
            FTPLoad lftp = new FTPLoad();
            lftp.ftp_load(DataApp.TxtBoxfilepath, textBox_ftp.Text, textBox_login.Text, textBox_password.Text, textBox1, dateTimePicker1.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpZip upZip = new UpZip();
            upZip.createXML();

            //AnalyseXML axml = new AnalyseXML();
            //axml.xml_analyse(DataApp.TxtBoxfilepath, textBox1, dateTimePicker1.Value);
            //Thread.Sleep(20000);
            ParseXML parseXML = new ParseXML();
            parseXML.Parse_XML();

            UpZip.delXML();
            //parseXML.Parse_ProtocolEF();
            //dataGridView1.DataSource = parseXML.ReadXml();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormList formList = new FormList();
            formList.ShowDialog();
        }

        /**/
        /**/
    }
    /*  */

    /* LASTED */
    public class LoadFTP
    {
        public string MyDecodeDate(DateTime D)
        {
        return D.Year.ToString("D4") + D.Month.ToString("D2") + D.Day.ToString("D2"); 
        }

        public void UnZipFile(String sourceFile, String destPath)
        {
            Process iStartProcess = new Process(); // новый процесс
            //iStartProcess.StartInfo.FileName = @"C:\Program Files\7-Zip\7z.exe"; // путь к запускаемому файлу
            iStartProcess.StartInfo.FileName = @"D:\programm\WinRar\WinRAR.exe";//D:\programm\WinRar\WinRAR.exe
            iStartProcess.StartInfo.Arguments = " x "+destPath+sourceFile + " -y -r -o"+destPath; // эта строка указывается, если программа запускается с параметрами 
            //iStartProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; // эту строку указываем, если хотим запустить программу в скрытом виде
            iStartProcess.Start(); // запускаем программу
            iStartProcess.WaitForExit(300000); // эту строку указываем, если нам надо будет ждать завершения программы определённое время
        }

        public void ftp_load(string filepath, String ftp, String login, String pass, TextBox tbx, DateTime D )
        {

            try
            {
                // Удаляем все файлы zip
                FileInfo[] path = new DirectoryInfo(filepath).GetFiles("protocol*.zip", SearchOption.TopDirectoryOnly);
                foreach (FileInfo file in path)
                {
                    File.Delete(file.FullName);
                }

                // Удаляем все файлы xml
                path = new DirectoryInfo(filepath).GetFiles("*.xml", SearchOption.TopDirectoryOnly);
                foreach (FileInfo file in path)
                {
                    File.Delete(file.FullName);
                }

                // Удаляем все файлы sig
                path = new DirectoryInfo(filepath).GetFiles("*.sig", SearchOption.TopDirectoryOnly);
                foreach (FileInfo file in path)
                {
                    File.Delete(file.FullName);
                }


                //Сам клиент ФТП
                FtpClient client = new FtpClient();

                //Задаём параметры клиента.
                client.PassiveMode = true; //Включаем пассивный режим.
                int TimeoutFTP = 10000; //Таймаут.
                int FTP_PORT = 21;

                //Подключаемся к FTP серверу.
                client.Connect(TimeoutFTP, ftp, FTP_PORT);
                client.Login(TimeoutFTP, login, pass);

                client.ChangeDirectory(TimeoutFTP, "fcs_regions");

                FtpItem[] items = client.GetDirectoryList(TimeoutFTP, null);

                Regex regexmask = new Regex(@"^protocol\w*" + MyDecodeDate(D) + @"\w*_" + MyDecodeDate(D.AddDays(1)) + @"\w*.xml.zip$");

                FtpItem[] fitems;

                foreach (FtpItem item in items)
                {
                    if (FtpItemType.Directory == item.ItemType)
                    {
                        tbx.AppendText(item.Name + "\n");
                        //client.ChangeDirectory(TimeoutFTP, item.Name+"/protocols/currMonth");
                        fitems = client.GetDirectoryList(TimeoutFTP, item.Name+"/protocols/currMonth");
                        foreach (FtpItem fitem in fitems)
                        {
                            if (FtpItemType.File == fitem.ItemType)
                            {
                              if (regexmask.IsMatch(fitem.Name))
                              {
                                  client.GetFile(TimeoutFTP, filepath + fitem.Name, item.Name + "/protocols/currMonth/" + fitem.Name);
                                tbx.AppendText(fitem.Name + "\n");
                              }
                            }
                        }
                        Array.Clear(fitems, 0, 0);
                    }
                   
                }

                client.Disconnect(TimeoutFTP);
                //tbx.AppendText(DateTime.Now.ToString()+" "+"message\n");

            }

           
            catch (Exception fError)
            {
                tbx.AppendText(DateTime.Now.ToString() + " Ошибка: " + fError.Message + "\n");
            }


            // Распаковываем архивы

            tbx.AppendText(DateTime.Now.ToString() + " Работает 7-ZIP\n");
            //UnZipFile("*.zip", filepath);
            tbx.AppendText(DateTime.Now.ToString() + " Отработал 7-ZIP\n");
            

        }
    }

    class Purchase
    {
        public string number { get; set; }
        public string pub_name { get; set; }
        public string pub_inn { get; set; }
        public string pub_kpp { get; set; }
        public string price { get; set; }
    }

    public class AnalyseXML
    {
        public void Parse_ProtocolEF(string f, TextBox tbx, NpgsqlConnection conn, DateTime D)
        {
            Purchase pur = new Purchase();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(f);

            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {
                // обход всех узлов в корневом элементе
                foreach (XmlElement xnode in xRoot)
                {
                    if (xnode.Name == "ns2:epProtocolEF2020Final")
                    {
                        foreach (XmlNode childnode in xnode.ChildNodes)
                        {
                            if (childnode.Name == "ns9:commonInfo")    // общая информация
                            {
                                foreach (XmlNode childnode1 in childnode.ChildNodes)
                                {
                                    if (childnode1.Name == "ns9:purchaseNumber")  // номер закупки
                                    {
                                        pur.number = childnode1.InnerText;
                                        break;
                                    }
                                }

                            }
                            else if (childnode.Name == "ns9:protocolPublisherInfo")   // информация о заказчике
                            {
                                foreach (XmlNode childnode1 in childnode.ChildNodes)
                                {
                                    if (childnode1.Name == "ns9:publisherOrg")  // организация заказчик
                                    {
                                        foreach (XmlNode childnode2 in childnode1.ChildNodes)
                                        {
                                            if (childnode2.Name == "ns9:fullName")  // название заказчика
                                            {
                                                pur.pub_name = childnode2.InnerText;
                                            }
                                            else if (childnode2.Name == "ns9:INN")  // ИНН заказчика
                                            {
                                                pur.pub_inn = childnode2.InnerText;
                                            }
                                            else if (childnode2.Name == "ns9:KPP")  // КПП заказчика
                                            {
                                                pur.pub_kpp = childnode2.InnerText;
                                            }

                                        }
                                        break;
                                    }
                                }
                            }
                            else if (childnode.Name == "ns9:protocolInfo")   // Протокол
                            {
                                foreach (XmlNode childnode1 in childnode.ChildNodes)
                                {
                                    if (childnode1.Name == "ns9:applicationsInfo")   // информация о предложениях
                                    {
                                        foreach (XmlNode childnode2 in childnode1.ChildNodes)
                                        {
                                            if (childnode2.Name == "ns9:applicationInfo")  // предложение
                                            {
                                                string finp = null;
                                                string rating = null;
                                                foreach (XmlNode childnode3 in childnode2.ChildNodes)
                                                {
                                                    if (childnode3.Name == "ns9:finalPrice")  // окончательная цена
                                                    {
                                                        finp = childnode3.InnerText;
                                                    }
                                                    if (childnode3.Name == "ns9:admittedInfo")
                                                    {
                                                        foreach (XmlNode childnode4 in childnode3.ChildNodes)
                                                        {
                                                            if (childnode4.Name == "ns9:appAdmittedInfo")
                                                            {
                                                                foreach (XmlNode childnode5 in childnode4.ChildNodes)
                                                                {
                                                                    if (childnode5.Name == "ns9:appRating")   // место
                                                                    {
                                                                        rating = childnode5.InnerText;
                                                                    }

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if ((finp != null) && (rating != null))
                                                {
                                                    if (rating == "1")
                                                    {
                                                        pur.price = finp;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Сохраняем в БД
            if (pur.price != null)
            {

                try
                {
                    // не используется!
                    String TmpSQL;
                    TmpSQL = "insert into purchases (anum, pub_name, proto_date, pub_inn, pub_kpp, load_proto_date, winner_price) values ('"+
                             pur.number + "', "+"'" + pur.pub_name+"', " + D.ToString()+", "+
                             "'"+pur.pub_inn+"', "+
                             "'"+pur.pub_kpp+"', "+
                             D.ToString()+", "+ pur.price+")";

                    conn.Open();
                    NpgsqlCommand comm = new NpgsqlCommand("insert into users (fam) values ('ivanov')", conn);

                    comm.ExecuteNonQuery();

                    conn.Close();
                }
                catch (Exception fError)
                {
                    tbx.AppendText(DateTime.Now.ToString() + " PostgresQL error: " + fError.Message + "\n");
                }
              

                tbx.AppendText(DateTime.Now.ToString() + " " + pur.number + " " + pur.price + "\n");
            }
        }

        public void xml_analyse(String filepath, TextBox tbx, DateTime D)
        {
           // перебираем файлы по маске

           string[] allFoundFiles;

           NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=5432;User Id=custom_adm;Password=61488871;Database=custom_test;");

           allFoundFiles = Directory.GetFiles(filepath, "epProtocolEF2020Final_*.xml", SearchOption.TopDirectoryOnly);
           foreach (string ffile in allFoundFiles)
           {
               Parse_ProtocolEF(ffile, tbx, conn, D);
               
           }

            Array.Clear(allFoundFiles, 0, 0);
        }

        public void Parse_Notification(string f, TextBox tbx, NpgsqlConnection conn, DateTime D)
        {

        }
    }
}
