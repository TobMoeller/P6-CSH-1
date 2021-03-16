using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P6_CSH_1 {
    public class Day5Stadt {
        public static List<Day5Stadt> Staedte { get; set; } = new List<Day5Stadt>();
        public string Name { get; set; }
        public int Einwohnerzahl { get; set; }
        public string Landkreis { get; set; }
        public string Bundesland { get; set; }

        public Day5Stadt(string name, int einwohnerzahl, string landkreis, string bundesland) {
            Name = name;
            Einwohnerzahl = einwohnerzahl;
            Landkreis = landkreis;
            Bundesland = bundesland;
            Staedte.Add(this);
        }
        public Day5Stadt() {

        }

        public static void Ausgabe(Day5Stadt stadt) {
            Console.WriteLine("Stadt: " + stadt.Name);
            Console.WriteLine("Einwohnerzahl: " + stadt.Einwohnerzahl);
            Console.WriteLine("Landkreis: " + stadt.Landkreis);
            Console.WriteLine("Bundesland: " + stadt.Bundesland);
            Console.WriteLine();
        }

        public static void AlleAusgeben() {
            foreach (Day5Stadt stadt in Staedte) {
                Ausgabe(stadt);
            }
        }
    }
}
