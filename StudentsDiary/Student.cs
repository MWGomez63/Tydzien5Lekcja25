using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsDiary
{
    // aby zabezpieczć klasę przed dalszym dziedziczeniem używamy SEALED
    // public sealed class Student : Person
    public class Student
    {
        // to jest pole: [public int Id;] a to jest właściwość: [public int Id { get; set; }]

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Comments { get; set; }

        // oceny z przedmiotów
        public string Math { get; set; }
        public string Technology { get; set; }
        public string Physics { get; set; }
        public string PolishLang { get; set; }
        public string ForeignLang { get; set; }

        // czy uczeń ma zajęcia dodatkowe
        public bool AdditionalActivities { get; set; }

        // Id grupy (klasa)
        public string GroupID { get; set; }
    }
}

