using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poranny_asystent
{
    class Prognoza
    {
        private string miasto;
        private int temperatura;
        private string niebo;
        private int odczuwalna;
        private int wiatr;
        private int porywy;
        private string chmury;
        private double opady;
        private int cisnienie;
        private int wilgotnosc;
        private string termika;
        private string biomet;

        public string Miasto { get; set; }
        public int Temperatura { get; set; }
        public string Niebo { get; set; }
        public int Odczuwalna { get; set; }
        public int Wiatr { get; set; }
        public int Porywy { get; set; }
        public string Chmury { get; set; }
        public double Opady { get; set; }
        public int Cisnienie { get; set; }
        public int Wilgotnosc { get; set; }
        public string Termika { get; set; }
        public string Biomet { get; set; }

        public Prognoza()
        {
            miasto = "?";
            temperatura = -100;
            niebo = "?";
            odczuwalna = -100;
            wiatr = -1;
            porywy = -1;
            chmury = "?";
            opady = -1.0;
            cisnienie = -1;
            wilgotnosc = -1;
            termika = "?";
            biomet = "?";
        }

    }
}
