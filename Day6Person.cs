using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace P6_CSH_1 {
    public class Day6Person {
        [XmlAttribute("Name")]
        public string Name;
        [XmlAttribute("Vorname")]
        public string Vorname;
        [XmlAttribute("Alter")]
        public int Alter;

        public Day6Person() { }
        public Day6Person(string famnam, string vnam, int age) {
            Name = famnam;
            Vorname = vnam;
            Alter = age;
        }
    }
    public class Day6PersonenListe {

        [XmlElement("Personenarchiv")]
        public string ArchivName;

        [XmlElement("Einzelangaben")]
        public List<Day6Person> Personen;

        public Day6PersonenListe(string name) {
            ArchivName = name;
        }
        public Day6PersonenListe() {

        }
    }
}
