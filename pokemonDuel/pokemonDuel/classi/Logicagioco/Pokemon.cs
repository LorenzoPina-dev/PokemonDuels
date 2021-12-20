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
        public List<int> Mosse;
        public string urlTexture;
        public Nodo posizione;
        public bool mio;
        private static int idArrivato=0;
        public Pokemon()
        {
        }
        public Pokemon(string csv)
        {
            string[] campi = csv.Split(';');
            id = int.Parse(campi[0]);
            Nome = campi[1];
            Salti = int.Parse(campi[2]);
            Mosse = new List<int>();
            for (int i = 0; i < int.Parse(campi[3]); i++)
                Mosse.Add(idArrivato++);
            urlTexture =id + ".png";
            posizione = null;

        }
    }
}
