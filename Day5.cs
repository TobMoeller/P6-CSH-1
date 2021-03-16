using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace P6_CSH_1 {
    class Day5 : Day {
        public Day5() : base(5) {
            addAufgabe("Mitschrift 1: XmlSerializer", Transcript1);
            addAufgabe("Mitschrift 2: Deserialize", Transcript2);
            addAufgabe("Übung 1: XmlSerializer", Aufgabe01);
        }
        public void Transcript1() {
            // XML Serializer
            // Einzelne XML Datei
            string quellVerzeichnis = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\dev\P6-CSH\P6-Dateien";
            string datei = quellVerzeichnis + @"\XmlTest.xml";
            Kurs kurs = new Kurs();
            kurs.KursBezeichner = "CSH4";
            kurs.Beginn = Convert.ToDateTime("16.03.2021");
            kurs.Dauer = 8;

            using (FileStream fileStream = new FileStream(datei, FileMode.Create, FileAccess.Write)) {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Kurs));
                xmlSerializer.Serialize(fileStream, kurs);
                fileStream.Close();
            }

            // Aus einer Collection
            new Kurs("CSH4", Convert.ToDateTime("16.03.2021"), 9);
            new Kurs("CSH1", Convert.ToDateTime("01.01.2001"), 10);

            string datei2 = quellVerzeichnis + @"\XmlTestCollection.xml";
            using (FileStream fileStream = new FileStream(datei2, FileMode.Create, FileAccess.Write)) {
                XmlSerializer xmlSerializer = new XmlSerializer(Kurs.Kurse.GetType());
                xmlSerializer.Serialize(fileStream, Kurs.Kurse);
                fileStream.Close();
            }
        }

        public void Transcript2() {
            // XML Deserialize
            string quellVerzeichnis = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\dev\P6-CSH\P6-Dateien";
            string datei = quellVerzeichnis + @"\XmlTest.xml";
            string datei2 = quellVerzeichnis + @"\XmlTestCollection.xml";

            // Einzelne Datei
            Kurs kurs1;
            using (FileStream fileStream = new FileStream(datei, FileMode.Open, FileAccess.Read)) {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Kurs));
                kurs1 = xmlSerializer.Deserialize(fileStream) as Kurs;
                Kurs.KursAusgabe(kurs1);
            }

            // Collection
            Console.WriteLine("\nAus Liste:\n");
            using (FileStream fileStream = new FileStream(datei2, FileMode.Open, FileAccess.Read)) {
                XmlSerializer xmlSerializer = new XmlSerializer(Kurs.Kurse.GetType());
                Kurs.Kurse = xmlSerializer.Deserialize(fileStream) as List<Kurs>;
                Kurs.AlleAusgeben();
            }

        }

        public void Aufgabe01() {
            string quellVerzeichnis = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\dev\P6-CSH\P6-Dateien";
            
            Teil1(quellVerzeichnis);
            Teil2(quellVerzeichnis);
        }

        public void Teil1(string quellVerzeichnis) {
            string datei = quellVerzeichnis + @"\stadt_einzel.xml";
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Day5Stadt));

            // Serialization
            Day5Stadt stadt = new Day5Stadt("Hauswurz", 3000, "Fulda", "Hessen");
            using (FileStream fileStream = new FileStream(datei, FileMode.Create, FileAccess.Write)) {
                xmlSerializer.Serialize(fileStream, stadt);
            }

            // Deserialization
            Day5Stadt tempStadt;
            using (FileStream fileStream = new FileStream(datei, FileMode.Open, FileAccess.Read)) {
                tempStadt = xmlSerializer.Deserialize(fileStream) as Day5Stadt;
            }
            Day5Stadt.Ausgabe(tempStadt);
        }

        public void Teil2(string quellVerzeichnis) {
            string datei = quellVerzeichnis + @"\stadt_liste.xml";
            //XmlSerializer xmlSerializer = new XmlSerializer(Day5Stadt.Staedte.GetType());
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Day5Stadt>));

            new Day5Stadt("Hauswurz2", 6000, "Fulda2", "Hessen2");
            new Day5Stadt("Hauswurz3", 9000, "Fulda3", "Hessen3");
            new Day5Stadt("Hauswurz4", 12000, "Fulda4", "Hessen4");
            new Day5Stadt("Hauswurz5", 15000, "Fulda5", "Hessen5");

            // Serialization
            using (FileStream fileStream = new FileStream(datei, FileMode.Create, FileAccess.Write)) {
                xmlSerializer.Serialize(fileStream, Day5Stadt.Staedte);
            }

            // Deserialization
            using (FileStream fileStream = new FileStream(datei, FileMode.Open, FileAccess.Read)) {
                Day5Stadt.Staedte = xmlSerializer.Deserialize(fileStream) as List<Day5Stadt>;
            }
            Day5Stadt.AlleAusgeben();
        }

    }
    public class Kurs {
        public static List<Kurs> Kurse { get; set; } = new List<Kurs>();
        public string KursBezeichner { get; set; }
        public DateTime Beginn { get; set; }
        public int Dauer { get; set; }

        public Kurs() {

        }

        public Kurs(string kursBezeichner, DateTime beginn, int dauer) {
            KursBezeichner = kursBezeichner;
            Beginn = beginn;
            Dauer = dauer;
            Kurse.Add(this);
        }

        public static void KursAusgabe(Kurs kurs) {
            Console.WriteLine("Bezeichner: " + kurs.KursBezeichner);
            Console.WriteLine("Beginn: " + kurs.Beginn.ToShortDateString());
            Console.WriteLine("Dauer: " + kurs.Dauer);
        }

        public static void AlleAusgeben() {
            foreach (Kurs kurs in Kurse) {
                KursAusgabe(kurs);
            }
        }
    }
}
