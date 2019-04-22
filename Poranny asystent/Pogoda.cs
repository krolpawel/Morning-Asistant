using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Poranny_asystent
{
    class Pogoda
    {
        private Form1 main;
        private List<Prognoza> prognozy = new List<Prognoza>();
        private string dzien;

        public Pogoda(Form1 mf) {
            main=mf;
        }
        public List<Prognoza> Prognozy
        {
            get { return prognozy; }
            set { prognozy = value; }
        }

        public void PobierzDom()
        {
            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;
            string str = webClient.DownloadString("http://www.twojapogoda.pl/polska/slaskie/dabrowa-gornicza");
            if (File.Exists(@"D:\pogoda-dom.txt")) { File.Delete(@"D:\pogoda-dom.txt"); }
            StreamWriter sw = new StreamWriter(@"D:\pogoda-dom.txt", true);
            sw.Write(str);
            sw.Close();
        }
        public void PobierzPraca() {
            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;
            string str = webClient.DownloadString("http://www.twojapogoda.pl/polska/slaskie/czestochowa");
            if (File.Exists(@"D:\pogoda-praca.txt")) { File.Delete(@"D:\pogoda-praca.txt"); }
            StreamWriter sw = new StreamWriter(@"D:\pogoda-praca.txt", true);
            sw.Write(str);
            sw.Close();
        }
        public void Wczytaj(string miejsce)
        {
            StreamReader sr = new StreamReader(@"D:\pogoda-"+miejsce+".txt");
            string line;
            
            line=sr.ReadLine();
            while (line != "\t\t<h3 class=\"underline\">Dziś</h3>") line = sr.ReadLine();   // da się regulować frazami Dziś / Jutro
            Prognoza prognoza = new Prognoza();
            while (line != "\t\t\t</ul>") {
                line=sr.ReadLine();
                switch(line) {
                    case "\t\t\t<div class=\"info\">":
                        line=sr.ReadLine();
                        line=line.Substring(12);
                        line=line.Substring(0,line.IndexOf("<"));
                        prognoza.Temperatura=Convert.ToInt32(line);
                        line=sr.ReadLine();
                        line=line.Substring(4);
                        line=line.Substring(0,line.IndexOf("\t"));
                        prognoza.Niebo=line;
                        break;
                    case "\t\t\t\t\t<div class=\"label\">T. odczuw.</div>":
                        line = sr.ReadLine();
                        line = line.Substring(24);
                        line=line.Substring(0,line.IndexOf(" "));
                        prognoza.Odczuwalna=Convert.ToInt32(line);
                        break;
                    case "\t\t\t\t\t<div class=\"label\">Wiatr</div>":
                        line = sr.ReadLine();
                        line = sr.ReadLine();
                        line = line.Substring(6);
                        line=line.Substring(0,line.IndexOf(" "));
                        prognoza.Wiatr=Convert.ToInt32(line);
                        break;
                    case "\t\t\t\t\t<div class=\"label\">Porywy</div>":
                        line = sr.ReadLine();
                        line = line.Substring(24);
                        line=line.Substring(0,line.IndexOf(" "));
                        prognoza.Porywy=Convert.ToInt32(line);
                        break;
                    case "\t\t\t\t\t<div class=\"label\">Chmury</div>":
                        line=sr.ReadLine();
                        line = line.Substring(24);
                        line=line.Substring(0,line.IndexOf("%"));
                        prognoza.Chmury=line;
                        break;
                    case "\t\t\t\t\t<div class=\"label\">Opady <img src=\"/images/forecast/ico-water.png\" alt=\"\" title=\"Spodziewana suma opadów\"/></div>":
                        line=sr.ReadLine();
                        line = line.Substring(24);
                        line=line.Substring(0,line.IndexOf(" "));
                        line = line.Replace('.', ',');
                        prognoza.Opady=Convert.ToDouble(line);
                        break;
                    case "\t\t\t\t\t<div class=\"label\">Ciśnienie</div>":
                        line=sr.ReadLine();
                        line = line.Substring(24);
                        line=line.Substring(0,line.IndexOf(" "));
                        prognoza.Cisnienie=Convert.ToInt32(line);
                        break;
                    case "\t\t\t\t\t<div class=\"label\">Wilgotność</div>":
                        line=sr.ReadLine();
                        line = line.Substring(24);
                        line=line.Substring(0,line.IndexOf(" "));
                        prognoza.Wilgotnosc=Convert.ToInt32(line);
                        break;
                    case "\t\t\t\t\t<div class=\"label\">Termika</div>":
                        line=sr.ReadLine();
                        line = line.Substring(24);
                        line=line.Substring(0,line.IndexOf("<"));
                        prognoza.Termika=line;
                        break;
                    case "\t\t\t\t\t<div class=\"label\">Biomet</div>":
                        line=sr.ReadLine();
                        line = line.Substring(24);
                        line=line.Substring(0,line.IndexOf("<"));
                        prognoza.Biomet=line;
                        break;
                }
            }
            prognoza.Miasto = miejsce;
            prognozy.Add(prognoza);
            sr.Close();
        }
    }
}
