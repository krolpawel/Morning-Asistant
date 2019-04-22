using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poranny_asystent
{
    public class PlanDzien:PlanZajec
    {
        private int _dayId;
        private string _dayName;
        private List<Lekcja> _lekcje = new List<Lekcja>();

        public List<Lekcja> Lekcje
        {
            get { return _lekcje; }
            set { _lekcje = value; }
        }
        public PlanDzien()
        {
            _dayId = 0;
            _dayName = "?";
        }
        public PlanDzien(int dayId, string dayName)
        {
            _dayId = dayId;
            _dayName = dayName;
        }

        public int DayId {
            get { return _dayId; }
            set { _dayId = value; }
        }
        public string DayName {
            get { return _dayName; }
            set { _dayName = value; }
        }

        public void AddLesson(Lekcja l)
        {
            _lekcje.Add(l);
        }
    }
}
