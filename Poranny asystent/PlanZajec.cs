using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using HtmlAgilityPack;
using System.Windows.Forms;

namespace Poranny_asystent
{
    public class PlanZajec
    {
        //private Form1 main;
        private List<PlanDzien> dni = new List<PlanDzien>();

        public List<PlanDzien> Dni
        {
            get { return dni; }
            set {dni=value;}
        }
        public PlanZajec() { }
        /*public PlanZajec(Form1 mainForm)
        {
            main = mainForm;
        }*/
        public void PobierzHAP()
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            //HtmlDocument wholePage = htmlWeb.Load("http://wimii.pcz.pl/download/plan_stacjonarny/o39j.html");
            HtmlAgilityPack.HtmlDocument wholePage = htmlWeb.Load(Properties.Settings.Default.planLink);

            //praca wstrzymana ze względu na brak możliwości jednoznacznej identyfikacji elementów w tabeli.
            //do weryfikacji (czy HAP'em można to ogarnąć)
        }
        public bool Pobierz(string co)
        {
            try
            {
                WebClient webClient = new WebClient();
                
                string str = webClient.DownloadString(Properties.Settings.Default.planLink);
                //string str = webClient.DownloadString("http://www.google.pl");
                str = FixEncoding(str);
                StreamWriter sw = null;
                if (co == "baza")
                {
                    if (File.Exists("plan_baza.txt")) { File.Delete("plan_baza.txt"); }
                    sw = new StreamWriter("plan_baza.txt", true);
                }
                else if (co == "checkUpdates")
                {
                    if (File.Exists("plan_checkUpdates.txt")) { File.Delete("plan_checkUpdates.txt"); }
                    sw = new StreamWriter("plan_checkUpdates.txt", true);
                }
                else return false;
                sw.Write(str);
                sw.Close();
            }
            catch (Exception ex) {
                MessageBox.Show("err: " + ex);
                return false; 
            }
            return true;
        }
        public void Wczytaj(string co)
        {
            StreamReader sr=null;
            if (co == "baza")
                sr = new StreamReader("plan_baza.txt");
            else if (co == "checkUpdates")
                sr = new StreamReader("plan_checkUpdates.txt");
            else
                return;
            DateTime date = DateTime.Today;
            /*switch (dow)
            {
                case "Monday": dayId = 1; break;
                case "Tuesday": dayId = 2; break;
                case "Wednesday": dayId = 3; break;
                case "Thursday": dayId = 4; break;
                case "Friday": dayId = 5; break;
                default: dayId = -1; break;
            }*/
            //main.Label2 = "Day ID: " + dayId;
            int trCounter = 0;
            int tdCounter = 0;
            string line;
            
            
            while(trCounter<13) {
                line=sr.ReadLine();
                if (line == "<tr valign=top>")
                {
                    if (trCounter == 0)
                    {
                        line = sr.ReadLine(); //pusta komórka
                        line = sr.ReadLine();
                        while (line != "</tr>")
                        {
                            tdCounter++;
                            PlanDzien pdTemp = new PlanDzien();
                            pdTemp.DayId = tdCounter;
                            pdTemp.DayName = line.Substring(line.IndexOf(">") + 1, line.IndexOf("<", line.IndexOf(">"))-(line.IndexOf(">") + 1));
                            pdTemp.DayName = pdTemp.DayName.First().ToString().ToUpper() + String.Join("", pdTemp.DayName.Skip(1));
                            dni.Add(pdTemp);
                            line = sr.ReadLine();
                        }
                        tdCounter = 0;
                    }

                    if (trCounter != 0)
                    {
                        if (tdCounter == 0)
                        {
                            line = sr.ReadLine();
                            tdCounter++;
                        }
                        while (tdCounter < dni.Count()+1 && tdCounter>0)
                        {
                            line = sr.ReadLine();
                            if (line != "<td>&nbsp</td>")
                            {
                                Lekcja lessonTemp = new Lekcja();
                                lessonTemp.Cutter(line);
                                lessonTemp.StartTime = trCounter + 7;
                                dni[tdCounter-1].AddLesson(lessonTemp);
                            }
                            tdCounter++;
                        }
                    tdCounter = 0;
                    }
                    trCounter++;
                }
            }
            sr.Close();
            for (int i = 0; i < dni.Count; i++)
            {
                if (dni[i].Lekcje.Count == 0)
                {
                    dni.RemoveAt(i);
                }
            }
        }
        public string FixEncoding(string s)
        {
            s = s.Replace('¦', 'ś');
            s = s.Replace('±', 'ą');
            return s;
        }
    }
}
