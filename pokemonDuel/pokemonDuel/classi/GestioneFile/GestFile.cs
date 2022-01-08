using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.GestioneFile
{
    class GestFile
    {
        public static List<string> Leggi(string file)
        {
            StreamReader sr = new StreamReader(file);
            List<string> righe=new List<string>();
            string line = "";
            while ((line = sr.ReadLine()) != null)
                righe.Add(line);
            sr.Close();
            return righe;
        }
        public static void SalvaInfoGiocatore(string testo)
        {
            StreamWriter sr = new StreamWriter("./file/InfoUtente.csv");
            sr.WriteLine(testo);
            sr.Flush();
            sr.Close();
        }
        public static List<string> LeggiInfoGiocatore()
        {
            List<string> ris = new List<string>();
            if (File.Exists("./file/InfoUtente.csv"))
            {
                StreamReader sr = new StreamReader("./file/InfoUtente.csv");
                string line = "";
                while ((line = sr.ReadLine()) != null)
                    ris.Add(line);
                sr.Close();
            }
            return ris;
        }
        public static void AppendPokemonComprato(int comprato) {
            StreamWriter sr = new StreamWriter("./file/InfoUtente.csv",true);
            sr.Write(";" + comprato);
            sr.Close();
        }
    }
}
