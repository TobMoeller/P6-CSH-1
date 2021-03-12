using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P6_CSH_1 {
    class Day2 : Day {
        public Day2() : base(2) {
            addAufgabe("Mitschrift 1: Directory (und LINQ)", Transcript1);
            addAufgabe("Mitschrift 2: StreamWriter / -Reader", Transcript2);
            addAufgabe("Mitschrift 3: ZipFile", Transcript3);
        }

        public void Transcript1() {
            string[] verzeichnisse;
            string aktuell;
            string suchString = "";
            Console.WriteLine("Eingabe gesuchte Zeichenkette: ");
            suchString = Console.ReadLine();
            //Directory.SetCurrentDirectory(@"H:\");
            aktuell = Directory.GetCurrentDirectory();
            Console.WriteLine("Das aktuelle Verzeichnis lautet: {0}", aktuell);
            Directory.CreateDirectory(aktuell + @"\1103_SubDir01");
            Directory.CreateDirectory(aktuell + @"\1103_SubDir02");
            verzeichnisse = Directory.GetDirectories(aktuell);

            //var dateien = from auswahlDateien in Directory.EnumerateFiles(aktuell, "*.*", SearchOption.AllDirectories)
            //              from zeile in File.ReadLines(auswahlDateien)
            //              where zeile.Contains(suchString)
            //              select new {
            //                  Datei = auswahlDateien,
            //                  Zeile = zeile
            //              };
            //foreach (var d in dateien) {
            //    Console.WriteLine("{0} beinhaltet {1}\n", d.Datei, d.Zeile);
            //}

            //Console.WriteLine("{0} zeilen gefunden.", dateien.Count());

            DirectoryInfo dinf = new DirectoryInfo(aktuell); // Keine Prüfung auf Existenz
            
        }

        public void Transcript2() {
            Console.WriteLine(Environment.GetEnvironmentVariable("OS"));
            //string dateiName = Path.GetTempPath(); // Gibt Pfad für Temp Ordner, ansonsten mit Environment.SpecialFolder.?
            string dateiName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\dev\P6-CSH\P6-Dateien\test1.txt";
            //Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            Console.WriteLine(dateiName);

            try {
                StreamWriter streamWriter = new StreamWriter(dateiName, false); // true - Fortschreiben / false - Überschreiben
                for (int i = 0; i < 10; i++) {
                    streamWriter.WriteLine($"Dies ist die {i}. Zeile");
                }
                streamWriter.Flush();
                streamWriter.Close();
            } catch (Exception ex) {
                Console.WriteLine("Writer Fehler: " + ex.Message);
            }

            try {
                StreamReader streamReader = new StreamReader(dateiName);
                string txt = "";
                // Lese die gesamte Datei
                txt = streamReader.ReadToEnd();
                Console.WriteLine(txt);

                streamReader.Close();
                streamReader = new StreamReader(dateiName);

                // Optional: Zeile für Zeile
                while (streamReader.Peek() >= 0) {
                    txt = streamReader.ReadLine();
                    Console.WriteLine(txt);
                }
                streamReader.Close();
            } catch (Exception ex) {
                Console.WriteLine("Reader Fehler: " + ex.Message);
            }
        }

        public void Transcript3() {
            string quellVerzeichnis = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\dev\P6-CSH\P6-Dateien";

            // Zip Vorgang
            string zipDatei = quellVerzeichnis + @"\zipper.zip";
            Console.WriteLine(zipDatei);
            if (File.Exists(zipDatei)) {
                File.Delete(zipDatei);
                Console.WriteLine(zipDatei + " erfolgreich gelöscht");
            }

            ZipFile.CreateFromDirectory(quellVerzeichnis + @"\zippen", zipDatei);

            // Entpacken
            string zielVerzeichnis = quellVerzeichnis + @"\entzippen";
            if (!Directory.Exists(zielVerzeichnis)) {
                Directory.CreateDirectory(zielVerzeichnis);
            } else {
                if (Directory.GetFiles(zielVerzeichnis).Length > 0) {
                    foreach (string dateiName in Directory.GetFiles(zielVerzeichnis)) {
                        try {
                            File.Delete(dateiName);
                            Console.WriteLine(dateiName + " wurde gelöscht");
                        } catch (Exception ex) {
                            Console.WriteLine("Fehler beim Löschen");
                        }
                    }
                }
            }
            ZipFile.ExtractToDirectory(zipDatei, zielVerzeichnis);
        }
    }
}
