using pokemonDuel.classi.Logicagioco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pokemonDuel.classi.GestioneFile
{
    class StoreInfo
    {
        public Dictionary<string, Pokemon> Pokedex;
        public Dictionary<string, Mossa> Mosse;
        static StoreInfo instance=null;
        public static StoreInfo Instance()
        {
            if (instance == null)
                instance = new StoreInfo();
            return instance;
        }
        private StoreInfo()
        {
            Mosse= new Dictionary<string, Mossa>();
            List<string> Tmosse = LeggiFile.Leggi("./file/mosse.csv");
            foreach (string m in Tmosse)
            {
                Mossa mossa = new Mossa(m);
                Mosse.Add(mossa.nome,mossa);
            }
            Pokedex = new Dictionary<string, Pokemon>();
            List<string> pokemon = LeggiFile.Leggi("./file/pokemon.csv");
            foreach (string s in pokemon)
            {
                Pokemon p = new Pokemon(s);
                Pokedex.Add(p.Nome,p);
            }
        }
    }
}
