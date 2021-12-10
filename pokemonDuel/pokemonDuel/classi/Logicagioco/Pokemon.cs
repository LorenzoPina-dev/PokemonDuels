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
        public string Nome;
        public List<string> Mosse;
        public string urlTexture;
        public bool mio;
        public Nodo posizione;
        public Pokemon(string csv)
        {
            string[] campi = csv.Split(';');
            Nome = campi[0];
            Mosse = new List<string>();
            for (int i = 1; i < campi.Length; i++)
                Mosse.Add(campi[i]);
            urlTexture = Nome.ToUpper() + ".png";
            posizione = null;

        }
    }
}
