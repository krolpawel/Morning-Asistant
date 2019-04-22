using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Windows.Forms;
using System.Threading;

namespace Poranny_asystent
{
    class Kolej
    {
        private List<Pociag> pociagi = new List<Pociag>();

        public List<Pociag> Pociagi
        {
            get { return pociagi; }
            set { pociagi = value; }
        }
        public void PobierzIP()
        {
            //InfoPasażer

            string str;
            string URL = Properties.Settings.Default.ipLink;
            if (URL == "") { return; }
            System.Net.WebRequest req = System.Net.WebRequest.Create(URL);
            req.Proxy = new System.Net.WebProxy(URL, true); // no proxy
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            str = sr.ReadToEnd().Trim();
            //str = WebUtility.HtmlDecode(str);
            str = str.Substring(str.IndexOf("Odjazd planowy"));
            str=str.Substring(str.IndexOf("<tr"));
            if (File.Exists(@"D:\pociagi-ip.txt")) { File.Delete(@"D:\pociagi-ip.txt"); }
            StreamWriter sw = new StreamWriter(@"D:\pociagi-ip.txt", true);
            sw.Write(str);
            sw.Close();
        }
        public void PobierzPKP(string data, int godzina, int minuty, int copy)
        {
            copy++;
            //Rozkłady PKP
            string str="",link1="",link2="";
            //data w formacie dd.mm.yy
            string link = Properties.Settings.Default.pkpLink;
            try
            {
                link1=link.Substring(0,link.IndexOf("&date=")+6);
                link2=link.Substring(link.IndexOf("&REQ0HafasSearchForw="));
            }
            catch (Exception ex) { MessageBox.Show("Błędny link!"); return; }
            //////////////http://rozklad-pkp.pl/pl/tp?queryPageDisplayed=yes&REQ0JourneyStopsS0A=1&REQ0JourneyStopsS0G=5100925&REQ0JourneyStopsS0ID=&REQ0JourneyStops1.0G=&REQ0JourneyStopover1=&REQ0JourneyStops2.0G=&REQ0JourneyStopover2=&REQ0JourneyStopsZ0A=1&REQ0JourneyStopsZ0G=5100007&REQ0JourneyStopsZ0ID=&date=01.12.14&dateStart=01.12.14&dateEnd=01.12.14&REQ0JourneyDate=01.12.14&time=13%3A41&REQ0JourneyTime=13%3A41&REQ0HafasSearchForw=1&existBikeEverywhere=yes&existHafasAttrInc=yes&existHafasAttrInc=yes&REQ0JourneyProduct_prod_section_0_0=1&REQ0JourneyProduct_prod_section_1_0=1&REQ0JourneyProduct_prod_section_2_0=1&REQ0JourneyProduct_prod_section_3_0=1&REQ0JourneyProduct_prod_section_0_1=1&REQ0JourneyProduct_prod_section_1_1=1&REQ0JourneyProduct_prod_section_2_1=1&REQ0JourneyProduct_prod_section_3_1=1&REQ0JourneyProduct_prod_section_0_2=1&REQ0JourneyProduct_prod_section_1_2=1&REQ0JourneyProduct_prod_section_2_2=1&REQ0JourneyProduct_prod_section_3_2=1&REQ0JourneyProduct_prod_section_0_3=1&REQ0JourneyProduct_prod_section_1_3=1&REQ0JourneyProduct_prod_section_2_3=1&REQ0JourneyProduct_prod_section_3_3=1&REQ0JourneyProduct_opt_section_0_list=0%3A000000&existOptimizePrice=1&existHafasAttrExc=yes&REQ0HafasChangeTime=0%3A1&existSkipLongChanges=0&REQ0HafasAttrExc=&REQ0HafasAttrExc=&REQ0HafasAttrExc=&REQ0HafasAttrExc=&REQ0HafasAttrExc=&REQ0HafasAttrExc=&REQ0HafasAttrExc=&REQ0HafasAttrExc=&REQ0HafasAttrExc=&REQ0HafasAttrExc=&REQ0HafasAttrExc=&existHafasAttrInc=yes&existHafasAttrExc=yes&wDayExt0=Pn|Wt|%C5%9Ar|Cz|Pt|So|Nd&start=start&existUnsharpSearch=yes&came_from_form=1#focus
            //string URL = "http://rozklad-pkp.pl/pl/tp?queryPageDisplayed=yes&REQ0JourneyStopsS0A=1&REQ0JourneyStopsS0G=5100925&REQ0JourneyStopsS0ID=&REQ0JourneyStops1.0G=&REQ0JourneyStopover1=&REQ0JourneyStops2.0G=&REQ0JourneyStopover2=&REQ0JourneyStopsZ0A=1&REQ0JourneyStopsZ0G=5100007&REQ0JourneyStopsZ0ID=&date="+data+"&dateStart="+data+"&dateEnd="+data+"&REQ0JourneyDate="+data+"&time="+godzina+"%3A"+minuty+"&REQ0JourneyTime="+godzina+"%3A"+minuty+"&REQ0HafasSearchForw=1&REQ0HafasNoOfChanges=0&existBikeEverywhere=yes&existHafasAttrInc=yes&existHafasAttrInc=yes&REQ0JourneyProduct_prod_section_0_0=1&REQ0JourneyProduct_prod_section_1_0=1&REQ0JourneyProduct_prod_section_2_0=1&REQ0JourneyProduct_prod_section_3_0=1&REQ0JourneyProduct_prod_section_0_1=1&REQ0JourneyProduct_prod_section_1_1=1&REQ0JourneyProduct_prod_section_2_1=1&REQ0JourneyProduct_prod_section_3_1=1&REQ0JourneyProduct_prod_section_0_2=1&REQ0JourneyProduct_prod_section_1_2=1&REQ0JourneyProduct_prod_section_2_2=1&REQ0JourneyProduct_prod_section_3_2=1&REQ0JourneyProduct_prod_section_0_3=1&REQ0JourneyProduct_prod_section_1_3=1&REQ0JourneyProduct_prod_section_2_3=1&REQ0JourneyProduct_prod_section_3_3=1&REQ0JourneyProduct_opt_section_0_list=0%3A000000&existOptimizePrice=1&existHafasAttrExc=yes&REQ0HafasChangeTime=0%3A1&existSkipLongChanges=0&REQ0HafasAttrExc=P1&REQ0HafasAttrExc=P5&REQ0HafasAttrExc=P2&REQ0HafasAttrExc=P7&REQ0HafasAttrExc=P3&REQ0HafasAttrExc=P4&REQ0HafasAttrExc=&REQ0HafasAttrExc=P9&REQ0HafasAttrExc=P8&REQ0HafasAttrExc=P6&REQ0HafasAttrExc=O1&existHafasAttrInc=yes&existHafasAttrExc=yes&wDayExt0=Pn|Wt|%C5%9Ar|Cz|Pt|So|Nd&start=start&existUnsharpSearch=yes&came_from_form=1#focus";
            string URL = link1+data+"&dateStart="+data+"&dateEnd="+data+"&REQ0JourneyDate="+data+"&time="+godzina+"%3A"+minuty+"&REQ0JourneyTime="+godzina+"%3A"+minuty+link2;
            try
            {
                System.Net.WebRequest req = System.Net.WebRequest.Create(URL);
                req.Proxy = new System.Net.WebProxy(URL, false); //true means no proxy
                System.Net.WebResponse resp = req.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                str = sr.ReadToEnd().Trim();
                str = WebUtility.HtmlDecode(str);
                str = str.Substring(str.IndexOf("<tbody>"));
            }
            catch (Exception ex)
            {
                if (copy < 100)
                    //MessageBox.Show("Błąd: "+ex);
                    PobierzPKP(data, godzina, minuty, copy);
                else
                    MessageBox.Show("Błąd: rozkład kolejowy wywoływany " + copy + " razy bez skutku");
                return;
            }
            //MessageBox.Show(copy.ToString());
            str=str.Substring(0,str.IndexOf("</tbody>")+8);
            if (File.Exists(@"D:\pociagi.txt")) { File.Delete(@"D:\pociagi.txt"); }
            StreamWriter sw = new StreamWriter(@"D:\pociagi.txt", true);
            sw.Write(str);
            sw.Close();
        }

