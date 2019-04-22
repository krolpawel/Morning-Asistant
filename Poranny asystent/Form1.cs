using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System.Threading.Tasks;
using System.Threading;
using System.Resources;
using System.Speech;
using System.Speech.Synthesis;
using SpeechLib;
using System.Media;
using WMPLib;
using System.Xml.Serialization;

namespace Poranny_asystent
{
    public partial class Form1 : Form
    {
        private PlanZajec pz=null,pzCheck=null;
        private Kolej kol;
        private Miejska miej;
        private int lookForDayNr;
        private DateTime wakeUpDT;
        private bool playingAlarm=false;
        private SoundPlayer dzwonek = new System.Media.SoundPlayer("beep-06.wav");
        private DateTime stopPlaying;
        private WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        private DateTime nextIcrease;
        private OpenFileDialog fd;

        public Form1()
        {
            InitializeComponent();
            tbLinkPlan.Text = Properties.Settings.Default.planLink;
            tbLinkKZK.Text = Properties.Settings.Default.kzkLink;
            tbLinkIP.Text = Properties.Settings.Default.ipLink;
            tbLinkPKP.Text = Properties.Settings.Default.pkpLink;
            tbInter1.Text = Convert.ToString(Properties.Settings.Default.interWakeDom);
            tbInter2.Text = Convert.ToString(Properties.Settings.Default.interDomBus);
            tbInter3.Text = Convert.ToString(Properties.Settings.Default.interBusPoc);
            tbInter4.Text = Convert.ToString(Properties.Settings.Default.interPocUcz);
        }
        private void label1_Click(object sender, EventArgs e){}
        public string Label1
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }
        public string Label2
        {
            get { return label2.Text; }
            set { label2.Text = value; }
        }
        public string Label3
        {
            get { return label3.Text; }
            set { label3.Text = value; }
        }
        public string Label4
        {
            get { return label4.Text; }
            set { label4.Text = value; }
        }
        public string Label5
        {
            get { return label5.Text; }
            set { label5.Text = value; }
        }
        public string Label6
        {
            get { return label6.Text; }
            set { label6.Text = value; }
        }
        public string Label7
        {
            get { return label7.Text; }
            set { label7.Text = value; }
        }
        public string Label8
        {
            get { return label8.Text; }
            set { label8.Text = value; }
        }
        
