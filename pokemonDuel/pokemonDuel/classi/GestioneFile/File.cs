using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.GestioneFile
{
    class File
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
    }
}