        public void Wczytaj()
        {
            HtmlWeb htmlWeb = new HtmlWeb() { AutoDetectEncoding = false, OverrideEncoding = Encoding.GetEncoding("utf-8") };
            HtmlWeb htmlWebIP = new HtmlWeb() { AutoDetectEncoding = false, OverrideEncoding = Encoding.GetEncoding("utf-8") };
            HtmlAgilityPack.HtmlDocument htmlDocument = htmlWeb.Load(@"D:\pociagi.txt"); 
            HtmlAgilityPack.HtmlDocument htmlDocumentIP = htmlWeb.Load(@"D:\pociagi-ip.txt");

            IEnumerable<HtmlNode> sugestionList = htmlDocument.DocumentNode.Descendants("tr");
            IEnumerable<HtmlNode> connectionsList = htmlDocumentIP.DocumentNode.Descendants("tr");
            for (int i = 0; i < sugestionList.Count(); i++)
            {
                HtmlNode node = sugestionList.ElementAt(i);
                int outer = node.OuterHtml.IndexOf("selected");
                bool sel=false;
                if (outer != -1 && outer<100)
                    sel = true; //sugerowane przez wyszukiwarkę PKP połączenie
                IEnumerable<HtmlNode> elementsList = node.Descendants().Where(x => x.Name == "span" && x.Attributes.Contains("class") && x.Attributes["class"].Value.Split().Contains("clear-lowres"));
                IEnumerable<HtmlNode> odjazdList = elementsList.ElementAt(2).Descendants().Where(x => x.Name == "span");
                IEnumerable<HtmlNode> przyjazdList = elementsList.ElementAt(3).Descendants().Where(x => x.Name == "span");
                IEnumerable<HtmlNode> dataList = node.Descendants().Where(x => x.Name == "td" && x.Attributes.Contains("headers") && x.Attributes["headers"].Value.Split().Contains("hafasOVDate"));
                IEnumerable<HtmlNode> durationList = node.Descendants().Where(x => x.Name == "td" && x.Attributes.Contains("headers") && x.Attributes["headers"].Value.Split().Contains("hafasOVDuration"));
                IEnumerable<HtmlNode> numberList = node.Descendants().Where(x => x.Name == "img" && x.Attributes.Contains("alt") && x.Attributes["alt"].Value.Contains("KS"));
                /*
                 * elementsList:
                 * 0 - nazwa stacji start
                 * 1 - nazwa stacji koniec
                 * 2 - głębiej godzina odjazdu
                 * 3 - głębiej godzina przyjazdu
                 * 
                 * odjazdList/przyjazdList:
                 * 2 - godzina
                 * 
                 * dataList:
                 * 0 - data/daty
                 * 
                 * durationList:
                 * 0 - czas podróży
                 * 
                 * numberList:
                 * 0 - numer pociągu
                 */
                string stacjaStart = elementsList.ElementAt(0).InnerHtml;
                string stacjaKoniec = elementsList.ElementAt(1).InnerHtml;
                stacjaKoniec = stacjaKoniec.Substring(stacjaKoniec.IndexOf(">") + 1);
                string godzinaStart = odjazdList.ElementAt(2).InnerHtml;//.Substring(1);
                if (godzinaStart.IndexOf("0") == 0) godzinaStart=godzinaStart.Substring(1);
                string godzinaKoniec = przyjazdList.ElementAt(2).InnerHtml;//.Substring(1);
                if (godzinaKoniec.IndexOf("0") == 0) godzinaKoniec=godzinaKoniec.Substring(1);
                string dataStart = dataList.ElementAt(0).InnerHtml;
                if (dataStart.IndexOf("0") == 0) dataStart = dataStart.Substring(1);
                string dataKoniec = dataStart.Substring(dataStart.IndexOf("<br>") + 4);
                if (dataKoniec.IndexOf("0") == 0) dataKoniec = dataKoniec.Substring(1);
                dataStart = dataStart.Substring(0, dataStart.IndexOf("<br>"));
                if (dataKoniec == "")
                {
                    dataKoniec = dataStart;
                }
                string czasPodrozy = durationList.ElementAt(0).InnerHtml;
                int nrPociagu = Convert.ToInt32(numberList.ElementAt(0).Attributes["alt"].Value.Substring(3));
                int opoznienie = -1;
                //dane z IP
                for (int j = 0; j < connectionsList.Count(); j++)
                {
                    HtmlNode nodeIP = connectionsList.ElementAt(j);
                    IEnumerable<HtmlNode> aList = nodeIP.Descendants().Where(x => x.Name == "a" && x.Attributes.Contains("href") && x.Attributes["href"].Value.Contains("index_pociag.php"));
                    string nrPocIP = aList.ElementAt(0).InnerHtml;
                    nrPocIP = nrPocIP.Substring(0, nrPocIP.IndexOf(" "));
                    if (Convert.ToInt32(nrPocIP) == nrPociagu)
                    {
                        IEnumerable<HtmlNode> tdList = nodeIP.Descendants().Where(x => x.Name == "td" && x.Attributes.Contains("class") && x.Attributes["class"].Value == "in");
                        string op = tdList.ElementAt(5).InnerHtml;
                        opoznienie = Convert.ToInt32(op.Substring(0, op.IndexOf(" ")));
                        break;
                    }
                }
                Pociag train = new Pociag(nrPociagu,dataStart,dataKoniec,stacjaStart,stacjaKoniec,godzinaStart,godzinaKoniec,czasPodrozy,opoznienie, sel);
                pociagi.Add(train);
            }

            /*
            StreamReader sr = new StreamReader(@"E:\pociagi-ip.txt");
            string line;
            line = sr.ReadLine();
            while (line != "<td align=\"center\"><h4>Opóźnienie<br>odjazdu</h4></td>") line=sr.ReadLine();
            line = sr.ReadLine();
            line = sr.ReadLine();
            line = sr.ReadLine();
            while (line != "</table>")
            {
                Pociag p = new Pociag();
                p.Wiersz = line.Substring(0, line.IndexOf("</tr>") + 5);
                line = line.Substring(line.IndexOf("</tr>") + 5);
                pociagi.Add(p);
            }
            for (int i = 0; i < pociagi.Count(); i++)
            {
                line = pociagi[i].Wiersz;
                line=line.Substring(line.IndexOf("\">")+2);
                pociagi[i].NrPociagu = Convert.ToInt32(line.Substring(0, line.IndexOf(" ")));
                line=line.Substring(line.IndexOf(">201")+1);
                pociagi[i].Data=line.Substring(0,10);
                line=line.Substring(line.IndexOf("<b>")+3);
                pociagi[i].Relacja=line.Substring(0,line.IndexOf("</b>"));
                pociagi[i].Relacja=pociagi[i].Relacja.Replace("<br>"," - ");
                line=line.Substring(line.IndexOf("<td")+13);
                pociagi[i].Odjazd=line.Substring(0,5);
                line=line.Substring(line.IndexOf("<td")+13);
                pociagi[i].Opoznienie=Convert.ToInt32(line.Substring(0,line.IndexOf(" ")));
            }
             */
        }
    }
}
