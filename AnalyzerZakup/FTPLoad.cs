using BytesRoad.Net.Ftp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AnalyzerZakup
{
    internal class FTPLoad
    {
        public string MyDecodeDate(DateTime D)
        {
            return D.Year.ToString("D4") + D.Month.ToString("D2") + D.Day.ToString("D2");
        }

        public void UnZipFile(string sourceFile, string destPath)
        {
            Process iStartProcess = new Process(); // новый процесс
                                                    //iStartProcess.StartInfo.FileName = @"C:\Program Files\7-Zip\7z.exe"; // путь к запускаемому файлу
            iStartProcess.StartInfo.FileName = @"D:\programm\WinRar\WinRAR.exe";//D:\programm\WinRar\WinRAR.exe
            iStartProcess.StartInfo.Arguments = " x " + destPath + sourceFile + " -y -r -o" + destPath; // эта строка указывается, если программа запускается с параметрами 
                                                                                                        //iStartProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; // эту строку указываем, если хотим запустить программу в скрытом виде
            iStartProcess.Start(); // запускаем программу
            iStartProcess.WaitForExit(300000); // эту строку указываем, если нам надо будет ждать завершения программы определённое время
        }

        public void ftp_load(string filepath, string ftp, string login, string pass, TextBox tbx, DateTime D)
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
                        fitems = client.GetDirectoryList(TimeoutFTP, item.Name + "/protocols/currMonth");
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
            UnZipFile("*.zip", filepath);
            tbx.AppendText(DateTime.Now.ToString() + " Отработал 7-ZIP\n");


        }
    }
}
