using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Windows.Forms;

namespace Poranny_asystent
{
    class Miejska:Form1
    {
        Form1 main;
        private List<Autobus> autobusy = new List<Autobus>();

        public Miejska(Form1 m)
        {
            main = m;
        }
        public List<Autobus> Autobusy
        {
            get { return autobusy; }
            set { autobusy = value; }
        }
        public void Pobierz(string data, int godzina, int minuty)
        {
            string link = Properties.Settings.Default.kzkLink;
            try { 
                link = link.Substring(0, link.IndexOf("&godzina"));
                link = link.Substring(link.IndexOf("start"));
            }
            catch (Exception ex) { MessageBox.Show("Błędny Link"); }
            string str;
            string Parameters = link+"&godzina="+FixDate(godzina)+"&minuty="+FixDate(minuty)+"&wyj_przyj=przyj&data="+data+"&czas_na_przesiadke=0";
            WebRequest req = WebRequest.Create("http://rozklady.kzkgop.pl/ajax/wyszukaj.php");
            req.Proxy = new System.Net.WebProxy("http://rozklady.kzkgop.pl/ajax/wyszukaj.php", true); //no proxy
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;
            Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            WebResponse resp = req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            str=sr.ReadToEnd().Trim();

            if (File.Exists(@"D:\autobusy.txt")) { File.Delete(@"D:\autobusy.txt"); }
            StreamWriter sw = new StreamWriter(@"D:\autobusy.txt", true);
            sw.Write(str);
            sw.Close();
            
            HtmlWeb htmlWeb = new HtmlWeb() { AutoDetectEncoding = false, OverrideEncoding = Encoding.GetEncoding("utf-8") };
            HtmlAgilityPack.HtmlDocument htmlDocument = htmlWeb.Load(@"D:\autobusy.txt"); 
            IEnumerable<HtmlNode> sugestionsList = htmlDocument.DocumentNode.Descendants("tbody");
            for (int i = 0; i < sugestionsList.Count(); i++)
            {
                HtmlNode node = sugestionsList.ElementAt(i); //ładujemy pierwsza sugestię
                IEnumerable<HtmlNode> elementsList = node.Descendants("a").Where(x=>x.Attributes.Contains("onclick")); //elementy danej sugestii - tylko w linkach (a)
                IEnumerable<HtmlNode> instructionsList = node.Descendants().Where(x=>x.Name=="td" && x.Attributes.Contains("class") && x.Attributes["class"].Value.Split().Contains("instruction_cell"));
                IEnumerable<HtmlNode> timeList=node.Descendants().Where(x=>x.Name=="td" && x.Attributes.Contains("class") && x.Attributes["class"].Value.Split().Contains("time_cell"));
                /*
                 * elementsList:
                 * 0 - nr autobusu
                 * 1 - przystanek startowy
                 * 2 - nr autobusu
                 * 3 - przystanek docelowy
                 * 
                 * instructionsList:
                 * 0 - kierunek
                 * 1 - ilość przystanków
                 * 
                 * timeList:
                 * 0 - czas odjazdu
                 * 1 - czas przyjazdu
                 * 
                 */
                string busNr = elementsList.ElementAt(0).InnerHtml;
                string przystanekStart=elementsList.ElementAt(1).InnerHtml;
                string przystanekKoniec=elementsList.ElementAt(3).InnerHtml;
                string kierunek = instructionsList.ElementAt(0).InnerHtml;
                kierunek=kierunek.Substring(kierunek.IndexOf("</a>")+4);
                kierunek=kierunek.Substring(1,kierunek.Length-2);
                kierunek = kierunek.Substring(11);
                string iloscPrzystankow = instructionsList.ElementAt(1).InnerHtml;
                iloscPrzystankow=iloscPrzystankow.Substring(11);
                iloscPrzystankow = iloscPrzystankow.Substring(0, iloscPrzystankow.IndexOf(" "));
                string odjazd = timeList.ElementAt(0).InnerHtml;
                odjazd=odjazd.Substring(odjazd.IndexOf("-->")+3);
                string przyjazd = timeList.ElementAt(1).InnerHtml;
                przyjazd=przyjazd.Substring(przyjazd.IndexOf("-->")+3);

                Autobus bus = new Autobus(busNr, przystanekStart, przystanekKoniec, kierunek, Convert.ToInt32(iloscPrzystankow), odjazd, przyjazd);
                autobusy.Add(bus);
            }
        }
        public string FixDate(int dane)
        {
            if (dane < 10)
                return "0" + dane.ToString();
            else
                return dane.ToString();
        }
    }
}