        //przyciski
        private void button1_Click(object sender, EventArgs e)
        {
            LoadPlan();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            LoadKolej("7.01.2015",15,00);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            
        }
        private void button4_Click(object sender, EventArgs e)
        {
            LoadCalendar();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            LoadAll();
            //p.Close();
            /*LoadPlan();
            LoadPogoda();
            LoadKolej();
            LoadMiejska();
            LoadCalendar();*/
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (tbLinkPlan.Enabled == false)
            {
                tbLinkPlan.Enabled = true;
                button6.Text = "OK";
            }
            else if (tbLinkPlan.Enabled == true)
            {
                if (LinkValidator(tbLinkPlan.Text, "pcz.pl/") == true)
                {
                    tbLinkPlan.Enabled = false;
                    button6.Text = "Zmień...";
                    Properties.Settings.Default.planLink = tbLinkPlan.Text;
                    Properties.Settings.Default.Save();
                }
                else { MessageBox.Show("Błędny link!"); }
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (tbLinkKZK.Enabled == false)
            {
                tbLinkKZK.Enabled = true;
                button7.Text = "OK";
            }
            else if (tbLinkKZK.Enabled == true)
            {
                if( LinkValidator(tbLinkKZK.Text,"rozklady.kzkgop.pl/") == true && LinkValidator(tbLinkKZK.Text,"start=") == true && 
                LinkValidator(tbLinkKZK.Text,"koniec=") == true) {
                    tbLinkKZK.Enabled = false;
                    button7.Text = "Zmień...";
                    Properties.Settings.Default.kzkLink = tbLinkKZK.Text;
                    Properties.Settings.Default.Save();
                }
                else { MessageBox.Show("Błędny Link!"); }

            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (tbLinkIP.Enabled == false)
            {
                tbLinkIP.Enabled = true;
                button8.Text = "OK";
            }
            else if (tbLinkIP.Enabled == true)
            {
                if (LinkValidator(tbLinkIP.Text, "http://infopasazer.intercity.pl/index3.php?nr_sta=") == true)
                {
                    tbLinkIP.Enabled = false;
                    button8.Text = "Zmień...";
                    Properties.Settings.Default.ipLink = tbLinkIP.Text;
                    Properties.Settings.Default.Save();
                }
                else { MessageBox.Show("Błędny link!"); }
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (tbLinkPKP.Enabled == false)
            {
                tbLinkPKP.Enabled = true;
                button9.Text = "OK";
            }
            else if (tbLinkPKP.Enabled == true)
            {
                if (LinkValidator(tbLinkPKP.Text, "http://rozklad-pkp.pl") == true)
                {
                    tbLinkPKP.Enabled = false;
                    button9.Text = "Zmień...";
                    Properties.Settings.Default.pkpLink = tbLinkPKP.Text;
                    Properties.Settings.Default.Save();
                }
                else { MessageBox.Show("Błędny link!"); }
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (tbInter1.Enabled == false)
            {
                tbInter1.Enabled = true;
                button10.Text = "OK";
            }
            else if (tbInter1.Enabled == true)
            {
                try
                {
                    Properties.Settings.Default.interWakeDom = Convert.ToInt32(tbInter1.Text);
                    Properties.Settings.Default.Save();
                    tbInter1.Enabled = false;
                    button10.Text = "Zmień...";
                }
                catch (Exception ex) { MessageBox.Show("Na pewno wpisałeś liczbę minut?"); }
            }
        }
        private void button11_Click(object sender, EventArgs e)
        {
            if (tbInter2.Enabled == false)
            {
                tbInter2.Enabled = true;
                button11.Text = "OK";
            }
            else if (tbInter2.Enabled == true)
            {
                try
                {
                    Properties.Settings.Default.interDomBus = Convert.ToInt32(tbInter2.Text);
                    Properties.Settings.Default.Save();
                    tbInter2.Enabled = false;
                    button11.Text = "Zmień...";
                }
                catch (Exception ex) { MessageBox.Show("Na pewno wpisałeś liczbę minut?"); }
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            if (tbInter3.Enabled == false)
            {
                tbInter3.Enabled = true;
                button12.Text = "OK";
            }
            else if (tbInter3.Enabled == true)
            {
                try
                {
                    Properties.Settings.Default.interBusPoc = Convert.ToInt32(tbInter3.Text);
                    Properties.Settings.Default.Save();
                    tbInter3.Enabled = false;
                    button12.Text = "Zmień...";
                }
                catch (Exception ex) { MessageBox.Show("Na pewno wpisałeś liczbę minut?"); }
            }
        }
        private void button13_Click(object sender, EventArgs e)
        {
            if (tbInter4.Enabled == false)
            {
                tbInter4.Enabled = true;
                button13.Text = "OK";
            }
            else if (tbInter4.Enabled == true)
            {
                try
                {
                    Properties.Settings.Default.interPocUcz = Convert.ToInt32(tbInter4.Text);
                    Properties.Settings.Default.Save();
                    tbInter4.Enabled = false;
                    button13.Text = "Zmień...";
                }
                catch (Exception ex) { MessageBox.Show("Na pewno wpisałeś liczbę minut?"); }
            }
        }

        //zdarzenia
        private void dataGridViewPlan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex;
            int row = e.RowIndex;
            string startTimeFromCell = dataGridViewPlan.Rows[row].Cells[0].Value.ToString();
            int startTime = Convert.ToInt32(startTimeFromCell.Substring(0, startTimeFromCell.Length - 3));
            string day = "";
            Lekcja lekcja = null;
            try
            {
                for (int i = 0; i < pz.Dni[col - 1].Lekcje.Count; i++)
                {
                    if (pz.Dni[col - 1].Lekcje[i].StartTime == startTime)
                    {
                        lekcja = pz.Dni[col - 1].Lekcje[i];
                        day = pz.Dni[col - 1].DayName;
                    }
                }
                if (lekcja != null)
                {
                    Form2 f2 = new Form2(lekcja, day);
                    f2.Show();
                }
                else
                {
                    MessageBox.Show("Tu masz wolne!");
                }
            }
            catch (Exception ex) { MessageBox.Show("Kliknięcie tu nic Ci nie da :)"); }
        }
        private void tbLinkPlan_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button6_Click(sender, e);
        }
        private void tbLinkKZK_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button7_Click(sender, e);
        }
        private void tbLinkIP_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button8_Click(sender, e);
        }
        private void tbLinkPKP_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button9_Click(sender, e);
        }
        private void tbInter1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button10_Click(sender, e);
        }
        private void tbInter2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button11_Click(sender, e);
        }
        private void tbInter3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button12_Click(sender, e);
        }
        private void tbInter4_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button13_Click(sender, e);
        }

        //metody inne
        public string FixedMinutes(int c)
        {
            string wyj = Convert.ToString(c);
            if (c < 10)
            {
                return "0" + wyj;
            }
            return wyj;
        }
        public string UsunPolskie(string c)
        {
            c = c.Replace('ą', 'a');
            c = c.Replace('ć', 'c');
            c = c.Replace('ę', 'e');
            c = c.Replace('ł', 'l');
            c = c.Replace('ó', 'o');
            c = c.Replace('ś', 's');
            c = c.Replace('ż', 'z');
            c = c.Replace('ź', 'z');
            return c;
        }
        public bool LinkValidator(string link, string lookFor)
        {
            if (link.IndexOf("http://") != 0) return false;
            if (link.IndexOf(lookFor) == -1) return false;
            return true;

        }
        public void Err(string s)
        {
            MessageBox.Show("BŁĄD:\n" + s);
        }
        public void CurrentCellPlan()
        {
            int row=0, col=0;
            string dow = DateTime.Now.DayOfWeek.ToString();
            int dowNr = DowNr(dow); //numer dnia w tygodniu
            int h = DateTime.Now.Hour;
            int m = DateTime.Now.Minute;
            string dowO = DowPL(dowNr); //polska nazwa dnia tygodnia
            //ustalanie dnia + zabezpieczenie na dni w których nie ma zajęć
            bool diffDay=false;
            bool ctl = false;   //kontrola pętli
            while (ctl == false)
            {
                for (int i = 0; i < pz.Dni.Count(); i++)
                {
                    if (dowO == pz.Dni[i].DayName)
                    {
                        col = i + 1;
                        ctl = true;
                        break;
                    }
                }
                if (ctl == false)
                {
                    diffDay=true;
                    if (dowNr <= 7) dowNr++;
                    else dowNr = 1;
                    dowO = DowPL(dowNr);
                }
            }
            lookForDayNr = dowNr;
            //ustalanie godziny
            row = pz.Dni[col - 1].Lekcje[0].StartTime; //ustawienie na czas startowy lekcji pierwszej danego dnia
            //Gdy jesteśmy przed zajęciami
            if (h <= pz.Dni[col - 1].Lekcje[0].StartTime || diffDay==true)
            {
                for (int i = 0; i < dataGridViewPlan.Rows.Count; i++)
                {
                    if (dataGridViewPlan[col, i].Value.ToString() != "")
                    {
                        dataGridViewPlan.CurrentCell = dataGridViewPlan[col, i];
                        break;
                    }
                }
            }
            //gdy po zajęciach
            else if (h > pz.Dni[col - 1].Lekcje[pz.Dni[col - 1].Lekcje.Count-1].StartTime)
            {
                ctl = false;
                while (ctl == false)
                {
                    if (dowNr < 7) dowNr++;
                    else dowNr = 1;
                    dowO = DowPL(dowNr);
                    for (int i = 0; i < pz.Dni.Count(); i++)
                    {
                        if (dowO == pz.Dni[i].DayName)
                        {
                            col = i + 1;
                            ctl = true;
                            break;
                        }
                    }
                }
                lookForDayNr = dowNr;
                for (int i = 0; i < dataGridViewPlan.Rows.Count; i++)
                {
                    if (dataGridViewPlan[col, i].Value!=null && dataGridViewPlan[col,i].Value.ToString()!="")
                    {
                        dataGridViewPlan.CurrentCell = dataGridViewPlan[col, i];
                        break;
                    }
                }
            }
            //pomiędzy pierwszą a ostatnią lekcją
            else if (h > pz.Dni[col - 1].Lekcje[0].StartTime && h <= pz.Dni[col - 1].Lekcje[pz.Dni[col - 1].Lekcje.Count-1].StartTime)
            {
                string hRow="";
                for (int i = 0; i < dataGridViewPlan.Rows.Count; i++)
                {
                    hRow = dataGridViewPlan[0, i].Value.ToString(); //godzina dla wiersza
                    hRow = hRow.Substring(0, hRow.IndexOf(':'));
                    if (Convert.ToInt32(hRow) == h)
                    {
                        dataGridViewPlan.CurrentCell = dataGridViewPlan[col, i];
                        break;
                    }
                }
            }
        }
        public string DowPL(int dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 1: return "Poniedziałek";
                case 2: return "Wtorek";
                case 3: return "Środa";
                case 4: return "Czwartek";
                case 5: return "Piątek";
                case 6: return "Sobota";
                case 7: return "Niedziela";
                default: return "N/A";
            }
        }
        public int DowNr(string dow)
        {
            switch (dow)
            {
                case "Monday": return 1;
                case "Tuesday": return 2;
                case "Wednesday": return 3;
                case "Thursday": return 4;
                case "Friday": return 5;
                case "Saturday": return 6;
                case "Sunday": return 7;
                default: return 0;
            }
        }
        public string FindNextDay(string co)
        {
            int now = DowNr(DateTime.Now.DayOfWeek.ToString());
            int lf=lookForDayNr;
            if (lf < now)
                lf += 7;
            DateTime dOut = DateTime.Now.AddDays(lf - now);
            switch (co)
            {
                case "k": return dOut.Day + "." + dOut.Month + "." + dOut.Year;
                case "m": return dOut.Year + "-" + FixDate(dOut.Month) + "-" + FixDate(dOut.Day);
                case "year": return dOut.Year.ToString();
                case "month": return dOut.Month.ToString();
                case "day": return dOut.Day.ToString();
                default: return "ERROR in function FindNextDay";
            }
        }
        public string FixDate(int dane)
        {
            if (dane < 10)
                return "0" + dane.ToString();
            else
                return dane.ToString();
        }
        public void setCurrentAlarm(int h, int m)
        {
            wakeUpDT = new DateTime(Convert.ToInt32(FindNextDay("year")), Convert.ToInt32(FindNextDay("month")), Convert.ToInt32(FindNextDay("day")), h, m, 0);
        }
        //metody ładujące
        public void PlanToDataGrid()
        {
            dataGridViewPlan.Columns.Clear();
            dataGridViewPlan.Columns.Add("startTime", "Start");
            for (int i = 0; i < pz.Dni.Count; i++)
            {
                string name = pz.Dni[i].DayName;
                name = UsunPolskie(name);
                dataGridViewPlan.Columns.Add(name, name.ToUpper());
            }
            for (int i = 4; i < 23; i++)
            {
                string[] tab = new string[pz.Dni.Count + 1];
                for (int l = 0; l < pz.Dni.Count; l++) tab[l] = "";
                bool iscontent = false;
                for (int j = 0; j < pz.Dni.Count; j++)
                {
                    for (int k = 0; k < pz.Dni[j].Lekcje.Count; k++)
                    {
                        if (pz.Dni[j].Lekcje[k].StartTime == i)
                        {
                            tab[0] = Convert.ToString(pz.Dni[j].Lekcje[k].StartTime) + ":00";
                            tab[j + 1] = pz.Dni[j].Lekcje[k].Name + " (" + pz.Dni[j].Lekcje[k].Type.Substring(0, 3) + ")";
                            iscontent = true;
                        }
                    }
                }
                if (iscontent == true)
                {
                    dataGridViewPlan.Rows.Add(tab);
                }
            }
        }
        public void LoadPlan()
        {
            pz = null;
            pz = new PlanZajec();
            //gdy jeszcze nie ma planu użytkownika
            if (!File.Exists("plan_twoj.xml"))
            {
                DialogResult dr = MessageBox.Show("Aktualnie nie posiadasz zdefiniowanego planu zajęć.\n Czy chcesz pobrać i skonfigurować swój plan?", "InfoBox", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                {
                    MessageBox.Show("Operacja anulowana!");
                    return;
                }
                else if (dr == DialogResult.Yes)
                {
                    
                    if (!pz.Pobierz("baza"))
                    {
                        Err("Nie udało się pobrać planu! 1");
                        return;
                    }
                    pz.Wczytaj("baza");
                    SerializujPlan();
                    PlanToDataGrid();
                    int tabIndex;
                    for (tabIndex = 0; tabIndex < tabControl1.TabPages.Count; tabIndex++) if (tabControl1.TabPages[tabIndex].Name == "tabPlan") break;
                    tabControl1.SelectedTab = tabControl1.TabPages[tabIndex];
                    CurrentCellPlan();
                }
            }
            else
            {
                pz = null;
                DeserializujPlan();
                PlanToDataGrid();
                CurrentCellPlan();
                //sprawdzanie czy była aktyalizacja planu na podstawie porównania plików
                if (!pz.Pobierz("checkUpdates"))
                {
                    Err("Nie udało się pobrać planu! 2");
                    return;
                }
                if (File.Exists("plan_baza.txt") && File.Exists("plan_checkUpdates.txt"))
                {
                    StreamReader sr = new StreamReader("plan_baza.txt"); 
                    StreamReader srU = new StreamReader("plan_checkUpdates.txt");
                    string ciag1 = sr.ReadToEnd();
                    string ciag2 = srU.ReadToEnd();
                    if (ciag1 != ciag2) { 
                        DialogResult dr = MessageBox.Show("Plan prawdopodobnie zmienił się!\n Czy chcesz zweryfikować różnice?","InfoBox",MessageBoxButtons.YesNo);
                        if (dr == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(Properties.Settings.Default.planLink);
                            int tabIndex;
                            for (tabIndex = 0; tabIndex < tabControl1.TabPages.Count; tabIndex++) if (tabControl1.TabPages[tabIndex].Name == "tabPlan") break;
                            tabControl1.SelectedTab = tabControl1.TabPages[tabIndex];
                        }
                    }
                    sr.Close();
                    srU.Close();
                    File.Delete("plan_checkUpdates.txt");
                }
            }
        }
        public void LoadPogoda()
        {
            Pogoda pog = new Pogoda(this);
            pog.PobierzDom();
            pog.PobierzPraca();

            pog.Wczytaj("dom");
            pog.Wczytaj("praca");

            dataGridViewPogoda.Rows.Clear();
            //dataGridViewPogoda.Rows.Add("Miasto", pog.Prognozy[0].Miasto, pog.Prognozy[1].Miasto);
            dataGridViewPogoda.Rows.Add("Temperatura", pog.Prognozy[0].Temperatura, pog.Prognozy[1].Temperatura);
            dataGridViewPogoda.Rows.Add("Odczuwalna", pog.Prognozy[0].Odczuwalna, pog.Prognozy[1].Odczuwalna);
            dataGridViewPogoda.Rows.Add("Niebo", pog.Prognozy[0].Niebo, pog.Prognozy[1].Niebo);
            dataGridViewPogoda.Rows.Add("Wiatr", pog.Prognozy[0].Wiatr, pog.Prognozy[1].Wiatr);
            dataGridViewPogoda.Rows.Add("Porywy", pog.Prognozy[0].Porywy, pog.Prognozy[1].Porywy);
            dataGridViewPogoda.Rows.Add("Chmury", pog.Prognozy[0].Chmury, pog.Prognozy[1].Chmury);
            dataGridViewPogoda.Rows.Add("Opady", pog.Prognozy[0].Opady, pog.Prognozy[1].Opady);
            dataGridViewPogoda.Rows.Add("Ciśnienie", pog.Prognozy[0].Cisnienie, pog.Prognozy[1].Cisnienie);
            dataGridViewPogoda.Rows.Add("Wilgotność", pog.Prognozy[0].Wilgotnosc, pog.Prognozy[1].Wilgotnosc);
            dataGridViewPogoda.Rows.Add("Termika", pog.Prognozy[0].Termika, pog.Prognozy[1].Termika);
            dataGridViewPogoda.Rows.Add("Biomet", pog.Prognozy[0].Biomet, pog.Prognozy[1].Biomet);
            label2.Text="Completed";
        }
        public void LoadKolej(string data, int h, int m)
        {
            kol = new Kolej();
            kol.PobierzPKP(data, h, m, 0);
            kol.PobierzIP();
            kol.Wczytaj();
            dataGridViewKolej.Rows.Clear();
            int sel=-1;
            for (int i = 0; i < kol.Pociagi.Count(); i++)
            {
                Pociag poc = kol.Pociagi[i];
                string opoz = Convert.ToString(poc.Opoznienie);
                if (poc.Opoznienie == -1) opoz = "N/A";
                if (poc.Selected == true) sel = i;
                dataGridViewKolej.Rows.Add(poc.DataStart + "\n" + poc.Odjazd, poc.DataKoniec + "\n" + poc.Przyjazd, poc.NrPociagu, poc.StacjaStart, poc.StacjaKoniec, poc.CzasPodrozy, opoz);
            }
            Label3 = "Completed";
            dataGridViewKolej.CurrentCell = dataGridViewKolej.Rows[sel].Cells[0];
        }
        public void LoadMiejska(string data, int h, int m)
        {
            Label4 = "Working...";
            miej = new Miejska(this);
            miej.Pobierz(data, h, m);
            dataGridViewKZKGOP.Rows.Clear();
            int thH = 0, thM = 0, thI = 0;
            for (int i = 0; i < miej.Autobusy.Count(); i++)
            {
                Autobus bus = miej.Autobusy[i];
                dataGridViewKZKGOP.Rows.Add(bus.Odjazd, bus.Przyjazd, bus.BusNr, bus.Kierunek, bus.PrzystanekStart, bus.PrzystanekKoniec, bus.IloscPrzystankow);
                int tH = Convert.ToInt32(bus.Przyjazd.Substring(0, bus.Przyjazd.IndexOf(":")));
                int tM = Convert.ToInt32(bus.Przyjazd.Substring(bus.Przyjazd.IndexOf(":") + 1));
                if (tH > thH || (tH==thH && tM>thM) )
                {
                    thH = tH;
                    thM = tM;
                    thI = i;
                }
            }
            dataGridViewKZKGOP.CurrentCell = dataGridViewKZKGOP.Rows[thI].Cells[0];
            Label4 = "Completed";
        }
        public void LoadCalendar()
        {
            dataGridViewKalendarz.Rows.Clear();
            //GOOGLE CALENDAR

            UserCredential credential = null; //zerowanie zmiennej uprawnień
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "",
                    ClientSecret = "",
                },
                new[] { CalendarService.Scope.Calendar },
                "user",
                CancellationToken.None).Result;
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Poranny Asystent",
            });
            IList<string> scope = new List<string>();
            //przestrzeń
            scope.Add(CalendarService.Scope.Calendar);

            //pobranie listy kalendarzy
            IList<CalendarListEntry> list = service.CalendarList.List().Execute().Items;
            IEnumerable<CalendarListEntry> primList = list.Where(x => x.Primary == true);
            CalendarListEntry primary = primList.ElementAt(0);

            EventsResource.ListRequest req = service.Events.List(primary.Id);   //tworzenie zapytania do głównego kalendarza
            req.SingleEvents = true;    //określa że zwróci pojedyncze eventy
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(now.Year, now.Month, now.Day, 0, 1, 1);
            DateTime end = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            req.TimeMin = start;
            req.TimeMax = end;
            Events results = req.Execute();
            IEnumerable<Event> eventsToday = results.Items;
            string sDate;
            string eDate;
            bool allDay = false;
            foreach (Event ev in eventsToday)
            {
                if (ev.Start.Date != null)
                {
                    sDate = ev.Start.Date;
                    eDate = ev.End.Date;
                    allDay = true;
                }
                else
                {
                    sDate = Convert.ToString(ev.Start.DateTime.Value.Day) + "/" + Convert.ToString(ev.Start.DateTime.Value.Month) + "/" + Convert.ToString(ev.Start.DateTime.Value.Year + " " + Convert.ToString(ev.Start.DateTime.Value.Hour) + ":" + FixedMinutes(ev.Start.DateTime.Value.Minute));
                    eDate = Convert.ToString(ev.End.DateTime.Value.Day) + "/" + Convert.ToString(ev.End.DateTime.Value.Month) + "/" + Convert.ToString(ev.End.DateTime.Value.Year + " " + Convert.ToString(ev.End.DateTime.Value.Hour) + ":" + FixedMinutes(ev.End.DateTime.Value.Minute));
                }
                dataGridViewKalendarz.Rows.Add(sDate, eDate, ev.Summary, ev.Description, ev.Location, allDay);
            }
        }
        public void LoadAll()
        {
            LoadPlan();
            string dow=DowPL(lookForDayNr);
            //ustalanie godziny startu pierwszej lekcji
            int startTime=0;
            for(int i=0;i<pz.Dni.Count;i++) {
                if(pz.Dni[i].DayName==dow)
                    startTime=pz.Dni[i].Lekcje[0].StartTime;
            }
            //
            DateTime dt1 = new DateTime(2000,1,1,startTime,0,0);
            int interval1 = Properties.Settings.Default.interPocUcz;
            dt1=dt1.AddMinutes(-1.0 * interval1);
            LoadKolej(FindNextDay("k"), dt1.Hour, dt1.Minute);
            string t1 = "0:00";
            for (int i = 0; i < kol.Pociagi.Count; i++)
            {
                if (kol.Pociagi[i].Selected == true)
                {
                    t1 = kol.Pociagi[i].Odjazd;
                    break;
                }
            }
            int hTemp = Convert.ToInt32(t1.Substring(0, t1.IndexOf(":")));
            int mTemp = Convert.ToInt32(t1.Substring(t1.IndexOf(":") + 1));
            int interval2 = Properties.Settings.Default.interBusPoc;
            dt1 = new DateTime(2000, 1, 1, hTemp, mTemp, 0);
            dt1 = dt1.AddMinutes(-1.0 * interval2);

            LoadMiejska(FindNextDay("m"),dt1.Hour,dt1.Minute);
            t1 = miej.Autobusy[miej.Autobusy.Count - 1].Odjazd;
            hTemp = Convert.ToInt32(t1.Substring(0, t1.IndexOf(":")));
            mTemp = Convert.ToInt32(t1.Substring(t1.IndexOf(":") + 1));
            int interval3 = Properties.Settings.Default.interDomBus;
            dt1 = new DateTime(2000, 1, 1, hTemp, mTemp, 0);
            dt1 = dt1.AddMinutes(-1.0 * interval3);
            int interval4 = Properties.Settings.Default.interWakeDom;
            dt1 = dt1.AddMinutes(-1.0 * interval4);

            LoadCalendar();
            LoadPogoda();
            setCurrentAlarm(dt1.Hour, dt1.Minute);
            timer1.Start();
            Label8 = "Pobudka w " + DowPL(lookForDayNr) + " o " + dt1.ToString("H:mm");
        }
        public void SerializujPlan()
        {
            FileStream fs = new FileStream("./plan_twoj.xml", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            XmlSerializer serializer = new XmlSerializer(typeof(PlanZajec));
            serializer.Serialize(fs, pz);
            fs.Close();
        }
        public void DeserializujPlan()
        {
            FileStream fs = new FileStream("./plan_twoj.xml", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            XmlSerializer serializer = new XmlSerializer(typeof(PlanZajec));
            pz = (PlanZajec)serializer.Deserialize(fs);
            fs.Close();
        }
        //metody nadzorujące
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            Label6 = now.ToString();
            if (wakeUpDT.ToString() == now.ToString() && !playingAlarm)
            {
                playingAlarm = true;
                stopPlaying = now.AddSeconds(50.0);
                wplayer.URL = "a1.mp3";
                wplayer.settings.setMode("loop", true);
                wplayer.settings.volume = Convert.ToInt32(Properties.Settings.Default.startVolume);
                wplayer.controls.play();
                nextIcrease = now.AddSeconds(Convert.ToInt32(Properties.Settings.Default.increasingTime));
                stopPlaying = now.AddSeconds(Convert.ToInt32(Properties.Settings.Default.alarmDuration));
                Label8 = stopPlaying.ToString();
                Label5 = wplayer.settings.volume.ToString();
            }
            else if (now.ToString() == nextIcrease.ToString() && playingAlarm)
            {
                if (Convert.ToInt32(wplayer.settings.volume.ToString()) + Convert.ToInt32(Properties.Settings.Default.increasingProcent) < 100)
                {
                    wplayer.settings.volume += Convert.ToInt32(Properties.Settings.Default.increasingProcent);
                    nextIcrease = now.AddSeconds(Convert.ToInt32(Properties.Settings.Default.increasingTime));
                }
                else
                    wplayer.settings.volume = 100;
                Label5 = wplayer.settings.volume.ToString();
            }
            if (now.ToString() == stopPlaying.ToString() && playingAlarm)
            {
                playingAlarm = false;
                wplayer.controls.stop();
            }
        }
        //do wyrzucenia
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void tabKZKGOP_Click(object sender, EventArgs e) { }
        private void tbInter1_TextChanged(object sender, EventArgs e) { }
        private void tabMain_Click(object sender, EventArgs e){ }

        //w budowie
        SpeechSynthesizer synth = new SpeechSynthesizer();
        private void button14_Click(object sender, EventArgs e)
        {
            foreach (InstalledVoice voice in synth.GetInstalledVoices())
            {
                VoiceInfo info = voice.VoiceInfo;

                MessageBox.Show(" Name:          " + info.Name);
                
            }
            /*if (richTextBox1.Text != "")
            {
                System.Collections.ObjectModel.ReadOnlyCollection<System.Speech.Synthesis.InstalledVoice> tab = reader.GetInstalledVoices();
                for (int i = 0; i < tab.Count(); i++)
                {
                    MessageBox.Show(tab[i].VoiceInfo.Name);
                }
                reader.Dispose();
                reader = new SpeechSynthesizer();
                reader.SpeakAsync(richTextBox1.Text);
            }
            else
            {
                MessageBox.Show("Enter the text");
            }*/
        }
        private void pokazTray_Click(object sender, EventArgs e)
        {
           // IkonaTray.Visible = false;
            this.Show();

            this.WindowState = FormWindowState.Normal;
        }
        private void button16_Click(object sender, EventArgs e)
        {
            fd = new OpenFileDialog();
            fd.Filter = "Pliki MP3 (.mp3)|*.mp3";
            fd.Multiselect = false;
            if (fd.ShowDialog() == DialogResult.OK)
            {

            }
        }

    }
}
