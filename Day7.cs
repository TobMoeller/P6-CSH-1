using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P6_CSH_1 {
    class Day7 : Day {
        public Day7() : base(7) {
            addAufgabe("Prüfungsvorbereitung: File / Directory / Path", Transcript1);
            addAufgabe("Prüfungsvorbereitung: FileStream / CryptoStream und ZipFile", Transcript2);
        }

        /*
         *  Teil 1: File / Directory / Path
         */

        public void Transcript1() {
            /*
             * Methoden der Klassen File und Directory
             */
            string[] textBereich = new string[5];
            string path = @"C:\Users\tmide\Documents\dev\P6-CSH\P6-Dateien\Vorbereitung1903";

            for (int i = 0; i < 5; i++) textBereich[i] = "Textmitteilung Nr." + (i + 1);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            Directory.SetCurrentDirectory(path);
            /*
             * Mit File arbeiten
             */
            File.WriteAllText("text.txt", TextVerbund(textBereich));
            if (!File.Exists("textkopie.txt")) {
                Console.WriteLine("Datei nicht vorhanden - textkopie angelegt");
                File.OpenWrite("textkopie.txt");
            }
            File.Copy("text.txt", "textkopie.txt", true);
            string[] textkopie = new string[textBereich.Length];
            textkopie = File.ReadAllLines("text.txt");
            foreach (string s in textkopie) {
                Console.WriteLine(s);
            }
            Console.WriteLine("-------------------------------------------------------");
            FileInfo fi = new FileInfo("text.txt");
            Console.WriteLine("Vollst. Name: " + fi.FullName);
            Console.WriteLine("Größe: " + fi.Length + " Bytes");
            Console.WriteLine("Verzeichnis: " + fi.Directory);
            Console.WriteLine("erzeugt am: " + fi.CreationTime);
            Console.WriteLine("-------------------------------------------------------");
            Console.ReadKey();
            /*
             * Mit Directory und DirectoryInfo arbeiten
             */
            string current = path;
            string[] unterVerzeichnisse;
            Console.WriteLine("-------------------------------------------------------");
            DirectoryInfo di = new DirectoryInfo(current);
            di.CreateSubdirectory("fileDir");
            EinzelAusgabe(Directory.GetDirectories(current));
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine(di.Root);
            Directory.SetCurrentDirectory(di.Root.FullName);
            unterVerzeichnisse = Directory.GetDirectories(Directory.GetCurrentDirectory());
            foreach (string s in unterVerzeichnisse) {
                Console.WriteLine(s);
            }
            Console.ReadKey();
        }
        string TextVerbund(string[] strarr) {
            StringBuilder aber = new StringBuilder();
            foreach (string s in strarr) {
                aber.Append("(Verbund) " + s + Environment.NewLine);
                //Console.WriteLine(aber);
            }
            return aber.ToString();
        }

        void EinzelAusgabe(string[] strarr) {
            foreach (string s in strarr) {
                Console.WriteLine(s);
            }
        }

        /*
         *  Teil 2: FileStream / CryptoStream und ZipFile
         */

        public void Transcript2() {
            int i;
            /*
             * Dateioperationen in C#
             */
            byte[] ausgabeZeichen = new byte[52];
            for (i = 0; i < 26; i++) {
                ausgabeZeichen[i] = (byte)('a' + i);
            }

            for (int j = 0; j < 26; j++) {
                ausgabeZeichen[i] = (byte)('A' + j);
                i++;
            }
            try {
                Directory.SetCurrentDirectory(@"C:\Users\tmide\Documents\dev\P6-CSH\P6-Dateien\Vorbereitung1903");
                using (FileStream schreiben = new FileStream("wiederholung_teil02.txt", FileMode.Create, FileAccess.Write)) {
                    schreiben.Write(ausgabeZeichen, 0, ausgabeZeichen.Length);
                    schreiben.Close();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

            /*
             * Arbeit mit StreamReader
             */
            string dsn = Directory.GetCurrentDirectory() + @"\wiederholung_teil02.txt";
            if (File.Exists(dsn)) {
                FileStream fsl = new FileStream(dsn, FileMode.Open, FileAccess.Read);
                StreamReader lesen = new StreamReader(fsl);
                Console.WriteLine("Dateiinhalt gelesen:");
                Console.WriteLine(lesen.ReadToEnd());
                fsl.Close();
            } else {
                Console.WriteLine("Datei nicht gefunden");
            }
            Console.WriteLine("---------------------------------------------------");
            string text = "Wiederholen ist wichtig";
            Console.WriteLine("Cäsar-Verschlüsselung");
            string chiffre = CaesarCode(text);
            Console.WriteLine("Originaltext: " + text + Environment.NewLine + " Verschlüsselt: " + chiffre);
            Console.WriteLine("Entschlüsselt: " + CaesarCode(chiffre));
            Console.ReadKey();
            /*
             * Anwendung StreamWriter
             */
            StreamWriter schreib = new StreamWriter(dsn, true);
            schreib.WriteLine(Environment.NewLine + text);
            schreib.WriteLine(chiffre);
            schreib.Close();
            Console.WriteLine("Verschhlüsselten Text in Datei gespeichert.");
            /*
             * Anwendung ZipFile
             */
            string zipDatei = @"H:\wiederholung1903.zip";
            if (File.Exists(zipDatei)) File.Delete(zipDatei);
            ZipFile.CreateFromDirectory(Directory.GetCurrentDirectory(), zipDatei);
            Console.WriteLine("ZIP-Archiv erstellt von Verz.: " + Directory.GetCurrentDirectory());
        }
        string CaesarCode(string s) {
            string zk_ori = s.ToLower();
            char[] zk_enc = zk_ori.ToCharArray();

            for (int i = 0; i < zk_ori.Length; i++) {
                if (zk_ori[i] >= 'a' && zk_ori[i] < 'n') {
                    zk_enc[i] = Convert.ToChar(Convert.ToInt32(zk_ori[i]) + 13);
                } else if (zk_ori[i] >= 'n' && zk_ori[i] <= 'z') {
                    zk_enc[i] = Convert.ToChar(Convert.ToInt32(zk_ori[i]) - 13);
                } else {
                    zk_enc[i] = Convert.ToChar(zk_ori[i]);
                }
            }
            return new string(zk_enc);
        }
    }
}
