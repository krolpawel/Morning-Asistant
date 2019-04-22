using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poranny_asystent
{
     public class Lekcja
    {
        private int _startTime;
        private string _name;
        private string _type;
        private string _teacher;
        private string _place;

        public Lekcja()
        {
            _startTime = 0;
            _name = "?";
            _type = "?";
            _teacher = "?";
            _place = "?";
        }

        public int StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public string Teacher
        {
            get { return _teacher; }
            set { _teacher = value; }
        }
        public string Place
        {
            get { return _place; }
            set { _place = value; }
        }

        public string TypeFinder(string text)
        {
            switch (text)
            {
                case " wyk.": text = "Wykład"; break;
                case " lab.": text = "Laboratorium"; break;
                case " ćw.": text = "ćwiczenia"; break;
                default: text = "nieznany"; break;
            }
            return text;
        }

        public void Cutter(string all)
        {
            _name = all.Substring(4);
            _name = _name.Substring(0, _name.IndexOf("<",2)); //wycięcie do końca wiersza
            _type = TypeFinder(_name.Substring(_name.IndexOf(" ", _name.Length - 6))); //wyciągnięcie i podmiana typu
            _name = _name.Substring(0, _name.IndexOf(" ", _name.Length - 6)); //odcięcie rodzaju zajęć
            _teacher = all.Substring(all.IndexOf("<br>"));
            _teacher = _teacher.Substring(4);
            _place = _teacher.Substring(_teacher.IndexOf("<br>"));
            _place = _place.Substring(4);
            _place = _place.Substring(0, _place.Length - 5);
            _teacher = _teacher.Substring(0, _teacher.IndexOf("<br>"));
        }
    }
}
