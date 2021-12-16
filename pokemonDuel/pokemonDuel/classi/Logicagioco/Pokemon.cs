using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace pokemonDuel.classi.Logicagioco
{
    class Pokemon
    {
        public int id;
        public string Nome;
        public int Salti;
        public List<string> Mosse;
        public string urlTexture;
        public Nodo posizione;
        public bool mio;
        public Pokemon()
        {
        }
        public Pokemon(string csv)
        {
            string[] campi = csv.Split(';');
            id = int.Parse(campi[0]);
            Nome = campi[1];
            Mosse = new List<string>();
            for (int i = 2; i < campi.Length; i++)
                Mosse.Add(campi[i]);
            urlTexture =id + ".png";
            posizione = null;

        }
    }
}
