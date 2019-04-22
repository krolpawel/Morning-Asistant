using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poranny_asystent
{
    class Autobus
    {
        private string busNr;
        private string przystanekStart;
        private string przystanekKoniec;
        private string kierunek;
        private int iloscPrzystankow;
        private string odjazd;
        private string przyjazd;

        public Autobus()
        {
            busNr = "?";
            przystanekStart = "?";
            przystanekKoniec = "?";
            kierunek = "?";
            iloscPrzystankow = 0;
            odjazd = "?";
            przyjazd = "?";
        }
        public Autobus(string bn, string ps, string pk, string k, int ip, string o, string p)
        {
            busNr = bn;
            przystanekStart = ps;
            przystanekKoniec = pk;
            kierunek = k;
            iloscPrzystankow = ip;
            odjazd = o;
            przyjazd = p;
        }

        public string BusNr
        {
            get { return busNr; }
            set { busNr = value; }
        }
        public string PrzystanekStart
        {
            get { return przystanekStart; }
            set { przystanekStart = value; }
        }
        public string PrzystanekKoniec
        {
            get { return przystanekKoniec; }
            set { przystanekKoniec = value; }
        }
        public string Kierunek
        {
            get { return kierunek; }
            set { kierunek = value; }
        }
        public int IloscPrzystankow
        {
            get { return iloscPrzystankow; }
            set { iloscPrzystankow = value; }
        }
        public string Odjazd
        {
            get { return odjazd; }
            set { odjazd = value; }
        }
        public string Przyjazd
        {
            get { return przyjazd; }
            set { przyjazd = value; }
        }

    }
}
