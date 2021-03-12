using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace P6_CSH_1 {
    class Day3 : Day {
        public Day3() : base(3) {
            addAufgabe("Mitschrift 1: DES Encryption", Transcript1);
            addAufgabe("Mitschrift 2: Vertiefung Streams", Transcript2);
            addAufgabe("Mitschrift 3: AES Encryption", Transcript3);
        }

        public void Transcript1() {
            try {

                // DES Verschlüsselung

                string zuVerschluesseln = "Wir verschlüsseln mit DES";
                string verschluesselt;
                string offenerSchluessel = "fiae4610"; // public key, muss 8 Zeichen für 128bit Verschlüsselung sein
                string geheimSchluessel = "testi420"; // initialization vector, kann auch identisch mit public key sein muss gleiche Länge haben
                byte[] offenerSchluesselInByte = Encoding.UTF8.GetBytes(offenerSchluessel);
                byte[] geheimSchluesselInByte = Encoding.UTF8.GetBytes(geheimSchluessel);
                MemoryStream memoryStream = null;
                CryptoStream cryptoStream = null;
                byte[] zuVerschluesselnInByte = Encoding.UTF8.GetBytes(zuVerschluesseln);

                Console.WriteLine("Zu Verschlüsseln: " + zuVerschluesseln);

                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider()) {
                    memoryStream = new MemoryStream();
                    cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(offenerSchluesselInByte, geheimSchluesselInByte), CryptoStreamMode.Write);
                    //cryptoStream.Write(zuVerschluesselnInByte, 0, zuVerschluesselnInByte.Length); // entweder
                    StreamWriter streamWriter = new StreamWriter(cryptoStream); // oder
                    streamWriter.Write(zuVerschluesseln);
                    streamWriter.Flush();
                    cryptoStream.FlushFinalBlock();
                    streamWriter.Flush();
                    verschluesselt = Convert.ToBase64String(memoryStream.ToArray());
                    memoryStream.Close();
                    cryptoStream.Close();

                    Console.WriteLine("Verschlüsselt: " + verschluesselt);
                }

                // Entschlüsselung
                string entschluesselt = "";
                byte[] verschluesseltInByte = new byte[verschluesselt.Replace(" ", "+").Length]; // byte Array mit Länge von verschlüsselt mit ersetzten Leerzeichen
                verschluesseltInByte = Convert.FromBase64String(verschluesselt.Replace(" ", "+"));

                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider()) {
                    memoryStream = new MemoryStream();
                    cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(offenerSchluesselInByte, geheimSchluesselInByte), CryptoStreamMode.Write);
                    cryptoStream.Write(verschluesseltInByte, 0, verschluesseltInByte.Length);
                    cryptoStream.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    entschluesselt = encoding.GetString(memoryStream.ToArray());

                    Console.WriteLine("Entschlüsselt: " + entschluesselt);
                }

            } catch (Exception ex) {
                Console.WriteLine("Fehler: " + ex.Message);
            }
        }

        public void Transcript2() {
            MemoryStream memoryStream = new MemoryStream();
            //StreamWriter streamWriter = new StreamWriter(memoryStream);
            byte[] eingabeFeld;
            string eingabe = "Das ist ein TestString im Arbeitsspeicher";
            eingabeFeld = new byte[eingabe.Length];
            for (int i = 0; i < eingabe.Length; i++) {
                eingabeFeld[i] = (byte)eingabe[i];
            }
            memoryStream.Write(eingabeFeld, 0, eingabeFeld.Length);
            Console.WriteLine("Memory-Stream: " + memoryStream.Length);

            byte[] uebertragenFeld = new byte[memoryStream.Length];
            memoryStream.Read(uebertragenFeld, 0, uebertragenFeld.Length);
            Encoding encoding = Encoding.ASCII;
            Console.WriteLine(encoding.GetString(memoryStream.ToArray()));
        }

        public void Transcript3() {
            // AES Encryption
            string quellVerzeichnis = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\dev\P6-CSH\P6-Dateien";
            string datei = quellVerzeichnis + @"\AES.txt";
            Console.WriteLine(datei);

            // Einheitlicher Schlüssel zum Ver- und Entschlüsseln
            byte[] key = ErzeugeSchluessel("LeipzigerAllerlei2021", 32);
            AesEncrypt(datei, key);
            AesDecrypt(datei, key);
            AesDecryptTest(datei, key);
        }

        public void AesEncrypt(string datei, byte[] key) {
            try {

                FileStream fileStream = new FileStream(datei, FileMode.OpenOrCreate);

                //Erzeugen eines Objektes der Klasse Aes (Advanced Encryption Standard)  
                //und Festlegen des einheitlichen Schlüssels.  
                Aes aes = Aes.Create();
                aes.Key = key;

                // IV (Initialization Vector) an den Anfang der Datei speichern
                fileStream.Write(aes.IV, 0, aes.IV.Length);
                // Kontrolle Ausgabe IV:
                foreach (byte b in aes.IV) {
                    Console.Write(b + " ");
                }
                Console.WriteLine(Convert.ToBase64String(aes.IV));
                Console.WriteLine("Blocksize: " + aes.IV.Length);

                // neues CryptoStream Objekt durch Weiterleiten des FileStream Objektes und Verschlüsseln mit AES Algorithmus
                CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

                // StreamWriter zum erzeugen eines Text-Datenstroms
                StreamWriter streamWriter = new StreamWriter(cryptoStream);

                // Schreiben von Text wird automatisch verschlüsselt
                streamWriter.WriteLine("Verschlüsseln von Daten ist heutzutage einfach ein MUSS!");
                streamWriter.WriteLine("Testitest - ");
                streamWriter.Write("Geschrieben am " + DateTime.Now.ToLongTimeString());

                streamWriter.Close();
                cryptoStream.Close();
                fileStream.Close();

                Console.WriteLine("Verschlüsselung erfolgreich!\n\n");

            } catch (Exception ex) {
                Console.WriteLine("Fehler: " + ex.Message);
            }
        }

        public void AesDecrypt(string datei, byte[] key) {
            try {
                if (File.Exists(datei)) {
                    FileStream fileStream = new FileStream(datei, FileMode.Open);

                    Aes aes = Aes.Create();

                    // Einlesen IV von Dateianfang
                    byte[] iv = new byte[aes.IV.Length];
                    fileStream.Read(iv, 0, iv.Length);

                    // Ausgabe IV
                    foreach (byte b in iv) {
                        Console.Write(b + " ");
                    }
                    Console.WriteLine(Convert.ToBase64String(iv));

                    Console.WriteLine("Blocksize: " + iv.Length);

                    // Erzeugen Cryptostream Objekt
                    CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read);

                    // Lesen der Datei
                    StreamReader streamReader = new StreamReader(cryptoStream);

                    Console.WriteLine("Entschlüsselt: " + streamReader.ReadToEnd() + Environment.NewLine);

                    streamReader.Close();
                    cryptoStream.Close();
                    fileStream.Close();
                }
            } catch (Exception ex) {
                Console.WriteLine("Fehler: " + ex.Message);
            }
        }

        public void AesDecryptTest(string datei, byte[] key) {
            try {
                if (File.Exists(datei)) {
                    FileStream fileStream = new FileStream(datei, FileMode.Open);

                    Aes aes = Aes.Create();
                    aes.Key = key;

                    // Einlesen IV von Dateianfang
                    byte[] iv = new byte[aes.IV.Length];
                    fileStream.Read(iv, 0, aes.IV.Length);
                    aes.IV = iv;

                    // Ausgabe IV
                    foreach (byte b in aes.IV) {
                        Console.Write(b + " ");
                    }
                    Console.WriteLine(Convert.ToBase64String(aes.IV));

                    Console.WriteLine("Blocksize: " + aes.IV.Length);

                    // Erzeugen Cryptostream Objekt
                    CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read);

                    // Lesen der Datei
                    StreamReader streamReader = new StreamReader(cryptoStream);

                    Console.WriteLine("Entschlüsselt: " + streamReader.ReadToEnd() + Environment.NewLine);

                    streamReader.Close();
                    cryptoStream.Close();
                    fileStream.Close();
                }
            } catch (Exception ex) {
                Console.WriteLine("Fehler: " + ex.Message);
            }
        }

        public byte[] ErzeugeSchluessel(string original, int laenge) {
            byte[] erzeugterSchluessel = new byte[laenge];
            byte[] temp = Encoding.UTF8.GetBytes(original);

            for (int i = 0; i < original.Length; i++) {
                erzeugterSchluessel[i] = (byte)temp[i];
            }

            return erzeugterSchluessel;
        }
    }
}
