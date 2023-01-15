using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Diagnostics;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace AnalyzerZakup
{
    internal class UpZip
    {
        public void createXML()
        {
            try
            {
                string[] fileEntries = Directory.GetFiles(DataApp.TxtBoxfilepath); 
                for (int i = 0; i < fileEntries.Length; i++)
                {
                    //MessageBox.Show("Get Files " + fileEntries[i], fileEntries[i]);
                }

                for (int i = 0; i < fileEntries.Length; i++)
                {
                    //MessageBox.Show("Start uparhive " + i + " D.Z" + DataApp.TxtBoxFileZip + " D.f" + DataApp.TxtBoxfilepath);
                    ZipFile.ExtractToDirectory(fileEntries[i], DataApp.TxtBoxfilepath);
                }
                delFile();
            }
            catch (Exception e)
            {
                MessageBox.Show("failed UpZip:" + e.ToString());
            }

        }

        #region DELFILE
        public static void delFile()
        {
            //delXML();
            //delZIP();
            delSig();
            delFcs();
        }
        public static void delXML() 
        {
            try
            {
                string[] xmlList = Directory.GetFiles(DataApp.TxtBoxfilepath, "*.xml");
                // Delete source files that were copied.
                foreach (string f in xmlList)
                {
                    File.Delete(f);
                }
            }
            catch (DirectoryNotFoundException dirNotFound) { Console.WriteLine("Exception Dir - ", dirNotFound.Message); }
            catch (Exception e) { Console.WriteLine("Exception - ", e); }
        }
        public static void delZIP()
        {
            try
            {
                string[] xmlList = Directory.GetFiles(DataApp.TxtBoxfilepath, "*.zip");
                // Delete source files that were copied.
                foreach (string f in xmlList)
                {
                    File.Delete(f);
                }
            }
            catch (DirectoryNotFoundException dirNotFound) { Console.WriteLine("Exception Dir - ", dirNotFound.Message); }
            catch (Exception e) { Console.WriteLine("Exception - ", e); }
        }
        static void delSig()
        {
            try
            {
                string[] sigList = Directory.GetFiles(DataApp.TxtBoxfilepath, "*.sig");
                // Delete source files that were copied.
                foreach (string f in sigList)
                {
                    File.Delete(f);
                }
            }
            catch (DirectoryNotFoundException dirNotFound) { Console.WriteLine("Exception Dir - ", dirNotFound.Message); }
            catch (Exception e) { Console.WriteLine("Exception - ", e); }
        }
        static void delFcs()
        {
            try
            {
                string[] FcsList = Directory.GetFiles(DataApp.TxtBoxfilepath, "f*");
                // Delete source files that were copied.
                foreach (string f in FcsList)
                {
                    File.Delete(f);
                }
            }
            catch (DirectoryNotFoundException dirNotFound) { Console.WriteLine("Exception Dir - ", dirNotFound.Message); }
            catch (Exception e) { Console.WriteLine("Exception - ", e); }
        }
        #endregion 
    }
}
