using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poranny_asystent
{
    class Pociag
    {
        private int nrPociagu;
        private string dataStart;
        private string dataKoniec;
        private string stacjaStart;
        private string stacjaKoniec;
        private string odjazd;
        private string przyjazd;
        private string czasPodrozy;
        private int opoznienie;
        private bool selected;

        public int NrPociagu {
            get { return nrPociagu; }
            set { nrPociagu = value; }
        }
        public string DataStart {
            get { return dataStart; }
            set { dataStart = value; }
        }
        public string DataKoniec
        {
            get { return dataKoniec; }
            set { dataKoniec = value; }
        }
        public string StacjaStart {
            get { return stacjaStart; }
            set { stacjaStart = value; }
        }
        public string StacjaKoniec
        {
            get { return stacjaKoniec; }
            set { stacjaKoniec = value; }
        }
        public string Odjazd {
            get { return odjazd; }
            set { odjazd = value; }
        }
        public string Przyjazd
        {
            get { return przyjazd; }
            set { przyjazd = value; }
        }
        public string CzasPodrozy
        {
            get { return czasPodrozy; }
            set { czasPodrozy = value; }
        }
        public int Opoznienie {
            get { return opoznienie; }
            set { opoznienie = value; }
        }
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }
        public Pociag()
        {
            nrPociagu = -1;
            dataStart = "?";
            dataKoniec = "?";
            stacjaStart = "?";
            stacjaKoniec = "?";
            odjazd = "?";
            przyjazd = "?";
            czasPodrozy = "?";
            opoznienie = -1;
            selected = false;
        }
        public Pociag(int np, string ds, string dk, string ss, string sk, string o, string p, string cp, int op, bool sel)
        {
            nrPociagu = np;
            dataStart = ds;
            dataKoniec = dk;
            stacjaStart = ss;
            stacjaKoniec = sk;
            odjazd = o;
            przyjazd = p;
            czasPodrozy = cp;
            opoznienie = op;
            selected = sel;
        }
    }
}
