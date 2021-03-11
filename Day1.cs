using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P6_CSH_1 {
    class Day1 : Day{
        public Day1() : base(1) {
            addAufgabe("Mitschrift 1: FileStream", Transcript1);
            addAufgabe("Mitschrift 2: File", Transcript2);
            addAufgabe("Mitschrift 3: FileInfo", Transcript3);
            addAufgabe("Übung 1", Uebung1);
        }
        public void Transcript1() {
            /*
             *      Schreiben
             */

            // Array mit je 1byte Werten (0-255)
            // Wenn man (char) 'z' mit dabei char, wandelt er alles andere Zeichen um wegen verschiedenen Zeichensätzen (UTF16 / ASCII)
            // ASCII ist symmetrisch:
            // A - 0x41 / 65, Z - 0x5A 90
            // a - 0x61 97, z - 0x7A 122
            byte[] arrAusgabe = { 1, 0, 2, 66, 67, 68, 4, (byte)'B', 5, 0, 6, 255, 0, 7, 0, 8, 0, 9, 0, 10, 0 };
            // @ steht für Verbatimschreibweise: \ wird als Zeichen aufgenommen
            string path = @"C:\Users\tmide\Documents\dev\P6-CSH\P6-Dateien\Testfile.txt";
            // char nutzt in C# 2byte
            Console.WriteLine($"Anzahl Bytes byte: {sizeof(byte)}, Anzahl Bytes char: {sizeof(char)}");
            FileStream fistr = new FileStream(path, FileMode.Create);
            try {
                fistr.Write(arrAusgabe, 0, arrAusgabe.Length);
                fistr.Close(); // Idealerweise innerhalb try/catch da ohne Abschluss kein EOF gesetzt wird und Datei freigegeben
            } catch (Exception ex) {
                Console.WriteLine(Environment.NewLine + "Datei failed: " + ex.Message);
            }
            Console.WriteLine("Schritt 1: Datei geschrieben");

            /*
             *      Lesen
             */

            byte[] arrEingabe = ReadFile(path);
            foreach (byte item in arrEingabe) {
                Console.WriteLine("byte: " + item);
            }
        }

        public byte[] ReadFile(string filePath) {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try {
                int length = (int)fileStream.Length;  // ermitteln der Dateigroesse
                buffer = new byte[length];            // erstellen eines Byte-Puffers
                int count;                            // aktuelle Anzahl der gelesenen Bytes
                int sum = 0;                          // Gesamtanzahl der gelesenen Zeichen

                // einlesen, solange die Lese-Methode nicht das Dateiende anzeigt (0)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0) {
                    sum += count;  // sum als Puffergroesse vor dem nächsten Einlesen
                }
            } finally {
                fileStream.Close(); // Beim lesenden Zugriff kann Close auch im finally stehen
            }
            return buffer;
        }

        public void Transcript2() {
            string dsn = @"C:\Users\tmide\Documents\dev\P6-CSH\P6-Dateien\strassen_osm.txt"; // dsn = DataSetName

            try {
                // Prüfen auf Existenz
                if (File.Exists(dsn)) {
                    Console.WriteLine("Datei ist vorhanden");
                    File.Copy(dsn, @"C:\Users\tmide\Documents\dev\P6-CSH\P6-Dateien\CSH4\strassen_dup.txt", true);
                    Console.WriteLine("Datei {0} wurde kopiert", dsn);
                    // Datei kpieren, wenn Kopie existierte dann diese überschreiben (overwrite - param = true)
                } else {
                    Console.WriteLine("Datei existiert nicht");
                    File.OpenWrite(dsn);
                    // falls nicht existiert, dann wird eine leere Datei erzeugt
                }
            } catch (Exception ex) {
                Console.WriteLine("Fehler: " + ex.Message);
            }
        }

        public void Transcript3() {
            FileInfo fi = new FileInfo(@"C:\Users\tmide\Documents\dev\P6-CSH\P6-Dateien\strassen_osm.txt");
            if (fi.Exists) {
                fi.OpenRead();
            }
            // Datei ist eigentlich sowieso zum Lesen geöffnet, das OpenRead legt die Datei aber auch an, falls diese nicht existiert
            // es wird die Datei zum Lesen geöffnet und im FilePointer aktualisiert
            Console.WriteLine($"Fullname: {fi.FullName}\n" +
                $"Laenge: {(fi.Length / 1024.0 / 1024).ToString("00.##")} MiB\n" + // das ToString rundet hier ebenfalls kaufmännisch
                $"Name: {fi.Name}"
            );
        }

        /*
         *  Übung 1
         *  
            Sie erstellen eine Klasse Kfz. Die Klasse hat die Attribute
            • Kfz-Kennzeichen
            • Baujahr
            • Leistung in kW
            • Hubraum in ccm
            • Typ
            • Modell
            Die Daten von 10 Kfz-Objekten sind vom Kfz-Verwalter in einer generischen Liste zu speichern. 
            Nachdem alle Objekte erfasst wurden, ist die Liste in einer "kfz.csv"-Datei (Verzeichnis mit dem Namen "_kfzVerwaltung" zu 
            speichern. Es soll ebenfalls eine Wiederherstellfunktion entwickelt werden, die alle Daten aus der Datei einliest und wieder 
            in der generischen Liste speichert. Nach der Wiederherstellung sollen alle Einträge aus der generischen Liste auf der Console 
            (oder GUI) angezeigt werden. Mit der Funktion Sicherung soll die csv-Datei kopiert werden. Die Kennung der Sicherungsdatei 
            soll .sec lauten.
        */

        public void Uebung1() {
            Verwalter vw = new Verwalter();
            vw.Erfassen(new Kfz("A BC 123", 1989, 50, 1600, "BMW", "3er"));
            vw.Erfassen(new Kfz("D EF 456", 2000, 150, 2400, "Ford", "Mustang"));
            vw.DateiSchreiben();
            vw.DateiLesen();
            vw.DateiSichern();
        }

        public class Kfz {
            string kfzKennzeichen;
            int baujahr;
            int leistung;
            int hubraum;
            string typ;
            string modelle;

            public string KfzKennzeichen { get => kfzKennzeichen; set => kfzKennzeichen = value; }
            public int Baujahr { get => baujahr; set => baujahr = value; }
            public int Leistung { get => leistung; set => leistung = value; }
            public int Hubraum { get => hubraum; set => hubraum = value; }
            public string Typ { get => typ; set => typ = value; }
            public string Modelle { get => modelle; set => modelle = value; }

            public Kfz(string kfzKennzeichen, int baujahr, int leistung, int hubraum, string typ, string modelle) {
                KfzKennzeichen = kfzKennzeichen;
                Baujahr = baujahr;
                Leistung = leistung;
                Hubraum = hubraum;
                Typ = typ;
                Modelle = modelle;
            }
            public Kfz() {

            }
        }

        public class Verwalter {
            private List<Kfz> kfzListe;
            internal List<Kfz> KfzListe { get => kfzListe; set => kfzListe = value; }

            public Verwalter() {
                KfzListe = new List<Kfz>();
            }

            public void Erfassen(Kfz k) {
                if (k != null) KfzListe.Add(k);
                Console.WriteLine("Anzahl der Kfz: " + KfzListe.Count);
            }

            public void DateiSchreiben() {
                string dsn = @"kfz.csv";
                string ausgabe = "";
                FileStream fs = new FileStream(dsn, FileMode.Append, FileAccess.Write, FileShare.None);
                StreamWriter swr = new StreamWriter(fs); // Zum Schreiben von Texten
                foreach (Kfz kfz in KfzListe) {
                    ausgabe += kfz.KfzKennzeichen + ";";
                    ausgabe += kfz.Baujahr + ";";
                    ausgabe += kfz.Leistung + ";";
                    ausgabe += kfz.Hubraum + ";";
                    ausgabe += kfz.Typ + ";";
                    ausgabe += kfz.Modelle + ";";
                    swr.WriteLine(ausgabe);
                    ausgabe = "";
                }
                swr.Close();
                fs.Close();
            }
            public void DateiLesen() {
                string zeile;
                string[] werte;
                string dsn = @"kfz.csv";
                kfzListe.Clear();
                if (File.Exists(dsn)) {
                    StreamReader srd = new StreamReader(dsn);
                    while(srd.Peek() != -1) { // Prüft ob das nächste Zeichen nicht -1 ist (EOF)
                        Kfz kfz = new Kfz();
                        zeile = srd.ReadLine();
                        werte = zeile.Split(';');
                        kfz.KfzKennzeichen = werte[0];
                        kfz.Baujahr = Convert.ToInt32(werte[1]);
                        kfz.Leistung = Convert.ToInt32(werte[2]);
                        kfz.Hubraum = Convert.ToInt32(werte[3]);
                        kfz.Typ = werte[4];
                        kfz.Modelle = werte[5];
                        kfzListe.Add(kfz);
                    }
                    srd.Close();
                }
                foreach (Kfz k in KfzListe) {
                    Console.WriteLine("Typ: " + k.Typ);
                }
            }
            public void DateiSichern() {
                string dsn = @"kfz.csv";
                if (File.Exists(dsn)) {
                    File.Copy(dsn, @"kfz.sec");
                }
            }
        }
    }

}
