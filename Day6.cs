using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace P6_CSH_1 {
    class Day6 : Day {
        public Day6() : base(6) {
            addAufgabe("Mitschrift 1: XmlSerializer Ergänzung", Transcript01);
            addAufgabe("Übung 1: XmlReader / Writer", Uebung1);
        }

        public void Transcript01() {
            string quellVerzeichnis = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\dev\P6-CSH\P6-Dateien";

            Day6PersonenListe archiv = new Day6PersonenListe("Archivo");
            archiv.Personen = new List<Day6Person>();
            archiv.Personen.Add(new Day6Person("Müller", "Hans", 40));
            archiv.Personen.Add(new Day6Person("Meyer", "Franz", 45));
            archiv.Personen.Add(new Day6Person("Klausen", "Britta", 42));
            XSerializer(archiv, quellVerzeichnis);
            archiv = XDeserializer(quellVerzeichnis);
            foreach (Day6Person p in archiv.Personen) {
                Console.WriteLine("Name: {0} Vorname: {1} Alter: {2} Jahre", p.Name, p.Vorname, p.Alter);
            }
        }
        static Day6PersonenListe XDeserializer(string quellVerzeichnis) {
            string path = quellVerzeichnis + @"\1703_testXMLElements.xml";
            XmlSerializer deserializer = new XmlSerializer(typeof(Day6PersonenListe));
            using (FileStream xmlEingabe = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                return (Day6PersonenListe)deserializer.Deserialize(xmlEingabe);
            }
        }

        static void XSerializer(Day6PersonenListe pl, string quellVerzeichnis) {
            string path = quellVerzeichnis + @"\1703_testXMLElements.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(Day6PersonenListe));
            using (FileStream xmlAusgabe = new FileStream(path, FileMode.Create, FileAccess.Write))
            // FileStream xmlAusgabe = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Append);
            {
                serializer.Serialize(xmlAusgabe, pl);
                //xmlAusgabe.Close(); // verzichtbar bei using Anweisung
            }
        }


        public void Uebung1() {
            string quellVerzeichnis = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\dev\P6-CSH\P6-Dateien";
            XmlEinlesen(quellVerzeichnis);
            Hausverwaltung.VerwaltungAusgeben();
            CsvSchreiben(quellVerzeichnis);
        }

        public void XmlEinlesen(string quellVerzeichnis) {
            string datei = quellVerzeichnis + "\\hausverwaltung.xml";
            FileStream fileStream = new FileStream(datei, FileMode.Open, FileAccess.Read);
            XmlReader xmlReader = XmlReader.Create(fileStream);

            Hausverwaltung hausverwaltung = null;
            Haus haus = null;

            while (xmlReader.Read()) {
                if (xmlReader.NodeType == XmlNodeType.Element) {
                    if (xmlReader.Name == "HausVerwaltung") {
                        if (xmlReader.HasAttributes) {
                            xmlReader.MoveToNextAttribute();
                            hausverwaltung = new Hausverwaltung(Convert.ToInt32(xmlReader.Value));
                            //Console.WriteLine("Wir kriegen: " + xmlReader.Value);
                        }
                    } else if (xmlReader.Name == "Haus") {
                        haus = new Haus();
                        hausverwaltung?.Haeuser.Add(haus);
                        if (xmlReader.HasAttributes) {
                            while (xmlReader.MoveToNextAttribute()) {
                                if (xmlReader.Name == "ID") {
                                    haus.ID = Convert.ToInt32(xmlReader.Value);
                                } else if (xmlReader.Name == "Neukunde") {
                                    haus.Neukunde = xmlReader.Value == "ja" ? true : false;
                                }
                            }
                        }
                        for (int i = 0; i < 5; i++) {
                            xmlReader.Read();
                            //Console.WriteLine(xmlReader.Name);
                            if (xmlReader.NodeType == XmlNodeType.Element) {
                                //Console.WriteLine(i + " " + xmlReader.ReadElementContentAsDouble());
                                switch (xmlReader.Name) {
                                    case "Muell": haus.Muell = xmlReader.ReadElementContentAsDouble(); break;
                                    case "Strom": haus.Strom = xmlReader.ReadElementContentAsDouble(); break;
                                    case "Hausmeister": haus.Hausmeister = xmlReader.ReadElementContentAsDouble(); break;
                                    case "Strassenreinigung": haus.Strassenreinigung = xmlReader.ReadElementContentAsDouble(); break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CsvSchreiben(string quellVerzeichnis) {
            string datei = quellVerzeichnis + "\\hausverwaltung.csv";
            FileStream fileStream = new FileStream(datei, FileMode.Create, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            foreach (Hausverwaltung hausverwaltung in Hausverwaltung.Verwaltungen) {
                streamWriter.WriteLine("Jahr:;" + hausverwaltung.Jahr);
                streamWriter.WriteLine("HausID;Neukunde;Muell;Strom;Hausmeister;Strassenreinigung;Gesamt");
                double verwaltungSumme = hausverwaltung.Summieren();
                foreach (Haus haus in hausverwaltung.Haeuser) {
                    streamWriter.WriteLine(
                        haus.ID + ";" + 
                        (haus.Neukunde ? "ja" : "nein") + ";" +
                        haus.Muell + ";" +
                        haus.Strom + ";" +
                        haus.Hausmeister + ";" +
                        haus.Strassenreinigung + ";" + 
                        haus.KostenAddieren()
                    );
                }
                streamWriter.WriteLine(";Summe;" +
                        hausverwaltung.Muell + ";" +
                        hausverwaltung.Strom + ";" +
                        hausverwaltung.Hausmeister + ";" +
                        hausverwaltung.Strassenreinigung + ";" +
                        verwaltungSumme
                );
            }
            streamWriter.Flush();
            streamWriter.Close();
            fileStream.Close();
        }
    }

    public class Haus {
        public int ID { get; set; }
        public bool Neukunde { get; set; }
        public double Muell { get; set; }
        public double Strom { get; set; }
        public double Hausmeister { get; set; }
        public double Strassenreinigung { get; set; }

        public void HausAusgeben() {
            Console.WriteLine("ID: " + ID);
            if (Neukunde) Console.WriteLine("Neukunde: " + Neukunde);
            Console.WriteLine("Muell: " + Muell);
            Console.WriteLine("Strom: " + Strom);
            Console.WriteLine("Hausmeister: " + Hausmeister);
            Console.WriteLine("Strassenreinigung: " + Strassenreinigung);
            Console.WriteLine();
        }
        public double KostenAddieren() {
            return Muell + Strom + Hausmeister + Strassenreinigung;
        }
    }

    public class Hausverwaltung {
        public static List<Hausverwaltung> Verwaltungen { get; set; } = new List<Hausverwaltung>();
        public double Muell { get; set; }
        public double Strom { get; set; }
        public double Hausmeister { get; set; }
        public double Strassenreinigung { get; set; }
        public int Jahr { get; set; }
        public List<Haus> Haeuser { get; set; }
        public Hausverwaltung(int jahr) {
            Jahr = jahr;
            Haeuser = new List<Haus>();
            Verwaltungen.Add(this);
        }
        public void AllesAusgeben() {
            Console.WriteLine("\n--- Verwaltung des Jahres: " + Jahr + " ---\n");
            foreach (Haus haus in Haeuser) {
                haus.HausAusgeben();
            }
        }
        public static void VerwaltungAusgeben() {
            foreach (Hausverwaltung hausverwaltung in Verwaltungen) {
                hausverwaltung.AllesAusgeben();
            }
        }
        public double Summieren() {
            foreach (Haus haus in Haeuser) {
                Muell += haus.Muell;
                Strom += haus.Strom;
                Hausmeister += haus.Hausmeister;
                Strassenreinigung += haus.Strassenreinigung;
            }
            return Muell + Strom + Hausmeister + Strassenreinigung;
        }
    }
}
