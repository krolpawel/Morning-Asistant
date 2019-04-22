using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poranny_asystent
{
    public partial class Form2 : Form
    {
        private Lekcja lekcja;
        string day;

        public Form2()
        {
            InitializeComponent();
        }
        public Form2(Lekcja l, string d)
        {
            InitializeComponent();
            lekcja = l;
            day = d;
            Wczytaj();
        }
        public void Wczytaj()
        {
            label2.Text = lekcja.Name;
            label4.Text = day + " / " + lekcja.StartTime + ":00";
            label6.Text = lekcja.Teacher;
            if (lekcja.Teacher == "")
            {
                linkLabel1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string name = lekcja.Teacher;
            try
            {
                name = name.Substring(0, name.IndexOf(" ",name.IndexOf(" ") + 2));
            }
            catch (Exception ex) { System.Console.WriteLine("Problem z wyodrębnieniem nazwiska!\n"+ex); }
            name=name.Replace(" ", "+");
            string link = "http://www.google.pl/search?q=Politechnika+Czestochowska+" + name;
            System.Diagnostics.Process.Start(link);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string przedmiot = lekcja.Name;
            string link = "http://www.google.pl/search?q=Politechnika+Czestochowska+" + przedmiot;
            System.Diagnostics.Process.Start(link);
        }
    }
}
